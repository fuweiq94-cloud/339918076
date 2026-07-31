using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 圆形表盘样式的 U 轴角度指示控件。
    /// 显示：刻度线、当前角度指针、目标角度标记、范围区域。
    /// </summary>
    [ToolboxBitmap(typeof(ZBarView), "Resources.ZBarView.bmp")]
    [DefaultProperty("TargetAngle")]
    [Description("圆形表盘轴指示控件：显示刻度、当前角度、目标角度和范围区域。专用于 U 轴等旋转轴。")]
    public class URotationView : Control
    {
        // —— 范围配置（与 GlobalSetting.UMin/UMax一致）——
        [Category("Behavior")]
        [DefaultValue(-0.87f)]
        [Description("角度范围最小值（弧度，默认 -50° ≈ -0.87rad）。")]
        public float RangeMin { get; set; }

        [Category("Behavior")]
        [DefaultValue(1.74f)]
        [Description("角度范围最大值（弧度，默认 100° ≈ 1.74rad）。")]
        public float RangeMax { get; set; }

        // —— 位置数据（只读，由外部更新）——
        [Browsable(false)]
        public float CurrentAngle { get; private set; }

        [Category("Data")]
        [DefaultValue(0f)]
        [Description("目标角度（弧度）。")]
        public float TargetAngle { get; set; }

        // —— 样式配置 ——
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("是否显示刻度线。")]
        public bool ShowTicks { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("是否显示范围区域扇形。")]
        public bool ShowRangeArea { get; set; }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "245, 247, 250")]
        [Description("背景颜色。")]
        public new Color BackColor { get; set; }

        public URotationView()
        {
            RangeMin = -0.87f;  // -50°
            RangeMax = 1.74f;   // 100°
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.FromArgb(245, 247, 250);
            SetStyle(ControlStyles.Selectable, true);
            Width = 300;
            Height = 300;
        }

        /// <summary>
        /// 由后端位置反馈直接更新实际角度（严禁使用模拟数据）。
        /// </summary>
        public void UpdateActual(float rad)
        {
            CurrentAngle = rad;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);
            PaintHelper.FillBackground(g, this);

            int centerX = ClientRectangle.Width / 2;
            int centerY = ClientRectangle.Height / 2;
            int radius = Math.Min(centerX, centerY) - 20;  // 留出边距

            if (radius <= 0) return;

            Brush textBrush = Brushes.Black;
            Pen outerPen = new Pen(Color.DarkGray, 2);
            Pen innerPen = new Pen(Color.FromArgb(100, 150, 200), 1.5f);
            Brush rangeBrush = new SolidBrush(Color.FromArgb(100, 0, 200, 0));
            
            try
            {
                // 1️⃣ 绘制背景圆盘
                using (GraphicsPath bgPath = new GraphicsPath())
                {
                    bgPath.AddEllipse(centerX - radius, centerY - radius, radius * 2, radius * 2);
                    using (Brush b = new SolidBrush(Color.White))
                    using (Pen bp = new Pen(Color.FromArgb(180, 190, 200), 2))
                    {
                        g.FillPath(b, bgPath);
                        g.DrawPath(bp, bgPath);
                    }
                }

                // 2️⃣ 绘制范围区域扇形
                if (ShowRangeArea && RangeMax > RangeMin)
                {
                    using (GraphicsPath arcPath = new GraphicsPath())
                    {
                        arcPath.AddLine(centerX, centerY,
                            centerX + (int)(radius * Math.Cos(RangeMin)),
                            centerY - (int)(radius * Math.Sin(RangeMin)));
                        
                        arcPath.AddArc(centerX - radius, centerY - radius, radius * 2, radius * 2,
                            (float)(RangeMin * 180 / Math.PI),
                            (float)((RangeMax - RangeMin) * 180 / Math.PI));
                        
                        arcPath.AddLine(centerX, centerY,
                            centerX + (int)(radius * Math.Cos(RangeMax)),
                            centerY - (int)(radius * Math.Sin(RangeMax)));
                        arcPath.CloseFigure();

                        g.FillPath(rangeBrush, arcPath);
                        using (Pen arcPen = new Pen(Color.Green, 1))
                        {
                            g.DrawPath(arcPen, arcPath);
                        }
                    }
                }

                // 3️⃣ 绘制主要刻度线（四个象限）
                Font axisLabelFont = new Font("微软雅黑", 12F, FontStyle.Bold);
                Font smallFont = new Font("Consolas", 9F);
                
                for (int i = 0; i < 4; i++)
                {
                    float angle = i * Math.PI / 2;
                    int tickLen = radius + 10;
                    int x1 = centerX + (int)(tickLen * Math.Cos(angle));
                    int y1 = centerY - (int)(tickLen * Math.Sin(angle));
                    int x2 = centerX + (int)((radius + 25) * Math.Cos(angle));
                    int y2 = centerY - (int)((radius + 25) * Math.Sin(angle));
                    
                    // 刻度线
                    using (Pen tp = new Pen(Color.Blue, 2))
                        g.DrawLine(tp, x1, y1, x2, y2);
                    
                    // 标注文字（0°, 90°, 180°, 270°）
                    double deg = angle * 180 / Math.PI;
                    string label = deg.ToString() + "°";
                    SizeF textSize = g.MeasureString(label, axisLabelFont);
                    g.DrawString(label, axisLabelFont, textBrush,
                        x2 - textSize.Width / 2,
                        y2 - textSize.Height / 2);
                }

                // 4️⃣ 绘制指针（当前角度）
                float currentRad = CurrentAngle;
                float pointerLength = radius - 25;
                float pointerX = centerX + pointerLength * Math.Cos(currentRad);
                float pointerY = centerY - pointerLength * Math.Sin(currentRad);

                using (Pen pointerPen = new Pen(Color.Red, 3))
                using (Brush arrowBrush = new SolidBrush(Color.Red))
                {
                    // 指针线
                    g.DrawLine(pointerPen, centerX, centerY, pointerX, pointerY);

                    // 箭头三角形
                    float arrowAngle = currentRad + Math.PI / 2;
                    float arrowLen = 12;
                    PointF arrowHead1 = new PointF(
                        pointerX + arrowLen * Math.Cos(arrowAngle),
                        pointerY - arrowLen * Math.Sin(arrowAngle));
                    float arrowAngle2 = currentRad - Math.PI / 2;
                    PointF arrowHead2 = new PointF(
                        pointerX + arrowLen * Math.Cos(arrowAngle2),
                        pointerY - arrowLen * Math.Sin(arrowAngle2));

                    using (GraphicsPath arrowPath = new GraphicsPath())
                    {
                        arrowPath.AddLine(centerX, centerY, pointerX, pointerY);
                        arrowPath.AddLine(pointerX, pointerY, arrowHead1.X, arrowHead1.Y);
                        arrowPath.AddLine(arrowHead1.X, arrowHead1.Y, arrowHead2.X, arrowHead2.Y);
                        arrowPath.CloseFigure();

                        g.FillPath(arrowBrush, arrowPath);
                    }
                }

                // 5️⃣ 绘制目标角度标记（虚线圆圈）
                float targetRad = TargetAngle;
                float targetX = centerX + (radius - 10) * Math.Cos(targetRad);
                float targetY = centerY - (radius - 10) * Math.Sin(targetRad);
                
                using (Pen tp = new Pen(Color.Orange, 2))
                {
                    tp.DashStyle = DashStyle.Dash;
                    g.DrawEllipse(tp, targetX - 8, targetY - 8, 16, 16);
                }

                // 6️⃣ 中心点
                using (Brush cp = new SolidBrush(Color.Blue))
                {
                    g.FillEllipse(cp, centerX - 5, centerY - 5, 10, 10);
                }

                // 7️⃣ 底部数值显示
                double currentDeg = CurrentAngle * 180 / Math.PI;
                double targetDeg = TargetAngle * 180 / Math.PI;
                
                string currentText = $"当前：{currentDeg:F2}°";
                string targetText = $"目标：{targetDeg:F2}°";
                
                SizeF curSize = g.MeasureString(currentText, smallFont);
                SizeF tgtSize = g.MeasureString(targetText, smallFont);
                
                g.DrawString(currentText, smallFont, Brushes.Black,
                    centerX - curSize.Width / 2, centerY + radius + 10);
                g.DrawString(targetText, smallFont, Brushes.DarkOrange,
                    centerX - tgtSize.Width / 2, centerY + radius + 30);
            }
            finally
            {
                outerPen.Dispose();
                innerPen.Dispose();
                rangeBrush.Dispose();
            }
        }

        /// <summary>根据弧度转换为度数的辅助方法</summary>
        private static float RadToDeg(float rad)
        {
            return rad * 180 / Math.PI;
        }

        /// <summary>根据度数转换为弧度的辅助方法</summary>
        private static float DegToRad(float deg)
        {
            return deg * Math.PI / 180;
        }
    }
}
