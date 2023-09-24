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

			var content = value as ContentControl;

			if (content == null)
			{
				//規定値を返す
				return TextWrapping.NoWrap;
			}

			switch (content.Content.ToString())
			{
				case "NoWrap":
					return TextWrapping.NoWrap;
				case "Wrap":
					return TextWrapping.Wrap;
				case "WrapWithOverflow":
					return TextWrapping.WrapWithOverflow;
				default:
					return TextWrapping.NoWrap;
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
