using System;
using System.Windows.Forms;
using InterfaceDefine;
using MainModule;

namespace MainControlProcessModule
{
    /// <summary>
    /// 主控制工艺模组（对应 DOMO.CS 中的 ProcessModuleDemo）。
    /// 继承 ProcessModuleBase（平台提供的基类），
    /// 封装 XYZ 三轴手动控制业务：
    /// 持有 XyzControllerHub（业务层）与三个 AxisJogService（JOG 服务），
    /// 运行界面为 RunForm（DOMO 模式：new RunForm(this)，与模组共享同一份业务状态）。
    /// </summary>
    /// <remarks>
    /// 支持的平台命令（Action）：
    ///   GOTO x y z     → 三轴移动到指定目标
    ///   ORIGIN         → 回原点
    ///   CENTER         → 回中心
    ///   JOG axis dir   → 寸动一步（axis = X/Y/Z，dir = +1/-1）
    ///   SETSPEED value → 设置速度档位 [0,100]
    ///   STOP / ESTOP   → 停止所有运动
    /// </remarks>
    public class MainControlProcessModule : ProcessModuleBase
    {
        internal RunForm runForm;
        internal UnifiedRunForm unifiedRunForm;
        internal URotationForm unirotationForm;  // U 轴旋转控制表单
        public ModuleSettingForm settingForm;

        public MainControlGlobalSetting globalSetting;
        public MainControlProjectSetting projectSetting;

        // —— 业务核心：与运行界面共享同一个 Hub ——
        private XyzControllerHub _hub;
        private AxisJogService[] _jogServices;

        /// <summary>模组持有的 XYZ 控制器（集成现有业务层）。</summary>
        public XyzControllerHub Hub { get { return _hub; } }

        /// <summary>模组持有的 JOG 服务数组（XYZU 四轴）。</summary>
        public AxisJogService[] JogServices { get { return _jogServices; } }

        public override dynamic FunctionCaller
        {
            get { return this; }
        }

        public override string GetInfo()
        {
            return "主控制工艺模组";
        }

        public override bool Init(string strName)
        {
            try
            {
                processModuleName = strName;
                bInitOK = false;
                LoadSetting();

                // 保证预设点位列表有默认值
                if (projectSetting.Presets.Count == 0)
                    AddDefaultPresets();

                // 创建业务层（轴范围与速度取自全局参数）
                _hub = new XyzControllerHub(null,
                    globalSetting.XMin, globalSetting.XMax,
                    globalSetting.YMin, globalSetting.YMax,
                    globalSetting.ZMin, globalSetting.ZMax,
                    globalSetting.UMin, globalSetting.UMax);
                _hub.SpeedSetting = globalSetting.SpeedSetting;

                // JOG 服务：每个轴一个（XYZU 四轴）
                _jogServices = new AxisJogService[]
                {
                    new AxisJogService(_hub.X),
                    new AxisJogService(_hub.Y),
                    new AxisJogService(_hub.Z),
                    new AxisJogService(_hub.U)
                };
                ApplyJogSetting();

                // 注册平台事件：打开项目时重新加载项目参数
                ProjectManager.openProject += OnOpenProject;

                bInitOK = true;
                return bInitOK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(strName + "工艺模组初始化异常" + ex.Message + ex.StackTrace);
                return false;
            }
        }

        #region 注册平台事件

        /// <summary>
        /// 打开项目时重新加载项目文件，初始化工艺模组。
        /// </summary>
        private bool OnOpenProject(string strProjectPath)
        {
            LoadSetting();
            return bInitOK;
        }

        #endregion

        /// <summary>把 JOG 全局参数应用到 JOG 服务。</summary>
        private void ApplyJogSetting()
        {
            JogMode mode = globalSetting.JogIncremental ? JogMode.Incremental : JogMode.Continuous;
            foreach (AxisJogService s in _jogServices)
            {
                s.SetMode(mode);
                s.SetStepDistance(globalSetting.JogStep);
            }
        }

        /// <summary>补一组默认预设点位（统一界面使用）。</summary>
        private void AddDefaultPresets()
        {
            projectSetting.Presets.Add(new PresetPoint("原点", 0f, 0f, 0f));
            projectSetting.Presets.Add(new PresetPoint("中心", 0f, 0f, 50f));
            projectSetting.Presets.Add(new PresetPoint("A工位", 30f, 40f, 10f));
            projectSetting.Presets.Add(new PresetPoint("B工位", -50f, 60f, 20f));
        }

        /// <summary>重新加载工艺模组。</summary>
        public override bool ReOpen()
        {
            LoadSetting();
            if (_hub != null)
                _hub.SpeedSetting = globalSetting.SpeedSetting;
            if (_jogServices != null)
                ApplyJogSetting();
            return bInitOK;
        }

