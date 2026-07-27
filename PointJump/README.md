# PointJump 点位跳转工艺模块

## 模块概述

PointJump 是一个独立的工艺模块，专门用于**预设点位管理和坐标跳转控制**。该模块可单独部署使用，不依赖于任何其他工艺模块。

## 功能特性

### 核心功能
- ✅ 预设点位管理（添加、删除、加载）
- ✅ 快速坐标跳转（GOTO 命令）
- ✅ 预设点位调用（GOTOPOINT 命令）
- ✅ 当前目标保存（SAVEPOINT 命令）
- ✅ 速度档位调节 [0-100]
- ✅ 轴范围限制保护

### 支持的命令

```
GOTO <x> <y> <z>          → 跳转到指定坐标
GOTOPOINT <name>          → 跳转到指定预设点位
SAVEPOINT <name>          → 保存当前目标为预设点位
DELETEPOINT <name>        → 删除预设点位
SETSPEED <value>          → 设置速度档位 [0,100]
STOP                      → 停止所有运动
```

### 示例用法

#### 跳转到绝对坐标
```csharp
// 通过平台调用
module.Action("GOTO", "50.0", "30.0", "20.0");
```

#### 跳转到预设点位
```csharp
// 调用预设点位"原点"
module.Action("GOTOPOINT", "原点");

// 调用预设点位"A 工位"
module.Action("GOTOPOINT", "A 工位");
```

#### 保存当前位置
```csharp
// 将当前的 XYZ 目标保存为新点位"C 工位"
module.Action("SAVEPOINT", "C 工位");
```

## 模块组成

### 核心类

| 类名 | 描述 |
|------|------|
| `PointJumpProcessModule` | 主模块类，实现平台接口 |
| `PointJumpGlobalSetting` | 全局配置（轴范围、速度等） |
| `PointJumpProjectSetting` | 项目配置（预设点位列表、统计信息等） |
| `RunForm` | 运行界面 |
| `PresetPoint` | 预设点位数据结构 |

### 依赖组件

#### 业务逻辑层
- `XyzControllerHub`: XYZ 轴控制器（可从 ProcessModules.Common 引用）
- `AxisController`: 单轴控制器
- `AxisPosition`: 位置数据容器

#### UI 控件（可选）
- `DroLabel`: 数字读数显示
- `XYView`: XY 平面轨迹视图
- `ZBarView`: Z 轴高度显示

## 配置说明

### 全局设置 (PointJumpGlobalSetting)

存储以下参数:
- **轴范围**: XMin, XMax, YMin, YMax, ZMin, ZMax, UMin, UMax
- **速度档位**: SpeedSetting (0-100)
- **任务变量**: TaskItemSetting（系统变量）

配置文件路径：`./Config/PointJump_GlobalSettings.json`

### 项目设置 (PointJumpProjectSetting)

存储以下参数:
- **预设点位列表**: Presets（点位数组）
- **跳转次数**: JumpCount（统计信息）

配置文件路径：`./Config/PointJump_ProjectSettings.json`

### 预设点位数据结构

```csharp
public class PresetPoint
{
    public string Name { get; set; }      // 点位名称
    public float X { get; set; }          // X 坐标
    public float Y { get; set; }          // Y 坐标
    public float Z { get; set; }          // Z 坐标
}
```

默认点位:
```
原点：(0, 0, 0)
中心：(0, 0, 50)
A 工位：(30, 40, 10)
B 工位：(-50, 60, 20)
```

## 使用方法

### 1. 在项目中引用

确保项目引用了必要的 DLL:
```xml
<References>
    <Reference Include="InterfaceDefine">
        <!-- 平台接口定义 -->
    </Reference>
    <Reference Include="MainModule">
        <!-- 平台核心模块 -->
    </Reference>
    <Reference Include="ProcessModules.Common">
        <!-- 公共业务逻辑和控件库 -->
    </Reference>
</References>
```

### 2. 初始化模块

