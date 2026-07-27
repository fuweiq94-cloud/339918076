---
kind: build_system
name: MSBuild + .csproj 多 DLL 工艺模组构建系统
category: build_system
scope:
    - '**'
source_files:
    - ProcessModules.sln
    - ProcessModules.csproj
    - MainControl/MainControl.csproj
    - PointJump/PointJump.csproj
    - Trajectory/Trajectory.csproj
    - 独立 DLL 构建说明.md
---

本项目使用 Visual Studio / MSBuild（.NET Framework 4.6.1，旧式非 SDK 风格 .csproj）构建一个包含三个独立工艺模块的解决方案，每个模块输出独立的 DLL，供上位机平台通过反射加载。

**构建系统与工具链**
- 构建工具：MSBuild（ToolsVersion 14.0，对应 Visual Studio 2015），通过 `$(MSBuildToolsPath)\Microsoft.CSharp.targets` 引入 C# 编译目标
- 解决方案：`ProcessModules.sln` 管理三个子项目，支持 Debug/Release 两种配置，平台均为 Any CPU
- 语言与框架：C#（LangVersion=latest），目标框架 .NET Framework 4.6.1，启用确定性构建（Deterministic=true）
- 可选 CLI：文档中提供 `dotnet build ProcessModules.sln -c Release` 作为备选方式

**核心构建产物**
- `PointJump.dll` — 点位跳转工艺模块（命名空间 PointJumpProcessModule，类 PointJumpProcessModule）
- `MainControl.dll` — 主控制工艺模块（命名空间 MainControlProcessModule，类 MainControlProcessModule）
- `Trajectory.dll` — 轨迹查看工艺模块（命名空间 TrajectoryViewProcessModule，类 TrajectoryViewProcessModule）
- 根目录 `ProcessModules.csproj` 为合并项目，将全部源文件编译进单一 `ProcessModules.dll`（历史遗留，当前以三个独立 DLL 为主）

**共享代码组织策略**
三个子项目通过 `<Compile Include="..\Logic\...">` + `<Link>` 元素引用上级目录的公共 Logic/Controls 资源，实现“源代码一份、各 DLL 独立编译”的模式。每个 DLL 均包含完整的 Logic/Controls 副本，避免运行时依赖。

**构建约定与约束**
- 每个 DLL 仅包含一个 FunctionCaller（反调接口），这是平台扫描的硬性要求，由《独立 DLL 构建说明.md》明确约束
- 命名空间与类名必须完全一致（如 `PointJumpProcessModule.PointJumpProcessModule`），以便平台通过反射定位
- 输出路径固定为各子项目 `bin\Debug\` 或 `bin\Release\`，DLL 需复制到平台插件目录（如 `./Plugins/`）后由平台自动扫描加载
- 资源文件（.resx、.bmp 图标）通过 `<EmbeddedResource>` 嵌入到对应 DLL 中

**典型构建命令**
```bash
msbuild ProcessModules.sln /p:Configuration=Release
msbuild PointJump\PointJump.csproj /p:Configuration=Release
```

**无 CI/Docker 配置**：仓库未包含 GitHub Actions、Jenkins、Dockerfile 等自动化流水线文件，构建主要依赖本地 Visual Studio 或 MSBuild 命令行。