using Serilog;
using Xunit.Abstractions;

namespace LrControl.Tests
{
    public abstract class TestSuite
    {
        protected TestSuite(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output)
                .CreateLogger();
        }
    }
}