using System;
using System.Windows;
using AmongUsLauncher.Helpers;

namespace AmongUsLauncher;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        DispatcherUnhandledException += (_, args) =>
        {
            MessageBox.Show(
                $"An error occurred:\n\n{args.Exception.Message}",
                "Among Us Launcher",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            args.Handled = true;
        };

        base.OnStartup(e);

        var config = ConfigHelper.Load();

        if (!config.Registered)
        {
            var wizard = new SetupWizard();
            var result = wizard.ShowDialog();

            if (result != true)
            {
                Shutdown();
                return;
            }

            config = ConfigHelper.Load();
        }

        var mainWindow = new MainWindow();
        mainWindow.Show();
    }
}
