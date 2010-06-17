using DigitalFrame.Core;
using DigitalFrame.Service.FileSystemImage.ViewModels;
using DigitalFrame.Service.FileSystemImage.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace DigitalFrame.Service.FileSystemImage
{
    public class FileSystemImageSettingsModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public FileSystemImageSettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterServiceAndViews();

            var fileSystemImageSettingsView = Container.Resolve<FileSystemImageSettingsView>();
            RegionManager.Regions[Regions.ImageSettings].Add(fileSystemImageSettingsView);
        }

        private void RegisterServiceAndViews()
        {
            Container.RegisterType<IFileSystemImageSettingsViewModel, FileSystemImageSettingsViewModel>();
        }
    }
}
