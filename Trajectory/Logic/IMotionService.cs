using System;

namespace ProcessModules
{
    /// <summary>
    /// 统一运动控制后端服务接口（标准化硬件抽象层）。
    ///
    /// 设计原则：
    /// 1. 所有轴控制指令（移动、速度、坐标系、回原点、使能等）通过唯一的
    ///    ExecuteCommand(MotionCommand) 方法下发，便于后续替换和维护。
    /// 2. 实际位置通过 PositionUpdated 事件由后端实时推送到前端，
    ///    前端严禁使用模拟数据，确保前后端数据同步的一致性和准确性。
    /// 3. 接口实现应在后端服务中完成（如运动控制卡驱动、PLC 通信等），
    ///    不得直接嵌入前端页面代码。
    ///
    /// 替换方式：只需提供新的 IMotionService 实现并注入模组，
    /// 前端界面和工艺模组逻辑无需任何修改。
    /// </summary>
    public interface IMotionService
    {
        // ============== 指令下发（前端 → 后端，唯一入口）==============

        /// <summary>
        /// 执行运动控制指令（统一入口）。
        /// 不管是移动、速度设置、坐标系切换、回原点还是急停，
        /// 全部通过此方法下发，由后端根据 CommandType 分发执行。
        /// </summary>
        /// <param name="command">统一指令结构。</param>
        /// <returns>true = 指令已被后端接受；false = 指令被拒绝（如未连接、参数非法）。</returns>
        bool ExecuteCommand(MotionCommand command);

        // ============== 位置反馈（后端 → 前端，实时推送）==============

        /// <summary>
        /// 实际位置实时更新事件。
        /// 后端在接收到编码器/光栅尺反馈或发出轴控制指令后，
        /// 必须通过此事件将最新位置推送给前端。
        /// 前端订阅此事件以实时刷新 DRO、视图等位置显示。
        /// </summary>
        event EventHandler<AxisPositionEventArgs> PositionUpdated;

        /// <summary>
        /// 获取当前位置快照（主动查询，用于初始化或断线重连后同步）。
        /// </summary>
        AxisPosition GetPosition();

        // ============== 状态 ==============

        /// <summary>后端服务是否已连接（运动控制卡/PLC 通信正常）。</summary>
        bool IsConnected { get; }

        /// <summary>管理的轴数量（如 4 = X/Y/Z/U）。</summary>
        int AxisCount { get; }

        // ============== 生命周期 ==============

        /// <summary>打开连接（初始化运动控制卡/建立 PLC 通信）。</summary>
        bool Open();

        /// <summary>关闭连接（释放硬件资源）。</summary>
        void CloseService();
    }
}
