namespace ProcessModules.Trajectory
{
    /// <summary>
    /// 运动轨迹查看窗体的设计器文件。
    /// 兼容 VS2017 WinForms 设计器：纯声明式逐行属性设置，无 Lambda / 集合初始化器。
    /// </summary>
    partial class RunForm
    {
        private System.ComponentModel.IContainer components = null;

        // —— 布局容器 ——
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlRight;

        // —— XY 视图（轨迹显示）——
        private ProcessModules.XYView xyView;

        // —— Z 轴视图 ——
        private ProcessModules.ZBarView zBar;

        // —— 实时坐标 DRO ——
        private System.Windows.Forms.GroupBox grpDro;
        private System.Windows.Forms.TableLayoutPanel tlpDro;
        private ProcessModules.DroLabel droX;
        private ProcessModules.DroLabel droY;
        private ProcessModules.DroLabel droZ;

        // —— 轨迹控制 ——
        private System.Windows.Forms.GroupBox grpTrail;
        private System.Windows.Forms.TableLayoutPanel tlpTrail;
        private System.Windows.Forms.CheckBox cbShowTrail;
        private System.Windows.Forms.Button btnClearTrail;
        private System.Windows.Forms.Label lblTrailInfo;

        // —— 快速移动 ——
        private System.Windows.Forms.GroupBox grpMove;
        private System.Windows.Forms.TableLayoutPanel tlpMove;
        private System.Windows.Forms.Button btnOrigin;
        private System.Windows.Forms.Button btnCenter;
        private System.Windows.Forms.Button btnRandom;

        // —— 速度控制 ——
        private System.Windows.Forms.GroupBox grpSpeed;
        private System.Windows.Forms.TableLayoutPanel tlpSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trbSpeed;

        // —— 状态栏 ——
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHint;

