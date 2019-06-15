using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DevelopmentSupport.Common.Converters
{
    /// <summary>
    /// bool値をVisibilityに変換するコンバータ
    /// </summary>
    class BoolToVisivilityConverter : IValueConverter
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
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
