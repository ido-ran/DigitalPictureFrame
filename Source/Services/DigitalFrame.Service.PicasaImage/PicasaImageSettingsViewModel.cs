using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Service.PicasaImage {
  public class PicasaImageSettingsViewModel : SettingsViewModel<PicasaImageSettings>, IPicasaImageSettingsViewModel {

    private string username;
    private string password;

    public PicasaImageSettingsViewModel(IRepository<PicasaImageSettings> repository, IEventAggregator eventAggregator) 
      : base(repository, eventAggregator) {
    }

    public string Username {
      get { return username; }
      set {
        username = value;
        NotifyPropertyChanged(() => Username);
      }
    }

    public string Password {
      get { return password; }
      set {
        password = value;
        NotifyPropertyChanged(() => Password);
      }
    }

    protected override void OnSaveSettings(object obj) {
      var settings = new PicasaImageSettings()
      {
        Username = username,
        Password = Encrypt(password)
      };

      Repository.Save(settings);

      PublishSettingsSavedEvent(settings);
    }

    protected override void OnLoadSettings(object obj) {
      var settings = Repository.Load();

      Username = settings.Username;
      Password = Decrypt(settings.Password);
    }

    private string Encrypt(string clearText) {
      return clearText;
    }

    private string Decrypt(string chiperText) {
      return chiperText;
    }
  }
}
