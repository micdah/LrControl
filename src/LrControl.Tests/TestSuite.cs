using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace LrControl.Tests
{
    public abstract class TestSuite
    {
        protected TestSuite(ITestOutputHelper output)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, LogEventLevel.Verbose)
                .CreateLogger();
        }
    }
}