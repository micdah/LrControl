using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LrControlCore.Configurations;
using micdah.LrControlApi;
using Serilog;

namespace micdah.LrControl
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private LrApi _lrApi;
        private MainWindow _mainWindow;
        private MainWindowModel _viewModel;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (IsShutdownRequest(e))
            {
                TerminateAllInstances();
            }
            else
            {
                SetupExceptionHandling();
                SetupLogging();
                ShowMainWindow();
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            if (Settings.Current.SaveConfigurationOnExit)
            {
                _viewModel.SaveConfiguration();
            }

            Settings.Current.SetLastUsed(_viewModel.InputDevice, _viewModel.OutputDevice);
            Settings.Current.Save();

            _lrApi.Dispose();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) =>
                {
                    new ErrorDialog(args.ExceptionObject as Exception).Show();
                };

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

        private void SetupLogging()
        {
            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.sss} [{level}] {Message}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole(outputTemplate: template)
                .WriteTo.RollingFile("LrControl.exe.{Date}.log", outputTemplate: template)
                .CreateLogger();
        }

        private void ShowMainWindow()
        {
            // Create LrApi
            _lrApi = new LrApi();

            // Create and show main window
            _viewModel = new MainWindowModel(_lrApi);
            _mainWindow = new MainWindow(_viewModel)
            {
                WindowState = Settings.Current.StartMinimized ? WindowState.Minimized : WindowState.Normal
            };
            _viewModel.DialogProvider = new MainWindowDialogProvider(_mainWindow);

            _viewModel.LoadConfiguration();
            _viewModel.RefreshAvailableDevices();
            _viewModel.InputDeviceName = Settings.Current.LastUsedInputDevice;
            _viewModel.OutputDeviceName = Settings.Current.LastUsedOutputDevice;

            _mainWindow.Show();
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
                    other.CloseMainWindow();
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