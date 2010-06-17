using System;
using DigitalFrame.Core;
using DigitalFrame.Core.Extensions;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.Core.Weather;

namespace DigitalFrame.Service.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherServiceDataProvider _weatherServiceDataProvider;

        public WeatherService(IWeatherServiceDataProvider weatherServiceDataProvider)
        {
            _weatherServiceDataProvider = weatherServiceDataProvider;
            _weatherServiceDataProvider.WeatherRetrieved += (s, e) => WeatherRetrieved.Raise(s, e);
        }

        public event EventHandler<EventArgs<WeatherReport>> WeatherRetrieved;

        public void GetLatestWeatherReport(string zipCode, UnitsSystems us)
        {
            _weatherServiceDataProvider.GetLatestWeather(zipCode, us);
        }
    }
}
