﻿using System;
using System.Threading.Tasks;

namespace ProgressLogger.Services
{
	public interface ICacheService
	{
		Task AddOrUpdate<T> (string key, T item, DateTimeOffset? expiration = null);

		Task<T> GetValueOrDefault<T> (string key, T defaultValue = default(T));

		Task Remove<T> (string regex);
	}
}