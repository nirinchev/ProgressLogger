using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace ProgressLogger.Models
{
	public class SeriesInfo
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string PosterUrl { get; set; }

		public string BackdropUrl { get; set; }

		public string Genres { get; set; }

		public IEnumerable<SeasonInfo> SeasonInfoes { get; set; }

		public WatchStatus Status
		{
			get
			{
				if (this.SeasonInfoes == null || this.SeasonInfoes.Any(i => i.Status == WatchStatus.Unknown))
				{
					return WatchStatus.Unknown;
				}
				else if (this.SeasonInfoes.Any(i => i.Status == WatchStatus.Incomplete))
				{
					return WatchStatus.Incomplete;
				}

				return WatchStatus.Complete;
			}
		}

		static SeriesInfo()
		{
			Mapper.CreateMap<SeriesInfo, SeriesInfo>();
		}
	}
}