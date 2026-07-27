# MainControl 主控制工艺模块

## 模块概述

MainControl 是一个独立的工艺模块，专门用于**三轴手动控制和 JOG 寸动功能**。该模块可单独部署使用，不依赖于任何其他工艺模块。

## 功能特性

### 核心功能
- ✅ XYZ 三轴坐标控制（GOTO）
- ✅ 回原点操作（ORIGIN）
- ✅ 回中心操作（CENTER）
- ✅ JOG 寸动控制（增量/连续模式）
- ✅ 速度档位调节 [0-100]
- ✅ 轴范围限制保护
- ✅ 紧急停止（ESTOP）

### 支持的命令

```
GOTO <x> <y> <z>     → 移动到指定坐标
ORIGIN               → 回原点
CENTER               → 回中心
JOG <axis> <dir>     → JOG 寸动 (axis=X/Y/Z, dir=+1/-1)
SETSPEED <value>     → 设置速度档位 [0,100]
STOP / ESTOP         → 停止/紧急停止
```

### 示例用法

#### 移动到指定坐标
```csharp
module.Action("GOTO", "50.0", "30.0", "20.0");
```

#### 回原点
```csharp
module.Action("ORIGIN");
```

#### JOG 寸动
```csharp
// X 轴正向寸动一步
module.Action("JOG", "X", "+1");

// Y 轴负向寸动一步
module.Action("JOG", "Y", "-1");
```

#### 回中心
```csharp
module.Action("CENTER");
```

## 模块组成

### 核心类

| 类名 | 描述 |
|------|------|
| `MainControlProcessModule` | 主模块类，实现平台接口 |
| `MainControlGlobalSetting` | 全局配置（轴范围、速度、JOG 参数） |
| `MainControlProjectSetting` | 项目配置（最后目标坐标、统计信息等） |
| `UnifiedRunForm` | 统一运行界面 |
| `RunForm` | 基础运行界面 |
| `AxisLimitForm` | 轴范围设置界面 |
| `AxisJogService` | JOG 寸动服务 |

### 依赖组件

#### 业务逻辑层
- `XyzControllerHub`: XYZ 轴控制器（可从 ProcessModules.Common 引用）
- `AxisJogService`: JOG 寸动控制服务
- `JogMode`: JOG 模式枚举（增量式/连续式）
- `AxisController`: 单轴控制器
- `AxisPosition`: 位置数据容器

#### UI 控件（可选）
- `DroLabel`: 数字读数显示
- `JogButton`: JOG 控制按钮
- `XYView`: XY 平面视图

## 配置说明

### 全局设置 (MainControlGlobalSetting)

存储以下参数:
- **轴范围**: XMin, XMax, YMin, YMax, ZMin, ZMax, UMin, UMax
- **速度档位**: SpeedSetting (0-100)
- **JOG 设置**: 
  - JogIncremental: true=增量式，false=连续式
  - JogStep: 增量步距
- **任务变量**: TaskItemSetting（系统变量）

配置文件路径：`./Config/MainControl_GlobalSettings.json`

### 项目设置 (MainControlProjectSetting)

存储以下参数:
- **预设点位列表**: Presets（点位数组）
- **最后目标坐标**: LastTargetX, LastTargetY, LastTargetZ
- **跳转次数**: GotoCount（统计信息）

配置文件路径：`./Config/MainControl_ProjectSettings.json`

### JOG 模式说明

#### 增量式 (Incremental)
- 每次按下移动固定距离
- 松开即停止
- 适合精确定位

#### 连续式 (Continuous)
- 按住持续运动
- 需要按 STOP 或 ESTOP 停止
- 适合大范围移动

默认模式：**连续式**（JogIncremental = false）

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
using ProcessModules.MainControl;

// 创建模块实例
var module = new MainControlProcessModule();

// 初始化（strName 必须与文件名一致，例如"MainControl"）
bool initSuccess = module.Init("MainControl");

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
// 移动坐标
int result = module.Action("GOTO", "50.0", "30.0", "20.0");

// JOG 寸动
result = module.Action("JOG", "X", "+1");

// 回原点
result = module.Action("ORIGIN");

// 设置 JOG 模式为增量式
module.SetParam("JogIncremental", true);

// 设置 JOG 步距
module.SetParam("JogStep", "1.5");

// 获取当前位置
float currentX = (float)module.GetParam("CurrentX");
float currentY = (float)module.GetParam("CurrentY");
float currentZ = (float)module.GetParam("CurrentZ");
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

```csharp
module.AlarmOccurred += (sender, e) =>
{
    Console.WriteLine($"[{e.ModuleName}] {e.Message}");
};
```

### 平台事件

