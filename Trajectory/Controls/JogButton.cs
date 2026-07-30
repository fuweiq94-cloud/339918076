using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 点动按钮（Jog Button）。
    /// 按住不放：每隔一个间隔重复触发 Jog 事件（机床上的 JOG 模式）。
    /// 松开：立即触发 Stop 事件。
    /// 常用场景：步进电机连续进给、机械臂点动控制。
    /// </summary>
    [ToolboxBitmap(typeof(JogButton), "Resources.JogButton.bmp")]
    [DefaultEvent("Jog")]
    [DefaultProperty("Direction")]
    [Description("点动按钮：按住时按间隔重复触发 Jog 事件，松开时触发 Stop 事件。常用于机床 JOG 模式。")]
    public class JogButton : Control
    {
        /// <summary>每按一次持续触发时的方向标识（+1 / -1 或任意对象）。</summary>
        [Category("Behavior")]
        [DefaultValue(1)]
        [Description("持续触发时传递给 Jog/Stop 事件的方向标识（通常 +1 或 -1）。")]
        public int Direction { get; set; }

        /// <summary>首次按下后多久开始连续触发（毫秒）。</summary>
        [Category("Behavior")]
        [DefaultValue(400)]
        [Description("首次按下后多久开始连续触发（毫秒）。")]
        public int InitialDelay { get; set; }

        /// <summary>连续触发的间隔（毫秒），越小移动越快。</summary>
        [Category("Behavior")]
        [DefaultValue(80)]
        [Description("连续触发的间隔（毫秒），越小移动越快。")]
        public int RepeatInterval { get; set; }

        private bool _pressed;
        private bool _hover;
        private readonly Timer _timer;
        private DateTime _pressTime;

        /// <summary>每次点动触发（首次按下会立即触发一次，之后按间隔重复）。</summary>
        [Category("Action")]
        [Description("按下/连续触发时引发，参数为 Direction。")]
        public event EventHandler<JogEventArgs> Jog;

        /// <summary>松开按钮时触发。</summary>
        [Category("Action")]
        [Description("松开按钮时引发，参数为 Direction。")]
        public event EventHandler<JogEventArgs> Stop;

        public JogButton()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            Direction = +1;
            InitialDelay = 400;
            RepeatInterval = 80;
            Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            ForeColor = Color.FromArgb(50, 60, 80);
            BackColor = Color.FromArgb(240, 243, 248);
            SetStyle(ControlStyles.Selectable, true);

            _timer = new Timer();
            _timer.Interval = RepeatInterval;
            // 工控机旧编译器不支持 Lambda，用命名方法 + 委托构造。
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnJog();
        }

        // —— 鼠标交互 ——
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BeginPress();
                Focus();
            }
            base.OnMouseDown(e);
        }
        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            EndPress();
            base.OnMouseUp(e);
        }
        
        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            // 鼠标离开时如果还按着，当作松开处理（避免“卡住一直触发”）
            EndPress();
            Invalidate();
            base.OnMouseLeave(e);
        }
        
        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }
        
        // —— 键盘也能用：空格/回车按住等效鼠标按住 ——
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && !_pressed)
            {
                BeginPress();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                EndPress();
                e.Handled = true;
            }
            base.OnKeyUp(e);
        }
        
        /// <summary>按下：启动定时器并立即触发一次 Jog。</summary>
        private void BeginPress()
        {
            _pressed = true;
            _pressTime = DateTime.Now;
            _timer.Interval = InitialDelay;
            _timer.Start();
            OnJog();
            Invalidate();
        }
        
        /// <summary>松开：停止定时器并触发 Stop。</summary>
        private void EndPress()
        {
            if (!_pressed) return;
            _pressed = false;
            _timer.Stop();
            OnStop();
            Invalidate();
        }

        protected virtual void OnJog()
        {
            EventHandler<JogEventArgs> h = Jog;
            if (h != null) h(this, new JogEventArgs(Direction));
        }

        protected virtual void OnStop()
        {
            EventHandler<JogEventArgs> h = Stop;
            if (h != null) h(this, new JogEventArgs(Direction));
        }

        // —— 绘制 ——
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);

            // 圆角矩形区域
            RectangleF rect = new RectangleF(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = RoundedRect(rect, 8))
            {
                // 背景渐变
                Color top, bot;
                if (_pressed)
                {
                    top = Color.FromArgb(60, 120, 220);
                    bot = Color.FromArgb(40, 90, 200);
                }
                else if (_hover)
                {
                    top = Color.FromArgb(230, 238, 250);
                    bot = Color.FromArgb(205, 218, 235);
                }
                else
                {
                    top = Color.FromArgb(245, 248, 252);
                    bot = Color.FromArgb(220, 228, 240);
                }
                using (LinearGradientBrush lb = new LinearGradientBrush(rect, top, bot, 90F))
                    g.FillPath(lb, path);

                // 描边
                using (Pen p = new Pen(_pressed ? Color.FromArgb(30, 70, 170) : Color.FromArgb(170, 185, 205), 1.2f))
                    g.DrawPath(p, path);
            }

            // 文字
            using (Brush tb = new SolidBrush(_pressed ? Color.White : ForeColor))
            {
                string text = string.IsNullOrEmpty(Text) ? (Direction >= 0 ? "►" : "◄") : Text;
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(text, Font, tb, rect, sf);
            }

            // 按下时的指示灯（左上角小红点）
            if (_pressed)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(220, 80, 80)))
                    g.FillEllipse(b, 6, 6, 6, 6);
            }
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2;
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// JogButton 点动/停止事件参数（VS2017 设计器兼容：标准 EventArgs，替代泛型 EventHandler&lt;int&gt;）。
    /// </summary>
    public class JogEventArgs : EventArgs
    {
        /// <summary>点动方向（+1 / -1）。</summary>
        public int Direction { get; private set; }

        public JogEventArgs(int direction)
        {
            Direction = direction;
        }
    }
}
