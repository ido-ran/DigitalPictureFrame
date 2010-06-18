using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using DigitalFrame.Core;

namespace DigitalFrame.Service.PicasaImage {
  public class PicasaImageSettingsModule : IModule {

    private IUnityContainer Container { get; set; }
    private IRegionManager RegionManager { get; set; }

    public PicasaImageSettingsModule(IUnityContainer container, IRegionManager regionManager) {
      Container = container;
      RegionManager = regionManager;
    }

    public void Initialize() {
      RegisterServiceAndViews();

      var picasaSettingsView = Container.Resolve<PicasaImageSettingsView>();
      RegionManager.Regions[Regions.ImageSettings].Add(picasaSettingsView);
    }

    private void RegisterServiceAndViews() {
      Container.RegisterType<IPicasaImageSettingsViewModel, PicasaImageSettingsViewModel>();
    }

  }
}
