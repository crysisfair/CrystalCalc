using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WuliCalc
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WuliCalc"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WuliCalc;assembly=WuliCalc"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:NumberPanel/>
    ///
    /// </summary>
    public class NumberPanel : Grid 
    {
        Number _N;
        int _ID;

        public int DataWidth { get; set; } 

        // |--------------------------------------------------------------------|
        // |                                                                    |
        // |                 |----------------------------------------          |
        // |                 |                   ----------                     |
        // |_GridLeftPadding | _GridLeftMargin  |          |                    |
        // |                 |                   ----------                     |
        // |                 |----------------------------------------          |
        // |                                                                    |
        // |--------------------------------------------------------------------|
        float _TextBoxWidth = 25.0F;
        float _TextBoxHeight = 10.0F;
        float _TextBoxLeftMargin= 2.5F;
        float _TextBoxRightMargin = 2.5F;
        float _TextBoxTopMargin = 10.0F;
        float _TextBoxBottomMargin = 10.0F;
        float _GridLeftPadding = 10.0F;
        float _GridRightPadding = 10.0F;
        float _GridLeftMargin = 10.0F;
        float _GridRightMargin = 10.0F;
        float _GridTopMargin = 10.0F;
        float _GridBottomMargin = 10.0F;

        public NumberPanel(int width) : base()
        {
            DataWidth = width;
            Height = CalcGridHeight();
            Width = CalcGridWidth(DataWidth);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            List<TextBox> tbs = new List<TextBox>();
            for(int i = 0; i < DataWidth; i += 1)
            {
                TextBox tb = new TextBox();
                tb.Text = i.ToString();
                tbs.Add(tb);
                ColumnDefinition col = new ColumnDefinition();
                ColumnDefinitions.Add(col);
                SetColumn(tb, i);
                Children.Add(tb);
            }
        }

        protected float CalcGridWidth(int width)
        {
            return _GridLeftPadding + width * (_TextBoxLeftMargin + _TextBoxWidth + _TextBoxRightMargin) + _GridRightPadding;
        }

        protected float CalcGridHeight()
        {
            return _TextBoxTopMargin + _TextBoxHeight + _TextBoxBottomMargin;
        }

        protected float CalcPanelWidth(int width)
        {
            return _GridLeftMargin + CalcGridWidth(width) + _GridRightMargin;
        }

        protected float CalcPanelHeight(int width)
        {
            return _GridTopMargin + CalcGridHeight() + _GridBottomMargin;
        }

    }
}
