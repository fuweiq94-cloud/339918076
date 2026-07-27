# 独立 DLL 构建配置说明

## 📋 背景

### 问题

平台软件在扫描 DLL 时，**如果发现单个 DLL 文件中包含多个反调接口（FunctionCaller）会触发警报**。

原来的 `ProcessModules.csproj` 会将三个工艺模块编译到一个 DLL 中：
```
ProcessModules.dll
├── PointJumpProcessModule.PointJumpProcessModule (FunctionCaller)
├── MainControlProcessModule.MainControlProcessModule (FunctionCaller)
└── TrajectoryViewProcessModule.TrajectoryViewProcessModule (FunctionCaller)
```

❌ **问题**: 一个 DLL 包含 3 个 FunctionCaller，平台会报错！

---

## ✅ 解决方案

将三个工艺模块分别打包成**独立的 DLL 文件**，确保每个 DLL 只包含一个反调接口。

### 新的构建输出

```
PointJump.dll
└── PointJumpProcessModule.PointJumpProcessModule (FunctionCaller) ✅

MainControl.dll
└── MainControlProcessModule.MainControlProcessModule (FunctionCaller) ✅

Trajectory.dll
└── TrajectoryViewProcessModule.TrajectoryViewProcessModule (FunctionCaller) ✅
```

✅ **每个 DLL 只包含 1 个 FunctionCaller，平台不会报错！**

---

## 📦 三个独立项目

### 1️⃣ PointJump 项目

