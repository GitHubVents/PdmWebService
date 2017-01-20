using EPDM.Interop.epdm;
using PDM_WebService.WcfServiceLibrary.DataContracts;
using PDMWebService.Data.PDM;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.Linq;
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
            Console.WriteLine("InitSolidWorks");
            try
            {
                //   sldWoksApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");

                //test
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
          
          var modelDocs  = sldWoksApp.GetDocuments()  ;
            foreach (var eachModelDoc in modelDocs)
            {
                eachModelDoc.Close();
            }
        }
      public  static void CloseDocument(IModelDoc2 swModel )
        {
            sldWoksApp.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt");
        }
       
        public static void CloseAllDocumentsAndExit()
        {
            CloseAllDocuments();
            SldWoksApp.ExitApp();
        }




       

        public static bool IsSheetMetalPart(IPartDoc swPart)
        {
            var isSheetMetal = false;
            try
            {
                var vBodies = swPart.GetBodies2((int)swBodyType_e.swSolidBody, false);

                foreach (Body2 vBody in vBodies)
                {
                    isSheetMetal = vBody.IsSheetMetal();
                    if (isSheetMetal)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return isSheetMetal;
        }

    }
}
