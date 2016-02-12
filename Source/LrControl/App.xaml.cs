using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (IsShutdownRequest(e))
            {
                TerminateAllInstances();
            }

            var mainWindow = new MainWindow();

            mainWindow.Show();
        }

        private static bool IsShutdownRequest(StartupEventArgs e)
        {
            return e.Args.Any(x => x.ToLowerInvariant().Equals("/shutdown"));
        }

        private static void TerminateAllInstances()
        {
            var current = Process.GetCurrentProcess();

            var others = Process.GetProcessesByName(current.ProcessName)
                .Where(p => p.Id != current.Id)
                .ToList();
            foreach (var other in others)
            {
                try
                {
                    other.Kill();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to kill process ({other.Id}): {e.Message}");
                }
            }

            current.Kill();
        }
    }
}