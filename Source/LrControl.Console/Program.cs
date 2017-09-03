using LrControl.Core;

namespace LrControl.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var lrControlApplication = LrControlApplication.Create();

            lrControlApplication.ConnectionStatus += (connected, version) =>
                System.Console.WriteLine($"Connection status: {connected} {version}");

            lrControlApplication.UpdateConnectionStatus();

            System.Console.ReadLine();
        }
    }
}
