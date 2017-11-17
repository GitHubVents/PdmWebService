using Patterns.Observer;
using ServiceTypes.Constants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.IO;

namespace PDMWebService.Data.Solid.ElementsCase
{

    public sealed class SpigotBuilder : ProductBuilderBehavior
    {       
        public SpigotBuilder() : base()
        {            
            SetProperties(@"Проекты\12 - Вибровставка", @"Библиотека проектирования\DriveWorks\12 - Spigot");
        }

        private int warning = 0, error = 0;

        public string Build(SpigotType_e type, Vector2 spigotSize)
        {            
            base.PartPrototypeName = GetPrototypeName(type);
            base.PartName = GetSpigotName(type, spigotSize);               
            Dimension dimension;
            int addDimH = base.PartPrototypeName == "12-30" ? 10 : 1;

            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder,base.PartName);
            
            string drawingName = base.PartPrototypeName == "12-30" ? base.PartPrototypeName : "12-00";
            var modelSpigotDrw = Path.Combine( RootFolder,SourceFolder,$"{drawingName}.SLDDRW");

            ModelDoc2 swDrawingSpigot = SolidWorksAdapter.OpenDocument(modelSpigotDrw, swDocumentTypes_e.swDocDRAWING);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("12-00");
            AssemblyDoc assemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
            DeleteEquations(base.PartPrototypeName);
            SolidWorksDocument.ForceRebuild3(true);

            #region formuls           
            var w = (Convert.ToDouble(spigotSize.X) - 1) / 1000;   
            var h = Convert.ToDouble((Convert.ToDouble(spigotSize.Y) + addDimH) / 1000);   
            const double step = 50;
            var weldWidth = Convert.ToDouble((Math.Truncate(Convert.ToDouble(spigotSize.X) / step) + 1));    
            var weldHeight = Convert.ToDouble((Math.Truncate(Convert.ToDouble(spigotSize.Y) / step) + 1));  
            #endregion

            DeleteComponents((int)type);
            if (base.PartPrototypeName == "12-20")
            {
                
                base.PartName = $"12-20-{spigotSize.Y}.SLDPRT";               
                if (CheckExistPart != null)
                { 
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if(NewPartPath != string.Empty && NewPartPath!= null)
                {
                                     
                    SolidWorksDocument.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);                    
                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
 
                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    SolidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Вытянуть1@12-20-001.Part")));
                    dimension.SystemValue = h - 0.031;
                    SolidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая1@12-20-001.Part")));
                    dimension.SystemValue = weldHeight;
                    Console.WriteLine( NewPartPath);
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(base.NewPartPath);
                }

                //12-20-002
          
                base.PartName = $"12-20-{spigotSize.X}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + base.PartName);
                if (CheckExistPart != null)
                {
                    Console.WriteLine(RootFolder );
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if(NewPartPath != string.Empty && NewPartPath!= null)
                {
                    
                    SolidWorksDocument.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);                 
                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                   
                    SolidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Вытянуть1@12-20-002.Part")));
                    dimension.SystemValue = w - 0.031;
                    SolidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая1@12-20-002.Part")));
                    dimension.SystemValue = weldWidth;
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath);
                }

