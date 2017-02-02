using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebService.TestDataBaseDomian;

namespace TestWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_TaskSystemRepository.CreateDxf_TestMethod();
            Test_TaskSystemRepository.CreatePdf_TestMethod();
            Console.ReadKey();
        }
    }
}
