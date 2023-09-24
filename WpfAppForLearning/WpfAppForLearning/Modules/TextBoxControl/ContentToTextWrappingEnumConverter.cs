using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfAppForLearning.Modules.TextBoxControl
{
	public class ContentToTextWrappingEnumConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				//規定値を返す
				return TextWrapping.NoWrap;
			}

			if (!(value is ContentControl content))
			{
				//規定値を返す
				return TextWrapping.NoWrap;
			}

			return content.Content.ToString() switch
			{
				"NoWrap" => TextWrapping.NoWrap,
				"Wrap" => TextWrapping.Wrap,
				"WrapWithOverflow" => TextWrapping.WrapWithOverflow,
				_ => TextWrapping.NoWrap,
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
