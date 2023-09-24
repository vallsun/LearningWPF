using System;
using System.Collections.Generic;
using System.Windows;

namespace WPFAppFrameWork.Common.Service
{
	/// <summary>
	/// ウィンドウに関する機能を提供するサービス
	/// </summary>
	/// <typeparam name="TWindow"></typeparam>
	/// <typeparam name="TWindowViewModel"></typeparam>
	public class WindowService : IWindowService
	{
		private Dictionary<Type, Type> m_WindowDictionary = new Dictionary<Type, Type>();

		public Window Owner { get; set; }

		/// <summary>
		/// ウィンドウとVMを登録する
		/// </summary>
		/// <param name="w"></param>
		/// <param name="vm"></param>
		void IWindowService.Register<TWindow, TWindowViewModel>()
		{
			m_WindowDictionary.Add(typeof(TWindowViewModel), typeof(TWindow));
		}

		bool? IWindowService.ShowDialog(ViewModelBase vm)
		{
			var w = CreateWindowInstance(vm);
			return w.ShowDialog();
		}

		void IWindowService.Show(ViewModelBase vm)
		{
			var w = CreateWindowInstance(vm);
			w.Show();
		}

		private Window? CreateWindowInstance(ViewModelBase vm)
		{
			if (!m_WindowDictionary.ContainsKey(vm.GetType()))
			{
				return null;
			}
			var windowType = m_WindowDictionary[vm.GetType()];
			if (!(Activator.CreateInstance(windowType) is Window w))
			{
				return null;
			}
			w.Owner = Owner;
			w.DataContext = vm;

			return w;
		}
	}
}
