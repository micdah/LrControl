using System;
using System.Diagnostics;
using System.Threading;
using LrControl.Core;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace LrControl.Console
{
    public class Program
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<Program>();
        private static ILrControlApplication _lrControlApplication;

        public static void Main(string[] args)
        {
            InitializeLogging();
            
            if (args != null && args.Length > 0 && args[0] == "/Terminate")
            {
                TerminateApplication();
            }
            else
            {
                RunApplication();
            }
        }

        private static void RunApplication()
        {
            Log.Information("Starting LrControl application");

            _lrControlApplication = LrControlApplication.Create();
            _lrControlApplication.ConnectionStatus += (connected, version) =>
                Log.Information("Connection status: {Connected} {Version}", connected, version);

            _lrControlApplication.UpdateConnectionStatus();

            Log.Information("Available input devices:");
            foreach (var info in _lrControlApplication.DeviceBrowser.InputDevices)
                Log.Information("\tDevice: {@Info}", info);

            Log.Information("Available output devices:");
            foreach (var info in _lrControlApplication.DeviceBrowser.OutputDevices)
                Log.Information("\tDevice: {@Info}", info);

            Log.Information("Application running...");

            // Block process until terminated
            new ManualResetEvent(false).WaitOne();
        }
        
        private static void TerminateApplication()
        {
            Log.Information("Terminating LrControl application");

            var currentProcess = Process.GetCurrentProcess();
            var processName = currentProcess.ProcessName;

            foreach (var process in Process.GetProcessesByName(processName))
            {
                if (process.Id != currentProcess.Id)
                {
                    Log.Information("Killing process {Name}:{Id}", process.ProcessName, process.Id);
                    process.Kill();
                }
            }
        }

        private static void InitializeLogging()
        {
            // Configure logging
            const string template =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.sss} [{Level:u3}] {Message:lj}    ({SourceContext}){NewLine}{Exception}";

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()//.Verbose()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: template)
                // TODO Find solution for log file placement
                .WriteTo.RollingFile("/Users/micdah/LrControl.exe.{Date}.log",
                    outputTemplate: template,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    retainedFileCountLimit: 5,
                    shared: true)
                .CreateLogger();
        }
    }
}