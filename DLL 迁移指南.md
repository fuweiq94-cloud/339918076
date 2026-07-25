# ProcessModules DLL 迁移指南

## 概述

本文档说明如何将 ProcessModules 项目从"自包含模式"（使用临时替代文件）迁移到"真实 DLL 引用模式"（引用 InterfaceDefine.dll 和 MainModule.dll）。

## 当前状态

- **自包含模式**：ProcessModules 内部包含 6 个临时替代文件，模拟外部 DLL 的接口定义
- **目标模式**：移除临时文件，直接引用真实的 InterfaceDefine.dll 和 MainModule.dll

## 迁移步骤（等待 DLL 提供后执行）

### 第 1 步：准备 DLL 文件

1. 将 `InterfaceDefine.dll` 复制到 `d:\zm\C#\XyzController\ProcessModules\` 目录
2. 将 `MainModule.dll` 复制到 `d:\zm\C#\XyzController\ProcessModules\` 目录

### 第 2 步：修改 ProcessModules.csproj

#### 删除以下 6 个 Compile 引用（搜索文件名即可找到）：

```xml
<!-- 删除这 6 行 -->
<Compile Include="MachineStatus.cs" />
<Compile Include="ModuleAlarmEventArgs.cs" />
<Compile Include="ModuleVariables.cs" />
<Compile Include="ProcessModuleBase.cs" />
<Compile Include="ProcessModuleEnvironment.cs" />
<Compile Include="XmlSerializationHelper.cs" />
```

#### 在 `<ItemGroup>` 的 Reference 区域添加真实 DLL 引用：

在现有的 System、System.Core 等引用之后添加：

```xml
<Reference Include="InterfaceDefine">
  <HintPath>InterfaceDefine.dll</HintPath>
</Reference>
<Reference Include="MainModule">
  <HintPath>MainModule.dll</HintPath>
</Reference>
```

### 第 3 步：确认命名空间兼容性

确保以下 using 语句仍然有效（DLL 中的命名空间应与临时文件一致）：

```csharp
using InterfaceDefine;   // ProcessModuleBase, ProcessModuleEnvironment
using MainModule;        // ModuleAlarmEventArgs, DataType, TaskItemSetting
using ProcessModules;    // ProcessModuleManager, IMotionService
```

如果 DLL 的命名空间有变化，需要全局搜索替换这些 using 语句。

### 第 4 步：编译验证

```powershell
cd d:\zm\C#\XyzController\ProcessModules
& "D:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" ProcessModules.sln /p:Configuration=Debug
```

检查是否有编译错误：
- `找不到类型 xxx` → 命名空间或引用路径问题
- `程序集加载失败` → DLL 版本不兼容或缺失

### 第 5 步：运行测试

1. 重新编译 DOMOPlatform
2. 启动 DOMOPlatform.exe
3. 切换到"主控制工艺模组"标签页
4. 测试以下功能：
   - 实时坐标显示
   - JOG 控制
   - 预设点位保存/跳转
   - 轴限位设置弹窗打开
   - XYView 上菱形标记显示

## 待删除的文件列表

当 DLL 集成成功后，可以手动删除以下文件（已不再需要）：

- `ProcessModules\MachineStatus.cs`
- `ProcessModules\ModuleAlarmEventArgs.cs`（注意：如果在 DLL 中没有重复定义则保留）
- `ProcessModules\ModuleVariables.cs`
- `ProcessModules\ProcessModuleBase.cs`
- `ProcessModules\ProcessModuleEnvironment.cs`
- `ProcessModules\XmlSerializationHelper.cs`

## 已知依赖关系

### ProcessModuleBase.cs（将被替换）

从 DOMO.CS 分析，InterfaceDefine.dll 中的 `ProcessModuleBase` 类包含：

**字段**（public，可直接访问）：
- `bool bAlarm` - 报警状态
- `bool bInitOK` - 初始化完成标志
- `string processModuleName` - 模组名称
- `TaskItemSetting taskItemSetting` - 流程变量集

