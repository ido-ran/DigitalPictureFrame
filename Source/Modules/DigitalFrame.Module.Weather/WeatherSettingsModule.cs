using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using DigitalFrame.Module.Weather.ViewModels;
using DigitalFrame.Module.Weather.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Module.Weather
{
    public class WeatherSettingsModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public WeatherSettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            var weatherSettingsView = Container.Resolve<WeatherSettingsView>();

            RegionManager.Regions[Regions.ModuleSettings].Add(weatherSettingsView);    
        }

        private void RegisterViewsAndServices()
        {
            Container.RegisterType<IWeatherSettingsViewModel, WeatherSettingsViewModel>();
            Container.RegisterType<IRepository<WeatherSettings>, WeatherSettingsRepository>();
        }
    }
}