        // —— 动画定时器 ——
        private System.Windows.Forms.Timer animTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.xyView = new ProcessModules.XYView();
            this.zBar = new ProcessModules.ZBarView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpDro = new System.Windows.Forms.GroupBox();
            this.tlpDro = new System.Windows.Forms.TableLayoutPanel();
            this.droX = new ProcessModules.DroLabel();
            this.droY = new ProcessModules.DroLabel();
            this.droZ = new ProcessModules.DroLabel();
            this.grpTrail = new System.Windows.Forms.GroupBox();
            this.tlpTrail = new System.Windows.Forms.TableLayoutPanel();
            this.cbShowTrail = new System.Windows.Forms.CheckBox();
            this.btnClearTrail = new System.Windows.Forms.Button();
            this.lblTrailInfo = new System.Windows.Forms.Label();
            this.grpMove = new System.Windows.Forms.GroupBox();
            this.tlpMove = new System.Windows.Forms.TableLayoutPanel();
            this.btnOrigin = new System.Windows.Forms.Button();
            this.btnCenter = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            this.grpSpeed = new System.Windows.Forms.GroupBox();
            this.tlpSpeed = new System.Windows.Forms.TableLayoutPanel();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpDro.SuspendLayout();
            this.tlpDro.SuspendLayout();
            this.grpTrail.SuspendLayout();
            this.tlpTrail.SuspendLayout();
            this.grpMove.SuspendLayout();
            this.tlpMove.SuspendLayout();
            this.grpSpeed.SuspendLayout();
            this.tlpSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.xyView);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(12);
            this.splitMain.Panel1MinSize = 200;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.zBar);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.splitMain.Panel2MinSize = 70;
            this.splitMain.Size = new System.Drawing.Size(1084, 724);
            this.splitMain.SplitterDistance = 1000;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 0;
            // 
            // xyView
            // 
            this.xyView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.xyView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xyView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.xyView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.xyView.Location = new System.Drawing.Point(12, 12);
            this.xyView.Margin = new System.Windows.Forms.Padding(6);
            this.xyView.Name = "xyView";
            this.xyView.RangeMax = 100F;
            this.xyView.RangeMin = -100F;
            this.xyView.Size = new System.Drawing.Size(984, 708);
            this.xyView.TabIndex = 0;
            this.xyView.TargetX = 0F;
            this.xyView.TargetY = 0F;
            // 
            // zBar
            // 
            this.zBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.zBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zBar.Location = new System.Drawing.Point(4, 8);
            this.zBar.Margin = new System.Windows.Forms.Padding(4);
            this.zBar.Name = "zBar";
            this.zBar.RangeMax = 100F;
            this.zBar.RangeMin = -50F;
            this.zBar.Size = new System.Drawing.Size(72, 708);
            this.zBar.TabIndex = 0;
            this.zBar.TargetZ = 0F;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.grpSpeed);
            this.pnlRight.Controls.Add(this.grpMove);
            this.pnlRight.Controls.Add(this.grpTrail);
            this.pnlRight.Controls.Add(this.grpDro);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1084, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(8);
            this.pnlRight.Size = new System.Drawing.Size(340, 724);
            this.pnlRight.TabIndex = 1;
            // 
            // grpDro
            // 
            this.grpDro.Controls.Add(this.tlpDro);
            this.grpDro.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDro.Location = new System.Drawing.Point(8, 8);
            this.grpDro.Name = "grpDro";
            this.grpDro.Padding = new System.Windows.Forms.Padding(4);
            this.grpDro.Size = new System.Drawing.Size(324, 160);
            this.grpDro.TabIndex = 0;
            this.grpDro.TabStop = false;
            this.grpDro.Text = "实时坐标";
            // 
            // tlpDro
            // 
            this.tlpDro.ColumnCount = 1;
            this.tlpDro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDro.Controls.Add(this.droX, 0, 0);
            this.tlpDro.Controls.Add(this.droY, 0, 1);
            this.tlpDro.Controls.Add(this.droZ, 0, 2);
            this.tlpDro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDro.Location = new System.Drawing.Point(4, 28);
            this.tlpDro.Name = "tlpDro";
            this.tlpDro.RowCount = 3;
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpDro.Size = new System.Drawing.Size(316, 128);
            this.tlpDro.TabIndex = 0;
            // 
            // droX
            // 
            this.droX.AxisName = "X";
            this.droX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droX.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droX.Location = new System.Drawing.Point(3, 3);
            this.droX.Margin = new System.Windows.Forms.Padding(3);
            this.droX.Name = "droX";
            this.droX.Size = new System.Drawing.Size(310, 36);
            this.droX.TabIndex = 0;
            this.droX.Unit = "mm";
            this.droX.Value = 0F;
            // 
            // droY
            // 
            this.droY.AxisName = "Y";
            this.droY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droY.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droY.Location = new System.Drawing.Point(3, 45);
            this.droY.Margin = new System.Windows.Forms.Padding(3);
            this.droY.Name = "droY";
            this.droY.Size = new System.Drawing.Size(310, 36);
            this.droY.TabIndex = 1;
            this.droY.Unit = "mm";
            this.droY.Value = 0F;
            // 
            // droZ
            // 
            this.droZ.AxisName = "Z";
            this.droZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droZ.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droZ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droZ.Location = new System.Drawing.Point(3, 87);
            this.droZ.Margin = new System.Windows.Forms.Padding(3);
            this.droZ.Name = "droZ";
            this.droZ.Size = new System.Drawing.Size(310, 38);
            this.droZ.TabIndex = 2;
            this.droZ.Unit = "mm";
            this.droZ.Value = 0F;
            // 
            // grpTrail
            // 
            this.grpTrail.Controls.Add(this.tlpTrail);
            this.grpTrail.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTrail.Location = new System.Drawing.Point(8, 168);
            this.grpTrail.Name = "grpTrail";
            this.grpTrail.Padding = new System.Windows.Forms.Padding(4);
            this.grpTrail.Size = new System.Drawing.Size(324, 130);
            this.grpTrail.TabIndex = 1;
            this.grpTrail.TabStop = false;
            this.grpTrail.Text = "轨迹控制";
            // 
            // tlpTrail
            // 
            this.tlpTrail.ColumnCount = 2;
            this.tlpTrail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTrail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTrail.Controls.Add(this.cbShowTrail, 0, 0);
            this.tlpTrail.Controls.Add(this.btnClearTrail, 1, 0);
            this.tlpTrail.Controls.Add(this.lblTrailInfo, 0, 1);
            this.tlpTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTrail.Location = new System.Drawing.Point(4, 28);
            this.tlpTrail.Name = "tlpTrail";
            this.tlpTrail.RowCount = 2;
            this.tlpTrail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpTrail.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTrail.Size = new System.Drawing.Size(316, 98);
            this.tlpTrail.TabIndex = 0;
            // 
            // cbShowTrail
            // 
            this.cbShowTrail.Checked = true;
            this.cbShowTrail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbShowTrail.Location = new System.Drawing.Point(3, 3);
            this.cbShowTrail.Name = "cbShowTrail";
            this.cbShowTrail.Size = new System.Drawing.Size(152, 34);
            this.cbShowTrail.TabIndex = 0;
            this.cbShowTrail.Text = "显示轨迹";
            this.cbShowTrail.UseVisualStyleBackColor = true;
            // 
            // btnClearTrail
            // 
            this.btnClearTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearTrail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearTrail.Location = new System.Drawing.Point(161, 3);
            this.btnClearTrail.Name = "btnClearTrail";
            this.btnClearTrail.Size = new System.Drawing.Size(152, 34);
            this.btnClearTrail.TabIndex = 1;
            this.btnClearTrail.Text = "清除轨迹";
            this.btnClearTrail.UseVisualStyleBackColor = true;
            // 
            // lblTrailInfo
            // 
            this.tlpTrail.SetColumnSpan(this.lblTrailInfo, 2);
            this.lblTrailInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTrailInfo.Location = new System.Drawing.Point(3, 40);
            this.lblTrailInfo.Name = "lblTrailInfo";
            this.lblTrailInfo.Size = new System.Drawing.Size(310, 58);
            this.lblTrailInfo.TabIndex = 2;
            this.lblTrailInfo.Text = "轨迹点数：0";
            this.lblTrailInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpMove
            // 
            this.grpMove.Controls.Add(this.tlpMove);
            this.grpMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMove.Location = new System.Drawing.Point(8, 298);
            this.grpMove.Name = "grpMove";
            this.grpMove.Padding = new System.Windows.Forms.Padding(4);
            this.grpMove.Size = new System.Drawing.Size(324, 80);
            this.grpMove.TabIndex = 2;
            this.grpMove.TabStop = false;
            this.grpMove.Text = "快速移动";
            // 
            // tlpMove
            // 
            this.tlpMove.ColumnCount = 3;
            this.tlpMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpMove.Controls.Add(this.btnOrigin, 0, 0);
            this.tlpMove.Controls.Add(this.btnCenter, 1, 0);
            this.tlpMove.Controls.Add(this.btnRandom, 2, 0);
            this.tlpMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMove.Location = new System.Drawing.Point(4, 28);
            this.tlpMove.Name = "tlpMove";
            this.tlpMove.RowCount = 1;
            this.tlpMove.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMove.Size = new System.Drawing.Size(316, 48);
            this.tlpMove.TabIndex = 0;
            // 
            // btnOrigin
            // 
            this.btnOrigin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOrigin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOrigin.Location = new System.Drawing.Point(3, 3);
            this.btnOrigin.Name = "btnOrigin";
            this.btnOrigin.Size = new System.Drawing.Size(99, 42);
            this.btnOrigin.TabIndex = 0;
            this.btnOrigin.Text = "回原点";
            this.btnOrigin.UseVisualStyleBackColor = true;
            // 
            // btnCenter
            // 
            this.btnCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCenter.Location = new System.Drawing.Point(108, 3);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Size = new System.Drawing.Size(99, 42);
            this.btnCenter.TabIndex = 1;
            this.btnCenter.Text = "居中";
            this.btnCenter.UseVisualStyleBackColor = true;
            // 
            // btnRandom
            // 
            this.btnRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRandom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRandom.Location = new System.Drawing.Point(213, 3);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(100, 42);
            this.btnRandom.TabIndex = 2;
            this.btnRandom.Text = "随机";
            this.btnRandom.UseVisualStyleBackColor = true;
            // 
            // grpSpeed
            // 
            this.grpSpeed.Controls.Add(this.tlpSpeed);
            this.grpSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSpeed.Location = new System.Drawing.Point(8, 378);
            this.grpSpeed.Name = "grpSpeed";
            this.grpSpeed.Padding = new System.Windows.Forms.Padding(4);
            this.grpSpeed.Size = new System.Drawing.Size(324, 80);
            this.grpSpeed.TabIndex = 3;
            this.grpSpeed.TabStop = false;
            this.grpSpeed.Text = "运动速度";
            // 
            // tlpSpeed
            // 
            this.tlpSpeed.ColumnCount = 2;
            this.tlpSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSpeed.Controls.Add(this.lblSpeed, 0, 0);
            this.tlpSpeed.Controls.Add(this.trbSpeed, 1, 0);
            this.tlpSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSpeed.Location = new System.Drawing.Point(4, 28);
            this.tlpSpeed.Name = "tlpSpeed";
            this.tlpSpeed.RowCount = 1;
            this.tlpSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSpeed.Size = new System.Drawing.Size(316, 48);
            this.tlpSpeed.TabIndex = 0;
            // 
            // lblSpeed
            // 
            this.lblSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeed.Location = new System.Drawing.Point(3, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(64, 48);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "速度：中";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trbSpeed
            // 
            this.trbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbSpeed.Location = new System.Drawing.Point(73, 3);
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(240, 42);
            this.trbSpeed.TabIndex = 1;
            this.trbSpeed.TickFrequency = 10;
            this.trbSpeed.Value = 20;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblHint});
            this.statusStrip.Location = new System.Drawing.Point(0, 724);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1424, 29);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(700, 24);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "就绪";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHint
            // 
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(709, 24);
            this.lblHint.Text = "左键拖动设定目标 · 右键回原点 · 观察运动轨迹";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // TrajectoryViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1424, 753);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "RunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运动轨迹查看  ·  XYZ 工控机";
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.grpDro.ResumeLayout(false);
            this.tlpDro.ResumeLayout(false);
            this.grpTrail.ResumeLayout(false);
            this.tlpTrail.ResumeLayout(false);
            this.grpMove.ResumeLayout(false);
            this.tlpMove.ResumeLayout(false);
            this.grpSpeed.ResumeLayout(false);
            this.tlpSpeed.ResumeLayout(false);
            this.tlpSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
