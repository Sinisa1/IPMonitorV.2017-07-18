using System.Net;
namespace IPMonitor
{
    public class Timer
    {

        public IPAddress referenceIP = new IPAddress(0);
        public IPAddress currentIP  {get;set;}       //new IPAddress(0);
        public bool killActive = false;

        public string currentIPmsg;
        public Web web = new Web();
        public IpInfoIO ipInfo = new IpInfoIO();

        public string ResetIpString()
        {

   //         initialIP = web.GetIpifyIPAddress(Web.URL_IpiFy);
            referenceIP = web.GetIPAddress();

            killActive = false;
            Logger.logger.InfoFormat("IP Manually reset to current IP {0}", referenceIP);
            return referenceIP.ToString();
        }

        public void SetReferenceIP(string inIp)
        {
            IPAddress.TryParse(inIp, out referenceIP);

        }
        public void SetCurrentlIP(IPAddress inIp)
        {
            currentIP = inIp;

        }

    }
}