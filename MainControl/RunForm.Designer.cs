namespace ProcessModules.MainControl
{
    /// <summary>
    /// 主控制工艺模组运行界面的设计器文件。
    /// 此文件采用 VS 设计器兼容的纯声明式写法：每个控件属性逐行显式设置，
    /// 不使用循环 / 辅助方法 / 集合初始化器之外的语法，以便在 VS2017+ WinForms 设计器中打开。
    /// </summary>
    partial class RunForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // —— 布局容器 ——
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlRight;

        // —— XY 视图（自定义控件，来自本类库 Controls）——
        private ProcessModules.XYView xyView;

        // —— Z 视图（自定义控件）——
        private ProcessModules.ZBarView zBar;

        // —— X 轴组 ——
        private System.Windows.Forms.GroupBox grpX;
        private System.Windows.Forms.TrackBar trbX;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.Button btnXMinus;
        private System.Windows.Forms.Button btnXPlus;

        // —— Y 轴组 ——
        private System.Windows.Forms.GroupBox grpY;
        private System.Windows.Forms.TrackBar trbY;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.Button btnYMinus;
        private System.Windows.Forms.Button btnYPlus;

        // —— Z 轴组 ——
        private System.Windows.Forms.GroupBox grpZ;
        private System.Windows.Forms.TrackBar trbZ;
        private System.Windows.Forms.NumericUpDown nudZ;
        private System.Windows.Forms.Button btnZMinus;
        private System.Windows.Forms.Button btnZPlus;

        // —— 通用组 ——
        private System.Windows.Forms.GroupBox grpCommon;
        private System.Windows.Forms.TableLayoutPanel tlpCommon;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trbSpeed;
        private System.Windows.Forms.CheckBox cbTrail;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnCenter;
        private System.Windows.Forms.Button btnClearTrail;
        private System.Windows.Forms.Button btnRandom;

        // —— JOG 控制组 ——
        private System.Windows.Forms.GroupBox grpJog;
        private System.Windows.Forms.TableLayoutPanel tlpJog;
        private System.Windows.Forms.RadioButton rbIncremental;
        private System.Windows.Forms.RadioButton rbContinuous;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.NumericUpDown nudJogStep;
        private System.Windows.Forms.Label lblAxisHdrX;
        private System.Windows.Forms.Label lblAxisHdrY;
        private System.Windows.Forms.Label lblAxisHdrZ;
        private ProcessModules.JogButton jogXMinus;
        private ProcessModules.JogButton jogXPlus;
        private ProcessModules.JogButton jogYMinus;
        private ProcessModules.JogButton jogYPlus;
        private ProcessModules.JogButton jogZMinus;
        private ProcessModules.JogButton jogZPlus;
        private System.Windows.Forms.Button btnEStop;

        // —— 状态栏 ——
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblHint;

        // —— 动画定时器 ——
        private System.Windows.Forms.Timer animTimer;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">true 表示释放托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// 设计器支持所必需的方法，不要修改。
        /// 此方法内的内容设计器会自动管理。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.xyView = new ProcessModules.XYView();
            this.zBar = new ProcessModules.ZBarView();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpCommon = new System.Windows.Forms.GroupBox();
            this.tlpCommon = new System.Windows.Forms.TableLayoutPanel();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnCenter = new System.Windows.Forms.Button();
            this.btnClearTrail = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            this.cbTrail = new System.Windows.Forms.CheckBox();
            this.grpJog = new System.Windows.Forms.GroupBox();
            this.tlpJog = new System.Windows.Forms.TableLayoutPanel();
            this.rbIncremental = new System.Windows.Forms.RadioButton();
            this.rbContinuous = new System.Windows.Forms.RadioButton();
            this.lblStep = new System.Windows.Forms.Label();
            this.nudJogStep = new System.Windows.Forms.NumericUpDown();
            this.btnEStop = new System.Windows.Forms.Button();
            this.lblAxisHdrX = new System.Windows.Forms.Label();
            this.jogXMinus = new ProcessModules.JogButton();
            this.jogXPlus = new ProcessModules.JogButton();
            this.lblAxisHdrY = new System.Windows.Forms.Label();
            this.jogYMinus = new ProcessModules.JogButton();
            this.jogYPlus = new ProcessModules.JogButton();
            this.lblAxisHdrZ = new System.Windows.Forms.Label();
            this.jogZMinus = new ProcessModules.JogButton();
            this.jogZPlus = new ProcessModules.JogButton();
            this.grpZ = new System.Windows.Forms.GroupBox();
            this.btnZMinus = new System.Windows.Forms.Button();
            this.trbZ = new System.Windows.Forms.TrackBar();
            this.btnZPlus = new System.Windows.Forms.Button();
            this.nudZ = new System.Windows.Forms.NumericUpDown();
            this.grpY = new System.Windows.Forms.GroupBox();
            this.btnYMinus = new System.Windows.Forms.Button();
            this.trbY = new System.Windows.Forms.TrackBar();
            this.btnYPlus = new System.Windows.Forms.Button();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.grpX = new System.Windows.Forms.GroupBox();
            this.btnXMinus = new System.Windows.Forms.Button();
            this.trbX = new System.Windows.Forms.TrackBar();
            this.btnXPlus = new System.Windows.Forms.Button();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHint = new System.Windows.Forms.ToolStripStatusLabel();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.grpCommon.SuspendLayout();
            this.tlpCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            this.grpJog.SuspendLayout();
            this.tlpJog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJogStep)).BeginInit();
            this.grpZ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZ)).BeginInit();
            this.grpY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            this.grpX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Margin = new System.Windows.Forms.Padding(4);
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
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(12, 18, 12, 18);
            this.splitMain.Panel2MinSize = 80;
            this.splitMain.Size = new System.Drawing.Size(1754, 1253);
            this.splitMain.SplitterDistance = 1660;
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
            this.xyView.Size = new System.Drawing.Size(1624, 1217);
            this.xyView.TabIndex = 0;
            this.xyView.Click += new System.EventHandler(this.xyView_Click);
            // 
            // zBar
            // 
            this.zBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.zBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zBar.Location = new System.Drawing.Point(12, 18);
            this.zBar.Margin = new System.Windows.Forms.Padding(6);
            this.zBar.Name = "zBar";
            this.zBar.Size = new System.Drawing.Size(64, 1217);
            this.zBar.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.grpCommon);
            this.pnlRight.Controls.Add(this.grpJog);
            this.pnlRight.Controls.Add(this.grpZ);
            this.pnlRight.Controls.Add(this.grpY);
            this.pnlRight.Controls.Add(this.grpX);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1754, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(6);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(18);
            this.pnlRight.Size = new System.Drawing.Size(810, 1253);
            this.pnlRight.TabIndex = 1;
            // 
            // grpCommon
            // 
            this.grpCommon.Controls.Add(this.tlpCommon);
            this.grpCommon.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCommon.Location = new System.Drawing.Point(18, 999);
            this.grpCommon.Margin = new System.Windows.Forms.Padding(6);
            this.grpCommon.Name = "grpCommon";
            this.grpCommon.Padding = new System.Windows.Forms.Padding(6);
            this.grpCommon.Size = new System.Drawing.Size(748, 338);
            this.grpCommon.TabIndex = 4;
            this.grpCommon.TabStop = false;
            this.grpCommon.Text = "通用";
            // 
            // tlpCommon
            // 
            this.tlpCommon.ColumnCount = 4;
            this.tlpCommon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCommon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCommon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCommon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCommon.Controls.Add(this.lblSpeed, 0, 0);
            this.tlpCommon.Controls.Add(this.trbSpeed, 1, 0);
            this.tlpCommon.Controls.Add(this.btnZero, 0, 1);
            this.tlpCommon.Controls.Add(this.btnCenter, 1, 1);
            this.tlpCommon.Controls.Add(this.btnClearTrail, 2, 1);
            this.tlpCommon.Controls.Add(this.btnRandom, 3, 1);
            this.tlpCommon.Controls.Add(this.cbTrail, 0, 2);
            this.tlpCommon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommon.Location = new System.Drawing.Point(6, 30);
            this.tlpCommon.Margin = new System.Windows.Forms.Padding(6);
            this.tlpCommon.Name = "tlpCommon";
            this.tlpCommon.RowCount = 3;
            this.tlpCommon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpCommon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpCommon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpCommon.Size = new System.Drawing.Size(736, 302);
            this.tlpCommon.TabIndex = 0;
            // 
            // lblSpeed
            // 
            this.lblSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeed.Location = new System.Drawing.Point(6, 0);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(172, 99);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "速度：中";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trbSpeed
            // 
            this.tlpCommon.SetColumnSpan(this.trbSpeed, 3);
            this.trbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbSpeed.Location = new System.Drawing.Point(190, 6);
            this.trbSpeed.Margin = new System.Windows.Forms.Padding(6);
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(540, 87);
            this.trbSpeed.TabIndex = 1;
            this.trbSpeed.TickFrequency = 10;
            this.trbSpeed.Value = 10;
            // 
            // btnZero
            // 
            this.btnZero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnZero.Location = new System.Drawing.Point(6, 105);
            this.btnZero.Margin = new System.Windows.Forms.Padding(6);
            this.btnZero.Name = "btnZero";
            this.btnZero.Padding = new System.Windows.Forms.Padding(4);
            this.btnZero.Size = new System.Drawing.Size(172, 90);
            this.btnZero.TabIndex = 2;
            this.btnZero.Text = "回原点(0,0,0)";
            this.btnZero.UseVisualStyleBackColor = true;
            // 
            // btnCenter
            // 
            this.btnCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCenter.Location = new System.Drawing.Point(190, 105);
            this.btnCenter.Margin = new System.Windows.Forms.Padding(6);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Padding = new System.Windows.Forms.Padding(4);
            this.btnCenter.Size = new System.Drawing.Size(172, 90);
            this.btnCenter.TabIndex = 3;
            this.btnCenter.Text = "居中";
            this.btnCenter.UseVisualStyleBackColor = true;
            // 
            // btnClearTrail
            // 
            this.btnClearTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearTrail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearTrail.Location = new System.Drawing.Point(374, 105);
            this.btnClearTrail.Margin = new System.Windows.Forms.Padding(6);
            this.btnClearTrail.Name = "btnClearTrail";
            this.btnClearTrail.Padding = new System.Windows.Forms.Padding(4);
            this.btnClearTrail.Size = new System.Drawing.Size(172, 90);
            this.btnClearTrail.TabIndex = 4;
            this.btnClearTrail.Text = "清除轨迹";
            this.btnClearTrail.UseVisualStyleBackColor = true;
            // 
            // btnRandom
            // 
            this.btnRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRandom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRandom.Location = new System.Drawing.Point(558, 105);
            this.btnRandom.Margin = new System.Windows.Forms.Padding(6);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Padding = new System.Windows.Forms.Padding(4);
            this.btnRandom.Size = new System.Drawing.Size(172, 90);
            this.btnRandom.TabIndex = 5;
            this.btnRandom.Text = "随机位置";
            this.btnRandom.UseVisualStyleBackColor = true;
            // 
            // cbTrail
            // 
            this.cbTrail.Checked = true;
            this.cbTrail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlpCommon.SetColumnSpan(this.cbTrail, 4);
            this.cbTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTrail.Location = new System.Drawing.Point(6, 207);
            this.cbTrail.Margin = new System.Windows.Forms.Padding(6);
            this.cbTrail.Name = "cbTrail";
            this.cbTrail.Size = new System.Drawing.Size(724, 89);
            this.cbTrail.TabIndex = 6;
            this.cbTrail.Text = "显示轨迹";
            this.cbTrail.UseVisualStyleBackColor = true;
            // 
            // grpJog
            // 
            this.grpJog.Controls.Add(this.tlpJog);
            this.grpJog.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpJog.Location = new System.Drawing.Point(18, 624);
            this.grpJog.Margin = new System.Windows.Forms.Padding(4);
            this.grpJog.Name = "grpJog";
            this.grpJog.Padding = new System.Windows.Forms.Padding(6);
            this.grpJog.Size = new System.Drawing.Size(748, 375);
            this.grpJog.TabIndex = 5;
            this.grpJog.TabStop = false;
            this.grpJog.Text = "JOG 控制（按住按钮移动）";
            // 
            // tlpJog
            // 
            this.tlpJog.ColumnCount = 4;
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tlpJog.Controls.Add(this.rbIncremental, 0, 0);
            this.tlpJog.Controls.Add(this.rbContinuous, 1, 0);
            this.tlpJog.Controls.Add(this.lblStep, 2, 0);
            this.tlpJog.Controls.Add(this.nudJogStep, 3, 0);
            this.tlpJog.Controls.Add(this.btnEStop, 0, 4);
            this.tlpJog.Controls.Add(this.lblAxisHdrX, 0, 1);
            this.tlpJog.Controls.Add(this.jogXMinus, 1, 1);
            this.tlpJog.Controls.Add(this.jogXPlus, 2, 1);
            this.tlpJog.Controls.Add(this.lblAxisHdrY, 0, 2);
            this.tlpJog.Controls.Add(this.jogYMinus, 1, 2);
            this.tlpJog.Controls.Add(this.jogYPlus, 2, 2);
            this.tlpJog.Controls.Add(this.lblAxisHdrZ, 0, 3);
            this.tlpJog.Controls.Add(this.jogZMinus, 1, 3);
            this.tlpJog.Controls.Add(this.jogZPlus, 2, 3);
            this.tlpJog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpJog.Location = new System.Drawing.Point(6, 30);
            this.tlpJog.Margin = new System.Windows.Forms.Padding(4);
            this.tlpJog.Name = "tlpJog";
            this.tlpJog.RowCount = 5;
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpJog.Size = new System.Drawing.Size(736, 339);
            this.tlpJog.TabIndex = 0;
            // 
            // rbIncremental
            // 
            this.rbIncremental.Checked = true;
            this.rbIncremental.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbIncremental.Location = new System.Drawing.Point(4, 4);
            this.rbIncremental.Margin = new System.Windows.Forms.Padding(4);
            this.rbIncremental.Name = "rbIncremental";
            this.rbIncremental.Size = new System.Drawing.Size(82, 40);
            this.rbIncremental.TabIndex = 0;
            this.rbIncremental.TabStop = true;
            this.rbIncremental.Text = "寸动";
            this.rbIncremental.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbIncremental.UseVisualStyleBackColor = true;
            // 
            // rbContinuous
            // 
            this.rbContinuous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbContinuous.Location = new System.Drawing.Point(94, 4);
            this.rbContinuous.Margin = new System.Windows.Forms.Padding(4);
            this.rbContinuous.Name = "rbContinuous";
            this.rbContinuous.Size = new System.Drawing.Size(247, 40);
            this.rbContinuous.TabIndex = 1;
            this.rbContinuous.Text = "连续（按住）";
            this.rbContinuous.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbContinuous.UseVisualStyleBackColor = true;
            // 
            // lblStep
            // 
            this.lblStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStep.Location = new System.Drawing.Point(349, 0);
            this.lblStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(247, 48);
            this.lblStep.TabIndex = 2;
            this.lblStep.Text = "步长：";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudJogStep
            // 
            this.nudJogStep.DecimalPlaces = 2;
            this.nudJogStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudJogStep.Location = new System.Drawing.Point(604, 4);
            this.nudJogStep.Margin = new System.Windows.Forms.Padding(4);
            this.nudJogStep.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudJogStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudJogStep.Name = "nudJogStep";
            this.nudJogStep.Size = new System.Drawing.Size(128, 31);
            this.nudJogStep.TabIndex = 3;
            this.nudJogStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnEStop
            // 
            this.btnEStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tlpJog.SetColumnSpan(this.btnEStop, 4);
            this.btnEStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEStop.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnEStop.ForeColor = System.Drawing.Color.White;
            this.btnEStop.Location = new System.Drawing.Point(4, 282);
            this.btnEStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnEStop.Name = "btnEStop";
            this.btnEStop.Size = new System.Drawing.Size(728, 53);
            this.btnEStop.TabIndex = 13;
            this.btnEStop.Text = "■ 急停 (E-STOP)";
            this.btnEStop.UseVisualStyleBackColor = false;
            // 
            // lblAxisHdrX
            // 
            this.lblAxisHdrX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisHdrX.Location = new System.Drawing.Point(4, 48);
            this.lblAxisHdrX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisHdrX.Name = "lblAxisHdrX";
            this.lblAxisHdrX.Size = new System.Drawing.Size(82, 76);
            this.lblAxisHdrX.TabIndex = 4;
            this.lblAxisHdrX.Text = "X 轴";
            this.lblAxisHdrX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogXMinus
            // 
            this.jogXMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogXMinus.Direction = -1;
            this.jogXMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogXMinus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogXMinus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogXMinus.Location = new System.Drawing.Point(94, 52);
            this.jogXMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogXMinus.Name = "jogXMinus";
            this.jogXMinus.Size = new System.Drawing.Size(247, 68);
            this.jogXMinus.TabIndex = 5;
            this.jogXMinus.Text = "◄ X-";
            // 
            // jogXPlus
            // 
            this.jogXPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogXPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogXPlus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogXPlus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogXPlus.Location = new System.Drawing.Point(349, 52);
            this.jogXPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogXPlus.Name = "jogXPlus";
            this.jogXPlus.Size = new System.Drawing.Size(247, 68);
            this.jogXPlus.TabIndex = 6;
            this.jogXPlus.Text = "X+ ►";
            // 
            // lblAxisHdrY
            // 
            this.lblAxisHdrY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisHdrY.Location = new System.Drawing.Point(4, 124);
            this.lblAxisHdrY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisHdrY.Name = "lblAxisHdrY";
            this.lblAxisHdrY.Size = new System.Drawing.Size(82, 76);
            this.lblAxisHdrY.TabIndex = 7;
            this.lblAxisHdrY.Text = "Y 轴";
            this.lblAxisHdrY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogYMinus
            // 
            this.jogYMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogYMinus.Direction = -1;
            this.jogYMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogYMinus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogYMinus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogYMinus.Location = new System.Drawing.Point(94, 128);
            this.jogYMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogYMinus.Name = "jogYMinus";
            this.jogYMinus.Size = new System.Drawing.Size(247, 68);
            this.jogYMinus.TabIndex = 8;
            this.jogYMinus.Text = "▼ Y-";
            // 
            // jogYPlus
            // 
            this.jogYPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogYPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogYPlus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogYPlus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogYPlus.Location = new System.Drawing.Point(349, 128);
            this.jogYPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogYPlus.Name = "jogYPlus";
            this.jogYPlus.Size = new System.Drawing.Size(247, 68);
            this.jogYPlus.TabIndex = 9;
            this.jogYPlus.Text = "▲ Y+";
            // 
            // lblAxisHdrZ
            // 
            this.lblAxisHdrZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisHdrZ.Location = new System.Drawing.Point(4, 200);
            this.lblAxisHdrZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisHdrZ.Name = "lblAxisHdrZ";
            this.lblAxisHdrZ.Size = new System.Drawing.Size(82, 78);
            this.lblAxisHdrZ.TabIndex = 10;
            this.lblAxisHdrZ.Text = "Z 轴";
            this.lblAxisHdrZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogZMinus
            // 
            this.jogZMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogZMinus.Direction = -1;
            this.jogZMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogZMinus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogZMinus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogZMinus.Location = new System.Drawing.Point(94, 204);
            this.jogZMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogZMinus.Name = "jogZMinus";
            this.jogZMinus.Size = new System.Drawing.Size(247, 70);
            this.jogZMinus.TabIndex = 11;
            this.jogZMinus.Text = "▽ Z-";
            // 
            // jogZPlus
            // 
            this.jogZPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.jogZPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogZPlus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.jogZPlus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.jogZPlus.Location = new System.Drawing.Point(349, 204);
            this.jogZPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogZPlus.Name = "jogZPlus";
            this.jogZPlus.Size = new System.Drawing.Size(247, 70);
            this.jogZPlus.TabIndex = 12;
            this.jogZPlus.Text = "△ Z+";
            // 
            // grpZ
            // 
            this.grpZ.Controls.Add(this.btnZMinus);
            this.grpZ.Controls.Add(this.trbZ);
            this.grpZ.Controls.Add(this.btnZPlus);
            this.grpZ.Controls.Add(this.nudZ);
            this.grpZ.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpZ.Location = new System.Drawing.Point(18, 422);
            this.grpZ.Margin = new System.Windows.Forms.Padding(6);
            this.grpZ.Name = "grpZ";
            this.grpZ.Padding = new System.Windows.Forms.Padding(6);
            this.grpZ.Size = new System.Drawing.Size(748, 202);
            this.grpZ.TabIndex = 3;
            this.grpZ.TabStop = false;
            this.grpZ.Text = "Z 轴";
            // 
            // btnZMinus
            // 
            this.btnZMinus.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnZMinus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnZMinus.Location = new System.Drawing.Point(6, 30);
            this.btnZMinus.Margin = new System.Windows.Forms.Padding(6);
            this.btnZMinus.Name = "btnZMinus";
            this.btnZMinus.Size = new System.Drawing.Size(126, 166);
            this.btnZMinus.TabIndex = 0;
            this.btnZMinus.Text = "▽ -1";
            this.btnZMinus.UseVisualStyleBackColor = true;
            // 
            // trbZ
            // 
            this.trbZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbZ.Location = new System.Drawing.Point(6, 30);
            this.trbZ.Margin = new System.Windows.Forms.Padding(6);
            this.trbZ.Maximum = 100;
            this.trbZ.Minimum = -50;
            this.trbZ.Name = "trbZ";
            this.trbZ.Size = new System.Drawing.Size(475, 166);
            this.trbZ.TabIndex = 1;
            this.trbZ.TickFrequency = 10;
            // 
            // btnZPlus
            // 
            this.btnZPlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnZPlus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnZPlus.Location = new System.Drawing.Point(481, 30);
            this.btnZPlus.Margin = new System.Windows.Forms.Padding(6);
            this.btnZPlus.Name = "btnZPlus";
            this.btnZPlus.Size = new System.Drawing.Size(126, 166);
            this.btnZPlus.TabIndex = 2;
            this.btnZPlus.Text = "+1 △";
            this.btnZPlus.UseVisualStyleBackColor = true;
            // 
            // nudZ
            // 
            this.nudZ.DecimalPlaces = 2;
            this.nudZ.Dock = System.Windows.Forms.DockStyle.Right;
            this.nudZ.Location = new System.Drawing.Point(607, 30);
            this.nudZ.Margin = new System.Windows.Forms.Padding(6);
            this.nudZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudZ.Name = "nudZ";
            this.nudZ.Size = new System.Drawing.Size(135, 31);
            this.nudZ.TabIndex = 3;
            this.nudZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpY
            // 
            this.grpY.Controls.Add(this.btnYMinus);
            this.grpY.Controls.Add(this.trbY);
            this.grpY.Controls.Add(this.btnYPlus);
            this.grpY.Controls.Add(this.nudY);
            this.grpY.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpY.Location = new System.Drawing.Point(18, 220);
            this.grpY.Margin = new System.Windows.Forms.Padding(6);
            this.grpY.Name = "grpY";
            this.grpY.Padding = new System.Windows.Forms.Padding(6);
            this.grpY.Size = new System.Drawing.Size(748, 202);
            this.grpY.TabIndex = 2;
            this.grpY.TabStop = false;
            this.grpY.Text = "Y 轴";
            // 
            // btnYMinus
            // 
            this.btnYMinus.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnYMinus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnYMinus.Location = new System.Drawing.Point(6, 30);
            this.btnYMinus.Margin = new System.Windows.Forms.Padding(6);
            this.btnYMinus.Name = "btnYMinus";
            this.btnYMinus.Size = new System.Drawing.Size(126, 166);
            this.btnYMinus.TabIndex = 0;
            this.btnYMinus.Text = "▼ -1";
            this.btnYMinus.UseVisualStyleBackColor = true;
            // 
            // trbY
            // 
            this.trbY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbY.Location = new System.Drawing.Point(6, 30);
            this.trbY.Margin = new System.Windows.Forms.Padding(6);
            this.trbY.Maximum = 100;
            this.trbY.Minimum = -100;
            this.trbY.Name = "trbY";
            this.trbY.Size = new System.Drawing.Size(475, 166);
            this.trbY.TabIndex = 1;
            this.trbY.TickFrequency = 10;
            // 
            // btnYPlus
            // 
            this.btnYPlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnYPlus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnYPlus.Location = new System.Drawing.Point(481, 30);
            this.btnYPlus.Margin = new System.Windows.Forms.Padding(6);
            this.btnYPlus.Name = "btnYPlus";
            this.btnYPlus.Size = new System.Drawing.Size(126, 166);
            this.btnYPlus.TabIndex = 2;
            this.btnYPlus.Text = "+1 ▲";
            this.btnYPlus.UseVisualStyleBackColor = true;
            // 
            // nudY
            // 
            this.nudY.DecimalPlaces = 2;
            this.nudY.Dock = System.Windows.Forms.DockStyle.Right;
            this.nudY.Location = new System.Drawing.Point(607, 30);
            this.nudY.Margin = new System.Windows.Forms.Padding(6);
            this.nudY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(135, 31);
            this.nudY.TabIndex = 3;
            this.nudY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpX
            // 
            this.grpX.Controls.Add(this.btnXMinus);
            this.grpX.Controls.Add(this.trbX);
            this.grpX.Controls.Add(this.btnXPlus);
            this.grpX.Controls.Add(this.nudX);
            this.grpX.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpX.Location = new System.Drawing.Point(18, 18);
            this.grpX.Margin = new System.Windows.Forms.Padding(6);
            this.grpX.Name = "grpX";
            this.grpX.Padding = new System.Windows.Forms.Padding(6);
            this.grpX.Size = new System.Drawing.Size(748, 202);
            this.grpX.TabIndex = 1;
            this.grpX.TabStop = false;
            this.grpX.Text = "X 轴";
            // 
            // btnXMinus
            // 
            this.btnXMinus.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnXMinus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnXMinus.Location = new System.Drawing.Point(6, 30);
            this.btnXMinus.Margin = new System.Windows.Forms.Padding(6);
            this.btnXMinus.Name = "btnXMinus";
            this.btnXMinus.Size = new System.Drawing.Size(126, 166);
            this.btnXMinus.TabIndex = 0;
            this.btnXMinus.Text = "◀ -1";
            this.btnXMinus.UseVisualStyleBackColor = true;
            // 
            // trbX
            // 
            this.trbX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbX.Location = new System.Drawing.Point(6, 30);
            this.trbX.Margin = new System.Windows.Forms.Padding(6);
            this.trbX.Maximum = 100;
            this.trbX.Minimum = -100;
            this.trbX.Name = "trbX";
            this.trbX.Size = new System.Drawing.Size(475, 166);
            this.trbX.TabIndex = 1;
            this.trbX.TickFrequency = 10;
            this.trbX.Scroll += new System.EventHandler(this.trbX_Scroll);
            // 
            // btnXPlus
            // 
            this.btnXPlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnXPlus.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnXPlus.Location = new System.Drawing.Point(481, 30);
            this.btnXPlus.Margin = new System.Windows.Forms.Padding(6);
            this.btnXPlus.Name = "btnXPlus";
            this.btnXPlus.Size = new System.Drawing.Size(126, 166);
            this.btnXPlus.TabIndex = 2;
            this.btnXPlus.Text = "+1 ▶";
            this.btnXPlus.UseVisualStyleBackColor = true;
            // 
            // nudX
            // 
            this.nudX.DecimalPlaces = 2;
            this.nudX.Dock = System.Windows.Forms.DockStyle.Right;
            this.nudX.Location = new System.Drawing.Point(607, 30);
            this.nudX.Margin = new System.Windows.Forms.Padding(6);
            this.nudX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(135, 31);
            this.nudX.TabIndex = 3;
            this.nudX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblHint});
            this.statusStrip.Location = new System.Drawing.Point(0, 1253);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(3, 0, 27, 0);
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(2564, 29);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1724, 24);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "就绪";
            this.lblStatus.ToolTipText = "实时坐标";
            // 
            // lblHint
            // 
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(810, 24);
            this.lblHint.Text = "快捷键：方向键/WASD 控 X/Y；Q/E 或 PgUp/PgDn 控 Z；Shift 加速；Space 回原点；Esc 清轨迹";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 10;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2564, 1282);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(1908, 965);
            this.Name = "RunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XYZ 轴实时控制器  ·  WinForms  ·  .NET Framework 4.6.1";
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.grpCommon.ResumeLayout(false);
            this.tlpCommon.ResumeLayout(false);
            this.tlpCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            this.grpJog.ResumeLayout(false);
            this.tlpJog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudJogStep)).EndInit();
            this.grpZ.ResumeLayout(false);
            this.grpZ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZ)).EndInit();
            this.grpY.ResumeLayout(false);
            this.grpY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            this.grpX.ResumeLayout(false);
            this.grpX.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
