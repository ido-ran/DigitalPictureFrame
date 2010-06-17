using System;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class WeatherRange : Weather
    {
        #region WeatherProperties

        private DateTime _endTime;
        private double _highTemperature;
        private double _lowTemperature;
        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    NotifyPropertyChanged(() => StartTime);
                }
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    NotifyPropertyChanged(() => EndTime);
                }
            }
        }

        public double HighTemperature
        {
            get { return Math.Floor(_highTemperature); }
            set
            {
                if (_highTemperature != value)
                {
                    _highTemperature = value;
                    NotifyPropertyChanged(() => HighTemperature);
                    NotifyPropertyChanged(() => HighTemperatureString);
                }
            }
        }

        public double LowTemperature
        {
            get { return Math.Floor(_lowTemperature); }
            set
            {
                if (_lowTemperature != value)
                {
                    _lowTemperature = value;
                    NotifyPropertyChanged(() => LowTemperature);
                    NotifyPropertyChanged(() => LowTemperatureString);
                }
            }
        }

        #endregion

        #region UIProperties

        // These properties are provided for convenient databinding in UI

        public string HighTemperatureString
        {
            get { return HighTemperature + "°"; }
        }

        public string LowTemperatureString
        {
            get { return LowTemperature + "°"; }
        }

        #endregion
    }
}