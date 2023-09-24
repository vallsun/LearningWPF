using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfAppForLearning.Modules.KeyboardNavigation
{
	class StringToNavigationModeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string inputString = value as string;
			return inputString switch
			{
				"Local" => KeyboardNavigationMode.Local,
				"Continue" => KeyboardNavigationMode.Continue,
				"Contained" => KeyboardNavigationMode.Contained,
				"Cycle" => KeyboardNavigationMode.Cycle,
				"Once" => KeyboardNavigationMode.Once,
				"None" => KeyboardNavigationMode.None,
				_ => KeyboardNavigationMode.Continue,
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
