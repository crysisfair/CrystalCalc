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
            DataWidth = width;
            _N = new Number(0, DataWidth);
            _N = (Number)~_N;
            RowDefinition rowHeader = new RowDefinition();
            RowDefinitions.Add(rowHeader);
            Height = CalcPanelHeight();
            Margin = new Thickness(_GridLeftMargin, _GridTopMargin, _GridRightMargin, _GridBottomMargin);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            RowDefinition rowLabel = new RowDefinition();
            RowDefinitions.Add(rowLabel);
            RowDefinition rowTb = new RowDefinition();
            RowDefinitions.Add(rowTb);
            Render(width);
        }

        public void Render(int width)
        {
            DataWidth = width;
            _N.SetNewWidth(width);
            Children.Clear();
            ColumnDefinitions.Clear();
            Width = CalcPanelWidth(DataWidth);

            Label value = new Label();
            value.Content = String.Format("Width: {0} Value: {1}", width.ToString(), _N.ToString()) ;
            SetRow(value, 0);
            SetColumnSpan(value, width);
            Children.Add(value);

            for(int i = 0; i < DataWidth; i += 1)
            {
                Label lb = new Label();
                lb.Content = (width - 1 - i).ToString();
                lb.VerticalContentAlignment = VerticalAlignment.Center;
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.Height = _LabelHeight;
                lb.Width = _LabelWidth;
                SetColumn(lb, i);
                SetRow(lb, 1);

                TextBox tb = new TextBox();
                tb.Text = _N.GetBit(i).ToString();
                tb.HorizontalContentAlignment = HorizontalAlignment.Center;
                tb.VerticalContentAlignment = VerticalAlignment.Center;
                tb.Height = _TextBoxHeight;
                tb.Width = _TextBoxWidth;
                tb.Tag = (width - 1 - i).ToString();
                tb.Margin = new Thickness(_TextBoxLeftMargin, _TextBoxTopMargin, _TextBoxRightMargin, _TextBoxBottomMargin);
                tb.KeyDown += OnTextBoxKeyPress;
                tb.PreviewMouseDown += OnTextBoxMouseClick;
                ColumnDefinition col = new ColumnDefinition();
                ColumnDefinitions.Add(col);
                SetColumn(tb, i);
                SetRow(tb, 2);


                Children.Add(tb);
                Children.Add(lb);
            }
        }

        private void OnTextBoxMouseClick(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = int.Parse(tb.Tag.ToString());
            _N.RevertBit(id);
            Render(DataWidth);
            e.Handled = true;
        }

        private void OnTextBoxKeyPress(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if(e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                tb.Text = "1";
            }
            else if(e.Key == Key.D0 || e.Key == Key.NumPad0)
            {
                tb.Text = "0";
            }
            e.Handled = true;
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
            return 3 * (_TextBoxTopMargin + _TextBoxHeight + _TextBoxBottomMargin);
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
            Pen pen = new Pen(Brushes.DarkGray, _BorderThickness);
            dc.DrawRoundedRectangle(null, pen, rect, _GridRoundSize, _GridRoundSize);
            dc.DrawLine(pen, new Point(0, CalcGridHeight() / 3), new Point(CalcPanelWidth(DataWidth), CalcPanelHeight() / 3));
        }
    }
}
