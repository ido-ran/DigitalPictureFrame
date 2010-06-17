using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Service.FileSystemImage.ViewModels
{
    public class FileSystemImageSettingsViewModel : SettingsViewModel<FileSystemImageSettings>, IFileSystemImageSettingsViewModel
    {
        private string _path;

        public FileSystemImageSettingsViewModel(IRepository<FileSystemImageSettings> repository, IEventAggregator eventAggregator)
            : base(repository, eventAggregator)
        {
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    NotifyPropertyChanged(() => Path);
                }
            }
        }

        protected override void OnSaveSettings(object obj)
        {
            var fileSystemImageSettings = new FileSystemImageSettings() { Path = Path};

            Repository.Save(fileSystemImageSettings);

            PublishSettingsSavedEvent(fileSystemImageSettings);
        }

        protected override void OnLoadSettings(object obj)
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            var fileSystemImageSettings = Repository.Load();

            Path = fileSystemImageSettings.Path;
        }
    }
}