using System;
using System.Windows.Forms;
using InterfaceDefine;
using MainModule;

namespace ProcessModules.PointJump
{
    /// <summary>
    /// 点位跳转工艺模组（对应 DOMO.CS 中的 ProcessModuleDemo）。
    /// 继承 ProcessModuleBaseEx（平台 ProcessModuleBase + 本库扩展），
    /// 封装点位管理与跳转业务：
    /// 持有 XyzControllerHub（业务层）与预设点位列表（项目参数），
    /// 运行界面为 RunForm（DOMO 模式：new RunForm(this)，界面与模组共享同一份数据）。
    /// </summary>
    /// <remarks>
    /// 支持的平台命令（Action）：
    ///   GOTO x y z          → 跳转到指定坐标
    ///   GOTOPOINT name      → 跳转到指定预设点位
    ///   SAVEPOINT name      → 把当前目标保存为预设点位
    ///   DELETEPOINT name    → 删除指定预设点位
    ///   SETSPEED value      → 设置速度档位 [0,100]
    ///   STOP                → 冻结所有轴目标到当前值
    /// </remarks>
    public class PointJumpProcessModule : ProcessModuleBaseEx
    {
        internal RunForm runForm;
        public ModuleSettingForm settingForm;

        public PointJumpGlobalSetting globalSetting;
        public PointJumpProjectSetting projectSetting;

        // —— 业务核心：与运行界面共享同一个 Hub ——
        private XyzControllerHub _hub;

        /// <summary>模组持有的 XYZ 控制器（集成现有业务层）。</summary>
        public XyzControllerHub Hub { get { return _hub; } }

        /// <summary>注入/替换后端运动控制服务（转发到内部 Hub，供上位机平台对接）。</summary>
        public override void SetMotionService(IMotionService service)
        {
            if (_hub != null)
                _hub.SetService(service);
        }

        public override dynamic FunctionCaller
        {
            get { return this; }
        }

        public override string GetInfo()
        {
            return "点位跳转工艺模组";
        }

        public override bool Init(string strName)
        {
            try
            {
                processModuleName = strName;
                bInitOK = false;
                LoadSetting();

                // 创建业务层（轴范围与速度取自全局参数）
                // null = 后端服务尚未接入（待引入真实 DLL 后通过 SetService 注入）
                _hub = new XyzControllerHub(null,
                    globalSetting.XMin, globalSetting.XMax,
                    globalSetting.YMin, globalSetting.YMax,
                    globalSetting.ZMin, globalSetting.ZMax,
                    globalSetting.UMin, globalSetting.UMax);
                _hub.SpeedSetting = globalSetting.SpeedSetting;

                // 首次运行时项目参数为空，补一组默认点位
                if (projectSetting.Presets.Count == 0)
                    AddDefaultPresets();

                // 注册平台事件：打开项目时重新加载项目参数（对齐 DOMO：ProjectManager.openProject）
                ProjectManager.openProject += OnOpenProject;

                bInitOK = true;
                SetModuleVariable("点位跳转初始化完成", "true");
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
            if (projectSetting.Presets.Count == 0)
                AddDefaultPresets();
            return bInitOK;
        }

        #endregion

        /// <summary>补一组默认预设点位（对应 PointJumpForm.AddDefaultPresets）。</summary>
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
                globalSetting = PointJumpGlobalSetting.Load(processModuleName);
                projectSetting = PointJumpProjectSetting.Load(processModuleName);

                taskItemSetting = globalSetting.taskItemSetting;
                GetModuleVariable("点位跳转首次运行", DataType.布尔, "false");
                GetModuleVariable("点位跳转初始化完成", DataType.布尔, "false");
                GetModuleVariable("主平台请求跳转", DataType.布尔, "false");
                GetModuleVariable("跳转完成", DataType.布尔, "false");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(processModuleName + "加载setting发生异常" + ex.Message + ex.StackTrace);
                globalSetting = PointJumpGlobalSetting.Load(processModuleName);
                return false;
            }
        }

