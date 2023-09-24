using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAppForLearning.Modules.TextBoxControl
{
	public class ContentToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			var str = value as ContentControl;
			var ret = str.Content.ToString() == "True" ? true : false;
			return ret;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
