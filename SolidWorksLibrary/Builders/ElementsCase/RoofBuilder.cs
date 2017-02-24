using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using ServiceConstants;

namespace SolidWorksLibrary.Builders.ElementsCase
{ 
    //TO DO loock save as
  
  public  class RoofBuilder : ProductBuilderBehavior    {
        ModelDoc2 solidWorksDocument { get; set; }
        public  RoofBuilder() 
        {
            ComponentsPathList =     new List<string>();
            SetProperties(@"\15 - Крыша", @"\15 - Roof");
        }
        public string Build(RoofType type, int width, int lenght, bool onlyPath)  
        {
            string newPartPath = string.Empty;
            string modelName;
            switch (type)
            {
                case  RoofType.One:
                case  RoofType.Two:
                case  RoofType.Three:
                case  RoofType.Four:
                case  RoofType.Five:
                case  RoofType.Six:
                    modelName = "15-000";
                    break;
                default:
                    modelName = "15-000";
                    break;
            }



            string newRoofName = "15-0" + type + "-" + width + "-" + lenght;
            string newRoofPath = $@"{RootFolder}{SubjectDestinationFolder}\{newRoofName}.SLDASM";
            bool IsExistPart = false;
            CheckExistPart(newRoofName+".SLDASM",out IsExistPart,out newRoofPath);   
            var modelRoofPath = $@"{RootFolder}{SourceFolder}\{modelName}.SLDASM";     
              solidWorksDocument = SolidWorksAdapter.SldWoksAppExemplare.OpenDoc6(modelRoofPath, (int)swDocumentTypes_e.swDocASSEMBLY,
                (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel + (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "00", 0, 0);

            AssemblyDoc solidWorcsAssemvlyDocyment = (AssemblyDoc)solidWorksDocument;
            solidWorcsAssemvlyDocyment.ResolveAllLightWeightComponents(false);
             
            DeleteComponents((int)type);
             

            #region Сохранение и изменение элементов

             var addwidth = 100;
            var addwidth2 = 75;
            var type4 = 0;
            var divwidth = 1;
            if (type ==  RoofType.Two || type ==  RoofType.Six)
            {
                addwidth = 75;
                divwidth = 2;
            }
            if (type ==  RoofType.Four)
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
                SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("15-001", true, 0);
                var newPartName = $"15-0{type}-01-{width}-{lenght}";
               
                CheckExistPart(newPartName, out IsExistPart, out newPartPath);
                if ( IsExistPart)
                {
                    solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("15-000.SLDASM", true, 0)));
                    solidWorksDocument.Extension.SelectByID2("15-001-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorcsAssemvlyDocyment.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-001.SLDPRT");
                }
                else
                {
                      newPartPath = $@"{SourceFolder}\{SubjectDestinationFolder}\{newPartName}.SLDPRT";

                    //EditPartParameters("15-001",
                    //    $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}",
                    //    new[,]
                    //    {
                    //        {"D1@Эскиз1",  type ==  RoofType.Five || type == RoofType.Six ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                    //        {"D2@Эскиз1", Convert.ToString(widthD)},
                    //        {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                    //        {"D1@Эскиз27", Convert.ToString(90)},
                    //        {"D2@Эскиз27", Convert.ToString((75-4.62))},

                    //        {"D1@Эскиз24", type ==  RoofType.Five || type == RoofType.Six? Convert.ToString(149.53) : Convert.ToString(9.53)},

                    //        {"D1@Кривая2", Convert.ToString(weldW2*1000)},
                    //        {"D1@Кривая1", Convert.ToString(weldW*1000)}
                    //    } );
                    try
                    {

                        VentsMatdll(new[] { "1700" }, new[] { "", "Шаргень", "2" }, newPartName);
                    }
                    catch (Exception e)
                    {
                        Patterns.Observer.MessageObserver.Instance.SetMessage(e.ToString()) ;
                    }

                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                }


      
            //15-002
            if (type ==  RoofType.Six)
            {
                try
                {
                    SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("15-002", true, 0);
                      newPartName = $"15-0{type}-02-{width}-{lenght}";
                  

                    CheckExistPart(newPartName, out IsExistPart, out newPartPath);
                    if (IsExistPart)
                    {
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("15-000.SLDASM", true, 0)));
                        solidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        solidWorcsAssemvlyDocyment.ReplaceComponents(newPartPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-002.SLDPRT");
                    }
                    else
                    {
                          newPartPath =
                         $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}.SLDPRT";
                        //EditPartParameters("15-002",
                        //    $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}",
                        //    new[,]
                        //    {
                        //        {"D1@Эскиз1",  type ==  RoofType.Five || type ==  RoofType.Six ? Convert.ToString(140 + lengthD + type4) : Convert.ToString(lengthD + type4)},
                        //        {"D2@Эскиз1", Convert.ToString(widthD)},
                        //        {"D4@Эскиз27", Convert.ToString(addwidth2-4.62)},
                        //        {"D1@Эскиз27", Convert.ToString(90)},
                        //        {"D2@Эскиз27", Convert.ToString((75-4.62))},

                        //        {"D2@Эскиз23", type ==  RoofType.Five || type ==  RoofType.Six ? Convert.ToString(165) : Convert.ToString(25)},

                        //        {"D1@Кривая2", Convert.ToString(weldW2*1000)},
                        //        {"D1@Кривая1", Convert.ToString(weldW*1000)}
                        //    } );
                        try
                        {
                            VentsMatdll(new[] { "1700" }, new[] { "", "Шаргень", "2" }, newPartName);
                        }
                        catch (Exception e)
                        {
                            //MessageBox.Show(e.ToString());
                        }

                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                    }
                    solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("15-000.SLDASM", true, 0)));
                    solidWorksDocument.Extension.SelectByID2("15-001-3@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2@15-000", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocument.EditSuppress2();
                }
                catch (Exception e)
                {
                    Patterns.Observer.MessageObserver.Instance.SetMessage(e.ToString());
                }
            }
            else if (type != RoofType.Six)
            {
                solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("15-000.SLDASM", true, 0)));
                solidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
            }

            switch (type)
            {
                case  RoofType.Two:
                case  RoofType.Six:
                    solidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocument.EditDelete();
                    break;
                default:
                    solidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-26@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocument.EditDelete();
                    break;
            }

            #endregion

            //  GabaritsForPaintingCamera(swDoc);
            try
            {
                solidWorksDocument.ForceRebuild3(true);
                Console.WriteLine("newRoofPath " + newRoofPath);
                solidWorksDocument.SaveAs2(newRoofPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
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

        protected override void DeleteComponents(int type)
        {
            RoofType etype = (RoofType)type;
            if (etype ==  RoofType.One || etype == RoofType.Five)
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                        (int)swDeleteSelectionOptions_e.swDelete_Children;
                solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.Extension.DeleteSelection2(deleteOption);
                solidWorksDocument.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть4@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.DeleteSelection2(deleteOption);

                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
            }
            if (etype ==  RoofType.Two || etype ==  RoofType.Six)
            {
                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.EditUnsuppress2(); solidWorksDocument.ClearSelection2(true);
                solidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2(); solidWorksDocument.ClearSelection2(true);
                solidWorksDocument.Extension.SelectByID2("DerivedCrvPattern2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2(); solidWorksDocument.ClearSelection2(true);
                solidWorksDocument.Extension.SelectByID2("Симметричный1", "MATE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditSuppress2(); solidWorksDocument.ClearSelection2(true);
                solidWorksDocument.Extension.SelectByID2("Расстояние1", "MATE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2();

                solidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Зеркальное отражение1@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Зеркальное отражение1@15-002-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
            }
            if (etype ==  RoofType.Three || etype ==  RoofType.Four)
            {
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                        (int)swDeleteSelectionOptions_e.swDelete_Children;

                solidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();

                solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.Extension.DeleteSelection2(deleteOption);
                solidWorksDocument.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2();
                solidWorksDocument.Extension.SelectByID2("Эскиз23@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditSuppress2();

                solidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditDelete();
                solidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2();
                solidWorksDocument.ClearSelection2(true);
                solidWorksDocument.Extension.SelectByID2("MirrorFasteners - M8", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                solidWorksDocument.EditUnsuppress2();
            }
            solidWorksDocument.ForceRebuild3(false);
        }
    }
}
