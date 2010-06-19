using System.Windows.Controls;
using DigitalFrame.Module.Image.ViewModels;

namespace DigitalFrame.Module.Image.Views
{
    /// <summary>
    /// Interaction logic for ImageSettingsView.xaml
    /// </summary>
    public partial class ImageSettingsView : UserControl
    {
        public ImageSettingsView(IImageSettingsViewModel viewModel)
        {
            InitializeComponent();
        }

        public object Header {
          get { return "Images"; }
        }
    }
}
