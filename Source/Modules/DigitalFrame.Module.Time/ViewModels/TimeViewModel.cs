using System;
using System.Windows.Threading;
using DigitalFrame.Core;

namespace DigitalFrame.Module.Time.ViewModels
{
    public class TimeViewModel : NotifyPropertyChangedBase, ITimeViewModel
    {
        private DateTime _current;

        public TimeViewModel()
        {
            Current = DateTime.Now;

            var timer = new DispatcherTimer
                            {
                                Interval = new TimeSpan(0, 0, 0, 1)
                            };

            timer.Tick += OnTimerElapsed;

            timer.Start();
        }

        public DateTime Current
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;

                    NotifyPropertyChanged(() => Current);
                    NotifyPropertyChanged(() => CurrentFormattedTime);
                    NotifyPropertyChanged(() => CurrentFormattedDate);
                }
            }
        }

        public string CurrentFormattedDate
        {
            get            
            {
                return Current.ToShortDateString();
            }
        }

        public string CurrentFormattedTime
        {
            get
            {
                return Current.ToLongTimeString();
            }
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            Current = DateTime.Now;
        }
    }
}