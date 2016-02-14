using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

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
                return;
            }

            SetupLogging();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void SetupLogging()
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message\n"
            };
            patternLayout.ActivateOptions();


            var consoleAppender = new ColoredConsoleAppender
            {
                Threshold = Level.Debug,
                Layout = patternLayout
            };
            consoleAppender.ActivateOptions();
            hierarchy.Root.AddAppender(consoleAppender);


            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
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