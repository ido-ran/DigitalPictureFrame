using DigitalFrame.Core;
using DigitalFrame.Infrastructure;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace DigitalFrame.Module.Settings.ViewModels
{
    public class SettingsShellViewModel : NotifyPropertyChangedBase, ISettingsShellViewModel
    {
        private bool _isVisible;

        public SettingsShellViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand<object>(Save);
            CancelCommand = new DelegateCommand<object>(Cancel);

            EventAggregator.GetEvent<DisplaySettingsEvent>().Subscribe(o => IsVisible = true);
        }

        private IEventAggregator EventAggregator { get; set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    NotifyPropertyChanged(() => IsVisible);

                    if (value == true)
                    {
                        EventAggregator.GetEvent<LoadSettingsEvent>().Publish(null);
                    }
                }
            }
        }

        private void Save(object obj)
        {
            EventAggregator.GetEvent<SaveSettingsEvent>().Publish(null);
        }

        private void Cancel(object obj)
        {
            IsVisible = false;
        }
    }
}