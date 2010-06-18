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
        private readonly DispatcherTimer timer;

        private BitmapImage _currentImage;
        private string _currentTitle;
        private object _userMessage;

        private static readonly string LOADING_MSG = "Loading Images...";
        private static readonly string LOAD_FAIL_MSG = "Fail to load images, please check your settings";

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

            _userMessage = LOADING_MSG;

            _imageService = imageService;
            _imageService.ImagesLoaded += OnImagesLoaded;
            _imageService.GetImageComplete += OnGetImageCompleted;

            timer = new DispatcherTimer(DispatcherPriority.Normal, _dispatcher);
            timer.Interval = TimeSpan.FromSeconds(15); // TODO: Get from settings
            timer.Tick += OnTimerElapsed;
        }

        private void OnImagesLoaded(object sender, EventArgs e)
        {
            _imageService.GetImage();

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

        public object UserMessage 
        {
          get { return _userMessage; }
          set {
            if (!object.Equals(_userMessage, value)) {
              _userMessage = value;
              NotifyPropertyChanged(() => UserMessage);
            }
          }
        }

        private void OnGetImageCompleted(object sender, GetImageEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(delegate 
              {
                if (e.Image == null) {
                  CurrentImage = null;
                  CurrentTitle = string.Empty;

                  // If we got null image we stop asking for images.
                  timer.Stop();

                  UserMessage = LOAD_FAIL_MSG;
                }
                else {
                  UserMessage = LOADING_MSG;
                  CurrentImage = e.Image;
                  CurrentTitle = e.Title ?? e.Image.UriSource.ToString();
                }
              }));
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            _imageService.GetImage();
        }
    }
}