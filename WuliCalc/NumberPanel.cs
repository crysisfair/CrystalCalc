using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        bool _DecTbEdit = false;
        bool _HexTbEdit = false;
        bool _IsTextBoxHandled = false;
        int _LastHandledTextBoxId = 0;

        public int DataWidth { get; set; } 

        public Number N { get; }

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

        SolidColorBrush _TextBoxOneBg = Brushes.BlueViolet;
        SolidColorBrush _TextBoxZeroBg = Brushes.White;

        public NumberPanel(int width, int id) : base()
        {
            DataWidth = width;
            _ID = id;
            _N = new Number(0, DataWidth);
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

        protected int Min(int n1, int n2)
        {
            return (n1 < n2) ? n1 : n2;
        }


        public void Render(int width)
        {
            DataWidth = width;
            _N.SetNewWidth(width);
            Children.Clear();
            ColumnDefinitions.Clear();
            Width = CalcPanelWidth(DataWidth);

            Label hexLb = new Label();
            hexLb.Content = "HEX: ";
            hexLb.HorizontalAlignment = HorizontalAlignment.Right;
            Label decLb = new Label();
            decLb.Content = "DEC: ";
            decLb.HorizontalAlignment = HorizontalAlignment.Right;
            TextBox hexTb = new TextBox();
            hexTb.Text = _N.ToString("X");
            TextBox decTb = new TextBox();
            decTb.Text = _N.ToString("D");

            int hexLbStart = 0;
            int hexLbWidth = Min(3, width / 3);
            int hexTbStart = hexLbStart + hexLbWidth;
            int hexTbWidth = Min(8, width / 3);
            int decLbStart = hexTbStart + hexTbWidth;
            int decLbWidth = Min(3, width / 3);
            int decTbStart = decLbStart + decLbWidth;
            int decTbWidth = hexTbWidth;
            SetRow(hexLb, 0);
            SetRow(hexTb, 0);
            SetRow(decLb, 0);
            SetRow(decTb, 0);
            SetColumn(hexLb, hexLbStart);
            SetColumnSpan(hexLb, hexLbWidth);
            SetColumn(hexTb, hexTbStart);
            SetColumnSpan(hexTb, hexTbWidth);
            SetColumn(decLb, decLbStart);
            SetColumnSpan(decLb, decLbWidth);
            SetColumn(decTb, decTbStart);
            SetColumnSpan(decTb, decTbWidth);
            Children.Add(hexLb);
            Children.Add(hexTb);
            Children.Add(decLb);
            Children.Add(decTb);
            hexTb.KeyDown += HexTb_KeyDown;
            hexTb.TextChanged += HexTb_TextChanged;
            decTb.KeyDown += DecTb_KeyDown;
            decTb.TextChanged += DecTb_TextChanged;

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
                if(tb.Text == "1")
                {
                    tb.Background = _TextBoxOneBg;
                }
                else
                {
                    tb.Background = _TextBoxZeroBg;
                }
                tb.HorizontalContentAlignment = HorizontalAlignment.Center;
                tb.VerticalContentAlignment = VerticalAlignment.Center;
                tb.Height = _TextBoxHeight;
                tb.Width = _TextBoxWidth;
                tb.Tag = (width - 1 - i).ToString();
                tb.Margin = new Thickness(_TextBoxLeftMargin, _TextBoxTopMargin, _TextBoxRightMargin, _TextBoxBottomMargin);
                tb.KeyDown += OnTextBoxKeyPress;
                tb.PreviewMouseDown += OnTextBoxMouseClick;
                tb.MouseEnter += OnTextBoxMouseEnter;
                tb.ContextMenu = null;
                ColumnDefinition col = new ColumnDefinition();
                ColumnDefinitions.Add(col);
                SetColumn(tb, i);
                SetRow(tb, 2);


                Children.Add(tb);
                Children.Add(lb);
            }

            if(_DecTbEdit == true)
            {
                _DecTbEdit = false;
                decTb.Focus();
                decTb.SelectionStart = decTb.Text.Length;
            }
            if(_HexTbEdit == true)
            {
                _HexTbEdit = false;
                hexTb.Focus();
                hexTb.SelectionStart = hexTb.Text.Length;
            }
        }

        private void OnTextBoxMouseEnter(object sender, MouseEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                TextBox tb = sender as TextBox;
                int id = int.Parse(tb.Tag.ToString());
                if(id != _LastHandledTextBoxId || _IsTextBoxHandled == false)
                {
                    _LastHandledTextBoxId = id;
                    _IsTextBoxHandled = true;
                    _N.RevertBit(id);
                    Render(DataWidth);
                }
                e.Handled = true;
            }
        }

        private void HexTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            _N = new Number(tb.Text, 16, DataWidth);
            _HexTbEdit = true;
            Render(DataWidth);
        }

        private void DecTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            _N = new Number(tb.Text, 10, DataWidth);
            _DecTbEdit = true;
            Render(DataWidth);
        }

        private void DecTb_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void HexTb_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.A && e.Key <= Key.F) || e.Key == Key.Delete)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void OnTextBoxMouseClick(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                TextBox tb = sender as TextBox;
                int id = int.Parse(tb.Tag.ToString());
                _N.RevertBit(id);
                Render(DataWidth);
                e.Handled = true;
            }
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

        public double CalcActualHeight()
        {
            return CalcPanelHeight();
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
