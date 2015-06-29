using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace ProgressLogger.Helpers
{
	public class Grouping<K, T> : ObservableCollection<T>
	{
		public K Key { get; }

		public Grouping(K key) : this(key, Enumerable.Empty<T>())
		{
		}

		public Grouping(K key, IEnumerable<T> items) 
		{ 
			this.Key = key; 

			foreach (var item in items)
			{
				this.Items.Add(item);
			}
		}
	}
}