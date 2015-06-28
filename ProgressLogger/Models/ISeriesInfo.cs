using System.Globalization;

namespace ProgressLogger
{
	public interface ISeriesInfo
	{
		int Id { get; }

		string Name { get; }

		string PosterUrl { get; }
	}

	public class BasicSeriesInfo : ISeriesInfo
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string PosterUrl { get; set; }
	}

	public static class SeriesInfoExtensions
	{
		public static bool Filter(this ISeriesInfo info, string query)
		{
			return string.IsNullOrEmpty(query) ||
				   CultureInfo.InvariantCulture.CompareInfo.IndexOf(info.Name, query, CompareOptions.OrdinalIgnoreCase) > -1;
		}
	}
}