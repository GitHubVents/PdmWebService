using PDMWebService.Properties;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.Solid.PartBuilders
{
    public class VibroInsertionBuilder : AbstractBuilder 
    {
        private List<FileInfo> NewComponents;
        /// <summary>
        ///  Папка для сохранения компонентов "Вибровставки". 
        /// </summary>
        public string SpigotDestinationFolder { get; set; } = @"\Проекты\Blauberg\12 - Вибровставка";
        /// <summary>
        ///  Папка с исходной моделью "Вибровставки". 
        /// </summary>
        public string SpigotFolder { get; set; } = @"\Библиотека проектирования\DriveWorks\12 - Spigot";

        public VibroInsertionBuilder() :base()
        {

        }

        public string Build(string type, string width, string height)
        {
            if (DataConverter.IsConvertToInt(new[] { width, height }) == false)
            {
                return "";
            }
            string modelName;

            switch (type)
            {
                case "20":
                    modelName = "12-20";
                    break;
                case "30":
                    modelName = "12-30";
                    break;
                default:
                    modelName = "12-00";
                    break;
            }

            var newSpigotName = modelName + "-" + width + "-" + height;
            var newSpigotPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newSpigotName}";

            if (File.Exists(newSpigotPath + ".SLDDRW"))
            {
                //MessageBox.Show(newSpigotPath + ".SLDDRW", "Данная модель уже находится в базе");
                return "";
            }

            var drawing = "12-00";
            if (modelName == "12-30")
            {
                drawing = modelName;
            }
            Dimension myDimension;
            var modelSpigotDrw = $@"{Settings.Default.SourceFolder}{SpigotFolder}\{drawing}.SLDDRW";

          PDM.PDMAdapter.Instance.GetLastVersionAsmPdm(modelSpigotDrw );

            //if (!Warning()) return "";
            var swDrwSpigot = SolidWorksInstance.SldWoksApp.OpenDoc6(modelSpigotDrw, (int)swDocumentTypes_e.swDocDRAWING,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);
            SolidWorksInstance.SldWoksApp.Visible = true;

            ModelDoc2 swDoc = SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00", false, 0);
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            switch (modelName)
            {
                case "12-20":
                     DelEquations(5, swDoc);
                    DelEquations(4, swDoc);
                     DelEquations(3, swDoc);
                    break;
                case "12-30":
                     DelEquations(0, swDoc);
                     DelEquations(0, swDoc);
                    DelEquations(0, swDoc);
                    break;
            }
            swDoc.ForceRebuild3(true);

            string newPartName;
            string newPartPath;
            IModelDoc2 swPartDoc;

            #region Удаление ненужного

            if (type == "20")
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-30-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.002-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
            }
            if (type == "30")
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-2@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-001-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-3@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-20-002-4@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-5@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-6@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-7@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("ВНС-96.61.001-8@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Клей-1@12-00", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
            }

            swDoc.Extension.SelectByID2("30", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            swDoc.EditDelete();
            swDoc.Extension.SelectByID2("20", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            swDoc.EditDelete();

            #endregion

            #region Сохранение и изменение элементов

            var addDimH = 1;
            if (modelName == "12-30")
            {
                addDimH = 10;
            }

            var w = (Convert.ToDouble(width) - 1) / 1000;
            var h = Convert.ToDouble((Convert.ToDouble(height) + addDimH) / 1000);
            const double step = 50;
            var weldW = Convert.ToDouble((Math.Truncate(Convert.ToDouble(width) / step) + 1));
            var weldH = Convert.ToDouble((Math.Truncate(Convert.ToDouble(height) / step) + 1));

            if (modelName == "12-20")
            {
                //12-20-001
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-20-001", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-20-{height}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-20-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-20-001.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-20-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-20-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-20-001.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }

                //12-20-002
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-20-002", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-20-{width}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-20-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-20-002.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-20-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-20-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-20-002.Part")));
                    myDimension.SystemValue = weldW;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }

                //12-003
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-003", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-003-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D3@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    swDoc.Extension.SelectByID2("D2@Эскиз1@12-003-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    swDoc.EditRebuild3();
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }
            }

            if (modelName == "12-30")
            {
                //12-30-001
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-30-001", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-30-{height}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-30-001-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-30-001.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-30-001.Part")));
                    myDimension.SystemValue = h - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-30-001-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-30-001.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }

                //12-30-002
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-30-002", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-30-{width}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-30-002-1@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-30-002.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D1@Вытянуть1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@12-30-002.Part")));
                    myDimension.SystemValue = w - 0.031;
                    swDoc.Extension.SelectByID2("D1@Кривая1@12-30-002-1@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D1@Кривая1@12-30-002.Part")));
                    myDimension.SystemValue = weldH;
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }

                //12-003
                SolidWorksInstance.SldWoksApp.IActivateDoc2("12-003", false, 0);
                swPartDoc = SolidWorksInstance.SldWoksApp.IActiveDoc2;
                newPartName = $"12-03-{width}-{height}.SLDPRT";
                newPartPath = $@"{Settings.Default.DestinationFolder}\{SpigotDestinationFolder}\{newPartName}";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("12-00.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("12-003-2@12-00", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("12-003.SLDPRT");
                }
                else
                {
                    swDoc.Extension.SelectByID2("D3@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D3@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = w;
                    swDoc.Extension.SelectByID2("D2@Эскиз1@12-003-2@12-00", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    myDimension = ((Dimension)(swDoc.Parameter("D2@Эскиз1@12-003.Part")));
                    myDimension.SystemValue = h;
                    swDoc.EditRebuild3();
                    swPartDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                    NewComponents.Add(new FileInfo(newPartPath));
                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }
            }

            #endregion

            GabaritsForPaintingCamera(swDoc);


            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(newSpigotPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksInstance.SldWoksApp.CloseDoc(newSpigotName + ".SLDASM");
            NewComponents.Add(new FileInfo(newSpigotPath + ".SLDASM"));
            swDrwSpigot.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)(SolidWorksInstance.SldWoksApp.IActivateDoc3(drawing + ".SLDDRW", true, 0));
            drw.ActivateSheet("DRW1");
            var m = 5;
            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500)
            { m = 10; }
            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850)
            { m = 15; }
            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250)
            { m = 20; }
            drw.SetupSheet5("DRW1", 12, 12, 1, m, true, Settings.Default.DestinationFolder + @"\Vents-PDM\\Библиотека проектирования\\Templates\\Основные надписи\\A3-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);
            //swDrwSpigot.SaveAs2(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            var errors = 0;
            var warnings = 0;

            swDrwSpigot.SaveAs4(newSpigotPath + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref errors, ref warnings);

            NewComponents.Add(new FileInfo(newSpigotPath + ".SLDDRW"));
            // //MessageBox.Show(new FileInfo(newSpigotPath + ".SLDDRW").Name);
            SolidWorksInstance.CloseAllDocumentsAndExit();
            //MessageBox.Show(newSpigotPath, "Модель построена");

             //  PDMWebService.Data.PDM.PDMAdapter.Instance.CheckInOutPdm(NewComponents, true);

            return newSpigotPath;
        }



    }
}
