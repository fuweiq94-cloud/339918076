# ProcessModules DLL 迁移检查清单（基于 DOMO 代码分析）

## 一、DLL 接口预测确认（明天 DLL 到位后核对）

### InterfaceDefine.dll 应包含：

| 类型 | 预期位置/命名空间 | DOMO 代码依据 |
|------|------------------|--------------|
| `ProcessModuleBase` (abstract class) | `InterfaceDefine` | DOMO.CS 第 12 行 |
| `IMotionService` (interface) | `InterfaceDefine` | 工艺模组已有 |
| `AppParam` (static class) | `InterfaceDefine` | GETSEETING.CS 第 35 行 |
| `CommKit.XMLSerializationHelper` (static class) | `InterfaceDefine.CommKit` | GETSEETING.CS 第 36 行 |
| AlarmItem 类 | `InterfaceDefine` | DOMO.CS 第 76 行 |

### MainModule.dll 应包含：

| 类型 | 预期位置/命名空间 | DOMO 代码依据 |
|------|------------------|--------------|
| `ProjectManager` (static class) | `MainModule` | MAINMODUO.CS 第 20 行 |
| `TaskItemSetting` (class) | `MainModule` | DOMO.CS 第 19、119 行 |
| `TaskVariable` (class) | `MainModule` | DOMO.CS 第 81 行 |
| `DataType` (enum) | `MainModule` | DOMO.CS 第 120 行 |
| `MachineStatus` (static class) | `MainModule` | DOMO.CS 第 83、257 行 |
| `AlarmOccurred` (event) | `MainModule.FrmManagement.frmAlarm` | DOMO.CS 第 53 行 |

---

## 二、待删除的临时文件（共 6 个）

当 DLL 引用成功后，手动删除以下文件：

- ✅ `ProcessModuleBase.cs` - 已被 InterfaceDefine.ProcessModuleBase 替代
- ✅ `ProcessModuleEnvironment.cs` - 拆分为 AppParam + ProjectManager
- ✅ `XmlSerializationHelper.cs` - 移入 InterfaceDefine.CommKit 命名空间
- ✅ `ModuleVariables.cs` - TaskItemSetting + TaskVariable 移入 MainModule
- ✅ `MachineStatus.cs` - 简化为 MainModule.MachineStatus 静态类
- ⚠️ `ModuleAlarmEventArgs.cs` - **需要确认**：如果 DLL 中有重复定义则删除，否则保留

---

## 三、需要修改的引用路径

### 在 csproj 中：

#### 替换 1: XML 序列化 Helper

```xml
<!-- 删除 -->
<Compile Include="XmlSerializationHelper.cs" />

<!-- 改为使用 DLL 中的版本 -->
<!-- 所有代码中需将 -->
XmlSerializationHelper.ReadFromFile<T>(...) 
<!-- 改为 -->
InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile<T>(...)
```

#### 添加 DLL 引用：

```xml
<ItemGroup>
  <Reference Include="InterfaceDefine">
    <HintPath>InterfaceDefine.dll</HintPath>
  </Reference>
  <Reference Include="MainModule">
    <HintPath>MainModule.dll</HintPath>
  </Reference>
</ItemGroup>
```

---

## 四、需要全局替换的代码模式

### MachineStatus → MainModule.MachineStatus

搜索并替换所有出现 `MachineStatus.bSemiProcess` 的地方：

```csharp
// 原代码（DOMOPlatform\SimulatedMotionService.cs 等）
MachineStatus.bSemiProcess = false;

// 改为
MainModule.MachineStatus.bSemiProcess = false;
```

### AppParam.Path() + ProjectManager.projectSetting

如果需要保留 ProcessModuleEnvironment.cs 作为兼容层，则内部实现应为：

