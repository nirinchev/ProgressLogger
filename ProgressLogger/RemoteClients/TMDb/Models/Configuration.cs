using Newtonsoft.Json;

namespace ProgressLogger.RemoteClients.TMDb.Models
{
	public class Configuration
	{
		[JsonProperty("images")]
		public ImagesConfiguration ImagesConfiguration { get; set; }
	}
}