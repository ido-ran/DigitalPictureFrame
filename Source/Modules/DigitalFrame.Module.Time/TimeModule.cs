using DigitalFrame.Core.Interfaces;
using DigitalFrame.Module.Time.ViewModels;
using DigitalFrame.Module.Time.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Module.Time
{
    public class TimeModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public TimeModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            var timeView = Container.Resolve<TimeView>();

            RegionManager.Regions["TimeRegion"].Add(timeView);
        }

        private void RegisterViewsAndServices()
        {
            Container.RegisterType<ITimeViewModel, TimeViewModel>();
        }
    }
}
