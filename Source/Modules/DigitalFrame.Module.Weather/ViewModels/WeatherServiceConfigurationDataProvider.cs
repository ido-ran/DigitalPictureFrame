namespace DigitalFrame.Module.Weather.ViewModels
{
    public class WeatherServiceConfigurationDataProvider : IWeatherServiceConfigurationDataProvider
    {
        public string GetZipCode()
        {
            return "90210";
        }
    }
}