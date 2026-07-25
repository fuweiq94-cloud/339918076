# PlatformMotionAdapter 使用说明

> **文件位置**：`ProcessModules\Logic\PlatformMotionAdapter.cs`  
> **作用**：将 ProcessModules 工艺模组的运动指令，转发到上位机平台 DLL 提供的方法  
> **适用读者**：负责对接上位机平台的开发工程师

---

## 一、这个文件是干什么的？

### 1.1 一句话解释

**PlatformMotionAdapter 是一个「翻译器」**：把 ProcessModules 内部的标准运动指令，翻译成上位机平台 DLL 能听懂的方法调用。

### 1.2 为什么需要它？

```
┌─────────────────────────────────────────────────────────────┐
│  ProcessModules 工艺模组（您能改的源代码）                      │
│                                                             │
│  用户点击 GOTO → Hub.SetTarget(10,20,5)                     │
│       ↓                                                     │
│  MotionCommand { Type=MoveAbsolute, Positions=[10,20,5,0] } │
│       ↓                                                     │
│  IMotionService.ExecuteCommand(command)                      │
│       ↓                                                     │
│  ┌─────────────────────────────────────────────────────┐    │
│  │  PlatformMotionAdapter（本文件 = 翻译器）             │    │
│  │                                                     │    │
│  │  收到 MoveAbsolute → 调用平台 moveABS(10, 20, 5)    │    │
│  │  收到 Home         → 调用平台 movehoome()           │    │
│  │  收到 Stop         → 调用平台 stop()                │    │
│  └─────────────────────────────────────────────────────┘    │
│       ↓                                                     │
└───────┼─────────────────────────────────────────────────────┘
        ↓
┌─────────────────────────────────────────────────────────────┐
│  上位机平台 DLL（编译好的成品，不可修改）                       │
│                                                             │
│  moveABS(x, y, z)     ← 绝对移动                            │
│  movejump(...)        ← 点位跳转                            │
│  movego(...)          ← 相对移动/启动                        │
│  movehoome()          ← 回原点                              │
└─────────────────────────────────────────────────────────────┘
```

### 1.3 核心原则

| 原则 | 说明 |
|------|------|
| **只改这一个文件** | 其他所有模组代码（MainControl/PointJump/Trajectory）无需任何修改 |
| **不改接口** | `IMotionService`、`MotionCommand`、`XyzControllerHub` 保持不变 |
| **不改 UI** | 所有 RunForm / SettingForm 保持不变 |
| **平台方法不可改** | 您只能调用平台提供的方法，不能修改平台 DLL |

---

## 二、文件结构速览

```
PlatformMotionAdapter.cs
│
├── 构造函数
│   └── PlatformMotionAdapter(object platformMotionObject)
│       传入平台运动控制对象
│
├── IMotionService 接口实现
│   ├── Open()              → 连接平台
│   ├── CloseService()      → 断开连接
│   ├── ExecuteCommand()    → 核心：分发指令到平台方法 ★
│   ├── GetPosition()       → 读取当前位置
│   ├── IsConnected         → 连接状态
│   └── AxisCount           → 轴数量（4）
│
├── 平台方法调用（TODO 区域）★ 您需要修改的地方
│   ├── CallMoveABS()       → 调用 moveABS
│   ├── CallMoveJump()      → 调用 movejump
│   ├── CallMoveGo()        → 调用 movego
│   ├── CallMoveHome()      → 调用 movehoome
│   ├── CallStop()          → 调用 stop
│   ├── CallEmergencyStop() → 调用急停
│   └── CallSetSpeed()      → 调用设速
│
└── 位置推送
    ├── RaisePositionUpdated(float x, y, z, u)
    └── RaisePositionUpdated(float[] actual)
```

---

## 三、您需要修改的地方（共 3 步）

### 步骤 1：确认平台对象类型

**位置**：文件第 40 行附近

```csharp
// 当前写法（通用 object 类型）：
private object _platformMotion;

// 如果您知道平台 DLL 中的具体类型名，改为：
// private XxxMotionControl _platformMotion;
// 或
// private IMotionControl _platformMotion;
```

**如何确认**：查看平台 DLL 的文档或头文件，找到包含 `moveABS` 方法的类名。

---

### 步骤 2：修改方法调用（核心！）

**位置**：搜索文件中的 `TODO` 标记

#### moveABS（绝对移动）

