using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PDMWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8080;
            Console.WriteLine("http://" + LocalIPAddress().ToString() + ":"+port);
                Service service = Service.CreateService(new Uri("http://" + LocalIPAddress().ToString() + ":"+port));
                service.Start();

          
            
            Console.ReadLine();
        }
        private static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }


     
        //private static Comands ConsoleInterface ()
        //{

        //}

        //enum Comands { ServiceConfiguration, StartService, StopService /*other*/  }

    }
}
