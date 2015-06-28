using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace ProgressLogger
{
	public static class ObservableCollectionExtensions
	{
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}

		public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items.ToArray())
			{
				collection.Remove(item);
			}
		}

		public static void Sort<T, TKey>(this ObservableCollection<T> collection, Func<T, TKey> keyComparer)
		{
			var sorted = collection.OrderBy(keyComparer).ToArray();
			for (var i = 0; i < sorted.Length; i++)
			{
				collection.Move(collection.IndexOf(sorted[i]), i);
			}
		}
	}
}