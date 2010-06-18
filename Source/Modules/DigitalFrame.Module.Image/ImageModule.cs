using DigitalFrame.Core.Interfaces;
using DigitalFrame.Module.Image.ViewModels;
using DigitalFrame.Module.Image.Views;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.FileSystemImage;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using DigitalFrame.Service.PicasaImage;

namespace DigitalFrame.Module.Image
{
    public class ImageModule : IModule
    {
        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public ImageModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            var imageView = Container.Resolve<ImageView>();

            RegionManager.Regions["ImagesRegion"].Add(imageView);
        }

        private void RegisterViewsAndServices()
        {
            Container.RegisterType<IImageViewModel, ImageViewModel>();
            Container.RegisterType<IImageService, PicasaImageService>();
            Container.RegisterType<IRepository<PicasaImageSettings>, PicasaImageSettingsRepository>();
            //Container.RegisterType<IRepository<FileSystemImageSettings>, FileSystemImageSettingsRepository>(); 
        }
    }
}
