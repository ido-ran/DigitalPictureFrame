using System;
using DigitalFrame.Core;
using DigitalFrame.Service.Core.Weather;

namespace DigitalFrame.Service.Core
{
    public interface IWeatherService
    {
        event EventHandler<EventArgs<WeatherReport>> WeatherRetrieved;

        void GetLatestWeatherReport(string zipCode, UnitsSystems us);
    }
}