using System;
using Xamarin.Forms;
using ProgressLogger.Models;
using System.Linq;

namespace ProgressLogger.Converters
{
	public class SeriesToProgressConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var series = value as SeriesInfo;
			if (series != null)
			{
				var eps = series.SeasonInfoes?.Where(s => s != null && s.Episodes != null)
								.SelectMany(s => s.Episodes)
								.Where(e => e != null) ?? Enumerable.Empty<EpisodeInfo>();
				var completeCount = eps.Count(e => e.Status == WatchStatus.Complete);

				var totalCount = eps.Count();

				var completion = 1.0 * completeCount / totalCount;
				return $"{completeCount}/{totalCount} ({completion:p})";
			}

			return "unknown";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}