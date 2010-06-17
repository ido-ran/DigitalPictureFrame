using System.Windows.Controls;
using DigitalFrame.Module.Image.ViewModels;

namespace DigitalFrame.Module.Image.Views
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView(IImageViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
