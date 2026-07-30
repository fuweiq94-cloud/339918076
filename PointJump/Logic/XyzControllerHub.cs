using System;

namespace ProcessModules
{
    /// <summary>
    /// XYZU 四轴统一控制器（业务层 / 前端与后端服务的中间层）。
    ///
    /// 职责：
    /// 1. 持有 IMotionService 引用，所有控制指令通过 service.ExecuteCommand 统一下发。
    /// 2. 订阅 service.PositionUpdated，将后端推送的实际位置写入各轴 AxisController.Current。
    /// 3. 前端界面只与本 Hub 交互（读 Target/Current、设目标），不直接接触后端实现。
    /// 4. 严禁使用模拟位置数据——Current 值全部来自后端服务推送。
    ///
    /// 替换后端：只需注入不同的 IMotionService 实现，Hub 和前端代码无需修改。
    /// </summary>
    public class XyzControllerHub
    {
        // —— 四轴 ——
        public AxisController X { get; private set; }
        public AxisController Y { get; private set; }
        public AxisController Z { get; private set; }
        public AxisController U { get; private set; }

        /// <summary>轴数组（按索引：0=X, 1=Y, 2=Z, 3=U），便于循环操作。</summary>
        private AxisController[] _axes;

        /// <summary>后端运动控制服务（唯一指令出口 + 位置反馈来源）。</summary>
        private IMotionService _service;

        /// <summary>
        /// 速度档位，取值 [0, 100]。修改时自动通过 MotionCommand 下发到后端。
        /// </summary>
        public int SpeedSetting
        {
            get { return _speedSetting; }
            set
            {
                _speedSetting = Math.Max(0, Math.Min(100, value));
                SendCommand(MotionCommand.CreateSetSpeed(_speedSetting));
            }
        }
        private int _speedSetting;

        /// <summary>任一轴的目标值或实际位置变化时触发（供前端订阅刷新 UI）。</summary>
        public event EventHandler Changed;

        /// <summary>后端服务是否已连接。</summary>
        public bool IsConnected
        {
            get { return _service != null && _service.IsConnected; }
        }

        /// <summary>
        /// 创建 Hub 并注入后端运动控制服务。
        /// </summary>
        /// <param name="service">后端服务实例（null = 未连接状态，指令不会下发）。</param>
        /// <param name="xMin">X 轴下限</param><param name="xMax">X 轴上限</param>
        /// <param name="yMin">Y 轴下限</param><param name="yMax">Y 轴上限</param>
        /// <param name="zMin">Z 轴下限</param><param name="zMax">Z 轴上限</param>
        /// <param name="uMin">U 轴下限</param><param name="uMax">U 轴上限</param>
        public XyzControllerHub(IMotionService service,
                                float xMin, float xMax,
                                float yMin, float yMax,
                                float zMin, float zMax,
                                float uMin, float uMax)
        {
            X = new AxisController("X", xMin, xMax);
            Y = new AxisController("Y", yMin, yMax);
            Z = new AxisController("Z", zMin, zMax);
            U = new AxisController("U", uMin, uMax);
            _axes = new AxisController[] { X, Y, Z, U };

            // 工控机旧编译器不支持 Lambda（=>），用命名方法 + 委托构造。
            X.Changed += new EventHandler(ForwardAxisChanged);
            Y.Changed += new EventHandler(ForwardAxisChanged);
            Z.Changed += new EventHandler(ForwardAxisChanged);
            U.Changed += new EventHandler(ForwardAxisChanged);

            _speedSetting = 20;
            SetService(service);
        }

        // ============== 后端服务管理 ==============

        /// <summary>
        /// 注入/替换后端运动控制服务。
        /// 替换时自动取消旧服务订阅、订阅新服务的位置推送。
        /// </summary>
        public void SetService(IMotionService service)
        {
            if (_service != null)
                _service.PositionUpdated -= new EventHandler<AxisPositionEventArgs>(Service_PositionUpdated);

            _service = service;

            if (_service != null)
            {
                _service.PositionUpdated += new EventHandler<AxisPositionEventArgs>(Service_PositionUpdated);
                // 连接后立即同步一次当前位置
                AxisPosition pos = _service.GetPosition();
                if (pos != null)
                    ApplyPosition(pos);
            }
        }

        /// <summary>
        /// 后端位置推送回调：将实际位置写入各轴 Current，触发前端刷新。
        /// 这是前端获取真实位置的唯一途径（严禁模拟）。
        /// </summary>
        private void Service_PositionUpdated(object sender, AxisPositionEventArgs e)
        {
            if (e.Position == null) return;
            ApplyPosition(e.Position);
        }

