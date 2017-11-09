using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WuliCalc
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        protected int _DataWidth = 32;

        public MainWindow()
        {
            InitializeComponent();
            NumberPanel p = new NumberPanel(_DataWidth, 0);
            SetOperands(1);
        }

        protected void SetUiWidthHeight(double width, double height)
        {
            contentGrid.Width = width;
            contentGrid.Height = height;
            mainMenu.Width = width;
            mainGrid.Width = contentGrid.Width;
            mainMenu.Width = mainGrid.Width;
        }
    
        protected void SetDataWidth(int dataWidth)
        {
            double? panelWidth = null;
            double newHeight = 0.0f;
            _DataWidth = dataWidth;
            foreach(UIElement e in contentGrid.Children)
            {
                NumberPanel p = e as NumberPanel;
                newHeight += p.Height;
                p.Render(dataWidth);
                panelWidth = p.GetActualWidth();
            }
            if(panelWidth > 0.0f)
            {
                SetUiWidthHeight((double)panelWidth, newHeight);
            }
        }

        protected void SetOperands(int operands)
        {
            if(operands > 0)
            {
                int curCount = contentGrid.Children.Count;
                NumberPanel p0 = new NumberPanel(_DataWidth, 0);
                double height = p0.CalcActualHeight() * curCount;
                if(operands > curCount)
                {
                    double width = 0.0f;
                    for(int i = 0; i < operands - curCount; i += 1)
                    {
                        RowDefinition row = new RowDefinition();
                        contentGrid.RowDefinitions.Add(row);
                        NumberPanel p = new NumberPanel(_DataWidth, curCount + i);
                        width = p.GetActualWidth();
                        contentGrid.Children.Add(p);
                        Grid.SetRow(p, contentGrid.RowDefinitions.Count - 1);
                        height += p.Height;
                    }
                    SetUiWidthHeight(width, height);
                }
                else
                {
                    if(curCount > 0)
                    {
                        contentGrid.Children.RemoveRange(operands - 1, curCount - operands);
                        contentGrid.RowDefinitions.RemoveRange(operands - 1, curCount - operands);
                        NumberPanel p = new NumberPanel(_DataWidth, 0);
                        SetUiWidthHeight(contentGrid.ActualWidth, contentGrid.ActualHeight - p.Height * (curCount - operands));
                    }
                }
                for(int i = 0; i < contentGrid.Children.Count; i += 1)
                {
                    NumberPanel p = contentGrid.Children[i] as NumberPanel;
                    Grid.SetRow(p, i);
                }
            }
        }

        private void DataWidthMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            int width = 0;
            if(int.TryParse(m.Header.ToString(), out width) == true)
            {
                SetDataWidth(width);
            }
        }

        private void OperandsMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = sender as MenuItem;
            int operands = 1;
            if(int.TryParse(m.Header.ToString(), out operands) == true)
            {
                SetOperands(operands);
            }
        }

        private void StayOnTop_Click(object sender, RoutedEventArgs e)
        {
            if (this.Topmost == true)
            {
                this.Topmost = false;
            }
            else
            {
                this.Topmost = true;
            }
        }
    }
}
