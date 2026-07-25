using System;
using System.Linq;
using InterfaceDefine;
using MainModule;

namespace ProcessModules
{
    /// <summary>
    /// 扩展基类：继承平台 ProcessModuleBase，补齐 DLL 中没有的成员。
    /// 三个业务模组继承本类即可，不要直接继承 ProcessModuleBase。
    /// </summary>
    public abstract class ProcessModuleBaseEx : ProcessModuleBase
    {
        public event EventHandler<ModuleAlarmEventArgs> AlarmOccurred;

        public virtual void SetMotionService(IMotionService service) { }

        // 对齐 DOMO：InsertAlarm 写报警标志；额外触发本库 AlarmOccurred 供 Manager 订阅
        protected new void InsertAlarm(string message)
        {
            bAlarm = true;
            var h = AlarmOccurred;
            if (h != null)
                h(this, new ModuleAlarmEventArgs(processModuleName, message));
        }

        // 对齐 DOMO.CS 中的 GetModuleVariable
        protected string GetModuleVariable(string varName, DataType dataType, string varValue = "")
        {
            taskItemSetting.dicTaskVariables = taskItemSetting.listTaskVariables.ToDictionary(p => p.strName);
            if (!taskItemSetting.dicTaskVariables.ContainsKey(varName))
            {
                TaskVariable taskVar = new TaskVariable();
                taskVar.strName = varName;
                taskVar.strValue = varValue;
                taskVar.DataType = dataType;
                taskItemSetting.AddNewVariable(taskVar);
            }
            return GetStringVariable(varName);
        }

        protected string GetStringVariable(string varName)
        {
            if (taskItemSetting == null || taskItemSetting.dicTaskVariables == null)
                return "";
            TaskVariable v;
            if (taskItemSetting.dicTaskVariables.TryGetValue(varName, out v) && v != null)
                return v.strValue ?? "";
            return "";
        }

        protected void SetModuleVariable(string varName, string varValue)
        {
            GetModuleVariable(varName, DataType.字符串, varValue);
            taskItemSetting.dicTaskVariables[varName].strValue = varValue;
        }
    }
}
