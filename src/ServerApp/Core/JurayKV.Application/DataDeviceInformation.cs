using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace JurayKV.Application
{
    public class DataDeviceInformation
    {
        public string GetMacAddress()
        {
            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var firstInterface = networkInterfaces?.FirstOrDefault();
                return firstInterface?.GetPhysicalAddress()?.ToString() ?? "MAC address not available";
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return "Error getting MAC address: " + ex.Message;
            }
        }

        public string GetIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddress = host?.AddressList?.FirstOrDefault()?.ToString();
                return ipAddress ?? "IP address not available";
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return "Error getting IP address: " + ex.Message;
            }
        }

    }
}
