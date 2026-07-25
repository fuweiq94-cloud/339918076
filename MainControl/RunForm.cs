using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessModules.MainControl
{
    /// <summary>
    /// 主控制工艺模组运行界面（对应 DOMO 模板中的 RunForm，纯 UI 层）。
    /// 不包含任何"算坐标/算动画"的业务逻辑，
    /// 所有状态都委托给所属模组的 XyzControllerHub（业务层）。
    /// 本类只负责：捕获用户输入 → 转发给 hub → 监听 hub 变化 → 刷新控件。
    /// </summary>
    /// <remarks>
    /// 兼容性说明：
    /// 1. 工控机上的旧 .NET 编译器不支持 Lambda 表达式 (s,e) => ...
    ///    因此所有事件订阅都使用命名方法 + 显式委托构造（new EventHandler(...)）。
    /// 2. 所有业务初始化（创建 hub、订阅事件、SyncUiFromHub、动画启动）
    ///    都放在 OnLoad 里 —— VS 设计器只执行构造函数，跳过 OnLoad，
    ///    这样设计器不会触发业务逻辑，避免红屏异常。
    /// </remarks>
    public partial class RunForm : Form
    {
        // —— 所属工艺模组（DOMO 模式：new RunForm(this)，业务核心从模组获取）——
        private readonly MainControlProcessModule _module;

        // —— 业务核心（OnLoad 中从模组获取）——
        private XyzControllerHub _hub;

        // —— JOG 服务：每个轴一个 ——
        private AxisJogService[] _jogServices;

        // —— UI 同步锁：防止 hub.Changed 回调里又改 UI 触发新事件 ——
        private bool _syncing;

        /// <summary>
        /// 无参构造：仅供 VS 设计器实例化使用。
        /// 运行时请使用带参构造函数注入工艺模组。
        /// </summary>
        public RunForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DOMO 模式构造：由所属工艺模组创建运行界面（new RunForm(this)）。
        /// </summary>
        /// <param name="module">所属主控制工艺模组。</param>
        public RunForm(MainControlProcessModule module)
        {
            _module = module;
            InitializeComponent();
            // ★ 故意留空：业务初始化全部挪到 OnLoad
            //    （设计器只执行构造函数，不执行 OnLoad，避免红屏）
        }

        /// <summary>
        /// 窗体首次加载时触发。VS 设计器不会执行此方法。
        /// 所有"启动副作用"（业务层创建、范围同步、事件订阅、初始刷新、
        /// 动画启动、Z 条锁定）都在这里完成。
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 1) 从所属工艺模组获取业务层（模组持有 Hub，界面与其共享）
            _hub = _module.Hub;
            trbSpeed.Value = MathHelper.Clamp(_hub.SpeedSetting, trbSpeed.Minimum, trbSpeed.Maximum);

            // 2) 同步自定义视图的范围（XYView/ZBarView 自己维护坐标范围）
            xyView.RangeMin = trbX.Minimum;
            xyView.RangeMax = trbX.Maximum;
            zBar.RangeMin = trbZ.Minimum;
            zBar.RangeMax = trbZ.Maximum;

            // 3) 绑定 UI 事件 → 转发到 hub
            HookEvents();

            // 4) 监听 hub 变化 → 刷新 UI（不用 Lambda）
            _hub.Changed += new EventHandler(Hub_Changed);

            // 5) 初始刷新一次，让 UI 反映 hub 默认状态
            SyncUiFromHub();
            UpdateSpeedLabel();

            // 6) 启动动画定时器
            animTimer.Start();

            // 7) 锁定 Z 条宽度：禁止用户拖动分隔条改变 Z 条宽度。
            splitMain.IsSplitterFixed = true;
            FixSplitterDistance();
            splitMain.SplitterMoved += new SplitterEventHandler(SplitMain_SplitterMoved);
        }

        /// <summary>窗体关闭时退订 Hub，避免重复订阅与已释放控件被回调。</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_hub != null)
                _hub.Changed -= Hub_Changed;
            base.OnFormClosed(e);
        }


        /// <summary>
        /// 让 Panel2（Z 条）始终保持在右侧 80px 宽。
        /// </summary>
        private void FixSplitterDistance()
        {
            int panel2Width = 80;
            int target = splitMain.Width - panel2Width - splitMain.SplitterWidth;
            if (splitMain.SplitterDistance != target && target > 0)
                splitMain.SplitterDistance = target;
        }

        private void SplitMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            FixSplitterDistance();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (splitMain != null) FixSplitterDistance();
        }

        // ============== UI → 业务 ==============

        private void HookEvents()
        {
            // —— 滑块、数字框、按钮（全部用命名方法 + 委托构造）——
            trbX.Tag = _hub.X; trbY.Tag = _hub.Y; trbZ.Tag = _hub.Z;
            trbX.Scroll += new EventHandler(Trb_Scroll);
            trbY.Scroll += new EventHandler(Trb_Scroll);
            trbZ.Scroll += new EventHandler(Trb_Scroll);

            nudX.Tag = _hub.X; nudY.Tag = _hub.Y; nudZ.Tag = _hub.Z;
            nudX.ValueChanged += new EventHandler(Nud_ValueChanged);
            nudY.ValueChanged += new EventHandler(Nud_ValueChanged);
            nudZ.ValueChanged += new EventHandler(Nud_ValueChanged);

            btnXMinus.Tag = new object[] { _hub.X, -1 };
            btnXPlus.Tag  = new object[] { _hub.X, +1 };
            btnYMinus.Tag = new object[] { _hub.Y, -1 };
            btnYPlus.Tag  = new object[] { _hub.Y, +1 };
            btnZMinus.Tag = new object[] { _hub.Z, -1 };
            btnZPlus.Tag  = new object[] { _hub.Z, +1 };
            btnXMinus.Click += new EventHandler(BtnStep_Click);
            btnXPlus.Click  += new EventHandler(BtnStep_Click);
            btnYMinus.Click += new EventHandler(BtnStep_Click);
            btnYPlus.Click  += new EventHandler(BtnStep_Click);
            btnZMinus.Click += new EventHandler(BtnStep_Click);
            btnZPlus.Click  += new EventHandler(BtnStep_Click);

            // —— 通用按钮 ——
            btnZero.Click      += new EventHandler(BtnZero_Click);
            btnCenter.Click    += new EventHandler(BtnCenter_Click);
            btnRandom.Click    += new EventHandler(BtnRandom_Click);
            btnClearTrail.Click += new EventHandler(BtnClearTrail_Click);

            trbSpeed.Scroll += new EventHandler(TrbSpeed_Scroll);

            // —— JOG 服务：三轴各创建一个 ——
            _jogServices = new AxisJogService[]
            {
                new AxisJogService(_hub.X),
                new AxisJogService(_hub.Y),
                new AxisJogService(_hub.Z)
            };

            // 寸动 / 连续模式切换
            rbIncremental.CheckedChanged += new EventHandler(RbMode_CheckedChanged);

            // 步长变化（寸动模式生效）
            nudJogStep.ValueChanged += new EventHandler(NudJogStep_ValueChanged);

            // JogButton 控件：Jog 事件 = 按下/持续触发；Stop 事件 = 松开
            // 注意：寸动模式下 Jog 事件会重复触发，但每次走一步；
            //       连续模式下 Jog 只在按下时触发一次（设目标到限位），Stop 才停。
            BindJogButton(jogXPlus, _jogServices[0]);
            BindJogButton(jogXMinus, _jogServices[0]);
            BindJogButton(jogYPlus, _jogServices[1]);
            BindJogButton(jogYMinus, _jogServices[1]);
            BindJogButton(jogZPlus, _jogServices[2]);
            BindJogButton(jogZMinus, _jogServices[2]);

            // 急停按钮
            btnEStop.Click += new EventHandler(BtnEStop_Click);

            // —— 鼠标拖动 XY 视图设定目标 ——
            xyView.TargetSetByMouse += new EventHandler<TargetSetEventArgs>(XyView_TargetSetByMouse);

            // —— 键盘 ——
            this.KeyDown += new KeyEventHandler(RunForm_KeyDown);
        }

        // —— hub 变化 → 刷新 UI ——
        private void Hub_Changed(object sender, EventArgs e)
        {
            // 硬件回调可能跨线程，安全地切到 UI 线程刷新
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(Hub_Changed), sender, e);
                return;
            }
            SyncUiFromHub();
        }

        // —— 滑块事件处理器（通过 Tag 获取对应轴）——
        private void Trb_Scroll(object sender, EventArgs e)
        {
            TrackBar trb = (TrackBar)sender;
            AxisController axis = (AxisController)trb.Tag;
            axis.SetTarget(trb.Value);
        }

        // —— 数字框事件处理器（通过 Tag 获取对应轴）——
        private void Nud_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            AxisController axis = (AxisController)nud.Tag;
            axis.SetTarget((float)nud.Value);
        }

        // —— 单步按钮（通过 Tag 获取轴和方向）——
        private void BtnStep_Click(object sender, EventArgs e)
        {
            object[] data = (object[])((Control)sender).Tag;
            AxisController axis = (AxisController)data[0];
            int direction = (int)data[1];
            axis.Step(direction);
        }

        // —— 通用按钮 ——
        private void BtnZero_Click(object sender, EventArgs e)      { _hub.ResetToOrigin(); }
        private void BtnCenter_Click(object sender, EventArgs e)    { _hub.SetToCenter(); }
        private void BtnRandom_Click(object sender, EventArgs e)    { _hub.SetRandomTarget(new Random()); }
        private void BtnClearTrail_Click(object sender, EventArgs e) { xyView.ClearTrail(); }

        // —— 速度滑块 ——
        private void TrbSpeed_Scroll(object sender, EventArgs e)
        {
            _hub.SpeedSetting = trbSpeed.Value;
            UpdateSpeedLabel();
        }

        // —— JOG 模式切换 ——
        private void RbMode_CheckedChanged(object sender, EventArgs e)
        {
            JogMode m = rbIncremental.Checked ? JogMode.Incremental : JogMode.Continuous;
            ForEachJog(delegate(AxisJogService s) { s.SetMode(m); });
        }

        // —— JOG 步长 ——
        private void NudJogStep_ValueChanged(object sender, EventArgs e)
        {
            float step = (float)nudJogStep.Value;
            ForEachJog(delegate(AxisJogService s) { s.SetStepDistance(step); });
        }

        // —— 急停 ——
        private void BtnEStop_Click(object sender, EventArgs e)
        {
            ForEachJog(delegate(AxisJogService s) { s.EmergencyStop(); });
        }

        /// <summary>对三个 JOG 服务统一执行操作。</summary>
        private void ForEachJog(Action<AxisJogService> action)
        {
            foreach (AxisJogService s in _jogServices)
                action(s);
        }

        // —— 鼠标点击 XY 视图设定目标 ——
        private void XyView_TargetSetByMouse(object sender, TargetSetEventArgs e)
        {
            _hub.X.SetTarget(e.X);
            _hub.Y.SetTarget(e.Y);
        }

        /// <summary>
        /// 把 JogButton 控件的事件接到 AxisJogService。
        /// JogButton.Jog 事件会在按下/重复触发时调用；Stop 在松开时调用。
        /// </summary>
        private void BindJogButton(JogButton btn, AxisJogService service)
        {
            // 把 service 通过 closure 替换为 JogButton.Tag 携带，便于在命名方法里取回。
            btn.Tag = service;
            btn.Jog += new EventHandler<JogEventArgs>(JogButton_Jog);
            btn.Stop += new EventHandler<JogEventArgs>(JogButton_Stop);
        }

        private void JogButton_Jog(object sender, JogEventArgs e)
        {
            JogButton btn = (JogButton)sender;
            AxisJogService service = (AxisJogService)btn.Tag;
            service.OnJogStart(e.Direction);
        }

        private void JogButton_Stop(object sender, JogEventArgs e)
        {
            JogButton btn = (JogButton)sender;
            AxisJogService service = (AxisJogService)btn.Tag;
            service.OnJogStop();
        }

        private void RunForm_KeyDown(object sender, KeyEventArgs e)
        {
            int big = e.Shift ? 10 : 1;
            bool handled = true;

            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    _hub.X.Step(-big); break;
                case Keys.Right:
                case Keys.D:
                    _hub.X.Step(+big); break;
                case Keys.Down:
                case Keys.S:
                    _hub.Y.Step(-big); break;
                case Keys.Up:
                case Keys.W:
                    _hub.Y.Step(+big); break;
                case Keys.PageDown:
                case Keys.Q:
                    _hub.Z.Step(-big); break;
                case Keys.PageUp:
                case Keys.E:
                    _hub.Z.Step(+big); break;
                case Keys.Space:
                    _hub.ResetToOrigin(); break;
                case Keys.Escape:
                    xyView.ClearTrail(); break;
                default:
                    handled = false; break;
            }
            if (handled) e.Handled = true;
        }

        // ============== 业务 → UI ==============

        /// <summary>
        /// 把 hub 的状态（目标值 + 当前值）反向同步到所有 UI 控件。
        /// </summary>
        private void SyncUiFromHub()
        {
            if (_syncing) return;
            _syncing = true;
            try
            {
                // 滑块（必须整数）
                trbX.Value = MathHelper.Clamp((int)Math.Round(_hub.X.Target), trbX.Minimum, trbX.Maximum);
                trbY.Value = MathHelper.Clamp((int)Math.Round(_hub.Y.Target), trbY.Minimum, trbY.Maximum);
                trbZ.Value = MathHelper.Clamp((int)Math.Round(_hub.Z.Target), trbZ.Minimum, trbZ.Maximum);

                // 数字框（支持小数）
                if (nudX.Value != (decimal)_hub.X.Target)
                    nudX.Value = MathHelper.Clamp((decimal)_hub.X.Target, nudX.Minimum, nudX.Maximum);
                if (nudY.Value != (decimal)_hub.Y.Target)
                    nudY.Value = MathHelper.Clamp((decimal)_hub.Y.Target, nudY.Minimum, nudY.Maximum);
                if (nudZ.Value != (decimal)_hub.Z.Target)
                    nudZ.Value = MathHelper.Clamp((decimal)_hub.Z.Target, nudZ.Minimum, nudZ.Maximum);

                // 自定义视图：目标和当前都同步
                xyView.TargetX = _hub.X.Target;
                xyView.TargetY = _hub.Y.Target;
                zBar.TargetZ = _hub.Z.Target;
                // 当前值不能直接赋（CurrentX/Y/Z 是只读），靠 animTimer 推进后视图自己刷新
            }
            finally { _syncing = false; }
        }

        // ============== 范围同步（设置界面保存后由模组调用）==============

        /// <summary>从模组全局参数同步轴范围到视图控件（无需改源码）。</summary>
        public void ApplyRanges()
        {
            MainControlGlobalSetting gs = _module.globalSetting;
            xyView.XMin = gs.XMin;
            xyView.XMax = gs.XMax;
            xyView.YMin = gs.YMin;
            xyView.YMax = gs.YMax;
            zBar.RangeMin = gs.ZMin;
            zBar.RangeMax = gs.ZMax;
            xyView.Invalidate();
            zBar.Invalidate();
        }

        // ============== 定时刷新 ==============
        private void animTimer_Tick(object sender, EventArgs e)
        {
            // 位置由后端服务实时推送，无需模拟推进。
            // 定时器仅用于周期性刷新视图（将后端报告的实际位置显示到控件）。
            xyView.UpdateActual(_hub.X.Current, _hub.Y.Current);
            zBar.UpdateActual(_hub.Z.Current);

            UpdateStatusLive();
        }

        // ============== 状态栏 ==============
        private void UpdateStatusLive()
        {
            lblStatus.Text = string.Format(
                "当前 X={0:F2}  Y={1:F2}  Z={2:F2}   →  目标 X={3:F2}  Y={4:F2}  Z={5:F2}",
                _hub.X.Current, _hub.Y.Current, _hub.Z.Current,
                _hub.X.Target, _hub.Y.Target, _hub.Z.Target);
        }

        private void UpdateSpeedLabel()
        {
            string label;
            int v = trbSpeed.Value;
            if (v < 20) label = "速度：慢";
            else if (v < 60) label = "速度：中";
            else if (v < 90) label = "速度：快";
            else label = "速度：瞬时";
            lblSpeed.Text = label;
        }

        private void trbX_Scroll(object sender, EventArgs e)
        {

        }

        private void xyView_Click(object sender, EventArgs e)
        {

        }
    }
}
