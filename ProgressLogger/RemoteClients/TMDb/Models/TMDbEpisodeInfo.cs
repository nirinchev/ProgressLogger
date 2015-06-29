using System;
using Newtonsoft.Json;
using AutoMapper;
using ProgressLogger.Models;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class TMDbEpisodeInfo
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("overview")]
		public string Synopsys { get; set; }

		[JsonProperty("air_date")]
		public DateTime? AirDate { get; set; }

		[JsonProperty("episode_number")]
		public int Number { get; set; }

		[JsonProperty("still_path")]
		public string StillPath { get; set; }

		[JsonIgnore]
		public string StillUrl { get; set; }

		static TMDbEpisodeInfo()
		{
			Mapper.CreateMap<TMDbEpisodeInfo, EpisodeInfo>();
		}
	}
}