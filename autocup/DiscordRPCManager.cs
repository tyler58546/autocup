using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autocup
{
    class DiscordRPCManager
    {
        Discord.ActivityManager activityManager;
        public void UpdateRPC(String details, long? startTimestamp)
        {
            activityManager = ((App)System.Windows.Application.Current).discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = "Download: http://cup.wtf",
                Details = details,
                Assets =
                {
                    LargeImage = "cup"
                },
                Instance = true,
            };
            if (startTimestamp.HasValue)
            {
                activity = new Discord.Activity
                {
                    State = "Download: http://cup.wtf",
                    Details = details,
                    Timestamps =
                      {
                          Start = startTimestamp.Value
                      },
                    Instance = true,
                };
            }
            try
            {
                //System.GC.EndNoGCRegion();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //GC.TryStartNoGCRegion(8388608);
            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result != Discord.Result.Ok)
                {
                    Console.WriteLine("Failed to update Discord RPC!");
                }
                //GC.EndNoGCRegion();
            });
        }
    }
}
