---
kind: external_dependency
name: 平台核心模块 MainModule.dll
slug: mainmodule-dll
category: external_dependency
category_hints:
    - vendor_identity
    - client_constraint
scope:
    - '**'
source_files:
    - ProcessModules.csproj
---

### 平台核心模块
- **角色**: 上位机平台的核心运行时模块，提供 TaskItemSetting、TaskVariable、DataType、MachineStatus 等业务类型
- **集成点**: 工艺模块通过 AppParam 和 ProjectManager 访问平台配置和项目管理功能
- **使用模式**: 作为外部依赖被引用，但本地开发环境不提供，需等待平台部署时获取
- **约束**: 与 InterfaceDefine.dll 配合使用，共同构成平台接口体系
- **现状**: 本地编译无法通过是正常的，需要平台环境才能完整构建