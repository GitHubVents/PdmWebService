using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PDMWebService.Data.Solid
{
    public abstract   class SolidWorksInstance  
    { 
        private static SldWorks sldWoksApp;
        public static SldWorks SldWoksApp
        {
            get
            {
                InitSolidWorks();
                return  sldWoksApp;
            }
        }

        private static void InitSolidWorks()
        {
            try
            {
                sldWoksApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }
            catch
            {
                if (sldWoksApp == null)
                {
                    Process[] processes = Process.GetProcessesByName("SLDWORKS");
                    foreach (var process in processes)
                    {
                        process.Kill();
                    }

                    sldWoksApp = new SldWorks() { Visible = true };

                }
            }
        }

        public static void CloseAllDocuments ()
        {
           ModelDoc [] modelDocs  = sldWoksApp.GetDocuments() as ModelDoc[];
            foreach (var eachModelDoc in modelDocs)
            {
                eachModelDoc.Close();
            }
        }


        public static void CloseAllDocumentsAndExit()
        {
            CloseAllDocuments();
            sldWoksApp.ExitApp();
        }




        public static string ConvertToDXF(string path, string configuration)
        {
            Exception ex;
            Console.WriteLine("Convert dile " + path);
            ExportPartData.Dxf.Save(path, configuration, out ex, true, true, @"D:\TEMP\dxf\", true);
            return null;
        }
    }
}
