using Patterns.Observer;
using ServiceTypes.Constants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace PDMWebService.Data.Solid.ElementsCase
{

    public sealed class SpigotBuilder : ProductBuilderBehavior
    {

        private int warning = 0, error = 0;
        public SpigotBuilder() : base()
        {
            SetProperties(@"Проекты\12 - Вибровставка", @"Библиотека проектирования\DriveWorks\12 - Spigot");
            //SetProperties(ipsModuleObject.SessionID.ToString() + @"\Workspace\Проекты\AirVents\12", ipsModuleObject.SessionID.ToString() + @"\Workspace\SW Library");
            base.AssemblyName = "12-00";
        }

        

        public string Build(int type, Vector2 spigotSize, int materialID) // должен возвращать лист с путями к файлам 
        {
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            long tempId = 0;

            List<long> idObjWithNewPath = new List<long>();

            DataSet ds = null;
            
            //DataTable dt = ipsModuleObject.GetIMBASETable((long)IMBASE_TablesID.Spigot, out ds, out dictionary);

            base.PartPrototypeName = GetPrototypeName((SpigotType_e)type);
            string drawingNameWithExt = "12-00.SLDDRW";               
            int addDimH = base.PartPrototypeName == "12-30" ? 10 : 1;


            string drawingSpigotPath = Path.Combine(RootFolder, SourceFolder, drawingNameWithExt);
            SolidWorksAdapter.OpenDocument(drawingSpigotPath, swDocumentTypes_e.swDocDRAWING);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(base.AssemblyName + ".SLDASM");
            SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);

            DeleteEquations(base.PartPrototypeName);
            DeleteComponents(type);
            SolidWorksDocument.ForceRebuild3(true);
            
            #region formuls   

            double w = spigotSize.X;
            double h = spigotSize.Y - addDimH;
            const double step = 50;
            double weldWidth = Math.Truncate(spigotSize.X / step)*1000 + 1;
            double weldHeight = Math.Truncate(spigotSize.Y / step)*1000 + 1;  

            #endregion

            if (base.PartPrototypeName == "12-20")
            {
                #region 12-20

                base.PartName = $"12-20-{spigotSize.Y}";
                    
                //if (ipsModuleObject.CheckForSimilarRows(base.PartName, dt, dictionary))// добавить out filePath
                //{
                //    // открывать детальку 
                //    SolidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    base.parameters.Add("D1@Вытянуть1", h - 31);
                    base.parameters.Add("D1@Кривая1", weldHeight);
                    EditPartParameters("12-20-001", base.NewPartPath, materialID);
                    //tempId = op.WriteIntoIMBASE_Spigot_Table(dt, base.NewPartPath + ".SLDPRT", "Обозначениеttt", base.PartName, spigotSize.X.ToString(), spigotSize.Y.ToString(), type.ToString(), 1);
                    //ComponentsPathList.Add(base.NewPartPath);
                    idObjWithNewPath.Add(tempId);
                //}

                //12-20-002

                base.PartName = $"12-20-{spigotSize.X}";

                //if (ipsModuleObject.CheckForSimilarRows(base.PartName, dt, dictionary))
                //{
                //    //открывать детальку 
                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

                    base.parameters.Add("D1@Вытянуть1", w - 31);
                    base.parameters.Add("D1@Кривая1", weldWidth);
                    EditPartParameters("12-20-002", base.NewPartPath, materialID);
                // tempId = op.WriteIntoIMBASE_Spigot_Table(dt, base.NewPartPath + ".SLDPRT", "Обозначениеttt", base.PartName, spigotSize.X.ToString(), spigotSize.Y.ToString(), type.ToString(), 1);
                //ComponentsPathList.Add(base.NewPartPath);
                //Part=1296
                //Изделие=1052
                idObjWithNewPath.Add(tempId);
                //}

                //12-003 
                base.PartName = $"12-03-{spigotSize.X}-{spigotSize.Y}";
                //if (ipsModuleObject.CheckForSimilarRows(base.PartName, dt, dictionary))
                //{
                //    //открывать детальку
                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

                    base.parameters.Add("D3@Эскиз1", w);
                    base.parameters.Add("D2@Эскиз1", h);
                    EditPartParameters("12-003", base.NewPartPath, materialID);
                // tempId = op.WriteIntoIMBASE_Spigot_Table(dt, base.NewPartPath + ".SLDPRT", "Обозначениеttt", base.PartName, spigotSize.X.ToString(), spigotSize.Y.ToString(), type.ToString(), 1);

                //ComponentsPathList.Add(base.NewPartPath);
                idObjWithNewPath.Add(tempId);
                //}
                #endregion
            }
            else if (base.PartPrototypeName == "12-30")
            {
                #region 12-30
                //12-30-001

                base.PartName = $"12-30-{spigotSize.Y}";

                //if (CheckExistPart != null)
                //{
                //    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                //}
                //else
                //{
                //    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                //}

                //if (NewPartPath != string.Empty && NewPartPath != null)
                //{
                //    SolidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

                    base.parameters.Add("D1@Вытянуть1", h - 31);
                    base.parameters.Add("D1@Кривая1", weldHeight);
                    //base.parameters.Add("D5@Эскиз1", weldHeight);
                    EditPartParameters("12-30-001", base.NewPartPath, materialID);
                ComponentsPathList.Add(base.NewPartPath);

                //}

                //12-30-002           
                base.PartName = $"12-30-{spigotSize.X}";

                //if (CheckExistPart != null)
                //{
                //    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                //}
                //else
                //{
                //    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                //}

                //if (NewPartPath != string.Empty && NewPartPath != null)
                //{
                //    SolidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);

                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

                    base.parameters.Add("D1@Вытянуть1", w - 31);
                    base.parameters.Add("D1@Кривая1", weldHeight);
                    EditPartParameters("12-30-002", base.NewPartPath, materialID);
                ComponentsPathList.Add(base.NewPartPath);
                //}
                //12-003

                base.PartName = $"12-03-{spigotSize.X}-{spigotSize.Y}";
                //if (CheckExistPart != null)
                //{
                //    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                //}
                //else
                //{
                //    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                //}

                //if (NewPartPath != string.Empty && NewPartPath != null)
                //{

                //    SolidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                //}
                //else
                //{
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

                    base.parameters.Add("D3@Эскиз1", w);
                    base.parameters.Add("D2@Эскиз1", h);
                    EditPartParameters("12-003", base.NewPartPath, materialID);
                ComponentsPathList.Add(base.NewPartPath); 

                //}
                #endregion
            }

            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(base.AssemblyName + ".SLDASM");
            SolidWorksDocument.ForceRebuild3(true);

            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, GetSpigotName((SpigotType_e)type, spigotSize));
            SolidWorksDocument.Extension.SaveAs(NewPartPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, NewPartPath + ".SLDASM");
            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(base.NewPartPath + ".SLDASM");


            //tempId = op.WriteIntoIMBASE_Spigot_Table(dt, base.AssemblyName + ".SLDASM", "ОбозначениеСборка", base.PartName, spigotSize.X.ToString(), spigotSize.Y.ToString(), type.ToString(), 1);


            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(drawingNameWithExt);
            SolidWorksDRW = SolidWorksAdapter.ToDrawingDoc(SolidWorksDocument);

            base.SolidWorksDRW.ActivateSheet("DRW1");
            SolidWorksDRW.SetupSheet5("DRW1", 12, 12, 1, GetDrawingScale(spigotSize), true, @"\\pdmsrv\SolidWorks Admin\Templates\Основные надписи\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);

            SolidWorksDocument.Extension.SaveAs(NewPartPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, NewPartPath + ".SLDDRW");
            

            // create relations between docs
            //ipsModuleObject.MakeRelationsBtwnDocs(idObjWithNewPath, tempId);

            SolidWorksAdapter.CloseAllDocumentsAndExit();
           
            return NewPartPath;
            
        }

        #region clear document
        /// <summary>
        /// delete equation for model, by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="swModel"></param>
        private void DeleteEquation(int index, IModelDoc2 swModel)
        {
            try
            {
                EquationMgr myEqu = swModel.GetEquationMgr();
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
        private void DeleteEquations(string modelName)
        {
            switch (modelName)
            {
                case "12-20":
                    DeleteEquation(5, SolidWorksDocument);
                    DeleteEquation(4, SolidWorksDocument);
                    DeleteEquation(3, SolidWorksDocument);
                    break;
                case "12-30":
                    DeleteEquation(0, SolidWorksDocument);
                    DeleteEquation(0, SolidWorksDocument);
                    DeleteEquation(0, SolidWorksDocument);
                    break;
            }
        }

        /// <summary>
        /// Удаляет лишние компоненты для каждого типа.
        /// </summary>
        /// <param name="type"></param>
        protected override void DeleteComponents(int type)
        {
            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

            if ( (SpigotType_e)type == SpigotType_e.Twenty_mm)
            {              

                SolidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-30-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.002-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }
            if ((SpigotType_e)type == SpigotType_e.Thirty_mm)
            {
                 
                SolidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-20-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ВНС-96.61.001-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Клей-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }

            SolidWorksDocument.Extension.SelectByID2("30", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            SolidWorksDocument.EditDelete();
            SolidWorksDocument.Extension.SelectByID2("20", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            SolidWorksDocument.EditDelete();
        }
        #endregion



        /// <summary>
        ///  Determinate and returns spigot name by main params
        /// </summary>
        /// <param name="spigotType">Spigot type</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isShowExtension">Put true if need name and extension </param>
        /// <returns></returns>
        private static string GetSpigotName(SpigotType_e spigotType, Vector2 size,  bool isShowExtension = false)
        {
            // if we need show extension for example that 
            // a check the availability in the data base
            string spigotName = GetPrototypeName(spigotType) + "-" + size.X + "-" + size.Y;
            if (isShowExtension)    spigotName += ".SLDASM";
            return spigotName;
        }

        /// <summary>
        /// Determinate model name by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetPrototypeName(SpigotType_e type)
        {
            switch (type)
            {
                case SpigotType_e.Twenty_mm:
                    return "12-20";
                    
                case SpigotType_e.Thirty_mm:
                    return "12-30";

                default:
                    throw new Exception("Type {" + type + "} is invalid");
            }
        }

        protected override  void InitiatorSaveExeption(int error, int warning, string path = "")
        {
            if (error != 0 || warning != 0)
            {
                var exeption = new Exception
                    ("Failed save file " + " error code {" + error + "}, error description: " +
                    (swFileSaveError_e)error + ", warning code {" + warning +
                    "}, warning description: " + (swFileSaveWarning_e)warning);

                MessageObserver.Instance.SetMessage(exeption.ToString(), Patterns.Observer.MessageType.Error);
            }
        }

        /// <summary>
        /// Returns drawing scale.
        /// </summary>
        /// <param name="size">Panel size as Vector2</param>
        /// <returns></returns>
        private int GetDrawingScale(Vector2 size)
        {
            int scale = 5;
            if (Convert.ToInt32(size.X) > 500 || Convert.ToInt32(size.Y) > 500)
            { scale = 10; }
            if (Convert.ToInt32(size.X) > 850 || Convert.ToInt32(size.Y) > 850)
            { scale = 15; }
            if (Convert.ToInt32(size.X) > 1250 || Convert.ToInt32(size.Y) > 1250)
            { scale = 20; }

            return scale;
        }
    }
}