using System;

namespace ProcessModules
{
    /// <summary>
    /// 单个轴的业务控制器。
    /// 负责管理：当前值、目标值、范围限位、动画推进。
    /// 不依赖任何 UI 类型（不引用 System.Windows.Forms / System.Drawing），
    /// 因此可以单独单元测试，也可以被控制台程序、Web 后端、串口驱动等复用。
    /// </summary>
    public class AxisController
    {
        // —— 范围（构造后不可变）——
        public float Min { get; private set; }
        public float Max { get; private set; }
        public string Name { get; private set; }

        // —— 状态 ——
        public float Current { get; private set; }
        public float Target { get; private set; }

        /// <summary>当前值是否已经达到目标（误差 &lt; 0.001）。</summary>
        public bool AtTarget
        {
            get { return Math.Abs(Current - Target) < 0.001f; }
        }

        /// <summary>当前值/目标值变化时触发。</summary>
        public event EventHandler Changed;

        public AxisController(string name, float min, float max, float initial = 0f)
        {
            if (max <= min) throw new ArgumentException("max 必须大于 min");
            Name = name;
            Min = min;
            Max = max;
            float clamped = MathHelper.Clamp(initial, Min, Max);
            Current = clamped;
            Target = clamped;
        }

        // ============== 操作 ==============

        /// <summary>设置目标值（会自动限制到 [Min, Max]）。</summary>
        public void SetTarget(float value)
        {
            float clamped = MathHelper.Clamp(value, Min, Max);
            if (Math.Abs(clamped - Target) < 0.0001f) return;
            Target = clamped;
            OnChanged();
        }

        /// <summary>在当前目标上加减一个步长。</summary>
        public void Step(int delta)
        {
            SetTarget(Target + delta);
        }

        /// <summary>把目标设为范围中点。</summary>
        public void SetToCenter()
        {
            SetTarget((Min + Max) * 0.5f);
        }

        /// <summary>把目标设为下限。</summary>
        public void SetToMin()
        {
            SetTarget(Min);
        }

        /// <summary>把目标设为上限。</summary>
        public void SetToMax()
        {
            SetTarget(Max);
        }

        /// <summary>目标回到 0（如果 0 在范围内）。</summary>
        public void ResetToOrigin()
        {
            if (0f >= Min && 0f <= Max) SetTarget(0f);
            else SetToCenter();
        }

        /// <summary>
        /// 推进一帧动画：让 Current 以插值方式逼近 Target。
        /// lerpFraction 越大移动越快，取值 [0, 1]。
        /// 注意：lerpFraction = 0 时完全不移动（不允许"吸附到目标"）。
        /// </summary>
        public void Advance(float lerpFraction)
        {
            float k = MathHelper.Clamp01(lerpFraction);

            // lerp = 0 → 不动；否则按比例推进
            if (k <= 0f) return;

            float delta = (Target - Current) * k;

            // 剩余距离很小时，直接吸附到目标（避免无穷小迭代）
            if (Math.Abs(Target - Current) < 0.0005f)
            {
                if (!AtTarget)
                {
                    Current = Target;
                    OnChanged();
                }
                return;
            }

            if (Math.Abs(delta) >= 0.00005f)
            {
                Current += delta;
                OnChanged();
            }
        }

        // ============== 后端反馈 ==============

        /// <summary>
        /// 运行时修改轴范围（由设置界面保存后调用，无需改源码）。
        /// 修改后当前值和目标值会自动限制到新范围内。
        /// </summary>
        public void SetRange(float min, float max)
        {
            if (max <= min) return;
            Min = min;
            Max = max;
            // 把当前值和目标值限制到新范围
            Current = MathHelper.Clamp(Current, Min, Max);
            Target = MathHelper.Clamp(Target, Min, Max);
            OnChanged();
        }

        /// <summary>
        /// 由后端服务更新实际位置（严禁前端直接调用）。
        /// 当 IMotionService 推送位置反馈时，Hub 调用此方法将真实值写入 Current。
        /// </summary>
        public void UpdateCurrent(float actualValue)
        {
            if (Math.Abs(actualValue - Current) < 0.0001f) return;
            Current = actualValue;
            OnChanged();
        }

        // ============== 工具 ==============

        protected virtual void OnChanged()
        {
            EventHandler h = Changed;
            if (h != null) h(this, EventArgs.Empty);
        }
    }
}
