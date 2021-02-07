using System.Windows;
using WPFAppFrameWork.XamlPad;

namespace XamlPadTestDriver
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var window = new XamlPadWindow();
			window.Show();
		}
	}
}
