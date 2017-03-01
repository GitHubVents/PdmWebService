﻿using Patterns.Observer;
using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.Collections.Generic;

namespace PDMWebService.Data.Solid.ElementsCase
{
     
    public sealed class SpigotBuilder : ProductBuilderBehavior
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
        
            string newPartPath = string.Empty;

           
           
            Dimension dimension;

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
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-20-001");
                NewPartPath = $"12-20-{height}.SLDPRT";
                MessageObserver.Instance.SetMessage("Check exist part. " + NewPartPath);
                if (CheckExistPart != null)
                {
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-20-001.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-20-001.Part")));
                    dimension.SystemValue = h - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-20-001.Part")));
                    dimension.SystemValue = weldHeight;
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
                }

                //12-20-002
                MessageObserver.Instance.SetMessage("12-20-002");
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-20-002");
                NewPartPath = $"12-20-{width}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + NewPartPath);
                if (CheckExistPart != null)
                {
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-20-002.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-20-002.Part")));
                    dimension.SystemValue = w - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-20-002.Part")));
                    dimension.SystemValue = weldWidth;
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
                }

                //12-003
                MessageObserver.Instance.SetMessage("12-20-003");
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-003");
                NewPartPath = $"12-03-{width}-{height}.SLDPRT";
                MessageObserver.Instance.SetMessage("Check exist part. " + NewPartPath);
                if (CheckExistPart != null)
                {
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                    MessageObserver.Instance.SetMessage("\tCheckExistPartEvent");
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }
                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    dimension.SystemValue = w;
                    solidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    dimension.SystemValue = h;
                    solidWorksDocument.EditRebuild3();
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
                }
            }
            if (modelName == "12-30")
            {
                //12-30-001

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-30-001");
                NewPartPath = $"12-30-{height}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + NewPartPath);
                if (CheckExistPart != null)
                {
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-30-001.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-30-001.Part")));
                    dimension.SystemValue = h - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-30-001.Part")));
                    dimension.SystemValue = weldHeight;
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
                }
                //12-30-002
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-30-002");
                NewPartPath = $"12-30-{width}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + NewPartPath);
                if (CheckExistPart != null)
                {
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-30-002.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@12-30-002.Part")));
                    dimension.SystemValue = w - 0.031;
                    solidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D1@Кривая1@12-30-002.Part")));
                    dimension.SystemValue = weldHeight;
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
                }
                //12-003

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-003");
                NewPartPath = $"12-03-{width}-{height}.SLDPRT";
                if (CheckExistPart != null)
                {
                    CheckExistPart(NewPartPath, out IsPartExist, out newPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if (IsPartExist)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00.SLDASM");
                    solidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    newPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{NewPartPath}";
                    solidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    dimension.SystemValue = w;
                    solidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(solidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    dimension.SystemValue = h;
                    solidWorksDocument.EditRebuild3();
                    SolidWorksDocument.Extension.SaveAs(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                    InitiatorSaveExeption(error, warning, newPartPath);
                    ComponentsPathList.Add(newPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath);
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



 