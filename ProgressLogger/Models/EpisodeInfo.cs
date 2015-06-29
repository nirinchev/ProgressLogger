using System;
using AutoMapper;

namespace ProgressLogger.Models
{
	public class EpisodeInfo
	{
		public int Id { get; set; }
		
		public string Name { get; set; }

		public string Synopsys { get; set; }

		public DateTime? AirDate { get; set; }

		public int Number { get; set; }

		public string StillUrl { get; set; }

		public WatchStatus Status { get; set; }

		static EpisodeInfo()
		{
			Mapper.CreateMap<EpisodeInfo, EpisodeInfo>();
		}
	}
}