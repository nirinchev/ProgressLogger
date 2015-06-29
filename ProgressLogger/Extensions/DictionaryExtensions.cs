using System.Collections.Generic;

namespace ProgressLogger
{
	public static class DictionaryExtensions
	{
		public static T GetValueOrDefault<K,T>(this IDictionary<K, T> dict, K key, T defaultValue = default(T))
		{
			T result;
			if (dict.TryGetValue(key, out result))
			{
				return result;
			}

			return defaultValue;
		}
	}
}