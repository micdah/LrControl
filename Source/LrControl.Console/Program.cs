using System;
using LrControl.Core;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace LrControl.Console
{
    class Program
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<Program>();

        static void Main()
        {
            InitializeLogging();

            var lrControlApplication = LrControlApplication.Create();

            lrControlApplication.ConnectionStatus += (connected, version) =>
                Log.Information("Connection status: {Connected} {Version}", connected, version);

            lrControlApplication.UpdateConnectionStatus();

            Log.Information("Available input devices:");
            foreach (var info in lrControlApplication.InputDevices)
                Log.Information("\tDevice: {@Info}", info);

            Log.Information("Available output devices:");
            foreach (var info in lrControlApplication.OutputDevices)
                Log.Information("\tDevice: {@Info}", info);

            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadLine();

            lrControlApplication.Dispose();
        }

        private static void InitializeLogging()
        {
            // Configure logging
            const string template =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.sss} [{Level:u3}] {Message:lj}    ({SourceContext}){NewLine}{Exception}";

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: template)
                .WriteTo.RollingFile("LrControl.exe.{Date}.log",
                    outputTemplate: template,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    retainedFileCountLimit: 5,
                    shared: true)
                .CreateLogger();
        }
    }
}