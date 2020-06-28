using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFAppFrameWork.Common.Converters
{
    /// <summary>
    /// bool値をVisibilityに変換するコンバータ
    /// </summary>
	public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// bool値をVisivbilityに変換するコンバータ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool a)
            {
                return a ? Visibility.Visible : Visibility.Collapsed;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
