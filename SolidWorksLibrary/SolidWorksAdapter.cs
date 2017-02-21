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
                SldWoksApp.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt");
                MessageObserver.Instance.SetMessage("Closed opened document " + swModel.GetTitle(), MessageType.Success);
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Failed close document " + swModel.GetTitle() + "\t" + ex, MessageType.Warning);
            }
        }

        /// <summary>
        /// Closing all opened documents and exist from SolidWorks Application
        /// </summary>
        public static void CloseAllDocumentsAndExit()
        {

            try
            {               
               // CloseAllDocuments();
                SldWoksApp.ExitApp();
                MessageObserver.Instance.SetMessage("Exit from  SolidWorks Application", MessageType.System);
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Failed exit from  SolidWorks Application", MessageType.Warning);
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



        public static ModelDoc2 OpenDocument(string path, swDocumentTypes_e documentType, string configuration = "00")
        {
            int errors = 0, warnings = 0;

            int openDocOptions = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
            if (documentType == swDocumentTypes_e.swDocDRAWING) {
                openDocOptions += (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel;
            }

            var swDocument = SolidWorksAdapter.SldWoksApp.OpenDoc6(path, (int)documentType, openDocOptions, configuration, errors, warnings);
            SolidWorksAdapter.SldWoksApp.Visible = true;

            if (errors != 0)
            {
                MessageObserver.Instance.SetMessage("Error at open solid works document: code {" + errors + "}, description error {" + (swFileLoadError_e)errors + "}");
            }
            if (warnings != 0)
            {
                MessageObserver.Instance.SetMessage("Warning at open solid works document: code {" + warnings + "}, description warning {" + (swFileLoadWarning_e)errors + "}");
            }


            return swDocument;
        }

        public static ModelDoc2 AcativeteDoc(string docTitle)
        {

            int errors = 0;
        


            if (errors != 0)
            {
                MessageObserver.Instance.SetMessage("Exeption at activate solid works document: code {" + errors+"}, description error {" + (swActivateDocError_e) errors + "}");
            }

            return  SolidWorksAdapter.SldWoksApp.ActivateDoc3(docTitle, true, (int) swOpenDocOptions_e.swOpenDocOptions_Silent, errors);
        }
    }
}
