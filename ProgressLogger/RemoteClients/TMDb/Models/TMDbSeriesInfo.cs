using System;
using Newtonsoft.Json;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class TMDbSeriesInfo : ISeriesInfo
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("original_name")]
		public string Name { get; set; }

		[JsonProperty("poster_path")]
		public string PosterPath { get; set; }

		[JsonIgnore]
		public string PosterUrl { get; set; }
	}
}