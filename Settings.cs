using System;
using System.Collections.Generic;
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

       public static List<IPCheckURLsConfigInstanceElement> IPCheckURLs { get;
            
                //    IList<MyConfigInstanceElement> outElements = new List<MyConfigInstanceElement>();
                //    MyConfigSection config = System.Configuration.ConfigurationManager.GetSection("IPCheckURLs") as MyConfigSection;
                //    //config.Instances.to;
                //    outElements=(IList<MyConfigInstanceElement>)config.Instances;
                //    return outElements;
                //}





                set;
        }

        static  Settings()
       {
           appToKillList = ConfigurationManager.AppSettings.Get("AppToKill");
           LogEntryWritePeriod = double.Parse(ConfigurationManager.AppSettings.Get("LogEntryWritePeriod"));
           IPCheckTimerInterval = double.Parse(ConfigurationManager.AppSettings.Get("IPCheckTimerInterval"));
           TimeoutInterval = int.Parse(ConfigurationManager.AppSettings.Get("TimeoutInterval"));

           appToKillArray = appToKillList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            IPCheckURLs = new List<IPCheckURLsConfigInstanceElement>();
            IPCheckURLsConfigSection config = System.Configuration.ConfigurationManager.GetSection("IPCheckURLs") as IPCheckURLsConfigSection;
             foreach (IPCheckURLsConfigInstanceElement e in config.Instances)
            {
                IPCheckURLs.Add(e); // Try to find LINQ solution
            }
          



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
