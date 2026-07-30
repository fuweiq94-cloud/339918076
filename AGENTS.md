# AGENTS.md

This file provides guidance to Qoder (qoder.com) when working with code in this repository.

## Build Commands

```bash
# 构建整个解决方案（三个独立模块 DLL）
msbuild ProcessModules.sln /p:Configuration=Debug

# 构建单个模块
msbuild PointJump\PointJump.csproj /p:Configuration=Debug
msbuild MainControl\MainControl.csproj /p:Configuration=Debug
msbuild Trajectory\Trajectory.csproj /p:Configuration=Debug

# Release 构建
msbuild ProcessModules.sln /p:Configuration=Release
```

- 目标框架：.NET Framework 4.6.1，旧式（非 SDK 风格）.csproj
- 无测试项目、无 lint 配置
- 输出路径：各模块 `bin\Debug\` 或 `bin\Release\`

## Architecture Overview

本项目是**工艺模组类库**，运行于上位机平台软件中。平台通过反射加载各模组 DLL，模组通过平台提供的外部 DLL 与主程序交互。

### 平台-模组双向调用架构

```
平台软件（主程序）
  ├── InterfaceDefine.dll  → 提供 ProcessModuleBase 基类、AppParam、XMLSerializationHelper
  ├── MainModule.dll       → 提供 TaskItemSetting、TaskVariable、DataType、MachineStatus、ProjectManager
  └── 反射加载模组 DLL     → 通过 "命名空间.类名" 字符串实例化模组
```

**关键约束**：`InterfaceDefine.dll` 和 `MainModule.dll` 不在本仓库中，编译时缺少这些引用导致的错误是**预期行为**，不可自行实现替代。

### 模组反射加载契约

平台通过完整类型名字符串加载模组，因此：

- **命名空间 = 类名 = 模块名 + "ProcessModule"**
- PointJump → `PointJumpProcessModule.PointJumpProcessModule`
- MainControl → `MainControlProcessModule.MainControlProcessModule`
- Trajectory → `TrajectoryViewProcessModule.TrajectoryViewProcessModule`

违反此约定将导致平台无法加载模组。

### 解决方案结构

`.sln` 包含三个**独立 DLL 项目**，每个模组完全解耦（无共享状态、无相互引用）：

| 项目 | 输出 DLL | 命名空间 | 功能 |
|------|----------|----------|------|
| PointJump/ | PointJump.dll | PointJumpProcessModule | 点位跳转 |
| MainControl/ | MainControl.dll | MainControlProcessModule | XYZ 三轴手动控制 |
| Trajectory/ | Trajectory.dll | TrajectoryViewProcessModule | 轨迹查看 |

根目录不再有合并项目，三个模组完全独立：

### 每个模组的完整结构

```rnModuleName/
├── Logic/                      ← 运动控制核心业务层
│   ├── AxisController.cs       ← 单轴状态管理
│   ├── AxisJogService.cs       ← JOG 寸动服务
│   ├── AxisPosition.cs         ← 位置数据结构
│   ├── IMotionService.cs       ← 硬件抽象接口
│   ├── JogMode.cs              ← JOG 模式枚举
│   ├── MotionCommand.cs        ← 统一指令结构
│   ├── PlatformMotionAdapter.cs
│   ├── PlatformMotionService.cs
│   └── XyzControllerHub.cs     ← XYZU四轴统一控制器
├── Controls/                   ← 自定义 WinForms 控件
│   ├── XYView.cs               ← XY 平面视图
│   ├── ZBarView.cs             ← Z 轴条状视图
│   ├── DroLabel.cs             ← DRO 数码管标签
│   ├── JogButton.cs            ← JOG 操作按钮
│   ├── MathHelper.cs           ← 数学工具类
│   └── PaintHelper.cs          ← 绘图工具类
├── Resources/                  ← 资源位图
│   ├── XYView.bmp
│   ├── ZBarView.bmp
│   ├── DroLabel.bmp
│   └── JogButton.bmp
├── ModuleNameProcessModule.cs  ← 继承 ProcessModuleBase，模组入口
├── ModuleNameGlobalSetting.cs  ← 全局参数（XML 序列化）
├── ModuleNameProjectSetting.cs ← 项目参数（XML 序列化）
├── RunForm.cs                  ← 运行界面（嵌入平台 Panel 显示）
└── RunForm.Designer.cs         ← 设计器文件
```

### ProcessModuleBase 生命周期（必须 override 的方法）

`Init(strName)` → `LoadSetting()` → `ShowRunForm(panel)` / `ShowSettingForm(panel)` → `Action(params)` → `Save()` → `StopAll()` → `Close()`

### 运动控制分层

```
UI (RunForm / UnifiedRunForm)
  ↓ 只与 Hub 交互
XyzControllerHub（业务中间层，持有四轴 AxisController）
  ↓ 通过 MotionCommand 统一下发
IMotionService（硬件抽象接口）
  ↓ 实现
PlatformMotionAdapter（桥接平台 DLL 的 moveABS/movejump/movego/movehoome）
```

- 前端严禁使用模拟位置数据，`Current` 值全部来自后端 `PositionUpdated` 事件推送
- 替换后端只需注入不同 `IMotionService` 实现，Hub 和 UI 无需修改

### 扁平化架构优势

- **完全独立**：每个模组包含完整的 Logic、Controls、Resources 代码，无需外部依赖
- **易于维护**：修改某模组代码时，不会影响其他模组
- **便于分发**：可以单独编译、测试、部署任意模组
- **清晰边界**：每个模组的职责边界清晰，无隐式共享状态

### 共享代码机制变更

**之前**：根目录`Logic/`和`Controls/`文件夹被三个模组通过`<Compile Include="..\Logic\...">`共享

**现在**：每个模组在自己的子目录中包含完整的 Logic 和 Controls 代码副本，.csproj 直接引用本地路径

```xml
<!-- MainControl.csproj -->
<Compile Include="Logic\AxisController.cs" />
<Compile Include="Controls\XYView.cs" />
```

## Critical Rules

1. **DOMO/ 目录是平台提供的参考模板**，其中对 InterfaceDefine/MainModule 的引用编译不通过是正常的，不可删除或自行实现闭环
2. **所有模组必须直接继承 `InterfaceDefine.dll` 中的 `ProcessModuleBase`**，禁止创建中间扩展基类
3. **禁止 override 平台未定义的方法**（如 `SetMotionService`），`ProcessModuleBase` 的 API 以外部 DLL 为准
4. **模组间完全解耦**：禁止共享状态、禁止相互依赖、每个模组独立实现设置和界面
5. **旧式 C# 语法约束**：工控机编译器不支持 Lambda（`=>`），使用命名方法 + 委托构造