        /// <summary>
        /// 将位置快照应用到各轴。
        /// 仅更新 Actual（真实反馈）→ Current；忽略反馈中的 Target，
        /// 避免后端 Target 覆盖用户/Action 正在设定的本地目标。
        /// </summary>
        private void ApplyPosition(AxisPosition pos)
        {
            if (pos.Actual != null)
            {
                for (int i = 0; i < _axes.Length && i < pos.Actual.Length; i++)
                    _axes[i].UpdateCurrent(pos.Actual[i]);
            }
            // 故意不应用 pos.Target：本地 Target 由 SetTarget / JOG / Action 独占
        }

        // ============== 指令下发（统一通过 MotionCommand）==============

        /// <summary>向后端发送指令（内部统一出口）。</summary>
        private bool SendCommand(MotionCommand cmd)
        {
            if (_service == null || !_service.IsConnected) return false;
            return _service.ExecuteCommand(cmd);
        }

        /// <summary>一次性设置 X/Y/Z/U 四个轴的绝对目标位置。</summary>
        public void SetTarget(float x, float y, float z, float u)
        {
            X.SetTarget(x);
            Y.SetTarget(y);
            Z.SetTarget(z);
            U.SetTarget(u);
            SendCommand(MotionCommand.CreateMoveAbsolute(
                new float[] { X.Target, Y.Target, Z.Target, U.Target }, _speedSetting));
        }

        /// <summary>一次性设置 X/Y/Z 三个轴的绝对目标位置（U 轴不变）。</summary>
        public void SetTarget(float x, float y, float z)
        {
            X.SetTarget(x);
            Y.SetTarget(y);
            Z.SetTarget(z);
            SendCommand(MotionCommand.CreateMoveAbsolute(
                new float[] { X.Target, Y.Target, Z.Target, U.Target }, _speedSetting));
        }

        /// <summary>相对移动（各轴偏移量）。</summary>
        public void MoveRelative(float dx, float dy, float dz, float du)
        {
            X.SetTarget(X.Target + dx);
            Y.SetTarget(Y.Target + dy);
            Z.SetTarget(Z.Target + dz);
            U.SetTarget(U.Target + du);
            SendCommand(MotionCommand.CreateMoveRelative(
                new float[] { dx, dy, dz, du }, _speedSetting));
        }

        /// <summary>四轴回原点。</summary>
        public void Home()
        {
            SendCommand(new MotionCommand(MotionCommandType.Home));
        }

        /// <summary>四轴回到原点 (0,0,0,0)（兼容旧接口）。</summary>
        public void ResetToOrigin()
        {
            SetTarget(0f, 0f, 0f, 0f);
        }

        /// <summary>四轴移到各自范围中点。</summary>
        public void SetToCenter()
        {
            ForEachAxis(delegate(AxisController a) { a.SetToCenter(); });
            SendCommand(MotionCommand.CreateMoveAbsolute(
                new float[] { X.Target, Y.Target, Z.Target, U.Target }, _speedSetting));
        }

        /// <summary>停止所有轴运动。</summary>
        public void Stop()
        {
            // 冻结本地目标，避免 UI/动画继续追旧目标
            for (int i = 0; i < _axes.Length; i++)
                _axes[i].SetTarget(_axes[i].Current);
            SendCommand(new MotionCommand(MotionCommandType.Stop));
        }

        /// <summary>急停：冻结本地目标并向硬件下发急停指令。</summary>
        public void EmergencyStop()
        {
            for (int i = 0; i < _axes.Length; i++)
                _axes[i].SetTarget(_axes[i].Current);
            SendCommand(new MotionCommand(MotionCommandType.EmergencyStop));
        }

        /// <summary>设置坐标系。</summary>
        public void SetCoordinateSystem(int csIndex)
        {
            SendCommand(MotionCommand.CreateSetCoordinateSystem(csIndex));
        }

        /// <summary>四轴随机目标（仅用于演示/测试）。</summary>
        public void SetRandomTarget(Random rng)
        {
            ForEachAxis(delegate(AxisController a)
            {
                a.SetTarget(rng.Next((int)a.Min, (int)a.Max + 1));
            });
            SendCommand(MotionCommand.CreateMoveAbsolute(
                new float[] { X.Target, Y.Target, Z.Target, U.Target }, _speedSetting));
        }

        /// <summary>
        /// 运行时修改四轴范围（由设置界面保存后调用，无需改源码）。
        /// </summary>
        public void SetAxisRanges(float xMin, float xMax,
                                  float yMin, float yMax,
                                  float zMin, float zMax,
                                  float uMin, float uMax)
        {
            X.SetRange(xMin, xMax);
            Y.SetRange(yMin, yMax);
            Z.SetRange(zMin, zMax);
            U.SetRange(uMin, uMax);
        }

        // ============== 内部工具 ==============

        private void ForEachAxis(Action<AxisController> action)
        {
            action(X);
            action(Y);
            action(Z);
            action(U);
        }

        private void ForwardAxisChanged(object sender, EventArgs e)
        {
            OnChanged();
        }

        protected virtual void OnChanged()
        {
            EventHandler h = Changed;
            if (h != null) h(this, EventArgs.Empty);
        }
    }
}
