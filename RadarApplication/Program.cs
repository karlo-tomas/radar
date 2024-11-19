using RadarLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        long decimalRadar = 2130706433; //IP 127.0.0.1 in decimal
        string radarIP = HelperMethods.DecimalToIpAddress(decimalRadar);
        string radarPort = "8001";
        string radarUri = $"http://{radarIP}:{radarPort}";
        var radarControl = new RadarControl(radarUri);
        int delayMilliseconds = 2000;

        while (true)
        {
            await radarControl.FetchRadarDataAsync();

            if (radarControl.IsRadarOnline)
            {
                if (delayMilliseconds > 2000)
                    delayMilliseconds = 2000;
                //Console.WriteLine("Radar is online. Fetching targets...");
                var filteredTargets = radarControl.CurrentTargets
                    .Where(t => t.Angle >= -45 && t.Angle <= 45 && t.Distance <= 100)
                    .ToList();

                if (filteredTargets.Any())
                {
                    Console.WriteLine("Visible targets:");
                    foreach (var target in filteredTargets)
                    {
                        Console.WriteLine(target);
                    }
                }
                else
                {
                    Console.WriteLine("No targets within the radar's scope");
                }
            }
            else
            {
                delayMilliseconds = 10000;

                Console.WriteLine($"[{DateTime.Now}] Radar is offline!");
                if (!string.IsNullOrWhiteSpace(radarControl.LastError))
                {
                    Console.WriteLine($"Error: {radarControl.LastError}");
                }
                else
                {
                    Console.WriteLine("No error details available.");
                }

                Console.WriteLine("Last discovered targets:");
                foreach (var target in radarControl.CurrentTargets)
                {
                    Console.WriteLine(target);
                }
            }

            await Task.Delay(delayMilliseconds); // Poll every 2 seconds
        }
    }
}