```csharp
private bool CallMoveABS(MotionCommand command)
{
    float x = command.Positions[0];
    float y = command.Positions[1];
    float z = command.Positions[2];
    float u = command.Positions.Length > 3 ? command.Positions[3] : 0f;

    // ===== 取消注释其中一种 =====

    // 如果平台方法签名是 moveABS(float x, float y, float z)：
    // _platformMotion.moveABS(x, y, z);

    // 如果平台方法签名是 moveABS(float x, float y, float z, float u)：
    // _platformMotion.moveABS(x, y, z, u);

    // 如果平台方法签名是 moveABS(float[] positions)：
    // _platformMotion.moveABS(new float[] { x, y, z, u });

    // 如果编译时不确定类型，用 dynamic：
    // dynamic motion = _platformMotion;
    // motion.moveABS(x, y, z);

    return true;
}
```

#### movehoome（回原点）

```csharp
private bool CallMoveHome()
{
    // 如果平台方法签名是 movehoome()（所有轴一起回）：
    // _platformMotion.movehoome();

    // 如果平台方法签名是 movehoome(int axis)（逐轴回）：
    // _platformMotion.movehoome(0); // X
    // _platformMotion.movehoome(1); // Y
    // _platformMotion.movehoome(2); // Z

    return true;
}
```

#### movego（相对移动）

```csharp
private bool CallMoveGo(MotionCommand command)
{
    // 如果平台方法签名是 movego()（启动已设定的运动）：
    // _platformMotion.movego();

    // 如果平台方法签名是 movego(float dx, float dy, float dz)：
    // float dx = command.Positions[0];
    // float dy = command.Positions[1];
    // float dz = command.Positions[2];
    // _platformMotion.movego(dx, dy, dz);

    return true;
}
```

#### movejump（点位跳转）

```csharp
private bool CallMoveJump(MotionCommand command)
{
    // 如果平台方法签名是 movejump(float x, float y, float z)：
    // _platformMotion.movejump(x, y, z);

    // 如果平台方法签名是 movejump(int pointIndex)：
    // _platformMotion.movejump(pointIndex);

    return true;
}
```

---

### 步骤 3：删除调试日志（可选）

确认调用正确后，删除所有 `Debug.WriteLine` 行：

```csharp
// 删除这类行：
System.Diagnostics.Debug.WriteLine("[PlatformMotionAdapter] moveABS(...)");
```

---

## 四、指令映射表

| 模组操作 | 内部指令类型 | 适配器方法 | 平台 DLL 方法 |
|---------|-------------|-----------|--------------|
| 点击 GOTO / 输入坐标移动 | `MoveAbsolute` | `CallMoveABS()` | `moveABS(x, y, z)` |
| 点位跳转 | `MoveAbsolute` | `CallMoveJump()` | `movejump(...)` |
| JOG 寸动 | `MoveRelative` | `CallMoveGo()` | `movego(...)` |
| 回原点 | `Home` | `CallMoveHome()` | `movehoome()` |
| 停止 | `Stop` | `CallStop()` | `stop()` |
| 急停 | `EmergencyStop` | `CallEmergencyStop()` | `emergencyStop()` |
| 设速度 | `SetSpeed` | `CallSetSpeed()` | `setSpeed(value)` |

---

## 五、集成方式

### 5.1 平台启动时注入（推荐）

```csharp
// 在上位机平台启动代码中（或 ProcessModuleManager 初始化时）：

// 1. 获取平台运动控制对象（从平台 DLL 中获取）
object platformMotion = 获取平台运动对象(); // 包含 moveABS 等方法的对象

// 2. 创建适配器
IMotionService adapter = new PlatformMotionAdapter(platformMotion);

// 3. 打开连接
if (!adapter.Open())
{
    MessageBox.Show("运动控制连接失败");
    return;
}

// 4. 初始化模组
ProcessModuleManager.InitAll();

// 5. 注入适配器到所有模组 ★
ProcessModuleManager.InjectServiceToAll(adapter);

// 完成！此后所有运动指令自动转发到平台 DLL
```

### 5.2 位置反馈接入

如果平台 DLL 有位置变化回调，在回调中调用：

```csharp
// 平台位置回调中：
void OnPlatformPositionChanged(float x, float y, float z, float u)
{
    // 将真实位置推送给 ProcessModules
    adapter.RaisePositionUpdated(x, y, z, u);
    // → Hub 自动更新 Current → UI 自动刷新 DRO 显示
}
```

---

## 六、完整调用链路示例

### 用户点击「GOTO 10 20 5」按钮：

