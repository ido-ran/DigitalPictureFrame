using System.Windows;
using DigitalFrame.ViewModels;

namespace DigitalFrame.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView(IShellViewViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}