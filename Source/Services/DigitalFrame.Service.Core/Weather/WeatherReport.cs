using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class WeatherReport : NotifyPropertyChangedBase
    {
        #region WeatherProperties

        public WeatherReport()
        {
            _forecast = new List<WeatherRange>();
        }

        private WeatherPoint _latestWeather;
        public WeatherPoint LatestWeather
        {
            get { return _latestWeather; }
            set {
                if (_latestWeather != value)
                {
                    _latestWeather = value;
                    NotifyPropertyChanged(() => LatestWeather);
                    NotifyPropertyChanged(() => SkyCode);
                    NotifyPropertyChanged(() => BackgroundImage);
                    NotifyPropertyChanged(() => BackgroundImage2);
                    NotifyPropertyChanged(() => BackgroundImage3);
                }
            }
        }

        private List<WeatherRange> _forecast;
        public List<WeatherRange> Forecast
        {
            get { return _forecast; }
            set {
                if (_forecast != value)
                {
                    _forecast = value;
                    NotifyPropertyChanged(() => Forecast);
                }
            }
        }

        private Location _location;
        public Location Location
        {
            get { return _location; }
            set {
                if (_location != value)
                {
                    _location = value;
                    NotifyPropertyChanged(() => Location);
                }
            }
        }

        private UnitsSystems _unitsSystem;
        public UnitsSystems UnitsSystem
        {
            get { return _unitsSystem; }
            set {
                if (_unitsSystem != value)
                {
                    _unitsSystem = value;
                    NotifyPropertyChanged(() => UnitsSystem);
                }
            }
        }

        #endregion

        #region UIProperties

        private int _skyCode;
        public int SkyCode
        {
            get { return _skyCode; }
            set {
                if (_skyCode != value)
                {
                    _skyCode = value;
                    NotifyPropertyChanged(() => SkyCode);
                    NotifyPropertyChanged(() => BackgroundImage);
                    NotifyPropertyChanged(() => BackgroundImage2);
                    NotifyPropertyChanged(() => BackgroundImage3);
                }
            }
        }

        #region UndockedBackgroundProperties
        public Uri BackgroundImage
        {
            get
            {
                Uri uri = null;

                if (!IsNight())
                {
                    if (44 == _skyCode || 36 == _skyCode || 34 == _skyCode || 32 == _skyCode || 31 == _skyCode)
                    {
                        uri = new Uri("Images/undocked_sun.png", UriKind.Relative);
                    }
                }
                else
                {
                    string moonShape = ComputeMoonShape();
                    if (44 == _skyCode || 36 == _skyCode || 34 == _skyCode || 32 == _skyCode || 31 == _skyCode)
                    {
                        uri = new Uri("Images/undocked_moon-" + moonShape + ".png", UriKind.Relative);
                    }
                }

                return uri;
            }
        }

        public Uri BackgroundImage2
        {
            get
            {
                string theWeatherState = WeatherState(_skyCode);
                return new Uri("Images/undocked_" + theWeatherState + ".png", UriKind.Relative);
            }
        }

        public Uri BackgroundImage3
        {
            get
            {
                Uri uri = null;

                if (IsNight())
                {
                    uri = new Uri("Images/BLACK-base.png", UriKind.Relative);
                }
                else
                {
                    if (44 == _skyCode || 36 == _skyCode || 34 == _skyCode || 32 == _skyCode || 31 == _skyCode)
                    {
                        uri = new Uri("Images/BLUE-base.png", UriKind.Relative);
                    }
                    else
                    {
                        uri = new Uri("Images/GRAY-base.png", UriKind.Relative);
                    }
                }

                return uri;
            }
        }
        #endregion

        public Brush TextColor
        {
            get
            {
                return (IsNight()) ? Brushes.White : Brushes.Black;
            }
        }

        #endregion

        #region PrivateMethods

        private static string WeatherState(int code)
        {
            string theWeatherState = "";
            switch (code)
            {
                case (26):
                case (27):
                case (28):
                    theWeatherState = "cloudy";
                    break;
                case (35):
                case (39):
                case (45):
                case (46):
                    theWeatherState = "few-showers";
                    break;
                case (19):
                case (20):
                case (21):
                case (22):
                    theWeatherState = "foggy";
                    break;
                case (29):
                case (30):
                case (33):
                    theWeatherState = "partly-cloudy";
                    break;
                case (5):
                case (13):
                case (14):
                case (15):
                case (16):
                case (18):
                case (25):
                case (41):
                case (42):
                case (43):
                    theWeatherState = "snow";
                    break;
                case (1):
                case (2):
                case (3):
                case (4):
                case (37):
                case (38):
                case (47):
                    theWeatherState = "thunderstorm";
                    break;
                case (31):
                case (32):
                case (34):
                case (36):
                case (44):        // Note 44- "Data Not Available"
                    theWeatherState = "";
                    break;
                case (23):
                case (24):
                    theWeatherState = "windy";
                    break;
                case (9):
                case (10):
                case (11):
                case (12):
                case (40):
                    theWeatherState = "rainy";
                    break;
                case (6):
                case (7):
                case (8):
                case (17):
                    theWeatherState = "hail";
                    break;
                default:
                    theWeatherState = "";
                    break;
            }
            return theWeatherState;
        }

        private bool IsNight()
        {

            // TODO: Make the IsNight algorithm more sophisticated
            if ((_latestWeather.SkyCondition.SkyCondition.Contains("Cloudy")) ||
                (_latestWeather.SkyCondition.SkyCondition.Contains("Fair")) ||
                (_latestWeather.SkyCondition.SkyCondition.Contains("Sprinkles")))
            {
                return true;
            }
            return (LatestWeather.Time.Hour >= 6 && LatestWeather.Time.Hour <= 18) ? false : true;

        }

        private string ComputeMoonShape()
        {
            int Year = LatestWeather.Time.Year;
            int Month = LatestWeather.Time.Month;
            int Day = LatestWeather.Time.Day;
            // Variable names used: J, K1, K2, K3, MM, P2, V, YY
            //double P2 = 3.14159 * 2;    
            int YY = Year - (int)((12 - Month) / 10);
            int MM = Month + 9;
            if (MM >= 12) { MM = MM - 12; }
            int K1 = (int)(365.25 * (YY + 4712));
            int K2 = (int)(30.6 * MM + .5);
            int K3 = (int)((int)((YY / 100) + 49) * .75) - 38;
            // J is the Julian date at 12h UT on day in question
            int J = K1 + K2 + Day + 59;
            // Adjust for Gregorian calendar, if applicable   
            if (J > 2299160) { J = J - K3; }
            // Calculate illumination (synodic) phase
            double V = (J - 2451550.1) / 29.530588853;
            V = V - (int)(V);
            // Normalize values to range from 0 to 1
            if (V < 0) { V = V + 1; }
            // Moon's age in days from New Moon
            double AG = V * 29.53;
            string retVal;
            if ((AG > 27.6849270496875) || (AG <= 1.8456618033125))
            {
                retVal = "New";
            }
            else if ((AG > 1.8456618033125) && (AG <= 5.5369854099375))
            {
                retVal = "Waxing-Crescent";
            }
            else if ((AG > 5.5369854099375) && (AG <= 9.2283090165625))
            {
                retVal = "First-Quarter";
            }
            else if ((AG > 9.2283090165625) && (AG <= 12.9196326231875))
            {
                retVal = "Waxing-Gibbous";
            }
            else if ((AG > 12.9196326231875) && (AG <= 16.6109562298125))
            {
                retVal = "Full";
            }
            else if ((AG > 16.6109562298125) && (AG <= 20.3022798364375))
            {
                retVal = "Waning-Gibbous";
            }
            else if ((AG > 20.3022798364375) && (AG <= 23.9936034430625))
            {
                retVal = "Last-Quarter";
            }
            else if ((AG > 23.9936034430625) && (AG <= 27.6849270496875))
            {
                retVal = "Waning-Crescent";
            }
            else
            {
                retVal = "Full";
            }

            return retVal;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, e);
        }
    }
}