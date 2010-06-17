using System.Windows.Controls;
using DigitalFrame.Module.Weather.ViewModels;

namespace DigitalFrame.Module.Weather.Views
{
    /// <summary>
    /// Interaction logic for WeatherSettingsView.xaml
    /// </summary>
    public partial class WeatherSettingsView : UserControl
    {
        public WeatherSettingsView(IWeatherSettingsViewModel weatherSettingsViewModel)
        {
            InitializeComponent();

            DataContext = weatherSettingsViewModel;
        }
    }
}
