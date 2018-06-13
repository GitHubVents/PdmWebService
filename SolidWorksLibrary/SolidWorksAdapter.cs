using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SolidWorksLibrary
{
    public static class SolidWorksAdapter
    {
        /// <summary>
        /// SolidWorks exemplare
        /// </summary>
        private static SldWorks swApp;


        public static void DisposeSOLID()
        {
            swApp = null;
        }

        /// <summary>
        /// Get SolidWorksExemplare
        /// </summary>
        public static SldWorks SldWoksAppExemplare
        {
            get
            {
                if (swApp == null)
                {
                    //swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                    InitializeSolidWorks();
                }
                return swApp;
            }
        }

        /// <summary>
        /// Initialize SolidWorks exemplare
        /// </summary>
        //private static void InitSolidWorks()
        //{
        //    try
        //    {
        //        sldWoksApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
        //    }
        //    catch (Exception)
        //    {
        //        sldWoksApp = new SldWorks { Visible = true };
        //    }
        //    if (sldWoksApp == null)
        //    {
        //        return;
        //    }



        //    previous

            //    if (sldWoksApp == null)
            //    {
            //        sldWoksApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            //        //    MessageObserver.Instance.SetMessage("\t\tTake an existing exemplar SolidWorks Application", MessageType.System);

            //        //    //MessageObserver.Instance.SetMessage("\t\tFailed take an existing exemplar SolidWorks Application " + ex, MessageType.Warning);
            //        //    //Process[] processes = Process.GetProcessesByName("SLDWORKS");
            //        //    //int processesLength = processes.Length;
            //        //    //if (processesLength > 0) {
            //        //    //    foreach (var process in processes) {
            //        //    //        process.Kill();
            //        //    //    }

            //        //sldWoksApp = new SldWorks() { Visible = true };
            //        //sldWoksApp.DocumentVisible(false, (int)swDocumentTypes_e.swDocPART + (int)swDocumentTypes_e.swDocASSEMBLY);
            //    }
            //}

        [DllImport("ole32.dll")]
        static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
        private static void InitializeSolidWorks()
        {
            string monikerName = "SolidWorks_PID_";
            object app;
            IBindCtx context = null;
            IRunningObjectTable rot = null;
            IEnumMoniker monikers = null;

            try
            {
                CreateBindCtx(0, out context);

                context.GetRunningObjectTable(out rot);
                rot.EnumRunning(out monikers);

                IMoniker[] moniker = new IMoniker[1];

                while (monikers.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    var curMoniker = moniker.First();

                    string name = null;

                    if (curMoniker != null)
                    {
                        try
                        {
                            curMoniker.GetDisplayName(context, null, out name);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            MessageObserver.Instance.SetMessage("Failed to get SolidWorks_PID." + "\t" + ex, MessageType.Error);
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                    if (name.Contains(monikerName))
                    {
                        rot.GetObject(curMoniker, out app);
                        swApp = (SldWorks)app;
                        swApp.Visible = true;
                        return;
                    }
                }
                string progId = "SldWorks.Application";

                Type progType = Type.GetTypeFromProgID(progId);
                app = Activator.CreateInstance(progType) as SldWorks;
                swApp = (SldWorks)app;
                swApp.Visible = true;
                return;
            }
            finally
            {
                if (monikers != null)
                {
                    Marshal.ReleaseComObject(monikers);
                }
                if (rot != null)
                {
                    Marshal.ReleaseComObject(rot);
                }
                if (context != null)
                {
                    Marshal.ReleaseComObject(context);
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
            
            try
            {
                SldWoksAppExemplare.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt");
                //MessageObserver.Instance.SetMessage("Closed opened document " + swModel.GetTitle(), MessageType.Success);
            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Failed close document " + swModel.GetTitle() + "\t" + ex, MessageType.Warning);
                //System.Windows.Forms.MessageBox.Show("Failed close document " + swModel.GetTitle());
            }
        }

        /// <summary>
        /// Closing all opened documents and exist from SolidWorks Application
        /// </summary>
        public static void CloseAllDocumentsAndExit()
        {
            try
            {               
                CloseAllDocuments();
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
        public static bool IsSheetMetalPart(IPartDoc swPart)
        {
            bool isSheetMetal = false;
            try
            {
                var bodies = swPart.GetBodies2((int)swBodyType_e.swSolidBody, false);
                foreach (Body2 body in bodies)
                {
                    isSheetMetal = body.IsSheetMetal();

                    MessageObserver.Instance.SetMessage("Check is sheet metal part; returned " + isSheetMetal, MessageType.Success);

                    if (isSheetMetal)
                    {
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

        //public static ModelDoc2 OpenDocument(string path, swDocumentTypes_e documentType, string configuration = "")
        //{
        //    if (!File.Exists(path))
        //    {
        //        MessageObserver.Instance.SetMessage($"Error at open solid works document {path} file not exists. Maybe it is virtual document", MessageType.Error);
        //        throw new Exception($"Error at open solid works document {path} file not exists. Maybe it is virtual document" );
        //    }
        //    int errors = 0, warnings = 0;
        //    int openDocOptions = (int)swOpenDocOptions_e.swOpenDocOptions_ReadOnly;
        //    if (documentType == swDocumentTypes_e.swDocASSEMBLY) {  openDocOptions += (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel; }

        //    var SolidWorksDocumentument = SldWoksAppExemplare.OpenDoc6(path, (int)documentType, openDocOptions, configuration, ref errors, ref warnings);

        //    if (errors != 0)
        //    {
        //        MessageObserver.Instance.SetMessage($"Error at open solid works document {path}; error code {errors }, description error { (swFileLoadError_e)errors }" ) ;
        //    }
        //    if (warnings != 0)
        //    {
        //        MessageObserver.Instance.SetMessage("Warning at open solid works document: code {" + warnings + "}, description warning {" + (swFileLoadWarning_e)errors + "}");
        //    }
        //    return SolidWorksDocumentument;
        //}

        public static ModelDoc2 OpenDocument(string path, swDocumentTypes_e documentType, string configuration = "")
        {
            if (!File.Exists(path))
            {
                MessageObserver.Instance.SetMessage($"Error at open solid works document {path} file not exists. Maybe it is virtual document", MessageType.Error);
                throw new Exception($"Error at open solid works document {path} file not exists. Maybe it is virtual document");
            }
            
            DocumentSpecification swDocSpecification;

            swDocSpecification = SolidWorksAdapter.SldWoksAppExemplare.GetOpenDocSpec(path);
            swDocSpecification.ReadOnly = false;
            swDocSpecification.DocumentType = (int)documentType;
            swDocSpecification.UseLightWeightDefault = false;
            swDocSpecification.LightWeight = false;
            swDocSpecification.Silent = true;
            swDocSpecification.IgnoreHiddenComponents = false;
            swDocSpecification.ViewOnly = false;
            swDocSpecification.InteractiveAdvancedOpen = false;
            swDocSpecification.ConfigurationName = configuration;

            ModelDoc2 SolidWorksDocumentument = null;

            if (swDocSpecification != null)
            {
                SolidWorksDocumentument = SldWoksAppExemplare.OpenDoc7(swDocSpecification);
                if (SolidWorksDocumentument == null)
                {
                    System.Windows.Forms.MessageBox.Show("Не удалось открыть документ " + path);
                    MessageObserver.Instance.SetMessage("Failed to open document " + path + System.Environment.NewLine + "Error : " + (swFileLoadError_e)swDocSpecification.Error + System.Environment.NewLine +
                                                    " Warning: " + (swFileLoadWarning_e)swDocSpecification.Warning);
                }
            }
            else
            {
                MessageObserver.Instance.SetMessage("Failed to get specification on " + path + System.Environment.NewLine + "Error : " + (swFileLoadError_e)swDocSpecification.Error + System.Environment.NewLine +
                                                   " Warning: " + (swFileLoadWarning_e)swDocSpecification.Warning);
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
        public static AssemblyDoc ToAssemblyDocument(ModelDoc2 document)
        {
            swComponentResolveStatus_e res = swComponentResolveStatus_e.swResolveOk;
            AssemblyDoc swAsm = null;
            if ((int)swDocumentTypes_e.swDocASSEMBLY == document.GetType())
            {
                swAsm = (AssemblyDoc)document;
                //res = (swComponentResolveStatus_e)swAsm.ResolveAllLightWeightComponents(false);
                //MessageObserver.Instance.SetMessage("Resolve All LightWeight Components: code {" + res + "}");
            }
            return swAsm;
        }

        public static DrawingDoc ToDrawingDoc(ModelDoc2 document)
        {
            DrawingDoc drw = (DrawingDoc)document;
            return drw;
        }

        public static int ToInt(this double value)
        {
            return Convert.ToInt32(value);
        }
    }
}