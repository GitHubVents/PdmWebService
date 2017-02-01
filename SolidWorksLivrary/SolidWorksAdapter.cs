using Patterns;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace SolidWorksLibrary
{
    public abstract   class SolidWorksAdapter  
    { 
        /// <summary>
        /// SolidWorks exemplare
        /// </summary>
        private static SldWorks sldWoks_app;
 
        /// <summary>
        /// Get SolidWorksExemplare
        /// </summary>
        public static SldWorks SldWoksApp
        {
            get
            {
                InitSolidWorks();
                return  sldWoks_app;
            }
        }
        /// <summary>
        /// Initialize SolidWorks exemplare
        /// </summary>
        private static void InitSolidWorks()
        {
            if (sldWoks_app == null)
            {
                MessageObserver.Instance.SetMessage("Initialize SolidWorks exemplare");
            try
            {
              
                sldWoks_app = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                MessageObserver.Instance.SetMessage("\t\tTake an existing exemplar SolidWorks Application", MessageType.System);
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("\t\tFailed take an existing exemplar SolidWorks Application " + ex, MessageType.Warning);
                
                    Process[] processes = Process.GetProcessesByName("SLDWORKS");
                    int processesLength = processes.Length;
                    if (processesLength > 0)
                    { 
                        foreach (var process in processes)
                        {
                            process.Kill();
                        }
                    }                    
                    sldWoks_app = new SldWorks() { Visible = true };
                    MessageObserver.Instance.SetMessage("\t\tCreate exemplar SolidWorks Application", MessageType.System);
                }
            }
        }

        /// <summary>
        /// Closing all opened documents
        /// </summary>
        public static void CloseAllDocuments()
        {
            try
            {
                var modelDocs = SldWoksApp.GetDocuments();
                foreach (var eachModelDoc in modelDocs)
                {
                    eachModelDoc.Close();
                }
                MessageObserver.Instance.SetMessage("\t\tClosed all opened documents", MessageType.System);
            }
            catch(Exception ex)
            {
                MessageObserver.Instance.SetMessage("\t\tFailed close documents " + ex, MessageType.Error);
            }
        }

        /// <summary>
        /// Closing opened document
        /// </summary>
        /// <param name="swModel"></param>
        public static void CloseDocument(IModelDoc2 swModel)
        {

            //swModel.Close();
            // sldWoks_app.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt"); ????????

            try
            {
                swModel.Close();
                MessageObserver.Instance.SetMessage("Closed opened document " + swModel.GetTitle(), MessageType.Success);
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Failed close document " + swModel.GetTitle() + "\t" + ex, MessageType.Error);
            }
        }

        /// <summary>
        /// Closing all opened documents and exist from SolidWorks Application
        /// </summary>
        public static void CloseAllDocumentsAndExit()
        {

            CloseAllDocuments();
            try
            {
                MessageObserver.Instance.SetMessage("Exit from  SolidWorks Application" , MessageType.Success);
                SldWoksApp.ExitApp();
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Failed exit from  SolidWorks Application", MessageType.Success);
            } 
        }       

        /// <summary>
        /// Check is sheet metal part
        /// </summary>
        /// <param name="swPart"></param>
        /// <returns></returns>
        public static bool IsSheetMetalPart(IPartDoc swPart)
        {
            var isSheetMetal = false;
            try
            {
                var bodies = swPart.GetBodies2((int)swBodyType_e.swSolidBody, false);
                foreach (Body2 body in bodies)
                {
                    isSheetMetal = body.IsSheetMetal();
                    if (isSheetMetal)
                    {
                        MessageObserver.Instance.SetMessage("Check is sheet metal part; returned " + isSheetMetal, MessageType.Success);
                        return true;
                    }
                }               
            }
            catch (Exception)
            {
                MessageObserver.Instance.SetMessage("Failed check is sheet metal part; returned " + false, MessageType.Success);
                return false;
            }
            return isSheetMetal;
        }
    }
}
