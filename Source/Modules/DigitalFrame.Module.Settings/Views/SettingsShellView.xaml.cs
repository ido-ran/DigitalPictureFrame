using System.Windows.Controls;
using DigitalFrame.Module.Settings.ViewModels;
using Microsoft.Practices.Composite.Regions;

namespace DigitalFrame.Module.Settings.Views
{
    /// <summary>
    /// Interaction logic for SettingsShellView.xaml
    /// </summary>
    public partial class SettingsShellView : UserControl
    {
        public SettingsShellView(ISettingsShellViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
