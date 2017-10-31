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
        double _TextBoxHeight = 25.0F;
        double _TextBoxWidth = 25.0F;
        double _TextBoxLeftMargin= 0F;
        double _TextBoxRightMargin = 0F;
        double _TextBoxTopMargin = 0F;
        double _TextBoxBottomMargin = 0F;

        double _LabelHeight = 25.0F;
        double _LabelWidth = 25.0F;

        double _GridLeftPadding = 0.0F;
        double _GridRightPadding = 0.0F;
        double _GridLeftMargin = 0.0F;
        double _GridRightMargin = 0.0F;
        double _GridTopMargin = 0.0F;
        double _GridBottomMargin = 0.0F;

        double _BorderThickness = 1.0F;
        double _GridRoundSize = 1.0F;

        public NumberPanel(int width) : base()
        {
            Render(width);
        }

        public void Render(int width)
        {
            DataWidth = width;
            Children.Clear();
            Height = CalcPanelHeight();
            Width = CalcPanelWidth(DataWidth);
            Margin = new Thickness(_GridLeftMargin, _GridTopMargin, _GridRightMargin, _GridBottomMargin);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            RowDefinition rowLabel = new RowDefinition();
            RowDefinitions.Add(rowLabel);
            RowDefinition rowTb = new RowDefinition();
            RowDefinitions.Add(rowTb);

            List<TextBox> tbs = new List<TextBox>();
            for(int i = 0; i < DataWidth; i += 1)
            {
                TextBox tb = new TextBox();
                tb.Text = i.ToString();
                tb.HorizontalContentAlignment = HorizontalAlignment.Center;
                tb.VerticalContentAlignment = VerticalAlignment.Center;
                tb.Height = _TextBoxHeight;
                tb.Width = _TextBoxWidth;
                tb.Margin = new Thickness(_TextBoxLeftMargin, _TextBoxTopMargin, _TextBoxRightMargin, _TextBoxBottomMargin);
                tbs.Add(tb);
                ColumnDefinition col = new ColumnDefinition();
                ColumnDefinitions.Add(col);
                SetColumn(tb, i);
                SetRow(tb, 1);

                Label lb = new Label();
                lb.Content = i.ToString();
                lb.VerticalContentAlignment = VerticalAlignment.Center;
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.Height = _LabelHeight;
                lb.Width = _LabelWidth;
                SetColumn(lb, i);
                SetRow(lb, 0);

                Children.Add(tb);
                Children.Add(lb);
            }
        }

        public double GetActualWidth()
        {
            return CalcPanelWidth(DataWidth);
        }

        protected double CalcGridWidth(int width)
        {
            return _GridLeftPadding + width * (_TextBoxLeftMargin + _TextBoxWidth + _TextBoxRightMargin) + _GridRightPadding;
        }

        protected double CalcGridHeight()
        {
            return 2 * (_TextBoxTopMargin + _TextBoxHeight + _TextBoxBottomMargin);
        }

        protected double CalcPanelWidth(int width)
        {
            return _GridLeftMargin + CalcGridWidth(width) + _GridRightMargin;
        }

        protected double CalcPanelHeight()
        {
            return _GridTopMargin + CalcGridHeight() + _GridBottomMargin;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Rect rect = new Rect(new Size(CalcPanelWidth(DataWidth), CalcPanelHeight()));
            Pen pen = new Pen(Brushes.Black, _BorderThickness);
            dc.DrawRoundedRectangle(null, pen, rect, _GridRoundSize, _GridRoundSize);
        }
    }
}
