using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public abstract class Weather : NotifyPropertyChangedBase
    {
        private double? _humidity;
        private double? _precipitation;

        private Sky _skyCondition;
        private UnitsSystems _unitsSystem;

        private Wind _windVector;

        protected Weather()
        {
            UnitsSystem = UnitsSystems.Imperial;

            _skyCondition = new Sky();
            _windVector = new Wind();
        }

        public double? Humidity
        {
            get { return _humidity; }
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    NotifyPropertyChanged(() => Humidity);
                }
            }
        }

        public double? Precipitation
        {
            get { return _precipitation; }
            set
            {
                if (_precipitation != value)
                {
                    _precipitation = value;
                    NotifyPropertyChanged(() => Precipitation);
                 
                }
            }
        }

        public string PrecipitationString
        {
            get
            {
                return Precipitation + "%";
            }
        }

        public Sky SkyCondition
        {
            get { return _skyCondition; }
            set
            {
                if (_skyCondition != value)
                {
                    _skyCondition = value;
                    NotifyPropertyChanged(() => SkyCondition);
                }
            }
        }

        public Wind WindVector
        {
            get { return _windVector; }
            set
            {
                if (_windVector != value)
                {
                    _windVector = value;
                    NotifyPropertyChanged(() => WindVector);
                }
            }
        }

        public UnitsSystems UnitsSystem
        {
            get { return _unitsSystem; }
            set
            {
                if (_unitsSystem != value)
                {
                    _unitsSystem = value;
                    NotifyPropertyChanged(() => UnitsSystem);
                }
            }
        }
    }
}