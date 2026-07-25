using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// DRO（Digital Read Out）数字读数控件。
    /// 机床/CNC 设备上常见的"大字号坐标显示"风格：
    /// - 轴名（如 X / Y / Z）
    /// - 高对比等宽大数字
    /// - 数值变化时短暂高亮（模拟真实 DRO 跳数效果）
    /// - 可设置报警阈值，超过变红
    /// </summary>
    [ToolboxBitmap(typeof(DroLabel), "Resources.DroLabel.bmp")]
    [DefaultProperty("Value")]
    [Description("DRO 数字读数控件：机床/CNC 风格的大字号坐标显示，数值变化时高亮，支持报警阈值。")]
    public class DroLabel : Control
    {
        /// <summary>显示的轴名（如 X / Y / Z）。</summary>
        [Category("Appearance")]
        [DefaultValue("X")]
        [Description("显示的轴名（如 X / Y / Z / U）。")]
        public string AxisName { get; set; }

        /// <summary>当前数值。</summary>
        [Category("Data")]
        [DefaultValue(0f)]
        [Description("当前显示的数值。变化 >0.0001 时会触发高亮并重绘。")]
        public float Value
        {
            get { return _value; }
            set
            {
                if (Math.Abs(value - _value) > 0.0001f)
                {
                    _value = value;
                    _flashTime = DateTime.Now;   // 记录变化时间，用于高亮
                    Invalidate();
                }
            }
        }

        /// <summary>小数位数。</summary>
        [Category("Data")]
        [DefaultValue(3)]
        [Description("显示的小数位数。")]
        public int Decimals { get; set; }

        /// <summary>单位字符串（如 mm / inch）。</summary>
        [Category("Appearance")]
        [DefaultValue("mm")]
        [Description("单位字符串（如 mm / inch / °）。")]
        public string Unit { get; set; }

        /// <summary>报警上限，超过则数字变红。设为 null 不检查。</summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("报警上限。超过时数字变红；设为 null 不检查。")]
        public float? AlarmHigh { get; set; }

        /// <summary>报警下限，低于则数字变红。</summary>
        [Category("Behavior")]
        [DefaultValue(null)]
        [Description("报警下限。低于时数字变红；设为 null 不检查。")]
        public float? AlarmLow { get; set; }

        /// <summary>数值变化后高亮持续多久（毫秒）。</summary>
        [Category("Behavior")]
        [DefaultValue(200)]
        [Description("数值变化后高亮持续多久（毫秒）。")]
        public int FlashDuration { get; set; }

        private float _value;
        private DateTime _flashTime;
        private readonly Timer _flashTimer;

        public DroLabel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            AxisName = "X";
            Value = 0;
            Decimals = 3;
            Unit = "mm";
            FlashDuration = 200;
            BackColor = Color.FromArgb(20, 28, 40);
            ForeColor = Color.FromArgb(120, 255, 180);  // DRO 经典的绿色
            Font = new Font("Consolas", 22F, FontStyle.Bold);
            SetStyle(ControlStyles.Selectable, false);

            _flashTimer = new Timer();
            _flashTimer.Interval = 50;
            // 工控机旧编译器不支持 Lambda，用命名方法 + 委托构造。
            _flashTimer.Tick += new EventHandler(FlashTimer_Tick);
        }

        private void FlashTimer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - _flashTime).TotalMilliseconds > FlashDuration)
            {
                _flashTimer.Stop();
                Invalidate();
            }
        }

        public void SetValue(float v)
        {
            Value = v;   // 走属性，触发高亮
            if (!_flashTimer.Enabled) _flashTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);

            // 背景（深色面板）
            PaintHelper.FillBackground(g, this);

            // 内嵌边框（凹陷效果）
            using (Pen p1 = new Pen(Color.FromArgb(50, 60, 75)))
            using (Pen p2 = new Pen(Color.FromArgb(8, 12, 20)))
            {
                g.DrawLine(p1, 0, 0, Width - 1, 0);
                g.DrawLine(p1, 0, 0, 0, Height - 1);
                g.DrawLine(p2, Width - 1, 0, Width - 1, Height - 1);
                g.DrawLine(p2, 0, Height - 1, Width - 1, Height - 1);
            }

            // 轴名（左侧）
            RectangleF nameRect = new RectangleF(6, 0, 28, Height);
            using (Font nf = new Font("Segoe UI", 12F, FontStyle.Bold))
            using (Brush nb = new SolidBrush(Color.FromArgb(200, 210, 230)))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(AxisName ?? "?", nf, nb, nameRect, sf);
            }

            // 主数值（中间大字）
            string numText = _value.ToString("F" + Decimals);
            // 处理负号：让数字保持对齐（前面补空格）
            if (!_value.ToString("F" + Decimals).StartsWith("-"))
                numText = " " + numText;

            bool isFlash = (DateTime.Now - _flashTime).TotalMilliseconds < FlashDuration;
            bool isAlarm = (AlarmHigh.HasValue && _value > AlarmHigh.Value)
                        || (AlarmLow.HasValue && _value < AlarmLow.Value);

            Color numColor;
            if (isAlarm)        numColor = Color.FromArgb(255, 90, 90);
            else if (isFlash)   numColor = Color.FromArgb(255, 255, 100);   // 变化时黄色闪一下
            else                numColor = ForeColor;

            RectangleF numRect = new RectangleF(34, 0, Width - 70, Height);
            using (Brush nb = new SolidBrush(numColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(numText, Font, nb, numRect, sf);
            }

            // 单位（右侧）
            using (Font uf = new Font("Segoe UI", 8.5F))
            using (Brush ub = new SolidBrush(Color.FromArgb(150, 160, 180)))
            {
                RectangleF unitRect = new RectangleF(Width - 38, 0, 32, Height);
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(Unit ?? "", uf, ub, unitRect, sf);
            }
        }
    }
}
