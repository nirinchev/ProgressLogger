using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ProgressLogger.Models
{
	public class SeasonInfo
	{
		public int Id { get; set; }

		public int EpisodeCount { get; set; }

		public DateTime? AirDate { get; set; }

		public string PosterUrl { get; set; }

		public int Number { get; set; }

		public IEnumerable<EpisodeInfo> Episodes { get; set; }

		public WatchStatus Status
		{
			get
			{
				if (this.Episodes == null || this.Episodes.Any(i => i.Status == WatchStatus.Unknown))
				{
					return WatchStatus.Unknown;
				}
				else if (this.Episodes.Any(i => i.Status == WatchStatus.Incomplete))
				{
					return WatchStatus.Incomplete;
				}

				return WatchStatus.Complete;
			}
		}

		static SeasonInfo()
		{
			Mapper.CreateMap<SeasonInfo, SeasonInfo>();
		}
	}
}