# Trajectory 轨迹查看工艺模块

## 模块概述

Trajectory 是一个独立的工艺模块，专门用于**运动轨迹的记录、显示和分析**。该模块可单独部署使用，不依赖于任何其他工艺模块。

## 功能特性

### 核心功能
- ✅ 运动轨迹实时记录
- ✅ XY 平面轨迹可视化
- ✅ Z 轴高度条形图显示
- ✅ 随机轨迹演示（RANDOM）
- ✅ 轨迹清除和保存
- ✅ 轨迹显示开关控制
- ✅ 速度档位调节 [0-100]

### 支持的命令

```
GOTO <x> <y> <z>       → 移动到指定坐标并记录轨迹
ORIGIN / CENTER        → 回原点/中心
RANDOM                 → 生成随机目标轨迹
CLEARTRAIL             → 清除所有轨迹点
SHOWTRAIL <on/off>     → 显示/隐藏轨迹
SETSPEED <value>       → 设置速度档位 [0,100]
STOP                   → 停止运动
```

### 示例用法

#### 移动并记录轨迹
```csharp
module.Action("GOTO", "50.0", "30.0", "20.0");
// 移动过程中会记录路径上的所有点
```

#### 显示轨迹
```csharp
// 开启轨迹显示
module.Action("SHOWTRAIL", "on");

// 关闭轨迹显示
module.Action("SHOWTRAIL", "off");
```

#### 随机轨迹演示
```csharp
// 生成一系列随机移动点
for (int i = 0; i < 10; i++)
{
    module.Action("RANDOM");
    Thread.Sleep(500); // 间隔 0.5 秒
}
```

#### 清除轨迹
```csharp
module.Action("CLEARTRAIL");
```

## 模块组成

### 核心类

| 类名 | 描述 |
|------|------|
| `TrajectoryViewProcessModule` | 主模块类，实现平台接口 |
| `TrajectoryGlobalSetting` | 全局配置（轴范围、速度、轨迹设置） |
| `TrajectoryProjectSetting` | 项目配置（轨迹点数、统计信息等） |
| `RunForm` | 运行界面（含轨迹显示） |

### 依赖组件

#### 业务逻辑层
- `XyzControllerHub`: XYZ 轴控制器及轨迹记录（可从 ProcessModules.Common 引用）
- `AxisController`: 单轴控制器
- `AxisPosition`: 位置数据容器

#### UI 控件
- `XYView`: XY 平面轨迹绘制控件 ⭐
- `ZBarView`: Z 轴高度条形图控件 ⭐
- `DroLabel`: 数字读数显示
- `JogButton`: JOG 控制按钮
- `PaintHelper`: 绘图辅助工具

## 配置说明

### 全局设置 (TrajectoryGlobalSetting)

存储以下参数:
- **轴范围**: XMin, XMax, YMin, YMax, ZMin, ZMax, UMin, UMax
- **速度档位**: SpeedSetting (0-100)
- **轨迹设置**: 
  - ShowTrail: true=显示轨迹，false=隐藏
- **任务变量**: TaskItemSetting（系统变量）

配置文件路径：`./Config/Trajectory_GlobalSettings.json`

### 项目设置 (TrajectoryProjectSetting)

存储以下参数:
- **轨迹点数**: LastTrailPointCount
- **清除次数**: ClearTrailCount
- **其他统计信息**

配置文件路径：`./Config/Trajectory_ProjectSettings.json`

### 轨迹数据结构

轨迹点由 `XyzControllerHub` 内部管理，每个点包含:
```csharp
class TrailPoint
{
    public double X { get; set; }      // X 坐标
    public double Y { get; set; }      // Y 坐标
    public double Z { get; set; }      // Z 坐标
    public DateTime Timestamp { get; } // 时间戳
}
```

运行界面上的轨迹数据显示:
- **XYView**: 在 XY 平面上绘制轨迹线
- **ZBarView**: 显示 Z 轴高度的柱状图

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
using ProcessModules.Trajectory;

