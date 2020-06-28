using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFAppFrameWork.Common.Converters
{
	/// <summary>
	/// bool値を反転するコンバータ
	/// </summary>
	public class ReverseBooleanConverter : IValueConverter
	{
		/// <summary>
		/// bool値を反転する
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is bool a)
			{
				return a ? Visibility.Collapsed : Visibility.Visible;
			}
			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
