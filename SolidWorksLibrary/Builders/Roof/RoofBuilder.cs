using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidWorksLibrary.Builders.Roof
{
    [Serializable]
        public delegate void CheckExistPartHandler(string partName, out bool isExesitPatrt, out string pathToPartt);
  public  class RoofBuilder
    {

     public   CheckExistPartHandler CheckExistPart;
        List<string> ComponentsPathList;



        /// <summary>
        /// Папка с исходной моделью "Крыши".
        /// </summary>
        private string RoofFolder { get; set; } = @"\15 - Roof";
        /// <summary>
        /// Папка для сохранения компонентов "Крыши". 
        /// </summary>
        private string RoofDestinationFolder { get; set; } = @"\15 - Крыша";

        private string RootFolder = @"D:\TestPDM"; 
        public  RoofBuilder() 
        {
            ComponentsPathList =     new List<string>();
        }
        public string Build(int type, int width, int lenght, bool onlyPath)
        {
            string newPartPath = string.Empty;
            string modelName;
            switch (type)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    modelName = "15-000";
                    break;
                default:
                    modelName = "15-000";
                    break;
            }



            var newRoofName = "15-0" + type + "-" + width + "-" + lenght;
            var newRoofPath = $@"{RootFolder}{RoofDestinationFolder}\{newRoofName}.SLDASM";
            bool IsExistPart = false;
            CheckExistPart(newRoofName+".SLDASM",out IsExistPart,out newRoofPath);
            if (File.Exists(newRoofPath))
            {
                if (onlyPath) return newRoofPath;
                //MessageBox.Show(newRoofPath, "Данная модель уже находится в базе");
                return "";
            }

          //  PDM.SolidWorksPdmAdapter.Instance.GetLastVersionAsmPdm($@"{Settings.Default.SourceFolder}{SpigotFolder}\{"15-000.SLDASM"}");

            var modelRoofPath = $@"{RootFolder}{RoofFolder}\{modelName}.SLDASM";

            //if (!Warning()) return "";
            var swDoc = SolidWorksAdapter.SldWoksApp.OpenDoc6(modelRoofPath, (int)swDocumentTypes_e.swDocASSEMBLY,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel + (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "00", 0, 0);


            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

            #region Удаление ненужного

            if (type == 1 || type == 5)
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
            if (type == 2|| type == 6)
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
            if (type == 3 || type == 4)
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
            if (type == 2 || type == 6)
            {
                addwidth = 75;
                divwidth = 2;
            }
            if (type == 4)
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
                SolidWorksAdapter.SldWoksApp.IActivateDoc2("15-001", true, 0);
                var newPartName = $"15-0{type}-01-{width}-{lenght}";
               
                CheckExistPart(newPartName, out IsExistPart, out newPartPath);
                if ( IsExistPart)
                {
                    swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("15-001-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksApp.CloseDoc("15-001.SLDPRT");
                }
                else
                {
                      newPartPath = $@"{RoofFolder}\{RoofDestinationFolder}\{newPartName}.SLDPRT";

                    SwPartParamsChangeWithNewName("15-001",
                        $@"{RootFolder}\{RoofDestinationFolder}\{newPartName}",
                        new[,]
                        {
                            {"D1@Эскиз1",  type == 5 || type == 6 ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                            {"D2@Эскиз1", Convert.ToString(widthD)},
                            {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                            {"D1@Эскиз27", Convert.ToString(90)},
                            {"D2@Эскиз27", Convert.ToString((75-4.62))},

                            {"D1@Эскиз24", type == 5 || type == 6? Convert.ToString(149.53) : Convert.ToString(9.53)},

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
                        Patterns.Observer.MessageObserver.Instance.SetMessage(e.ToString()) ;
                    }

                    SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
                }


      
            //15-002
            if (type == 6)
            {
                try
                {
                    SolidWorksAdapter.SldWoksApp.IActivateDoc2("15-002", true, 0);
                      newPartName = $"15-0{type}-02-{width}-{lenght}";
                  

                    CheckExistPart(newPartName, out IsExistPart, out newPartPath);
                    if (IsExistPart)
                    {
                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", true, true);
                        SolidWorksAdapter.SldWoksApp.CloseDoc("15-002.SLDPRT");
                    }
                    else
                    {
                          newPartPath =
                         $@"{RootFolder}\{RoofDestinationFolder}\{newPartName}.SLDPRT";
                        SwPartParamsChangeWithNewName("15-002",
                            $@"{RootFolder}\{RoofDestinationFolder}\{newPartName}",
                            new[,]
                            {
                                {"D1@Эскиз1",  type == 5 || type == 6 ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                                {"D2@Эскиз1", Convert.ToString(widthD)},
                                {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                                {"D1@Эскиз27", Convert.ToString(90)},
                                {"D2@Эскиз27", Convert.ToString((75-4.62))},

                                {"D2@Эскиз23", type == 5 || type == 6 ? Convert.ToString(165) : Convert.ToString(25)},

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

                        SolidWorksAdapter.SldWoksApp.CloseDoc(newPartName);
                    }
                    swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("15-001-3@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент2@15-000", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditSuppress2();
                }
                catch (Exception e)
                {
                    Patterns.Observer.MessageObserver.Instance.SetMessage(e.ToString());
                }
            }
            else if (type != 6)
            {
                swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2("15-000.SLDASM", true, 0)));
                swDoc.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

            switch (type)
            {
                case 2:
                case 6:
                    swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
                default:
                    swDoc.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-26@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    break;
            }

            #endregion

            //  GabaritsForPaintingCamera(swDoc);
            try
            {
                swDoc.ForceRebuild3(true);
                Console.WriteLine("newRoofPath " + newRoofPath);
                swDoc.SaveAs2(newRoofPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                newComponents.Add(new FileInfo(newRoofPath));
                SolidWorksAdapter.CloseAllDocumentsAndExit();
                //  PDMWebService.Data.PDM.SolidWorksPdmAdapter.Instance.CheckInOutPdm(newComponents, true);
                Console.WriteLine("RoofBuilder строка 324 пересмотреть CheckInOutPdm");

                foreach (var newComponent in ComponentsPathList)
                {
                    //   ExportXmlSql.Export(newComponent.FullName);
                }

            }
            catch (Exception ex)
            {
                Patterns.Observer.MessageObserver.Instance.SetMessage(ex.ToString());
            }
            if (onlyPath) return newRoofPath;
            //MessageBox.Show(newRoofPath, "Модель построена");

            return newRoofPath;
        }



        protected void SwPartParamsChangeWithNewName(string partName, string newName, string[,] newParams,
            bool newFuncOfAdding, IReadOnlyList<string> copies)
        {

            ModelDoc2 swDoc = null;
            try
            {
                //Логгер.Отладка($"Начало изменения детали {partName}", "", "", "SwPartParamsChangeWithNewName");
                int error = 0;
                int warnings = 0;

                swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksApp.ActivateDoc2(partName + ".SLDPRT", true, 0)));
                var modName = swDoc.GetPathName();
                for (var i = 0; i < newParams.Length / 2; i++)
                {

                    var myDimension = ((Dimension)(swDoc.Parameter(newParams[i, 0] + "@" + partName + ".SLDPRT")));
                    var param = Convert.ToDouble(newParams[i, 1]);
                    var swParametr = param;
                    myDimension.SystemValue = swParametr / 1000;
                    swDoc.EditRebuild3();

                    if (newName == "")
                    {
                        return;
                    }
                    swDoc.EditRebuild3();
                    swDoc.ForceRebuild3(false);

                    if (!newFuncOfAdding)
                    {
                        ComponentsPathList.Add(newName + ".SLDPRT");
                    }

                    if (newFuncOfAdding)
                    {
                        // todo проверить
                        //if (!string.IsNullOrEmpty(copies)) return;
                        //ComponentsPathListFull.Add(new VaultSystem.VentsCadFile
                        //{
                        //    LocalPartFileInfo = new FileInfo(newName + ".SLDPRT").FullName,
                        //    PartIdSql = Convert.ToInt32(newName.Substring(newName.LastIndexOf('-') + 1))
                        //});
                    }


                    swDoc.SaveAs2(new FileInfo(newName + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                        false, true);

                    if (copies != null)
                    {
                        // //MessageBox.Show("copies - " + copies + "  addingInName - " + addingInName);
                        swDoc.SaveAs2(new FileInfo(copies[0] + ".SLDPRT").FullName,
                            (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
                        swDoc.SaveAs2(new FileInfo(copies[1] + ".SLDPRT").FullName,
                            (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
                    }


                    SolidWorksAdapter.SldWoksApp.CloseDoc(newName + ".SLDPRT");
                    //Логгер.Отладка($"Деталь {partName} изменена и сохранена по пути {new FileInfo(newName).FullName}", "",
                    //    "", "SwPartParamsChangeWithNewName");
                    //  //MessageBox.Show("Все хорошо... же.");
                }
            }
            catch (Exception e)
            {

                string param = "с параметрами ";
                foreach (var item in newParams)
                {
                    param += item + ", ";
                }
                //MessageBox.Show(e.ToString() + isNullStr + " для детали " + partName + param);
            }
        }


        protected void VentsMatdll(IList<string> materialP1, IList<string> покрытие, string newName)
        {
           // ModelDoc2 model = SolidWorksInstance.SldWoksApp.ActivateDoc2(newName, true, 0);
           // if (model == null) throw new NullReferenceException("Модель ненайдена");

           // try
           // {
           //     // //MessageBox.Show(newName);

           //   //  var setMaterials = new SetMaterials();
           // //    ToSQL.Conn = Settings.Default.ConnectionToSQL;
           //  //   var toSql = new ToSQL();

           //     // //MessageBox.Show($"Conn - {ToSQL.Conn} SetMaterials {setMaterials == null} toSql - {toSql == null} _swApp {_swApp == null} levelId - {Convert.ToInt32(materialP1[0])}");

           ////     setMaterials?.ApplyMaterial("", "00", Convert.ToInt32(materialP1[0]), SolidWorksInstance.SldWoksApp);
           //   //  model?.Save();

           //     foreach (var confname in setMaterials.GetConfigurationNames(SolidWorksInstance.SldWoksApp))
           //     {
           //         foreach (var matname in setMaterials.GetCustomProperty(confname, SolidWorksInstance.SldWoksApp))
           //         {
           //             toSql.AddCustomProperty(confname, matname.Name, SolidWorksInstance.SldWoksApp);
           //         }
           //     }

           //     if (покрытие != null)
           //     {
           //         if (покрытие[1] != "0")
           //         {
           //             setMaterials.SetColor("00", покрытие[0], покрытие[1], покрытие[2], SolidWorksInstance.SldWoksApp);
           //         }
           //         SolidWorksInstance.SldWoksApp.IActiveDoc2.Save();
           //     }

           //     try
           //     {
           //         string message;
           //         setMaterials.CheckSheetMetalProperty("00", SolidWorksInstance.SldWoksApp, out message);
           //         if (message != null)
           //         {
           //             //  //MessageBox.Show(message, newName + " 858 ");
           //         }
           //     }
           //     catch (Exception e)
           //     {
           //         //MessageBox.Show($"{newName}\n{e.ToString()}\n{e.StackTrace}", "VentsMatdll");
           //     }
           // }
           // catch (Exception e)
           // {
           //     //MessageBox.Show($"{newName}\n{e.ToString()}\n{e.StackTrace}\n{newName}", "VentsMatdll 2");
           // }

           // GabaritsForPaintingCamera(model);

           // model?.Save();
        }

    }
}
