using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace IPMonitor {
    public class IpInfoIO : IIPInfo {
           

        public IpInfoIO() {
            urlAddress = "http://IpInfo.io";
        }
        public override IpInfoLocation GetIpInfo() {
            IpInfoLocation location = new IpInfoLocation();
            try {
                using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {
                    using (Stream stream = webClient.OpenRead(urlAddress)) {
                        if (stream != null) {

                            using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
                                string htmlCode = reader.ReadToEnd();
                                {
                                    if (htmlCode != String.Empty) {
                                        location = JsonConvert.DeserializeObject<IpInfoLocation>(htmlCode);
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Utilities.KillProgram(Settings.appToKillArray); // Continue killing until reset
                Logger.logger.Error("GetIpInfo", ex);

            }

            return location;
        }


        public override Dictionary<string, string> LocationToDictionary(IpInfoLocation location) {

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

}
