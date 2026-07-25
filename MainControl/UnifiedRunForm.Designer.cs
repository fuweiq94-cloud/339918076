namespace ProcessModules.MainControl
{
    /// <summary>
    /// 统一运行界面设计器文件。
    /// 合并 MainControl / PointJump / Trajectory 三个模组的全部功能。
    /// VS2017 设计器兼容：纯字面常量，无 Lambda / 表达式。
    /// </summary>
    partial class UnifiedRunForm
    {
        private System.ComponentModel.IContainer components = null;

        // —— 布局 ——
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlRight;

        // —— XY/Z 视图 ——
        private ProcessModules.XYView xyView;
        private ProcessModules.ZBarView zBar;

        // —— DRO 实时坐标 ——
        private System.Windows.Forms.GroupBox grpDro;
        private System.Windows.Forms.TableLayoutPanel tlpDro;
        private ProcessModules.DroLabel droX;
        private ProcessModules.DroLabel droY;
        private ProcessModules.DroLabel droZ;

        // —— 目标坐标输入 ——
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.TableLayoutPanel tlpTarget;
        private System.Windows.Forms.Label lblTX;
        private System.Windows.Forms.NumericUpDown nudTX;
        private System.Windows.Forms.Label lblTY;
        private System.Windows.Forms.NumericUpDown nudTY;
        private System.Windows.Forms.Label lblTZ;
        private System.Windows.Forms.NumericUpDown nudTZ;
        private System.Windows.Forms.Button btnJump;

        // —— 预设点位 ——
        private System.Windows.Forms.GroupBox grpPresets;
        private System.Windows.Forms.TableLayoutPanel tlpPresets;
        private System.Windows.Forms.ListView lvPresets;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colPX;
        private System.Windows.Forms.ColumnHeader colPY;
        private System.Windows.Forms.ColumnHeader colPZ;
        private System.Windows.Forms.Button btnSavePos;
        private System.Windows.Forms.Button btnDeletePos;
        private System.Windows.Forms.Button btnGotoSelected;

        // —— JOG 控制 ——
        private System.Windows.Forms.GroupBox grpJog;
        private System.Windows.Forms.TableLayoutPanel tlpJog;
        private System.Windows.Forms.RadioButton rbIncremental;
        private System.Windows.Forms.RadioButton rbContinuous;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.NumericUpDown nudJogStep;
        private System.Windows.Forms.Label lblAxisX;
        private ProcessModules.JogButton jogXMinus;
        private ProcessModules.JogButton jogXPlus;
        private System.Windows.Forms.Label lblAxisY;
        private ProcessModules.JogButton jogYMinus;
        private ProcessModules.JogButton jogYPlus;
        private System.Windows.Forms.Label lblAxisZ;
        private ProcessModules.JogButton jogZMinus;
        private ProcessModules.JogButton jogZPlus;
        private System.Windows.Forms.Button btnEStop;

        // —— 通用控制 ——
        private System.Windows.Forms.GroupBox grpCommon;
        private System.Windows.Forms.TableLayoutPanel tlpCommon;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trbSpeed;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Button btnCenter;
        private System.Windows.Forms.Button btnClearTrail;
        private System.Windows.Forms.CheckBox cbTrail;

        // —— 设置按钮 ——
        private System.Windows.Forms.Button btnSettings;

        // —— 状态栏 ——
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

        // —— 定时器 ——
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
            this.btnSettings = new System.Windows.Forms.Button();
            this.grpCommon = new System.Windows.Forms.GroupBox();
            this.tlpCommon = new System.Windows.Forms.TableLayoutPanel();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnCenter = new System.Windows.Forms.Button();
            this.btnClearTrail = new System.Windows.Forms.Button();
            this.cbTrail = new System.Windows.Forms.CheckBox();
            this.grpJog = new System.Windows.Forms.GroupBox();
            this.tlpJog = new System.Windows.Forms.TableLayoutPanel();
            this.rbIncremental = new System.Windows.Forms.RadioButton();
            this.rbContinuous = new System.Windows.Forms.RadioButton();
            this.lblStep = new System.Windows.Forms.Label();
            this.nudJogStep = new System.Windows.Forms.NumericUpDown();
            this.btnEStop = new System.Windows.Forms.Button();
            this.lblAxisX = new System.Windows.Forms.Label();
            this.jogXMinus = new ProcessModules.JogButton();
            this.jogXPlus = new ProcessModules.JogButton();
            this.lblAxisY = new System.Windows.Forms.Label();
            this.jogYMinus = new ProcessModules.JogButton();
            this.jogYPlus = new ProcessModules.JogButton();
            this.lblAxisZ = new System.Windows.Forms.Label();
            this.jogZMinus = new ProcessModules.JogButton();
            this.jogZPlus = new ProcessModules.JogButton();
            this.grpPresets = new System.Windows.Forms.GroupBox();
            this.tlpPresets = new System.Windows.Forms.TableLayoutPanel();
            this.lvPresets = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPZ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSavePos = new System.Windows.Forms.Button();
            this.btnDeletePos = new System.Windows.Forms.Button();
            this.btnGotoSelected = new System.Windows.Forms.Button();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.tlpTarget = new System.Windows.Forms.TableLayoutPanel();
            this.lblTX = new System.Windows.Forms.Label();
            this.nudTX = new System.Windows.Forms.NumericUpDown();
            this.lblTY = new System.Windows.Forms.Label();
            this.nudTY = new System.Windows.Forms.NumericUpDown();
            this.lblTZ = new System.Windows.Forms.Label();
            this.nudTZ = new System.Windows.Forms.NumericUpDown();
            this.btnJump = new System.Windows.Forms.Button();
            this.grpDro = new System.Windows.Forms.GroupBox();
            this.tlpDro = new System.Windows.Forms.TableLayoutPanel();
            this.droX = new ProcessModules.DroLabel();
            this.droY = new ProcessModules.DroLabel();
            this.droZ = new ProcessModules.DroLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.grpPresets.SuspendLayout();
            this.tlpPresets.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.tlpTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTZ)).BeginInit();
            this.grpDro.SuspendLayout();
            this.tlpDro.SuspendLayout();
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
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(8, 12, 8, 12);
            this.splitMain.Panel2MinSize = 80;
            this.splitMain.Size = new System.Drawing.Size(1504, 1101);
            this.splitMain.SplitterDistance = 1418;
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
            this.xyView.Size = new System.Drawing.Size(1394, 1077);
            this.xyView.TabIndex = 0;
            // 
            // zBar
            // 
            this.zBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.zBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zBar.Location = new System.Drawing.Point(8, 12);
            this.zBar.Margin = new System.Windows.Forms.Padding(6);
            this.zBar.Name = "zBar";
            this.zBar.Size = new System.Drawing.Size(64, 1077);
            this.zBar.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.btnSettings);
            this.pnlRight.Controls.Add(this.grpCommon);
            this.pnlRight.Controls.Add(this.grpJog);
            this.pnlRight.Controls.Add(this.grpPresets);
            this.pnlRight.Controls.Add(this.grpTarget);
            this.pnlRight.Controls.Add(this.grpDro);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1504, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(14);
            this.pnlRight.Size = new System.Drawing.Size(460, 1101);
            this.pnlRight.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSettings.Location = new System.Drawing.Point(14, 1044);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(432, 50);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.Text = "⚙ 轴限位设置";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // grpCommon
            // 
            this.grpCommon.Controls.Add(this.tlpCommon);
            this.grpCommon.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCommon.Location = new System.Drawing.Point(14, 884);
            this.grpCommon.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpCommon.Name = "grpCommon";
            this.grpCommon.Padding = new System.Windows.Forms.Padding(8);
            this.grpCommon.Size = new System.Drawing.Size(432, 160);
            this.grpCommon.TabIndex = 4;
            this.grpCommon.TabStop = false;
            this.grpCommon.Text = "通用控制";
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
            this.tlpCommon.Controls.Add(this.cbTrail, 3, 1);
            this.tlpCommon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommon.Location = new System.Drawing.Point(8, 28);
            this.tlpCommon.Name = "tlpCommon";
            this.tlpCommon.RowCount = 2;
            this.tlpCommon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCommon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCommon.Size = new System.Drawing.Size(416, 124);
            this.tlpCommon.TabIndex = 0;
            // 
            // lblSpeed
            // 
            this.lblSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeed.Location = new System.Drawing.Point(4, 0);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(96, 62);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "速度：中";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trbSpeed
            // 
            this.tlpCommon.SetColumnSpan(this.trbSpeed, 3);
            this.trbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbSpeed.Location = new System.Drawing.Point(108, 4);
            this.trbSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(304, 54);
            this.trbSpeed.TabIndex = 1;
            this.trbSpeed.TickFrequency = 10;
            this.trbSpeed.Value = 10;
            // 
            // btnZero
            // 
            this.btnZero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnZero.Location = new System.Drawing.Point(4, 66);
            this.btnZero.Margin = new System.Windows.Forms.Padding(4);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(96, 54);
            this.btnZero.TabIndex = 2;
            this.btnZero.Text = "回原点";
            this.btnZero.UseVisualStyleBackColor = true;
            // 
            // btnCenter
            // 
            this.btnCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCenter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCenter.Location = new System.Drawing.Point(108, 66);
            this.btnCenter.Margin = new System.Windows.Forms.Padding(4);
            this.btnCenter.Name = "btnCenter";
            this.btnCenter.Size = new System.Drawing.Size(96, 54);
            this.btnCenter.TabIndex = 3;
            this.btnCenter.Text = "居中";
            this.btnCenter.UseVisualStyleBackColor = true;
            // 
            // btnClearTrail
            // 
            this.btnClearTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearTrail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClearTrail.Location = new System.Drawing.Point(212, 66);
            this.btnClearTrail.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearTrail.Name = "btnClearTrail";
            this.btnClearTrail.Size = new System.Drawing.Size(96, 54);
            this.btnClearTrail.TabIndex = 4;
            this.btnClearTrail.Text = "清轨迹";
            this.btnClearTrail.UseVisualStyleBackColor = true;
            // 
            // cbTrail
            // 
            this.cbTrail.Checked = true;
            this.cbTrail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTrail.Location = new System.Drawing.Point(316, 66);
            this.cbTrail.Margin = new System.Windows.Forms.Padding(4);
            this.cbTrail.Name = "cbTrail";
            this.cbTrail.Size = new System.Drawing.Size(96, 54);
            this.cbTrail.TabIndex = 5;
            this.cbTrail.Text = "轨迹";
            this.cbTrail.UseVisualStyleBackColor = true;
            // 
            // grpJog
            // 
            this.grpJog.Controls.Add(this.tlpJog);
            this.grpJog.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpJog.Location = new System.Drawing.Point(14, 564);
            this.grpJog.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpJog.Name = "grpJog";
            this.grpJog.Padding = new System.Windows.Forms.Padding(8);
            this.grpJog.Size = new System.Drawing.Size(432, 320);
            this.grpJog.TabIndex = 3;
            this.grpJog.TabStop = false;
            this.grpJog.Text = "JOG 控制";
            // 
            // tlpJog
            // 
            this.tlpJog.ColumnCount = 4;
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpJog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpJog.Controls.Add(this.rbIncremental, 0, 0);
            this.tlpJog.Controls.Add(this.rbContinuous, 1, 0);
            this.tlpJog.Controls.Add(this.lblStep, 2, 0);
            this.tlpJog.Controls.Add(this.nudJogStep, 3, 0);
            this.tlpJog.Controls.Add(this.btnEStop, 0, 4);
            this.tlpJog.Controls.Add(this.lblAxisX, 0, 1);
            this.tlpJog.Controls.Add(this.jogXMinus, 1, 1);
            this.tlpJog.Controls.Add(this.jogXPlus, 2, 1);
            this.tlpJog.Controls.Add(this.lblAxisY, 0, 2);
            this.tlpJog.Controls.Add(this.jogYMinus, 1, 2);
            this.tlpJog.Controls.Add(this.jogYPlus, 2, 2);
            this.tlpJog.Controls.Add(this.lblAxisZ, 0, 3);
            this.tlpJog.Controls.Add(this.jogZMinus, 1, 3);
            this.tlpJog.Controls.Add(this.jogZPlus, 2, 3);
            this.tlpJog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpJog.Location = new System.Drawing.Point(8, 28);
            this.tlpJog.Name = "tlpJog";
            this.tlpJog.RowCount = 5;
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpJog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpJog.Size = new System.Drawing.Size(416, 284);
            this.tlpJog.TabIndex = 0;
            // 
            // rbIncremental
            // 
            this.rbIncremental.Checked = true;
            this.rbIncremental.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbIncremental.Location = new System.Drawing.Point(4, 4);
            this.rbIncremental.Margin = new System.Windows.Forms.Padding(4);
            this.rbIncremental.Name = "rbIncremental";
            this.rbIncremental.Size = new System.Drawing.Size(72, 36);
            this.rbIncremental.TabIndex = 0;
            this.rbIncremental.TabStop = true;
            this.rbIncremental.Text = "寸动";
            this.rbIncremental.UseVisualStyleBackColor = true;
            // 
            // rbContinuous
            // 
            this.rbContinuous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbContinuous.Location = new System.Drawing.Point(84, 4);
            this.rbContinuous.Margin = new System.Windows.Forms.Padding(4);
            this.rbContinuous.Name = "rbContinuous";
            this.rbContinuous.Size = new System.Drawing.Size(100, 36);
            this.rbContinuous.TabIndex = 1;
            this.rbContinuous.Text = "连续";
            this.rbContinuous.UseVisualStyleBackColor = true;
            // 
            // lblStep
            // 
            this.lblStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStep.Location = new System.Drawing.Point(192, 0);
            this.lblStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(100, 44);
            this.lblStep.TabIndex = 2;
            this.lblStep.Text = "步长:";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudJogStep
            // 
            this.nudJogStep.DecimalPlaces = 2;
            this.nudJogStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudJogStep.Location = new System.Drawing.Point(300, 6);
            this.nudJogStep.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudJogStep.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudJogStep.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            this.nudJogStep.Name = "nudJogStep";
            this.nudJogStep.Size = new System.Drawing.Size(112, 31);
            this.nudJogStep.TabIndex = 3;
            this.nudJogStep.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // btnEStop
            // 
            this.btnEStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tlpJog.SetColumnSpan(this.btnEStop, 4);
            this.btnEStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEStop.ForeColor = System.Drawing.Color.White;
            this.btnEStop.Location = new System.Drawing.Point(4, 234);
            this.btnEStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnEStop.Name = "btnEStop";
            this.btnEStop.Size = new System.Drawing.Size(408, 46);
            this.btnEStop.TabIndex = 13;
            this.btnEStop.Text = "■ 急停";
            this.btnEStop.UseVisualStyleBackColor = false;
            // 
            // lblAxisX
            // 
            this.lblAxisX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisX.Location = new System.Drawing.Point(4, 44);
            this.lblAxisX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisX.Name = "lblAxisX";
            this.lblAxisX.Size = new System.Drawing.Size(72, 62);
            this.lblAxisX.TabIndex = 4;
            this.lblAxisX.Text = "X";
            this.lblAxisX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogXMinus
            // 
            this.jogXMinus.Direction = -1;
            this.jogXMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogXMinus.Location = new System.Drawing.Point(84, 48);
            this.jogXMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogXMinus.Name = "jogXMinus";
            this.jogXMinus.Size = new System.Drawing.Size(100, 54);
            this.jogXMinus.TabIndex = 5;
            this.jogXMinus.Text = "◄ X-";
            // 
            // jogXPlus
            // 
            this.jogXPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogXPlus.Location = new System.Drawing.Point(192, 48);
            this.jogXPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogXPlus.Name = "jogXPlus";
            this.jogXPlus.Size = new System.Drawing.Size(100, 54);
            this.jogXPlus.TabIndex = 6;
            this.jogXPlus.Text = "X+ ►";
            // 
            // lblAxisY
            // 
            this.lblAxisY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisY.Location = new System.Drawing.Point(4, 106);
            this.lblAxisY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisY.Name = "lblAxisY";
            this.lblAxisY.Size = new System.Drawing.Size(72, 62);
            this.lblAxisY.TabIndex = 7;
            this.lblAxisY.Text = "Y";
            this.lblAxisY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogYMinus
            // 
            this.jogYMinus.Direction = -1;
            this.jogYMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogYMinus.Location = new System.Drawing.Point(84, 110);
            this.jogYMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogYMinus.Name = "jogYMinus";
            this.jogYMinus.Size = new System.Drawing.Size(100, 54);
            this.jogYMinus.TabIndex = 8;
            this.jogYMinus.Text = "▼ Y-";
            // 
            // jogYPlus
            // 
            this.jogYPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogYPlus.Location = new System.Drawing.Point(192, 110);
            this.jogYPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogYPlus.Name = "jogYPlus";
            this.jogYPlus.Size = new System.Drawing.Size(100, 54);
            this.jogYPlus.TabIndex = 9;
            this.jogYPlus.Text = "▲ Y+";
            // 
            // lblAxisZ
            // 
            this.lblAxisZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAxisZ.Location = new System.Drawing.Point(4, 168);
            this.lblAxisZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAxisZ.Name = "lblAxisZ";
            this.lblAxisZ.Size = new System.Drawing.Size(72, 62);
            this.lblAxisZ.TabIndex = 10;
            this.lblAxisZ.Text = "Z";
            this.lblAxisZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jogZMinus
            // 
            this.jogZMinus.Direction = -1;
            this.jogZMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogZMinus.Location = new System.Drawing.Point(84, 172);
            this.jogZMinus.Margin = new System.Windows.Forms.Padding(4);
            this.jogZMinus.Name = "jogZMinus";
            this.jogZMinus.Size = new System.Drawing.Size(100, 54);
            this.jogZMinus.TabIndex = 11;
            this.jogZMinus.Text = "▽ Z-";
            // 
            // jogZPlus
            // 
            this.jogZPlus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jogZPlus.Location = new System.Drawing.Point(192, 172);
            this.jogZPlus.Margin = new System.Windows.Forms.Padding(4);
            this.jogZPlus.Name = "jogZPlus";
            this.jogZPlus.Size = new System.Drawing.Size(100, 54);
            this.jogZPlus.TabIndex = 12;
            this.jogZPlus.Text = "△ Z+";
            // 
            // grpPresets
            // 
            this.grpPresets.Controls.Add(this.tlpPresets);
            this.grpPresets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPresets.Location = new System.Drawing.Point(14, 334);
            this.grpPresets.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpPresets.Name = "grpPresets";
            this.grpPresets.Padding = new System.Windows.Forms.Padding(8);
            this.grpPresets.Size = new System.Drawing.Size(432, 230);
            this.grpPresets.TabIndex = 2;
            this.grpPresets.TabStop = false;
            this.grpPresets.Text = "预设点位";
            // 
            // tlpPresets
            // 
            this.tlpPresets.ColumnCount = 3;
            this.tlpPresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpPresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpPresets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpPresets.Controls.Add(this.lvPresets, 0, 0);
            this.tlpPresets.Controls.Add(this.btnSavePos, 0, 1);
            this.tlpPresets.Controls.Add(this.btnDeletePos, 1, 1);
            this.tlpPresets.Controls.Add(this.btnGotoSelected, 2, 1);
            this.tlpPresets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPresets.Location = new System.Drawing.Point(8, 28);
            this.tlpPresets.Name = "tlpPresets";
            this.tlpPresets.RowCount = 2;
            this.tlpPresets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPresets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpPresets.Size = new System.Drawing.Size(416, 194);
            this.tlpPresets.TabIndex = 0;
            // 
            // lvPresets
            // 
            this.lvPresets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName, this.colPX, this.colPY, this.colPZ});
            this.tlpPresets.SetColumnSpan(this.lvPresets, 3);
            this.lvPresets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPresets.FullRowSelect = true;
            this.lvPresets.GridLines = true;
            this.lvPresets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvPresets.HideSelection = false;
            this.lvPresets.Location = new System.Drawing.Point(4, 4);
            this.lvPresets.Margin = new System.Windows.Forms.Padding(4);
            this.lvPresets.MultiSelect = false;
            this.lvPresets.Name = "lvPresets";
            this.lvPresets.Size = new System.Drawing.Size(408, 138);
            this.lvPresets.TabIndex = 0;
            this.lvPresets.UseCompatibleStateImageBehavior = false;
            this.lvPresets.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "名称";
            this.colName.Width = 90;
            // 
            // colPX
            // 
            this.colPX.Text = "X";
            this.colPX.Width = 90;
            // 
            // colPY
            // 
            this.colPY.Text = "Y";
            this.colPY.Width = 90;
            // 
            // colPZ
            // 
            this.colPZ.Text = "Z";
            this.colPZ.Width = 90;
            // 
            // btnSavePos
            // 
            this.btnSavePos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSavePos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSavePos.Location = new System.Drawing.Point(4, 150);
            this.btnSavePos.Margin = new System.Windows.Forms.Padding(4);
            this.btnSavePos.Name = "btnSavePos";
            this.btnSavePos.Size = new System.Drawing.Size(130, 40);
            this.btnSavePos.TabIndex = 1;
            this.btnSavePos.Text = "保存当前";
            this.btnSavePos.UseVisualStyleBackColor = true;
            // 
            // btnDeletePos
            // 
            this.btnDeletePos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeletePos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDeletePos.Location = new System.Drawing.Point(142, 150);
            this.btnDeletePos.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeletePos.Name = "btnDeletePos";
            this.btnDeletePos.Size = new System.Drawing.Size(130, 40);
            this.btnDeletePos.TabIndex = 2;
            this.btnDeletePos.Text = "删除选中";
            this.btnDeletePos.UseVisualStyleBackColor = true;
            // 
            // btnGotoSelected
            // 
            this.btnGotoSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGotoSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGotoSelected.Location = new System.Drawing.Point(280, 150);
            this.btnGotoSelected.Margin = new System.Windows.Forms.Padding(4);
            this.btnGotoSelected.Name = "btnGotoSelected";
            this.btnGotoSelected.Size = new System.Drawing.Size(132, 40);
            this.btnGotoSelected.TabIndex = 3;
            this.btnGotoSelected.Text = "跳转选中";
            this.btnGotoSelected.UseVisualStyleBackColor = true;
            // 
            // grpTarget
            // 
            this.grpTarget.Controls.Add(this.tlpTarget);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTarget.Location = new System.Drawing.Point(14, 194);
            this.grpTarget.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Padding = new System.Windows.Forms.Padding(8);
            this.grpTarget.Size = new System.Drawing.Size(432, 140);
            this.grpTarget.TabIndex = 1;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "目标坐标";
            // 
            // tlpTarget
            // 
            this.tlpTarget.ColumnCount = 4;
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpTarget.Controls.Add(this.lblTX, 0, 0);
            this.tlpTarget.Controls.Add(this.nudTX, 1, 0);
            this.tlpTarget.Controls.Add(this.lblTY, 2, 0);
            this.tlpTarget.Controls.Add(this.nudTY, 3, 0);
            this.tlpTarget.Controls.Add(this.lblTZ, 0, 1);
            this.tlpTarget.Controls.Add(this.nudTZ, 1, 1);
            this.tlpTarget.Controls.Add(this.btnJump, 2, 1);
            this.tlpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTarget.Location = new System.Drawing.Point(8, 28);
            this.tlpTarget.Name = "tlpTarget";
            this.tlpTarget.RowCount = 2;
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpTarget.Size = new System.Drawing.Size(416, 104);
            this.tlpTarget.TabIndex = 0;
            // 
            // lblTX
            // 
            this.lblTX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTX.Location = new System.Drawing.Point(4, 0);
            this.lblTX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTX.Name = "lblTX";
            this.lblTX.Size = new System.Drawing.Size(32, 48);
            this.lblTX.TabIndex = 0;
            this.lblTX.Text = "X:";
            this.lblTX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTX
            // 
            this.nudTX.DecimalPlaces = 2;
            this.nudTX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTX.Location = new System.Drawing.Point(44, 8);
            this.nudTX.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.nudTX.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudTX.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudTX.Name = "nudTX";
            this.nudTX.Size = new System.Drawing.Size(160, 31);
            this.nudTX.TabIndex = 1;
            this.nudTX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTY
            // 
            this.lblTY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTY.Location = new System.Drawing.Point(212, 0);
            this.lblTY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTY.Name = "lblTY";
            this.lblTY.Size = new System.Drawing.Size(32, 48);
            this.lblTY.TabIndex = 2;
            this.lblTY.Text = "Y:";
            this.lblTY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTY
            // 
            this.nudTY.DecimalPlaces = 2;
            this.nudTY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTY.Location = new System.Drawing.Point(252, 8);
            this.nudTY.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.nudTY.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudTY.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudTY.Name = "nudTY";
            this.nudTY.Size = new System.Drawing.Size(160, 31);
            this.nudTY.TabIndex = 3;
            this.nudTY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTZ
            // 
            this.lblTZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTZ.Location = new System.Drawing.Point(4, 48);
            this.lblTZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTZ.Name = "lblTZ";
            this.lblTZ.Size = new System.Drawing.Size(32, 56);
            this.lblTZ.TabIndex = 4;
            this.lblTZ.Text = "Z:";
            this.lblTZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTZ
            // 
            this.nudTZ.DecimalPlaces = 2;
            this.nudTZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTZ.Location = new System.Drawing.Point(44, 56);
            this.nudTZ.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.nudTZ.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudTZ.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudTZ.Name = "nudTZ";
            this.nudTZ.Size = new System.Drawing.Size(160, 31);
            this.nudTZ.TabIndex = 5;
            this.nudTZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnJump
            // 
            this.btnJump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(220)))));
            this.tlpTarget.SetColumnSpan(this.btnJump, 2);
            this.btnJump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJump.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnJump.ForeColor = System.Drawing.Color.White;
            this.btnJump.Location = new System.Drawing.Point(212, 52);
            this.btnJump.Margin = new System.Windows.Forms.Padding(4);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(200, 48);
            this.btnJump.TabIndex = 6;
            this.btnJump.Text = "▶ 跳转";
            this.btnJump.UseVisualStyleBackColor = false;
            // 
            // grpDro
            // 
            this.grpDro.Controls.Add(this.tlpDro);
            this.grpDro.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDro.Location = new System.Drawing.Point(14, 14);
            this.grpDro.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.grpDro.Name = "grpDro";
            this.grpDro.Padding = new System.Windows.Forms.Padding(8);
            this.grpDro.Size = new System.Drawing.Size(432, 180);
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
            this.tlpDro.Location = new System.Drawing.Point(8, 28);
            this.tlpDro.Name = "tlpDro";
            this.tlpDro.RowCount = 3;
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpDro.Size = new System.Drawing.Size(416, 144);
            this.tlpDro.TabIndex = 0;
            // 
            // droX
            // 
            this.droX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droX.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droX.Location = new System.Drawing.Point(4, 4);
            this.droX.Margin = new System.Windows.Forms.Padding(4);
            this.droX.Name = "droX";
            this.droX.Size = new System.Drawing.Size(408, 40);
            this.droX.TabIndex = 0;
            // 
            // droY
            // 
            this.droY.AxisName = "Y";
            this.droY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droY.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droY.Location = new System.Drawing.Point(4, 52);
            this.droY.Margin = new System.Windows.Forms.Padding(4);
            this.droY.Name = "droY";
            this.droY.Size = new System.Drawing.Size(408, 40);
            this.droY.TabIndex = 1;
            // 
            // droZ
            // 
            this.droZ.AxisName = "Z";
            this.droZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droZ.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.droZ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droZ.Location = new System.Drawing.Point(4, 100);
            this.droZ.Margin = new System.Windows.Forms.Padding(4);
            this.droZ.Name = "droZ";
            this.droZ.Size = new System.Drawing.Size(408, 40);
            this.droZ.TabIndex = 2;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus });
            this.statusStrip.Location = new System.Drawing.Point(0, 1101);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1964, 29);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1940, 24);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "就绪";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.AnimTimer_Tick);
            // 
            // UnifiedRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1964, 1130);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1400, 800);
            this.Name = "UnifiedRunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XYZ 统一控制台";
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
            this.grpPresets.ResumeLayout(false);
            this.tlpPresets.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.tlpTarget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTZ)).EndInit();
            this.grpDro.ResumeLayout(false);
            this.tlpDro.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
