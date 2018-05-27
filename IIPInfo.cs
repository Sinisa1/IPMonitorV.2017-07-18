using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
namespace IPMonitor {
    public abstract class IIPInfo {
        protected  string urlAddress { get; set; }
       public abstract IpInfoAttributes PopulateIpInfoAttributes();
        //   public abstract Dictionary<string, string> LocationToDictionary(IpInfoLocation location);
      //    public abstract Dictionary<string, string> LocationToDictionary(IpInfoLocation location);
        
        public virtual Dictionary<string, string> GetIpInfoDictionary() {
   //         IpInfoLocation location = new IpInfoLocation();
            Dictionary<string, string> values = new Dictionary<string, string>();
            try {
                using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {
                    using (Stream stream = webClient.OpenRead(urlAddress)) {
                        if (stream != null) {

                            using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
                                string jsonResponse = reader.ReadToEnd();
                                {
                                    if (jsonResponse != String.Empty) {

                                        values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Logger.logger.Error("GetIpInfo", ex);
            }
            return values;
        }
        public Dictionary<string, string> LocationToDictionary(IpInfoAttributes location) {

            Dictionary<string, string> outDict = new Dictionary<string, string>();
            outDict["ip"] = location.ip;
            outDict["hostname"] = location.hostname;
            outDict["city"] = location.city;
            outDict["region_name"] = location.region_name;
            outDict["country_name"] = location.country_name;
            outDict["location"] = location.location;
            outDict["organization"] = location.organization;
            outDict["postal_code"] = location.postal_code;

            return outDict;
        }
    }

  
    /// <summary>
    /// Generic container for the attributes returned from a query service
    /// </summary>
    public class IpInfoAttributes {
        public string ip_info_url;  // service used to get the IP info
        public string ip;
        public string hostname;
        public string country_code;
        public string region_code;
        public string country_name;
        public string region_name;
        public string city;
        public string postal_code;
        public string latitude;
        public string longitude;
        public string timezone;
        public string metro_code;
        public string isp;
        public string location;
        public string organization;
        public string as_string; // Check the actual description of the "as" field.
        public string status;
    }

    /// <summary>
    /// specific to IPINfo.io
    /// </summary>
    [Obsolete]
    public class IpInfoLocation {
        
        public string ip;
        public string hostname;
        public string country_code;
        public string region_code;
        public string country_name;
        public string region_name;
        public string city;
        public string postal_code;
        public string latitude;
        public string longitude;
        public string metro_code;
        public string isp;
        public string location;
        public string organization;
        public string as_string; // Check the actual description of the "as" field.
    }
}
