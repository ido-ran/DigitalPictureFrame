using System;
using System.Windows.Threading;
using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.Core.Weather;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Module.Weather.ViewModels
{
    public class WeatherViewModel : NotifyPropertyChangedBase, IWeatherViewModel
    {
        private readonly IWeatherService _weatherService;
        private WeatherReport _weatherReport;
        private WeatherSettings _weatherSettings;

        public WeatherViewModel(IRepository<WeatherSettings> repository, IWeatherService weatherService,
                                IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            _weatherService = weatherService;
            _weatherService.WeatherRetrieved += (s, e) => WeatherReport = e.Payload;

            _weatherSettings = repository.Load();

            GetLatestWeather();

            var timer = new DispatcherTimer
                            {
                                Interval = new TimeSpan(0, 1, 0, 0)
                            };

            timer.Tick += OnTimerElapsed;

            timer.Start();

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            var settingsChangedEvent = EventAggregator.GetEvent<SettingsChangedEvent<WeatherSettings>>();

            if (settingsChangedEvent != null)
            {
                settingsChangedEvent.Subscribe(OnSettingsChangedEvent);
            }
        }

        private IEventAggregator EventAggregator { get; set; }

        public WeatherReport WeatherReport
        {
            get { return _weatherReport; }
            set
            {
                if (_weatherReport != value)
                {
                    _weatherReport = value;
                    NotifyPropertyChanged(() => WeatherReport);
                }
            }
        }

        private void OnSettingsChangedEvent(WeatherSettings weatherSettings)
        {
            _weatherSettings = weatherSettings;

            GetLatestWeather();
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            GetLatestWeather();
        }

        private void GetLatestWeather()
        {
            _weatherService.GetLatestWeatherReport(_weatherSettings.ZipCode, UnitsSystems.Imperial);
        }

    }
}