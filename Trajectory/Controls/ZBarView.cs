using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 竖直进度条样式的 Z 轴指示控件。
    /// 显示：刻度、当前 Z 值、目标 Z 值标记。
    /// 也可用于 U 轴等其他单轴指示（设置 TargetZ 即可）。
    /// </summary>
    [ToolboxBitmap(typeof(ZBarView), "Resources.ZBarView.bmp")]
    [DefaultProperty("TargetZ")]
    [Description("竖直条形轴指示控件：显示刻度、当前值、目标值标记。常用于 Z 轴或 U 轴。")]
    public class ZBarView : Control
    {
        [Category("Behavior")]
        [DefaultValue(-50f)]
        [Description("轴范围最小值（mm 或 °）。")]
        public float RangeMin { get; set; }

        [Category("Behavior")]
        [DefaultValue(100f)]
        [Description("轴范围最大值（mm 或 °）。")]
        public float RangeMax { get; set; }

        [Browsable(false)]
        public float CurrentZ { get; private set; }

        [Category("Data")]
        [DefaultValue(0f)]
        [Description("目标位置（mm 或 °）。")]
        public float TargetZ { get; set; }

        public ZBarView()
        {
            RangeMin = -50f;
            RangeMax = 100f;
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.FromArgb(245, 247, 250);
            SetStyle(ControlStyles.Selectable, true);
        }

        public void Advance(float lerp)
        {
            float k = MathHelper.ClampLerp(lerp);
            CurrentZ += (TargetZ - CurrentZ) * k;
            Invalidate();
        }

        /// <summary>
        /// 由后端位置反馈直接更新实际位置（严禁使用模拟数据）。
        /// </summary>
        public void UpdateActual(float z)
        {
            CurrentZ = z;
            Invalidate();
        }

        private RectangleF BarArea
        {
            get { return PaintHelper.VerticalBarArea(ClientRectangle, 28f, 18f, 26f); }
        }

        private float ZToScreen(float z)
        {
            return PaintHelper.ValueToY(BarArea, RangeMin, RangeMax, z);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);
            PaintHelper.FillBackground(g, this);

            RectangleF bar = BarArea;

            // 背景
            using (Brush bb = new SolidBrush(Color.FromArgb(225, 230, 238)))
                g.FillRectangle(bb, bar);

            // 中线 (Z=0)
            float zeroY = ZToScreen(0);
            if (zeroY > bar.Top && zeroY < bar.Bottom)
            {
                using (Pen p = new Pen(Color.FromArgb(180, 140, 140, 140), 1f))
                { p.DashStyle = DashStyle.Dash; g.DrawLine(p, bar.Left - 4, zeroY, bar.Right + 4, zeroY); }
            }

            // 填充：从 min 到 current
            float curY = ZToScreen(CurrentZ);
            float minY = ZToScreen(RangeMin);
            RectangleF fillRect = new RectangleF(bar.X,
                Math.Min(curY, minY), bar.Width, Math.Abs(curY - minY));
            using (LinearGradientBrush lb =
                new LinearGradientBrush(fillRect,
                    Color.FromArgb(120, 200, 160), Color.FromArgb(60, 160, 220),
                    LinearGradientMode.Vertical))
            {
                g.FillRectangle(lb, fillRect);
            }

            // 边框
            PaintHelper.DrawBarBorder(g, bar, Color.FromArgb(150, 160, 175));

            // 刻度
            PaintHelper.DrawVerticalTicks(g, bar, RangeMin, RangeMax,
                delegate(float v) { return ZToScreen(v); });

            // 目标 Z 标记（箭头）
            float tY = ZToScreen(TargetZ);
            using (Pen tp = new Pen(Color.FromArgb(220, 140, 40), 1.6f))
            {
                g.DrawLine(tp, bar.Left - 10, tY, bar.Left - 2, tY);
                g.DrawLine(tp, bar.Left - 2, tY, bar.Left - 6, tY - 4);
                g.DrawLine(tp, bar.Left - 2, tY, bar.Left - 6, tY + 4);
            }

            // 当前值指示条
            using (Pen cp = new Pen(Color.FromArgb(40, 80, 180), 2f))
                g.DrawLine(cp, bar.Left - 2, curY, bar.Right + 2, curY);

            // 顶部标题
            using (Font tf = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (Brush tbh = new SolidBrush(Color.FromArgb(50, 60, 80)))
            {
                PaintHelper.DrawCenteredText(g, "Z", tf, tbh, ClientRectangle.Width / 2f, 2f);
            }

            // 底部数值
            using (Font vf = new Font("Consolas", 9F, FontStyle.Bold))
            using (Brush vb = new SolidBrush(Color.FromArgb(40, 80, 180)))
            {
                string s = string.Format("{0:F2}", CurrentZ);
                PaintHelper.DrawCenteredText(g, s, vf, vb, ClientRectangle.Width / 2f,
                    ClientRectangle.Bottom - 16f);
            }
        }
    }
}
