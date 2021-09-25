using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using WPFAppFrameWork.Common;

namespace WPFAppFrameWork.XamlPad
{
	public class XamlPadWindowViewModel : ViewModelBase
	{
		#region 定数定義

		/// <summary>
		/// Xaml形式のテキストファイル
		/// </summary>
		private static readonly string fileName = Environment.CurrentDirectory + @"\win.xaml";

		#endregion

		#region 内部フィールド

		/// <summary>
		/// Xamlコード
		/// </summary>
		private string m_XamlText;

		/// <summary>
		/// エラーメッセージ
		/// </summary>
		private string m_ErrorMessage;

		#endregion

		#region プロパティ

		/// <summary>
		/// Xamlコード
		/// </summary>
		public string XamlText
		{
			get { return m_XamlText; }
			set { SetProperty(ref m_XamlText, value); }
		}

		/// <summary>
		/// エラーメッセージ
		/// </summary>
		public string ErrorMessage
		{
			get { return m_ErrorMessage; }
			set { SetProperty(ref m_ErrorMessage, value); }
		}

		/// <summary>
		/// 実行コマンド
		/// </summary>
		public DelegateCommand RunCommand { get; protected set; }

		#endregion

		#region 構築・消滅

		public XamlPadWindowViewModel()
			: base(null)
		{
			Initialize();
		}

		protected override void RegisterCommands()
		{
			base.RegisterCommands();
			RunCommand = new DelegateCommand(Run, CanRun);
		}

		#endregion

		#region 公開サービス

		/// <summary>
		/// Xaml形式のテキストをロードする
		/// </summary>
		public void LoadXamlText()
		{
			if (File.Exists(fileName))
				XamlText = File.ReadAllText(fileName);
		}

		/// <summary>
		/// Xaml形式のテキストをセーブする
		/// </summary>
		public void SaveXamlText()
		{
			File.WriteAllText(fileName, XamlText);
		}

		#endregion

		#region 内部処理

		private void Initialize()
		{
			LoadXamlText();
		}

		private bool CanRun()
		{
			return true;
		}

		private void Run()
		{
			File.WriteAllText(fileName, m_XamlText);
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
				ErrorMessage += DateTime.Now + "  ";
				ErrorMessage += ex.Message + Environment.NewLine;
			}
		}

		#endregion
	}
}
