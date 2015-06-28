using Newtonsoft.Json;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using ProgressLogger.RemoteClients.TMDb.Models;

namespace ProgressLogger.Models
{
	public class TMDbSeriesInfo
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("original_name")]
		public string Name { get; set; }

		[JsonProperty("overview")]
		public string Description { get; set; }

		[JsonProperty("poster_path")]
		public string PosterPath { get; set; }

		[JsonProperty("backdrop_path")]
		public string BackdropPath { get; set; }

		[JsonProperty("genres")]
		public IEnumerable<TMDbGenreInfo> GenreInfoes { get; set; }

		[JsonProperty("seasons")]
		public IEnumerable<TMDbSeasonInfo> SeasonInfoes { get; set; }

		[JsonIgnore]
		public string PosterUrl { get; set; }

		[JsonIgnore]
		public string BackdropUrl { get; set; }

		static TMDbSeriesInfo()
		{
			Mapper.CreateMap<TMDbSeriesInfo, TMDbSeriesInfo>()
				  .ForAllMembers(p => p.Condition(c => !c.IsSourceValueNull));

			Mapper.CreateMap<TMDbSeriesInfo, SeriesInfo>()
				.ForMember(p => p.Genres, opt => opt.MapFrom(i => i.GenreInfoes == null ? null : string.Join(", ", i.GenreInfoes.Select(g => g.Name))));
		}
	}
}