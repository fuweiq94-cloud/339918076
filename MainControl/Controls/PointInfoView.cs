using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 点位信息显示控件。
    /// 显示平台软件返回的预设点位列表及当前选中点位信息。
    /// </summary>
    [ToolboxBitmap(typeof(ZBarView), "Resources.ZBarView.bmp")]
    [DefaultProperty("Points")]
    [Description("点位信息显示控件：显示平台软件返回的预设点位列表及详细信息")]
    public class PointInfoView : Control
    {
        // —— 点位列表 ——
        [Category("Data")]
        [Browsable(false)]
        public List<PresetPoint> Points { get; private set; }

        // —— 当前选中的点位索引 ——
        [Category("Behavior")]
        [DefaultValue(-1)]
        [Description("当前选中的点位索引（-1 = 无选中）")]
        public int SelectedIndex { get; private set; }

        // —— 配置属性 ——
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("是否启用编辑模式（允许修改点位名称）")]
        public bool Editable { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "245, 247, 250")]
        public new Color BackColor { get; set; }

        // —— 内部数据绑定 ——
        private DataGridView dataGridView;
        private TextBox txtPointName;
        private Label lblCurrentPoint;
        private Button btnSelect;
        private readonly Color[] rowColors = new Color[]
        {
            Color.FromArgb(30, 200, 160),   // X - Greenish
            Color.FromArgb(30, 160, 200),   // Y - Blueish  
            Color.FromArgb(30, 200, 160),   // Z - Greenish
            Color.FromArgb(30, 160, 200)    // U - Blueish (if exists)
        };

        public PointInfoView()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(245, 247, 250);
            SetStyle(ControlStyles.AllPaintingInNonErased | 
                     ControlStyles.OptimizedDoubleBuffer | 
                     ControlStyles.ResizeRedraw, true);
            
            Size = new Size(320, 280);
            Editable = true;
            SelectedIndex = -1;
            Points = new List<PresetPoint>();

            // 初始化 UI 组件（设计时不触发，通过代码方式初始化）
            InitializeUI();
        }

        /// <summary>
        /// 初始化 UI 组件（用于设计器兼容）。
        /// </summary>
        private void InitializeUI()
        {
            if (DesignMode) return;

            // 禁用 AutoScroll，因为我们手动控制布局
            AutoScroll = false;

            // 标题标签
            Label lblTitle = new Label();
            lblTitle.Text = "当前点位";
            lblTitle.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(80, 90, 100);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Size = new Size(100, 25);
            Controls.Add(lblTitle);

            // 当前点位信息标签
            lblCurrentPoint = new Label();
            lblCurrentPoint.Text = "无选中点位";
            lblCurrentPoint.Font = new Font("Consolas", 9F);
            lblCurrentPoint.ForeColor = Color.Navy;
            lblCurrentPoint.Location = new Point(10, 35);
            lblCurrentPoint.Size = new Size(300, 40);
            lblCurrentPoint.BorderStyle = BorderStyle.FixedSingle;
            lblCurrentPoint.AutoSize = false;
            Controls.Add(lblCurrentPoint);

            // 表格视图
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(10, 85);
            dataGridView.Size = new Size(300, 150);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.MultiSelect = false;
            dataGridView.SelectionChanged += DataGrid_SelectionChanged;

            // 设置列
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "点位名", Width = 80 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColX", HeaderText = "X", Width = 60 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColY", HeaderText = "Y", Width = 60 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColZ", HeaderText = "Z", Width = 60 });

            dataGridView.DefaultCellStyle.BackColor = Color.White;
            dataGridView.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView.GridColor = Color.LightGray;
            dataGridView.RowTemplate.Height = 35;

            Controls.Add(dataGridView);

            // 选择按钮
            btnSelect = new Button();
            btnSelect.Text = "→ 跳转到此点位";
            btnSelect.Location = new Point(10, 240);
            btnSelect.Size = new Size(120, 30);
            btnSelect.BackColor = Color.FromArgb(200, 230, 255);
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Cursor = Cursors.Hand;
            btnSelect.Click += BtnSelect_Click;
            Controls.Add(btnSelect);

            // 刷新点位按钮
            Button btnRefresh = new Button();
            btnRefresh.Text = "刷新点位列表";
            btnRefresh.Location = new Point(140, 240);
            btnRefresh.Size = new Size(120, 30);
            btnRefresh.BackColor = Color.FromArgb(220, 250, 220);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += new EventHandler(BtnRefresh_Click);
            Controls.Add(btnRefresh);
        }

        /// <summary>
        /// 从平台获取并刷新点位列表。
        /// 注意：具体实现需根据平台 API 调整
        /// </summary>
        public void RefreshPointsList()
        {
            // 模拟从平台获取点位数据
            // 实际使用时应调用平台的真实 API，如：
            // var points = PlatformAPI.GetPresets();
            
            // 这里我们直接重新加载已有的 Points 列表
            ReloadPointsFromMemory();
        }

        /// <summary>
        /// 更新点位列表（从内存或外部源）。
        /// </summary>
        public void UpdatePoints(List<PresetPoint> newPoints)
        {
            Points.Clear();
            Points.AddRange(newPoints);
            ReloadPointsFromMemory();
        }

        /// <summary>
        /// 重新加载点到 DataGridView 中。
        /// </summary>
        private void ReloadPointsFromMemory()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ReloadPointsFromMemory));
                return;
            }

            dataGridView.Rows.Clear();

            foreach (var point in Points)
            {
                dataGridView.Rows.Add(
                    point.Name ?? "无名",
                    point.X.ToString("F2"),
                    point.Y.ToString("F2"),
                    point.Z.ToString("F2")
                );
            }

            // 恢复选中状态
            if (SelectedIndex >= 0 && SelectedIndex < Points.Count)
            {
                dataGridView.Rows[SelectedIndex].Selected = true;
                UpdateCurrentPointDisplay();
            }

            Invalidate();
        }

        /// <summary>
        /// 当 DataGridView 选择改变时触发。
        /// </summary>
        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int index = dataGridView.SelectedRows[0].Index;
                SelectPoint(index);
            }
        }

        /// <summary>
        /// 切换到指定索引的点位的。
        /// </summary>
        public void SelectPoint(int index)
        {
            if (index < 0 || index >= Points.Count)
            {
                SelectedIndex = -1;
                UpdateCurrentPointDisplay();
                return;
            }

            SelectedIndex = index;
            dataGridView.Rows[index].Selected = true;
            UpdateCurrentPointDisplay();
        }

        /// <summary>
        /// 更新当前点位信息显示。
        /// </summary>
        private void UpdateCurrentPointDisplay()
        {
            if (SelectedIndex >= 0 && SelectedIndex < Points.Count)
            {
                var pt = Points[SelectedIndex];
                lblCurrentPoint.Text = string.Format(
                    "{0} → X:{1:F2}  Y:{2:F2}  Z:{3:F2}",
                    pt.Name ?? "无名",
                    pt.X, pt.Y, pt.Z);
                lblCurrentPoint.ForeColor = Color.Navy;
            }
            else
            {
                lblCurrentPoint.Text = "无选中点位";
                lblCurrentPoint.ForeColor = Color.Gray;
            }
        }

        /// <summary>
        /// 选择按钮点击事件。
        /// </summary>
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (SelectedIndex >= 0 && SelectedIndex < Points.Count)
            {
                OnJumpToPointRequested?.Invoke(this, SelectedIndex);
            }
        }

        /// <summary>
        /// 刷新点位列表按钮点击事件。
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshPointsList();
        }

        /// <summary>
        /// 请求跳转到选定点位的事件。
        /// 由父窗体订阅此事件。
        /// </summary>
        public event EventHandler<int> JumpToPointRequested;

        /// <summary>
        /// 外部触发跳转到指定点位的事件。
        /// </summary>
        protected virtual void OnJumpToPointRequested(int index)
        {
            JumpToPointRequested?.Invoke(this, index);
        }

        /// <summary>
        /// 设置点位名称（可编辑）。
        /// </summary>
        public void SetPointName(int index, string name)
        {
            if (index >= 0 && index < Points.Count)
            {
                Points[index].Name = name;
                dataGridView.Rows[index].Cells["ColName"].Value = name ?? "无名";
                if (index == SelectedIndex)
                    UpdateCurrentPointDisplay();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);

            // 绘制背景条带（每个轴不同颜色）
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(240, 242, 245)))
            {
                Rectangle rect = ClientRectangle;
                g.FillRectangle(bgBrush, rect);
            }

            // 绘制边框
            using (Pen borderPen = new Pen(Color.FromArgb(150, 160, 175), 1))
            {
                Rectangle borderRect = ClientRectangle;
                g.DrawRectangle(borderPen, borderRect);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (dataGridView != null)
            {
                // 自动调整表格大小以适应容器
                int availableHeight = ClientRectangle.Height - 130; // 减去固定高度（标题 + 当前信息+按钮）
                if (availableHeight > 50)
                    dataGridView.Height = availableHeight;
            }
        }
    }
}
