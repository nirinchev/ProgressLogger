using Xamarin.Forms;
using DryIoc;
using DryIoc.MefAttributedModel;
using System.Collections.Generic;
using System.Reflection;
using ProgressLogger.Services;
using ProgressLogger.Views;
using System;

namespace ProgressLogger.Core
{
	public class App : Application
	{
		private readonly IContainer container;

		public App(Action<IContainer> setupContainer)
		{
			this.container = new Container().WithMefAttributedModel();
			this.container.RegisterExports(GetAssemblies());
			container.RegisterDelegate<IContainer>(r => container, Reuse.Singleton);
			if (setupContainer != null)
			{
				setupContainer(this.container);
			}

			var mainPage = container.Resolve<MainPage>();

			var navPage = new NavigationPage(mainPage);
			var navService = container.Resolve<INavigationService>();
			navService.Initialize(navPage);
			this.MainPage = navPage;
		}

		public static IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(App).Assembly;
		}
	}
}