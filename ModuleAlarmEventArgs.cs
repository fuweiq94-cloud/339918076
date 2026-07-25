using System;

namespace ProcessModules
{
    /// <summary>
    /// 工艺模组报警事件参数（本库扩展，供 ProcessModuleManager.SubscribeAlarms 使用）。
    /// 平台原生报警走 MainModule.FrmManagement.frmAlarm / AlarmItem；
    /// 本类型用于模组级 AlarmOccurred 事件，不依赖平台是否提供同名类型。
    /// </summary>
    public class ModuleAlarmEventArgs : EventArgs
    {
        /// <summary>产生报警的模组名称。</summary>
        public string ModuleName { get; private set; }

        /// <summary>报警内容。</summary>
        public string Message { get; private set; }

        /// <summary>报警发生时间。</summary>
        public DateTime Time { get; private set; }

        public ModuleAlarmEventArgs(string moduleName, string message)
        {
            ModuleName = moduleName;
            Message = message;
            Time = DateTime.Now;
        }
    }
}
