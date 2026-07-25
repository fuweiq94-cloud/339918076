using System;
using System.Collections.Generic;
using System.IO;
using InterfaceDefine;

namespace ProcessModules.PointJump
{
    /// <summary>
    /// 点位跳转工艺模组项目参数。
    /// 项目参数存放在 当前项目路径\模组名 目录，随项目切换而更换。
    /// 预设点位属于项目数据，随项目参数一起持久化。
    /// </summary>
    public class PointJumpProjectSetting
    {
        public string Name;

        /// <summary>预设点位列表（与 PointJumpForm 共享同一实例）。</summary>
        public List<PresetPoint> Presets = new List<PresetPoint>();

        /// <summary>本项目中累计执行跳转命令的次数。</summary>
        public int JumpCount;

        public PointJumpProjectSetting()
        {
        }

        /// <summary>
        /// 加载参数 —— 项目参数路径。
        /// </summary>
        /// <param name="actionerName">工艺模组名称。</param>
        public static PointJumpProjectSetting Load(string actionerName)
        {
            PointJumpProjectSetting pDoc = null;
            string sDirectory = Path.Combine(
                MainModule.ProjectManager.projectSetting.strProjectPath, actionerName, "PointJumpProjectSetting.xml");
            try
            {
                pDoc = InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile<PointJumpProjectSetting>(sDirectory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PointJumpProjectSetting 加载失败:" + ex.Message);
                pDoc = new PointJumpProjectSetting();
            }
            pDoc.Name = actionerName;
            if (pDoc.Presets == null)
                pDoc.Presets = new List<PresetPoint>();
            return pDoc;
        }

        /// <summary>
        /// 保存参数 —— 项目参数路径。
        /// </summary>
        public bool Save()
        {
            string sDirectory = Path.Combine(MainModule.ProjectManager.projectSetting.strProjectPath, Name);
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            sDirectory = Path.Combine(sDirectory, "PointJumpProjectSetting.xml");
            InterfaceDefine.CommKit.XMLSerializationHelper.SaveToFile(sDirectory, this);
            return true;
        }
    }
}