        /// <summary>保存工艺模组参数。</summary>
        public override bool Save()
        {
            try
            {
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

        /// <summary>将全局参数中的轴范围应用到 Hub 和运行界面。</summary>
        private void ApplyRanges()
        {
            if (_hub != null)
                _hub.SetAxisRanges(
                    globalSetting.XMin, globalSetting.XMax,
                    globalSetting.YMin, globalSetting.YMax,
                    globalSetting.ZMin, globalSetting.ZMax,
                    globalSetting.UMin, globalSetting.UMax);
            if (runForm != null && !runForm.IsDisposed)
                runForm.ApplyRanges();
        }

        /// <summary>显示工艺模组运行界面（DOMO 模式：RunForm 持有模组引用）。</summary>
        public override bool ShowRunForm(Panel panel)
        {
            foreach (Control controlItem in panel.Controls)
            {
                controlItem.Visible = false;
            }
            panel.Controls.Clear();
            if (runForm == null || runForm.IsDisposed)
                runForm = new RunForm(this);

            runForm.TopLevel = false;
            panel.Controls.Add(runForm);
            runForm.Dock = DockStyle.Fill;
            runForm.Show();
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
                    
                    SetModuleVariable("主平台请求跳转", "true");
                    _hub.SetTarget(x, y, z);
                    projectSetting.JumpCount++;
                    SetModuleVariable("跳转完成", "true");
                    break;
                case "GOTOPOINT":
                    // GOTOPOINT name
                    if (param.Length < 2)
                    {
                        InsertAlarm("GOTOPOINT 命令需要点位名称参数");
                        ret = -2;
                        break;
                    }
                    ret = GotoPreset(param[1].ToString());
                    break;
                case "SAVEPOINT":
                    // SAVEPOINT name：把当前目标保存为预设
                    if (param.Length < 2)
                    {
                        InsertAlarm("SAVEPOINT 命令需要点位名称参数");
                        ret = -2;
                        break;
                    }
                    SavePreset(param[1].ToString());
                    break;
                case "DELETEPOINT":
                    // DELETEPOINT name
                    if (param.Length < 2)
                    {
                        InsertAlarm("DELETEPOINT 命令需要点位名称参数");
                        ret = -2;
                        break;
                    }
                    ret = DeletePreset(param[1].ToString());
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
                    StopAll();
                    break;
                default:
                    InsertAlarm("未知命令:" + param[0].ToString());
                    ret = -1;
                    break;
            }
            return ret;
        }

        /// <summary>跳转到指定名称的预设点位。</summary>
        private int GotoPreset(string name)
        {
            PresetPoint pt = FindPreset(name);
            if (pt == null)
            {
                InsertAlarm("预设点位不存在:" + name);
                return -2;
            }
            SetModuleVariable("主平台请求跳转", "true");
            _hub.SetTarget(pt.X, pt.Y, pt.Z);
            projectSetting.JumpCount++;
            SetModuleVariable("跳转完成", "true");
            return 0;
        }

        /// <summary>把当前目标保存为预设点位（同名则覆盖坐标）。</summary>
        private void SavePreset(string name)
        {
            PresetPoint pt = FindPreset(name);
            if (pt == null)
            {
                projectSetting.Presets.Add(new PresetPoint(name, _hub.X.Target, _hub.Y.Target, _hub.Z.Target));
            }
            else
            {
                pt.X = _hub.X.Target;
                pt.Y = _hub.Y.Target;
                pt.Z = _hub.Z.Target;
            }
            if (runForm != null && !runForm.IsDisposed)
                runForm.RefreshPresets();
        }

        /// <summary>删除指定名称的预设点位。</summary>
        private int DeletePreset(string name)
        {
            PresetPoint pt = FindPreset(name);
            if (pt == null)
            {
                InsertAlarm("预设点位不存在:" + name);
                return -2;
            }
            projectSetting.Presets.Remove(pt);
            if (runForm != null && !runForm.IsDisposed)
                runForm.RefreshPresets();
            return 0;
        }

        /// <summary>按名称查找预设点位（不区分大小写）。</summary>
        private PresetPoint FindPreset(string name)
        {
            foreach (PresetPoint pt in projectSetting.Presets)
            {
                if (string.Equals(pt.Name, name, StringComparison.OrdinalIgnoreCase))
                    return pt;
            }
            return null;
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
                case "PresetCount": return projectSetting.Presets.Count;
                case "JumpCount": return projectSetting.JumpCount;
                case "CurrentX": return _hub.X.Current;
                case "CurrentY": return _hub.Y.Current;
                case "CurrentZ": return _hub.Z.Current;
                default: return GetStringVariable(key);
            }
        }

        public override bool StopAll()
        {
            // 向硬件下发急停，并冻结本地目标
            if (_hub != null)
                _hub.EmergencyStop();
            MachineStatus.bSemiProcess = false;
            SetModuleVariable("主平台请求跳转", "false");
            return true;
        }

        public override bool ReleaseAlarm()
        {
            bAlarm = false;
            return true;
        }
    }
}
