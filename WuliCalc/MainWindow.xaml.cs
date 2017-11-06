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
        public MainWindow()
        {
            InitializeComponent();
            IntPtr hWnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
            SetParent(hWnd, hWndProgMan);
            NumberPanel p = new NumberPanel(32);
            this.Width = p.GetActualWidth() + 20;
            mainMenu.Width = p.GetActualWidth();
            mainGrid.Width = p.GetActualWidth();
            contentGrid.Width = p.GetActualWidth();
            contentGrid.Children.Add(p);
        }
    
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        const int GWL_HWNDPARENT = -8;
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        protected void SetDataWidth(int dataWidth)
        {
            double? panelWidth = null;
            foreach(NumberPanel p in contentGrid.Children)
            {
                p.Render(dataWidth);
                panelWidth = p.GetActualWidth();
            }
            if(panelWidth > 0.0f)
            {
                this.Width = (double)panelWidth + 20;
                mainMenu.Width = (double)panelWidth;
                mainGrid.Width = (double)panelWidth;
                contentGrid.Width = (double)panelWidth;
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

        private void StayOnTop_Click(object sender, RoutedEventArgs e)
        {
            var handle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr hprog = FindWindowEx(
                FindWindowEx(
                    FindWindow("Progman", "Program Manager"),
                    IntPtr.Zero, "SHELLDLL_DefView", ""
                ),
                IntPtr.Zero, "SysListView32", "FolderView"
            );
            SetWindowLong(handle, GWL_HWNDPARENT, hprog);
            this.Topmost = true;
        }
    }
}
