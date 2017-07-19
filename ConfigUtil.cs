using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://stackoverflow.com/questions/2718095/custom-app-config-section-with-a-simple-list-of-add-elements
using System.Configuration;

namespace IPMonitor
{

    public class IPCheckURLsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public IPCheckURLsConfigInstanceCollection Instances
        {
            get { return (IPCheckURLsConfigInstanceCollection)this[""]; }
            set { this[""] = value; }
        }
    }
    public class IPCheckURLsConfigInstanceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IPCheckURLsConfigInstanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((IPCheckURLsConfigInstanceElement)element).Code;
        }
    }

    public class IPCheckURLsConfigInstanceElement : ConfigurationElement
    {

        // <add code="IPIFY" url="https://api.ipify.org" parser="IP_STRING" />

        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get { return (string)base["code"]; }
            set { base["code"] = value; }
        }
        [ConfigurationProperty("url", IsKey = true, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
        [ConfigurationProperty("parser", IsKey = true, IsRequired = true)]
        public string Parser
        {
            get { return (string)base["parser"]; }
            set { base["parser"] = value; }
        }

    }
}

