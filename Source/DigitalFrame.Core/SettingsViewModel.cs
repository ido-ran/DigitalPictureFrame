using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Core
{
    public abstract class SettingsViewModel<TSettings> : NotifyPropertyChangedBase
    {
        protected SettingsViewModel(IRepository<TSettings> repository, IEventAggregator eventAggregator)
        {
            Repository = repository;
            EventAggregator = eventAggregator;

            SubscribeToEvents();
        }

        protected IRepository<TSettings> Repository { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }

        private void SubscribeToEvents()
        {
            var loadSettingsEvent = EventAggregator.GetEvent<LoadSettingsEvent>();

            if (loadSettingsEvent != null)
            {
                loadSettingsEvent.Subscribe(OnLoadSettings);
            }

            var saveSettingsEvent = EventAggregator.GetEvent<SaveSettingsEvent>();

            if (saveSettingsEvent != null)
            {
                saveSettingsEvent.Subscribe(OnSaveSettings);
            }
        }

        protected abstract void OnSaveSettings(object obj);
        protected abstract void OnLoadSettings(object obj);

        protected void PublishSettingsSavedEvent(TSettings settings)
        {
            var settingsChangedEvent = EventAggregator.GetEvent<SettingsChangedEvent<TSettings>>();

            if (settingsChangedEvent != null)
            {
                settingsChangedEvent.Publish(settings);
            }
        }
    }
}