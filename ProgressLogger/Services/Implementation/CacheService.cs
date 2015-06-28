using System;
using System.Linq;
using Akavache;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reactive.Linq;
using System.ComponentModel.Composition;
using DryIoc.MefAttributedModel;

namespace ProgressLogger.Services.Implementation
{
	[Export(typeof(ICacheService)), SingletonReuseAttribute]
	public class CacheService : ICacheService
	{
		public CacheService ()
		{
			BlobCache.ApplicationName = "ProgressLogger";
		}

		public async Task AddOrUpdate<T> (string key, T item, double expirationHours)
		{
			await BlobCache.LocalMachine.InsertObject (key, item, DateTimeOffset.UtcNow.AddHours (expirationHours));
		}

		public async Task<T> GetValueOrDefault<T> (string key, T defaultValue)
		{
			try
			{
				return await BlobCache.LocalMachine.GetObject<T> (key);
			}
			catch
			{
				return defaultValue;
			}
		}

		public async Task Remove<T> (string regex)
		{
			try
			{
				var keys = await BlobCache.LocalMachine.GetAllKeys ();
				var rgx = new Regex (regex);
				var keysToRemove = keys.Where(x => rgx.IsMatch (x));
				if (keysToRemove.Any())
				{
					await BlobCache.LocalMachine.InvalidateObjects<T> (keysToRemove);
				}
			}
			catch
			{
			}
		}
	}
}