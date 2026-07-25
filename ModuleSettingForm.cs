using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProcessModules
{
    /// <summary>
    /// 工艺模组综合界面（对应 DOMO 模板中的 SettingForm）。
    /// 包含三个运行界面（嵌入式，不可拖拽）+ 全局参数 + 项目参数。
    /// 运行界面以 TopLevel=false 方式嵌入 TabPage，失去独立窗口特性，
    /// 不能自由拖拽移动位置，始终作为本界面的固定组成部分。
    /// </summary>
    public partial class ModuleSettingForm : Form
    {
        private readonly ProcessModuleBaseEx _module;

        /// <summary>
        /// 无参构造：供 VS 设计器实例化使用。
        /// 运行时请使用带参构造函数注入模组与参数对象。
        /// </summary>
        public ModuleSettingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构建综合界面：嵌入三个运行界面 + 参数设置。
        /// </summary>
        /// <param name="module">所属工艺模组（保存按钮调用 module.Save()）。</param>
        /// <param name="globalSetting">全局参数对象（显示在“全局参数”页）。</param>
        /// <param name="projectSetting">项目参数对象（显示在“项目参数”页）。</param>
        public ModuleSettingForm(ProcessModuleBaseEx module, object globalSetting, object projectSetting)
        {
            _module = module;
            InitializeComponent();

            this.Text = module.GetInfo() + " - 工艺模组";
            _gridGlobal.SelectedObject = globalSetting;
            _gridProject.SelectedObject = projectSetting;

            // 将三个运行界面嵌入到 TabControl 的前三个页（在参数页之前）
            EmbedRunForms();
        }

        /// <summary>
        /// 将 ProcessModuleManager 中注册的所有模组的运行界面嵌入到本界面的 TabControl 中。
        /// 每个 RunForm 以 TopLevel=false + Dock=Fill 方式嵌入，
        /// 失去独立窗口特性，不能自由拖拽移动。
        /// </summary>
        private void EmbedRunForms()
        {
            int insertIndex = 0;
            foreach (KeyValuePair<string, ProcessModuleBaseEx> kv in ProcessModuleManager.Modules)
            {
                ProcessModuleBaseEx m = kv.Value;
                if (m == null || !m.bInitOK) continue;

                // 创建承载面板
                Panel host = new Panel();
                host.Dock = DockStyle.Fill;
                host.BackColor = System.Drawing.Color.White;

                // 创建 TabPage
                TabPage page = new TabPage(m.GetInfo());
                page.Padding = new Padding(0);
                page.Controls.Add(host);

                // 插入到参数页之前
                _tab.TabPages.Insert(insertIndex, page);
                insertIndex++;

                // 调用模组的 ShowRunForm 将 RunForm 嵌入面板
                // （ShowRunForm 内部设置 TopLevel=false + Dock=Fill，确保不可拖拽）
                m.ShowRunForm(host);
            }

            // 默认选中第一个运行界面页
            if (_tab.TabPages.Count > 0)
                _tab.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_module == null) return;
            bool ok = _module.Save();
            MessageBox.Show(ok ? "参数保存成功。" : "参数保存失败，请查看报警信息。",
                _module.processModuleName,
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
    }
}
