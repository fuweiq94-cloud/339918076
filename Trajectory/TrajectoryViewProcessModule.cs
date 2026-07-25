using System;
using System.Windows.Forms;
using InterfaceDefine;
using MainModule;

namespace ProcessModules.Trajectory
{
    /// <summary>
    /// 轨迹查看工艺模组（对应 DOMO.CS 中的 ProcessModuleDemo）。
    /// 继承 ProcessModuleBaseEx（平台 ProcessModuleBase + 本库扩展），
    /// 封装轨迹观察与记录业务：
    /// 持有 XyzControllerHub（业务层），
    /// 运行界面为 RunForm（DOMO 模式：new RunForm(this)，共享业务状态）。
    /// </summary>
    /// <remarks>
    /// 支持的平台命令（Action）：
    ///   GOTO x y z       → 三轴移动到指定目标（同时记录轨迹）
    ///   ORIGIN / CENTER  → 回原点 / 回中心
    ///   RANDOM           → 随机目标（演示用）
    ///   CLEARTRAIL       → 清除轨迹
    ///   SHOWTRAIL on/off → 打开 / 关闭轨迹显示
    ///   SETSPEED value   → 设置速度档位 [0,100]
    ///   STOP             → 冻结所有轴目标到当前值
    /// </remarks>
    public class TrajectoryViewProcessModule : ProcessModuleBaseEx
    {
        internal RunForm runForm;
        public ModuleSettingForm settingForm;

        public TrajectoryGlobalSetting globalSetting;
        public TrajectoryProjectSetting projectSetting;

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
            return "轨迹查看工艺模组";
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

                // 注册平台事件：打开项目时重新加载项目参数（对齐 DOMO：ProjectManager.openProject）
                ProjectManager.openProject += OnOpenProject;

                bInitOK = true;
                SetModuleVariable("轨迹查看初始化完成", "true");
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

        /// <summary>重新加载工艺模组。</summary>
        public override bool ReOpen()
        {
            LoadSetting();
            if (_hub != null)
                _hub.SpeedSetting = globalSetting.SpeedSetting;
            if (runForm != null && !runForm.IsDisposed)
                runForm.ShowTrail = globalSetting.ShowTrail;
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
                globalSetting = TrajectoryGlobalSetting.Load(processModuleName);
                projectSetting = TrajectoryProjectSetting.Load(processModuleName);

                taskItemSetting = globalSetting.taskItemSetting;
                GetModuleVariable("轨迹查看首次运行", DataType.布尔, "false");
                GetModuleVariable("轨迹查看初始化完成", DataType.布尔, "false");
                GetModuleVariable("轨迹记录中", DataType.布尔, globalSetting.ShowTrail ? "true" : "false");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(processModuleName + "加载setting发生异常" + ex.Message + ex.StackTrace);
                globalSetting = TrajectoryGlobalSetting.Load(processModuleName);
                return false;
            }
        }

        /// <summary>保存工艺模组参数。</summary>
        public override bool Save()
        {
            try
            {
                // 把界面当前状态回写到参数对象
                if (globalSetting != null && _hub != null)
                    globalSetting.SpeedSetting = _hub.SpeedSetting;
                if (runForm != null && !runForm.IsDisposed)
                {
                    globalSetting.ShowTrail = runForm.ShowTrail;
                    projectSetting.LastTrailPointCount = runForm.TrailPointCount;
                }

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
                    if (param.Length < 4)
                    {
                        InsertAlarm("GOTO 命令需要 3 个坐标参数");
                        ret = -2;
                        break;
                    }
                    _hub.SetTarget(Convert.ToSingle(param[1]), Convert.ToSingle(param[2]), Convert.ToSingle(param[3]));
                    break;
                case "ORIGIN":
                    _hub.ResetToOrigin();
                    break;
                case "CENTER":
                    _hub.SetToCenter();
                    break;
                case "RANDOM":
                    _hub.SetRandomTarget(new Random());
                    break;
                case "CLEARTRAIL":
                    ClearTrail();
                    break;
                case "SHOWTRAIL":
                    // SHOWTRAIL on/off
                    if (param.Length < 2)
                    {
                        InsertAlarm("SHOWTRAIL 命令需要 on/off 参数");
                        ret = -2;
                        break;
                    }
                    SetShowTrail(param[1].ToString());
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

        /// <summary>清除轨迹（运行界面打开时同步清除视图上的轨迹）。</summary>
        private void ClearTrail()
        {
            if (runForm != null && !runForm.IsDisposed)
                runForm.ClearTrail();
            projectSetting.ClearTrailCount++;
        }

        /// <summary>打开 / 关闭轨迹显示。</summary>
        private void SetShowTrail(string onOff)
        {
            bool show = onOff.Equals("on", StringComparison.OrdinalIgnoreCase)
                || onOff.Equals("true", StringComparison.OrdinalIgnoreCase)
                || onOff == "1";
            globalSetting.ShowTrail = show;
            if (runForm != null && !runForm.IsDisposed)
                runForm.ShowTrail = show;
            SetModuleVariable("轨迹记录中", show ? "true" : "false");
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
                case "ShowTrail":
                    SetShowTrail(sValue == null ? "" : sValue.ToString());
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
                case "ShowTrail": return globalSetting.ShowTrail;
                case "TrailPointCount":
                    return (runForm != null && !runForm.IsDisposed) ? runForm.TrailPointCount : 0;
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
            return true;
        }

        public override bool ReleaseAlarm()
        {
            bAlarm = false;
            return true;
        }
    }
}
