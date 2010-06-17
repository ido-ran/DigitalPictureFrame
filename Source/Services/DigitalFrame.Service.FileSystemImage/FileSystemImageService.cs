using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using DigitalFrame.Core;
using DigitalFrame.Core.Extensions;
using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Service.Core;
using Microsoft.Practices.Composite.Events;

namespace DigitalFrame.Service.FileSystemImage
{
    public class FileSystemImageService : IImageService
    {
        private readonly object _locker = new object();
        private FileSystemImageSettings _fileSystemImageSettings;
        private IList<FileStatus> _imageFiles;
        private FileSystemWatcher _fileSystemWatcher;

        public FileSystemImageService(IRepository<FileSystemImageSettings> repository, IEventAggregator eventAggregator)
        {
            Repository = repository;
            EventAggregator = eventAggregator;

            _fileSystemImageSettings = Repository.Load();

            BeginLoadImageFiles();

            SubscribeToEvents();
        }

        private IRepository<FileSystemImageSettings> Repository { get; set; }
        private IEventAggregator EventAggregator { get; set; }

        #region IImageService Members

        public event EventHandler ImagesLoaded;
        public event EventHandler<GetImageEventArgs> GetImageComplete;

        public void GetImage()
        {
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += GetImage;
            backgroundWorker.RunWorkerAsync();
        }

        #endregion

        private void BeginLoadImageFiles()
        {
            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += LoadImageFiles;
            backgroundWorker.RunWorkerCompleted += LoadImageFilesCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void SubscribeToEvents()
        {
            var settingsChangedEvent = EventAggregator.GetEvent<SettingsChangedEvent<FileSystemImageSettings>>();

            if (settingsChangedEvent != null)
            {
                settingsChangedEvent.Subscribe(OnSettingsChangedEvent);
            }
        }

        private void OnSettingsChangedEvent(FileSystemImageSettings fileSystemImageSettings)
        {
            if (_fileSystemImageSettings.Path == fileSystemImageSettings.Path)
            {
                return;
            }

            _fileSystemImageSettings = fileSystemImageSettings;

            BeginLoadImageFiles();
        }

        private void LoadImageFilesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ImagesLoaded.Raise(this, EventArgs.Empty);

            if (string.IsNullOrEmpty(_fileSystemImageSettings.Path))
            {
                return;
            }

            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;

                _fileSystemWatcher.Deleted -= OnFileDeleted;
                _fileSystemWatcher.Created -= OnFileCreated;
                _fileSystemWatcher.Renamed -= OnFileRenamed;
            }

            try
            {
                _fileSystemWatcher = new FileSystemWatcher(_fileSystemImageSettings.Path)
                                         {
                                             IncludeSubdirectories = true,
                                             Filter = "*.jpg",
                                             NotifyFilter = NotifyFilters.FileName
                                         };

                _fileSystemWatcher.Deleted += OnFileDeleted;
                _fileSystemWatcher.Created += OnFileCreated;
                _fileSystemWatcher.Renamed += OnFileRenamed;

                _fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                // Swallow the exception.  This is typically because an invalid path was entered...
            }
        }

        private void LoadImageFiles(object sender, DoWorkEventArgs e)
        {
            var directoryInfo = new DirectoryInfo(_fileSystemImageSettings.Path);

            List<FileStatus> imageFiles;

            try
            {
                imageFiles = (from f in GetFiles(directoryInfo)
                              select new FileStatus
                                         {
                                             Path = f.FullName
                                         }).ToList();
            }
            catch (Exception)
            {
                // Swallow the exception.  This is typically because an invalid path was entered...
                imageFiles = new List<FileStatus>();
            }

            lock (_locker)
            {
                _imageFiles = imageFiles;
            }
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            FileStatus fileStatus = (from f in _imageFiles
                                     where f.Path == e.OldFullPath
                                     select f).SingleOrDefault();

            if (fileStatus != null)
            {
                fileStatus.Path = e.FullPath;
            }
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            FileStatus fileStatus = (from f in _imageFiles
                                     where f.Path == e.FullPath
                                     select f).SingleOrDefault();

            if (fileStatus != null)
            {
                _imageFiles.Remove(fileStatus);
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            _imageFiles.Add(new FileStatus
                                {
                                    Path = e.FullPath
                                });
        }

        private void GetImage(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if (_imageFiles == null)
            {
                GetImageComplete.Raise(this, new GetImageEventArgs());
                return;
            }

            string path;
            var r = new Random();

            lock (_locker)
            {
                int index;
                while (true)
                {
                    index = r.Next(_imageFiles.Count);

                    if (!_imageFiles[index].Used)
                    {
                        _imageFiles[index].Used = true;
                        break;
                    }
                }

                path = _imageFiles[index].Path;
            }

            CheckUsedFiles();

            BitmapImage image = LoadImage(path);

            image.Freeze();

            GetImageComplete.Raise(this, new GetImageEventArgs{Image = image});
        }

        private static BitmapImage LoadImage(string path)
        {
            //if (!_loading) { return; }

            byte[] buffer = File.ReadAllBytes(path);

            // by reading the data into an in-memory buffer, we prevent the file from being read in the UI thread -- which speeds up 
            // access dramatically!

            var memoryStream = new MemoryStream(buffer);

            //if (!_loading)
            //{
            //    mem.Dispose();
            //    return;
            //}

            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            bitmapImage.StreamSource = memoryStream;

            bitmapImage.EndInit();

            // if you dispose of the memory stream here, the image will be toast (burnt toast) 
            // (as the dispatcher won't have run yet).

            //_loading = false;

            return bitmapImage;
        }

        private void CheckUsedFiles()
        {
            int numberOfUsedFiles;
            int count;

            lock (_locker)
            {
                numberOfUsedFiles = (from f in _imageFiles
                                     where f.Used
                                     select f).Count();

                count = _imageFiles.Count;
            }

            if (numberOfUsedFiles > count*.75)
            {
                ResetUsedFiles();
            }
        }

        private void ResetUsedFiles()
        {
            lock (_locker)
            {
                foreach (FileStatus fileStatus in _imageFiles)
                {
                    fileStatus.Used = false;
                }
            }
        }

        private static IList<FileInfo> GetFiles(DirectoryInfo directoryInfo)
        {
            IList<FileInfo> imageFiles = new List<FileInfo>();

            FileInfo[] files = directoryInfo.GetFiles("*.jpg");

            foreach (FileInfo fileInfo in files)
            {
                imageFiles.Add(fileInfo);
            }

            DirectoryInfo[] directories = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);

            foreach (DirectoryInfo directory in directories)
            {
                foreach (FileInfo fileInfo in GetFiles(directory))
                {
                    imageFiles.Add(fileInfo);
                }
            }

            return imageFiles;
        }
    }
}