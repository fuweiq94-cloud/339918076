using System;

namespace ProcessModules
{
    /// <summary>
    /// 数学工具类：提取各模块中重复的数学运算。
    /// 放在 Controls 类库中，供主项目（Logic 层）和控件层共同使用。
    /// </summary>
    public static class MathHelper
    {
        /// <summary>将值限制到 [min, max] 范围内。</summary>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>将值限制到 [0, 1] 范围内。</summary>
        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        /// <summary>将值限制到 [0.02, 1] 范围内（动画插值系数的有效范围）。</summary>
        public static float ClampLerp(float value)
        {
            return Clamp(value, 0.02f, 1f);
        }

        /// <summary>将整数值限制到 [min, max] 范围内。</summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>将 decimal 值限制到 [min, max] 范围内。</summary>
        public static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
