using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceDefine;
using System.IO;
using MainModule;
using System.Windows.Forms;
namespace ProcessModuleDemo
{
public class ProcessModuleDemo : ProcessModuleBase
{
//ProcessModuleBase默认变量
//public bool bAlarm;
//public bool bInitOK;
//public string processModuleName;
//public TaskItemSetting taskItemSetting;
//System.Windows.Forms.ToolStripStatusLabel tssl;
//protected object lockObj;
    internal RunForm runForm;
    public SettingForm settingForm;

    public DemoGlobalSetting globalSetting;

    public DemoProjectSetting projectSetting;

    public override dynamic FunctionCaller
    {
        get
        {
            return this;
        }
    }

    public override string GetInfo()
    {
        return "工艺模组Demo";
    }

    public override bool Init(string strName)
    {
        try
        {
            processModuleName = strName;
            bInitOK = false;
            LoadSetting();
            if (runForm == null)
                runForm = new RunForm(this);
            
            FormMain.stopButtonClick += FormMainstopButtonClick;
            FormMain.autoManualClick += FormMainautoManualClick;
            FrmManagement.frmAlarm.AlarmOccurred += FrmAlarmAlarmOccurred;
            ProjectManager.openProject += ProjectManageropenProject;
            return bInitOK;
        }
        catch (Exception ex)
        {
            MessageBox.Show(strName + "工艺模组初始化异常" + ex.Message + ex.StackTrace);
            return false;
        }
    }

    #region 注册平台事件
    /// 
    /// 打开项目是重新加载项目文件，初始化工艺模组
    /// 
    /// 
    /// 
    private bool ProjectManageropenProject(string strProjectPath)
    {
        LoadSetting();
        //重新初始化项目
        return bInitOK;
    }
    private void FrmAlarmAlarmOccurred(AlarmItem obj)
    {

    }

    private bool FormMainautoManualClick()
    {
        MachineStatus.bSemiProcess = false;
        return true;
    }

    private bool FormMainstopButtonClick()
    {
        MachineStatus.bSemiProcess = false;
        return true;
    }

    #endregion

    /// 
    /// 重新加载工艺模组
    /// 
    /// 
    public override bool ReOpen()
    {
        //重新加载配置
        LoadSetting();
        //重新初始化项目
        return bInitOK;
    }

    public override bool Close()
    {
        return true;
    }

    public override bool LoadSetting()
    {
        try
        {
            globalSetting = DemoGlobalSetting.Load(processModuleName);
            projectSetting = DemoProjectSetting.Load(processModuleName);

            taskItemSetting = globalSetting.taskItemSetting;
            GetModuleVariable("Demo首次运行",DataType.布尔,"false");
            GetModuleVariable("主平台初始化完成", DataType.布尔, "false");
            GetModuleVariable("Demo初始化完成", DataType.布尔, "false");
            GetModuleVariable("主平台请求AA", DataType.布尔, "false");
            GetModuleVariable("AA完成", DataType.布尔, "false");               
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(processModuleName + $"加载setting发生异常{ex.Message}{ex.StackTrace}");
            globalSetting = DemoGlobalSetting.Load(processModuleName);
            return false;
        }
    }

    /// 
    /// 保存工艺模组参数
    /// 
    /// 
    public override bool Save()
    {
        try
        {
            //
            globalSetting.Save();
            projectSetting.Save();
            //
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存参数时发生异常,{ex.Message}{ex.StackTrace}");
            return false;
        }
        return true;
    }

    /// 
    /// 显示工艺模组运行界面
    /// 
    /// 
    /// 
    public override bool ShowRunForm(Panel panel)
    {
        foreach (Control controlItem in panel.Controls)
        {
            controlItem.Visible = false;
        }
        panel.Controls.Clear();
        if (runForm == null)
            runForm = new RunForm(this);
        
        runForm.TopLevel = false;
        //runForm.Size = panel.Size;
        //panel.AutoScroll = true;
        panel.Controls.Add(runForm);
        runForm.Dock = DockStyle.Fill;
        runForm.Show();
        return true;
    }

    /// 
    /// 显示工艺模组设置界面
    /// 
    /// 
    /// 
    public override bool ShowSettingForm(Panel panel)
    {
        foreach (Control controlItem in panel.Controls)
        {
            controlItem.Visible = false;
        }
        panel.Controls.Clear();
        if (settingForm == null)
            settingForm = new SettingForm(this);
        settingForm.TopLevel = false;
        //settingForm.Size = panel.ClientSize;
        //panel.AutoScroll = true;
        panel.Controls.Add(settingForm);
        settingForm.Dock = DockStyle.Fill;
        settingForm.Show();
        return true;
    }

    /// 
    /// 执行工艺模组相关动作
    /// 
    /// 
    /// 
    public override int Action(params object[] param)
    {
        string swName = param[0].ToString().ToUpper();
        int ret = 0;
        switch (swName)
        {
            case "AA":
                string param1 = param[1].ToString();

                MessageBox.Show("这是工艺模组:"+ processModuleName+",AA命令,"+ param1);
                //执行AA命令动作
                break;
            case "BB":
                MessageBox.Show("这是工艺模组:" + processModuleName + ",BB命令," );
                break;
            case "CC":
                MessageBox.Show("这是工艺模组:" + processModuleName + ",CC命令,");
                break;
            case "DD":
                break;
            case "EE":
                break;
            case "FF":
                break;
            default:
                InsertAlarm("未知命令:" + param[0].ToString());
                ret = -1;
                break;
        }
        return ret;
    }

    /// 
    /// 根据需要设置模组参数，比如CTQ参数等
    /// 
    /// 
    /// 
    /// 
    public override bool SetParam(object sKey, object sValue)
    {
        return true;
    }
    public override object GetParam(object itemName)
    {
        return null;
    }

    public override bool StopAll()
    {
        MachineStatus.bSemiProcess = false;
        return true;

    }

    public override bool ReleaseAlarm()
    {
        bAlarm = false;
        return true;
    }

    /// 
    /// 为模组添加变量
    /// 
    /// 
    /// 
    /// 
    /// 
    private string GetModuleVariable(string varName, DataType dataType, string varValue = "")
    {
        taskItemSetting.dicTaskVariables = new Dictionary();
        taskItemSetting.dicTaskVariables = taskItemSetting.listTaskVariables.ToDictionary(p => p.strName);
        if (!taskItemSetting.dicTaskVariables.ContainsKey(varName))
        {
            TaskVariable TaskVar = new TaskVariable();
            TaskVar.strName = varName;
            TaskVar.strValue = varValue;
            TaskVar.DataType = dataType;
            taskItemSetting.AddNewVariable(TaskVar);
        }
        return GetStringVariable(varName);
    }
}
}