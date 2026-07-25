namespace ProcessModules.PointJump
{
    /// <summary>
    /// 点位跳转窗体的设计器文件。
    /// 兼容 VS2017 WinForms 设计器：纯声明式逐行属性设置，无 Lambda / 集合初始化器。
    /// </summary>
    partial class RunForm
    {
        private System.ComponentModel.IContainer components = null;

        // —— 布局容器 ——
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlRight;

        // —— XY 视图 ——
        private ProcessModules.XYView xyView;

        // —— 实时坐标 DRO ——
        private System.Windows.Forms.GroupBox grpDro;
        private System.Windows.Forms.TableLayoutPanel tlpDro;
        private ProcessModules.DroLabel droX;
        private ProcessModules.DroLabel droY;
        private ProcessModules.DroLabel droZ;

        // —— 目标坐标输入 ——
        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.TableLayoutPanel tlpTarget;
        private System.Windows.Forms.Label lblTargetX;
        private System.Windows.Forms.NumericUpDown nudTargetX;
        private System.Windows.Forms.Label lblTargetY;
        private System.Windows.Forms.NumericUpDown nudTargetY;
        private System.Windows.Forms.Label lblTargetZ;
        private System.Windows.Forms.NumericUpDown nudTargetZ;
        private System.Windows.Forms.Button btnJump;

        // —— 预设点位 ——
        private System.Windows.Forms.GroupBox grpPresets;
        private System.Windows.Forms.TableLayoutPanel tlpPresets;
        private System.Windows.Forms.ListView lvPresets;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colX;
        private System.Windows.Forms.ColumnHeader colY;
        private System.Windows.Forms.ColumnHeader colZ;
        private System.Windows.Forms.Button btnSavePos;
        private System.Windows.Forms.Button btnDeletePos;
        private System.Windows.Forms.Button btnGotoSelected;

        // —— 速度控制 ——
        private System.Windows.Forms.GroupBox grpSpeed;
        private System.Windows.Forms.TableLayoutPanel tlpSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.TrackBar trbSpeed;

        // —— 状态栏 ——
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;

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
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpSpeed = new System.Windows.Forms.GroupBox();
            this.tlpSpeed = new System.Windows.Forms.TableLayoutPanel();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.grpPresets = new System.Windows.Forms.GroupBox();
            this.tlpPresets = new System.Windows.Forms.TableLayoutPanel();
            this.lvPresets = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colZ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSavePos = new System.Windows.Forms.Button();
            this.btnDeletePos = new System.Windows.Forms.Button();
            this.btnGotoSelected = new System.Windows.Forms.Button();
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.tlpTarget = new System.Windows.Forms.TableLayoutPanel();
            this.lblTargetX = new System.Windows.Forms.Label();
            this.nudTargetX = new System.Windows.Forms.NumericUpDown();
            this.lblTargetY = new System.Windows.Forms.Label();
            this.nudTargetY = new System.Windows.Forms.NumericUpDown();
            this.lblTargetZ = new System.Windows.Forms.Label();
            this.nudTargetZ = new System.Windows.Forms.NumericUpDown();
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
            this.grpSpeed.SuspendLayout();
            this.tlpSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            this.grpPresets.SuspendLayout();
            this.tlpPresets.SuspendLayout();
            this.grpTarget.SuspendLayout();
            this.tlpTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetZ)).BeginInit();
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
            this.splitMain.Panel2.Controls.Add(this.pnlRight);
            this.splitMain.Panel2MinSize = 420;
            this.splitMain.Size = new System.Drawing.Size(1926, 1101);
            this.splitMain.SplitterDistance = 1460;
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
            this.xyView.Size = new System.Drawing.Size(1512, 1077);
            this.xyView.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.Controls.Add(this.grpSpeed);
            this.pnlRight.Controls.Add(this.grpPresets);
            this.pnlRight.Controls.Add(this.grpTarget);
            this.pnlRight.Controls.Add(this.grpDro);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(4);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(14);
            this.pnlRight.Size = new System.Drawing.Size(460, 1101);
            this.pnlRight.TabIndex = 0;
            // 
            // grpSpeed
            // 
            this.grpSpeed.Controls.Add(this.tlpSpeed);
            this.grpSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSpeed.Location = new System.Drawing.Point(14, 882);
            this.grpSpeed.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpSpeed.Name = "grpSpeed";
            this.grpSpeed.Padding = new System.Windows.Forms.Padding(8);
            this.grpSpeed.Size = new System.Drawing.Size(432, 110);
            this.grpSpeed.TabIndex = 3;
            this.grpSpeed.TabStop = false;
            this.grpSpeed.Text = "运动速度";
            // 
            // tlpSpeed
            // 
            this.tlpSpeed.ColumnCount = 2;
            this.tlpSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tlpSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSpeed.Controls.Add(this.lblSpeed, 0, 0);
            this.tlpSpeed.Controls.Add(this.trbSpeed, 1, 0);
            this.tlpSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSpeed.Location = new System.Drawing.Point(6, 30);
            this.tlpSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.tlpSpeed.Name = "tlpSpeed";
            this.tlpSpeed.RowCount = 1;
            this.tlpSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSpeed.Size = new System.Drawing.Size(348, 84);
            this.tlpSpeed.TabIndex = 0;
            // 
            // lblSpeed
            // 
            this.lblSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeed.Location = new System.Drawing.Point(4, 0);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(97, 84);
            this.lblSpeed.TabIndex = 0;
            this.lblSpeed.Text = "速度：中";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trbSpeed
            // 
            this.trbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trbSpeed.Location = new System.Drawing.Point(109, 4);
            this.trbSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.Size = new System.Drawing.Size(235, 76);
            this.trbSpeed.TabIndex = 1;
            this.trbSpeed.TickFrequency = 10;
            this.trbSpeed.Value = 10;
            // 
            // grpPresets
            // 
            this.grpPresets.Controls.Add(this.tlpPresets);
            this.grpPresets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPresets.Location = new System.Drawing.Point(14, 522);
            this.grpPresets.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpPresets.Name = "grpPresets";
            this.grpPresets.Padding = new System.Windows.Forms.Padding(8);
            this.grpPresets.Size = new System.Drawing.Size(432, 340);
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
            this.tlpPresets.Location = new System.Drawing.Point(6, 30);
            this.tlpPresets.Margin = new System.Windows.Forms.Padding(4);
            this.tlpPresets.Name = "tlpPresets";
            this.tlpPresets.RowCount = 2;
            this.tlpPresets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPresets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpPresets.Size = new System.Drawing.Size(348, 324);
            this.tlpPresets.TabIndex = 0;
            // 
            // lvPresets
            // 
            this.lvPresets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colX,
            this.colY,
            this.colZ});
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
            this.lvPresets.Size = new System.Drawing.Size(340, 262);
            this.lvPresets.TabIndex = 0;
            this.lvPresets.UseCompatibleStateImageBehavior = false;
            this.lvPresets.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "名称";
            this.colName.Width = 80;
            // 
            // colX
            // 
            this.colX.Text = "X";
            this.colX.Width = 80;
            // 
            // colY
            // 
            this.colY.Text = "Y";
            this.colY.Width = 80;
            // 
            // colZ
            // 
            this.colZ.Text = "Z";
            this.colZ.Width = 80;
            // 
            // btnSavePos
            // 
            this.btnSavePos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSavePos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSavePos.Location = new System.Drawing.Point(4, 274);
            this.btnSavePos.Margin = new System.Windows.Forms.Padding(4);
            this.btnSavePos.Name = "btnSavePos";
            this.btnSavePos.Size = new System.Drawing.Size(107, 46);
            this.btnSavePos.TabIndex = 1;
            this.btnSavePos.Text = "保存当前";
            this.btnSavePos.UseVisualStyleBackColor = true;
            // 
            // btnDeletePos
            // 
            this.btnDeletePos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeletePos.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDeletePos.Location = new System.Drawing.Point(119, 274);
            this.btnDeletePos.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeletePos.Name = "btnDeletePos";
            this.btnDeletePos.Size = new System.Drawing.Size(107, 46);
            this.btnDeletePos.TabIndex = 2;
            this.btnDeletePos.Text = "删除选中";
            this.btnDeletePos.UseVisualStyleBackColor = true;
            // 
            // btnGotoSelected
            // 
            this.btnGotoSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGotoSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGotoSelected.Location = new System.Drawing.Point(234, 274);
            this.btnGotoSelected.Margin = new System.Windows.Forms.Padding(4);
            this.btnGotoSelected.Name = "btnGotoSelected";
            this.btnGotoSelected.Size = new System.Drawing.Size(110, 46);
            this.btnGotoSelected.TabIndex = 3;
            this.btnGotoSelected.Text = "跳转选中";
            this.btnGotoSelected.UseVisualStyleBackColor = true;
            // 
            // grpTarget
            // 
            this.grpTarget.Controls.Add(this.tlpTarget);
            this.grpTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTarget.Location = new System.Drawing.Point(14, 252);
            this.grpTarget.Margin = new System.Windows.Forms.Padding(6, 12, 6, 4);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Padding = new System.Windows.Forms.Padding(8);
            this.grpTarget.Size = new System.Drawing.Size(432, 260);
            this.grpTarget.TabIndex = 1;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "目标坐标输入";
            // 
            // tlpTarget
            // 
            this.tlpTarget.ColumnCount = 2;
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpTarget.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTarget.Controls.Add(this.lblTargetX, 0, 0);
            this.tlpTarget.Controls.Add(this.nudTargetX, 1, 0);
            this.tlpTarget.Controls.Add(this.lblTargetY, 0, 1);
            this.tlpTarget.Controls.Add(this.nudTargetY, 1, 1);
            this.tlpTarget.Controls.Add(this.lblTargetZ, 0, 2);
            this.tlpTarget.Controls.Add(this.nudTargetZ, 1, 2);
            this.tlpTarget.Controls.Add(this.btnJump, 0, 3);
            this.tlpTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTarget.Location = new System.Drawing.Point(6, 30);
            this.tlpTarget.Margin = new System.Windows.Forms.Padding(4);
            this.tlpTarget.Name = "tlpTarget";
            this.tlpTarget.RowCount = 4;
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpTarget.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTarget.Size = new System.Drawing.Size(348, 234);
            this.tlpTarget.TabIndex = 0;
            // 
            // lblTargetX
            // 
            this.lblTargetX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTargetX.Location = new System.Drawing.Point(4, 0);
            this.lblTargetX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetX.Name = "lblTargetX";
            this.lblTargetX.Size = new System.Drawing.Size(82, 54);
            this.lblTargetX.TabIndex = 0;
            this.lblTargetX.Text = "X:";
            this.lblTargetX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTargetX
            // 
            this.nudTargetX.DecimalPlaces = 2;
            this.nudTargetX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTargetX.Location = new System.Drawing.Point(96, 6);
            this.nudTargetX.Margin = new System.Windows.Forms.Padding(6);
            this.nudTargetX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTargetX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudTargetX.Name = "nudTargetX";
            this.nudTargetX.Size = new System.Drawing.Size(246, 31);
            this.nudTargetX.TabIndex = 1;
            this.nudTargetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTargetY
            // 
            this.lblTargetY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTargetY.Location = new System.Drawing.Point(4, 54);
            this.lblTargetY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetY.Name = "lblTargetY";
            this.lblTargetY.Size = new System.Drawing.Size(82, 54);
            this.lblTargetY.TabIndex = 2;
            this.lblTargetY.Text = "Y:";
            this.lblTargetY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTargetY
            // 
            this.nudTargetY.DecimalPlaces = 2;
            this.nudTargetY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTargetY.Location = new System.Drawing.Point(96, 60);
            this.nudTargetY.Margin = new System.Windows.Forms.Padding(6);
            this.nudTargetY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTargetY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudTargetY.Name = "nudTargetY";
            this.nudTargetY.Size = new System.Drawing.Size(246, 31);
            this.nudTargetY.TabIndex = 3;
            this.nudTargetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTargetZ
            // 
            this.lblTargetZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTargetZ.Location = new System.Drawing.Point(4, 108);
            this.lblTargetZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetZ.Name = "lblTargetZ";
            this.lblTargetZ.Size = new System.Drawing.Size(82, 54);
            this.lblTargetZ.TabIndex = 4;
            this.lblTargetZ.Text = "Z:";
            this.lblTargetZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTargetZ
            // 
            this.nudTargetZ.DecimalPlaces = 2;
            this.nudTargetZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudTargetZ.Location = new System.Drawing.Point(96, 114);
            this.nudTargetZ.Margin = new System.Windows.Forms.Padding(6);
            this.nudTargetZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTargetZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.nudTargetZ.Name = "nudTargetZ";
            this.nudTargetZ.Size = new System.Drawing.Size(246, 31);
            this.nudTargetZ.TabIndex = 5;
            this.nudTargetZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnJump
            // 
            this.btnJump.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(220)))));
            this.tlpTarget.SetColumnSpan(this.btnJump, 2);
            this.btnJump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJump.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJump.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnJump.ForeColor = System.Drawing.Color.White;
            this.btnJump.Location = new System.Drawing.Point(4, 166);
            this.btnJump.Margin = new System.Windows.Forms.Padding(4);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(340, 64);
            this.btnJump.TabIndex = 6;
            this.btnJump.Text = "▶ 跳转到目标点位";
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
            this.grpDro.Size = new System.Drawing.Size(432, 220);
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
            this.tlpDro.Location = new System.Drawing.Point(6, 30);
            this.tlpDro.Margin = new System.Windows.Forms.Padding(4);
            this.tlpDro.Name = "tlpDro";
            this.tlpDro.RowCount = 3;
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpDro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpDro.Size = new System.Drawing.Size(348, 204);
            this.tlpDro.TabIndex = 0;
            // 
            // droX
            // 
            this.droX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droX.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.droX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droX.Location = new System.Drawing.Point(4, 4);
            this.droX.Margin = new System.Windows.Forms.Padding(4);
            this.droX.Name = "droX";
            this.droX.Size = new System.Drawing.Size(340, 59);
            this.droX.TabIndex = 0;
            // 
            // droY
            // 
            this.droY.AxisName = "Y";
            this.droY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droY.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.droY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droY.Location = new System.Drawing.Point(4, 71);
            this.droY.Margin = new System.Windows.Forms.Padding(4);
            this.droY.Name = "droY";
            this.droY.Size = new System.Drawing.Size(340, 59);
            this.droY.TabIndex = 1;
            // 
            // droZ
            // 
            this.droZ.AxisName = "Z";
            this.droZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(28)))), ((int)(((byte)(40)))));
            this.droZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.droZ.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.droZ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.droZ.Location = new System.Drawing.Point(4, 138);
            this.droZ.Margin = new System.Windows.Forms.Padding(4);
            this.droZ.Name = "droZ";
            this.droZ.Size = new System.Drawing.Size(340, 62);
            this.droZ.TabIndex = 2;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 1101);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1926, 29);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1903, 24);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "就绪";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // PointJumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1926, 1130);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1339, 872);
            this.Name = "RunForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "点位跳转  ·  XYZ 工控机";
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.grpSpeed.ResumeLayout(false);
            this.tlpSpeed.ResumeLayout(false);
            this.tlpSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            this.grpPresets.ResumeLayout(false);
            this.tlpPresets.ResumeLayout(false);
            this.grpTarget.ResumeLayout(false);
            this.tlpTarget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetZ)).EndInit();
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
