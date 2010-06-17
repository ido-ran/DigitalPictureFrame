using System;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class WeatherPoint : Weather
    {
        private double _feelsLike;
        private double _temperature;
        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    NotifyPropertyChanged(() => Humidity);
                }
            }
        }

        public double Temperature
        {
            get { return Math.Floor(_temperature); }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    NotifyPropertyChanged(() => Temperature);
                    NotifyPropertyChanged(() => TemperatureString);
                }
            }
        }

        public string TemperatureString
        {
            get { return Temperature + "°"; }
        }

        public double FeelsLike
        {
            get { return Math.Floor(_feelsLike); }
            set
            {
                if (_feelsLike != value)
                {
                    _feelsLike = value;
                    NotifyPropertyChanged(() => FeelsLike);
                    NotifyPropertyChanged(() => FeelsLikeString);
                }
            }
        }

        public string FeelsLikeString
        {
            get { return FeelsLike + "°"; }
        }
    }
}