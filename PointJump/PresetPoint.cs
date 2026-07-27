namespace ProcessModules
{
    /// <summary>
    /// 预设点位数据（点位跳转工艺模组使用）。
    /// 由 PointJumpForm 界面层与 PointJumpProcessModule 工艺层共享，
    /// 通过 PointJumpProjectSetting 随项目参数持久化。
    /// 注意：使用公共字段是为了兼容 XML 序列化。
    /// </summary>
    public class PresetPoint
    {
        public string Name = "";
        public float X;
        public float Y;
        public float Z;

        public PresetPoint()
        {
        }

        public PresetPoint(string name, float x, float y, float z)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return string.Format("{0} (X={1:F2} Y={2:F2} Z={3:F2})", Name, X, Y, Z);
        }
    }
}
