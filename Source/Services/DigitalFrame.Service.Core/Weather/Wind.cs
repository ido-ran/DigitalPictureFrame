using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class Wind : NotifyPropertyChangedBase
    {
        private WindDirections _direction;
        private double? _speed;

        public double? Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    NotifyPropertyChanged(() => Speed);
                }
            }
        }

        public WindDirections Direction
        {
            get { return _direction; }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    NotifyPropertyChanged(() => Direction);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _speed, _direction);
        }
    }
}