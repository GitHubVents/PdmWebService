using Patterns.Observer;
using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.Collections.Generic;

namespace PDMWebService.Data.Solid.ElementsCase
{
     
    public sealed class SpigotBuilder : AbstractBuilder
    { 
        private string NewSpigotName { get; set; }
        private ModelDoc2 solidWorksDocument { get; set; }         
        public SpigotBuilder() : base()
        {
            SetProperties(@"12 - Вибровставка", @"12 - Spigot");
        }

        private int warning = 0, error = 0;

        public string Build(SpigotType type, int width, int height)
        {
            string modelName = GetModelName(type);
            NewSpigotName = GetSpigotName(type, width, height);

            string newPartName = string.Empty;
            string newPartPath = string.Empty;

            IModelDoc2 partModeltDocument;
            bool isPartExist = false;
            Dimension myDimension;

            int addDimH = modelName == "12-30" ? 10 : 1;

            string newSpigotPath = $@"{RootFolder}{SubjectDestinationFolder}\{NewSpigotName}";

            string drawingName = modelName == "12-30" ? modelName : "12-00";

            var modelSpigotDrw = $@"{RootFolder}{SourceFolder}\{drawingName}.SLDDRW";

            ModelDoc2 swDrawingSpigot = SolidWorksAdapter.OpenDocument(modelSpigotDrw, swDocumentTypes_e.swDocDRAWING);
            solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00");
            AssemblyDoc assemblyDocument = (AssemblyDoc)solidWorksDocument;
            assemblyDocument.ResolveAllLightWeightComponents(false);

            DeleteEquations(modelName);
            solidWorksDocument.ForceRebuild3(true);

            #region formuls           
            var w = (Convert.ToDouble(width) - 1) / 1000;  // ????????????
            var h = Convert.ToDouble((Convert.ToDouble(height) + addDimH) / 1000);  // ????????????
            const double step = 50;
            var weldWidth = Convert.ToDouble((Math.Truncate(Convert.ToDouble(width) / step) + 1));   // ????????????
            var weldHeight = Convert.ToDouble((Math.Truncate(Convert.ToDouble(height) / step) + 1));  // ????????????
            #endregion

            DeleteComponents((int)type);
            if (modelName == "12-20")
            {
                MessageObserver.Instance.SetMessage("12-20-001");
                //12-20-001
                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-20-001");
                newPartName = $"12-20-{height}.SLDPRT";
                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
                if (CheckExistPart != null)
                {
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-20-001.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-20-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-20-001.Part")));
                    myDimension.SystemValue = weldHeight;
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }

                //12-20-002
                MessageObserver.Instance.SetMessage("12-20-002");
                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-20-002");
                newPartName = $"12-20-{width}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
                if (CheckExistPart != null)
                {
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-20-002.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-20-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-20-002.Part")));
                    myDimension.SystemValue = weldWidth;
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }

                //12-003
                MessageObserver.Instance.SetMessage("12-20-003");
                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-003");
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
                if (CheckExistPart != null)
                {
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }
                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    solidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    solidWorksDocument.EditRebuild3();
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }
            }
            if (modelName == "12-30")
            {
                //12-30-001

                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-30-001");
                newPartName = $"12-30-{height}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
                if (CheckExistPart != null)
                {
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-30-001.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-30-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-30-001.Part")));
                    myDimension.SystemValue = weldHeight;
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }
                //12-30-002
                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-30-002");
                newPartName = $"12-30-{width}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
                if (CheckExistPart != null)
                {
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-30-002.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-30-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-30-002.Part")));
                    myDimension.SystemValue = weldHeight;
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }
                //12-003

                partModeltDocument = SolidWorksAdapter.AcativeteDoc("12-003");
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                if (CheckExistPart != null)
                {
                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (isPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";
                    solidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    solidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(solidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    solidWorksDocument.EditRebuild3();
                    partModeltDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }
            }

            solidWorksDocument.ForceRebuild3(true);
            solidWorksDocument.Extension.SaveAs(newSpigotPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, newSpigotPath + ".SLDASM");

            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewSpigotName + ".SLDASM");
            ComponentsPathList.Add(newSpigotPath + ".SLDASM");
            swDrawingSpigot.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)SolidWorksAdapter.AcativeteDoc(drawingName + ".SLDDRW");
            drw.ActivateSheet("DRW1");
            drw.SetupSheet5("DRW1", 12, 12, 1, GetDrawingScale(width, height), true, @"\\pdmsrv\SolidWorks Admin\Templates\Основные надписи\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);
            swDrawingSpigot.Extension.SaveAs(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, newSpigotPath + ".SLDDRW");

            ComponentsPathList.Add(newSpigotPath + ".SLDDRW");
            SolidWorksAdapter.CloseAllDocumentsAndExit();
            return newSpigotPath;
        }

