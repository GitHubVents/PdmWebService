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

            MesageObserver.Instance.SetMessage("Create DxfBulder",MessageType.Success);
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

            MesageObserver.Instance.SetMessage("Start build Dxf file", MessageType.System);

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

                        MesageObserver.Instance.SetMessage("\t\tOpened document " + Path.GetFileName(partPath), MessageType.System);
                        // Проверяет наличие дерева постоения в моделе.
                        if (modelDoc == null)
                        {
                            return isSave;
                        }
                    }
                    catch (Exception exception)
                    {
                        MesageObserver.Instance.SetMessage("\t\tFailed open SolidWorks document; message exception {" + exception.ToString() + " }", MessageType.Error);
                    }
                } 

                bool isSheetmetal = true;

                if (!SolidWorksAdapter.IsSheetMetalPart((IPartDoc)modelDoc))
                {
                    isSheetmetal = false;
                    if (!includeNonSheetParts)
                    {

                        SolidWorksAdapter.CloseDocument(modelDoc);
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
                    isSave = SaveThisConfigurationToDxf(eachConfiguration, fixBends ? solidWorksApp : null, modelDoc, out filePath, isSheetmetal);

                    if (isSave)
                    {
                        MesageObserver.Instance.SetMessage("\t\t" + eachConfiguration + " succsess building. Add to result list", MessageType.Error);
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
                throw exception;
            }
            return isSave;
        }

        public bool SaveThisConfigurationToDxf(string configuration, SldWorks swApp, IModelDoc2 swModel, out string dxfFilePath, bool isSheetmetal)
        {

            swModel.ShowConfiguration2(configuration);
            swModel.EditRebuild3();

            if (swApp != null && isSheetmetal)
            {
                List<Bends.SolidWorksFixPattern.PartBendInfo> list;
                Bends.Fix(swApp, out list, true);
            }

            var sDxfName = DxfName(swModel.GetTitle(), configuration) + ".dxf";
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
            Console.WriteLine("ExportToDWG");
            return swPart.ExportToDWG2(dxfFilePath, swModel.GetPathName(), isSheetmetal ? (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal : (int)swExportToDWG_e.swExportToDWG_ExportSelectedFacesOrLoops, true, varAlignment, false, false, sheetmetalOptions,
                isSheetmetal ? 0 : (int)swExportToDWG_e.swExportToDWG_ExportAnnotationViews);

        }
        private string DxfName(string fileName, string config)
        {
            return $"{fileName.Replace("ВНС-", "").ToLower().Replace(".sldprt", "")}-{config}";
        }

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

        private int SheetMetalOptions(int p0, int p1, int p2, int p3, int p4, int p5, int p6)
        {
            return p0 * 1 + p1 * 2 + p2 * 4 + p3 * 8 + p4 * 16 + p5 * 32 + p6 * 64;
        }
    }
}
