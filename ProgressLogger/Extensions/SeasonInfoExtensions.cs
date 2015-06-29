namespace ProgressLogger.Models
{
	public static class SeasonInfoExtensions
	{
		public static bool IsComplete(this SeasonInfo info)
		{
			return info.Episodes != null;
		}
	}
}