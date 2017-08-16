using System;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IPMonitor {
    public class Web {
        /// <summary>
        /// 

        //http://stackoverflow.com/questions/13270042/get-public-remote-ip-address
        //Akamai provides a "What is my IP" page that you can fetch:
        //require 'open-uri'
        //remote_ip = open('http://whatismyip.akamai.com').read

        //There are a few alternatives that do the same thing, though:
        //•http://whatismyip.akamai.com  OK
        //•http://ipecho.net/plain  //Restricted URL
        //•http://icanhazip.com    OK
        //•http://ipogre.com   DOWN
        //•http://ident.me  OK
        //•http://bot.whatismyipaddress.com  //OK

        /// </summary>
    //    public const string URL_IpiFy = "https://api.ipify.org";
        //      public const string URL_IpInfo = "http://IpInfo.io";
        //New list 2017-01-08
        // freegeoip.net 

        /// <summary>
        /// Returns IP Address from an URL. the web respose may be in different format. Use appropriate response parsers to extract the IP address.
        /// </summary>
        /// <param name="urlAddress_"></param>
        /// <returns></returns>
        /// 

        public IPAddress GetIPAddress() {
            IPAddress ip = new IPAddress(0);
            List<string> usedURLs = new List<string>();
            IPCheckURLsConfigInstanceElement currIPCheckURL = null;
            string msg;

            List<string> codes = Settings.IPCheckURLs.Select(x => x.Code).ToList<string>();
            int attemptNumber = 1;
            bool IsValid = false;
            // try {
            while (codes.Count > 0 && attemptNumber<=3) {
                try {
                    string currCode = codes.PickRandom();

                    currIPCheckURL = Settings.IPCheckURLs.Where(x => x.Code == currCode).FirstOrDefault();
                    currIPCheckURL.NumberUsed++;

                    if (true) {
                        ip = GetIPAddress(currIPCheckURL, ref IsValid);
                        currIPCheckURL.NumberFailed += IsValid ? 0 : 1;  //Add 1 if failed;

                    } else { //test call
                        int success = Settings.randomZeroOne.Next(0, 2);
                        IsValid = (success == 1);
                        currIPCheckURL.NumberFailed += ((currIPCheckURL.TestFailureCorrectionFactor == 0 ? 1.0 : currIPCheckURL.TestFailureCorrectionFactor) * (IsValid ? 0 : 1));  //Simulate success/failure by getting rnd  0 or 1;
                    }
                    currIPCheckURL.CalculateSuccessPercent();
                    if (IsValid) {
                        break; // Succcess 
                    } else {//failure. Try next code if any remained
                        Settings.LogEntriesAdd(string.Format("Attempt # {0}.Failed Code:{1}", attemptNumber, currIPCheckURL.Code));
                        codes = codes.Where(x => x != currCode).ToList<string>(); // Remove the currCode and try next random code
                        Thread.Sleep(500); //sleep .5 sec before the next attempt
                    }

                } catch (Exception ex) {
                    //if (codes.Count == 0) {
                    //    Utilities.KillProgram(Settings.appToKillArray); // Kill the app only if there are no more codes left, otherwise log an error and continue trying with remaining codes.
                    //}
                    Settings.LogEntriesAdd("GetIPAddress failed" + ex.Message);
                    Logger.logger.Error(string.Format("GetIPAddress failed. Failed Code:{0}",  currIPCheckURL.Code), ex);

                }

                attemptNumber++;
            }//while codes

            if (!IsValid) {
                Utilities.KillProgram(Settings.appToKillArray); // No more codes left to try. Kill the apps

                msg = string.Format("{0} ALL FAILED after {1} attempts", Utilities.GetDateTime(), attemptNumber - 1);
                Settings.LogEntriesAdd(msg);
                Logger.logger.Error(msg);
            }
            //} catch (Exception ex) {
            //    if (codes.Count == 0) {
            //        Utilities.KillProgram(Settings.appToKillArray); // Kill the app only if there are no more codes left, otherwise log an error and continue trying with remaining codes.
            //    }
            //    Settings.LogEntriesAdd("GetIPAddress failed" + ex.Message);
            //    Logger.logger.Error("GetIPAddress failed", ex);

            //}
            return ip;
        }


        public IPAddress GetIPAddress(IPCheckURLsConfigInstanceElement ipCheckURL, ref bool IsValid) {

            IPAddress ip = new IPAddress(0);
            try {
                Uri uri = new Uri(ipCheckURL.Url);

                using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {

                    //  client.GetWebRequest(uri);


                    using (Stream stream = webClient.OpenRead(uri)) {
                        if (stream != null) {
                            using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
                                string htmlCode;
                                while ((htmlCode = reader.ReadLine()) != null) {
                                    if (htmlCode != String.Empty) {
                                        ip = IPAddress.Parse(htmlCode);
                                        IsValid = true;
                                        return ip;
                                    }

                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                IsValid = false;
                //   Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset
                Logger.logger.Error(ipCheckURL.Code, ex);
            }

            return ip;


        }



        //public IPAddress GetIpifyIPAddress(string urlAddress_) {
        //    IPAddress ip = new IPAddress(0);
        //    try {
        //        Uri uri = new Uri(urlAddress_);

        //        using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {

        //            //  client.GetWebRequest(uri);


        //            using (Stream stream = webClient.OpenRead(uri)) {
        //                if (stream != null) {
        //                    using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
        //                        string htmlCode;
        //                        while ((htmlCode = reader.ReadLine()) != null) {
        //                            if (htmlCode != String.Empty) {
        //                                ip = IPAddress.Parse(htmlCode);
        //                                return ip;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    } catch (Exception ex) {
        //        Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset
        //        Logger.logger.Error("GetIpifyIPAddress", ex);
        //    }

        //    return ip;


        //}



        [Obsolete("Not in use")]
        public void GetIP() {

            var externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                                .Matches(externalIP)[0].ToString();
            WPFMessage.msg = string.Format("{0}   at:{1}", externalIP, DateTime.Now.ToLocalTime());
            //   return externalIP;
        }

        [Obsolete("Not in use")]
        public string GetIPString() {
            string externalIP = "ERROR";
            try {
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                                    .Matches(externalIP)[0].ToString();
                WPFMessage.msg = string.Format("{0}   at:{1}", externalIP, DateTime.Now.ToLocalTime());
            } catch (Exception ex) {
                externalIP += " " + ex.Message;
            }
            return externalIP;
        }

        [Obsolete("Not in use")]
        public static class WPFMessage {
            public static bool isWorkerRunning;
            public static string msg { get; set; }

        }


    }

    public class WebClientCustom : WebClient {
        public int timeout { get; set; }
        protected override WebRequest GetWebRequest(Uri uri) {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = timeout;// 20 * 60 * 1000;
            return w;
        }
    }
    //public class IPAddressCustom : IPAddress
    //{
    //    //public IPAddressCustom() : base(0) { }

    //    public IPAddressCustom(byte[] address) : base(address)
    //    {
    //        // must override this constructor to enable build //    
    //    }

    //    public IPAddressCustom(Int64 address) : base(address)
    //    {
    //        // must override this constructor to enable
    //        // build / compatibility with base class constructors
    //    }

    //    public IPAddressCustom(byte[] address, Int64 scopeid) : base(address, scopeid)
    //    {
    //        // must override this constructor to enable
    //        // build / compatibility with base class constructors
    //    }

    //    public bool IsValid { get; set; }// Set the value in the IP checker from URL

    //}

}