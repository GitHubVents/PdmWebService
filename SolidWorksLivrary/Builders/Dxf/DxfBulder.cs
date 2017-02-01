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
            this.solidWorksApp = SolidWorksAdapter.SldWoksApp;  // Get SolidWorks exemplar
            FolderToSaveDxf = @"D:\TEMP\dxf\"; // default value

            MessageObserver.Instance.SetMessage("Create DxfBulder",MessageType.Success);
        }
        /// <summary>
        /// SolidWorks exemplar
        /// </summary>
        private SldWorks solidWorksApp;

        /// <summary>
        /// The path to the folder in which to save the built by Dxf file
        /// </summary>
        public string FolderToSaveDxf { get; set; }

        /// <summary>
        /// Delegate handling the finished building the dxf file
        /// </summary>
        /// <param name="dxfList"></param>
        public delegate void FinishedBuildingHandler(List<DxfFile> dxfList);

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
            List<DxfFile> dxfList = new List<DxfFile>();          
            Build(pathTofile, IdPdm, currentVesin, ref dxfList, true, true, false);       
            if (FinishedBuilding != null && dxfList.Count > 0)
                FinishedBuilding(dxfList);
        }

        /// <summary>
        ///  Method which the encapsulates proccess building dxf view on based SolidWorks part.
        /// </summary>
        /// <param name="partPath">path to sorce part file</param>
        /// <param name="idPdm">id in pdm system</param>
        /// <param name="version">current or last version build file</param>
        /// <param name="dxfList">out a list of finished dxf files</param>
        /// <param name="fixBends">set whether you want fix bends</param>
        /// <param name="closeAfterSave">set whether you want closing files after build </param>
        /// <param name="includeNonSheetParts">set whether you want build dxf views from non sheet parts</param>
        /// <returns></returns>
        private bool Build(string partPath, int idPdm, int version, ref List<DxfFile> dxfList, bool fixBends, bool closeAfterSave, bool includeNonSheetParts )
        {
            // callback message code from solidWorks 
            int error = 0, warnings = 0;

            MessageObserver.Instance.SetMessage("Start build Dxf file", MessageType.System);

            bool isSave = false;
            if (dxfList == null)
                dxfList = new List<DxfFile>();

            try
            {
                IModelDoc2 modelDoc = null;
                if (!string.IsNullOrEmpty(partPath))
                {
                    try
                    {
                        string emptyConfigyration = "";
                        modelDoc = solidWorksApp.OpenDoc6(partPath, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, emptyConfigyration, error, warnings);
                        modelDoc = solidWorksApp.IActiveDoc2;

                        MessageObserver.Instance.SetMessage("\t\tOpened document " + Path.GetFileName(partPath), MessageType.System);
                        // Проверяет наличие дерева постоения в моделе.
                        if (modelDoc == null)
                        {
                            return isSave;
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageObserver.Instance.SetMessage("\t\tFailed open SolidWorks document; message exception {" + exception.ToString() + " }", MessageType.Error);
                        throw exception;
                    }
                } 

                bool isSheetmetal = true;

                
                if (!SolidWorksAdapter.IsSheetMetalPart((IPartDoc)modelDoc))
                {
                    isSheetmetal = false;
                    if (!includeNonSheetParts) // disable build  no sheet metal parts if IsSheetMetalPart = false, and return  
                    {
                        MessageObserver.Instance.SetMessage();
                       this.solidWorksApp.CloseDoc(modelDoc.GetTitle().ToLower().Contains(".sldprt") ? modelDoc.GetTitle() : modelDoc.GetTitle() + ".sldprt");
                        // SolidWorksAdapter.CloseDocument(modelDoc);
                        return isSave;
                    }
                }
                string[] swModelConfNames2 = (string[])modelDoc.GetConfigurationNames();

                var configurations = from name in swModelConfNames2
                                     let config = (Configuration)modelDoc.GetConfigurationByName(name)
                                     where !config.IsDerived()
                                     select name;

                foreach (var eachConfiguration in configurations)
                {
                    string filePath;                    
                    isSave =  SaveThisConfigurationToDxf(eachConfiguration, fixBends ? solidWorksApp : null, modelDoc, out filePath, isSheetmetal);

                    if (isSave)
                    {
                        MessageObserver.Instance.SetMessage("\t\t" + eachConfiguration + " succsess building. Add to result list", MessageType.Success);
                        dxfList.Add(new DxfFile
                        {
                            Configuration = eachConfiguration,
                            FilePath = filePath,
                            IdPdm = idPdm,
                            Version = version
                        });
                    }
                }
                if (closeAfterSave)
                {
                    SolidWorksAdapter.CloseDocument(modelDoc);
                }
            }

            catch (Exception exception)
            {
                MessageObserver.Instance.SetMessage("\t\tFailed build dxf; message exception {" + exception.ToString() + " }", MessageType.Error);
            }
            return isSave;
        }

        /// <summary>
        /// Convert to dxf input configuration of document 
        /// </summary>
        /// <param name="configuration">Building configuration</param>
        /// <param name="swApp">SolidWorks exemplare</param>
        /// <param name="swModel">Building document</param>
        /// <param name="dxfFilePath">Output path to file</param>
        /// <param name="isSheetmetal">Enable or disable well whether build no sheet metal parts</param>
        /// <returns></returns>
        public bool SaveThisConfigurationToDxf(string configuration, SldWorks swApp, IModelDoc2 swModel, out string dxfFilePath, bool isSheetmetal)
        {
            dxfFilePath = string.Empty;
            try
            {
                swModel.ShowConfiguration2(configuration);
                swModel.EditRebuild3();

                if (swApp != null && isSheetmetal)
                {
                    List<Bends.SolidWorksFixPattern.PartBendInfo> list;
                    Bends.Fix(swApp, out list, true);
                }

                var sDxfName = DxfNameBuild(swModel.GetTitle(), configuration) + ".dxf";
                dxfFilePath = Path.Combine(FolderToSaveDxf, sDxfName);

                Directory.CreateDirectory(FolderToSaveDxf);

                var dataAlignment = new double[12];

                dataAlignment[0] = 0.0;
                dataAlignment[1] = 0.0;
                dataAlignment[2] = 0.0;
                dataAlignment[3] = 1.0;
                dataAlignment[4] = 0.0;
                dataAlignment[5] = 0.0;
                dataAlignment[6] = 0.0;
                dataAlignment[7] = 1.0;
                dataAlignment[8] = 0.0;
                dataAlignment[9] = 0.0;
                dataAlignment[10] = 0.0;
                dataAlignment[11] = 1.0;
                object varAlignment = dataAlignment;

                var swPart = (IPartDoc)swModel;
                int sheetmetalOptions = SheetMetalOptions(true, false, false, false, false, true, false);

                bool isExportToDWG2 =  swPart.ExportToDWG2(dxfFilePath, swModel.GetPathName(), isSheetmetal ? (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal : (int)swExportToDWG_e.swExportToDWG_ExportSelectedFacesOrLoops, true, varAlignment, false, false, sheetmetalOptions,
                    isSheetmetal ? 0 : (int)swExportToDWG_e.swExportToDWG_ExportAnnotationViews);
                MessageObserver.Instance.SetMessage("\t\tCompleted building "  + swModel.GetTitle() + " with configuration \"" + configuration + "\"", MessageType.System);
                return isExportToDWG2;
            }
            catch(Exception exception)
            {
                string message = "\t\tFailed build dxf  " + swModel.GetTitle() + " with configuration \"" + configuration + "\"" + exception.ToString();
                MessageObserver.Instance.SetMessage(message, MessageType.Error);
                return false;
            }            
        }

        /// <summary>
        ///  Combinate fileName and config for new dxf file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private string DxfNameBuild(string fileName, string config)
        {
            return $"{fileName.Replace("ВНС-", "").ToLower().Replace(".sldprt", "")}-{config}";
        }




        /// <summary>
        ///  Compute sheet metal options
        /// </summary>
        /// <param name="ExportGeometry"></param>
        /// <param name="IcnludeHiddenEdges"></param>
        /// <param name="ExportBendLines"></param>
        /// <param name="IncludeScetches"></param>
        /// <param name="MergeCoplanarFaces"></param>
        /// <param name="ExportLibraryFeatures"></param>
        /// <param name="ExportFirmingTools"></param>
        /// <returns></returns>
        private int SheetMetalOptions(bool ExportGeometry, bool IcnludeHiddenEdges, bool ExportBendLines, bool IncludeScetches, bool MergeCoplanarFaces, bool ExportLibraryFeatures, bool ExportFirmingTools)
        {
            return SheetMetalOptions(
                ExportGeometry ? 1 : 0,
                IcnludeHiddenEdges ? 1 : 0,
                ExportBendLines ? 1 : 0,
                IncludeScetches ? 1 : 0,
                MergeCoplanarFaces ? 1 : 0,
                ExportLibraryFeatures ? 1 : 0,
                ExportFirmingTools ? 1 : 0);
        }
        /// <summary>
        /// Compute sheet metal options
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <param name="p6"></param>
        /// <returns></returns>
        private int SheetMetalOptions(int p0, int p1, int p2, int p3, int p4, int p5, int p6)
        {
            return p0 * 1 + p1 * 2 + p2 * 4 + p3 * 8 + p4 * 16 + p5 * 32 + p6 * 64;
        }
    }
}
