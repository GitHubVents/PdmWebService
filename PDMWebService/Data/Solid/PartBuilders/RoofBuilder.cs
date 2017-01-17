using PDMWebService.Properties;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDMWebService.Data.Solid.PartBuilders
{
    class RoofBuilder : AbstractBuilder
    {

        /// <summary>
        ///  Папка с исходной моделью "Вибровставки". 
        /// </summary>
        public string SpigotFolder { get; set; } = @"\Библиотека проектирования\DriveWorks\12 - Spigot";
        /// <summary>
        /// Папка с исходной моделью "Крыши".
        /// </summary>
        private string RoofFolder { get; set; } = @"\Библиотека проектирования\DriveWorks\15 - Roof";
        /// <summary>
        /// Папка для сохранения компонентов "Крыши". 
        /// </summary>
        private string RoofDestinationFolder { get; set; } = @"\Проекты\Blauberg\15 - Крыша";


        private RoofBuilder() : base()
        {

        }
        public string Build(string type, string width, string lenght, bool onlyPath)
        {
            string modelName;
            switch (type)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                    modelName = "15-000";
                    break;
                default:
                    modelName = "15-000";
                    break;
            }



            var newRoofName = "15-0" + type + "-" + width + "-" + lenght;
            var newRoofPath = $@"{Settings.Default.DestinationFolder}{RoofDestinationFolder}\{newRoofName}.SLDASM";

            if (File.Exists(newRoofPath))
            {
                if (onlyPath) return newRoofPath;
                //MessageBox.Show(newRoofPath, "Данная модель уже находится в базе");
                return "";
            }

            PDM.PDMAdapter.Instance.GetLastVersionAsmPdm($@"{Settings.Default.SourceFolder}{SpigotFolder}\{"15-000.SLDASM"}");

            var modelRoofPath = $@"{Settings.Default.SourceFolder}{RoofFolder}\{modelName}.SLDASM";

            //if (!Warning()) return "";
            var swDoc = SolidWorksInstance.SldWoksApp.OpenDoc6(modelRoofPath, (int)swDocumentTypes_e.swDocASSEMBLY,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);


            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            #region Удаление ненужного

            if (type == "1" || type == "5")
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                        (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
                swDoc.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Вырез-Вытянуть4@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);

                swDoc.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }
            if (type == "2" || type == "6")
            {
                swDoc.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditUnsuppress2(); swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2(); swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("DerivedCrvPattern2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2(); swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("Симметричный1", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2(); swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("Расстояние1", "MATE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();

                swDoc.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Зеркальное отражение1@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Зеркальное отражение1@15-002-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }
            if (type == "3" || type == "4")
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                        (int)swDeleteSelectionOptions_e.swDelete_Children;

                swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();

                swDoc.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                swDoc.Extension.DeleteSelection2(deleteOption);
                swDoc.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.Extension.SelectByID2("Эскиз23@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                swDoc.EditSuppress2();

                swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
                swDoc.ClearSelection2(true);
                swDoc.Extension.SelectByID2("MirrorFasteners - M8", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                swDoc.EditUnsuppress2();
            }
            swDoc.ForceRebuild3(false);

            #endregion

            #region Сохранение и изменение элементов

            var addwidth = 100;
            var addwidth2 = 75;
            var type4 = 0;
            var divwidth = 1;
            if (type == "2" || type == "6")
            {
                addwidth = 75;
                divwidth = 2;
            }
            if (type == "4")
            {
                type4 = 170;
                addwidth2 = 170 + 75;
            }

            var widthD = (Convert.ToDouble(width) / divwidth + addwidth);
            var lengthD = (Convert.ToDouble(lenght) - 28.5);
            const double step = 200;
            const double step2 = 150;
            var weldW = Convert.ToDouble((Math.Truncate(Convert.ToDouble(lenght) / step) + 1));
            var weldW2 = Convert.ToDouble((Math.Truncate(Convert.ToDouble(lenght) / step2) + 1));
            var newComponents = new List<FileInfo>();

            //15-001
            try
            {
                SolidWorksInstance.SldWoksApp.IActivateDoc2("15-001", true, 0);
                var newPartName = $"15-0{type}-01-{width}-{lenght}";
                var newPartPath = $@"{Settings.Default.DestinationFolder}\{RoofDestinationFolder}\{newPartName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("15-001-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("15-001.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("15-001",
                        $@"{Settings.Default.DestinationFolder}\{RoofDestinationFolder}\{newPartName}",
                        new[,]
                        {
                            {"D1@Эскиз1",  type == "5" || type == "6" ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                            {"D2@Эскиз1", Convert.ToString(widthD)},
                            {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                            {"D1@Эскиз27", Convert.ToString(90)},
                            {"D2@Эскиз27", Convert.ToString((75-4.62))},

                            {"D1@Эскиз24", type == "5" || type == "6" ? Convert.ToString(149.53) : Convert.ToString(9.53)},

                            {"D1@Кривая2", Convert.ToString(weldW2*1000)},
                            {"D1@Кривая1", Convert.ToString(weldW*1000)}
                        },
                        false,
                        null);
                    try
                    {
                        
                        VentsMatdll(new[] { "1700" }, new[] { "", "Шаргень", "2" }, newPartName);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.ToString());
                    }

                    SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                }
            }
            catch (Exception e)
            {
                //Logger.Log(LogLevel.Error, string.Format("Во время изменения детали 15-001.SLDPRT произошла ошибка"), e);
                //MessageBox.Show(e.ToString());
            }

            //15-002
            if (type == "6")
            {
                try
                {
                    SolidWorksInstance.SldWoksApp.IActivateDoc2("15-002", true, 0);
                    var newPartName = $"15-0{type}-02-{width}-{lenght}";
                    var newPartPath =
                        $@"{Settings.Default.DestinationFolder}\{RoofDestinationFolder}\{newPartName}.SLDPRT";
                    if (File.Exists(newPartPath))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", true, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("15-002.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("15-002",
                            $@"{Settings.Default.DestinationFolder}\{RoofDestinationFolder}\{newPartName}",
                            new[,]
                            {
                                {"D1@Эскиз1",  type == "5" || type == "6" ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                                {"D2@Эскиз1", Convert.ToString(widthD)},
                                {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                                {"D1@Эскиз27", Convert.ToString(90)},
                                {"D2@Эскиз27", Convert.ToString((75-4.62))},

                                {"D2@Эскиз23", type == "5" || type == "6" ? Convert.ToString(165) : Convert.ToString(25)},

                                {"D1@Кривая2", Convert.ToString(weldW2*1000)},
                                {"D1@Кривая1", Convert.ToString(weldW*1000)}
                            },
                            false,
                        null);
                        try
                        {
                            VentsMatdll(new[] { "1700" }, new[] { "", "Шаргень", "2" }, newPartName);
                        }
                        catch (Exception e)
                        {
                            //MessageBox.Show(e.ToString());
                        }

                        SolidWorksInstance.SldWoksApp.CloseDoc(newPartName);
                    }
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("15-001-3@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент2@15-000", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress2();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }
            }
            else if (type != "6")
            {
                swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            switch (type)
            {
                case "2":
                case "6":
                    swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
                default:
                    swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-26@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
            }

            #endregion

            GabaritsForPaintingCamera(swDoc);

            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(newRoofPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            newComponents.Add(new FileInfo(newRoofPath));
            SolidWorksInstance.CloseAllDocumentsAndExit();
            PDMWebService.Data.PDM.PDMAdapter.Instance.CheckInOutPdm(newComponents, true);

            foreach (var newComponent in NewComponents)
            {
                //   ExportXmlSql.Export(newComponent.FullName);
            }

            if (onlyPath) return newRoofPath;
            //MessageBox.Show(newRoofPath, "Модель построена");

            return newRoofPath;
        }
    }
}