                //12-003 
                base.PartName = $"12-03-{spigotSize.X}-{spigotSize.Y}.SLDPRT";               
                if (CheckExistPart != null)
                {
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath); 
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }
                if(NewPartPath != string.Empty && NewPartPath!= null)
                {
                    SolidWorksDocument.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true); 
                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    SolidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    dimension.SystemValue = w;
                    SolidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    dimension.SystemValue = h;
                    SolidWorksDocument.EditRebuild3();
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(base.NewPartPath);
                }
            }
            if (base.PartPrototypeName == "12-30")
            {
                //12-30-001

                base.PartName = $"12-30-{spigotSize.Y}.SLDPRT";

                MessageObserver.Instance.SetMessage("Check exist part. " + base.PartName);
                if (CheckExistPart != null)
                {
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if(NewPartPath != string.Empty && NewPartPath!= null)
                {
                    
                    SolidWorksDocument.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);               
                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    SolidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Вытянуть1@12-30-001.Part")));
                    dimension.SystemValue = h - 0.031;
                    SolidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая1@12-30-001.Part")));
                    dimension.SystemValue = weldHeight;
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath);
                }
                //12-30-002           
                base.PartName = $"12-30-{spigotSize.X}.SLDPRT";

           
                if (CheckExistPart != null)
                {
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }

                if(NewPartPath != string.Empty && NewPartPath!= null)
                {                    
                    SolidWorksDocument.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);

                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);

                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    SolidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Вытянуть1@12-30-002.Part")));
                    dimension.SystemValue = w - 0.031;
                    SolidWorksDocument.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая1@12-30-002.Part")));
                    dimension.SystemValue = weldHeight;
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath);
                }
                //12-003

                base.PartName = $"12-03-{spigotSize.X}-{spigotSize.Y}.SLDPRT";
                if (CheckExistPart != null)
                {
                    CheckExistPart(base.PartName, RootFolder, out base.NewPartPath);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("CheckExistPartEvent can not be null", MessageType.Warning);
                }
                
                if(NewPartPath != string.Empty && NewPartPath!= null)
                {
                     
                    SolidWorksDocument.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    assemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true); 
                }
                else
                {

                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    SolidWorksDocument.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз1@12-003.Part")));
                    dimension.SystemValue = w;
                    SolidWorksDocument.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    dimension = ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз1@12-003.Part")));
                    dimension.SystemValue = h;
                    SolidWorksDocument.EditRebuild3();
                    base.SolidWorksDocument.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref this.error, ref this.warning);
                    this.InitiatorSaveExeption(this.error, this.warning, base.NewPartPath);
                    ComponentsPathList.Add(base.NewPartPath); 
                }
            }
            

            SolidWorksDocument.ForceRebuild3(true);
            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, GetSpigotName(type, spigotSize)) + ".SLDASM";
            SolidWorksDocument.Extension.SaveAs(NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, NewPartPath);



            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(base.PartName + ".SLDASM");
            ComponentsPathList.Add(NewPartPath);

            swDrawingSpigot.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)SolidWorksAdapter.AcativeteDoc(drawingName + ".SLDDRW");


            drw.ActivateSheet("DRW1");
            drw.SetupSheet5("DRW1", 12, 12, 1, GetDrawingScale(spigotSize), true, @"\\pdmsrv\SolidWorks Admin\Templates\Основные надписи\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);

            swDrawingSpigot.Extension.SaveAs(NewPartPath.Replace(".SLDASM", ".SLDDRW"), (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
            InitiatorSaveExeption(error, warning, NewPartPath + ".SLDDRW");

            ComponentsPathList.Add(NewPartPath + ".SLDDRW");
            SolidWorksAdapter.CloseAllDocumentsAndExit();
           
            //FinishedBuild(ComponentsPathList);/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            return NewPartPath;
        }

        #region clear document
        /// <summary>
        /// delete equation for model, by index
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
        /// <param name="e_type"></param>
       
        protected override void DeleteComponents(int type)
        {
            SpigotType_e e_type = (SpigotType_e)type;

            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

            if (e_type == SpigotType_e.Twenty_mm)
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
            if (e_type == SpigotType_e.Thirty_mm)
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
        ///  Determinate adn returns spigot name by main params
        /// </summary>
        /// <param name="spigotType">Spigot type</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isShowExtension">Put true if need name and extension </param>
        /// <returns></returns>
        public static  string GetSpigotName(SpigotType_e spigotType, Vector2 size,  bool isShowExtension = false)
        {
            // if we need show extension for example that 
            // a check the availability in the data base
            var spigotName = GetPrototypeName(spigotType) + "-" + size.X + "-" + size.Y;
            if (isShowExtension)
                spigotName += ".SLDASM";
            return spigotName;
        }

        /// <summary>
        /// Determinate model name by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static  string GetPrototypeName(SpigotType_e type)
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