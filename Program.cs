using System;
using System.Timers;
using FH4RP.Helpers;
using FH4RP.Networking;
using Microsoft.Extensions.Configuration;

namespace FH4RP
{
    class Program
    {
        public static Config Config { get; private set; }
        public static TelemetryServer Server { get; private set; }
        
        static void Main(string[] args)
        {

            var settings = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true).Build();
            var section = settings.GetSection(nameof(Config));
            Config = section.Get<Config>() ?? new Config();
            
            Console.WriteLine("Forza Horizon 5 | Discord Rich Presence App");
            Console.WriteLine("Developed by jaiden-d modified by PiSaucer and P1NK-GANG");
            Console.WriteLine();
            Console.WriteLine("If you haven't already, open a command prompt as admin");
            Console.WriteLine("and run the following command to allow Forza Horizon 4's");
            Console.WriteLine("Telemetry Data Out to send to the local PC:");
            Console.WriteLine();
            Console.WriteLine("> CheckNetIsolation.exe LoopbackExempt -a -n=Microsoft.SunriseBaseGame_8wekyb3d8bbwe");
            Console.WriteLine();
            Server = new TelemetryServer(Config.ServerPort);
            Server.Start();

            Networking.DiscordRPC.Initialize();

            var timer = new Timer(1250);
            timer.Elapsed += (sender, a) => { Networking.DiscordRPC.SetPresence(Server.LastUpdate); };
            timer.Start();

            // Wait for keypress to close
            Console.Read();
            timer.Stop();
            Networking.DiscordRPC.discordClient.Dispose();
        }
    }
}
