using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using micdah.LrControl.Configurations;
using micdah.LrControlApi;

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
                HookupUnhandledExceptionDialog();
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

            Settings.Current.SetLastUsedFrom(_viewModel);
            Settings.Current.Save();

            _lrApi.Dispose();
        }

        private void HookupUnhandledExceptionDialog()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) => ShowExceptionDialog(args.ExceptionObject as Exception);

            Dispatcher.UnhandledException += (o, eventArgs) =>
            {
                ShowExceptionDialog(eventArgs.Exception);
                eventArgs.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                ShowExceptionDialog(args.Exception);
                args.SetObserved();
            };
        }

        private void ShowExceptionDialog(Exception exception)
        {
            new ErrorDialog(exception).Show();
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