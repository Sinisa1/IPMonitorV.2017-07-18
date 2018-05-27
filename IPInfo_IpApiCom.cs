using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace IPMonitor {
    public class IPInfo_IpApiCom : IIPInfo {
           

        public IPInfo_IpApiCom() {
            urlAddress = "http://ip-api.com/json";
        }

        public override IpInfoAttributes PopulateIpInfoAttributes() {
            IpInfoAttributes attributes = new IpInfoAttributes();

          Dictionary <string,string> inputDict = GetIpInfoDictionary();
            attributes.ip_info_url = urlAddress;
            attributes.hostname = urlAddress;
            attributes.ip = urlAddress;
            attributes.isp = urlAddress;
            attributes.latitude = urlAddress;
            attributes.location = urlAddress;
            attributes.longitude = urlAddress;
            attributes.metro_code = urlAddress;
            attributes.organization = urlAddress;
            attributes.postal_code = urlAddress;
            attributes.region_code = urlAddress;
            attributes.region_name = urlAddress;
            attributes.timezone = urlAddress;
            attributes.as_string = urlAddress;
            attributes.city = urlAddress;
            attributes.country_code = urlAddress;
            attributes.country_name = urlAddress;
   


            return attributes;
        }
        //public override Dictionary<string, string> GetIpInfoDictionary() {
        //    IpInfoLocation location = new IpInfoLocation();
        //    Dictionary<string, string> values = new Dictionary<string, string>();
        //    try {
        //        using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {
        //            using (Stream stream = webClient.OpenRead(urlAddress)) {
        //                if (stream != null) {

        //                    using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
        //                        string jsonResponse = reader.ReadToEnd();
        //                        {
        //                            if (jsonResponse != String.Empty) {

        //                                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    } catch (Exception ex) {
        //        Logger.logger.Error("GetIpInfo", ex);
        //    }
        //    return values;
        //}


        //public override Dictionary<string, string> LocationToDictionary(IpInfoLocation location) {

        //    Dictionary<string, string> outDict = new Dictionary<string, string>();
        //    outDict["ip"] = location.ip;
        //    outDict["hostname"] = location.hostname;
        //    outDict["city"] = location.city;
        //    outDict["region_name"] = location.region_name;
        //    outDict["country_name"] = location.country_name;
        //    outDict["location"] = location.location;
        //    outDict["organization"] = location.organization;
        //    outDict["postal_code"] = location.postal_code;

        //    return outDict;
        //}
    }

}