**抽象属性/方法**（必须重写）：
- `abstract dynamic FunctionCaller { get; }` - 供脚本调用的对象
- `abstract string GetInfo()` - 模组描述信息
- `abstract bool Init(string strName)` - 初始化
- `abstract bool ReOpen()` - 重新加载
- `abstract bool Close()` - 关闭
- `abstract bool LoadSetting()` - 加载参数
- `abstract bool Save()` - 保存参数
- `abstract bool ShowRunForm(Panel panel)` - 显示运行界面
- `abstract bool ShowSettingForm(Panel panel)` - 显示设置界面
- `abstract int Action(params object[] param)` - 执行命令（param[0] = 命令名如"AA"/"BB"等）
- `abstract bool SetParam(object key, object value)` - 设置参数
- `abstract object GetParam(object itemName)` - 读取参数
- `abstract bool StopAll()` - 停止所有运动
- `abstract bool ReleaseAlarm()` - 释放报警

**虚拟方法**（可选重写）：
- `virtual void SetMotionService(IMotionService service)` - 注入运动服务（默认空实现）

**受保护辅助方法**：
- `protected void InsertAlarm(string message)` - 插入报警并触发 AlarmOccurred 事件
- `protected string GetModuleVariable(varName, dataType, varValue)` - 添加/读取流程变量
- `protected string GetStringVariable(varName)` - 读取字符串变量

**检查点**：确保 InterfaceDefine.dll 中的 `ProcessModuleBase`类包含以上所有成员。

### ProcessModuleEnvironment.cs（将被替换）

目前提供的成员：
- `static string AppParamPath()`
- `static string CurrentProjectPath { get; set; }`
- `static event OpenProjectHandler OpenProject`
- `static bool SwitchProject(string strProjectPath)`

**检查点**：确保 MainModule.dll 中的 `ProcessModuleEnvironment` 或等效静态类包含以上成员。

### XmlSerializationHelper.cs（可能被替换）

目前提供的静态方法：
- `ReadFromFile<T>(string path)`
- `SaveToFile<T>(string path, T instance)`

**检查点**：确认 DLL 中是否有替代的序列化 helper，如果没有则可能需要保留此文件。

### ModuleVariables.cs → TaskItemSetting + TaskVariable

从 DOMO.CS 第 18、77-85 行可知：

**核心类型在 MainModule 命名空间中**：
```csharp
public class TaskItemSetting
{
    public List<TaskVariable> listTaskVariables;
    public Dictionary<string, TaskVariable> dicTaskVariables;  // strName 键索引
    
    public void AddNewVariable(TaskVariable tv) { ... }
    public void RebuildDictionary() { ... }
}

public class TaskVariable
{
    public string strName;       // 变量名
    public string strValue;      // 变量值
    public DataType DataType;    // 数据类型枚举
}

public enum DataType
{
    布尔，   // true/false
    字符串，  // text
    整数，   // int
    浮点，   // float
    // ... 其他类型
}
```

**迁移建议**：`GetModuleVariable` / `GetStringVariable` 方法的实现逻辑保持不变，只是引用来源变为 DLL。

## 兼容性保障

### 保持不变的代码

以下代码在迁移后应无需修改：

1. **工艺模组逻辑层**（MainControl/PointJump/Trajectory 中的 .cs 文件）
   - `MainControlProcessModule.Init()`
   - `ShowRunForm()`
   - `Action()` 命令处理
   - `SetParam()/GetParam()` 参数读写

2. **业务核心层**（Logic/*.cs）
   - `XyzControllerHub`
   - `AxisController`
   - `AxisJogService`
   - `IMotionService`及其实现

3. **UI 层**（RunForm/*.cs）
   - 事件绑定逻辑
   - UI 刷新逻辑
   - 用户交互处理

### 可能需要调整的代码

如果发现以下编译错误，说明 DLL 与临时文件实现不完全一致：

1. **缺少 protected 辅助方法**：
   ```csharp
   // 错误：无法访问受保护的 InsertAlarm 方法
   InsertAlarm("错误消息");
   ```
   **解决方案**：如果 DLL 没有提供这些辅助方法，可以保留对应的临时文件但注释掉它们的导出。

2. **字段访问权限差异**：
   ```csharp
   // 错误：taskItemSetting 是私有的
   taskItemSetting.dicTaskVariables.Add(...);
   ```
   **解决方案**：确认 DLL 中的字段访问修饰符。

## 回滚方案

如果迁移后发现严重问题，可以快速回滚到自包含模式：

1. 撤销 ProcessModules.csproj 的修改
2. 恢复 6 个临时替代文件
3. 删除新复制的 DLL 文件
4. 重新编译

## 联系支持

遇到问题时请提供：
- 编译错误的完整截图或文本
- DLL 的版本信息
- 具体的报错位置和代码片段
