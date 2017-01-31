using MessageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MesageObserver.Instance.ReceivedMessage += Instance_ReceivedMessage;
            var v = SolidWorksLibrary.SolidWorksAdapter.SldWoksApp;

            Console.ReadLine();

        }

        private static void Instance_ReceivedMessage(string massage)
        {
            Console.WriteLine(massage);
        }
    }
}
