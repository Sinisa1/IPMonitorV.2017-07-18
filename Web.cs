using System;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
namespace IPMonitor
{
    public class Web
    {
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
        public const string URL_IpiFy = "https://api.ipify.org";
        //      public const string URL_IpInfo = "http://IpInfo.io";
        //New list 2017-01-08
        // freegeoip.net 

            /// <summary>
            /// Returns IP Address from an URL. the web respose may be in different format. Use appropriate response parsers to extract the IP address.
            /// </summary>
            /// <param name="urlAddress_"></param>
            /// <returns></returns>
        public IPAddress GetIPAddress(IPCheckURLsConfigInstanceElement ipCheckURL)
         
        {

            IPAddress ip = new IPAddress(0);
            try
            {
                Uri uri = new Uri(ipCheckURL.Url);

                using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval })
                {

                    //  client.GetWebRequest(uri);


                    using (Stream stream = webClient.OpenRead(uri))
                    {
                        if (stream != null)
                        {
                            using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false))
                            {
                                string htmlCode;
                                while ((htmlCode = reader.ReadLine()) != null)
                                {
                                    if (htmlCode != String.Empty)
                                    {
                                        ip = IPAddress.Parse(htmlCode);
                                        return ip;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset
                Logger.logger.Error(ipCheckURL.Code, ex);
            }

            return ip;


        }



        public IPAddress GetIpifyIPAddress(string urlAddress_)
        {
            IPAddress ip = new IPAddress(0);
            try
            {
                Uri uri = new Uri(urlAddress_);

                using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval } )
                {

                    //  client.GetWebRequest(uri);


                    using (Stream stream = webClient.OpenRead(uri))
                    {
                        if (stream != null)
                        {
                            using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false))
                            {
                                string htmlCode;
                                while ((htmlCode = reader.ReadLine()) != null)
                                {
                                    if (htmlCode != String.Empty)
                                    {
                                        ip = IPAddress.Parse(htmlCode);
                                        return ip;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset
                Logger.logger.Error("GetIpifyIPAddress", ex);
            }

            return ip;


        }

   

        [Obsolete("Not in use")]
        public void GetIP()
        {

            var externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                                .Matches(externalIP)[0].ToString();
            WPFMessage.msg = string.Format("{0}   at:{1}", externalIP, DateTime.Now.ToLocalTime());
            //   return externalIP;
        }

        [Obsolete("Not in use")]
        public string GetIPString()
        {
            string externalIP = "ERROR";
            try
            {
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                                    .Matches(externalIP)[0].ToString();
                WPFMessage.msg = string.Format("{0}   at:{1}", externalIP, DateTime.Now.ToLocalTime());
            }
            catch (Exception ex)
            {
                externalIP += " " + ex.Message;
            }
            return externalIP;
        }

        [Obsolete("Not in use")]
        public static class WPFMessage
        {
            public static bool isWorkerRunning;
            public static string msg { get; set; }

        }


    }

    public class WebClientCustom : WebClient
    {
        public int timeout { get; set; }
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = timeout;// 20 * 60 * 1000;
            return w;
        }
    }

}