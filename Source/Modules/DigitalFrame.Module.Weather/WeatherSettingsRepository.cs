using System.Xml.Linq;
using DigitalFrame.Core;
using DigitalFrame.Module.Weather.ViewModels;

namespace DigitalFrame.Module.Weather
{
    public class WeatherSettingsRepository : Repository<WeatherSettings>
    {
        private const string FILE_NAME = "WeatherSettings.xml";
        private static readonly WeatherSettings DefaultWeatherSettings = new WeatherSettings {ZipCode = "90210"};

        public override WeatherSettings Load()
        {
            XDocument document = LoadDocument(FILE_NAME);

            if (document == null)
            {
                return DefaultWeatherSettings;
            }

            string zipCode = GetSettingValue(document, "ZipCode");

            return new WeatherSettings {ZipCode = zipCode};
        }

        public override void Save(WeatherSettings weatherSettings)
        {
            XDocument xDocument = CreateDocumentWithRoot(ROOT_ELEMENT_NAME);
            XElement zipCodeSetting = CreateSettingElement("ZipCode", weatherSettings.ZipCode);
            xDocument.Element(ROOT_ELEMENT_NAME).Add(zipCodeSetting);

            WriteDocument(FILE_NAME, xDocument);
        }
    }
}