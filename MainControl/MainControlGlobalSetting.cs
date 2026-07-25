using System;
using System.IO;
using InterfaceDefine;
using MainModule;

namespace ProcessModules.MainControl
{
    /// <summary>
    /// 主控制工艺模组全局参数（对应 DOMO 模板 GETSEETING.CS 中的 DemoGlobalSetting）。
    /// 全局参数存放在 AppParam\ProcessModule\模组名 目录，跨项目共享。
    /// </summary>
    public class MainControlGlobalSetting
    {
        public string name;

        // —— 轴范围（与 PointJump/Trajectory 一致：X/Y -100~100，Z/U -50~100）——
        public float XMin = -100f;
        public float XMax = 100f;
        public float YMin = -100f;
        public float YMax = 100f;
        public float ZMin = -50f;
        public float ZMax = 100f;
        public float UMin = -50f;
        public float UMax = 100f;

        /// <summary>速度档位 [0,100]，对应 MainForm.trbSpeed。</summary>
        public int SpeedSetting = 10;

        /// <summary>JOG 寸动步长（mm）。</summary>
        public float JogStep = 1.0f;

        /// <summary>JOG 模式：true = 寸动，false = 连续。</summary>
        public bool JogIncremental = true;

        /// <summary>模组任务项设置（流程变量，对应 DOMO 模板中的 taskItemSetting）。</summary>
        public TaskItemSetting taskItemSetting = new TaskItemSetting();

        public MainControlGlobalSetting()
        {
        }

        /// <summary>
        /// 加载参数 —— 全局参数路径。
        /// </summary>
        /// <param name="strName">工艺模组名称。</param>
        public static MainControlGlobalSetting Load(string strName)
        {
            MainControlGlobalSetting pDoc = null;
            try
            {
                string sDirectory = Path.Combine(
                    InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", strName, "MainControlGlobalSetting.xml");
                pDoc = InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile<MainControlGlobalSetting>(sDirectory);
            }
            catch (Exception eMy)
            {
                pDoc = new MainControlGlobalSetting();
                System.Diagnostics.Debug.WriteLine("ProcessModule" + strName + eMy.Message + eMy.StackTrace);
            }
            finally
            {
                pDoc.name = strName;
            }
            return pDoc;
        }

        /// <summary>
        /// 保存参数 —— 全局参数路径。
        /// </summary>
        public bool Save()
        {
            string sDirectory = Path.Combine(
                InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", name, "MainControlGlobalSetting.xml");
            InterfaceDefine.CommKit.XMLSerializationHelper.SaveToFile(sDirectory, this);
            return true;
        }
    }
}
