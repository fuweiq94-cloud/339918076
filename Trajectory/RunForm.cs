using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessModules.Trajectory
{
    /// <summary>
    /// 轨迹查看工艺模组运行界面（对应 DOMO 模板中的 RunForm，纯 UI 层）。
    /// 显示 XY 平面运动轨迹和 Z 轴位置条，用于观察和记录运动路径。
    /// 所有运动逻辑委托给所属模组的 XyzControllerHub（业务层），本类只负责：
    /// 捕获用户输入 → 转发给 hub → 监听 hub 变化 → 刷新控件。
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
        private readonly TrajectoryViewProcessModule _module;

        // —— 业务核心（OnLoad 中从模组获取）——
        private XyzControllerHub _hub;

        // —— UI 同步锁 ——
        private bool _syncing;

        // —— 轨迹统计 ——
        private int _trailPointCount;

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
        /// <param name="module">所属轨迹查看工艺模组。</param>
        public RunForm(TrajectoryViewProcessModule module)
        {
            _module = module;
            InitializeComponent();
            // ★ 故意留空：业务初始化全部挪到 OnLoad
            //    （设计器只执行构造函数，不执行 OnLoad，避免红屏）
        }

        /// <summary>
        /// 窗体首次加载时触发。VS 设计器不会执行此方法。
        /// 所有"启动副作用"（业务层创建、事件订阅、初始刷新、
        /// 动画启动、分隔条锁定）都在这里完成。
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 1) 从所属工艺模组获取业务层（模组持有 Hub，界面与其共享）
            _hub = _module.Hub;
            trbSpeed.Value = MathHelper.Clamp(_hub.SpeedSetting, trbSpeed.Minimum, trbSpeed.Maximum);

            // 2) 绑定 UI 事件
            HookEvents();

            // 3) 监听 hub 变化 → 刷新 UI
            _hub.Changed += new EventHandler(Hub_Changed);

            // 4) 初始刷新（按模组全局参数应用轨迹显示开关）
            cbShowTrail.Checked = _module.globalSetting.ShowTrail;
            SyncUiFromHub();
            UpdateSpeedLabel();

            // 5) 启动动画定时器
            animTimer.Start();

            // 6) 锁定分隔条
            splitMain.IsSplitterFixed = true;
        }

        // ============== 事件绑定 ==============

        private void HookEvents()
        {
            btnClearTrail.Click += new EventHandler(BtnClearTrail_Click);
            cbShowTrail.CheckedChanged += new EventHandler(CbShowTrail_CheckedChanged);
            btnOrigin.Click += new EventHandler(BtnOrigin_Click);
            btnCenter.Click += new EventHandler(BtnCenter_Click);
            btnRandom.Click += new EventHandler(BtnRandom_Click);
            trbSpeed.Scroll += new EventHandler(TrbSpeed_Scroll);
            xyView.TargetSetByMouse += new EventHandler<TargetSetEventArgs>(XyView_TargetSetByMouse);
            this.KeyDown += new KeyEventHandler(RunForm_KeyDown);
        }

        // ============== UI → 业务 ==============

        /// <summary>清除轨迹。</summary>
        private void BtnClearTrail_Click(object sender, EventArgs e)
        {
            ClearTrail();
        }

        // ============== 供工艺模组调用的公共接口 ==============

        /// <summary>供工艺模组调用：清除轨迹并复位计数。</summary>
        public void ClearTrail()
        {
            xyView.ClearTrail();
            _trailPointCount = 0;
            if (_hub != null)
                UpdateTrailInfo();
        }

        /// <summary>供工艺模组读写：是否显示轨迹。</summary>
        public bool ShowTrail
        {
            get { return cbShowTrail.Checked; }
            set { cbShowTrail.Checked = value; }
        }

        /// <summary>供工艺模组读取：当前轨迹点数。</summary>
        public int TrailPointCount
        {
            get { return _trailPointCount; }
        }

        /// <summary>切换轨迹显示。</summary>
        private void CbShowTrail_CheckedChanged(object sender, EventArgs e)
        {
            // XYView 内部通过 Invalidate 重绘，这里通过清除/保留轨迹实现
            // 如果取消勾选，清除当前轨迹显示
            if (!cbShowTrail.Checked)
            {
                xyView.ClearTrail();
            }
            xyView.Invalidate();
        }

        /// <summary>回原点。</summary>
        private void BtnOrigin_Click(object sender, EventArgs e)
        {
            _hub.ResetToOrigin();
        }

        /// <summary>居中。</summary>
        private void BtnCenter_Click(object sender, EventArgs e)
        {
            _hub.SetToCenter();
        }

        /// <summary>随机目标。</summary>
        private void BtnRandom_Click(object sender, EventArgs e)
        {
            _hub.SetRandomTarget(new Random());
        }

        /// <summary>速度滑块变化。</summary>
        private void TrbSpeed_Scroll(object sender, EventArgs e)
        {
            _hub.SpeedSetting = trbSpeed.Value;
            UpdateSpeedLabel();
        }

        /// <summary>鼠标点击 XY 视图设定目标。</summary>
        private void XyView_TargetSetByMouse(object sender, TargetSetEventArgs e)
        {
            _hub.X.SetTarget(e.X);
            _hub.Y.SetTarget(e.Y);
        }

        /// <summary>键盘快捷键。</summary>
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
                    xyView.ClearTrail();
                    _trailPointCount = 0;
                    UpdateTrailInfo();
                    break;
                default:
                    handled = false; break;
            }
            if (handled) e.Handled = true;
        }

        // ============== 业务 → UI ==============

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

        /// <summary>窗体关闭时退订 Hub，避免重复订阅与已释放控件被回调。</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_hub != null)
                _hub.Changed -= Hub_Changed;
            base.OnFormClosed(e);
        }

        /// <summary>把 hub 状态同步到 UI 控件。</summary>
        private void SyncUiFromHub()
        {
            if (_syncing) return;
            _syncing = true;
            try
            {
                // XY 视图目标
                xyView.TargetX = _hub.X.Target;
                xyView.TargetY = _hub.Y.Target;
                zBar.TargetZ = _hub.Z.Target;
            }
            finally { _syncing = false; }
        }

        // ============== 范围同步（设置界面保存后由模组调用）==============

        /// <summary>从模组全局参数同步轴范围到视图控件。</summary>
        public void ApplyRanges()
        {
            TrajectoryGlobalSetting gs = _module.globalSetting;
            xyView.XMin = gs.XMin;
            xyView.XMax = gs.XMax;
            xyView.YMin = gs.YMin;
            xyView.YMax = gs.YMax;
            zBar.RangeMin = gs.ZMin;
            zBar.RangeMax = gs.ZMax;
            xyView.Invalidate();
            zBar.Invalidate();
        }

        // ============== 动画 Tick ==============

        private void animTimer_Tick(object sender, EventArgs e)
        {
            // 位置由后端服务实时推送，严禁模拟。定时器仅用于刷新显示。
            if (cbShowTrail.Checked)
            {
                xyView.UpdateActual(_hub.X.Current, _hub.Y.Current);
                _trailPointCount++;
            }
            else
            {
                xyView.UpdateActual(_hub.X.Current, _hub.Y.Current);
            }
            zBar.UpdateActual(_hub.Z.Current);

            // 更新 DRO 为后端报告的实际值
            droX.SetValue(_hub.X.Current);
            droY.SetValue(_hub.Y.Current);
            droZ.SetValue(_hub.Z.Current);

            UpdateStatusLive();
            UpdateTrailInfo();
        }

        // ============== 辅助方法 ==============

        private void UpdateStatusLive()
        {
            lblStatus.Text = string.Format(
                "当前 X={0:F2}  Y={1:F2}  Z={2:F2}   →  目标 X={3:F2}  Y={4:F2}  Z={5:F2}",
                _hub.X.Current, _hub.Y.Current, _hub.Z.Current,
                _hub.X.Target, _hub.Y.Target, _hub.Z.Target);
        }

        private void UpdateTrailInfo()
        {
            lblTrailInfo.Text = "轨迹点数：" + _trailPointCount.ToString()
                + "   速度档位：" + _hub.SpeedSetting.ToString();
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
    }
}