```csharp
using ProcessModules.PointJump;

// 创建模块实例
var module = new PointJumpProcessModule();

// 初始化（strName 必须与文件名一致，例如"PointJump"）
bool initSuccess = module.Init("PointJump");

if (!initSuccess)
{
    MessageBox.Show("模块初始化失败!");
    return;
}
```

### 3. 显示运行界面

```csharp
// 在面板中显示运行界面
panel.Controls.Clear();
module.ShowRunForm(panel);
```

### 4. 显示设置界面

```csharp
// 在面板中显示设置界面
panel.Controls.Clear();
module.ShowSettingForm(panel);
```

### 5. 执行动作

```csharp
// 方式 1: 使用 Action 方法
int result = module.Action("GOTO", "50.0", "30.0", "20.0");

// 方式 2: 通过 FunctionCaller（平台反调）
dynamic caller = module.FunctionCaller;
caller.Action("GOTOPOINT", "原点");

// 方式 3: 设置参数
module.SetParam("SpeedSetting", "80");

// 方式 4: 获取参数
int speed = (int)module.GetParam("SpeedSetting");
```

### 6. 保存配置

```csharp
// 保存当前设置
bool saveSuccess = module.Save();
```

### 7. 关闭模块

```csharp
module.Close();
```

## 事件处理

### 报警事件

模块支持报警事件通知:

```csharp
// 订阅报警事件
module.AlarmOccurred += (sender, e) =>
{
    Console.WriteLine($"[{e.ModuleName}] {e.Message}");
};
```

### 平台事件

模块自动注册以下平台事件:
- `ProjectManager.openProject`: 打开项目时重新加载配置
- `FormMain.stopButtonClick`: 停止按钮点击
- `FormMain.autoManualClick`: 自动/手动切换

这些事件会自动更新模块状态。

## 变量管理

模块提供内置的变量管理系统:

### 标准变量

| 变量名 | 类型 | 说明 |
|--------|------|------|
| "PointJump 首次运行" | 布尔 | 标记是否首次启动 |
| "PointJump 初始化完成" | 布尔 | 初始化成功标志 |
| "主平台请求跳转" | 布尔 | 平台下发跳转请求 |
| "跳转完成" | 布尔 | 跳转操作完成 |

### 自定义变量

可通过以下方式添加自定义变量:

```csharp
// 添加变量
string value = GetModuleVariable("自定义变量", DataType.字符串, "默认值");

// 修改变量
SetModuleVariable("自定义变量", "新值");

// 获取变量
string currentValue = GetStringVariable("自定义变量");
```

## 错误处理

### 常见错误码

| 错误码 | 描述 | 解决方式 |
|--------|------|----------|
| -1 | 未知命令 | 检查命令拼写 |
| -2 | 参数错误 | 检查参数格式和数量 |
| -3 | 超出范围 | 调整坐标到允许范围 |

### 异常处理

所有关键操作都有 try-catch 包裹:
- `Init()`: 初始化异常
- `LoadSetting()`: 配置加载异常
- `Save()`: 配置保存异常
- `Action()`: 命令执行异常

## 性能考虑

### 优化建议

1. **预设点位数量**: 建议不超过 100 个，过多会影响性能
2. **速度设置**: 建议使用合理的速度范围，避免过快导致安全问题
3. **内存管理**: 关闭模块时调用 `Close()` 释放资源

### 线程安全

模块内部操作是线程安全的:
- Hub 的位置更新有同步机制
- UI 访问通过 Invoke 保证线程安全
- 配置读写使用文件锁

## 与其他模块的关系

### 独立性声明

✅ **PointJump 模块是完全独立的**,可以：
- 单独部署使用
- 不依赖 MainControl 或 Trajectory 模块
- 独立进行配置和运行
- 有自己的完整生命周期

⚠️ **共享的基础设施**（合理且必要）:
- `XyzControllerHub`: XYZ 运动控制核心（工具类）
- `Controls` 目录下的 UI 控件（通用组件）
- `ProcessModuleBaseEx`: 框架基类

