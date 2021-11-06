using System;
using System.Collections.Generic;
using System.Text;
using DiscordRPC;
using DiscordRPC.Logging;
using FH4RP.DataStructs;
using FH4RP.Helpers;

namespace FH4RP.Networking
{
    public static class DiscordRPC
    {
        public static DiscordRpcClient discordClient;

        //Fix For Time Resetting To 00:00 Each Update
        private static Timestamps elapsedTime;

        public static void Initialize()
        {
            discordClient = new DiscordRpcClient("906542327328280646");
            discordClient.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            discordClient.OnReady += (sender, e) => { Console.WriteLine($"[Discord] Received ready from {e.User.Username}!"); };
            discordClient.OnPresenceUpdate += (sender, e) => { /*Console.WriteLine("[Discord] Received presence update!");*/ };
            if (elapsedTime == null)
            {
                //Only Define Timestamp Once - Fix For Time Resetting To 00:00 Each Update - Only Done Once Project Is Opened And File Is Selected
                elapsedTime = Timestamps.Now;
            }
            discordClient.Initialize();
        }

        public static void SetPresence(TelemetryData data)
        {
            discordClient.SetPresence(new RichPresence()
            {
                Details = VehicleDB.Instance.GetVehicle(data.CarOrdinal).GetVehicleInfo(),
                State = $"[{data.CarClass.ToString()} | {data.CarPI}] - {data.GetMPH()} mph",
                Timestamps = elapsedTime,
                Assets = new Assets()
                {
                    LargeImageKey = "forza-horizon-5",
                    SmallImageKey = "h5-small"
                }
        });
        }

    }
}
