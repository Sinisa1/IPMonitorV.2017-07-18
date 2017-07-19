using System;

using System.Configuration;
namespace IPMonitor
{
   public static class Settings
    {
       public static string appToKillList { get; set; }
       public static string[] appToKillArray { get; set; }
       public static double LogEntryWritePeriod { get; set; }
       public static double IPCheckTimerInterval { get; set; }
       public static int TimeoutInterval { get; set; }

        static  Settings()
       {
           appToKillList = ConfigurationManager.AppSettings.Get("AppToKill");
           LogEntryWritePeriod = double.Parse(ConfigurationManager.AppSettings.Get("LogEntryWritePeriod"));
           IPCheckTimerInterval = double.Parse(ConfigurationManager.AppSettings.Get("IPCheckTimerInterval"));
           TimeoutInterval = int.Parse(ConfigurationManager.AppSettings.Get("TimeoutInterval"));

           appToKillArray = appToKillList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
       }
    }

    //   public static void SetAppsToKill()
    //   {
    //       appToKillList = ConfigurationManager.AppSettings.Get("AppToKill");
    //       LogEntryWritePeriod = double.Parse(ConfigurationManager.AppSettings.Get("LogEntryWritePeriod"));
    //       IPCheckTimerInterval = double.Parse(ConfigurationManager.AppSettings.Get("IPCheckTimerInterval"));
    //       TimeoutInterval = int.Parse(ConfigurationManager.AppSettings.Get("TimeoutInterval"));
           
    //       Settings.appToKillArray = appToKillList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    //   }
    //}
}
