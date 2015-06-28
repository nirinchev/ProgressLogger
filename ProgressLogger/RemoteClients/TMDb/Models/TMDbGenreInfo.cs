using Newtonsoft.Json;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class TMDbGenreInfo
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }
	}
}