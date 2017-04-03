using Patterns;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;


namespace SolidWorksLibrary
{
    public abstract   class SolidWorksAdapter  
    { 
        /// <summary>
        /// SolidWorks exemplare
        /// </summary>
        private static SldWorks sldWoksApp;
 
        /// <summary>
        /// Get SolidWorksExemplare
        /// </summary>
        public static SldWorks SldWoksAppExemplare
        {
            get
            {
                InitSolidWorks();
                return  sldWoksApp;
            }
        }
        /// <summary>
        /// Initialize SolidWorks exemplare
        /// </summary>
        private static void InitSolidWorks()
        {
            if (sldWoksApp == null) {
                MessageObserver.Instance.SetMessage("Initialize SolidWorks exemplare");
                try {                    
                     sldWoksApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                    MessageObserver.Instance.SetMessage("\t\tTake an existing exemplar SolidWorks Application", MessageType.System);
                }

                catch (Exception ex) {
                    MessageObserver.Instance.SetMessage("\t\tFailed take an existing exemplar SolidWorks Application " + ex, MessageType.Warning);
                    Process[] processes = Process.GetProcessesByName("SLDWORKS");
                    int processesLength = processes.Length;
                    if (processesLength > 0) {
                        foreach (var process in processes) {
                            process.Kill();
                        }
                    }
                    sldWoksApp = new SldWorks( )  
                    // Allow SOLIDWORKS to run in the background
                    // and be invisible
                    //sldWoks_app.UserControl = false;
                    { Visible = true };
                    MessageObserver.Instance.SetMessage("\t\tCreated exemplar SolidWorks Application", MessageType.System);
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
                var modelDocs = SldWoksAppExemplare.GetDocuments();
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
                SldWoksAppExemplare.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt");
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
                SldWoksAppExemplare.ExitApp();
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
            if (!File.Exists(path))
            {
                MessageObserver.Instance.SetMessage($"Error at open solid works document {path} file not exists. Maybe it is virtual document", MessageType.Error);
                throw new Exception($"Error at open solid works document {path} file not exists. Maybe it is virtual document" );
            }
            int errors = 0, warnings = 0;
            int openDocOptions = (int)swOpenDocOptions_e.swOpenDocOptions_Silent;
            if (documentType == swDocumentTypes_e.swDocASSEMBLY) {
                openDocOptions += (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel;
            }
            var SolidWorksDocumentument = SolidWorksAdapter.SldWoksAppExemplare.OpenDoc6(path, (int)documentType, openDocOptions, configuration, ref errors, ref warnings);
           
            if (errors != 0)
            {
                MessageObserver.Instance.SetMessage($"Error at open solid works document {path}; error code {errors }, description error { (swFileLoadError_e)errors }" ) ;
                throw new Exception($"Failed open document {path};  error code {errors }, description error { (swFileLoadError_e)errors }");
            }
            if (warnings != 0)
            {
                MessageObserver.Instance.SetMessage("Warning at open solid works document: code {" + warnings + "}, description warning {" + (swFileLoadWarning_e)errors + "}");
            }
            return SolidWorksDocumentument;
        }

        public static ModelDoc2 AcativeteDoc(string docTitle)
        {
            int errors = 0;
            ModelDoc2 modelDoc = SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc3(docTitle, true, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, errors);
       
            if (errors != 0)
            {
                MessageObserver.Instance.SetMessage("Exeption at activate solid works document: code {" + errors+"}, description error {" + (swActivateDocError_e) errors + "}");
            }
            return modelDoc;
        }

        /// <summary>
        /// Convert  ModelDoc2 to AssemblyDoc and resolve all light weight components
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static AssemblyDoc  ToAssemblyDocument(ModelDoc2 document)
        {
            AssemblyDoc swAsm =  (AssemblyDoc)document;
            swAsm.ResolveAllLightWeightComponents(false);
            return swAsm;
        }
    }
}
