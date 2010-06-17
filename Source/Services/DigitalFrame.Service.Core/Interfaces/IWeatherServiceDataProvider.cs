using System;
using DigitalFrame.Core;
using DigitalFrame.Service.Core.Weather;

namespace DigitalFrame.Service.Core
{
    public interface IWeatherServiceDataProvider
    {
        event EventHandler<EventArgs<WeatherReport>> WeatherRetrieved;

        void GetLatestWeather(string zipCode, UnitsSystems unitsSystems);
    }
}