using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class Location : NotifyPropertyChangedBase
    {
        private string _city;
        private string _country;
        private string _fullName;
        private double? _latitude;
        private double? _longitude;
        private string _state;
        private int? _zipCode;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    NotifyPropertyChanged(() => FullName);
                }
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (_country != value)
                {
                    _country = value;
                    NotifyPropertyChanged(() => Country);
                }
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyPropertyChanged(() => State);
                }
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    NotifyPropertyChanged(() => City);
                }
            }
        }

        public int? ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (_zipCode != value)
                {
                    _zipCode = value;
                    NotifyPropertyChanged(() => ZipCode);
                }
            }
        }

        public double? Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    NotifyPropertyChanged(() => Longitude);
                }
            }
        }

        public double? Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    NotifyPropertyChanged(() => Latitude);
                }
            }
        }
    }
}