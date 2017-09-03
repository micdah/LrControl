using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LrControl.Core;
using Serilog;

namespace LrControl.Ui
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<App>();

        private ILrControlApplication _lrControlApplication;
        
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (IsShutdownRequest(e))
            {
                Log.Information("Shutdown request received, terminating all running instances of LrControl.exe");
                TerminateAllInstances();
            }
            else
            {
                ConfigureExceptionHandling();
                ShowMainWindow();
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _lrControlApplication?.Dispose();
        }

        private void ConfigureExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) => { new ErrorDialog(args.ExceptionObject as Exception).Show(); };

            Dispatcher.UnhandledException += (o, eventArgs) =>
            {
                new ErrorDialog(eventArgs.Exception).Show();
                eventArgs.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                new ErrorDialog(args.Exception).Show();
                args.SetObserved();
            };
        }

        private void ShowMainWindow()
        {
            // Create and show main window
            _lrControlApplication = LrControlApplication.Create();

            var mainWindowViewModel = new MainWindowModel(Dispatcher.CurrentDispatcher, _lrControlApplication);
            var mainWindow = new MainWindow(mainWindowViewModel)
            {
                WindowState = _lrControlApplication.Settings.StartMinimized ? WindowState.Minimized : WindowState.Normal
            };
            mainWindowViewModel.DialogProvider = new MainWindowDialogProvider(mainWindow);
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
                try
                {
                    other.CloseMainWindow();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to kill process ({other.Id}): {e.Message}");
                }

            current.Kill();
        }
    }
}