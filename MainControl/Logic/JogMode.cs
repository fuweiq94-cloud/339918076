namespace ProcessModules
{
    /// <summary>
    /// JOG（手动进给）模式。
    /// </summary>
    public enum JogMode
    {
        /// <summary>
        /// 寸动：每按一次按钮移动一个固定步长（如 1mm）。
        /// 按一下走一步，到位置自动停。
        /// </summary>
        Incremental,

        /// <summary>
        /// 连续：按住按钮持续运动，松开立即停止。
        /// 用于大范围快速移动。
        /// </summary>
        Continuous
    }
}
