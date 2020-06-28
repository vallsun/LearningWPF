using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFAppFrameWork.Ribbon
{
	/// <summary>
	/// リボンのボタン
	/// </summary>
	public class RibbonButton : Button
    {
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(RibbonButton), new PropertyMetadata(null));
        static RibbonButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonButton), new FrameworkPropertyMetadata(typeof(RibbonButton)));
        }
    }
}
