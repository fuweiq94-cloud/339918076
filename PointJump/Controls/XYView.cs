using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// XY 平面俯视图控件。
    /// 显示：坐标网格、原点、十字光标、目标点、运动轨迹、鼠标交互拾取。
    /// </summary>
    [ToolboxBitmap(typeof(XYView), "Resources.XYView.bmp")]
    [DefaultEvent("TargetSetByMouse")]
    [DefaultProperty("TargetX")]
    [Description("XY 平面俯视图：显示坐标网格、目标点、运动轨迹，支持鼠标点击拾取目标坐标。")]
    public class XYView : Control
    {
        // 坐标范围（机械坐标系，单位默认 mm）—— X/Y 轴可独立设置
        [Category("Behavior")]
        [DefaultValue(-100f)]
        [Description("X 轴左限（mm）。")]
        public float XMin { get; set; }

        [Category("Behavior")]
        [DefaultValue(100f)]
        [Description("X 轴右限（mm）。")]
        public float XMax { get; set; }

        [Category("Behavior")]
        [DefaultValue(-100f)]
        [Description("Y 轴下限（mm）。")]
        public float YMin { get; set; }

        [Category("Behavior")]
        [DefaultValue(100f)]
        [Description("Y 轴上限（mm）。")]
        public float YMax { get; set; }

        /// <summary>统一设置 X/Y 范围（向后兼容）。</summary>
        [Browsable(false)]
        public float RangeMin
        {
            get { return XMin; }
            set { XMin = value; YMin = value; }
        }

        /// <summary>统一设置 X/Y 范围（向后兼容）。</summary>
        [Browsable(false)]
        public float RangeMax
        {
            get { return XMax; }
            set { XMax = value; YMax = value; }
        }

        // 当前显示的坐标（被动画逐渐逼近 TargetX/TargetY）
        [Browsable(false)]
        public float CurrentX { get; private set; }

        [Browsable(false)]
        public float CurrentY { get; private set; }

        // 目标坐标（滑块/键盘设定）
        [Category("Data")]
        [DefaultValue(0f)]
        [Description("目标 X 坐标（mm）。")]
        public float TargetX { get; set; }

        [Category("Data")]
        [DefaultValue(0f)]
        [Description("目标 Y 坐标（mm）。")]
        public float TargetY { get; set; }

        // 运动轨迹
        private readonly List<PointF> _trail = new List<PointF>();
        private const int MaxTrail = 400;

        // 预设点位标记
        private readonly List<PresetPoint> _presetMarkers = new List<PresetPoint>();

        // 鼠标拖拽相关
        private bool _dragging;
        private bool _hover;

        // 当用户通过鼠标点击/拖拽设定新目标时触发
        [Category("Action")]
        [Description("用户通过鼠标点击/拖拽设定新目标时触发。")]
        public event EventHandler<TargetSetEventArgs> TargetSetByMouse;

        public XYView()
        {
            XMin = -100f;
            XMax = 100f;
            YMin = -100f;
            YMax = 100f;
            DoubleBuffered = true;          // 关键：避免重绘闪烁
            ResizeRedraw = true;
            BackColor = Color.FromArgb(245, 247, 250);
            ForeColor = Color.FromArgb(60, 70, 90);
            Font = new Font("Segoe UI", 8.25F);
            // 让控件可接收键盘焦点
            SetStyle(ControlStyles.Selectable, true);
        }

        // —— 把机械坐标转换为像素坐标 ——
        private PointF ToScreen(float mx, float my)
        {
            RectangleF area = DrawableArea;
            float spanX = XMax - XMin;
            float spanY = YMax - YMin;
            if (spanX <= 0) spanX = 1;
            if (spanY <= 0) spanY = 1;

            float sx = area.Left + (mx - XMin) / spanX * area.Width;
            // Y 轴要翻转（屏幕坐标向下为正，数学坐标向上为正）
            float sy = area.Bottom - (my - YMin) / spanY * area.Height;
            return new PointF(sx, sy);
        }

        // 把像素坐标转成机械坐标
        private PointF ToMachine(float sx, float sy)
        {
            RectangleF area = DrawableArea;
            float spanX = XMax - XMin;
            float spanY = YMax - YMin;
            if (spanX <= 0) spanX = 1;
            if (spanY <= 0) spanY = 1;

            float mx = XMin + (sx - area.Left) / area.Width * spanX;
            float my = YMin + (area.Bottom - sy) / area.Height * spanY;
            return new PointF(mx, my);
        }

        // 可绘制区域（留出边距给刻度文字）
        private RectangleF DrawableArea
        {
            get
            {
                RectangleF r = ClientRectangle;
                const float pad = 28f;
                return new RectangleF(r.Left + pad, r.Top + pad,
                                      r.Width - pad * 2, r.Height - pad * 2);
            }
        }

        // 每一帧由主窗体的 Timer 调用：current 向 target 平滑过渡
        public void Advance(float speedFraction)
        {
            float lerp = MathHelper.ClampLerp(speedFraction);
            CurrentX += (TargetX - CurrentX) * lerp;
            CurrentY += (TargetY - CurrentY) * lerp;

            // 只有移动了一定距离才记录轨迹，避免点堆积
            if (_trail.Count == 0 ||
                Dist(_trail[_trail.Count - 1], new PointF(CurrentX, CurrentY)) > 0.5f)
            {
                _trail.Add(new PointF(CurrentX, CurrentY));
                if (_trail.Count > MaxTrail) _trail.RemoveAt(0);
            }
            Invalidate();
        }

        /// <summary>
        /// 由后端位置反馈直接更新实际位置（严禁使用模拟数据）。
        /// 与 Advance 不同，此方法不做插值动画，直接将 Current 设为后端报告的真实值。
        /// </summary>
        public void UpdateActual(float x, float y)
        {
            CurrentX = x;
            CurrentY = y;

            if (_trail.Count == 0 ||
                Dist(_trail[_trail.Count - 1], new PointF(CurrentX, CurrentY)) > 0.5f)
            {
                _trail.Add(new PointF(CurrentX, CurrentY));
                if (_trail.Count > MaxTrail) _trail.RemoveAt(0);
            }
            Invalidate();
        }

        public void ClearTrail()
        {
            _trail.Clear();
            Invalidate();
        }

        /// <summary>设置预设点位标记（在视图上显示菱形标记 + 名称）。</summary>
        public void SetPresetMarkers(List<PresetPoint> presets)
        {
            _presetMarkers.Clear();
            if (presets != null)
                _presetMarkers.AddRange(presets);
            Invalidate();
        }

        /// <summary>清除预设点位标记。</summary>
        public void ClearPresetMarkers()
        {
            _presetMarkers.Clear();
            Invalidate();
        }

        private static float Dist(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            PaintHelper.SetupGraphics(g);

            RectangleF area = DrawableArea;

            // 背景
            PaintHelper.FillBackground(g, this);

            DrawGrid(g, area);
            DrawAxes(g, area);
            DrawTrail(g);
            DrawPresetMarkers(g);
            DrawCrosshair(g, area);
            DrawTarget(g);
            DrawCurrent(g);
            DrawCornerInfo(g);
        }

        private void DrawGrid(Graphics g, RectangleF area)
        {
            float spanX = XMax - XMin;
            float spanY = YMax - YMin;
            int stepX = PaintHelper.ChooseStep(spanX, 10);
            int stepY = PaintHelper.ChooseStep(spanY, 10);
            using (Pen p = new Pen(Color.FromArgb(220, 224, 230), 1f))
            using (Pen pMajor = new Pen(Color.FromArgb(200, 206, 214), 1.4f))
            using (Brush tb = new SolidBrush(Color.FromArgb(120, 130, 145)))
            using (Font f = new Font("Segoe UI", 7.5F))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Far;
                sf.LineAlignment = StringAlignment.Center;

                // 竖线（X 方向）
                for (float v = XMin; v <= XMax + 0.01f; v += stepX)
                {
                    bool major = Math.Abs(v % (stepX * 2)) < 0.01f || Math.Abs(v) < 0.01f;
                    Pen pen = (Math.Abs(v) < 0.01f) ? null : (major ? pMajor : p);

                    PointF top = ToScreen(v, YMax);
                    PointF bot = ToScreen(v, YMin);
                    if (pen != null) g.DrawLine(pen, top, bot);

                    if (major)
                        g.DrawString(v.ToString("0"), f, tb, bot.X, area.Bottom + 6f, sf);
                }

                // 横线（Y 方向）
                StringFormat sf2 = new StringFormat();
                sf2.Alignment = StringAlignment.Far;
                sf2.LineAlignment = StringAlignment.Center;
                for (float v = YMin; v <= YMax + 0.01f; v += stepY)
                {
                    bool major = Math.Abs(v % (stepY * 2)) < 0.01f || Math.Abs(v) < 0.01f;
                    Pen pen = (Math.Abs(v) < 0.01f) ? null : (major ? pMajor : p);

                    PointF lft = ToScreen(XMin, v);
                    PointF rgt = ToScreen(XMax, v);
                    if (pen != null) g.DrawLine(pen, lft, rgt);

                    if (major)
                        g.DrawString(v.ToString("0"), f, tb, area.Left - 4f, lft.Y, sf2);
                }
            }
        }



        private void DrawAxes(Graphics g, RectangleF area)
        {
            // 原点十字（X=0, Y=0）
            PointF origin = ToScreen(0, 0);
            using (Pen p = new Pen(Color.FromArgb(120, 130, 150), 1.6f))
            {
                p.DashStyle = DashStyle.Dash;
                g.DrawLine(p, area.Left, origin.Y, area.Right, origin.Y);
                g.DrawLine(p, origin.X, area.Top, origin.X, area.Bottom);
            }
        }

        private void DrawTrail(Graphics g)
        {
            if (_trail.Count < 2) return;
            using (Pen p = new Pen(Color.FromArgb(80, 130, 220, 90), 2f))
            {
                p.LineJoin = LineJoin.Round;
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                PointF[] pts = new PointF[_trail.Count];
                for (int i = 0; i < _trail.Count; i++)
                    pts[i] = ToScreen(_trail[i].X, _trail[i].Y);
                g.DrawLines(p, pts);
            }
        }

        private void DrawPresetMarkers(Graphics g)
        {
            if (_presetMarkers.Count == 0) return;
            using (Pen pen = new Pen(Color.FromArgb(220, 160, 60), 1.8f))
            using (Brush brush = new SolidBrush(Color.FromArgb(180, 200, 160, 60)))
            using (Font f = new Font("Segoe UI", 7F))
            using (Brush tb = new SolidBrush(Color.FromArgb(180, 140, 50)))
            {
                for (int i = 0; i < _presetMarkers.Count; i++)
                {
                    PresetPoint pt = _presetMarkers[i];
                    PointF sp = ToScreen(pt.X, pt.Y);

                    // 菱形标记 (8x8)
                    PointF[] diamond = new PointF[4];
                    diamond[0] = new PointF(sp.X, sp.Y - 6);     // 上
                    diamond[1] = new PointF(sp.X + 6, sp.Y);     // 右
                    diamond[2] = new PointF(sp.X, sp.Y + 6);     // 下
                    diamond[3] = new PointF(sp.X - 6, sp.Y);     // 左
                    g.FillPolygon(brush, diamond);
                    g.DrawPolygon(pen, diamond);

                    // 名称标注
                    if (!string.IsNullOrEmpty(pt.Name))
                        g.DrawString(pt.Name, f, tb, sp.X + 8, sp.Y - 8);
                }
            }
        }

        private void DrawCrosshair(Graphics g, RectangleF area)
        {
            // 当前点的十字虚线（贯穿整个工作区）
            PointF cur = ToScreen(CurrentX, CurrentY);
            using (Pen p = new Pen(Color.FromArgb(90, 90, 200, 120), 1f))
            {
                p.DashStyle = DashStyle.Dot;
                g.DrawLine(p, area.Left, cur.Y, area.Right, cur.Y);
                g.DrawLine(p, cur.X, area.Top, cur.X, area.Bottom);
            }
        }

        private void DrawTarget(Graphics g)
        {
            // 目标点：空心小圆
            PointF t = ToScreen(TargetX, TargetY);
            using (Pen p = new Pen(Color.FromArgb(220, 140, 40), 1.6f))
            {
                g.DrawEllipse(p, t.X - 6, t.Y - 6, 12, 12);
                g.DrawLine(p, t.X - 9, t.Y, t.X + 9, t.Y);
                g.DrawLine(p, t.X, t.Y - 9, t.X, t.Y + 9);
            }
        }

        private void DrawCurrent(Graphics g)
        {
            // 当前点：实心圆 + 光晕
            PointF c = ToScreen(CurrentX, CurrentY);
            using (Brush halo = new SolidBrush(Color.FromArgb(60, 40, 120, 220)))
                g.FillEllipse(halo, c.X - 12, c.Y - 12, 24, 24);
            using (Brush core = new SolidBrush(_dragging || _hover
                       ? Color.FromArgb(220, 60, 80)
                       : Color.FromArgb(40, 120, 220)))
            {
                g.FillEllipse(core, c.X - 6, c.Y - 6, 12, 12);
            }
            using (Pen p = new Pen(Color.White, 1.5f))
                g.DrawEllipse(p, c.X - 6, c.Y - 6, 12, 12);
        }

        private void DrawCornerInfo(Graphics g)
        {
            string text = string.Format("X = {0:F2}   Y = {1:F2}", CurrentX, CurrentY);
            using (Font f = new Font("Consolas", 9F, FontStyle.Bold))
            using (Brush b = new SolidBrush(Color.FromArgb(50, 60, 80)))
            {
                SizeF sz = g.MeasureString(text, f);
                RectangleF box = new RectangleF(8, 8, sz.Width + 14, sz.Height + 6);
                using (Brush bg = new SolidBrush(Color.FromArgb(230, 235, 245)))
                    g.FillRectangle(bg, box);
                using (Pen p = new Pen(Color.FromArgb(200, 206, 216)))
                    g.DrawRectangle(p, box.X, box.Y, box.Width, box.Height);
                g.DrawString(text, f, b, box.X + 7, box.Y + 3);
            }

            string hint = "左键拖动设定目标 · 右键设为原点";
            using (Font fh = new Font("Segoe UI", 7.5F))
            using (Brush bh = new SolidBrush(Color.FromArgb(140, 145, 155)))
                g.DrawString(hint, fh, bh, 8, ClientRectangle.Bottom - 18f);
        }

        // —— 鼠标交互 ——
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                SetTargetFromPixel(e.X, e.Y);
                Focus();
            }
            else if (e.Button == MouseButtons.Right)
            {
                TargetX = 0; TargetY = 0;
                OnTargetSetByMouse(new PointF(0, 0));
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _hover = true;
            if (_dragging) SetTargetFromPixel(e.X, e.Y);
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _dragging = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        private void SetTargetFromPixel(float sx, float sy)
        {
            PointF m = ToMachine(sx, sy);
            TargetX = MathHelper.Clamp(m.X, XMin, XMax);
            TargetY = MathHelper.Clamp(m.Y, YMin, YMax);
            OnTargetSetByMouse(new PointF(TargetX, TargetY));
        }

        protected virtual void OnTargetSetByMouse(PointF p)
        {
            EventHandler<TargetSetEventArgs> h = TargetSetByMouse;
            if (h != null) h(this, new TargetSetEventArgs(p));
        }
    }

    /// <summary>
    /// XYView 鼠标设目标事件参数（VS2017 设计器兼容：标准 EventArgs，替代泛型 EventHandler&lt;PointF&gt;）。
    /// </summary>
    public class TargetSetEventArgs : EventArgs
    {
        /// <summary>用户设定的目标点（机械坐标）。</summary>
        public PointF Point { get; private set; }

        /// <summary>目标点 X 分量。</summary>
        public float X { get { return Point.X; } }

        /// <summary>目标点 Y 分量。</summary>
        public float Y { get { return Point.Y; } }

        public TargetSetEventArgs(PointF point)
        {
            Point = point;
        }
    }
}
