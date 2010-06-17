using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DigitalFrame.Core;
using DigitalFrame.Service.Core;

namespace DigitalFrame.Module.Image.ViewModels
{
    public class ImageViewModel : NotifyPropertyChangedBase, IImageViewModel
    {
        private readonly Dispatcher _dispatcher;
        private readonly IImageService _imageService;

        private BitmapImage _currentImage;
        private string _currentTitle;

        public ImageViewModel(IImageService imageService)
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running 
                _dispatcher = Dispatcher.CurrentDispatcher;
            }

            _imageService = imageService;
            _imageService.ImagesLoaded += OnImagesLoaded;
            _imageService.GetImageComplete += OnGetImageCompleted;
        }

        private void OnImagesLoaded(object sender, EventArgs e)
        {
            _imageService.GetImage();

            var timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 15)
            };

            timer.Tick += OnTimerElapsed;
            timer.Start();
        }

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    NotifyPropertyChanged(() => CurrentImage);
                }
            }
        }

        public string CurrentTitle 
        {
          get { return _currentTitle; }
          set 
          {
            _currentTitle = value;
            NotifyPropertyChanged(() => CurrentTitle);
          }
        }

        private void OnGetImageCompleted(object sender, GetImageEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(delegate 
              { 
                CurrentImage = e.Image;
                CurrentTitle = e.Title ?? e.Image.UriSource.ToString();
              }));
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            _imageService.GetImage();
        }
    }
}