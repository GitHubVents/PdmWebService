using Patterns;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using System.Linq;

namespace SolidWorksLibrary.Builders.Dxf
{
    /// <summary>
    /// DxfBuilder allows builds dxf views on based SolidWorks parts. 
    /// </summary>
    public class DxfBulder : Singeton<DxfBulder>
    {

        ModelDoc2 modelDoc = null;
        private DxfBulder() : base()
        {  
            MessageObserver.Instance.SetMessage("Create DxfBulder",MessageType.Success);
        }

        public string DxfFolder { get; set; }    
              

        /// <summary>
        /// Delegate handling the finished building the dxf file
        /// </summary>
        /// <param name="dxfList"></param>
        public delegate void FinishedBuildingHandler(DataToExport dataToExport);

        /// <summary>
        /// The action finished building the dxf file
        /// </summary>
        public event FinishedBuildingHandler FinishedBuilding;

        /// <summary>
        ///  Build dxf view on based SolidWorks part
        /// </summary>
        /// <param name="pathTofile">path to sorce part file</param>
        /// <param name="IdPdm"> id in pdm system </param>
        /// <param name="currentVesin"> current or last version build file </param>
        public void Build(string pathTofile, int IdPdm, int currentVesin, string configuration)
        {
            MessageObserver.Instance.SetMessage("\t\t debug: input path " + pathTofile+ " input id: " + IdPdm + "  input ver: " + currentVesin+ "\n Thread sleep 4 sec.");
            Build(pathTofile, IdPdm, currentVesin, false, configuration);       
        }

        /// <summary>
        ///  Method which the encapsulates proccess building dxf view on based SolidWorks part.
        /// </summary>
        /// <param name="partPath">path to sorce part file</param>
        /// <param name="idPdm">id in pdm system</param>
        /// <param name="version">current or last version build file</param>
        /// <param name="includeNonSheetParts">set whether you want build dxf views from non sheet parts</param>
        /// <returns></returns>
        private bool Build(string partPath, int idPdm, int version, bool includeNonSheetParts, string configuration)
        {

            MessageObserver.Instance.SetMessage("Start build Dxf file", MessageType.System);
            bool isSave = false;

            try
            {
                modelDoc = null;
                if (!string.IsNullOrEmpty(partPath))
                {
                    try
                    {
                        modelDoc = SolidWorksAdapter.OpenDocument(partPath, swDocumentTypes_e.swDocPART);
                        if (modelDoc == null)
                        {
                            return isSave;
                        }
                        MessageObserver.Instance.SetMessage("\tOpened document " + Path.GetFileName(partPath), MessageType.System);
                    }
                    catch (Exception exception)
                    {
                        MessageObserver.Instance.SetMessage("\tFailed open SolidWorks document; message exception {" + exception.ToString() + " }", MessageType.Error);
                        throw exception;
                    }
                }
                bool isSheetmetal = true;
                
                
                modelDoc.ShowConfiguration2(configuration);
                MessageObserver.Instance.SetMessage("\t Show configuration " + configuration, MessageType.System);

                
                Bends bends = Bends.Create(modelDoc, configuration);
                bends.FixEachBend();
                MessageObserver.Instance.SetMessage("\t Fix bends " + configuration, MessageType.System);


                byte[] dxfByteCode;
                DXF dxf;

                if (!Directory.Exists(DxfFolder))
                    Directory.CreateDirectory(DxfFolder);

                if (DxfFolder != null && DxfFolder != string.Empty)
                    dxf = new DXF(DxfFolder);
                else
                    dxf = new DXF();

                isSave = dxf.ConvertToDXF(configuration, modelDoc, out dxfByteCode, isSheetmetal);

                var dataToExport = CutList.GetDataToExport(modelDoc);

                if (isSave)
                {
                    MessageObserver.Instance.SetMessage("\t" + configuration + " succsess building. Add to result list", MessageType.Success);
                    // конфигурация получена при выполнении GetDataToExport 
                    try
                    {
                        dataToExport.DXFByte = dxfByteCode;
                        dataToExport.Configuration = configuration;
                        dataToExport.IdPdm = idPdm;
                        dataToExport.Version = version;
                        if (FinishedBuilding != null)
                            FinishedBuilding(dataToExport);
                    }
                    catch (Exception exception)
                    {
                        MessageObserver.Instance.SetMessage("\tFailed at notification about finished; message exception {" + exception.ToString() + " }", MessageType.Error);
                    }
                }

                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(modelDoc.GetTitle().ToLower().Contains(".sldprt") ? modelDoc.GetTitle() : modelDoc.GetTitle() + ".sldprt"); // out in func...
                SolidWorksAdapter.DisposeSOLID();
            }
            catch(Exception ex)
            {
                MessageObserver.Instance.SetMessage("\tFailed build dxf; message exception {" + ex.ToString() + " }", MessageType.Error);
                throw ex;
            }
            return isSave;
        }
    }
}