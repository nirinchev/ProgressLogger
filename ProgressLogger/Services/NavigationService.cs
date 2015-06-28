using Xamarin.Forms;
using System.ComponentModel.Composition;
using DryIoc;
using System;
using System.Threading.Tasks;
using DryIoc.MefAttributedModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProgressLogger
{
	[Export(typeof(INavigationService)), SingletonReuseAttribute]
	public class NavigationService : INavigationService
	{
		private static readonly IDictionary<string, Type> keys;

		private readonly IContainer container;
		private NavigationPage navPage;

		static NavigationService()
		{
			keys = App.GetAssemblies().SelectMany(a => a.ExportedTypes.Where(t => typeof(Page).IsAssignableFrom(t) &&
			                                                                      t.GetCustomAttributes<ExportAttribute>().Any()))
									  .ToDictionary(t => t.Name, t => t);
		}

		[ImportingConstructor]
		public NavigationService (IContainer container)
		{
			this.container = container;
		}

		public void Initialize(NavigationPage navPage)
		{
			this.navPage = navPage;
		}

		public Task NavigateTo(string key, bool modal)
		{
			Type type;
			if (!keys.TryGetValue(key, out type))
			{
				throw new ArgumentException("Uh oh, we've somehow lost a page type :/");
			}

			var page = this.container.Resolve(type) as Page;
			if (page == null)
			{
				throw new ArgumentException("Key doesn't represent valid page.");
			}

			return modal ? this.navPage.PushAsync(page) : this.navPage.PushAsync(page, true);
		}
	}
}