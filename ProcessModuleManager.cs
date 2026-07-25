using System;
using System.Collections.Generic;
using ProcessModules.MainControl;
using ProcessModules.PointJump;
using ProcessModules.Trajectory;

namespace ProcessModules
{
    /// <summary>
    /// 工艺模组管理器：统一注册、初始化、停止、保存所有工艺模组。
    /// 对应 DOMO 模板中主平台对工艺模组的集中管理。
    /// </summary>
    public static class ProcessModuleManager
    {
        private static readonly Dictionary<string, ProcessModuleBaseEx> _modules =
            new Dictionary<string, ProcessModuleBaseEx>(System.StringComparer.OrdinalIgnoreCase);

        private static bool _initialized;

        public static IEnumerable<KeyValuePair<string, ProcessModuleBaseEx>> Modules
        {
            get { return _modules; }
        }

        public static ProcessModuleBaseEx Get(string name)
        {
            ProcessModuleBaseEx m;
            if (_modules.TryGetValue(name, out m))
                return m;
            return null;
        }

        public static bool InitAll()
        {
            if (_initialized)
                return true;

            bool ok = true;
            ok = RegisterAndInit(new MainControlProcessModule(), "MainControl") && ok;
            ok = RegisterAndInit(new PointJumpProcessModule(), "PointJump") && ok;
            ok = RegisterAndInit(new TrajectoryViewProcessModule(), "TrajectoryView") && ok;
            _initialized = ok;
            return ok;
        }

        public static bool RegisterAndInit(ProcessModuleBaseEx module, string name)
        {
            _modules[name] = module;
            return module.Init(name);
        }

        public static bool StopAll()
        {
            bool ok = true;
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                ok = kv.Value.StopAll() && ok;
            return ok;
        }

        public static bool SaveAll()
        {
            bool ok = true;
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                ok = kv.Value.Save() && ok;
            return ok;
        }

        public static bool CloseAll()
        {
            bool ok = true;
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                ok = kv.Value.Close() && ok;
            _initialized = false;
            return ok;
        }

        public static void InjectServiceToAll(IMotionService service)
        {
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                kv.Value.SetMotionService(service);
        }

        public static void SubscribeAlarms(EventHandler<ModuleAlarmEventArgs> handler)
        {
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                kv.Value.AlarmOccurred += handler;
        }

        public static void UnsubscribeAlarms(EventHandler<ModuleAlarmEventArgs> handler)
        {
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in _modules)
                kv.Value.AlarmOccurred -= handler;
        }
    }
}