// 创建模块实例
var module = new TrajectoryViewProcessModule();

// 初始化
bool initSuccess = module.Init("Trajectory");

if (!initSuccess)
{
    MessageBox.Show("模块初始化失败!");
    return;
}
```

### 3. 显示运行界面

```csharp
// 在面板中显示运行界面（包含轨迹视图）
panel.Controls.Clear();
module.ShowRunForm(panel);

// 界面会自动显示 XY 轨迹图和 Z 轴条形图
```

### 4. 控制轨迹显示

```csharp
// 开启轨迹显示
module.SetParam("ShowTrail", true);

// 关闭轨迹显示
module.SetParam("ShowTrail", false);

// 或者通过命令
module.Action("SHOWTRAIL", "on");
module.Action("SHOWTRAIL", "off");
```

### 5. 执行动作

```csharp
// 移动并记录轨迹
module.Action("GOTO", "50.0", "30.0", "20.0");

// 获取轨迹点数
int trailCount = (int)module.GetParam("TrailPointCount");

// 随机轨迹演示
for (int i = 0; i < 5; i++)
{
    module.Action("RANDOM");
    await Task.Delay(500);
}

// 清除轨迹
module.Action("CLEARTRAIL");
```

### 6. 获取轨迹信息

```csharp
// 获取当前轨迹点数
int count = (int)module.GetParam("TrailPointCount");

// 获取速度设置
int speed = (int)module.GetParam("SpeedSetting");

// 获取轨迹显示状态
bool showTrail = (bool)module.GetParam("ShowTrail");
```

### 7. 保存配置

```csharp
// 保存当前设置
bool saveSuccess = module.Save();
```

### 8. 关闭模块

```csharp
module.Close();
```

## 事件处理

### 报警事件

```csharp
module.AlarmOccurred += (sender, e) =>
{
    Console.WriteLine($"[{e.ModuleName}] {e.Message}");
};
```

### 平台事件

模块自动注册平台事件:
- `ProjectManager.openProject`: 重新加载配置

## 变量管理

### 标准变量

| 变量名 | 类型 | 说明 |
|--------|------|------|
| "轨迹查看首次运行" | 布尔 | 标记是否首次启动 |
| "轨迹查看初始化完成" | 布尔 | 初始化成功标志 |
| "轨迹记录中" | 布尔 | 当前是否在记录轨迹 |

### 自定义变量

```csharp
// 添加变量
GetModuleVariable("自定义变量", DataType.字符串, "默认值");

// 修改变量
SetModuleVariable("自定义变量", "新值");
```

## 视觉组件详解

### XYView 控件

XYView 是轨迹显示的核心控件，功能包括:

1. **轨迹绘制**
   - 实时绘制 XY 平面轨迹线
   - 支持颜色、粗细自定义
   - 平滑插值显示

2. **坐标显示**
   - 显示当前 XY 坐标
   - 显示目标 XY 坐标
   - 显示最大/最小范围

3. **缩放和平移**
   - 鼠标滚轮缩放
   - 拖拽平移视图
   - 自动适应内容

### ZBarView 控件

ZBarView 显示 Z 轴高度变化:

1. **柱状图显示**
   - 实时更新高度柱
   - 颜色编码表示高度等级

2. **数值显示**
   - 当前 Z 值
   - 最大/最小 Z 值
   - 平均 Z 值

### DroLabel 控件

数字读数显示:

1. **实时数值**
   - X/Y/Z/U 坐标值
   - 小数位数可调
   - 刷新率可调

2. **状态指示**
   - 目标值 vs 当前位置
   - 到达标记

## 错误处理

### 常见错误码

| 错误码 | 描述 | 解决方式 |
|--------|------|----------|
| -1 | 未知命令 | 检查命令拼写 |
| -2 | 参数错误 | 检查参数格式 |
| -3 | 超出范围 | 调整坐标 |

### 异常处理

关键操作均有 try-catch 包裹:
- `Init()`: 初始化异常
- `LoadSetting()`: 配置加载异常
- `Save()`: 配置保存异常
- `Action()`: 命令执行异常

## 性能优化

### 建议

1. **轨迹点数量限制**: 
   - 建议不超过 10000 个点
   - 过多会影响渲染性能
   - 定期调用 CLEARTRAIL 清理

2. **显示优化**: 
   - 不需要时关闭轨迹显示 (`SHOWTRAIL off`)
   - 降低 UI 刷新频率

3. **内存管理**: 
   - 及时调用 Close()
   - 避免长时间累积轨迹数据

### 最佳实践

```csharp
// ✅ 推荐：定期清理轨迹
if (trailPoints.Count > MAX_POINTS)
{
    module.Action("CLEARTRAIL");
}

