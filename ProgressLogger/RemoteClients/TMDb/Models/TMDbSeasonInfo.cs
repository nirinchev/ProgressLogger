using System;
using Newtonsoft.Json;
using AutoMapper;
using ProgressLogger.Models;
using System.Collections.Generic;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class TMDbSeasonInfo
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("episode_count")]
		public int EpisodeCount { get; set; }

		[JsonProperty("air_date")]
		public DateTime? AirDate { get; set; }

		[JsonProperty("poster_path")]
		public string PosterPath { get; set; }

		[JsonProperty("season_number")]
		public int Number { get; set; }

		[JsonProperty("episodes")]
		public IEnumerable<TMDbEpisodeInfo> Episodes { get; set; }

		[JsonIgnore]
		public string PosterUrl { get; set; }

		static TMDbSeasonInfo()
		{
			Mapper.CreateMap<TMDbSeasonInfo, SeasonInfo>();
		}
	}
}