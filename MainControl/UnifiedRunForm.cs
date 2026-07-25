using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessModules.MainControl
{
    /// <summary>
    /// 统一运行界面：合并 MainControl / PointJump / Trajectory 三个模组的全部功能。
    /// 包含：XYView + ZBarView + DRO + 目标输入 + 预设点位 + JOG + 轨迹控制 + 速度 + 轴限位设置。
    /// </summary>
    public partial class UnifiedRunForm : Form
    {
        private readonly MainControlProcessModule _module;
        private XyzControllerHub _hub;
        private AxisJogService[] _jogServices;
        private bool _syncing;

        // 预设点位（与 PointJumpProjectSetting 共享）
        private readonly List<PresetPoint> _presets;
        private int _presetCounter;

        /// <summary>
        /// 无参构造：仅供 VS 设计器实例化使用。
        /// 运行时请使用带参构造函数注入工艺模组。
        /// </summary>
        public UnifiedRunForm()
        {
            InitializeComponent();
        }

        public UnifiedRunForm(MainControlProcessModule module)
        {
            _module = module;
            _presets = module.projectSetting.Presets;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 固定右侧面板宽度
            splitMain.IsSplitterFixed = true;
            FixSplitterDistance();
            splitMain.SplitterMoved += new SplitterEventHandler(SplitMain_SplitterMoved);

            // 获取业务层
            _hub = _module.Hub;
            trbSpeed.Value = MathHelper.Clamp(_hub.SpeedSetting, trbSpeed.Minimum, trbSpeed.Maximum);

            // 同步视图范围
            ApplyRanges();

            // 绑定事件
            HookEvents();

            // 监听 hub 变化
            _hub.Changed += new EventHandler(Hub_Changed);

            // JOG 服务
            _jogServices = new AxisJogService[]
            {
                new AxisJogService(_hub.X),
                new AxisJogService(_hub.Y),
                new AxisJogService(_hub.Z)
            };
            ApplyJogSetting();

            // 预设点位
            _presetCounter = _presets.Count;
            RefreshPresetList();
            xyView.SetPresetMarkers(_presets);

            // 初始刷新
            SyncUiFromHub();
            UpdateSpeedLabel();

            // 启动动画
            animTimer.Start();
        }

        private void FixSplitterDistance()
        {
            int panel2Width = 80;
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

        // ============== 事件绑定 ==============

        private void HookEvents()
        {
            // 目标输入 + 跳转
            btnJump.Click += new EventHandler(BtnJump_Click);

            // 预设点位
            btnSavePos.Click += new EventHandler(BtnSavePos_Click);
            btnDeletePos.Click += new EventHandler(BtnDeletePos_Click);
            btnGotoSelected.Click += new EventHandler(BtnGotoSelected_Click);
            lvPresets.DoubleClick += new EventHandler(LvPresets_DoubleClick);

            // JOG
            rbIncremental.CheckedChanged += new EventHandler(RbMode_CheckedChanged);
            nudJogStep.ValueChanged += new EventHandler(NudJogStep_ValueChanged);
            BindJogButton(jogXPlus, 0);
            BindJogButton(jogXMinus, 0);
            BindJogButton(jogYPlus, 1);
            BindJogButton(jogYMinus, 1);
            BindJogButton(jogZPlus, 2);
            BindJogButton(jogZMinus, 2);
            btnEStop.Click += new EventHandler(BtnEStop_Click);

            // 通用
            btnZero.Click += new EventHandler(BtnZero_Click);
            btnCenter.Click += new EventHandler(BtnCenter_Click);
            btnClearTrail.Click += new EventHandler(BtnClearTrail_Click);
            trbSpeed.Scroll += new EventHandler(TrbSpeed_Scroll);

            // XYView 鼠标
            xyView.TargetSetByMouse += new EventHandler<TargetSetEventArgs>(XyView_TargetSetByMouse);

            // 键盘
            this.KeyDown += new KeyEventHandler(UnifiedRunForm_KeyDown);
        }

        private void BindJogButton(JogButton btn, int axisIndex)
        {
            btn.Tag = axisIndex;
            btn.Jog += new EventHandler<JogEventArgs>(JogButton_Jog);
            btn.Stop += new EventHandler<JogEventArgs>(JogButton_Stop);
        }

        private void ApplyJogSetting()
        {
            MainControlGlobalSetting gs = _module.globalSetting;
            JogMode mode = gs.JogIncremental ? JogMode.Incremental : JogMode.Continuous;
            rbIncremental.Checked = gs.JogIncremental;
            rbContinuous.Checked = !gs.JogIncremental;
            nudJogStep.Value = (decimal)gs.JogStep;
            foreach (AxisJogService s in _jogServices)
            {
                s.SetMode(mode);
                s.SetStepDistance(gs.JogStep);
            }
        }

        // ============== UI → 业务 ==============

        private void BtnJump_Click(object sender, EventArgs e)
        {
            float x = (float)nudTX.Value;
            float y = (float)nudTY.Value;
            float z = (float)nudTZ.Value;
            _hub.SetTarget(x, y, z);
        }

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
            xyView.SetPresetMarkers(_presets);
        }

        private void BtnDeletePos_Click(object sender, EventArgs e)
        {
            if (lvPresets.SelectedIndices.Count == 0) return;
            int idx = lvPresets.SelectedIndices[0];
            if (idx >= 0 && idx < _presets.Count)
            {
                _presets.RemoveAt(idx);
                RefreshPresetList();
                xyView.SetPresetMarkers(_presets);
            }
        }

        private void BtnGotoSelected_Click(object sender, EventArgs e)
        {
            JumpToSelectedPreset();
        }

        private void LvPresets_DoubleClick(object sender, EventArgs e)
        {
            JumpToSelectedPreset();
        }

        private void JumpToSelectedPreset()
        {
            if (lvPresets.SelectedIndices.Count == 0) return;
            int idx = lvPresets.SelectedIndices[0];
            if (idx >= 0 && idx < _presets.Count)
            {
                PresetPoint pt = _presets[idx];
                _hub.SetTarget(pt.X, pt.Y, pt.Z);
            }
        }

        private void RbMode_CheckedChanged(object sender, EventArgs e)
        {
            JogMode m = rbIncremental.Checked ? JogMode.Incremental : JogMode.Continuous;
            foreach (AxisJogService s in _jogServices)
                s.SetMode(m);
        }

        private void NudJogStep_ValueChanged(object sender, EventArgs e)
        {
            float step = (float)nudJogStep.Value;
            foreach (AxisJogService s in _jogServices)
                s.SetStepDistance(step);
        }

        private void JogButton_Jog(object sender, JogEventArgs e)
        {
            JogButton btn = (JogButton)sender;
            int idx = (int)btn.Tag;
            _jogServices[idx].OnJogStart(e.Direction);
        }

        private void JogButton_Stop(object sender, JogEventArgs e)
        {
            JogButton btn = (JogButton)sender;
            int idx = (int)btn.Tag;
            _jogServices[idx].OnJogStop();
        }

        private void BtnEStop_Click(object sender, EventArgs e)
        {
            foreach (AxisJogService s in _jogServices)
                s.EmergencyStop();
        }

        private void BtnZero_Click(object sender, EventArgs e)
        {
            _hub.ResetToOrigin();
        }

        private void BtnCenter_Click(object sender, EventArgs e)
        {
            _hub.SetToCenter();
        }

        private void BtnClearTrail_Click(object sender, EventArgs e)
        {
            xyView.ClearTrail();
        }

        private void TrbSpeed_Scroll(object sender, EventArgs e)
        {
            _hub.SpeedSetting = trbSpeed.Value;
            UpdateSpeedLabel();
        }

        private void XyView_TargetSetByMouse(object sender, TargetSetEventArgs e)
        {
            _hub.X.SetTarget(e.X);
            _hub.Y.SetTarget(e.Y);
        }

        private void UnifiedRunForm_KeyDown(object sender, KeyEventArgs e)
        {
            int big = e.Shift ? 10 : 1;
            bool handled = true;
            switch (e.KeyCode)
            {
                case Keys.Left: case Keys.A: _hub.X.Step(-big); break;
                case Keys.Right: case Keys.D: _hub.X.Step(+big); break;
                case Keys.Down: case Keys.S: _hub.Y.Step(-big); break;
                case Keys.Up: case Keys.W: _hub.Y.Step(+big); break;
                case Keys.PageDown: case Keys.Q: _hub.Z.Step(-big); break;
                case Keys.PageUp: case Keys.E: _hub.Z.Step(+big); break;
                case Keys.Space: _hub.ResetToOrigin(); break;
                case Keys.Escape: xyView.ClearTrail(); break;
                default: handled = false; break;
            }
            if (handled) e.Handled = true;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            AxisLimitForm dlg = new AxisLimitForm(_module.globalSetting, _hub, this);
            dlg.ShowDialog(this);
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

        private void SyncUiFromHub()
        {
            if (_syncing) return;
            _syncing = true;
            try
            {
                droX.SetValue(_hub.X.Current);
                droY.SetValue(_hub.Y.Current);
                droZ.SetValue(_hub.Z.Current);
                xyView.TargetX = _hub.X.Target;
                xyView.TargetY = _hub.Y.Target;
                zBar.TargetZ = _hub.Z.Target;
                if (nudTX.Value != (decimal)_hub.X.Target)
                    nudTX.Value = MathHelper.Clamp((decimal)_hub.X.Target, nudTX.Minimum, nudTX.Maximum);
                if (nudTY.Value != (decimal)_hub.Y.Target)
                    nudTY.Value = MathHelper.Clamp((decimal)_hub.Y.Target, nudTY.Minimum, nudTY.Maximum);
                if (nudTZ.Value != (decimal)_hub.Z.Target)
                    nudTZ.Value = MathHelper.Clamp((decimal)_hub.Z.Target, nudTZ.Minimum, nudTZ.Maximum);
            }
            finally { _syncing = false; }
        }

        // ============== 范围同步 ==============

        /// <summary>从全局参数同步轴范围到视图控件。</summary>
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

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            xyView.UpdateActual(_hub.X.Current, _hub.Y.Current);
            zBar.UpdateActual(_hub.Z.Current);
            droX.SetValue(_hub.X.Current);
            droY.SetValue(_hub.Y.Current);
            droZ.SetValue(_hub.Z.Current);
            UpdateStatusLive();
        }

        // ============== 辅助方法 ==============

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
