using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 轴软限位距离显示控件。
    /// 实时显示各轴距离上下限位的剩余距离值。
    /// </summary>
    [ToolboxBitmap(typeof(ZBarView), "Resources.ZBarView.bmp")]
    [DefaultProperty("RemainingDistances")]
    [Description("轴软限位距离显示控件：显示 X/Y/Z/U 四轴距软限位的剩余距离（正值=距上限位，负值=距下限位）")]
    public class LimitDistanceView : Control
    {
        // —— 配置属性 ——
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("是否启用自动滚动/闪烁提示警告状态。")]
        public bool AutoWarn { get; set; }

        [Category("Data")]
        [Browsable(false)]
        public float[] RemainingDistances { get; private set; }

        // —— 内部数据绑定 ——
        private readonly string[] _axisNames = { "X", "Y", "Z", "U" };
        private Color[] _warningColors = new Color[4];
        private const int ROW_HEIGHT = 32;
        private const int COLUMN_GAP = 25;
        private const int AXIS_NAME_WIDTH = 35;
        private const int VALUE_WIDTH = 60;
        private const int RULER_WIDTH = 80;

        public LimitDistanceView()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(245, 247, 250);
            SetStyle(ControlStyles.AllPaintingInNonErased | 
                     ControlStyles.OptimizedDoubleBuffer | 
                     ControlStyles.ResizeRedraw, true);
            
            Size = new Size(320, 140);  // 4 行 × 32 + Padding
            AutoWarn = true;
            
            // 初始化警告颜色（每个轴不同）
            _warningColors = new Color[]
            {
                Color.FromArgb(200, 200, 50),   // X - Yellowish
                Color.FromArgb(50, 200, 120),   // Y - Greenish
                Color.FromArgb(200, 50, 100),   // Z - Reddish
                Color.FromArgb(50, 150, 200)    // U - Cyan-ish
            };
            
            // 初始化为空距离
            RemainingDistances = new float[4] { 0f, 0f, 0f, 0f };
        }

        /// <summary>
        /// 更新各轴的剩余距离值。
        /// </summary>
        public void UpdateDistances(float xRemaining, float yRemaining, float zRemaining, float uRemaining)
        {
            RemainingDistances = new float[] { xRemaining, yRemaining, zRemaining, uRemaining };
            Invalidate();  // 触发重绘
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);

            int left = 10;
            int top = 10;
            int right = ClientRectangle.Width - 10;
            int bottom = ClientRectangle.Height - 10;

            Brush textBrush = Brushes.Black;
            Font smallFont = new Font("Consolas", 9F);
            Font axisLabelFont = new Font("微软雅黑", 10F, FontStyle.Bold);
            Brush[] axisBrushes = new Brush[4];
            axisBrushes[0] = new SolidBrush(_warningColors[0]);
            axisBrushes[1] = new SolidBrush(_warningColors[1]);
            axisBrushes[2] = new SolidBrush(_warningColors[2]);
            axisBrushes[3] = new SolidBrush(_warningColors[3]);

            try
            {
                // 绘制标题 - 居左显示
                using (Brush titleBrush = new SolidBrush(Color.FromArgb(80, 90, 100)))
                using (Font titleFont = new Font("微软雅黑", 12F, FontStyle.Bold))
                {
                    string title = "软限位距离";
                    SizeF titleSize = g.MeasureString(title, titleFont);
                    g.DrawString(title, titleFont, titleBrush, 15, 5);  // 标题移到左上角，更大更明显
                }

                // 绘制表头
                string[] headers = { "轴", "余量", "刻度尺" };
                Point headerPositions = new Point(left + AXIS_NAME_WIDTH / 2, top);
                for (int i = 0; i < headers.Length; i++)
                {
                    Rectangle headerRect = new Rectangle(
                        left + i * (VALUE_WIDTH + COLUMN_GAP) + (i == 2 ? -RULER_WIDTH / 2 : 0),
                        top - 5,
                        i == 2 ? RULER_WIDTH : VALUE_WIDTH + (i == 0 ? AXIS_NAME_WIDTH : 0),
                        HEADER_HEIGHT);
                    
                    using (Brush headerBrush = Brushes.Gray)
                    using (Font headerFont = smallFont)
                    {
                        g.DrawString(headers[i], headerFont, headerBrush,
                            headerRect.X + headerRect.Width / 2 -
                            g.MeasureString(headers[i], headerFont).Width / 2,
                            headerRect.Y + headerRect.Height / 2 -
                            g.MeasureString(headers[i], headerFont).Height / 2);
                    }
                }

                // 绘制每一行的轴距离数据
                if (RemainingDistances != null && RemainingDistances.Length >= 4)
                {
                    for (int row = 0; row < 4; row++)
                    {
                        float remaining = RemainingDistances[row];
                        string axisName = _axisNames[row];
                        string valueText = $"{remaining:F2}";
                        
                        int rowTop = top + row * ROW_HEIGHT;
                        int rowHeight = Math.Min(ROW_HEIGHT, ClientRectangle.Height - rowTop - 10);

                        // 绘制背景（带颜色的半透明层）
                        using (Brush bgBrush = new SolidBrush(Color.FromArgb(30, _warningColors[row])))
                        {
                            Rectangle rect = new Rectangle(left - 5, rowTop - 2, right - left + 10, rowHeight + 4);
                            g.FillRectangle(bgBrush, rect);
                        }

                        // 绘制轴标签
                        using (Brush axisBrush = axisBrushes[row])
                        using (Font axisFont = axisLabelFont)
                        {
                            Rectangle nameRect = new Rectangle(left + 5, rowTop + 2, AXIS_NAME_WIDTH, rowHeight);
                            g.DrawString(axisName, axisFont, axisBrush,
                                nameRect.X + nameRect.Width / 2 -
                                g.MeasureString(axisName, axisFont).Width / 2,
                                nameRect.Y + nameRect.Height / 2 -
                                g.MeasureString(axisName, axisFont).Height / 2);
                        }

                        // 绘制数值
                        using (Brush valueBrush = textBrush)
                        using (Font valueFont = smallFont)
                        {
                            Rectangle valueRect = new Rectangle(
                                left + AXIS_NAME_WIDTH + COLUMN_GAP,
                                rowTop + 2,
                                VALUE_WIDTH,
                                rowHeight);
                            
                            // 根据正负值设置颜色
                            Color valColor = remaining >= 0 ? Color.Navy : Color.DarkRed;
                            using (Brush vBrush = new SolidBrush(valColor))
                            {
                                g.DrawString(valueText, valueFont, vBrush,
                                    valueRect.X + valueRect.Width / 2 -
                                    g.MeasureString(valueText, valueFont).Width / 2,
                                    valueRect.Y + valueRect.Height / 2 -
                                    g.MeasureString(valueText, valueFont).Height / 2);
                            }
                        }

                        // 绘制刻度尺示意图
                        DrawRulerScale(g, left + AXIS_NAME_WIDTH + VALUE_WIDTH + COLUMN_GAP, rowTop + rowHeight / 2,
                            remaining, _warningColors[row]);
                    }
                }
            }
            finally
            {
                foreach (var brush in axisBrushes) brush?.Dispose();
                smallFont.Dispose();
                axisLabelFont.Dispose();
            }
        }

        /// <summary>
        /// 绘制刻度尺示意图（显示当前位置在轴的范围中间位置）
        /// </summary>
        private void DrawRulerScale(Graphics g, int startX, int startY, float remainingValue, Color highlightColor)
        {
            // 简化版本：绘制一个水平线表示相对位置
            using (Pen scalePen = new Pen(Color.LightGray, 1.5f))
            using (Pen highlightPen = new Pen(highlightColor, 2f))
            {
                // 基础刻度线
                for (int i = -5; i <= 5; i++)
                {
                    int xPos = startX + i * 8;
                    int tickHeight = (Math.Abs(i) < 3) ? 10 : 5;
                    g.DrawLine(scalePen, xPos, startY - tickHeight / 2, xPos, startY + tickHeight / 2);
                }

                // 高亮当前剩余距离指示器
                int indicatorPos = startX + (int)(remainingValue % 10);
                g.DrawLine(highlightPen, indicatorPos, startY - 12, indicatorPos, startY + 12);
            }
        }

        private const int HEADER_HEIGHT = 20;
    }
}
