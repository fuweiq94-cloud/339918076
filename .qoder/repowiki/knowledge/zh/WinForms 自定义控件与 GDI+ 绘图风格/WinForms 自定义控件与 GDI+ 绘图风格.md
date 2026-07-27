---
kind: frontend_style
name: WinForms 自定义控件与 GDI+ 绘图风格
category: frontend_style
scope:
    - '**'
source_files:
    - Controls/DroLabel.cs
    - Controls/JogButton.cs
    - Controls/XYView.cs
    - Controls/ZBarView.cs
    - Controls/PaintHelper.cs
    - Controls/MathHelper.cs
    - MainControl/RunForm.Designer.cs
    - PointJump/RunForm.Designer.cs
    - Trajectory/RunForm.Designer.cs
---

本仓库的前端样式完全基于 .NET WinForms + GDI+ 自绘实现，没有使用 CSS/HTML/Sass 等 Web 技术栈。UI 由一组可复用的自定义控件（Controls 目录）和三个工艺模块的窗体（MainControl、PointJump、Trajectory）组成，整体呈现工业 CNC 设备人机界面的视觉风格。

**1. 使用的系统与工具**
- 框架：Windows Forms（WinForms），通过 Designer.cs 文件声明式布局。
- 绘图：System.Drawing / System.Drawing.Drawing2D 直接绘制，未引入第三方 UI 库或主题引擎。
- 资源：*.bmp 位图作为控件工具箱图标，嵌入在 Resources 目录中。

**2. 核心样式文件与位置**
- Controls/DroLabel.cs — DRO 数字读数控件，深色背景 + 高对比绿色大字号，数值变化时黄色闪烁，支持报警阈值变红。
- Controls/JogButton.cs — 点动按钮，圆角矩形 + 渐变背景，按下/悬停/默认三种状态配色，左上角红色指示灯。
- Controls/XYView.cs — XY 平面俯视图，网格坐标、原点十字、目标点空心圆、当前点实心圆带光晕、轨迹线、预设点位菱形标记。
- Controls/ZBarView.cs — Z/U 轴竖直条形指示，渐变填充条、刻度线、目标箭头标记、底部数值显示。
- Controls/PaintHelper.cs — 统一绘图工具类，提供抗锯齿设置、背景填充、刻度步长计算、垂直条映射、居中文本等公共方法。
- Controls/MathHelper.cs — 数学辅助（如 ClampLerp 插值）。
- 各模块 RunForm.Designer.cs — 窗体布局与控件属性声明，全部采用 VS2017 兼容的纯声明式写法。

**3. 架构与约定**
- 所有自定义控件继承 System.Windows.Forms.Control，重写 OnPaint 进行自绘，启用 DoubleBuffered + ResizeRedraw 避免闪烁。
- 颜色、字体、边框等视觉样式直接在控件构造函数中硬编码，未建立集中式的 Theme/Style 配置文件。
- 绘图逻辑通过 PaintHelper 提取重复代码，确保网格、刻度、文本对齐等视觉一致性。
- 控件通过 [ToolboxBitmap]、[Category]、[DefaultValue]、[Description] 等特性暴露给 VS 设计器，支持可视化拖拽配置。
- 三个工艺模块窗体共享同一套 Controls 组件，布局结构高度一致：SplitContainer 左侧放 XYView，右侧面板按功能分组（GroupBox + TableLayoutPanel）。

**4. 观察到的约定与约束**
- 字体：主界面使用 Segoe UI（8.25F~11F），DRO 数值使用 Consolas（22F Bold），坐标信息使用 Consolas 9F Bold。
- 配色体系：背景以浅灰（#F5F7FA）为主，前景文字深灰（#3C465A），强调色为蓝色系（#2878DC）和橙色（#DC8C28），报警用红色（#FF5A5A），DRO 经典绿（#78FFB4）。
- 交互反馈：JogButton 通过 LinearGradientBrush 实现三态渐变；XYView 当前点有半透明光晕效果；DroLabel 数值变化后 200ms 内黄色闪烁。
- 绘图质量：统一调用 PaintHelper.SetupGraphics 开启 AntiAlias 和 ClearTypeGridFit。
- 设计器兼容性：Designer.cs 文件禁止使用 Lambda、集合初始化器等语法，仅逐行显式设置属性，确保 VS2017 设计器可打开。
- 无响应式/自适应布局策略，控件尺寸通过 Margin/Padding/Size 固定声明。

该风格是典型的工控上位机 WinForms 自绘方案，视觉一致性由 PaintHelper 和各控件构造函数中的硬编码样式保证，而非通过外部样式文件或主题系统管理。