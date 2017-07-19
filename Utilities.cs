﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
namespace IPMonitor
{
    public static class Utilities
    {
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string GetDateTime()
        {
            return DateTime.Now.ToString(DATE_TIME_FORMAT);
        }

        public static int KillProgram(string[] appToKill)
        {
            try
            {
                foreach (string appName in appToKill)
                {
                    Process[] processes = Process.GetProcessesByName(appName);

                    foreach (Process proc in processes)
                    {
                        proc.Kill();
                        string status = Settings.appToKillList + " killed at " + Utilities.GetDateTime();
                        Logger.logger.InfoFormat("{0}", status);
                    }
                }
            }
            catch (Exception ex)
            {
                string status = " Failed to kill "+Settings.appToKillList +" "+ex.Message;
                Logger.logger.InfoFormat("{0}", status);

            }
            return 0;
        }
        public static void RunProcess(string name_)
        {
            Process.Start(name_);
        }
    }
 }