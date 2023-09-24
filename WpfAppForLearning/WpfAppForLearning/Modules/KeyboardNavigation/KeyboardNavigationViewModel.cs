using System;
using System.Windows;
using DevelopmentCommon.Common;
using WpfAppForLearning.ViewModel;
using WPFAppFrameWork;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
	/// <summary>
	/// KeyboardNavigationコンテンツのVM
	/// </summary>
	internal class KeyboardNavigationViewModel : ContentViewModel
	{
		#region フィールド

		private string n_NavigationMode1 = "Coontained";
		private string n_NavigationMode2 = "Coontained";

		#endregion

		#region プロパティ

		public string NavigationMode1 { get { return n_NavigationMode1; } set { SetProperty(ref n_NavigationMode1, value); } }
		public string NavigationMode2 { get { return n_NavigationMode2; } set { SetProperty(ref n_NavigationMode2, value); } }

		public DelegateCommand<Uri> NavigateCommand { get; set; }

		#endregion

		#region 構築・消滅

		public KeyboardNavigationViewModel()
			: base(null)
		{

		}

		/// <summary>
		/// コマンドの初期化
		/// </summary>
		protected override void RegisterCommands()
		{
			base.RegisterCommands();

			NavigateCommand = new DelegateCommand<Uri>(Navigate, CanNavigate);
		}

		#endregion

		#region 内部処理

		/// <summary>
		/// 指定されたURLへナビゲート可能か
		/// </summary>
		/// <param name="url">URL</param>
		private bool CanNavigate(Uri url)
		{
			if (url == null)
			{
				return false;
			}

			return true;

		}

		/// <summary>
		/// 指定されたURLへナビゲートする
		/// </summary>
		/// <param name="url">URL</param>
		private void Navigate(Uri url)
		{
			try
			{
				ProcessService.Navigate(url);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, App.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		#endregion
	}
}
