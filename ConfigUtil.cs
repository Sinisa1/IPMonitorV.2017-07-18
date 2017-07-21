using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://stackoverflow.com/questions/2718095/custom-app-config-section-with-a-simple-list-of-add-elements
using System.Configuration;

namespace IPMonitor {

    public class IPCheckURLsConfigSection : ConfigurationSection {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public IPCheckURLsConfigInstanceCollection Instances {
            get { return (IPCheckURLsConfigInstanceCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class IPCheckURLsConfigInstanceCollection : ConfigurationElementCollection {
        protected override ConfigurationElement CreateNewElement() {
            return new IPCheckURLsConfigInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            //set to whatever Element Property you want to use for a key
            return ((IPCheckURLsConfigInstanceElement)element).Code;
        }
    }

    public class IPCheckURLsConfigInstanceElement : ConfigurationElement {

        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        [ConfigurationProperty("code", IsKey = true, IsRequired = true)]
        public string Code {
            get { return (string)base["code"]; }
            set { base["code"] = value; }
        }
        [ConfigurationProperty("url", IsKey = false, IsRequired = true)]
        public string Url {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
        [ConfigurationProperty("parser", IsKey = false, IsRequired = true)]
        public string Parser {
            get { return (string)base["parser"]; }
            set { base["parser"] = value; }
        }

        [ConfigurationProperty("testFailureCorrectionFactor", IsKey = false, IsRequired = false)]
        public double TestFailureCorrectionFactor {
            get { return (double)base["testFailureCorrectionFactor"]; }
            set { base["testFailureCorrectionFactor"] = value==0.0?1.0:value; }
        }


        public double NumberUsed { get; set; } // Number of times used to check IP in the session
        public double NumberFailed { get; set; } // Number of times failed to return IP in the session
        private double ipCheckSuccessPercent;
        public double IPCheckSuccessPercent { // 100*(NumberUsed-NumberFailed)/NumberUsed as calculated value
            get { return ipCheckSuccessPercent; }
            private set { }
        }
        //public double TestFailureCorrectionFactor { get; set; } //  Set to value>1 to simulate higher failure rate.
        public void CalculateSuccessPercent() {
            ipCheckSuccessPercent = NumberUsed == 0 ? 100.0 : 100 * (NumberUsed - NumberFailed) / NumberUsed;
        }
    }
}

