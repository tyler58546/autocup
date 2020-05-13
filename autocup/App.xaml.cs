using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace autocup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Discord.Discord discord;
        DiscordRPCManager RPCManager = new DiscordRPCManager();
        System.Timers.Timer discordTimer = new System.Timers.Timer();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            discord = new Discord.Discord(709891028529971250, (UInt64)Discord.CreateFlags.Default);
            discord.RunCallbacks();
            RPCManager.UpdateRPC("Download at http://cup.wtf", "Idle", null);
            discordTimer.Elapsed += new System.Timers.ElapsedEventHandler(DiscordTimerElapsed);
            discordTimer.Interval = 500;
            discordTimer.Start();
        }
        private void DiscordTimerElapsed(object sender, ElapsedEventArgs e)
        {
            discord.RunCallbacks();
        }
    }
}
