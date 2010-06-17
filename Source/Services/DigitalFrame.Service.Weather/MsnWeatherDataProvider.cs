using System;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using DigitalFrame.Core;
using DigitalFrame.Core.Extensions;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.Core.Weather;

namespace DigitalFrame.Service.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class MsnWeatherDataProvider : IWeatherServiceDataProvider
    {
        private readonly object _lock = new object();
        private UnitsSystems _unitsSystem;
        private WeatherReport _weatherReport;
        private string _zipCode;

        #region IWeatherServiceDataProvider Members

        public event EventHandler<EventArgs<WeatherReport>> WeatherRetrieved;

        public void GetLatestWeather(string zipCode, UnitsSystems unitsSystems)
        {
            _zipCode = zipCode;
            _unitsSystem = unitsSystems;

            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += GetLatestWeather;
            backgroundWorker.RunWorkerCompleted += GetLatestWeatherCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        #endregion

        private void GetLatestWeatherCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_lock)
            {
                WeatherRetrieved.Raise(this, new EventArgs<WeatherReport> {Payload = _weatherReport});
            }
        }

        private void GetLatestWeather(object sender, DoWorkEventArgs e)
        {
            string feedUrl = "http://weather.service.msn.com/data.aspx?src=vista&wealocations=" + _zipCode;

            var reader = new XmlTextReader(feedUrl);

            XDocument weather;
            try
            {
                weather = XDocument.Load(reader);
            }
            catch (Exception ex)
            {
                // TODO:  Initialize or throw an exception...
                return;
            }

            var weatherReport = new WeatherReport
                                    {
                                        Location = (from w in weather.Descendants("weather")
                                                    select new Location
                                                               {
                                                                   FullName =
                                                                       GetString(w.Attribute("weatherlocationname"))
                                                               }).FirstOrDefault(),
                                        Forecast = (from f in weather.Descendants("forecast")
                                                    select new WeatherRange
                                                               {
                                                                   HighTemperature = GetDouble(f.Attribute("high")),
                                                                   LowTemperature = GetDouble(f.Attribute("low")),
                                                                   Precipitation = GetDouble(f.Attribute("precip")),
                                                                   SkyCondition =
                                                                       {
                                                                           SkyCondition =
                                                                               GetString(f.Attribute("skytextday")),
                                                                           SkyImage = GetUri(f.Attribute("skycodeday"))
                                                                       },
                                                                   StartTime =
                                                                       GetDateTime(
                                                                       GetDateTime(f.Attribute("date")).ToString("o")),
                                                                   EndTime =
                                                                       GetDateTime(
                                                                       GetDateTime(f.Attribute("date")).ToString("o"))
                                                               }).Skip(1).Take(3).ToList(),
                                        LatestWeather = (from c in weather.Descendants("current")
                                                         select new WeatherPoint
                                                                    {
                                                                        Temperature =
                                                                            GetDouble(c.Attribute("temperature")),
                                                                        FeelsLike = GetDouble(c.Attribute("feelslike")),
                                                                        Humidity = GetDouble(c.Attribute("humidity")),
                                                                        Precipitation = GetDouble(c.Attribute("precip")),
                                                                        SkyCondition =
                                                                            {
                                                                                SkyCondition =
                                                                                    GetString(c.Attribute("skytext")),
                                                                                SkyImage =
                                                                                    GetUri(c.Attribute("skycode"))
                                                                            },
                                                                        WindVector =
                                                                            ParseSpeed(c.Attribute("winddisplay")),
                                                                        Time =
                                                                            ParseTime(c.Attribute("observationtime"),
                                                                                      c.Attribute("date"))
                                                                    }).FirstOrDefault()
                                    };


            var skyCode = (from c in weather.Descendants("current")
                           select new
                                      {
                                          SkyCode = GetInt(c.Attribute("skycode"))
                                      }).FirstOrDefault();

            weatherReport.SkyCode = skyCode.SkyCode;

            if (_unitsSystem == UnitsSystems.Metric)
            {
                weatherReport = ConvertToMetric(_weatherReport);
            }

            lock (_lock)
            {
                _weatherReport = weatherReport;
            }
        }

        private static DateTime GetDateTime(XAttribute attribute)
        {
            if (attribute == null)
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(attribute.Value);
        }

        private static DateTime GetDateTime(string dateTime)
        {
            return DateTime.Parse(dateTime);
        }

        private static string GetString(XAttribute attribute)
        {
            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }

        private static Uri GetUri(XAttribute attribute)
        {
            const string SKY_IMAGE_RELATIVE_URL = "Images/";

            if (attribute == null)
            {
                return null;
            }

            return new Uri(SKY_IMAGE_RELATIVE_URL + attribute.Value + ".png", UriKind.RelativeOrAbsolute);
        }

        private static DateTime ParseTime(XAttribute timeAttribute, XAttribute dateAttribute)
        {
            DateTime time = timeAttribute == null ? DateTime.MinValue : DateTime.Parse(timeAttribute.Value);
            DateTime date = dateAttribute == null ? DateTime.MinValue : DateTime.Parse(dateAttribute.Value);

            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        private static Wind ParseSpeed(XAttribute value)
        {
            var wind = new Wind();

            if (value == null)
            {
                return wind;
            }

            string[] windInfo = value.Value.Split(' ');

            double speed;

            double.TryParse(windInfo[0], out speed);

            wind.Speed = speed;

            if (windInfo.Length == 3)
            {
                wind.Direction = (WindDirections) Enum.Parse(typeof (WindDirections), windInfo[2]);
            }

            return wind;
        }

        private static WeatherReport ConvertToMetric(WeatherReport result)
        {
            result.LatestWeather.Temperature = 5.0/9.0*(result.LatestWeather.Temperature - 32.0);
            foreach (WeatherRange wr in result.Forecast)
            {
                wr.HighTemperature = 5.0/9.0*(wr.HighTemperature - 32.0);
                wr.LowTemperature = 5.0/9.0*(wr.LowTemperature - 32.0);
            }
            return result;
        }

        private static double GetDouble(XAttribute value)
        {
            double result = 0d;

            if (value == null)
            {
                return result;
            }

            double.TryParse(value.Value, out result);

            return result;
        }

        private static int GetInt(XAttribute value)
        {
            int result = 0;

            if (value == null)
            {
                return result;
            }

            int.TryParse(value.Value, out result);

            return result;
        }
    }
}