---
kind: logging_system
name: 日志系统 — 基于 System.Diagnostics.Debug 的轻量调试输出
category: logging_system
scope:
    - '**'
source_files:
    - Logic/PlatformMotionAdapter.cs
    - MainControl/MainControlGlobalSetting.cs
    - MainControl/MainControlProjectSetting.cs
    - PointJump/PointJumpGlobalSetting.cs
    - PointJump/PointJumpProjectSetting.cs
    - Trajectory/TrajectoryGlobalSetting.cs
    - Trajectory/TrajectoryProjectSetting.cs
---

本仓库未引入任何第三方日志框架（如 NLog、Serilog、log4net 等），也未定义统一的日志抽象或集中式日志配置。代码中的“日志”行为完全依赖 .NET 内置的 `System.Diagnostics.Debug.WriteLine`，仅在 Debug 配置下向 Visual Studio 输出窗口打印错误与调试信息，属于开发期诊断输出，而非生产级日志系统。

**使用方式与分布**
- 运动控制适配器：`Logic/PlatformMotionAdapter.cs` 中在各类运动命令（moveABS、movejump、movego、movehome、Stop、EmergencyStop、SetSpeed）的异常分支处调用 `Debug.WriteLine`，以 `[PlatformMotionAdapter]` 前缀标识来源并附带异常消息。
- 各工艺模组的设置加载失败路径：`MainControl/MainControlGlobalSetting.cs`、`MainControl/MainControlProjectSetting.cs`、`PointJump/PointJumpGlobalSetting.cs`、`PointJump/PointJumpProjectSetting.cs`、`Trajectory/TrajectoryGlobalSetting.cs`、`Trajectory/TrajectoryProjectSetting.cs` 均在捕获异常后通过 `Debug.WriteLine` 输出模块名与堆栈/消息。

**架构与约定**
- 无独立日志类、工厂或配置文件；每个需要输出的位置直接调用 `System.Diagnostics.Debug.WriteLine`。
- 输出内容采用字符串拼接，包含模块/方法名前缀（如 `[PlatformMotionAdapter]`、`ProcessModule`、`MainControlProjectSetting`）和异常消息，便于在输出窗口中筛选。
- 仅用于调试阶段；Release 构建时这些语句仍会编译但不会输出到调试器（取决于宿主是否附加调试器）。

**约束与限制**
- 没有日志级别（Info/Warn/Error 等）划分，所有输出均为同等粒度的调试行。
- 没有结构化字段、没有文件/控制台/远程 sink 路由、没有异步写入或缓冲机制。
- 由于是纯 DLL 库，日志输出依赖宿主进程（上位机平台）附加调试器才能看到，不适合生产环境问题追踪。

综上，该仓库不存在成体系的日志系统，仅以分散的 `Debug.WriteLine` 作为开发期排错手段。