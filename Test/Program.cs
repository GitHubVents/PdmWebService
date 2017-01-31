using Patterns.Observer;
using SolidWorksLibrary.Builders.Dxf;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageObserver.Instance.ReceivedMessage += Instance_ReceivedMessage;


            DxfBulder.Instance.Build(@"D:\Vents-PDM\Проекты\Промышленная вентиляция\ВНС-47.00.420 - Патрубок металлический ПМ\ВНС-47.00.420.SLDPRT", 0, 4);

            Console.ReadLine();

        }

        private static void Instance_ReceivedMessage(MessageEventArgs message)
        {
            Console.WriteLine(message.Message + " type " + message.Type);
        }
    }
}
