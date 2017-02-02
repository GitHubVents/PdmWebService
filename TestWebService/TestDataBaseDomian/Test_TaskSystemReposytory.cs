using DataBaseDomian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebService.TestDataBaseDomian
{
    public abstract class Test_TaskSystemRepository
    {
        public static void CreateDxf_TestMethod()
        {
            Console.WriteLine("Start: CreateDxf_TestMethod");
            var count_TaskBefore = TaskSystemDataRepository.Instance.CountTasks;
            int taskInstance_id = TaskSystemDataRepository.Instance.CreateDxfTask(new int[] { 137138 });
            var count_TaskAfter = TaskSystemDataRepository.Instance.CountTasks;


            if ((count_TaskAfter - count_TaskBefore) == 1 && taskInstance_id != 0)
                Console.WriteLine("Succsess: CreateDxf_TestMethod");
            else
                Console.WriteLine("Error: CreateDxf_TestMethod");
        }


        public static void CreatePdf_TestMethod()
        {
            Console.WriteLine("Start: Create_Pdf_TestMethod");
            var count_TaskBefore = TaskSystemDataRepository.Instance.CountTasks;
            int taskInstance_id = TaskSystemDataRepository.Instance.CretaPdfTask(new int[] { 137138 });
            var count_TaskAfter = TaskSystemDataRepository.Instance.CountTasks;


            if ((count_TaskAfter - count_TaskBefore) == 1 && taskInstance_id != 0)
                Console.WriteLine("Succsess: Create_Pdf_TestMethodd");
            else
                Console.WriteLine("Error: Create_Pdf_TestMethod");
        }

    }
}
