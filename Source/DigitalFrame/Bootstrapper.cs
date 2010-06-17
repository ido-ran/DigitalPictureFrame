using System.Windows;
using DigitalFrame.ViewModels;
using DigitalFrame.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;

namespace DigitalFrame
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Container.RegisterType<IShellViewViewModel, ShellViewModel>();

            var shell = Container.Resolve<ShellView>();

            shell.Show();

            return shell;
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}