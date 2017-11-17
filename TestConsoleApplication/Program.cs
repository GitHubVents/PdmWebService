//using Patterns;
using PDMWebService.Data.Solid.ElementsCase;
//using PDMWebService.Data.Solid.PartBuilders;
using SolidWorksLibrary.Builders.Dxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test//ConsoleApplication
{
    class Program 
    {
        static void Main(string[] args)
        {
            //Patterns.Observer.MessageObserver.Instance.ReceivedMessage += Instance_ReceivedMessage;
            //DxfBulder.Instance.FinishedBuilding += Instance_FinishedBuilding;
            //DxfBulder.Instance.Build(@"C:\Users\Antonyk\Desktop\test documents\ВНС-900.00.9001.SLDPRT", 33, 1);

            SpigotBuilder sp = new SpigotBuilder();

            string s = sp.Build(ServiceTypes.Constants.SpigotType_e.Thirty_mm, new SolidWorksLibrary.Builders.ElementsCase.Vector2( 300, 666));

            Console.WriteLine(s);
            Console.ReadLine();
        }

        //private static void Instance_FinishedBuilding(DataToExport dataToExport)
        //{
        //    //Console.WriteLine("good");
        //}

        //private static void Instance_ReceivedMessage(Patterns.Observer.MessageEventArgs massage)
        //{
        //    //Console.WriteLine(massage.Message);
        //}
    }
}
