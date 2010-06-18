using System.Windows;
using DigitalFrame.ViewModels;
using DigitalFrame.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using System;
using System.Windows.Input;

namespace DigitalFrame
{
    public class Bootstrapper : UnityBootstrapper
    {
      private StartupEventArgs startupArgs;

      public Bootstrapper(StartupEventArgs startupArgs) {
        this.startupArgs = startupArgs;
      }

        protected override DependencyObject CreateShell()
        {
            Container.RegisterType<IShellViewViewModel, ShellViewModel>();

            var shell = Container.Resolve<ShellView>();

            if (Array.IndexOf<string>(startupArgs.Args, "-unattented") != -1) {
              // Make the shell full screen window.
              shell.WindowState = WindowState.Maximized;
              shell.WindowStyle = WindowStyle.None;
              Mouse.OverrideCursor = Cursors.None;
            }

            shell.Show();

            return shell;
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }
    }
}