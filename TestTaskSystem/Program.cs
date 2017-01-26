using PDM_WebService.WcfServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                 
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://192.168.14.43:8080");
            var myChannelFactory = new ChannelFactory<ISolidWebService>(myBinding, myEndpoint);

                myChannelFactory.Open();
        var    client = myChannelFactory.CreateChannel();


                 var dataList = client.GetTasksData();

                foreach (var item in dataList)
                {
                    Console.WriteLine("TaskData " + item.TaskId + " " + item.Status + " " + item.type);
                }


                // ServiceReference1.ISolidWebService sws = new ServiceReference1.SolidWebServiceClient();

                //  System.Threading.Thread.Sleep(5000);
                //Console.WriteLine(  sws.testStaticFielsdMethod());
                //Random rnd = new Random();
                //sws.CreateRoof(rnd.Next(100, 3000), rnd.Next(100, 3000), WebServiceExemplare.RoofTypes.One, rnd.Next(100, 3000));
                //sws.CreateVibroInsertion(rnd.Next(100, 3000), rnd.Next(100, 3000), WebServiceExemplare.VibroInsertionTypes.Thirty_mm, rnd.Next(100, 3000));


                //if (sws.testYesNotS(0) == 1)
                //    sws.testYesNotS(1);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
