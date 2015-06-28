using System;

namespace ProgressLogger.Models
{
	public class SeasonInfo
	{
		public int Id { get; set; }

		public int EpisodeCount { get; set; }

		public DateTime? AirDate { get; set; }

		public string PosterUrl { get; set; }

		public int Number { get; set; }

		public WatchStatus Status { get; set; }
	}
}