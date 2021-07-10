using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WPFAppFrameWork.XamlPad
{
	/// <summary>
	/// XamlPad.xaml の相互作用ロジック
	/// </summary>
	public partial class XamlPad : UserControl
	{
		#region 定数定義

		/// <summary>
		/// Xaml形式のテキストファイル
		/// </summary>
		private static readonly string fileName = Environment.CurrentDirectory + @"\win.xaml";

		#endregion

		#region 構築・消滅

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public XamlPad()
		{
			InitializeComponent();
		}

		#endregion

		#region イベントハンドラ

		/// <summary>
		/// ウィンドウを表示する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowWindow(object sender, RoutedEventArgs e)
		{
			File.WriteAllText(fileName, inputXmlTextBox.Text);
			Window myWindow = null;
			try
			{
				using (Stream sr = File.Open(fileName, FileMode.Open))
				{
					myWindow = (Window)XamlReader.Load(sr);
					myWindow.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				errorTextBlock.Text += DateTime.Now + "  ";
				errorTextBlock.Text += ex.Message + Environment.NewLine;
			}
		}

		/// <summary>
		/// コントロールが読み込まれたときの処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			LoadXamlText();
		}

		#endregion

		#region 公開サービス

		/// <summary>
		/// Xaml形式のテキストをロードする
		/// </summary>
		public void LoadXamlText()
		{
			if (File.Exists(fileName))
				inputXmlTextBox.Text = File.ReadAllText(fileName);
		}

		/// <summary>
		/// Xaml形式のテキストをセーブする
		/// </summary>
		public void SaveXamlText()
		{
			File.WriteAllText(fileName, inputXmlTextBox.Text);
		}

		#endregion
	}
}
