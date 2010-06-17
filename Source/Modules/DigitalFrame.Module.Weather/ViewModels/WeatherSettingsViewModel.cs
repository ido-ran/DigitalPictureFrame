using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Module.Weather.ViewModels
{
    public class WeatherSettingsViewModel : SettingsViewModel<WeatherSettings>, IWeatherSettingsViewModel
    {
        private string _zipCode;

        public WeatherSettingsViewModel(IRepository<WeatherSettings> repository, IEventAggregator eventAggregator)
            : base(repository, eventAggregator)
        {
        }

        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (_zipCode != value)
                {
                    _zipCode = value;
                    NotifyPropertyChanged(() => ZipCode);
                }
            }
        }

        protected override void OnSaveSettings(object obj)
        {
            var weatherSettings = new WeatherSettings {ZipCode = ZipCode};

            Repository.Save(weatherSettings);

            PublishSettingsSavedEvent(weatherSettings);
        }

        protected override void OnLoadSettings(object obj)
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            WeatherSettings settings = Repository.Load();

            ZipCode = settings.ZipCode;
        }
    }
}