        #region clear document
        /// <summary>
        /// dellete equation for model, by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="swModel"></param>
        public void DeleteEquation(int index, IModelDoc2 swModel)
        {
            try
            {
                var myEqu = swModel.GetEquationMgr();
                myEqu.Delete(index);
                swModel.EditRebuild3();
            }
            catch (Exception exception)
            {
                MessageObserver.Instance.SetMessage("Exeption at delete equations " + exception.ToString());
            }
        }
        /// <summary>
        /// Delete equations my model name
        /// </summary>
        /// <param name="modelName"></param>
        public void DeleteEquations(string modelName)
        {
            switch (modelName)
            {
                case "12-20":
                    DeleteEquation(5, solidWorksDocument);
                    DeleteEquation(4, solidWorksDocument);
                    DeleteEquation(3, solidWorksDocument);
                    break;
                case "12-30":
                    DeleteEquation(0, solidWorksDocument);
                    DeleteEquation(0, solidWorksDocument);
                    DeleteEquation(0, solidWorksDocument);
                    break;
            }
        }

        /// <summary>
        /// Удаляет лишние компоненты для каждого типа.
        /// </summary>
        /// <param name="e_type"></param>
       
        protected override void DeleteComponents(int type)
        {
            SpigotType e_type = (SpigotType)type;
            if (e_type == SpigotType.Twenty_mm)
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

                solidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-30-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }
            if (e_type == SpigotType.Thirty_mm)
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;
                solidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-20-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Клей-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }

            solidWorksDocument.Extension.SelectByID2("30", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            solidWorksDocument.EditDelete();
            solidWorksDocument.Extension.SelectByID2("20", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            solidWorksDocument.EditDelete();
        }
        #endregion



        /// <summary>
        ///  Determinate adn returns spigot name by main params
        /// </summary>
        /// <param name="spigotType">Spigot type</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isShowExtension">Put true if need name and extension </param>
        /// <returns></returns>
        public static  string GetSpigotName(SpigotType spigotType, int width, int height, bool isShowExtension = false)
        {
            // if we need show extension for example that 
            // a check the availability in the data base
            var spigotName = GetModelName(spigotType) + "-" + width + "-" + height;
            if (isShowExtension)
                spigotName += ".ASMSLD";
            return spigotName;
        }
        /// <summary>
        /// Determinate model name by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static  string GetModelName(SpigotType type)
        {
            switch (type)
            {
                case SpigotType.Twenty_mm:
                    return "12-20";

                case SpigotType.Thirty_mm:
                    return "12-30";

                default:
                    throw new Exception("Type {" + type + "} is invelid");
            }
        }


        /// <summary>
        /// Initiate exeption message and send to observer. 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="warning"></param>
        /// <param name="path"></param>
        protected override  void InitiatorSaveExeption(int error, int warning, string path = "")
        {
            if (error != 0 || warning != 0)
            {
                var exeption = new Exception
                    ("Failed save file " + " error code {" + error + "}, error description: " +
                    (swFileSaveError_e)error + ", warning code {" + warning +
                    "}, warning description: " + (swFileSaveWarning_e)warning);

                MessageObserver.Instance.SetMessage(exeption.ToString(), MessageType.Error);
            }
        }
        
        private int GetDrawingScale(int width, int height)
        {
            int scale = 5;
            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500)
            { scale = 10; }
            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850)
            { scale = 15; }
            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250)
            { scale = 20; }

            return scale;
        }
    }
}