        public override bool Close()
        {
            ProjectManager.openProject -= OnOpenProject;
            return true;
        }

        public override bool LoadSetting()
        {
            try
            {
                globalSetting = MainControlGlobalSetting.Load(processModuleName);
                projectSetting = MainControlProjectSetting.Load(processModuleName);

                taskItemSetting = globalSetting.taskItemSetting;
                GetModuleVariable("主控制首次运行", DataType.布尔, "false");
                GetModuleVariable("主控制初始化完成", DataType.布尔, "false");
                GetModuleVariable("主平台请求移动", DataType.布尔, "false");
                GetModuleVariable("移动完成", DataType.布尔, "false");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(processModuleName + "加载setting发生异常" + ex.Message + ex.StackTrace);
                globalSetting = MainControlGlobalSetting.Load(processModuleName);
                return false;
            }
        }

        /// <summary>保存工艺模组参数。</summary>
        public override bool Save()
        {
            try
            {
                // 把当前业务状态回写到参数对象
                if (_hub != null && projectSetting != null)
                {
                    projectSetting.LastTargetX = _hub.X.Target;
                    projectSetting.LastTargetY = _hub.Y.Target;
                    projectSetting.LastTargetZ = _hub.Z.Target;
                }
                if (globalSetting != null && _hub != null)
                    globalSetting.SpeedSetting = _hub.SpeedSetting;

                globalSetting.Save();
                projectSetting.Save();

                // 保存后立即应用新范围到 Hub 和运行界面（无需改源码）
                ApplyRanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存参数时发生异常," + ex.Message + ex.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>将全局参数中的轴范围应用到 Hub 和运行界面（设置界面保存后自动调用）。</summary>
        private void ApplyRanges()
        {
            if (_hub != null)
                _hub.SetAxisRanges(
                    globalSetting.XMin, globalSetting.XMax,
                    globalSetting.YMin, globalSetting.YMax,
                    globalSetting.ZMin, globalSetting.ZMax,
                    globalSetting.UMin, globalSetting.UMax);
            if (unifiedRunForm != null && !unifiedRunForm.IsDisposed)
                unifiedRunForm.ApplyRanges();
            if (runForm != null && !runForm.IsDisposed)
                runForm.ApplyRanges();
        }

        /// <summary>显示工艺模组运行界面（统一界面）。</summary>
        public override bool ShowRunForm(Panel panel)
        {
            foreach (Control controlItem in panel.Controls)
            {
                controlItem.Visible = false;
            }
            panel.Controls.Clear();
            if (unifiedRunForm == null || unifiedRunForm.IsDisposed)
                unifiedRunForm = new UnifiedRunForm(this);

            unifiedRunForm.ExternalJogServices = _jogServices;
            unifiedRunForm.TopLevel = false;
            panel.Controls.Add(unifiedRunForm);
            unifiedRunForm.Dock = DockStyle.Fill;
            unifiedRunForm.Show();
            return true;
        }

        /// <summary>显示工艺模组设置界面。</summary>
        public override bool ShowSettingForm(Panel panel)
        {
            foreach (Control controlItem in panel.Controls)
            {
                controlItem.Visible = false;
            }
            panel.Controls.Clear();
            if (settingForm == null || settingForm.IsDisposed)
                settingForm = new ModuleSettingForm(this, globalSetting, projectSetting);

            settingForm.TopLevel = false;
            panel.Controls.Add(settingForm);
            settingForm.Dock = DockStyle.Fill;
            settingForm.Show();
            return true;
        }

        /// <summary>显示 U 轴旋转控制面板（独立窗口）。</summary>
        public bool ShowURotationControl(Panel panel)
        {
            // U 轴表单作为独立窗口，不在 Panel 中显示
            if (unirotationForm == null || unirotationForm.IsDisposed)
                unirotationForm = new URotationForm(this);

            unirotationForm.StartPosition = FormStartPosition.CenterParent;
            unirotationForm.Show();
            return true;
        }

        /// <summary>执行工艺模组相关动作。</summary>
        public override int Action(params object[] param)
        {
            if (param == null || param.Length == 0)
            {
                InsertAlarm("空命令");
                return -1;
            }

            string swName = param[0].ToString().ToUpper();
            int ret = 0;
            switch (swName)
            {
                case "GOTO":
                    // GOTO x y z
                    if (param.Length < 4)
                    {
                        InsertAlarm("GOTO 命令需要 3 个坐标参数");
                        ret = -2;
                        break;
                    }
                    
                    float x, y, z;
                    if (!float.TryParse(param[1].ToString(), out x))
                    {
                        InsertAlarm("GOTO 命令 X 坐标格式错误:" + param[1]);
                        ret = -2;
                        break;
                    }
                    if (!float.TryParse(param[2].ToString(), out y))
                    {
                        InsertAlarm("GOTO 命令 Y 坐标格式错误:" + param[2]);
                        ret = -2;
                        break;
                    }
                    if (!float.TryParse(param[3].ToString(), out z))
                    {
                        InsertAlarm("GOTO 命令 Z 坐标格式错误:" + param[3]);
                        ret = -2;
                        break;
                    }
                    
                    SetModuleVariable("主平台请求移动", "true");
                    _hub.SetTarget(x, y, z);
                    projectSetting.GotoCount++;
                    SetModuleVariable("移动完成", "true");
                    break;
                case "ORIGIN":
                    _hub.ResetToOrigin();
                    break;
                case "CENTER":
                    _hub.SetToCenter();
                    break;
                case "JOG":
                    // JOG axis direction，例如 JOG X +1
                    if (param.Length < 3)
                    {
                        InsertAlarm("JOG 命令需要轴名和方向参数");
                        ret = -2;
                        break;
                    }
                    ret = JogAxis(param[1].ToString().ToUpper(), Convert.ToInt32(param[2]));
                    break;
                case "SETSPEED":
                    if (param.Length < 2)
                    {
                        InsertAlarm("SETSPEED 命令需要速度参数");
                        ret = -2;
                        break;
                    }
                    _hub.SpeedSetting = Math.Max(0, Math.Min(100, Convert.ToInt32(param[1])));
                    break;
                case "STOP":
                case "ESTOP":
                    StopAll();
                    break;
                default:
                    InsertAlarm("未知命令:" + param[0].ToString());
                    ret = -1;
                    break;
            }
            return ret;
        }

        /// <summary>对指定轴执行一次寸动。</summary>
        private int JogAxis(string axisName, int direction)
        {
            if (direction != 1 && direction != -1)
            {
                InsertAlarm("JOG 方向必须是 +1 或 -1");
                return -2;
            }
            int index;
            switch (axisName)
            {
                case "X": index = 0; break;
                case "Y": index = 1; break;
                case "Z": index = 2; break;
                case "U": index = 3; break;  // U 轴支持
                default:
                    InsertAlarm("未知轴名:" + axisName);
                    return -2;
            }
            // 寸动：按一下走一步；连续模式的停止由 STOP 命令完成
            _jogServices[index].SetMode(JogMode.Incremental);
            _jogServices[index].OnJogStart(direction);
            _jogServices[index].OnJogStop();
            _jogServices[index].SetMode(globalSetting.JogIncremental ? JogMode.Incremental : JogMode.Continuous);
            return 0;
        }

        /// <summary>根据需要设置模组参数，比如 CTQ 参数等。</summary>
        public override bool SetParam(object sKey, object sValue)
        {
            string key = sKey.ToString();
            switch (key)
            {
                case "SpeedSetting":
                    _hub.SpeedSetting = Math.Max(0, Math.Min(100, Convert.ToInt32(sValue)));
                    return true;
                case "JogStep":
                    globalSetting.JogStep = Convert.ToSingle(sValue);
                    ApplyJogSetting();
                    return true;
                case "JogIncremental":
                    globalSetting.JogIncremental = Convert.ToBoolean(sValue);
                    ApplyJogSetting();
                    return true;
                default:
                    SetModuleVariable(key, sValue == null ? "" : sValue.ToString());
                    return true;
            }
        }

        public override object GetParam(object itemName)
        {
            string key = itemName.ToString();
            switch (key)
            {
                case "SpeedSetting": return _hub.SpeedSetting;
                // X/Y/Z/U四轴当前位置
                case "CurrentX": return _hub.X.Current;
                case "CurrentY": return _hub.Y.Current;
                case "CurrentZ": return _hub.Z.Current;
                case "CurrentU": return _hub.U.Current;  // U 轴当前位置
                // X/Y/Z/U四轴目标位置
                case "TargetX": return _hub.X.Target;
                case "TargetY": return _hub.Y.Target;
                case "TargetZ": return _hub.Z.Target;
                case "TargetU": return _hub.U.Target;    // U 轴目标位置
                default: return GetStringVariable(key);
            }
        }

        public override bool StopAll()
        {
            foreach (AxisJogService s in _jogServices)
                s.EmergencyStop();
            // 向硬件下发急停，避免仅冻结本地 Target 而轴仍在运动
            if (_hub != null)
                _hub.EmergencyStop();
            MachineStatus.bSemiProcess = false;
            SetModuleVariable("主平台请求移动", "false");
            return true;
        }

        public override bool ReleaseAlarm()
        {
            bAlarm = false;
            return true;
        }
    }
}
