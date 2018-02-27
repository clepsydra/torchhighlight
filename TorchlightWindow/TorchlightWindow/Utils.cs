using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorchlightWindow
{
    using System.Net.NetworkInformation;

    class Utils
    {
        public static int FindAvailablePort()
        {
            int startingPort = 8001;

            var portArray = new List<int>();

            var properties = IPGlobalProperties.GetIPGlobalProperties();

            // Ignore active connections
            var connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                where n.LocalEndPoint.Port >= startingPort
                select n.LocalEndPoint.Port);

            // Ignore active tcp listners
            var endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                where n.Port >= startingPort
                select n.Port);

            // Ignore active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                where n.Port >= startingPort
                select n.Port);

            portArray.Sort();

            for (var port = startingPort; port < UInt16.MaxValue; port++)
            {
                if (!portArray.Contains(port))
                {
                    return port;
                }
            }
            
            throw new Exception("Unable to find free port");
        }
    }
}
