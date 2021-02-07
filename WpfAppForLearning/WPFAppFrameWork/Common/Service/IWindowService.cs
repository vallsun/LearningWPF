using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WPFAppFrameWork.Common.Service
{
	public interface IWindowService
	{
		/// <summary>
		/// ウィンドウとVMを登録する
		/// </summary>
		/// <typeparam name="TWindow"></typeparam>
		/// <typeparam name="TWindowViewModel"></typeparam>
		public void Register<TWindow, TWindowViewModel>()
			where TWindow : Window
			where TWindowViewModel : ViewModelBase;

		/// <summary>
		/// ウィンドウを表示する
		/// </summary>
		/// <param name="vm"></param>
		/// <returns></returns>
		bool? ShowDialog(ViewModelBase vm);

		/// <summary>
		/// ウィンドウを表示する
		/// </summary>
		/// <param name="vm"></param>
		void Show(ViewModelBase vm);
	}
}
