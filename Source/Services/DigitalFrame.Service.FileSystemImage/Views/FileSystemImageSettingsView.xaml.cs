using System.Windows.Controls;
using DigitalFrame.Service.FileSystemImage.ViewModels;

namespace DigitalFrame.Service.FileSystemImage.Views
{
    /// <summary>
    /// Interaction logic for FileSystemImageSettingsView.xaml
    /// </summary>
    public partial class FileSystemImageSettingsView : UserControl
    {
        public FileSystemImageSettingsView(IFileSystemImageSettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