// ❌ 不推荐：无限制累积轨迹
// 可能导致界面卡顿或内存不足
```

## 独立性声明

✅ **Trajectory 模块是完全独立的**,可以：
- 单独部署使用
- 不依赖 PointJump 或 MainControl 模块
- 独立进行配置和运行
- 有自己的完整生命周期

⚠️ **共享的基础设施**（工具类）:
- `XyzControllerHub`: XYZ 运动控制及轨迹记录
- `Controls` 目录下的 UI 控件（XYView, ZBarView 等）
- `ProcessModuleBaseEx`: 框架基类

这些是**底层工具库**,不是业务耦合。

## API 参考

### 核心方法

| 方法 | 说明 |
|------|------|
| `ClearTrail()` | 清除轨迹点 |
| `SetShowTrail(onOff)` | 设置轨迹显示状态 |
| `ApplyRanges()` | 应用轴范围限制 |

### 公共属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `Hub` | XyzControllerHub | XYZ 控制器 |
| `globalSetting` | TrajectoryGlobalSetting | 全局设置 |
| `projectSetting` | TrajectoryProjectSetting | 项目设置 |

### 运行时信息

可通过 GetParam 获取:

| 参数 | 类型 | 说明 |
|------|------|------|
| "TrailPointCount" | int | 当前轨迹点数 |
| "ShowTrail" | bool | 轨迹显示状态 |
| "SpeedSetting" | int | 速度档位 |
| "CurrentX/Y/Z" | float | 当前位置 |

## 故障排查

### Q1: 轨迹不显示

**原因**: 
- ShowTrail = false
- UI 控件未正确加载

**解决**: 
```csharp
module.SetParam("ShowTrail", true);
// 或
module.Action("SHOWTRAIL", "on");
```

### Q2: 界面卡顿

**原因**: 
- 轨迹点过多
- 频繁重绘

**解决**: 
- 调用 CLEARTRAIL 清理
- 减少轨迹点积累

### Q3: 轨迹点不准确

**原因**: 
- 采样频率过低
- 渲染插值误差

**解决**: 
- 增加采样频率
- 调整插值算法

## 使用场景

### 1. 路径规划验证

在正式运行前，使用 RANDOM 或 GOTO 模拟路径，确认轨迹符合预期。

```csharp
// 模拟加工路径
module.Action("SHOWTRAIL", "on");
module.Action("GOTO", "0", "0", "0");
module.Action("GOTO", "10", "10", "5");
module.Action("GOTO", "20", "0", "10");
// 观察轨迹是否符合预期
```

### 2. 设备调试

通过可视化轨迹检查设备的运动精度和异常。

### 3. 教学演示

展示 XYZ 三轴的联动效果和空间轨迹概念。

## 版本历史

- v1.0.0: 初始版本
  - 基础轨迹记录
  - XY/Z 可视化
  - 随机轨迹演示

## 总结

Trajectory 模块提供了直观的轨迹可视化工具，特别适合:
- 📊 运动路径分析
- 🔍 设备调试和验证
- 🎓 教学和数据展示
- 📝 轨迹数据采集

---

**注意**: InterfaceDefine 和 MainModule 的编译错误是正常的。
