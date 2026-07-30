using System;

namespace ProcessModules
{
    /// <summary>
    /// 运动控制指令类型枚举。
    /// 所有轴控制操作（移动、速度、坐标系、回原点等）均通过此枚举标识，
    /// 由 IMotionService.ExecuteCommand 统一分发。
    /// </summary>
    public enum MotionCommandType
    {
        /// <summary>绝对移动：移动到指定目标位置。</summary>
        MoveAbsolute,

        /// <summary>相对移动：从当前位置偏移指定量。</summary>
        MoveRelative,

        /// <summary>回原点（Home）。</summary>
        Home,

        /// <summary>停止所有轴运动。</summary>
        Stop,

        /// <summary>急停（立即切断运动）。</summary>
        EmergencyStop,

        /// <summary>设置运动速度。</summary>
        SetSpeed,

        /// <summary>设置坐标系编号。</summary>
        SetCoordinateSystem,

        /// <summary>使能轴（上电）。</summary>
        Enable,

        /// <summary>去使能轴（断电）。</summary>
        Disable,

        /// <summary>复位报警。</summary>
        Reset
    }

    /// <summary>
    /// 统一运动控制指令。
    /// 所有控制操作（不管是哪个轴的移动、速度设置、坐标系切换等）
    /// 都封装为此结构，通过 IMotionService.ExecuteCommand 唯一下发。
    /// 设计原则：一个方法入口，通过 CommandType 区分操作类型。
    /// </summary>
    public class MotionCommand
    {
        /// <summary>指令类型。</summary>
        public MotionCommandType CommandType;

        /// <summary>
        /// 目标位置数组（按轴索引：0=X, 1=Y, 2=Z, 3=U）。
        /// MoveAbsolute / MoveRelative 时有效；其他指令可为 null。
        /// </summary>
        public float[] Positions;

        /// <summary>
        /// 速度值（含义由后端定义：mm/s、百分比、档位等）。
        /// SetSpeed 时有效；MoveAbsolute/MoveRelative 时可选（为 0 表示使用当前速度）。
        /// </summary>
        public float Speed;

        /// <summary>
        /// 坐标系编号（SetCoordinateSystem 时有效）。
        /// 0 = 机械坐标系，1+ = 工件坐标系。
        /// </summary>
        public int CoordinateSystem;

        /// <summary>
        /// 扩展数据（供后端自定义使用，如轴掩码、加速度等）。
        /// </summary>
        public object Tag;

        public MotionCommand()
        {
        }

        public MotionCommand(MotionCommandType type)
        {
            CommandType = type;
        }

        /// <summary>快捷构造：绝对移动指令。</summary>
        public static MotionCommand CreateMoveAbsolute(float[] positions, float speed = 0f)
        {
            MotionCommand cmd = new MotionCommand(MotionCommandType.MoveAbsolute);
            cmd.Positions = positions;
            cmd.Speed = speed;
            return cmd;
        }

        /// <summary>快捷构造：相对移动指令。</summary>
        public static MotionCommand CreateMoveRelative(float[] offsets, float speed = 0f)
        {
            MotionCommand cmd = new MotionCommand(MotionCommandType.MoveRelative);
            cmd.Positions = offsets;
            cmd.Speed = speed;
            return cmd;
        }

        /// <summary>快捷构造：设置速度指令。</summary>
        public static MotionCommand CreateSetSpeed(float speed)
        {
            MotionCommand cmd = new MotionCommand(MotionCommandType.SetSpeed);
            cmd.Speed = speed;
            return cmd;
        }

        /// <summary>快捷构造：设置坐标系指令。</summary>
        public static MotionCommand CreateSetCoordinateSystem(int csIndex)
        {
            MotionCommand cmd = new MotionCommand(MotionCommandType.SetCoordinateSystem);
            cmd.CoordinateSystem = csIndex;
            return cmd;
        }
    }
}