这些是**底层工具库**,不是业务耦合。

### 对比其他模块

| 特性 | PointJump | MainControl | Trajectory |
|------|-----------|-------------|------------|
| **主要功能** | 点位跳转 | 手动控制 | 轨迹查看 |
| **独有命令** | GOTOPOINT, SAVEPOINT, DELETEPOINT | JOG, ORIGIN, CENTER | RANDOM, CLEARTRAIL, SHOWTRAIL |
| **数据存储** | 预设点位列表 | 最后目标坐标 | 轨迹点序列 |
| **UI 特点** | 点位列表 + 坐标显示 | 统一控制面板 | 轨迹可视化 |

## API 参考

### ProcessModuleBaseEx 继承成员

| 方法 | 说明 |
|------|------|
| `Init(string name)` | 初始化模块 |
| `LoadSetting()` | 加载配置 |
| `Save()` | 保存配置 |
| `ShowRunForm(Panel panel)` | 显示运行界面 |
| `ShowSettingForm(Panel panel)` | 显示设置界面 |
| `Action(params object[] param)` | 执行动作 |
| `SetParam(object key, object value)` | 设置参数 |
| `GetParam(object itemName)` | 获取参数 |
| `StopAll()` | 停止所有运动 |
| `ReleaseAlarm()` | 释放报警 |
| `ReOpen()` | 重新打开 |
| `Close()` | 关闭模块 |

### 公共属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `FunctionCaller` | dynamic | 平台反调接口 |
| `GetInfo()` | string | 模块信息 |
| `Hub` | XyzControllerHub | XYZ 控制器实例 |
| `globalSetting` | PointJumpGlobalSetting | 全局设置 |
| `projectSetting` | PointJumpProjectSetting | 项目设置 |

## 最佳实践

### 1. 正确使用预设点位

```csharp
// ✅ 推荐：先检查是否存在
PresetPoint pt = FindPreset("A 工位");
if (pt != null)
{
    module.Action("GOTOPOINT", "A 工位");
}

// ❌ 不推荐：直接调用可能不存在的点位
module.Action("GOTOPOINT", "不存在的点位");
```

### 2. 边界检查

```csharp
// ✅ 推荐：在保存前检查范围
if (IsInRange(currentX, currentY, currentZ))
{
    module.Action("SAVEPOINT", "新点位");
}

// ❌ 不推荐：保存到无效范围
module.Action("SAVEPOINT", "无效点位");
```

### 3. 错误处理

```csharp
// ✅ 推荐：捕获异常
try
{
    int result = module.Action("GOTO", "50", "30", "20");
    if (result < 0)
    {
        Console.WriteLine("命令执行失败");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"异常：{ex.Message}");
}

// ❌ 不推荐：不考虑错误情况
module.Action("GOTO", "50", "30", "20");
```

## 故障排查

### Q1: 模块初始化失败

**原因**: 
- 配置文件不存在或损坏
- 参数格式错误

**解决**: 
- 删除损坏的配置文件重新生成
- 检查 GlobalSetting 和 ProjectSetting 文件格式

### Q2: 点位跳转失败

**原因**:
- 目标超出轴范围
- 参数格式错误

**解决**:
- 检查轴范围设置
- 确认坐标值格式正确（浮点数）

### Q3: 预设点位无法保存

**原因**:
- 写入权限不足
- 存储空间不足

**解决**:
- 检查文件夹权限
- 清理磁盘空间

## 版本历史

- v1.0.0: 初始版本
  - 基础点位跳转功能
  - 预设点位管理
  - 速度档位调节

## 技术支持

如有问题，请检查:
1. 日志文件中的错误信息
2. 配置文件完整性
3. 模块初始化状态

## License

本项目作为上位机平台的工艺模块插件使用。

---

**注意**: 编译时出现的 `InterfaceDefine` 和 `MainModule` 引用错误是正常的，因为这些 DLL 由上位机平台提供。
