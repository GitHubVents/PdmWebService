using ServiceLibrary.ConcreteService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleServiceHost
{
    class Program
    {
        public static void startHost ()
        {

            using (System.ServiceModel.ServiceHost host = new
                System.ServiceModel.ServiceHost(typeof(VetsManagingWebService)))
            {

                host.Open();
                Console.WriteLine("Host started @ " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
        static void Main(string[] args)
        {
         //   string port = "8080";
          //  ServiceManager.Instance.BuildHost(new Uri("http://" + "192.168.14.43" + ":" + port));
          //  ServiceManager.Instance.StartHost();
            try
            {
                Thread thread = new Thread(startHost);
                thread.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                var v = Process.GetProcesses("ConsoleServiceHost.exe");

                foreach (var item in v)
                {
                    item.Kill();
                }
            }
            Thread.Sleep(500);
            Console.WriteLine("Press any key to exit");
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
    }
}