#region
//using SolidWorks.Interop.sldworks;
//using SolidWorks.Interop.swconst;
//using SolidWorksLibrary;
//using System;
//using System.Collections.Generic;
//using Patterns.Observer;
//namespace PDMWebService.Data.Solid.PartBuilders
//{
//    #region delegates
//    [Serializable]
//    public delegate void CheckExistPartHandler(string partName, out bool isExesitPatrt, out string pathToPartt);
//    [Serializable]
//    public delegate void FinishedBuildHandler(List<string> ComponentsPathList);
//    #endregion

//    /// <summary>
//    /// Build spigot assembly
//    /// </summary>
//    public class SpigotBuilder
//    {
//        #region fields, properties, events, consts
//        /// <summary>
//        /// The files info list by components which It contains after got or created 
//        /// </summary>
//        private List<string> ComponentsPathList { get; set; }

//        /// <summary>
//        /// Folder for saving components "Spigot"
//        /// </summary>
//        private string SpigotDestinationFolder = @"\12 - Вибровставка";

//        /// <summary>
//        ///  Папка с исходной моделью "Вибровставки".  Folder which contains the source spigot model 
//        /// </summary>
//        private string SpigotFolder { get; set; } = @"\12 - Spigot";

//        /// <summary>
//        /// Work model
//        /// </summary>
//        private ModelDoc2 solidWorksDoument { get; set; }



//        /// <summary>
//        /// Root path to file system
//        /// </summary>
//        private string RootFolder = @"C:\TestPDM";

//        // ========================= README about CheckExistPartEvent =====================================================================  
//        // Событие при вызове которого происходит проверка на наличия детали и возвращает через операторы out булево значение и путь к файлу
//        // если деталь существует. Это прозволяет не привязываться к формату хранения файлов таким как PDM, IPS, SQl проводнику итд...

//        /// <summary>
//        /// Provides notification and feedback to check for part
//        /// </summary>
//        public /*event*/ CheckExistPartHandler CheckExistPart { get; set; }
//        // ==================================================================================================================================

//        /// <summary>
//        /// Informing subscribers the completion of building 
//        /// </summary>
//        public /*event*/  FinishedBuildHandler FinishedBuild { get; set; }


//        private const double STEP = 50;

//        #endregion

//        public SpigotBuilder() : base()
//        {
//            ComponentsPathList = new List<string>();

//        }

//        /// <summary>
//        /// Determinate model name by type
//        /// </summary>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        private static string DeterminateModelName(int type)
//        {
//            switch (type)
//            {
//                case 20:
//                    return "12-20";

//                case 30:
//                    return "12-30";

//                default:
//                    return "12-00";
//            }
//        }

//        /// <summary>
//        ///  Determinate adn returns spigot name by main params
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="width"></param>
//        /// <param name="height"></param>
//        /// <param name="isShowExtension">Put true if need name and extension </param>
//        /// <returns></returns>
//        public static string GetSpigotName(int type, int width, int height, bool isShowExtension = false)
//        {
//            // if we need show extension for example that 
//            // a check the availability in the data base
//            var spigotName = DeterminateModelName(type) + "-" + width + "-" + height;
//            if (isShowExtension)
//                spigotName += ".ASMSLD";
//            return spigotName;
//        }

//        /// <summary>
//        /// Build spigot 
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="width"></param>
//        /// <param name="height"></param>
//        /// <returns></returns>
//        public string Build(int type, int width, int height)
//        { 
//            string modelName = DeterminateModelName(type);

//            var newSpigotName = GetSpigotName(type, width, height);
//            var newSpigotPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newSpigotName}";


//            // bear outside from builder
//            //if (File.Exists(newSpigotPath + ".SLDDRW"))
//            //{
//            //    //MessageBox.Show(newSpigotPath + ".SLDDRW", "Данная модель уже находится в базе");
//            //    return "";
//            //}

//            var drawing = "12-00";
//            if (modelName == "12-30")
//            {
//                drawing = modelName;
//            }
//            Dimension myDimension;
//            var modelSpigotDrw = $@"{RootFolder}{SpigotFolder}\{drawing}.SLDDRW";

//            //  PDM.SolidWorksPdmAdapter.Instance.GetLastVersionAsmPdm(modelSpigotDrw);

//            var swDrwSpigot = SolidWorksAdapter.SldWoksApp.OpenDoc6(modelSpigotDrw, (int)swDocumentTypes_e.swDocDRAWING,
//                                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel + (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "00", 0, 0);

