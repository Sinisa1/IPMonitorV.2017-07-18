using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPMonitor {
    public abstract class IIPInfo {
        protected  string urlAddress { get; set; }
        public abstract IpInfoLocation GetIpInfo();
        public abstract Dictionary<string, string> LocationToDictionary(IpInfoLocation location);
    }

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
        public string location;
        public string organization;
    }
}
