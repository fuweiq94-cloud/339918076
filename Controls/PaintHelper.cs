using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 绘图工具类：提取各自定义控件中重复的绘图逻辑。
    /// </summary>
    internal static class PaintHelper
    {
        /// <summary>设置高质量绘图模式（抗锯齿 + ClearType）。</summary>
        public static void SetupGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        /// <summary>用背景色填充整个客户区。</summary>
        public static void FillBackground(Graphics g, Control control)
        {
            using (Brush bg = new SolidBrush(control.BackColor))
                g.FillRectangle(bg, control.ClientRectangle);
        }

        /// <summary>
        /// 根据范围选择合适的刻度步长，使刻度线间隔美观。
        /// </summary>
        /// <param name="span">数值范围（max - min）。</param>
        /// <param name="targetSegments">期望的大约分段数。</param>
        public static int ChooseStep(float span, int targetSegments)
        {
            double raw = span / (double)targetSegments;
            double[] candidates = { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000 };
            double best = 1;
            double bestDiff = double.MaxValue;
            foreach (double c in candidates)
            {
                double d = Math.Abs(c - raw);
                if (d < bestDiff) { bestDiff = d; best = c; }
            }
            return (int)best;
        }

        /// <summary>把数值映射到竖直条上的 Y 像素坐标（值越大越靠上）。</summary>
        public static float ValueToY(RectangleF bar, float rangeMin, float rangeMax, float value)
        {
            float span = rangeMax - rangeMin;
            if (span <= 0) span = 1;
            float ratio = (value - rangeMin) / span;
            return bar.Bottom - ratio * bar.Height;
        }

        /// <summary>计算竖直条区域（居中、指定宽度和上下边距）。</summary>
        public static RectangleF VerticalBarArea(RectangleF clientRect, float barWidth, float topMargin, float bottomMargin)
        {
            float cx = clientRect.Width / 2f;
            return new RectangleF(cx - barWidth / 2f, clientRect.Top + topMargin,
                                  barWidth, clientRect.Height - topMargin - bottomMargin);
        }

        /// <summary>绘制条形边框。</summary>
        public static void DrawBarBorder(Graphics g, RectangleF bar, Color color)
        {
            using (Pen bp = new Pen(color))
                g.DrawRectangle(bp, bar.X, bar.Y, bar.Width, bar.Height);
        }

        /// <summary>绘制居中文本。</summary>
        public static void DrawCenteredText(Graphics g, string text, Font font, Brush brush, float x, float y)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString(text, font, brush, x, y, sf);
        }

        /// <summary>绘制居中居中文本（在矩形区域内水平垂直都居中）。</summary>
        public static void DrawCenteredTextInRect(Graphics g, string text, Font font, Brush brush, RectangleF rect)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(text, font, brush, rect, sf);
        }

        /// <summary>绘制竖直条右侧刻度线和数值标签。</summary>
        public static void DrawVerticalTicks(Graphics g, RectangleF bar,
            float rangeMin, float rangeMax, Func<float, float> valueToY)
        {
            int step = ChooseStep(rangeMax - rangeMin, 6);
            using (Pen tp = new Pen(Color.FromArgb(160, 170, 185)))
            using (Brush tb = new SolidBrush(Color.FromArgb(120, 130, 145)))
            using (Font f = new Font("Segoe UI", 7.5F))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                for (float v = rangeMin; v <= rangeMax + 0.01f; v += step)
                {
                    float y = valueToY(v);
                    g.DrawLine(tp, bar.Right + 1, y, bar.Right + 5, y);
                    g.DrawString(v.ToString("0"), f, tb, bar.Right + 8, y, sf);
                }
            }
        }
    }
}
