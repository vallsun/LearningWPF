﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFAppFrameWork
{
	public class TreeItemLeftMarginConverter : IValueConverter
	{

		public double Length { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is TreeViewItem item))
				return new Thickness(0);

			return new Thickness(Length * item.GetDepth(), 0, 0, 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
