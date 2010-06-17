using System;
using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Weather
{
    // Code adapted from Weather Reader User Control at http://wruc.codeplex.com
    public class Sky : NotifyPropertyChangedBase
    {
        private string _skyCondition;
        public string SkyCondition
        {
            get { return _skyCondition; }
            set {
                if (_skyCondition != value)
                {
                    _skyCondition = value;
                    NotifyPropertyChanged(() => SkyCondition);
                }
            }
        }

        private Uri _skyImage;
        public Uri SkyImage
        {
            get { return _skyImage; }
            set {
                if (_skyImage != value)
                {
                    _skyImage = value;
                    NotifyPropertyChanged(() => SkyImage);
                }
            }
        }
    }
}