//            SolidWorksAdapter.SldWoksApp.Visible = true;
//            solidWorksDoument = SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00", false, 0);

//            var assemblyDocument = (AssemblyDoc)solidWorksDoument;
//            assemblyDocument.ResolveAllLightWeightComponents(false);

//            #region DelEquations
//            switch (modelName)
//            {
//                case "12-20":
//                    DelEquations(5, solidWorksDoument);
//                    DelEquations(4, solidWorksDoument);
//                    DelEquations(3, solidWorksDoument);
//                    break;
//                case "12-30":
//                    DelEquations(0, solidWorksDoument);
//                    DelEquations(0, solidWorksDoument);
//                    DelEquations(0, solidWorksDoument);
//                    break;
//            }
//            #endregion

//            solidWorksDoument.ForceRebuild3(true);

//            string newPartName;
//            string newPartPath;
//            IModelDoc2 parModeltDocument;

//            DeleteDcumentComponents(Convert.ToInt32(type));


//            #region Сохранение и изменение элементов

//            var addDimH = 1;
//            if (modelName == "12-30")
//            {
//                addDimH = 10;
//            }

//            #region forumula
//            double w = (Convert.ToDouble(width) - 1) / 1000;
//            double h = Convert.ToDouble((height + addDimH) / 1000);
//            double weldW = Convert.ToDouble((Math.Truncate(width / STEP) + 1));
//            double weldH = Convert.ToDouble((Math.Truncate(height / STEP) + 1));
//            #endregion

//            bool isPartExist = false;
//            newPartPath = string.Empty;


//            if (modelName == "12-20")
//            {
//                MessageObserver.Instance.SetMessage("12-20-001");
//                //12-20-001
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-20-001", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-20-{height}.SLDPRT";


//                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {
//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-20-001.SLDPRT");
//                }
//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Вытянуть1@12-20-001.Part")));
//                    myDimension.SystemValue = h - 0.031;
//                    solidWorksDoument.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Кривая1@12-20-001.Part")));
//                    myDimension.SystemValue = weldH;
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }

//                //12-20-002
//                MessageObserver.Instance.SetMessage("12-20-002");
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-20-002", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-20-{width}.SLDPRT";

//                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {

//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-20-002.SLDPRT");
//                }

//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Вытянуть1@12-20-002.Part")));
//                    myDimension.SystemValue = w - 0.031;
//                    solidWorksDoument.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Кривая1@12-20-002.Part")));
//                    myDimension.SystemValue = weldW;
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }

//                //12-003
//                MessageObserver.Instance.SetMessage("12-20-003");
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-003", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-03-{width}-{height}.SLDPRT";

//                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {
//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-003.SLDPRT");
//                }
//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D3@Эскиз1@12-003.Part")));
//                    myDimension.SystemValue = w;
//                    solidWorksDoument.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D2@Эскиз1@12-003.Part")));
//                    myDimension.SystemValue = h;
//                    solidWorksDoument.EditRebuild3();
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }
//            }


//            if (modelName == "12-30")
//            {
//                //12-30-001
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-30-001", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-30-{height}.SLDPRT";

//                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {
//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-30-001.SLDPRT");
//                }
//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Вытянуть1@12-30-001.Part")));
//                    myDimension.SystemValue = h - 0.031;
//                    solidWorksDoument.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Кривая1@12-30-001.Part")));
//                    myDimension.SystemValue = weldH;
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }

//                //12-30-002
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-30-002", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-30-{width}.SLDPRT";

//                MessageObserver.Instance.SetMessage("Check exist part. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {
//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-30-002.SLDPRT");
//                }
//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Вытянуть1@12-30-002.Part")));
//                    myDimension.SystemValue = w - 0.031;
//                    solidWorksDoument.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D1@Кривая1@12-30-002.Part")));
//                    myDimension.SystemValue = weldH;
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }

//                //12-003
//                SolidWorksAdapter.SldWoksApp.IActivateDoc2("12-003", false, 0);
//                parModeltDocument = SolidWorksAdapter.SldWoksApp.IActiveDoc2;
//                newPartName = $"12-03-{width}-{height}.SLDPRT";

