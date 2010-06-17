using DigitalFrame.Core;
using DigitalFrame.Module.Image.ViewModels;
using DigitalFrame.Module.Image.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Module.Image
{
    public class ImageSettingsModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public ImageSettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            var imageSettingsView = Container.Resolve<ImageSettingsView>();

            RegionManager.Regions[Regions.ModuleSettings].Add(imageSettingsView);
        }

        private void RegisterViewsAndServices()
        {
            Container.RegisterType<IImageSettingsViewModel, ImageSettingsViewModel>();
        }
    }
}
