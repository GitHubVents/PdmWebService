using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;

namespace PDMWebService.Data.Solid
{
    public   class SolidAdapter : Singleton.AbstractSingeton<SolidAdapter>
    {


        private SldWorks sldWoksApp;
        private SolidAdapter():base()
        {
            this.InitSolidWorks();
        }

        public void InitSolidWorks()
        {
            if (sldWoksApp == null)
            {
                Process[] processes = Process.GetProcessesByName("SLDWORKS");
                foreach (var process in processes)
                {
                    process.Kill();
                }

                sldWoksApp = new SldWorks() { Visible = true };

                try
                {
                    System.Console.WriteLine("Солид запущен значение " + sldWoksApp.Visible);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Солид не запущен значение " + ex.Message);
                }
            }
        }

    
        public string ConvertToDXF (string path,string configuration)
        {
            Exception ex;
            Console.WriteLine("Convert dile " + path);
            ExportPartData.Dxf.Save(path, configuration, out ex, true, true, @"D:\TEMP\dxf\", true);
            return null;
        }
    }
}