//                MessageObserver.Instance.SetMessage("Тут должно быть событие. " + newPartName);
//                if (CheckExistPart != null)
//                {
//                    CheckExistPart(newPartName, out isPartExist, out newPartPath);
//                }
//                else
//                {
//                      M MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
//                }

//                if (isPartExist)
//                {
//                    solidWorksDoument = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
//                    solidWorksDoument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc("12-003.SLDPRT");
//                }
//                else
//                {
//                    newPartPath = $@"{RootFolder}\{SpigotDestinationFolder}\{newPartName}";
//                    solidWorksDoument.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D3@Эскиз1@12-003.Part")));
//                    myDimension.SystemValue = w;
//                    solidWorksDoument.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
//                    myDimension = ((Dimension)(solidWorksDoument.Parameter("D2@Эскиз1@12-003.Part")));
//                    myDimension.SystemValue = h;
//                    solidWorksDoument.EditRebuild3();
//                    parModeltDocument.SaveAs4(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
//                    ComponentsPathList.Add(newPartPath);
//                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
//                }
//            }

//            #endregion




//            solidWorksDoument.ForceRebuild3(true);
//            solidWorksDoument.SaveAs2(newSpigotPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
//            SolidWorksAdapter.SldWoksApp.CloseDoc(newSpigotName + ".SLDASM");
//            ComponentsPathList.Add(newSpigotPath + ".SLDASM");
//            swDrwSpigot.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
//            var drw = (DrawingDoc)(SolidWorksAdapter.SldWoksApp.IActivateDoc3(drawing + ".SLDDRW", true, 0));
//            drw.ActivateSheet("DRW1");
//            var m = 5;
//            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500)
//            { m = 10; }
//            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850)
//            { m = 15; }
//            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250)
//            { m = 20; }
//                         //    drw.SetupSheet5("DRW1", 12, 12, 1, m, true, @"\\pdmsrv\SolidWorks Admin\Templates\Основные надписи\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);
//            //swDrwSpigot.SaveAs2(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
//            var errors = 0;
//            var warnings = 0;

//            swDrwSpigot.SaveAs4(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);

//            ComponentsPathList.Add(newSpigotPath + ".SLDDRW");
//            SolidWorksAdapter.CloseAllDocumentsAndExit();

//            // ===============================================================================
//            if (FinishedBuild != null)
//            {
//                FinishedBuild(ComponentsPathList);
//            }
//            else
//            {
//                MessageObserver.Instance.SetMessage( new NullReferenceException("CheckExistPartEvent can not be null").ToString());
//            }

//            //  PDMWebService.Data.PDM.PDMAdapter.Instance.CheckInOutPdm(NewComponents, true);

//            return newSpigotPath;
//        }

//        public static void DelEquations(int index, IModelDoc2 swModel)
//        {
//            try
//            {
//                //Логгер.Информация($"Удаление уравнения #{index} в модели {swModel.GetPathName()}", "", null, "DelEquations");
//                var myEqu = swModel.GetEquationMgr();
//                myEqu.Delete(index);
//                swModel.EditRebuild3();
//                //myEqu.Add2(index, "\"" + System.Convert.ToChar(index) + "\"=" + index, false);
//            }
//            catch (Exception e)
//            {
//                //Логгер.Ошибка($"Удаление уравнения #{index} в модели {swModel.GetPathName()}. {e.Message}", e.StackTrace, null, "DelEquations");
//            }
//        }


//        /// <summary>
//        /// Удаляет лишние компоненты для каждого типа.
//        /// </summary>
//        /// <param name="type"></param>
//        private void DeleteDcumentComponents(int type)
//        {


//            if (type == 20)
//            {
//                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

//                solidWorksDoument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-30-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.002-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                solidWorksDoument.Extension.DeleteSelection2(deleteOption);
//            }
//            if (type == 30)
//            {
//                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
//                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
//                solidWorksDoument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-20-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("ВНС-96.61.001-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.SelectByID2("Клей-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                solidWorksDoument.Extension.DeleteSelection2(deleteOption);
//            }

//            solidWorksDoument.Extension.SelectByID2("30", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
//            solidWorksDoument.EditDelete();
//            solidWorksDoument.Extension.SelectByID2("20", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
//            solidWorksDoument.EditDelete();


//        }




//    }
//}
#endregion
