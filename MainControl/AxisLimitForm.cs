using System;
using System.Windows.Forms;

namespace ProcessModules.MainControl
{
    /// <summary>
    /// 轴限位设置弹窗：修改 X/Y/Z/U 四轴的上下限参数。
    /// 打开时从 GlobalSetting 读取当前值，确定后验证并写回。
    /// </summary>
    public partial class AxisLimitForm : Form
    {
        private readonly MainControlGlobalSetting _setting;
        private readonly XyzControllerHub _hub;
        private readonly UnifiedRunForm _runForm;

        /// <summary>
        /// 构造轴限位设置弹窗。
        /// </summary>
        /// <param name="setting">全局参数对象（读取/写回轴范围）。</param>
        /// <param name="hub">业务层（应用新范围）。</param>
        /// <param name="runForm">运行界面（刷新视图范围）。</param>
        public AxisLimitForm(MainControlGlobalSetting setting, XyzControllerHub hub, UnifiedRunForm runForm)
        {
            _setting = setting;
            _hub = hub;
            _runForm = runForm;
            InitializeComponent();
            LoadValues();
        }

        /// <summary>从全局参数加载当前轴范围到输入框。</summary>
        private void LoadValues()
        {
            nudXMin.Value = (decimal)_setting.XMin;
            nudXMax.Value = (decimal)_setting.XMax;
            nudYMin.Value = (decimal)_setting.YMin;
            nudYMax.Value = (decimal)_setting.YMax;
            nudZMin.Value = (decimal)_setting.ZMin;
            nudZMax.Value = (decimal)_setting.ZMax;
            nudUMin.Value = (decimal)_setting.UMin;
            nudUMax.Value = (decimal)_setting.UMax;
        }

        /// <summary>确定按钮：验证并应用新范围。</summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 验证：min 必须小于 max
            if (nudXMin.Value >= nudXMax.Value)
            {
                MessageBox.Show("X 轴最小值必须小于最大值。", "验证失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (nudYMin.Value >= nudYMax.Value)
            {
                MessageBox.Show("Y 轴最小值必须小于最大值。", "验证失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (nudZMin.Value >= nudZMax.Value)
            {
                MessageBox.Show("Z 轴最小值必须小于最大值。", "验证失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (nudUMin.Value >= nudUMax.Value)
            {
                MessageBox.Show("U 轴最小值必须小于最大值。", "验证失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 写回全局参数
            _setting.XMin = (float)nudXMin.Value;
            _setting.XMax = (float)nudXMax.Value;
            _setting.YMin = (float)nudYMin.Value;
            _setting.YMax = (float)nudYMax.Value;
            _setting.ZMin = (float)nudZMin.Value;
            _setting.ZMax = (float)nudZMax.Value;
            _setting.UMin = (float)nudUMin.Value;
            _setting.UMax = (float)nudUMax.Value;

            // 应用到 Hub
            if (_hub != null)
            {
                _hub.SetAxisRanges(
                    _setting.XMin, _setting.XMax,
                    _setting.YMin, _setting.YMax,
                    _setting.ZMin, _setting.ZMax,
                    _setting.UMin, _setting.UMax);
            }

            // 刷新运行界面视图范围
            if (_runForm != null)
                _runForm.ApplyRanges();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
