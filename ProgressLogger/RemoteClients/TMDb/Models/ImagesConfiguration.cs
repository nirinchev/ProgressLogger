using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class ImagesConfiguration
	{
		private IEnumerable<string> posterSizes;
		private Tuple<int, string>[] sizeInfos;

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
				this.sizeInfos = this.posterSizes.Select(GetSizeInfo)
												 .OrderBy(s => s.Item1)
												 .ToArray();
			}
		}

		public string GetPosterPath(string relative, int minWidth)
		{
			var size = this.sizeInfos.First(s => s.Item1 >= minWidth).Item2;
			return $"{BaseUrl}{size}{relative}";
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