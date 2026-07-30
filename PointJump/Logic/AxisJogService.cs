using System;

namespace ProcessModules
{
    /// <summary>
    /// JOG（手动进给）服务：把"按钮按下/松开"翻译成对 AxisController 的目标设置。
    /// 支持两种模式：
    /// - 寸动（Incremental）：按一次走一步（默认 1mm）
    /// - 连续（Continuous）：按住持续走，松开立即停
    ///
    /// 不依赖任何 UI 类型。可单独测试，可复用到串口/网口场景。
    /// </summary>
    public class AxisJogService
    {
        private readonly AxisController _axis;
        private JogMode _mode = JogMode.Incremental;
        private float _stepDistance = 1.0f;

        // 当前是否正在 JOG 中（仅连续模式会持续 true）
        private bool _isJogging;
        private int _currentDirection;

        public AxisController Axis { get { return _axis; } }
        public JogMode Mode { get { return _mode; } }
        public float StepDistance { get { return _stepDistance; } }
        public bool IsJogging { get { return _isJogging; } }

        public AxisJogService(AxisController axis)
        {
            if (axis == null) throw new ArgumentNullException("axis");
            _axis = axis;
        }

        /// <summary>切换模式。</summary>
        public void SetMode(JogMode mode)
        {
            if (_mode == mode) return;
            // 切换时如果在 JOG 中，先停止（避免连续→寸动切换时还在跑）
            if (_isJogging) OnJogStop();
            _mode = mode;
        }

        /// <summary>设置寸动步长（单位与轴一致，通常 mm）。</summary>
        public void SetStepDistance(float step)
        {
            if (step <= 0f) throw new ArgumentOutOfRangeException("step", "步长必须大于 0");
            _stepDistance = step;
        }

        /// <summary>
        /// 用户按下 JOG 按钮。
        /// <param name="direction">方向：+1 正向，-1 负向。</param>
        /// </summary>
        public void OnJogStart(int direction)
        {
            if (direction != 1 && direction != -1)
                throw new ArgumentOutOfRangeException("direction", "方向必须是 +1 或 -1");

            _currentDirection = direction;
            _isJogging = true;

            switch (_mode)
            {
                case JogMode.Incremental:
                    // 寸动：目标 += 步长 * 方向
                    // AxisController 会自动限位
                    _axis.SetTarget(_axis.Target + direction * _stepDistance);
                    break;

                case JogMode.Continuous:
                    // 连续：把目标设为该方向的限位
                    // AxisController 的 Advance 会持续推动 Current 向 Target 靠近，
                    // 直到用户松开按钮（OnJogStop 会把 Target 冻结到 Current）
                    float limit = direction > 0 ? _axis.Max : _axis.Min;
                    _axis.SetTarget(limit);
                    break;
            }
        }

        /// <summary>
        /// 用户松开 JOG 按钮（仅连续模式有效，寸动模式无需调用）。
        /// </summary>
        public void OnJogStop()
        {
            if (!_isJogging) return;
            _isJogging = false;
            _currentDirection = 0;

            if (_mode == JogMode.Continuous)
            {
                // 关键：把目标冻结在当前值，让动画立即停止
                _axis.SetTarget(_axis.Current);
            }
        }

        /// <summary>紧急停止：把目标和当前值都固定住。</summary>
        public void EmergencyStop()
        {
            _isJogging = false;
            _currentDirection = 0;
            _axis.SetTarget(_axis.Current);
        }
    }
}
