using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
namespace IPMonitor {
    public class IPInfo_IpInfoIO : IIPInfo {
           

        public IPInfo_IpInfoIO() {
            urlAddress = "http://IpInfo.io";
        }


        public override IpInfoAttributes PopulateIpInfoAttributes() {
            IpInfoAttributes attributes = new IpInfoAttributes();


            return attributes;
        }


        //public override IpInfoLocation GetIpInfo() {
        //    IpInfoLocation location = new IpInfoLocation();
        //    try {
        //        using (WebClientCustom webClient = new WebClientCustom() { timeout = Settings.TimeoutInterval }) {
        //            using (Stream stream = webClient.OpenRead(urlAddress)) {
        //                if (stream != null) {

        //                    using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, false)) {
        //                        string htmlCode = reader.ReadToEnd();
        //                        {
        //                            if (htmlCode != String.Empty) {
        //                                location = JsonConvert.DeserializeObject<IpInfoLocation>(htmlCode);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    } catch (Exception ex) {
        //        Logger.logger.Error("GetIpInfo", ex);
        //    }
        //    return location;
        //}



    }

}
