using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ProcessModules;

namespace MainControlProcessModule
{
    /// <summary>
    /// U 轴旋转控制面板（圆形角度控制专用界面）。
    /// </summary>
    public partial class URotationForm : Form
    {
        private readonly MainControlProcessModule _module;
        private readonly XyzControllerHub _hub;

        // —— UI 引用 ——
        private Panel pnlCircle;          // 圆形表盘
        private Label lblAngleValue;      // 当前角度数值
        private NumericUpDown nud_target;  // 目标角度输入
        private Button btnSetTarget;      // 设置目标按钮
        private Button btnHome;           // 回零按钮
        private Label lbl_status;         // 状态显示

        // —— JOG 服务 ——
        private AxisJogService _jogService;
        
        // —— UI 按钮 ——
        private JogButton jogButtonPlus;
        private JogButton jogButtonMinus;

        // —— 动画定时器 ——
        private Timer animTimer = new Timer();

        public URotationForm(MainControlProcessModule module)
        {
            _module = module;
            _hub = _module.Hub;
            
            InitializeComponent();
            
            // 绑定事件
            HookEvents();
            
            // 初始化 JOG 服务
            _jogService = new AxisJogService(_hub.U);
            
            // 初始刷新一次
            SyncUiFromHub();
        }

        /// <summary>事件绑定</summary>
        private void HookEvents()
        {
            this.KeyDown += RunForm_KeyDown;
            animTimer.Tick += AnimTimer_Tick;
        }

