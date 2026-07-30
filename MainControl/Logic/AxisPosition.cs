using System;

namespace ProcessModules
{
    /// <summary>
    /// 多轴位置快照：包含各轴的目标位置与实际位置。
    /// 由后端服务（IMotionService）实时上报，前端界面据此刷新显示。
    /// 严禁在前端使用模拟数据填充此结构——所有数值必须来自后端。
    /// </summary>
    public class AxisPosition
    {
        /// <summary>各轴目标位置（按轴索引：0=X, 1=Y, 2=Z, 3=U）。</summary>
        public float[] Target;

        /// <summary>各轴实际位置（来自后端编码器/光栅尺反馈）。</summary>
        public float[] Actual;

        /// <summary>各轴是否正在运动中。</summary>
        public bool[] IsMoving;

        /// <summary>各轴是否已完成回原点。</summary>
        public bool[] IsHomed;

        /// <summary>各轴是否处于报警状态。</summary>
        public bool[] IsAlarmed;

        /// <summary>位置数据的时间戳（后端生成时刻）。</summary>
        public DateTime Timestamp;

        /// <summary>轴数量。</summary>
        public int AxisCount
        {
            get { return Actual != null ? Actual.Length : 0; }
        }

        public AxisPosition()
        {
        }

        /// <summary>创建指定轴数的空位置快照（全部归零）。</summary>
        public static AxisPosition CreateEmpty(int axisCount)
        {
            AxisPosition pos = new AxisPosition();
            pos.Target = new float[axisCount];
            pos.Actual = new float[axisCount];
            pos.IsMoving = new bool[axisCount];
            pos.IsHomed = new bool[axisCount];
            pos.IsAlarmed = new bool[axisCount];
            pos.Timestamp = DateTime.Now;
            return pos;
        }

        /// <summary>深拷贝。</summary>
        public AxisPosition Clone()
        {
            AxisPosition copy = new AxisPosition();
            copy.Target = (float[])this.Target.Clone();
            copy.Actual = (float[])this.Actual.Clone();
            copy.IsMoving = (bool[])this.IsMoving.Clone();
            copy.IsHomed = (bool[])this.IsHomed.Clone();
            copy.IsAlarmed = (bool[])this.IsAlarmed.Clone();
            copy.Timestamp = this.Timestamp;
            return copy;
        }
    }

    /// <summary>
    /// 位置更新事件参数：后端 → 前端的实时位置推送载体。
    /// </summary>
    public class AxisPositionEventArgs : EventArgs
    {
        /// <summary>最新位置快照。</summary>
        public AxisPosition Position { get; private set; }

        public AxisPositionEventArgs(AxisPosition position)
        {
            Position = position;
        }
    }
}
