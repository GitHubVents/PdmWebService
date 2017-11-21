using Patterns;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

 namespace SolidWorksLibrary.Builders.Dxf
{
    /// <summary>
    /// DxfBuilder allows builds dxf views on based SolidWorks parts. 
    /// </summary>
    public class DxfBulder : Singeton<DxfBulder>
    {
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
        public void Build(string pathTofile, int IdPdm, int currentVesin)
        {
            MessageObserver.Instance.SetMessage("\t\t debug: input path " + pathTofile+ " input id: " + IdPdm + "  input ver: " + currentVesin+ "\n Thread sleep 4 sec.");
            Build(pathTofile, IdPdm, currentVesin, false);       
         
        }

        /// <summary>
        ///  Method which the encapsulates proccess building dxf view on based SolidWorks part.
        /// </summary>
        /// <param name="partPath">path to sorce part file</param>
        /// <param name="idPdm">id in pdm system</param>
        /// <param name="version">current or last version build file</param>
        /// <param name="dataToExport">out a list of finished dxf files</param> 
        
        /// <param name="includeNonSheetParts">set whether you want build dxf views from non sheet parts</param>
        /// <returns></returns>
        private bool Build(string partPath, int idPdm, int version  , bool includeNonSheetParts )
        {
            // callback message code from solidWorks 
           // int error = 0, warnings = 0;

            MessageObserver.Instance.SetMessage("Start build Dxf file", MessageType.System);

            bool isSave = false;
           

            try
            {
                ModelDoc2 modelDoc = null;
                if (!string.IsNullOrEmpty(partPath))
                {
                    try
                    {
                        modelDoc = SolidWorksAdapter.OpenDocument(partPath, swDocumentTypes_e.swDocPART);// SolidWorksAdapter.SldWoksAppExemplare.OpenDoc6(partPath, (int)  swDocumentTypes_e.swDocPART , (int)swOpenDocOptions_e.swOpenDocOptions_Silent, emptyConfigyration, error, warnings);
                       // modelDoc = SolidWorksAdapter.SldWoksAppExemplare.IActiveDoc2;

                        MessageObserver.Instance.SetMessage("\tOpened document " + Path.GetFileName(partPath), MessageType.System);
                        // Проверяет наличие дерева постоения в моделе.
                        if (modelDoc == null)
                        {
                            return isSave;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageObserver.Instance.SetMessage("\tFailed open SolidWorks document; message exception {" + exception.ToString() + " }", MessageType.Error);
                        throw exception;
                    }
                } 

                bool isSheetmetal = true;

                
                if (!SolidWorksAdapter.IsSheetMetalPart((IPartDoc)modelDoc))
                {
                    isSheetmetal = false;
                    if (!includeNonSheetParts) // disable build  no sheet metal parts if IsSheetMetalPart = false, and return  
                    {
                        try
                        {
                       SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(modelDoc.GetTitle().ToLower().Contains(".sldprt") ? modelDoc.GetTitle() : modelDoc.GetTitle() + ".sldprt");
                           
                        }
                        catch (Exception ex)
                        {
                            MessageObserver.Instance.SetMessage("Неудалось закрыть файл " + ex.Message);
                        }
                        return isSave;
                    }
                }
                string[] swModelConfNames2 = (string[])modelDoc.GetConfigurationNames();


                var configurations = from name in swModelConfNames2
                                     let config = (Configuration)modelDoc.GetConfigurationByName(name)
                                     where !config.IsDerived()
                                     select name;
                MessageObserver.Instance.SetMessage("\t got configuration "+configurations.Count()+" for opened document. Statrt bust configurations", MessageType.System);
                foreach (var eachConfiguration in configurations)
                {

                    modelDoc.ShowConfiguration2(eachConfiguration);
                    modelDoc.EditRebuild3();
                    MessageObserver.Instance.SetMessage("\t Show configuration " +eachConfiguration, MessageType.System);
                    if (  isSheetmetal)
                    { 
                        Bends bends = Bends.Create(modelDoc, eachConfiguration);
                        bends.Fix();
                    
                        MessageObserver.Instance.SetMessage("\t Fix bends " + eachConfiguration, MessageType.System);
                    }

                    byte[] dxfByteCode;
                    DXF dxf;

                    if (!Directory.Exists(DxfFolder))
                        Directory.CreateDirectory(DxfFolder);

                    if (DxfFolder != null && DxfFolder != string.Empty)
                        dxf = new DXF(DxfFolder);
                    else
                        dxf = new DXF( );

                    isSave  = dxf.ConvertToDXF(eachConfiguration,  modelDoc, out dxfByteCode, isSheetmetal);
                    // dataToExport is method parameter
                  var  dataToExport = CutList.GetDataToExport(  modelDoc);
                   

                    if (isSave)
                    {
                        MessageObserver.Instance.SetMessage("\t" + eachConfiguration + " succsess building. Add to result list", MessageType.Success);

                        // конфигурация получена при выполнении GetDataToExport 
                        try
                        {
                            dataToExport.DXFByte = dxfByteCode;
                            dataToExport.Configuration = eachConfiguration;
                            dataToExport.IdPdm = idPdm;
                            dataToExport.Version = version;
                            if (FinishedBuilding != null)
                                FinishedBuilding(dataToExport);
                        }
                        catch(Exception exception)
                        {
                            MessageObserver.Instance.SetMessage("\tFailed at notification about finished; message exception {" + exception.ToString() + " }", MessageType.Error);
                        }

                    }
                }
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(modelDoc.GetTitle().ToLower().Contains(".sldprt") ? modelDoc.GetTitle() : modelDoc.GetTitle() + ".sldprt"); // out in func...
                SolidWorksAdapter.SldWoksAppExemplare.ExitApp( );
            }

            catch (Exception exception)
            {
                MessageObserver.Instance.SetMessage("\tFailed build dxf; message exception {" + exception.ToString() + " }", MessageType.Error);
            }
            return isSave;
        }

    }
}