```
① RunForm 按钮点击
   → MainControlProcessModule.Action("GOTO", 10f, 20f, 5f)

② 模组处理
   → _hub.SetTarget(10, 20, 5)
   → SendCommand(MotionCommand { Type=MoveAbsolute, Positions=[10,20,5,0] })

③ Hub 转发
   → _service.ExecuteCommand(command)
   → PlatformMotionAdapter.ExecuteCommand(command)

④ 适配器翻译
   → case MoveAbsolute: CallMoveABS(command)
   → _platformMotion.moveABS(10, 20, 5)  ← 调用平台 DLL！

⑤ 平台执行
   → 硬件实际移动

⑥ 位置反馈（如果有）
   → 平台回调 → adapter.RaisePositionUpdated(10.1, 20.0, 5.0, 0)
   → Hub.Current 更新 → UI 刷新显示
```

---

## 七、常见问题

### Q1：编译报错「object 没有 moveABS 方法」

**原因**：`_platformMotion` 是 `object` 类型，编译器不知道它有 `moveABS` 方法。

**解决方案**（三选一）：

```csharp
// 方案A：改为具体类型（推荐）
private XxxMotionControl _platformMotion;

// 方案B：使用 dynamic（简单但无编译检查）
dynamic motion = _platformMotion;
motion.moveABS(x, y, z);

// 方案C：使用反射（最后手段）
_platformMotion.GetType()
    .GetMethod("moveABS")
    .Invoke(_platformMotion, new object[] { x, y, z });
```

---

### Q2：平台方法参数类型是 double 而不是 float

```csharp
// 修改 CallMoveABS 中的转换：
double x = (double)command.Positions[0];
double y = (double)command.Positions[1];
double z = (double)command.Positions[2];
_platformMotion.moveABS(x, y, z);
```

---

### Q3：平台方法有返回值（如 int 错误码）

```csharp
private bool CallMoveABS(MotionCommand command)
{
    int ret = _platformMotion.moveABS(x, y, z);
    if (ret != 0)
    {
        System.Diagnostics.Debug.WriteLine("moveABS 返回错误码: " + ret);
        return false;
    }
    return true;
}
```

---

### Q4：平台没有 stop / emergencyStop 方法

```csharp
// 用 moveABS 到当前位置来等效停止：
private bool CallStop()
{
    AxisPosition pos = GetPosition();
    _platformMotion.moveABS(pos.Actual[0], pos.Actual[1], pos.Actual[2]);
    return true;
}
```

---

### Q5：如何测试适配器是否正确工作？

1. 保留 `Debug.WriteLine` 日志
2. 在 VS 中运行，打开「输出」窗口
3. 操作界面（点击 GOTO / JOG / 回原点）
4. 观察输出窗口是否打印了对应的平台方法调用
5. 确认参数值正确后，再接入真实平台 DLL

---

## 八、修改检查清单

完成对接后，请逐项确认：

- [ ] `_platformMotion` 类型已改为平台 DLL 中的实际类型
- [ ] `CallMoveABS()` 中的 TODO 已替换为实际调用
- [ ] `CallMoveHome()` 中的 TODO 已替换为实际调用
- [ ] `CallMoveGo()` 中的 TODO 已替换为实际调用
- [ ] `CallStop()` 中的 TODO 已替换为实际调用
- [ ] `CallEmergencyStop()` 中的 TODO 已替换为实际调用
- [ ] `GetPosition()` 已实现真实位置读取
- [ ] 位置回调已接入 `RaisePositionUpdated()`
- [ ] 调试日志已清理（或保留为 Trace 级别）
- [ ] 编译通过，无错误
- [ ] 界面操作能正确触发平台方法

---

## 九、相关文件

| 文件 | 作用 | 是否需要修改 |
|------|------|-------------|
| `Logic\PlatformMotionAdapter.cs` | 平台方法适配器（本文件） | ✅ **需要修改** |
| `Logic\IMotionService.cs` | 运动服务接口定义 | ❌ 不改 |
| `Logic\MotionCommand.cs` | 统一指令结构 | ❌ 不改 |
| `Logic\XyzControllerHub.cs` | 业务大脑 | ❌ 不改 |
| `Logic\AxisPosition.cs` | 位置数据结构 | ❌ 不改 |
| `MainControl\MainControlProcessModule.cs` | 主控制模组 | ❌ 不改 |
| `PointJump\PointJumpProcessModule.cs` | 点位跳转模组 | ❌ 不改 |
| `Trajectory\TrajectoryViewProcessModule.cs` | 轨迹查看模组 | ❌ 不改 |
| `ProcessModuleManager.cs` | 模组管理器 | ❌ 不改 |

---

> **总结**：您只需要修改 `PlatformMotionAdapter.cs` 这一个文件，把 TODO 处的平台方法调用填上即可。其他所有代码保持不变！
