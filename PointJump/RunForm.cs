using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace ProcessModules.PointJump
{
    /// <summary>
    /// 点位跳转工艺模组运行界面（对应 DOMO 模板中的 RunForm，纯 UI 层）。
    /// 用于点位跳转操作：输入目标坐标或从预设列表选取，一键跳转。
    /// 所有运动逻辑委托给所属模组的 XyzControllerHub（业务层），本类只负责：
    /// 捕获用户输入 → 转发给 hub → 监听 hub 变化 → 刷新控件。
    /// </summary>
    /// <remarks>
    /// 兼容性说明：
    /// 1. 工控机上的旧 .NET 编译器不支持 Lambda 表达式 (s,e) => ...
    ///    因此所有事件订阅都使用命名方法 + 显式委托构造（new EventHandler(...)）。
    /// 2. 所有业务初始化（创建 hub、订阅事件、AddDefaultPresets、SyncUiFromHub）
    ///    都放在 OnLoad 里 —— VS 设计器只执行构造函数，跳过 OnLoad，
    ///    这样设计器不会触发业务逻辑，避免红屏异常。
    /// </remarks>
    public partial class RunForm : Form
    {
        // —— 所属工艺模组（DOMO 模式：new RunForm(this)，业务核心与点位从模组获取）——
        private readonly PointJumpProcessModule _module;

        // —— 业务核心（OnLoad 中从模组获取）——
        private XyzControllerHub _hub;

        // —— UI 同步锁 ——
        private bool _syncing;

        // —— 预设点位列表（与工艺模组项目参数共享同一实例）——
        private readonly List<PresetPoint> _presets;
        private int _presetCounter;

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
        /// <param name="module">所属点位跳转工艺模组。</param>
        public RunForm(PointJumpProcessModule module)
        {
            _module = module;
            _presets = module.projectSetting.Presets;
            InitializeComponent();
            // ★ 故意留空：业务初始化全部挪到 OnLoad
            //    （设计器只执行构造函数，不执行 OnLoad，避免红屏）
        }

        /// <summary>
        /// 窗体首次加载时触发。VS 设计器不会执行此方法。
        /// 所有"启动副作用"（业务层创建、事件订阅、预设加载、初始刷新、
        /// 动画启动）都在这里完成。
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 0) 固定右侧面板宽度，禁止用户拖动分隔条
            splitMain.IsSplitterFixed = true;
            FixSplitterDistance();
            splitMain.SplitterMoved += new SplitterEventHandler(SplitMain_SplitterMoved);

            // 1) 从所属工艺模组获取业务层（模组持有 Hub，界面与其共享）
            _hub = _module.Hub;
            trbSpeed.Value = MathHelper.Clamp(_hub.SpeedSetting, trbSpeed.Minimum, trbSpeed.Maximum);

            // 2) 绑定 UI 事件
            HookEvents();

            // 3) 监听 hub 变化 → 刷新 UI
            _hub.Changed += new EventHandler(Hub_Changed);

            // 4) 刷新预设点位列表（默认点位由模组在 Init 时保证）
            _presetCounter = _presets.Count;
            RefreshPresetList();

            // 5) 初始刷新
            SyncUiFromHub();
            UpdateSpeedLabel();

            // 6) 启动动画
            animTimer.Start();
        }

        /// <summary>
        /// 让 Panel2（右侧控制面板）始终保持在 440px 宽。
        /// </summary>
        private void FixSplitterDistance()
        {
            int panel2Width = 440;
            int target = splitMain.Width - panel2Width - splitMain.SplitterWidth;
            if (target > 0 && splitMain.SplitterDistance != target)
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

        // ============== 预设点位数据结构 ==============
        // 注：PresetPoint 为 ProcessModules 根命名空间下的共享类型，
        //     供界面层与点位跳转工艺模组（PointJumpProcessModule）共同使用。

        // ============== 事件绑定 ==============

        private void HookEvents()
        {
            btnJump.Click += new EventHandler(BtnJump_Click);
            btnSavePos.Click += new EventHandler(BtnSavePos_Click);
            btnDeletePos.Click += new EventHandler(BtnDeletePos_Click);
            btnGotoSelected.Click += new EventHandler(BtnGotoSelected_Click);
            trbSpeed.Scroll += new EventHandler(TrbSpeed_Scroll);
            lvPresets.DoubleClick += new EventHandler(LvPresets_DoubleClick);
            xyView.TargetSetByMouse += new EventHandler<TargetSetEventArgs>(XyView_TargetSetByMouse);
            this.KeyDown += new KeyEventHandler(RunForm_KeyDown);
        }

        // ============== UI → 业务 ==============

        /// <summary>跳转到用户输入的目标坐标。</summary>
        private void BtnJump_Click(object sender, EventArgs e)
        {
            float x = (float)nudTargetX.Value;
            float y = (float)nudTargetY.Value;
            float z = (float)nudTargetZ.Value;
            _hub.SetTarget(x, y, z);
            UpdateStatus("跳转到 X=" + x.ToString("F2") + " Y=" + y.ToString("F2") + " Z=" + z.ToString("F2"));
        }

        /// <summary>保存当前目标位置为预设。</summary>
        private void BtnSavePos_Click(object sender, EventArgs e)
        {
            _presetCounter++;
            PresetPoint pt = new PresetPoint();
            pt.Name = "P" + _presetCounter.ToString();
            pt.X = _hub.X.Target;
            pt.Y = _hub.Y.Target;
            pt.Z = _hub.Z.Target;
            _presets.Add(pt);
            RefreshPresetList();
            UpdateStatus("已保存预设 " + pt.Name);
        }

        /// <summary>删除选中的预设。</summary>
        private void BtnDeletePos_Click(object sender, EventArgs e)
        {
            if (lvPresets.SelectedIndices.Count == 0) return;
            int idx = lvPresets.SelectedIndices[0];
            if (idx >= 0 && idx < _presets.Count)
            {
                string name = _presets[idx].Name;
                _presets.RemoveAt(idx);
                RefreshPresetList();
                UpdateStatus("已删除预设 " + name);
            }
        }

        /// <summary>跳转到选中的预设点位。</summary>
        private void BtnGotoSelected_Click(object sender, EventArgs e)
        {
            JumpToSelectedPreset();
        }

        /// <summary>双击列表项直接跳转。</summary>
        private void LvPresets_DoubleClick(object sender, EventArgs e)
        {
            JumpToSelectedPreset();
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
            if (e.KeyCode == Keys.Enter)
            {
                BtnJump_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
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
                // DRO 实时坐标
                droX.SetValue(_hub.X.Current);
                droY.SetValue(_hub.Y.Current);
                droZ.SetValue(_hub.Z.Current);

                // XY 视图目标
                xyView.TargetX = _hub.X.Target;
                xyView.TargetY = _hub.Y.Target;
            }
            finally { _syncing = false; }
        }

        // ============== 范围同步（设置界面保存后由模组调用）==============

        /// <summary>从模组全局参数同步轴范围到视图控件。</summary>
        public void ApplyRanges()
        {
            PointJumpGlobalSetting gs = _module.globalSetting;
            xyView.XMin = gs.XMin;
            xyView.XMax = gs.XMax;
            xyView.YMin = gs.YMin;
            xyView.YMax = gs.YMax;
            xyView.Invalidate();
        }

        // ============== 动画 Tick ==============

        private void animTimer_Tick(object sender, EventArgs e)
        {
            // 位置由后端服务实时推送，严禁模拟。定时器仅用于刷新显示。
            xyView.UpdateActual(_hub.X.Current, _hub.Y.Current);

            // 更新 DRO 为后端报告的实际值
            droX.SetValue(_hub.X.Current);
            droY.SetValue(_hub.Y.Current);
            droZ.SetValue(_hub.Z.Current);

            UpdateStatusLive();
        }

        // ============== 辅助方法 ==============

        private void JumpToSelectedPreset()
        {
            if (lvPresets.SelectedIndices.Count == 0)
            {
                UpdateStatus("请先选择一个预设点位");
                return;
            }
            int idx = lvPresets.SelectedIndices[0];
            if (idx >= 0 && idx < _presets.Count)
            {
                PresetPoint pt = _presets[idx];
                _hub.SetTarget(pt.X, pt.Y, pt.Z);
                UpdateStatus("跳转到预设 " + pt.Name + " (X=" + pt.X.ToString("F2")
                    + " Y=" + pt.Y.ToString("F2") + " Z=" + pt.Z.ToString("F2") + ")");
            }
        }

        /// <summary>供工艺模组调用：预设点位被外部修改后刷新列表显示。</summary>
        public void RefreshPresets()
        {
            RefreshPresetList();
        }

        private void RefreshPresetList()
        {
            lvPresets.Items.Clear();
            for (int i = 0; i < _presets.Count; i++)
            {
                PresetPoint pt = _presets[i];
                ListViewItem item = new ListViewItem(pt.Name);
                item.SubItems.Add(pt.X.ToString("F2"));
                item.SubItems.Add(pt.Y.ToString("F2"));
                item.SubItems.Add(pt.Z.ToString("F2"));
                lvPresets.Items.Add(item);
            }
        }

        private void UpdateStatusLive()
        {
            lblStatus.Text = string.Format(
                "当前 X={0:F2}  Y={1:F2}  Z={2:F2}   →  目标 X={3:F2}  Y={4:F2}  Z={5:F2}",
                _hub.X.Current, _hub.Y.Current, _hub.Z.Current,
                _hub.X.Target, _hub.Y.Target, _hub.Z.Target);
        }

        private void UpdateStatus(string msg)
        {
            lblStatus.Text = msg;
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
