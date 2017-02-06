using Patterns;
using ServiceLibrary.ConcreteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServiceHost
{
    internal class ServiceManager : Singeton<ServiceManager>
    {
        protected ServiceManager()
        { 
        }
        private ServiceHost serviceHost { get; set; }
      

        public void BuildHost(Uri serviceAddress)
        {
            if (serviceHost == null)
            {
                try
                {
                    serviceHost = new ServiceHost(typeof(VentsManagingWebService),  serviceAddress); 
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Неудалось инициализировать веб службу\n\n" + exception);
                    throw new Exception("Неудалось инициализировать веб службу\n\n" + exception);
                }
            }
        }

        public void StartHost ()
        {
            
            serviceHost.Open();
            serviceHost.Closing += ServiceHost_Closing;
            serviceHost.Closed += ServiceHost_Closing;
        }

        private void ServiceHost_Closing(object sender, EventArgs e)
        {
            StartHost();
        }
    }
}
