using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFAppFrameWork.Common.Service;
using WPFAppFrameWork.XamlPad;

namespace WpfAppForLearning
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static IWindowService WindowService = new WindowService();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            // フォントを設定
            var font = new System.Windows.Media.FontFamily("メイリオ");
            var style = new Style(typeof(Window));
            style.Setters.Add(new Setter(Window.FontFamilyProperty, font));
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(style));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WindowService.Register<XamlPadWindow, XamlPadWindowViewModel>();
        }
    }
}
