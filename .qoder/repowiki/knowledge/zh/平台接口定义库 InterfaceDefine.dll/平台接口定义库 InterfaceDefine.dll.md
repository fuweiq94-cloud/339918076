---
kind: external_dependency
name: 平台接口定义库 InterfaceDefine.dll
slug: interfacedefine-dll
category: external_dependency
category_hints:
    - vendor_identity
    - client_constraint
scope:
    - '**'
source_files:
    - ProcessModules.csproj
    - PointJump.csproj
    - MainControl.csproj
    - Trajectory.csproj
---

### 平台接口定义库
- **角色**: 上位机平台提供的核心接口定义，包含 ProcessModuleBase 抽象基类、IProcessModule 接口、AppParam、ProjectManager、CommKit.XMLSerializationHelper 等类型
- **集成点**: 三个工艺模块（PointJump、MainControl、Trajectory）均通过继承 ProcessModuleBase 实现平台反调接口
- **使用模式**: 模块必须实现 FunctionCaller 属性返回自身实例，供平台通过反射调用 Action/SetParam/GetParam 等方法
- **约束**: 本地开发环境缺少此 DLL 导致编译失败是预期行为，运行时由平台提供
- **依赖关系**: 所有工艺模块直接依赖此库，无中间层