using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using ProgressLogger.Models;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class ImagesConfiguration
	{
		private IEnumerable<string> posterSizes;
		private Tuple<int, string>[] posterSizeInfos;

		private IEnumerable<string> backdropSizes;
		private Tuple<int, string>[] backdropSizeInfos;

		[JsonProperty("secure_base_url")]
		public string BaseUrl { get; set; }

		[JsonProperty("poster_sizes")]
		public IEnumerable<string> PosterSizes
		{
			get
			{
				return this.posterSizes;
			}
			set
			{
				this.posterSizes = value;
				this.posterSizeInfos = this.posterSizes.Select(GetSizeInfo)
												 .OrderBy(s => s.Item1)
												 .ToArray();
			}
		}

		[JsonProperty("backdrop_sizes")]
		public IEnumerable<string> BackdropSizes
		{
			get
			{
				return this.backdropSizes;
			}
			set
			{
				this.backdropSizes = value;
				this.backdropSizeInfos = this.backdropSizes.Select(GetSizeInfo)
					.OrderBy(s => s.Item1)
					.ToArray();
			}
		}

		public string GetPosterUrl(string relative, int minWidth)
		{
			var size = this.posterSizeInfos.First(s => s.Item1 >= minWidth).Item2;
			return $"{BaseUrl}{size}{relative}";
		}

		public string GetBackdropUrl(string relative, int minWidth)
		{
			var size = this.backdropSizeInfos.First(s => s.Item1 >= minWidth).Item2;
			return $"{BaseUrl}{size}{relative}";
		}

		public void UpdateUrls(TMDbSeriesInfo info, int posterWidth, int backdropWidth)
		{
			info.PosterUrl = this.GetPosterUrl(info.PosterPath, posterWidth);
			info.BackdropUrl = this.GetBackdropUrl(info.BackdropPath, backdropWidth);

			if (info.SeasonInfoes != null)
			{
				foreach (var seasonInfo in info.SeasonInfoes)
				{
					seasonInfo.PosterUrl = this.GetPosterUrl(seasonInfo.PosterPath, posterWidth);
				}
			}
		}

		private static Tuple<int, string> GetSizeInfo(string info)
		{
			if (info.Equals("original", StringComparison.OrdinalIgnoreCase))
			{
				return Tuple.Create(int.MaxValue, info);
			}

			return Tuple.Create(int.Parse(info.TrimStart('w')), info);
		}
	}
}