using System;
using System.Collections.Generic;
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
using WPFAppFrameWork.SoundPlayer.ViewModel;

namespace WPFAppFrameWork.SoundPlayer.View
{
	/// <summary>
	/// SounPlayerControl.xaml の相互作用ロジック
	/// </summary>
	public partial class SounPlayerControl : UserControl
	{
		public SounPlayerControl()
		{
			InitializeComponent();



		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var vm = m_MediaElement.DataContext as SoundPlayerViewModel;
			if (vm != null)
			{
				vm.PlayRequested += (sender, e) =>
				{
					m_MediaElement.Play();
				};

				vm.StopRequested += (sender, e) =>
				{
					m_MediaElement.Stop();
				};

				vm.PauseRequested += (sender, e) =>
				{
					m_MediaElement.Pause();
				};
			}
		}
	}
}
