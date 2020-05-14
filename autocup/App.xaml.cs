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

        private async void checkForUpdates()
        {
            Updater updater = new Updater();
            Update update = await updater.checkForUpdatesAsync();
            if (update != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("A new version of Autocup is available (" + update.version + "). Would you like to download it now?", "Update Available", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(update.downloadURL);
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", "0");
            discord = new Discord.Discord(709891028529971250, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            discord.RunCallbacks();
            RPCManager.UpdateRPC("Idle", null);
            discordTimer.Elapsed += new System.Timers.ElapsedEventHandler(DiscordTimerElapsed);
            discordTimer.Interval = 500;
            discordTimer.Start();

            checkForUpdates();
        }
        private void DiscordTimerElapsed(object sender, ElapsedEventArgs e)
        {
            discord.RunCallbacks();
        }
    }
}
