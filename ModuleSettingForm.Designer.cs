namespace ProcessModules
{
    partial class ModuleSettingForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this._tab = new System.Windows.Forms.TabControl();
            this._pageGlobal = new System.Windows.Forms.TabPage();
            this._gridGlobal = new System.Windows.Forms.PropertyGrid();
            this._pageProject = new System.Windows.Forms.TabPage();
            this._gridProject = new System.Windows.Forms.PropertyGrid();
            this._bottom = new System.Windows.Forms.Panel();
            this._lblHint = new System.Windows.Forms.Label();
            this._btnSave = new System.Windows.Forms.Button();
            this._tab.SuspendLayout();
            this._pageGlobal.SuspendLayout();
            this._pageProject.SuspendLayout();
            this._bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tab
            // 
            this._tab.Controls.Add(this._pageGlobal);
            this._tab.Controls.Add(this._pageProject);
            this._tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tab.Location = new System.Drawing.Point(0, 0);
            this._tab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._tab.Name = "_tab";
            this._tab.SelectedIndex = 0;
            this._tab.Size = new System.Drawing.Size(1185, 704);
            this._tab.TabIndex = 0;
            // 
            // _pageGlobal
            // 
            this._pageGlobal.Controls.Add(this._gridGlobal);
            this._pageGlobal.Location = new System.Drawing.Point(4, 28);
            this._pageGlobal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._pageGlobal.Name = "_pageGlobal";
            this._pageGlobal.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._pageGlobal.Size = new System.Drawing.Size(1177, 672);
            this._pageGlobal.TabIndex = 0;
            this._pageGlobal.Text = "全局参数";
            this._pageGlobal.UseVisualStyleBackColor = true;
            // 
            // _gridGlobal
            // 
            this._gridGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridGlobal.Location = new System.Drawing.Point(4, 4);
            this._gridGlobal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._gridGlobal.Name = "_gridGlobal";
            this._gridGlobal.Size = new System.Drawing.Size(1169, 664);
            this._gridGlobal.TabIndex = 0;
            this._gridGlobal.ToolbarVisible = false;
            // 
            // _pageProject
            // 
            this._pageProject.Controls.Add(this._gridProject);
            this._pageProject.Location = new System.Drawing.Point(4, 28);
            this._pageProject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._pageProject.Name = "_pageProject";
            this._pageProject.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._pageProject.Size = new System.Drawing.Size(1168, 668);
            this._pageProject.TabIndex = 1;
            this._pageProject.Text = "项目参数";
            this._pageProject.UseVisualStyleBackColor = true;
            // 
            // _gridProject
            // 
            this._gridProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridProject.Location = new System.Drawing.Point(4, 4);
            this._gridProject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._gridProject.Name = "_gridProject";
            this._gridProject.Size = new System.Drawing.Size(1160, 660);
            this._gridProject.TabIndex = 0;
            this._gridProject.ToolbarVisible = false;
            // 
            // _bottom
            // 
            this._bottom.Controls.Add(this._lblHint);
            this._bottom.Controls.Add(this._btnSave);
            this._bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottom.Location = new System.Drawing.Point(0, 704);
            this._bottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._bottom.Name = "_bottom";
            this._bottom.Size = new System.Drawing.Size(1185, 66);
            this._bottom.TabIndex = 1;
            // 
            // _lblHint
            // 
            this._lblHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblHint.ForeColor = System.Drawing.Color.DimGray;
            this._lblHint.Location = new System.Drawing.Point(15, 10);
            this._lblHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblHint.Name = "_lblHint";
            this._lblHint.Size = new System.Drawing.Size(960, 45);
            this._lblHint.TabIndex = 1;
            this._lblHint.Text = "修改后请点击保存，全局参数写入 AppParam，项目参数写入当前项目目录。";
            this._lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnSave
            // 
            this._btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnSave.Location = new System.Drawing.Point(1005, 10);
            this._btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(165, 45);
            this._btnSave.TabIndex = 0;
            this._btnSave.Text = "保存参数";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // ModuleSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1185, 770);
            this.Controls.Add(this._tab);
            this.Controls.Add(this._bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ModuleSettingForm";
            this.Text = "模组参数设置";
            this._tab.ResumeLayout(false);
            this._pageGlobal.ResumeLayout(false);
            this._pageProject.ResumeLayout(false);
            this._bottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tab;
        private System.Windows.Forms.TabPage _pageGlobal;
        private System.Windows.Forms.PropertyGrid _gridGlobal;
        private System.Windows.Forms.TabPage _pageProject;
        private System.Windows.Forms.PropertyGrid _gridProject;
        private System.Windows.Forms.Panel _bottom;
        private System.Windows.Forms.Label _lblHint;
        private System.Windows.Forms.Button _btnSave;
    }
}
