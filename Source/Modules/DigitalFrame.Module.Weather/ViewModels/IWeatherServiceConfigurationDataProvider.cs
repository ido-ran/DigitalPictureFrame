using System;

namespace DigitalFrame.Module.Weather.ViewModels
{
    public interface IWeatherServiceConfigurationDataProvider
    {
        string GetZipCode();
    }
}