using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.Views;
using MyVote.UI.Helpers;
using MyVote.UI.Ios;
using UIKit;
using MyVote.UI.Services;

namespace MyVote.UI
{
    public class MvxFormsSetup : MvxSetup
	{
        private readonly AppDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxTouchViewPresenter _presenter;

        public MvxFormsSetup(AppDelegate applicationDelegate, UIWindow window)
		{
            _window = window;
            _applicationDelegate = applicationDelegate;
		}

        protected MvxFormsSetup(AppDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
		{
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
		}

        protected UIWindow Window
        {
            get { return _window; }
        }

        protected AppDelegate ApplicationDelegate
        {
            get { return _applicationDelegate; }
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Touch", toReturn.Finders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override sealed IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateTouchViewsContainer();
            RegisterTouchViewCreator(container);
            return container;
        }

        protected virtual IMvxTouchViewsContainer CreateTouchViewsContainer()
        {
            return new MvxTouchViewsContainer();
        }

        protected virtual void RegisterTouchViewCreator(IMvxTouchViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxTouchViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTouchViewDispatcher(Presenter);
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            RegisterPresenter();
            RegisterLifetime();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxTouchSystem>(CreateTouchSystemProperties());
        }

        protected virtual MvxTouchSystem CreateTouchSystemProperties()
        {
            return new MvxTouchSystem();
        }

        [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
        protected virtual void RegisterOldStylePlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxTouchPlatformProperties>(new MvxTouchPlatformProperties());
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxTouchViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreatePresenter();
                return _presenter;
            }
        }

        protected virtual IMvxTouchViewPresenter CreatePresenter()
        {
            return new MvxPagePresenter(Window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxTouchModalHost>(presenter);
        }

        protected override IMvxApplication CreateApp()
        {
            return new MyVoteApp();
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxTouchBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders
        {
            get { return new List<Type>(); }
        }

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }

        // Use Autofac for IoC
		protected override Cirrious.CrossCore.IoC.IMvxIoCProvider CreateIocProvider()
		{
			return new AutofacMvxProvider();
		}
	}
}