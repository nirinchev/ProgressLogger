using System;
using Xamarin.Forms;
using System.Globalization;

namespace ProgressLogger.Converters
{
	public class BooleanToStarConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = value is bool && (bool)value;
			return status ? "★" : "☆";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;
			return str != null && str == "★";
		}
	}
}