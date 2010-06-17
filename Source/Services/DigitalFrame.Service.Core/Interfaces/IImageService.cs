using System;
using System.Windows.Media.Imaging;
using DigitalFrame.Core;

namespace DigitalFrame.Service.Core
{
    public interface IImageService
    {
        event EventHandler ImagesLoaded;
        event EventHandler<GetImageEventArgs> GetImageComplete;

        void GetImage();
    }

    public class GetImageEventArgs : EventArgs
    {
        public BitmapImage Image { get; set; }
        public string Title { get; set; }
    }
}