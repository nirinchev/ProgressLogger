using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class TMDbCollection<T>
	{
		[JsonProperty("results")]
		public IEnumerable<T> Items { get; set; }

		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }

		[JsonProperty("total_results")]
		public int TotalResults { get; set; }
	}
}