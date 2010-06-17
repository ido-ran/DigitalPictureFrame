using DigitalFrame.Core.Interfaces;
using DigitalFrame.Module.Weather.ViewModels;
using DigitalFrame.Module.Weather.Views;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.Weather;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Module.Weather
{
    public class WeatherModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public WeatherModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            var weatherView = Container.Resolve<WeatherView>();

            RegionManager.Regions["WeatherRegion"].Add(weatherView);
        }

        private void RegisterViewsAndServices()
        {
            Container.RegisterType<IWeatherViewModel, WeatherViewModel>();
            Container.RegisterType<IRepository<WeatherSettings>, WeatherSettingsRepository>();
            Container.RegisterType<IWeatherService, WeatherService>();
            Container.RegisterType<IWeatherServiceDataProvider, MsnWeatherDataProvider>();
        }
    }
}
