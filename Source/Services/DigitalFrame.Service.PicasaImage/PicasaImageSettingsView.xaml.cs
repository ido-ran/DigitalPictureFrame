using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalFrame.Service.PicasaImage {
  /// <summary>
  /// Interaction logic for PicasaImageSettingsView.xaml
  /// </summary>
  public partial class PicasaImageSettingsView : UserControl {
    public PicasaImageSettingsView(IPicasaImageSettingsViewModel viewModel) {
      InitializeComponent();

      DataContext = viewModel;
    }
  }
}
