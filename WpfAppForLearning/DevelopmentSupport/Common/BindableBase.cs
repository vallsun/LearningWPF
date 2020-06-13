using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentSupport.Common
{
	/// <summary>
	/// プロパティをバインド可能なクラス
	/// </summary>
	public class BindableBase : INotifyPropertyChanged
	{
		public BindableBase()
		{

		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value))
			{
				return false;
			}
			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}
}
