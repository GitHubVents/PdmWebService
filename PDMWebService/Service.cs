using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Timers;

namespace PDMWebService
{
  public  class Service
    { 
       
        // private string pathToFolder = @"D:\temp".ToUpper();
        private Uri defaultServiceAddress = new Uri("http://localhost:8080");
        private Uri serviceAddress;
        private ServiceHost host;
       
        private Service(Uri serviceAddress)
        {             
            this.serviceAddress = serviceAddress;
         
         
        }
        
        private Service()
        {            
            this.serviceAddress = defaultServiceAddress;
          //  this.host.OpenTimeout = TimeSpan.FromMinutes(3);
          //  this.host.CloseTimeout = TimeSpan.FromMinutes(3);

        }                 

        public static Service CreateService(Uri serviceAddress)
        {
            return new Service(serviceAddress);
            
        }
        public static Service CreateService()
        {
            return new Service();
        }

        public void Start()
        {            
            try
            {
                Build();
                host.Open();
                Logger.ToLog("Служба запущена" );
                host.Description.Behaviors.Add(new ServiceDebugBehavior());
            }
            catch(Exception ex)
            {
               Logger.ToLog( "Неудалось запустить веб службу" + ex);
            }
        }

        public void Stop()
        {
            try
            {
                host.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Неудалось остановить веб службу\n\n" + ex);
            }
        }

        private void Build ()
        {
            if (host == null)
            {
                try
                {
                    host = new ServiceHost(typeof(ServiceInterfaice), serviceAddress);
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    host.Description.Behaviors.Add(smb);                   
                }
                catch (Exception ex)
                {
                    throw new Exception("Неудалось инициализировать веб службу\n\n" + ex);
                }
            }        
        }

     
    }
}
