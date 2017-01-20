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
                
            }
            catch(Exception exception)
            {
                Console.WriteLine("Неудалось запустить веб службу" + exception);
               Logger.ToLog( "Неудалось запустить веб службу" + exception);
            }
        }

        public void Stop()
        {
            try
            {
                host.Close();
            }
            catch (Exception exception)
            {
                throw new Exception("Неудалось остановить веб службу\n\n" + exception);
            }
        }

        private void Build ()
        {
            if (host == null)
            {
                try
                {

                    host = new ServiceHost(typeof(ServiceInterfaice), serviceAddress);
                    foreach (var item in host.Description.Behaviors)
                    {
                        try
                        {
                            (item as ServiceDebugBehavior).IncludeExceptionDetailInFaults = true;
                          
                             
                        }
                        catch {   }
                    }

                    try
                    {
                        host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                         
                    }
                    catch {
                    }
                    try
                    {
                        ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                        smb.HttpGetEnabled = true;
                        smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                        smb.HttpGetBinding.CloseTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpGetBinding.OpenTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpGetBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpGetBinding.SendTimeout = new TimeSpan(0, 10, 0);


                        smb.HttpsGetBinding.CloseTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpsGetBinding.OpenTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpsGetBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        smb.HttpsGetBinding.SendTimeout = new TimeSpan(0, 10, 0);

                        host.Description.Behaviors.Add(smb);
                        Console.WriteLine("smb Да");
                    }
                    catch
                    {
                        Console.WriteLine("smb нет");
                    }
                

                    //host = new ServiceHost(typeof(ServiceInterfaice), serviceAddress);
                    //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

                    //smb.HttpGetBinding = new NetHttpBinding();
                    //smb.HttpsGetBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                    //smb.HttpsGetBinding.OpenTimeout = new TimeSpan(0, 10, 0);
                    //smb.HttpsGetBinding.SendTimeout = new TimeSpan(0, 10, 0);
                    //smb.HttpsGetBinding.CloseTimeout = new TimeSpan(0, 10, 0);

                    //smb.HttpGetEnabled = true;
                    //smb.HttpsGetEnabled = true;
                    //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    //host.Description.Behaviors.Add(smb);    


                }
                catch (Exception exception)
                {
                    Console.WriteLine("Неудалось инициализировать веб службу\n\n" + exception);
                    throw new Exception("Неудалось инициализировать веб службу\n\n" + exception);
                }
            }        
        }

     
    }
}
