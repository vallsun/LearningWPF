using System;
using System.Globalization;
using System.Windows.Data;
using ApplicationMode = DevelopmentSupportTool.ViewModels.ApplicationMode;

namespace DevelopmentSupportTool.Converters
{
	class ApplicationModeToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
			{
				return false;
			}
			var paramString = parameter.ToString();
			var valueString = value.ToString();
			return paramString == valueString;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool) || parameter == null)
			{
				return null;
			}

			if ((bool)value)
			{
				return Enum.Parse(typeof(ApplicationMode), parameter.ToString());
			}

			return null;
		}
	}
}
