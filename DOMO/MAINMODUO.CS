using MainModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProcessModuleDemo
{
public class DemoProjectSetting
{
public string Name;
    public int MyProperty { get; set; } = 5;
    public int MyProperty1 { get; set; } = 6;
    public int MyProperty2 { get; set; } = 7;
    public int MyProperty3 { get; set; } = 8;
    public static DemoProjectSetting Load(string actionerName)
    {
        DemoProjectSetting pDoc = null;
        string sDirectory = Path.Combine(ProjectManager.projectSetting.strProjectPath, actionerName, "DemoProjectSetting.xml");
        try
        {
            pDoc = InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile(sDirectory);
        }
        catch
        {
            pDoc = new DemoProjectSetting();
        }
        pDoc.Name = actionerName;
        return pDoc;
    }
    public bool Save()
    {
        string sDirectory = Path.Combine(ProjectManager.projectSetting.strProjectPath, Name);
        if (!Directory.Exists(sDirectory))
        {
            Directory.CreateDirectory(sDirectory);
        }
        sDirectory = Path.Combine(sDirectory, "DemoProjectSetting.xml");
        InterfaceDefine.CommKit.XMLSerializationHelper.SaveToFile(sDirectory, this);
        return true;
    }

}
}