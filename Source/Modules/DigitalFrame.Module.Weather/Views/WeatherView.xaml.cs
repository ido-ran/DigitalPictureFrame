using System.Windows.Controls;
using DigitalFrame.Module.Weather.ViewModels;

namespace DigitalFrame.Module.Weather.Views
{
    /// <summary>
    /// Interaction logic for WeatherView.xaml
    /// </summary>
    public partial class WeatherView : UserControl
    {
        private readonly IWeatherViewModel _viewModel;

        public WeatherView(IWeatherViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
