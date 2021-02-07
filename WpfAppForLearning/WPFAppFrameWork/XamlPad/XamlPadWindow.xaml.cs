﻿using System;
using System.Windows;

namespace WPFAppFrameWork.XamlPad
{
	/// <summary>
	/// XamlPadWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class XamlPadWindow : Window
	{
		public XamlPadWindow()
		{
			InitializeComponent();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			m_XamlPad.SaveXamlText();
		}
	}
}