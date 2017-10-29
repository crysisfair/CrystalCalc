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
    public class NumberPanel : Control
    {
        Number _N;
        public static readonly DependencyProperty DataWidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(NumberPanel),
            new FrameworkPropertyMetadata(Number.DefaultWidth, new PropertyChangedCallback(WidthPropertyChangedCallback)));

        static NumberPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberPanel), new FrameworkPropertyMetadata(typeof(NumberPanel)));
        }

        private static void WidthPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is NumberPanel)
            {
                NumberPanel panel = sender as NumberPanel;
                panel.OnWidthUpdated((int)arg.OldValue, (int)arg.NewValue);

            }
        }

        [Description("Get current width")]
        [Category("Common Properties")]
        public int DataWidth
        {
            get
            {
                return (int)this.GetValue(DataWidthProperty);
            }
            set
            {
                this.SetValue(DataWidthProperty, value);
            }
        }

        [Description("Happen after width is changed")]
        public event RoutedPropertyChangedEventHandler<int> WidthUpdated
        {
            add
            {
                this.AddHandler(WidthUpdatedEvent, value);
            }
            remove
            {
                this.RemoveHandler(WidthUpdatedEvent, value);
            }
        }

        public static readonly RoutedEvent WidthUpdatedEvent =
           EventManager.RegisterRoutedEvent("WidthUpdated",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumberPanel));

        protected virtual void OnWidthUpdated(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg =
                new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, WidthUpdatedEvent);
            this.RaiseEvent(arg);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            // Add render logic here
        }
    }
}
