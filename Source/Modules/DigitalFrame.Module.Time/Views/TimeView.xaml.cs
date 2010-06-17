using System.Windows.Controls;
using DigitalFrame.Module.Time.ViewModels;

namespace DigitalFrame.Module.Time.Views
{
    /// <summary>
    /// Interaction logic for TimeView.xaml
    /// </summary>
    public partial class TimeView : UserControl
    {
        public TimeView(ITimeViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
