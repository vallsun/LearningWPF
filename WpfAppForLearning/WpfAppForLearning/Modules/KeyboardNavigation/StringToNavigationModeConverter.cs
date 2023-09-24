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
			var inputString = value as string;
			switch (inputString)
			{
				case "Local":
					return KeyboardNavigationMode.Local;
				case "Continue":
					return KeyboardNavigationMode.Continue;
				case "Contained":
					return KeyboardNavigationMode.Contained;
				case "Cycle":
					return KeyboardNavigationMode.Cycle;
				case "Once":
					return KeyboardNavigationMode.Once;
				case "None":
					return KeyboardNavigationMode.None;
				default:
					return KeyboardNavigationMode.Continue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