**项目文件**: [`PointJump/PointJump.csproj`](file://d:\zm\ProcessModules\PointJump\PointJump.csproj)

**输出**: `PointJump.dll`

**包含的文件**:
- PointJumpProcessModule.cs (主类)
- PointJumpGlobalSetting.cs
- PointJumpProjectSetting.cs
- RunForm.cs / RunForm.Designer.cs
- Logic/ (公共业务层)
- Controls/ (公共控件)
- PresetPoint.cs

**命名空间**: `PointJumpProcessModule`  
**类名**: `PointJumpProcessModule`  
**完整类型名**: `PointJumpProcessModule.PointJumpProcessModule`

---

### 2️⃣ MainControl 项目

**项目文件**: [`MainControl/MainControl.csproj`](file://d:\zm\ProcessModules\MainControl\MainControl.csproj)

**输出**: `MainControl.dll`

**包含的文件**:
- MainControlProcessModule.cs (主类)
- MainControlGlobalSetting.cs
- MainControlProjectSetting.cs
- RunForm.cs / RunForm.Designer.cs
- UnifiedRunForm.cs / UnifiedRunForm.Designer.cs
- AxisLimitForm.cs / AxisLimitForm.Designer.cs
- Logic/ (公共业务层，包含 AxisJogService)
- Controls/ (公共控件，包含 JogButton)
- PresetPoint.cs

**命名空间**: `MainControlProcessModule`  
**类名**: `MainControlProcessModule`  
**完整类型名**: `MainControlProcessModule.MainControlProcessModule`

---

### 3️⃣ Trajectory 项目

**项目文件**: [`Trajectory/Trajectory.csproj`](file://d:\zm\ProcessModules\Trajectory\Trajectory.csproj)

**输出**: `Trajectory.dll`

**包含的文件**:
- TrajectoryViewProcessModule.cs (主类)
- TrajectoryGlobalSetting.cs
- TrajectoryProjectSetting.cs
- RunForm.cs / RunForm.Designer.cs
- Logic/ (公共业务层)
- Controls/ (公共控件)

**命名空间**: `TrajectoryViewProcessModule`  
**类名**: `TrajectoryViewProcessModule`  
**完整类型名**: `TrajectoryViewProcessModule.TrajectoryViewProcessModule`

---

## 🏗️ 解决方案结构

**解决方案文件**: [`ProcessModules.sln`](file://d:\zm\ProcessModules\ProcessModules.sln)

```
ProcessModules.sln
├── PointJump/PointJump.csproj → PointJump.dll
├── MainControl/MainControl.csproj → MainControl.dll
└── Trajectory/Trajectory.csproj → Trajectory.dll
```

---

## 🔧 编译方式

### 方式 1：使用 Visual Studio

1. 打开 `ProcessModules.sln`
2. 选择编译配置（Debug 或 Release）
3. 点击"生成" → "生成解决方案"
4. 输出 3 个独立的 DLL

### 方式 2：使用 MSBuild 命令行

```bash
# 编译所有项目
msbuild ProcessModules.sln /p:Configuration=Release

# 只编译 PointJump
msbuild PointJump\PointJump.csproj /p:Configuration=Release

# 只编译 MainControl
msbuild MainControl\MainControl.csproj /p:Configuration=Release

# 只编译 Trajectory
msbuild Trajectory\Trajectory.csproj /p:Configuration=Release
```

### 方式 3：使用 dotnet CLI（如果支持）

```bash
dotnet build ProcessModules.sln -c Release
```

---

## 📂 输出路径

### Debug 模式

```
PointJump/bin/Debug/PointJump.dll
MainControl/bin/Debug/MainControl.dll
Trajectory/bin/Debug/Trajectory.dll
```

### Release 模式

```
PointJump/bin/Release/PointJump.dll
MainControl/bin/Release/MainControl.dll
Trajectory/bin/Release/Trajectory.dll
```

---

## 🎯 平台调用方式

### 方式 1：分别加载三个 DLL

```csharp
// 加载 PointJump.dll
Assembly pointJumpAsm = Assembly.LoadFrom("PointJump.dll");
Type pointJumpType = pointJumpAsm.GetType("PointJumpProcessModule.PointJumpProcessModule");
IProcessModule pointJumpModule = (IProcessModule)Activator.CreateInstance(pointJumpType);
pointJumpModule.Init("PointJump");

// 加载 MainControl.dll
Assembly mainControlAsm = Assembly.LoadFrom("MainControl.dll");
Type mainControlType = mainControlAsm.GetType("MainControlProcessModule.MainControlProcessModule");
IProcessModule mainControlModule = (IProcessModule)Activator.CreateInstance(mainControlType);
mainControlModule.Init("MainControl");

// 加载 Trajectory.dll
Assembly trajectoryAsm = Assembly.LoadFrom("Trajectory.dll");
Type trajectoryType = trajectoryAsm.GetType("TrajectoryViewProcessModule.TrajectoryViewProcessModule");
IProcessModule trajectoryModule = (IProcessModule)Activator.CreateInstance(trajectoryType);
trajectoryModule.Init("Trajectory");
```

### 方式 2：扫描目录自动加载

```csharp
string pluginDir = "./Plugins";
string[] dllFiles = Directory.GetFiles(pluginDir, "*.dll");

foreach (string dllFile in dllFiles)
{
    Assembly asm = Assembly.LoadFrom(dllFile);
    
    // 查找继承自 ProcessModuleBase 的类
    var moduleTypes = asm.GetTypes()
        .Where(t => t.IsSubclassOf(typeof(ProcessModuleBase)))
        .ToList();
    
    foreach (Type moduleType in moduleTypes)
    {
        IProcessModule module = (IProcessModule)Activator.CreateInstance(moduleType);
        module.Init(moduleType.Name.Replace("ProcessModule", ""));
        
        Console.WriteLine($"已加载模块：{module.GetInfo()}");
    }
}
```

### 方式 3：配置文件指定

```json
{
  "modules": [
    {
      "name": "PointJump",
      "dll": "PointJump.dll",
      "type": "PointJumpProcessModule.PointJumpProcessModule"
    },
    {
      "name": "MainControl",
      "dll": "MainControl.dll",
      "type": "MainControlProcessModule.MainControlProcessModule"
    },
    {
      "name": "Trajectory",
      "dll": "Trajectory.dll",
      "type": "TrajectoryViewProcessModule.TrajectoryViewProcessModule"
    }
  ]
}
```

---

## 📊 对比表格

| 项目 | 原方案（单 DLL） | 新方案（3 个 DLL） |
|------|-----------------|-------------------|
| **输出文件** | ProcessModules.dll | PointJump.dll<br>MainControl.dll<br>Trajectory.dll |
| **FunctionCaller 数量** | 3 个 ❌ | 每个 DLL 1 个 ✅ |
| **平台扫描** | 触发警报 ❌ | 正常识别 ✅ |
| **部署灵活性** | 必须全部加载 | 可按需加载 ✅ |
| **文件大小** | 较大 | 较小 ✅ |
| **维护成本** | 低 | 中 |

---

## ⚙️ 公共代码处理

### 问题

三个模块都需要使用公共的业务层（Logic/）和控件（Controls/），如何避免代码重复？

### 解决方案：使用文件链接

在每个 .csproj 中，使用 `<Link>` 元素引用上级目录的公共文件：

```xml
<Compile Include="..\Logic\AxisController.cs">
  <Link>Logic\AxisController.cs</Link>
</Compile>
<Compile Include="..\Controls\XYView.cs">
  <Link>Controls\XYView.cs</Link>
</Compile>
```

**优点**:
- ✅ 源代码只有一份，易于维护
- ✅ 每个 DLL 独立编译，不依赖其他 DLL
- ✅ 平台加载时不需要额外的依赖

**缺点**:
- ⚠️ 每个 DLL 都包含公共代码的副本（编译后）
- ⚠️ 如果公共代码有 bug，需要重新编译所有模块

---

## 🔍 验证清单

编译完成后，请验证：

### 1. 输出文件

- [ ] PointJump.dll 已生成
- [ ] MainControl.dll 已生成
- [ ] Trajectory.dll 已生成

### 2. FunctionCaller 数量

使用反射检查每个 DLL：

```csharp
// 检查 PointJump.dll
Assembly asm = Assembly.LoadFrom("PointJump.dll");
var callers = asm.GetTypes()
    .Where(t => t.GetProperty("FunctionCaller") != null)
    .ToList();
Console.WriteLine($"PointJump.dll 包含 {callers.Count} 个 FunctionCaller");
// 应该输出：PointJump.dll 包含 1 个 FunctionCaller ✅

// 检查 MainControl.dll
asm = Assembly.LoadFrom("MainControl.dll");
callers = asm.GetTypes()
    .Where(t => t.GetProperty("FunctionCaller") != null)
    .ToList();
Console.WriteLine($"MainControl.dll 包含 {callers.Count} 个 FunctionCaller");
// 应该输出：MainControl.dll 包含 1 个 FunctionCaller ✅

// 检查 Trajectory.dll
asm = Assembly.LoadFrom("Trajectory.dll");
callers = asm.GetTypes()
    .Where(t => t.GetProperty("FunctionCaller") != null)
    .ToList();
Console.WriteLine($"Trajectory.dll 包含 {callers.Count} 个 FunctionCaller");
// 应该输出：Trajectory.dll 包含 1 个 FunctionCaller ✅
```

### 3. 命名空间与类名一致性

- [ ] PointJump.dll: 命名空间 == 类名 ✅
- [ ] MainControl.dll: 命名空间 == 类名 ✅
- [ ] Trajectory.dll: 命名空间 == 类名 ✅

### 4. 平台测试

- [ ] 平台扫描不报错
- [ ] 平台能正确识别三个模块
- [ ] 平台能正确调用每个模块的 FunctionCaller

---

## 📝 部署指南

### 步骤 1：编译

```bash
msbuild ProcessModules.sln /p:Configuration=Release
```

### 步骤 2：复制 DLL

将以下文件复制到平台的插件目录：

```
PointJump/bin/Release/PointJump.dll → ./Plugins/
MainControl/bin/Release/MainControl.dll → ./Plugins/
Trajectory/bin/Release/Trajectory.dll → ./Plugins/
```

### 步骤 3：配置平台

在平台配置文件中注册模块：

```json
{
  "plugins": [
    "PointJump.dll",
    "MainControl.dll",
    "Trajectory.dll"
  ]
}
```

### 步骤 4：启动平台

平台会自动扫描并加载这三个 DLL，每个 DLL 只包含一个 FunctionCaller，不会触发警报。

---

## 🎉 总结

通过本次修改：

✅ **每个 DLL 只包含 1 个 FunctionCaller** - 平台不会报错  
✅ **命名空间与类名完全一致** - 平台可以通过反射轻松找到模块  
✅ **三个模块完全独立** - 可以按需加载和部署  
✅ **公共代码通过文件链接共享** - 易于维护  

**核心原则**: 
> 一个 DLL = 一个工艺模块 = 一个 FunctionCaller

---

**配置时间**: 2026-07-26  
**输出文件**: PointJump.dll、MainControl.dll、Trajectory.dll  
**影响范围**: 三个工艺模块的构建和部署方式
