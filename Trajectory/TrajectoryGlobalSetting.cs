using System;
using System.IO;
using InterfaceDefine;
using MainModule;

namespace ProcessModules.Trajectory
{
    /// <summary>
    /// 轨迹查看工艺模组全局参数。
    /// 全局参数存放在 AppParam\ProcessModule\模组名 目录，跨项目共享。
    /// </summary>
    public class TrajectoryGlobalSetting
    {
        public string name;

        // —— 轴范围（与 TrajectoryViewForm 设计器范围一致：X/Y -100~100，Z -50~100）——
        public float XMin = -100f;
        public float XMax = 100f;
        public float YMin = -100f;
        public float YMax = 100f;
        public float ZMin = -50f;
        public float ZMax = 100f;
        public float UMin = -50f;
        public float UMax = 100f;

        /// <summary>速度档位 [0,100]，对应 TrajectoryViewForm.trbSpeed。</summary>
        public int SpeedSetting = 10;

        /// <summary>是否默认显示轨迹。</summary>
        public bool ShowTrail = true;

        /// <summary>模组任务项设置（流程变量）。</summary>
        public TaskItemSetting taskItemSetting = new TaskItemSetting();

        public TrajectoryGlobalSetting()
        {
        }

        /// <summary>
        /// 加载参数 —— 全局参数路径。
        /// </summary>
        /// <param name="strName">工艺模组名称。</param>
        public static TrajectoryGlobalSetting Load(string strName)
        {
            TrajectoryGlobalSetting pDoc = null;
            try
            {
                string sDirectory = Path.Combine(
                    InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", strName, "TrajectoryGlobalSetting.xml");
                pDoc = InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile<TrajectoryGlobalSetting>(sDirectory);
            }
            catch (Exception eMy)
            {
                pDoc = new TrajectoryGlobalSetting();
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
                InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", name, "TrajectoryGlobalSetting.xml");
            InterfaceDefine.CommKit.XMLSerializationHelper.SaveToFile(sDirectory, this);
            return true;
        }
    }
}
