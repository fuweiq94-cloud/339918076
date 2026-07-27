---
kind: configuration_system
name: XML 分层配置系统（全局设置与项目设置）
category: configuration_system
scope:
    - '**'
source_files:
    - DOMO/GETSEETING.CS
    - MainControl/MainControlGlobalSetting.cs
    - MainControl/MainControlProjectSetting.cs
    - PointJump/PointJumpGlobalSetting.cs
    - PointJump/PointJumpProjectSetting.cs
    - Trajectory/TrajectoryGlobalSetting.cs
    - Trajectory/TrajectoryProjectSetting.cs
---

本仓库采用基于 XML 序列化的双层配置体系：全局设置（GlobalSetting）与项目设置（ProjectSetting），通过统一的 `InterfaceDefine.CommKit.XMLSerializationHelper` 进行读写，并由 `InterfaceDefine.AppParam` 和 `MainModule.ProjectManager` 提供路径解析。

### 1. 配置分层与存储位置
- **全局设置**（跨项目共享）：存放于 `AppParam\ProcessModule\<模组名>` 目录，例如 `MainControlGlobalSetting.xml`、`PointJumpGlobalSetting.xml`、`TrajectoryGlobalSetting.xml`。由 `InterfaceDefine.AppParam.AppParamPath()` 解析根路径。
- **项目设置**（随项目切换）：存放于 `<当前项目路径>\<模组名>` 目录，例如 `MainControlProjectSetting.xml`、`PointJumpProjectSetting.xml`、`TrajectoryProjectSetting.xml`。由 `MainModule.ProjectManager.projectSetting.strProjectPath` 提供项目根路径。

### 2. 核心类与职责
- **GlobalSetting 类**（每个模组一个）：定义模组级默认参数（轴范围 X/Y -100~100、Z/U -50~100、速度档位 SpeedSetting、JOG 步长 JogStep、是否显示轨迹 ShowTrail 等），提供静态 `Load(strName)` 加载与实例 `Save()` 保存方法。
- **ProjectSetting 类**（每个模组一个）：定义项目级运行时数据（上次目标坐标 LastTargetX/Y/Z、累计命令计数 GotoCount/JumpCount/ClearTrailCount、预设点位列表 Presets 等），同样提供 Load/Save 方法。
- **DOMO 模板**（`GETSEETING.CS` / `MAINMODUO.CS`）：演示如何继承该模式实现新模组的配置类。

### 3. 序列化机制
所有配置均通过 `InterfaceDefine.CommKit.XMLSerializationHelper.ReadFromFile<T>()` 与 `SaveToFile(path, obj)` 完成 XML 反序列化/序列化，异常时回退到默认对象并记录调试日志。

### 4. 设计约定
- 每个工艺模组必须提供一对 `<模组名>GlobalSetting.cs` 与 `<模组名>ProjectSetting.cs`。
- GlobalSetting 字段包含轴限位、速度、JOG 模式等运行期可调参数；ProjectSetting 字段包含持久化业务状态（如预设点、计数器）。
- 加载失败时静默返回默认实例，保证模组可启动。
- 保存前自动创建目录（ProjectSetting.Save 中显式 `Directory.CreateDirectory`）。