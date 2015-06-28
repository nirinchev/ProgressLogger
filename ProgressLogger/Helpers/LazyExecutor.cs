using System;
using System.Threading.Tasks;

namespace ProgressLogger.Helpers
{
	public class LazyExecutor<T>
	{
		private Guid currentGuid;
		private Func<T, Task> action;
		private int delay;

		public LazyExecutor (Func<T, Task> action, int delay = 750)
		{
			this.action = action;
			this.delay = delay;
		}

		public LazyExecutor (Action<T> action, int delay = 750) : this (o =>
			{
				action (o);
				return Task.FromResult ((object)null);
			}, delay)
		{
		}

		public virtual async Task Run (T item)
		{
			var tag = this.currentGuid = Guid.NewGuid ();
			await Task.Delay (delay);
			if (tag == this.currentGuid)
			{
				await this.action (item);
			}
		}

		public void Cancel ()
		{
			this.currentGuid = Guid.Empty;
		}
	}

	public class LazyExecutor : LazyExecutor<object>
	{
		public LazyExecutor (Func<Task> action, int delay = 750) : base (o => action (), delay)
		{
		}

		public LazyExecutor (Action action, int delay = 750) : base (o =>
			{
				action ();
				return Task.FromResult ((object)null);
			}, delay)
		{
		}

		public Task Run ()
		{
			return base.Run (null);
		}
	}
}

