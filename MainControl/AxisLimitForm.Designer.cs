namespace ProcessModules.MainControl
{
    partial class AxisLimitForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblHdrAxis;
        private System.Windows.Forms.Label lblHdrMin;
        private System.Windows.Forms.Label lblHdrMax;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.NumericUpDown nudXMin;
        private System.Windows.Forms.NumericUpDown nudXMax;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.NumericUpDown nudYMin;
        private System.Windows.Forms.NumericUpDown nudYMax;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.NumericUpDown nudZMin;
        private System.Windows.Forms.NumericUpDown nudZMax;
        private System.Windows.Forms.Label lblU;
        private System.Windows.Forms.NumericUpDown nudUMin;
        private System.Windows.Forms.NumericUpDown nudUMax;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblHdrAxis = new System.Windows.Forms.Label();
            this.lblHdrMin = new System.Windows.Forms.Label();
            this.lblHdrMax = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.nudXMin = new System.Windows.Forms.NumericUpDown();
            this.nudXMax = new System.Windows.Forms.NumericUpDown();
            this.lblY = new System.Windows.Forms.Label();
            this.nudYMin = new System.Windows.Forms.NumericUpDown();
            this.nudYMax = new System.Windows.Forms.NumericUpDown();
            this.lblZ = new System.Windows.Forms.Label();
            this.nudZMin = new System.Windows.Forms.NumericUpDown();
            this.nudZMax = new System.Windows.Forms.NumericUpDown();
            this.lblU = new System.Windows.Forms.Label();
            this.nudUMin = new System.Windows.Forms.NumericUpDown();
            this.nudUMax = new System.Windows.Forms.NumericUpDown();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUMax)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.lblHdrAxis, 0, 0);
            this.tlpMain.Controls.Add(this.lblHdrMin, 1, 0);
            this.tlpMain.Controls.Add(this.lblHdrMax, 2, 0);
            this.tlpMain.Controls.Add(this.lblX, 0, 1);
            this.tlpMain.Controls.Add(this.nudXMin, 1, 1);
            this.tlpMain.Controls.Add(this.nudXMax, 2, 1);
            this.tlpMain.Controls.Add(this.lblY, 0, 2);
            this.tlpMain.Controls.Add(this.nudYMin, 1, 2);
            this.tlpMain.Controls.Add(this.nudYMax, 2, 2);
            this.tlpMain.Controls.Add(this.lblZ, 0, 3);
            this.tlpMain.Controls.Add(this.nudZMin, 1, 3);
            this.tlpMain.Controls.Add(this.nudZMax, 2, 3);
            this.tlpMain.Controls.Add(this.lblU, 0, 4);
            this.tlpMain.Controls.Add(this.nudUMin, 1, 4);
            this.tlpMain.Controls.Add(this.nudUMax, 2, 4);
            this.tlpMain.Controls.Add(this.btnOK, 1, 5);
            this.tlpMain.Controls.Add(this.btnCancel, 2, 5);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(12, 12);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 6;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tlpMain.Size = new System.Drawing.Size(426, 300);
            this.tlpMain.TabIndex = 0;
            // 
            // lblHdrAxis
            // 
            this.lblHdrAxis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHdrAxis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHdrAxis.Location = new System.Drawing.Point(4, 0);
            this.lblHdrAxis.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHdrAxis.Name = "lblHdrAxis";
            this.lblHdrAxis.Size = new System.Drawing.Size(72, 44);
            this.lblHdrAxis.TabIndex = 0;
            this.lblHdrAxis.Text = "轴";
            this.lblHdrAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHdrMin
            // 
            this.lblHdrMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHdrMin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHdrMin.Location = new System.Drawing.Point(84, 0);
            this.lblHdrMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHdrMin.Name = "lblHdrMin";
            this.lblHdrMin.Size = new System.Drawing.Size(165, 44);
            this.lblHdrMin.TabIndex = 1;
            this.lblHdrMin.Text = "最小值";
            this.lblHdrMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHdrMax
            // 
            this.lblHdrMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHdrMax.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblHdrMax.Location = new System.Drawing.Point(257, 0);
            this.lblHdrMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHdrMax.Name = "lblHdrMax";
            this.lblHdrMax.Size = new System.Drawing.Size(165, 44);
            this.lblHdrMax.TabIndex = 2;
            this.lblHdrMax.Text = "最大值";
            this.lblHdrMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblX
            // 
            this.lblX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblX.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblX.Location = new System.Drawing.Point(4, 44);
            this.lblX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(72, 50);
            this.lblX.TabIndex = 3;
            this.lblX.Text = "X:";
            this.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudXMin
            // 
            this.nudXMin.DecimalPlaces = 1;
            this.nudXMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudXMin.Location = new System.Drawing.Point(84, 50);
            this.nudXMin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudXMin.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudXMin.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudXMin.Name = "nudXMin";
            this.nudXMin.Size = new System.Drawing.Size(165, 31);
            this.nudXMin.TabIndex = 4;
            // 
            // nudXMax
            // 
            this.nudXMax.DecimalPlaces = 1;
            this.nudXMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudXMax.Location = new System.Drawing.Point(257, 50);
            this.nudXMax.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudXMax.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudXMax.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudXMax.Name = "nudXMax";
            this.nudXMax.Size = new System.Drawing.Size(165, 31);
            this.nudXMax.TabIndex = 5;
            // 
            // lblY
            // 
            this.lblY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblY.Location = new System.Drawing.Point(4, 94);
            this.lblY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(72, 50);
            this.lblY.TabIndex = 6;
            this.lblY.Text = "Y:";
            this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudYMin
            // 
            this.nudYMin.DecimalPlaces = 1;
            this.nudYMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudYMin.Location = new System.Drawing.Point(84, 100);
            this.nudYMin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudYMin.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudYMin.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudYMin.Name = "nudYMin";
            this.nudYMin.Size = new System.Drawing.Size(165, 31);
            this.nudYMin.TabIndex = 7;
            // 
            // nudYMax
            // 
            this.nudYMax.DecimalPlaces = 1;
            this.nudYMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudYMax.Location = new System.Drawing.Point(257, 100);
            this.nudYMax.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudYMax.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudYMax.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudYMax.Name = "nudYMax";
            this.nudYMax.Size = new System.Drawing.Size(165, 31);
            this.nudYMax.TabIndex = 8;
            // 
            // lblZ
            // 
            this.lblZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblZ.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblZ.Location = new System.Drawing.Point(4, 144);
            this.lblZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(72, 50);
            this.lblZ.TabIndex = 9;
            this.lblZ.Text = "Z:";
            this.lblZ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudZMin
            // 
            this.nudZMin.DecimalPlaces = 1;
            this.nudZMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudZMin.Location = new System.Drawing.Point(84, 150);
            this.nudZMin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudZMin.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudZMin.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudZMin.Name = "nudZMin";
            this.nudZMin.Size = new System.Drawing.Size(165, 31);
            this.nudZMin.TabIndex = 10;
            // 
            // nudZMax
            // 
            this.nudZMax.DecimalPlaces = 1;
            this.nudZMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudZMax.Location = new System.Drawing.Point(257, 150);
            this.nudZMax.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudZMax.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudZMax.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudZMax.Name = "nudZMax";
            this.nudZMax.Size = new System.Drawing.Size(165, 31);
            this.nudZMax.TabIndex = 11;
            // 
            // lblU
            // 
            this.lblU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblU.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblU.Location = new System.Drawing.Point(4, 194);
            this.lblU.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblU.Name = "lblU";
            this.lblU.Size = new System.Drawing.Size(72, 50);
            this.lblU.TabIndex = 12;
            this.lblU.Text = "U:";
            this.lblU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudUMin
            // 
            this.nudUMin.DecimalPlaces = 1;
            this.nudUMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudUMin.Location = new System.Drawing.Point(84, 200);
            this.nudUMin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudUMin.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudUMin.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudUMin.Name = "nudUMin";
            this.nudUMin.Size = new System.Drawing.Size(165, 31);
            this.nudUMin.TabIndex = 13;
            // 
            // nudUMax
            // 
            this.nudUMax.DecimalPlaces = 1;
            this.nudUMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudUMax.Location = new System.Drawing.Point(257, 200);
            this.nudUMax.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.nudUMax.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudUMax.Minimum = new decimal(new int[] { 10000, 0, 0, -2147483648 });
            this.nudUMax.Name = "nudUMax";
            this.nudUMax.Size = new System.Drawing.Size(165, 31);
            this.nudUMax.TabIndex = 14;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(84, 250);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(165, 44);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(257, 250);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(165, 44);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AxisLimitForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(450, 324);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AxisLimitForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "轴限位设置";
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUMax)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
