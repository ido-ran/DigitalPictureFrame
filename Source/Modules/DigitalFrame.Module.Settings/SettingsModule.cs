using DigitalFrame.Module.Settings.ViewModels;
using DigitalFrame.Module.Settings.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Module.Settings
{
    public class SettingsModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            
            var settingsShellViewViewModel = Container.Resolve<SettingsShellViewModel>();

            var settingsShellView = new SettingsShellView(settingsShellViewViewModel);

            RegionManager.Regions["SettingsRegion"].Add(settingsShellView);
        }
    }
}
