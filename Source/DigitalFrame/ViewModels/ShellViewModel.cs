using DigitalFrame.Infrastructure;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace DigitalFrame.ViewModels
{
    public class ShellViewModel : IShellViewViewModel
    {
        private IEventAggregator EventAggregator { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            SettingsCommand = new DelegateCommand<object>(Settings);
        }

        private void Settings(object obj)
        {
            EventAggregator.GetEvent<DisplaySettingsEvent>().Publish(null);;
        }

        public DelegateCommand<object> SettingsCommand { get; private set; }
    }
}