        /// <summary>同步 Hub 状态到 UI</summary>
        private void SyncUiFromHub()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(SyncUiFromHub));
                return;
            }

            // 更新 DRO 角度值（转换为度）
            float angleDeg = (float)(_hub.U.Current * 180.0 / Math.PI);
            lblAngleValue.Text = string.Format("{0:F2}°", angleDeg);

            // 限制角度范围在 [UMin, UMax]
            try
            {
                decimal degMin = (decimal)((double)_hub.U.Min * 180.0 / Math.PI);
                decimal degMax = (decimal)((double)_hub.U.Max * 180.0 / Math.PI);
                nud_target.Minimum = degMin;
                nud_target.Maximum = degMax;
            }
            catch { }

            // 更新目标框显示
            decimal targetDeg = (decimal)(_hub.U.Target * 180.0 / Math.PI);
            nud_target.Value = targetDeg;
        }

        /// <summary>动画定时器 - 周期性刷新 UI</summary>
        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            SyncUiFromHub();
            Refresh(); // 重绘圆形表盘
        }

        /// <summary>窗体加载时订阅 Hub 变化</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _hub.Changed += Hub_Changed;
            nud_target.Enter += new EventHandler(NudTarget_Enter);
            nud_target.Leave += new EventHandler(NudTarget_Leave);
        }

        /// <summary>窗体关闭时退订 Hub 变化</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _hub.Changed -= Hub_Changed;
        }

        private void Hub_Changed(object sender, EventArgs e)
        {
            SyncUiFromHub();
        }

        /// <summary>当前角度数值</summary>
        private string current_angle_value
        {
            get { return lblAngleValue.Text; }
            set { lblAngleValue.Text = value; }
        }

        /// <summary>目标角度输入框</summary>
        private decimal target_input
        {
            get { return nud_target.Value; }
            set { nud_target.Value = value; }
        }

        /// <summary>键盘快捷键</summary>
        private void RunForm_KeyDown(object sender, KeyEventArgs e)
        {
            int big = e.Shift ? 10 : 1;
            bool handled = true;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    _hub.U.Step(-big); break;
                case Keys.Right:
                    _hub.U.Step(+big); break;
                case Keys.Escape:
                    this.Close(); break;
                default:
                    handled = false; break;
            }
            if (handled) e.Handled = true;
        }

        /// <summary>设置目标角度</summary>
        private void SetTargetAngle()
        {
            try
            {
                // 从角度转换为弧度
                decimal deg = nud_target.Value;
                double rad = (double)deg * Math.PI / 180.0;
                
                // 检查是否在范围内
                if (rad >= _hub.U.Min && rad <= _hub.U.Max)
                {
                    _hub.U.SetTarget((float)rad);
                    lbl_status.Text = string.Format("已设置目标角度：{0:F2}° → 当前角度：{1:F2}°", 
                        (double)deg, _hub.U.Current * 180.0 / Math.PI);
                }
                else
                {
                    MessageBox.Show(string.Format("目标角度超出范围！\n范围：{0:F2}° ~ {1:F2}°",
                        (double)_hub.U.Min * 180.0 / Math.PI, (double)_hub.U.Max * 180.0 / Math.PI),
                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置目标角度失败：" + ex.Message, "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>回原点（0 弧度 = 0 度）</summary>
        private void BtnHome_Click(object sender, EventArgs e)
        {
            _hub.ResetToOrigin();
            lbl_status.Text = "已回原点 (0°)";
        }

        /// <summary>清空设计器代码（VS 设计器不需要此方法）</summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 
            // URotationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "U 轴旋转控制";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
            // 
            // pnl_circle
            // 
            pnlCircle = new Panel();
            pnlCircle.Location = new Point(30, 30);
            pnlCircle.Size = new Size(300, 300);
            pnlCircle.BackColor = Color.White;
            pnlCircle.BorderStyle = BorderStyle.FixedSingle;
            pnlCircle.Name = "pnl_circle";
            
            // 
            // lbl_angle_value
            // 
            lblAngleValue = new Label();
            lblAngleValue.Font = new Font("微软雅黑", 24F, FontStyle.Bold);
            lblAngleValue.ForeColor = Color.Blue;
            lblAngleValue.AutoSize = true;
            lblAngleValue.Location = new Point(30, 240);
            lblAngleValue.Text = "0.00°";
            lblAngleValue.Name = "lbl_angle_value";
            
            // 
            // grp_target
            // 
            GroupBox grp_target = new GroupBox();
            grp_target.Text = "目标角度设定";
            grp_target.Location = new Point(30, 340);
            grp_target.Size = new Size(300, 120);
            
            // 
            // lbl_target
            // 
            Label lbl_target = new Label();
            lbl_target.Location = new Point(20, 20);
            lbl_target.Text = "当前角度：";
            lbl_target.Size = new Size(80, 23);
            
            // 
            // nud_target
            // 
            nud_target = new NumericUpDown();
            nud_target.Location = new Point(100, 18);
            nud_target.Size = new Size(120, 23);
            nud_target.DecimalPlaces = 2;
            nud_target.Increment = new decimal(1);
            nud_target.KeyDown += new KeyEventHandler(NudTarget_KeyDown);
            
            // 
            // btn_set_target
            // 
            btnSetTarget = new Button();
            btnSetTarget.Location = new Point(100, 50);
            btnSetTarget.Size = new Size(120, 30);
            btnSetTarget.Text = "设置目标";
            btnSetTarget.Click += new EventHandler(BtnSetTarget_Click);
            
            // 
            // btn_home
            // 
            btnHome = new Button();
            btnHome.Location = new Point(100, 90);
            btnHome.Size = new Size(120, 30);
            btnHome.Text = "回零点 (0°)";
            btnHome.BackColor = Color.LightGreen;
            btnHome.Click += BtnHome_Click;
            
            // 
            // lbl_status
            // 
            lbl_status = new Label();
            lbl_status.Location = new Point(20, 180);
            lbl_status.Text = "U 轴旋转控制";
            lbl_status.ForeColor = Color.Gray;
            lbl_status.Font = new Font("微软雅黑", 9F);
            
            // 
            // trb_jogstep
            // 
            TrackBar trb_jogstep = new TrackBar();
            Label lblSpeed = new Label();
            trb_jogstep.Location = new Point(250, 30);
            trb_jogstep.Size = new Size(200, 45);
            trb_jogstep.Orientation = Orientation.Vertical;
            trb_jogstep.Minimum = 1;
            trb_jogstep.Maximum = 10;
            trb_jogstep.Value = 3;
            lblSpeed.Text = "JOG\n步长\n调节";
            lblSpeed.Location = new Point(260, 80);
            lblSpeed.AutoSize = true;
            trb_jogstep.Scroll += new EventHandler(TrbJogstep_Scroll);
            
            this.Controls.Add(trb_jogstep);
            this.Controls.Add(lblSpeed);
            
            // 
            // Jog Buttons
            // 
            jogButtonPlus = new JogButton();
            jogButtonPlus.Location = new Point(250, 100);
            jogButtonPlus.Size = new Size(60, 60);
            jogButtonPlus.Tag = _jogService;
            jogButtonPlus.Jog += JogButton_Jog;
            jogButtonPlus.Stop += JogButton_Stop;
            
            jogButtonMinus = new JogButton();
            jogButtonMinus.Location = new Point(320, 100);
            jogButtonMinus.Size = new Size(60, 60);
            jogButtonMinus.Tag = _jogService;
            jogButtonMinus.Jog += JogButton_Jog;
            jogButtonMinus.Stop += JogButton_Stop;
            
            // 添加到控件集合
            this.Controls.Add(pnlCircle);
            this.Controls.Add(lblAngleValue);
            this.Controls.Add(grp_target);
            grp_target.Controls.Add(lbl_target);
            grp_target.Controls.Add(nud_target);
            grp_target.Controls.Add(btnSetTarget);
            grp_target.Controls.Add(btnHome);
            this.Controls.Add(lbl_status);
            this.Controls.Add(trb_jogstep);
            this.Controls.Add(jogButtonPlus);
            this.Controls.Add(jogButtonMinus);
            
            // 启动动画定时器
            animTimer.Interval = 100;
            
            this.ResumeLayout(false);
        }

        #region 命名事件处理方法

        private void NudTarget_Enter(object sender, EventArgs e)
        {
            lbl_status.Text = "输入目标角度后按 Enter 或点击设置按钮";
        }

        private void NudTarget_Leave(object sender, EventArgs e)
        {
            lbl_status.Text = "U 轴旋转控制";
        }

        private void NudTarget_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SetTargetAngle();
        }

        private void BtnSetTarget_Click(object sender, EventArgs e)
        {
            SetTargetAngle();
        }

        private void TrbJogstep_Scroll(object sender, EventArgs e)
        {
            TrackBar trb = (TrackBar)sender;
            _jogService.SetStepDistance((float)trb.Value / 10f);
        }

        #endregion

        #region JOG 控制事件处理

        private void JogButton_Jog(object sender, JogEventArgs e)
        {
            JogButton btn = (JogButton)sender;
            AxisJogService service = (AxisJogService)btn.Tag;
            service.OnJogStart(e.Direction);
        }

        private void JogButton_Stop(object sender, JogEventArgs e)
        {
            _jogService.OnJogStop();
        }

        #endregion

        #region 绘制圆形表盘

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            
            // 清除背景
            using (Brush bgBrush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(bgBrush, pnlCircle.ClientRectangle);
            }
            
            // 绘制圆周
            Rectangle circleRect = new Rectangle(10, 10, 280, 280);
            Pen outerPen = new Pen(Color.DarkGray, 2);
            Pen innerPen = new Pen(Color.Blue, 1);
            g.DrawEllipse(innerPen, circleRect);
            g.DrawEllipse(outerPen, circleRect.X - 2, circleRect.Y - 2, circleRect.Width + 4, circleRect.Height + 4);
            
            // 绘制刻度线
            int radius = 130;
            int center = 150;
            float currentRad = (float)_hub.U.Current;
            
            Brush textBrush = Brushes.Black;
            Font axisLabelFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            Font smallFont = new Font("微软雅黑", 8F);
            
            // 四个主要刻度（90 度间隔）
            for (int i = 0; i < 4; i++)
            {
                float angle = (float)(i * Math.PI / 2);
                int x1 = center + (int)(radius * Math.Cos(angle));
                int y1 = center - (int)(radius * Math.Sin(angle));
                int x2 = center + (int)((radius - 15) * Math.Cos(angle));
                int y2 = center - (int)((radius - 15) * Math.Sin(angle));
                
                g.DrawLine(outerPen, x1, y1, x2, y2);
                
                // 添加文字标注（0°, 90°, 180°, 270°）
                float labelRadius = radius + 25;
                int lx = center + (int)(labelRadius * Math.Cos(angle));
                int ly = center - (int)(labelRadius * Math.Sin(angle));
                string label = (i * 90).ToString() + "°";
                Size textSize = TextRenderer.MeasureText(label, axisLabelFont);
                g.DrawString(label, axisLabelFont, textBrush, lx - textSize.Width / 2, ly - textSize.Height / 2);
            }
            
            // 绘制指针
            PointF centerPoint = new PointF(center, center);
            float pointerLength = radius - 20;
            float currentRadFloat = (float)_hub.U.Current;
            float pointerX = center + (float)(pointerLength * Math.Cos(currentRadFloat));
            float pointerY = center - (float)(pointerLength * Math.Sin(currentRadFloat));
            
            // 声明绘图资源
            Pen pointerPen = null;
            Brush arrowBrush = null;
            
            try
            {
                pointerPen = new Pen(Color.Red, 3);
                arrowBrush = new SolidBrush(Color.Red);
                
                // 绘制指针线
                g.DrawLine(pointerPen, centerPoint.X, centerPoint.Y, pointerX, pointerY);
                
                // 绘制箭头三角形
                float arrowAngle = currentRadFloat + (float)(Math.PI / 2);
                float arrowLen = 15;
                PointF arrowHead1 = new PointF(
                    pointerX + (float)(arrowLen * Math.Cos(arrowAngle)),
                    pointerY - (float)(arrowLen * Math.Sin(arrowAngle)));
                float arrowAngle2 = currentRadFloat - (float)(Math.PI / 2);
                PointF arrowHead2 = new PointF(
                    pointerX + (float)(arrowLen * Math.Cos(arrowAngle2)),
                    pointerY - (float)(arrowLen * Math.Sin(arrowAngle2)));
                
                GraphicsPath arrowPath = new GraphicsPath();
                arrowPath.AddLine(centerPoint.X, centerPoint.Y, pointerX, pointerY);
                arrowPath.AddLine(pointerX, pointerY, arrowHead1.X, arrowHead1.Y);
                arrowPath.AddLine(arrowHead1.X, arrowHead1.Y, arrowHead2.X, arrowHead2.Y);
                arrowPath.AddBezier(new PointF(center, center), new PointF(pointerX, pointerY), arrowHead1, arrowHead2);
                
                g.FillPath(arrowBrush, arrowPath);
            }
            finally
            {
                if (pointerPen != null) pointerPen.Dispose();
                if (arrowBrush != null) arrowBrush.Dispose();
            }
            
            // 绘制范围标记（基于全局参数）
            float minRad = (float)globalSetting.UMin;
            float maxRad = (float)globalSetting.UMax;
            double minDeg = minRad * 180.0 / Math.PI;
            double maxDeg = maxRad * 180.0 / Math.PI;
            
            Brush rangeBrush = new SolidBrush(Color.FromArgb(100, Color.Green));
            
            // 计算最小和最大角度的坐标位置
            float minPointerX = center + (float)(pointerLength * Math.Cos(minRad));
            float minPointerY = center - (float)(pointerLength * Math.Sin(minRad));
            float maxPointerX = center + (float)(pointerLength * Math.Cos(maxRad));
            float maxPointerY = center - (float)(pointerLength * Math.Sin(maxRad));
            
            // 绘制范围区域扇形
            using (GraphicsPath arcPath = new GraphicsPath())
            {
                arcPath.AddLine(center, center, minPointerX, minPointerY);
                arcPath.AddArc(circleRect, (float)(minRad * 180 / Math.PI), (float)((maxRad - minRad) * 180 / Math.PI));
                arcPath.AddLine(center, center, maxPointerX, maxPointerY);
                arcPath.CloseFigure();
                g.FillPath(rangeBrush, arcPath);
            }
            
            // 清理资源
            g.Dispose();
            outerPen.Dispose();
            innerPen.Dispose();
            textBrush.Dispose();
            axisLabelFont.Dispose();
            smallFont.Dispose();
            rangeBrush.Dispose();
        }

        private MainControlGlobalSetting globalSetting
        {
            get { return _module.globalSetting; }
        }

        #endregion
    }
}