```csharp
namespace ProcessModules // 或 InterfaceDefine
{
    /// <summary>
    /// 兼容性包装层：内部调用真实的 AppParam 和 ProjectManager。
    /// （可选：如果希望保持向后兼容可保留此文件）
    /// </summary>
    public static class ProcessModuleEnvironment
    {
        public static string AppParamPath()
        {
            return InterfaceDefine.AppParam.AppParamPath();
        }

        public static string CurrentProjectPath
        {
            get { return MainModule.ProjectManager.projectSetting.strProjectPath; }
            set { MainModule.ProjectManager.projectSetting.strProjectPath = value; }
        }

        public static event OpenProjectHandler OpenProject;
        
        public delegate bool OpenProjectHandler(string path);

        public static bool SwitchProject(string path)
        {
            CurrentProjectPath = path;
            if (OpenProject != null)
                return OpenProject(path);
            return true;
        }
    }
}
```

---

## 五、编译后可能遇到的错误及解决方案

### 错误 1：找不到类型 'ProcessModuleBase'

**原因**：using 语句缺失或 DLL 引用未生效  
**解决**：
```csharp
using InterfaceDefine;
```

### 错误 2：缺少受保护的 InsertAlarm 方法

**原因**：DLL 中该方法可能是 protected 无法直接访问  
**解决**：如果当前代码有直接使用，改为触发事件：
```csharp
if (AlarmOccurred != null)
    AlarmOccurred(this, new ModuleAlarmEventArgs(processModuleName, "消息"));
bAlarm = true;
```

### 错误 3：dicTaskVariables 字段不可访问

**原因**：DLL 中可能是 private 字段  
**解决**：使用公开方法 RebuildDictionary()：
```csharp
taskItemSetting.RebuildDictionary();
var exists = taskItemSetting.dicTaskVariables.ContainsKey(varName);
```

---

## 六、验证步骤（DLL 集成后执行）

### Step 1: 编译 ProcessModules

```powershell
cd d:\zm\C#\XyzController\ProcessModules
& "D:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" ProcessModules.sln /p:Configuration=Debug
```

预期结果：成功编译无错误

### Step 2: 检查引用的程序集

打开生成的 `ProcessModules.dll`，用 ILSpy 或 Reflector 查看：
- 是否引用了 `InterfaceDefine` 程序集
- 是否引用了 `MainModule` 程序集

### Step 3: 运行 DOMOPlatform

```powershell
# 先确保 ProcessModules.dll 已更新到 bin\Debug
Stop-Process -Name "DOMOPlatform" -Force
Start-Process "d:\zm\C#\XyzController\DOMOPlatform\bin\Debug\DOMOPlatform.exe"
```

测试功能：
1. ✅ 切换到"主控制工艺模组"标签页
2. ✅ 看到 UnifiedRunForm 统一界面
3. ✅ XYView 上显示菱形标记（原点/中心/A工位/B工位）
4. ✅ 点击"轴限位设置"按钮能打开弹窗
5. ✅ JOG 按钮工作正常
6. ✅ 预设点位能保存/跳转

### Step 4: 对比 DOMO 行为一致性

| 功能点 | DOMO 模板表现 | ProcessModules 表现 | 是否一致 |
|--------|--------------|-------------------|---------|
| Init 时读取 globalSetting | DemoGlobalSetting.Load() | MainControlGlobalSetting.Load() | ✅ |
| LoadSetting 时初始化变量 | GetModuleVariable() | GetModuleVariable() | ✅ |
| Alarm 事件触发方式 | AlarmOccurred 事件 | AlarmOccurred 事件 | ✅ |
| ShowRunForm 嵌入 Panel | TopLevel=false, Dock=Fill | TopLevel=false, Dock=Fill | ✅ |
| Action 命令解析 | switch(param[0]) | switch(param[0]) | ✅ |

---

## 七、回滚方案

如果迁移失败，撤销以下操作：

1. 恢复 git commit：`git reset --hard HEAD~1`
2. 重新编译 ProcessModules（使用临时文件）
3. 删除新复制的 InterfaceDefine.dll 和 MainModule.dll

---

## 八、联系支持

提供以下信息以获得更精准的帮助：
1. ✅ DLL 文件名（如 InterfaceDefine.dll v2.3.1）
2. ✅ 编译错误的完整截图
3. ✅ 具体报错的.cs 文件名和行号
4. ✅ 预期的 vs. 实际的命名空间对比

---

**最后更新时间**: 2026-07-22  
**依据版本**: DOMO 模板代码（DOMO.CS / GETSEETING.CS / MAINMODUO.CS）
