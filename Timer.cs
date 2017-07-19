using System.Net;
namespace IPMonitor
{
    public class Timer
    {

        public IPAddress initialIP = new IPAddress(0);
        public IPAddress currentIP  {get;set;}       //new IPAddress(0);
        public bool killActive = false;
        public string currentIPmsg;
        public Web web = new Web();
        public IpInfoIO ipInfo = new IpInfoIO();

        public string ResetIpString()
        {

            initialIP = web.GetIpifyIPAddress(Web.URL_IpiFy);

            killActive = false;
            Logger.logger.InfoFormat("IP Manually reset to current IP {0}", initialIP);
            return initialIP.ToString();
        }

        public void SetInitalIP(string inIp)
        {
            IPAddress.TryParse(inIp, out initialIP);

        }
        public void SetCurrentlIP(IPAddress inIp)
        {
            currentIP = inIp;

        }

    }
}