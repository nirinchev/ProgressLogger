using System.Globalization;

namespace ProgressLogger.Models
{
	public static class SeriesInfoExtensions
	{
		public static bool Filter(this SeriesInfo info, string query)
		{
			return string.IsNullOrEmpty(query) ||
				   CultureInfo.InvariantCulture.CompareInfo.IndexOf(info.Name, query, CompareOptions.OrdinalIgnoreCase) > -1;
		}

		public static bool IsComplete(this SeriesInfo info)
		{
			return !string.IsNullOrEmpty(info.Genres);
		}
	}
}