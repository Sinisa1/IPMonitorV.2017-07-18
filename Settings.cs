using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
namespace IPMonitor {
    public static class Settings {
        public static string appToKillList { get; set; }
        public static string[] appToKillArray { get; set; }
        public static double LogEntryWriteInterval { get; set; }
        public static double IPCheckTimerInterval { get; set; }
        public static int TimeoutInterval { get; set; }

        public static List<IPCheckURLsConfigInstanceElement> IPCheckURLs { get; set; }
        public static Random randomZeroOne;

        public static List<string> logEntries { get; set; }

        public static void Initialize() {
            appToKillList = ConfigurationManager.AppSettings.Get("AppToKill");
            LogEntryWriteInterval = double.Parse(ConfigurationManager.AppSettings.Get("LogEntryWriteInterval"));
            IPCheckTimerInterval = double.Parse(ConfigurationManager.AppSettings.Get("IPCheckTimerInterval"));
            TimeoutInterval = int.Parse(ConfigurationManager.AppSettings.Get("TimeoutInterval"));

            appToKillArray = appToKillList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            IPCheckURLs = new List<IPCheckURLsConfigInstanceElement>();
            IPCheckURLsConfigSection config = System.Configuration.ConfigurationManager.GetSection("IPCheckURLs") as IPCheckURLsConfigSection;
            foreach (IPCheckURLsConfigInstanceElement e in config.Instances) {
                e.CalculateSuccessPercent();
                IPCheckURLs.Add(e); // Try to find LINQ solution
            }

            randomZeroOne = new Random(1);
            logEntries = new List<string>();
        }
        public static List<Tuple<string, double, double, double>> GetIPCheckURLStatisticList() // Not used
        {
            return IPCheckURLs.Select(x => new Tuple<string, double, double, double>(x.Code, x.NumberUsed, x.NumberFailed, x.IPCheckSuccessPercent)).ToList();// < Tuple<string, long, long, double>>();
        }

        public static void LogEntriesAdd(string msg) {

            logEntries.Add(string.Format("{0} {1}", Utilities.GetDateTime(), msg));
            //     msgQueue.Enqueue(string.Format("{0} {1}", Utilities.GetDateTime(), msg));

            int msgCount = msgArrList.Count;
            if (msgCount > 100) {
                msgArrList.RemoveRange(0, 50);
            }
            msgArrList.Add(string.Format("{0} {1}", Utilities.GetDateTime(), msg));
        }


        // public static FixedSizeQueue<string> msgQueue = new FixedSizeQueue<string>(10);
        public static ArrayList msgArrList = new ArrayList(10);

    }


    public class FixedSizeQueue<T> {
        readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public int Size { get; private set; }

        public FixedSizeQueue(int size) {
            Size = size;
        }

        public void Enqueue(T obj) {
            // add item to the queue
            queue.Enqueue(obj);

            lock (this) // lock queue so that queue.Count is reliable
            {
                while (queue.Count > Size) // if queue count > max queue size, then dequeue an item
                {
                    T objOut;
                    queue.TryDequeue(out objOut);
                }
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