模块自动注册平台事件:
- `ProjectManager.openProject`: 重新加载配置
- `FormMain.stopButtonClick`: 触发停止
- `FormMain.autoManualClick`: 切换工作模式

## 变量管理

### 标准变量

| 变量名 | 类型 | 说明 |
|--------|------|------|
| "主控制首次运行" | 布尔 | 标记是否首次启动 |
| "主控制初始化完成" | 布尔 | 初始化成功标志 |
| "主平台请求移动" | 布尔 | 平台下发移动请求 |
| "移动完成" | 布尔 | 移动操作完成 |

### 自定义变量

```csharp
// 添加变量
GetModuleVariable("自定义变量", DataType.字符串, "默认值");

// 修改变量
SetModuleVariable("自定义变量", "新值");

// 获取变量
string value = GetStringVariable("自定义变量");
```

## 错误处理

### 常见错误码

| 错误码 | 描述 | 解决方式 |
|--------|------|----------|
| -1 | 空命令或未知命令 | 检查命令格式 |
| -2 | 参数错误 | 检查参数数量和格式 |
| -3 | 超出轴范围 | 调整坐标到允许范围 |

### 异常处理

关键操作均有异常捕获:
- `Init()`: 初始化异常
- `LoadSetting()`: 配置加载异常
- `Save()`: 配置保存异常
- `Action()`: 命令执行异常

## 性能优化

### 建议

1. **JOG 模式选择**: 
   - 精细调整时使用增量式
   - 大范围移动时使用连续式

2. **速度预设**: 
   - 常用场景预先设置合适速度
   - 避免频繁调整速度

3. **内存管理**: 
   - 及时调用 Close() 释放资源
   - 避免重复创建实例

## 独立性声明

✅ **MainControl 模块是完全独立的**,可以：
- 单独部署使用
- 不依赖 PointJump 或 Trajectory 模块
- 独立进行配置和运行
- 有自己的完整生命周期

⚠️ **共享的基础设施**（工具类）:
- `XyzControllerHub`: XYZ 运动控制核心
- `Controls` 目录下的 UI 控件
- `ProcessModuleBaseEx`: 框架基类

这些是**底层工具库**,不是业务耦合。

## API 参考

### 核心方法

| 方法 | 说明 |
|------|------|
| `JogAxis(axisName, direction)` | 对指定轴执行 JOG |
| `ApplyJogSetting()` | 应用 JOG 设置 |
| `ApplyRanges()` | 应用轴范围限制 |

### 公共属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `Hub` | XyzControllerHub | XYZ 控制器 |
| `globalSetting` | MainControlGlobalSetting | 全局设置 |
| `projectSetting` | MainControlProjectSetting | 项目设置 |
| `_jogServices` | AxisJogService[] | JOG 服务数组 |

## 最佳实践

### 1. JOG 使用建议

```csharp
// ✅ 推荐：先设置模式再 JOG
module.SetParam("JogIncremental", true);  // 增量式
module.Action("JOG", "X", "+1");          // 精确移动

// ❌ 不推荐：在连续模式下频繁 JOG
module.SetParam("JogIncremental", false); // 连续式
module.Action("JOG", "X", "+1");          // 可能需要 STOP
```

### 2. 安全操作

```csharp
// ✅ 推荐：先急停再操作
module.StopAll();
// ... 执行其他操作 ...

// ❌ 不推荐：跳过安全检查
// 直接执行可能危险的操
```

### 3. 参数验证

```csharp
// ✅ 推荐：验证参数范围
if (IsInRange(x, y, z))
{
    module.Action("GOTO", x.ToString(), y.ToString(), z.ToString());
}

// ❌ 不推荐：不验证直接使用
module.Action("GOTO", x.ToString(), y.ToString(), z.ToString());
```

## 故障排查

### Q1: JOG 不动作

**原因**: 
- 方向参数错误
- 轴被锁定

**解决**: 
- 确认方向为 +1 或 -1
- 检查是否有急停信号

### Q2: 无法回原点

**原因**: 
- 原点未校准
- 限位开关触发

**解决**: 
- 重新校准原点
- 检查限位开关状态

### Q3: 坐标超限报警

**原因**: 
- 目标超出轴范围

**解决**: 
- 调整目标坐标到允许范围
- 修改轴范围设置

## 版本历史

- v1.0.0: 初始版本
  - 基础坐标控制
  - JOG 寸动功能
  - 原点/中心复位

## 总结

MainControl 模块提供了完整的三轴手动控制能力，特别适合:
- 🔧 设备调试和维护
- 📏 精确定位和调整
- 🎛️ 人工操作流程

---

**注意**: InterfaceDefine 和 MainModule 的编译错误是正常的。
