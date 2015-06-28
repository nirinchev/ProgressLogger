using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace ProgressLogger
{
	public class Grouping<K, T> : ObservableCollection<T>
	{
		private readonly K key;

		public K Key
		{ 
			get
			{
				return this.Items.Any() ? this.key : default(K);
			}
		}

		public Grouping(K key) : this(key, Enumerable.Empty<T>())
		{
		}

		public Grouping(K key, IEnumerable<T> items) 
		{ 
			this.key = key; 

			foreach (var item in items)
			{
				this.Items.Add(item);
			}
		}
	}
}