using MainModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace ProcessModuleDemo
{
public class DemoGlobalSetting
{
public string name;
public int MyProperty { get; set; } = 1;
    public int MyProperty1 { get; set; } = 2;

    public int MyProperty2 { get; set; } = 3;

    public TaskItemSetting taskItemSetting = new TaskItemSetting();

    public DemoGlobalSetting()
    {
    }

    /// 
    /// 加载参数--全局参数路径
    /// 
    /// 工艺模组名称
    /// 
    public static DemoGlobalSetting Load(string strName)
    {
        DemoGlobalSetting pDoc = null;
        try
        {
            string sDirectory = Path.Combine(InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", strName, "DemoGlobalSetting.xml");
            pDoc = InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile(sDirectory);
        }
        catch (Exception eMy)
        {
            pDoc = new DemoGlobalSetting();
            FrmManagement.frmAlarm.InsertAlarmMessage("ProcessModule" + strName + eMy.Message + eMy.StackTrace);
        }
        finally
        {
            pDoc.name = strName;
        }
        return pDoc;
    }

    /// 
    /// 保存参数----全局参数路径
    /// 
    /// 
    public bool Save()
    {
        string sDirectory = Path.Combine(InterfaceDefine.AppParam.AppParamPath(), "ProcessModule", _name, "DemoGlobalSetting.xml");
        InterfaceDefine.CommKit.XMLSerializationHelper.SaveToFile(sDirectory, this);
        return true;
    }
}
}