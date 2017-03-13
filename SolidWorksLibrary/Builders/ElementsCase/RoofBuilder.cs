using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using ServiceConstants;


namespace SolidWorksLibrary.Builders.ElementsCase
{
    /// <summary>
    /// Build the roof and included in its parts
    /// </summary>
    public class RoofBuilder : ProductBuilderBehavior    {
     
        public  RoofBuilder() 
        {
            ComponentsPathList =     new List<string>();
            SetProperties(@"\15 - Крыша", @"\15 - Roof");
        }

        public string Build(RoofType_e type, int width, int lenght, bool onlyPath)  
        {
            switch (type)
            {
                case RoofType_e.One:
                case RoofType_e.Two:
                case RoofType_e.Three:
                case RoofType_e.Four:
                case RoofType_e.Five:
                case RoofType_e.Six:
               base.PartName = "15-000";
                break;
            }
            
              base.PartName = "15-0" + type + "-" + width + "-" + lenght;
              base.NewPartPath = $@"{RootFolder}{SubjectDestinationFolder}\{base.PartName}.SLDASM"; 
            CheckExistPart(base.PartName+".SLDASM",out  base.IsPartExist,out base.NewPartPath);   
            var modelRoofPath = $@"{RootFolder}{SourceFolder}\{base.PartName}.SLDASM";
            SolidWorksDocument = SolidWorksAdapter.OpenDocument(modelRoofPath, swDocumentTypes_e.swDocASSEMBLY); 

            AssemblyDoc solidWorcsAssemvlyDocyment = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);              
            DeleteComponents((int)type);            

            #region Сохранение и изменение элементов

             var addwidth = 100;
            var addwidth2 = 75;
            var type4 = 0;
            var divwidth = 1;
            if (type ==  RoofType_e.Two || type ==  RoofType_e.Six)
            {
                addwidth = 75;
                divwidth = 2;
            }
            if (type ==  RoofType_e.Four)
            {
                type4 = 170;
                addwidth2 = 170 + 75;
            }

            double widthD = (Convert.ToDouble(width) / divwidth + addwidth);
            double lengthD = (Convert.ToDouble(lenght) - 28.5);
            const double step = 200;
            const double step2 = 150;
            double weldW = Convert.ToDouble((Math.Truncate(Convert.ToDouble(lenght) / step) + 1));
            double weldW2 = Convert.ToDouble((Math.Truncate(Convert.ToDouble(lenght) / step2) + 1)); 

            //15-001
                SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("15-001", true, 0);
                var newPartName = $"15-0{type}-01-{width}-{lenght}";
               
                CheckExistPart(newPartName, out base.IsPartExist, out base.NewPartPath);
                if ( base.IsPartExist)
                {
                    SolidWorksDocument =  SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("15-001-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorcsAssemvlyDocyment.ReplaceComponents(base.NewPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-001.SLDPRT");
                }
                else
                {
                      base.NewPartPath = $@"{SourceFolder}\{SubjectDestinationFolder}\{newPartName}.SLDPRT";

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
            if (type ==  RoofType_e.Six)
            {
                try
                {
                    SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("15-002", true, 0);
                      newPartName = $"15-0{type}-02-{width}-{lenght}";
                  

                    CheckExistPart(newPartName, out base.IsPartExist, out base.NewPartPath);
                    if (base.IsPartExist)
                    {
                        SolidWorksDocument =  SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        solidWorcsAssemvlyDocyment.ReplaceComponents(base.NewPartPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-002.SLDPRT");
                    }
                    else
                    {
                          base.NewPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}.SLDPRT";
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
                        }

                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPartName);
                    }
                    SolidWorksDocument =  SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("15-001-3@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2@15-000", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                }
                catch (Exception e)
                {
                    Patterns.Observer.MessageObserver.Instance.SetMessage(e.ToString());
                }
            }
            else if (type != RoofType_e.Six)
            {
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("15-000.SLDASM" );
                SolidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }

            switch (type)
            {
                case  RoofType_e.Two:
                case  RoofType_e.Six:
                    SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    break;
                default:
                    SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-26@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    break;
            }

            #endregion             
            try
            {
                SolidWorksDocument.ForceRebuild3(true); 
                SolidWorksDocument.SaveAs2(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                ComponentsPathList.Add( base.NewPartPath);
                SolidWorksAdapter.CloseAllDocumentsAndExit();
            }
            catch (Exception ex)
            {
                Patterns.Observer.MessageObserver.Instance.SetMessage(ex.ToString());
            }
            if (onlyPath) return base.NewPartPath;

            return base.NewPartPath;
        }
          
        protected void VentsMatdll(IList<string> materialP1, IList<string> покрытие, string newName)
        {
           // ModelDoc2 model = SolidWorksInstance.SldWoksApp.ActivateDoc2(newName, true, 0);
           // if (model == null) throw new NullReferenceException("Модель ненайдена");

           // try
           // { 

           //   //  var setMaterials = new SetMaterials();  
           ////     setMaterials?.ApplyMaterial("", "00", Convert.ToInt32(materialP1[0]), SolidWorksInstance.SldWoksApp);
           //   //  model?.Save();
        

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
           //         }
           //     }
           //     catch (Exception e)
           //     {
           //     }
           // }
           // catch (Exception e)
           // {
           // }
            

           // model?.Save();
        }

        protected override void DeleteComponents(int type)
        {
            RoofType_e etype = (RoofType_e)type;


            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                        (int)swDeleteSelectionOptions_e.swDelete_Children;

            if (etype ==  RoofType_e.One || etype == RoofType_e.Five)
            {
                
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                SolidWorksDocument.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть4@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }
            if (etype ==  RoofType_e.Two || etype ==  RoofType_e.Six)
            {
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2(); SolidWorksDocument.ClearSelection2(true);
                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2(); SolidWorksDocument.ClearSelection2(true);
                SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2(); SolidWorksDocument.ClearSelection2(true);
                SolidWorksDocument.Extension.SelectByID2("Симметричный1", "MATE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2(); SolidWorksDocument.ClearSelection2(true);
                SolidWorksDocument.Extension.SelectByID2("Расстояние1", "MATE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();

                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Зеркальное отражение1@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Зеркальное отражение1@15-002-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }
            if (etype ==  RoofType_e.Three || etype ==  RoofType_e.Four)
            { 

                SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();

                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                SolidWorksDocument.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("Эскиз23@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.ClearSelection2(true);
                SolidWorksDocument.Extension.SelectByID2("MirrorFasteners - M8", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
            SolidWorksDocument.ForceRebuild3(false);
        }
    }
}
