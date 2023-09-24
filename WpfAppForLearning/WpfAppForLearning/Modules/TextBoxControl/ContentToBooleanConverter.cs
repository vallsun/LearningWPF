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
			ContentControl str = value as ContentControl;
			return str.Content.ToString() == "True";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
