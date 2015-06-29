using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using ProgressLogger.Models;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class ImagesConfiguration
	{
		private IDictionary<ImageType, Tuple<int, string>[]> sizes = new Dictionary<ImageType, Tuple<int, string>[]>();

		[JsonProperty("secure_base_url")]
		public string BaseUrl { get; set; }

		[JsonProperty("poster_sizes")]
		public IEnumerable<string> PosterSizes
		{
			get
			{
				return this.GetSizes(ImageType.Poster);
			}
			set
			{
				this.SetSizes(ImageType.Poster, value);
			}
		}

		[JsonProperty("backdrop_sizes")]
		public IEnumerable<string> BackdropSizes
		{
			get
			{
				return this.GetSizes(ImageType.Backdrop);
			}
			set
			{
				this.SetSizes(ImageType.Backdrop, value);
			}
		}

		[JsonProperty("still_sizes")]
		public IEnumerable<string> StillSizes
		{
			get
			{
				return this.GetSizes(ImageType.Still);
			}
			set
			{
				this.SetSizes(ImageType.Still, value);
			}
		}

		public string GetUrl(string relative, ImageType type, int minWidth)
		{
			var size = this.sizes.GetValueOrDefault(type)?.FirstOrDefault(s => s.Item1 >= minWidth)?.Item2;
			return size == null ? null : $"{BaseUrl}{size}{relative}";
		}

		public void UpdateUrls(TMDbSeriesInfo info, int posterWidth, int backdropWidth, int stillWidth)
		{
			info.PosterUrl = this.GetUrl(info.PosterPath, ImageType.Poster, posterWidth);
			info.BackdropUrl = this.GetUrl(info.BackdropPath, ImageType.Backdrop, backdropWidth);

			if (info.SeasonInfoes != null)
			{
				foreach (var seasonInfo in info.SeasonInfoes)
				{
					this.UpdateUrls(seasonInfo, posterWidth, stillWidth);
				}
			}
		}

		public void UpdateUrls(TMDbSeasonInfo info, int posterWidth, int stillWidth)
		{
			info.PosterUrl = this.GetUrl(info.PosterPath, ImageType.Poster, posterWidth);

			if (info.Episodes != null)
			{
				foreach (var ep in info.Episodes)
				{
					ep.StillUrl = this.GetUrl(ep.StillPath, ImageType.Still, stillWidth);
				}
			}
		}

		private IEnumerable<string> GetSizes(ImageType type)
		{
			return this.sizes.GetValueOrDefault(type)?.Select(t => t.Item2) ?? Enumerable.Empty<string>();
		}

		private void SetSizes(ImageType type, IEnumerable<string> values)
		{
			this.sizes[type] = values.Select(GetSizeInfo)
									 .OrderBy(s => s.Item1)
									 .ToArray();
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

	public enum ImageType
	{
		Poster,
		Backdrop,
		Still
	}
}