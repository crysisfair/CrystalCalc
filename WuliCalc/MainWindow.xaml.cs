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
            NumberPanel p = new NumberPanel(_DataWidth);
            SetOperands(1);
            SetUiWidth(p.GetActualWidth());
        }

        protected void SetUiWidth(double width)
        {
            contentGrid.Width = width;
            mainGrid.Width = contentGrid.Width;
            mainMenu.Width = mainGrid.Width;
        }
    
        protected void SetDataWidth(int dataWidth)
        {
            double? panelWidth = null;
            double newHeight = mainMenu.Height;
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
                SetUiWidth((double)panelWidth);
            }
        }

        protected void SetOperands(int operands)
        {
            if(operands > 0)
            {
                int curCount = contentGrid.Children.Count;
                if(operands > curCount)
                {
                    for(int i = 0; i < operands - curCount; i += 1)
                    {
                        RowDefinition row = new RowDefinition();
                        contentGrid.RowDefinitions.Add(row);
                        NumberPanel p = new NumberPanel(_DataWidth);
                        contentGrid.Children.Add(p);
                        Grid.SetRow(p, contentGrid.RowDefinitions.Count - 1);
                    }
                }
                else
                {
                    if(curCount > 0)
                    {
                        contentGrid.Children.RemoveRange(operands - 1, curCount - operands);
                    }
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
