using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using ServiceTypes.Constants;


namespace SolidWorksLibrary.Builders.ElementsCase
{
    /// <summary>
    /// Build the roof and included in its parts
    /// </summary>
    public class RoofBuilder : ProductBuilderBehavior
    {
     
        public  RoofBuilder() 
        {
            ComponentsPathList = new List<string>();
            SetProperties(@"Проекты\15 - Крыша", @"Библиотека проектирования\DriveWorks\15 - Roof");
        }

        public string Build(int type, double width, double lenght, bool onlyPath)
        {
            SolidWorksDocument = null;
            base.PartName = "15-000";
            base.NewPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{base.PartName}";
            string modelRoofPath = $@"{RootFolder}\{SourceFolder}\{base.PartName}.SLDASM";

            SolidWorksAdapter.OpenDocument(modelRoofPath, swDocumentTypes_e.swDocASSEMBLY, "00");
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(base.PartName + ".SLDASM");
            
            DeleteComponents(type);

            #region Сохранение и изменение элементов

            var addwidth = 100;
            var addwidth2 = 75;
            var type4 = 0;
            var divwidth = 1;
            if ((RoofType_e)type == RoofType_e.Two || (RoofType_e)type == RoofType_e.Six)
            {
                addwidth = 75;
                divwidth = 2;
            }
            else if ((RoofType_e)type == RoofType_e.Four)
            {
                type4 = 170;
                addwidth2 = 170 + 75;
            }

            double widthD = width / divwidth + addwidth;
            double lengthD = lenght - 28.5;
            const double step = 200;
            const double step2 = 150;
            double weldW = Math.Truncate(lenght / step) + 1;
            double weldW2 = Math.Truncate(lenght / step2) + 1;

            //15-001

            #region 

            string newPartName = $"15-0{type}-01-{width}-{lenght}";
            AssemblyName = $"15-0{type}-01-{width}-{lenght}.SLDASM";
            //CheckExistPart(NewPartPath, RootFolder, out NewPartPath);
            if (base.IsPartExist)
            {
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                SolidWorksDocument.Extension.SelectByID2("15-001-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                base.AssemblyDocument.ReplaceComponents(AssemblyName, "", true, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-001.SLDPRT");
            }
            else
            {
                base.NewPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";

                base.parameters.Add("D1@Эскиз1", (RoofType_e)type == RoofType_e.Five || (RoofType_e)type == RoofType_e.Six ? (140 + lengthD + type4) : (lengthD + type4));
                base.parameters.Add("D2@Эскиз1", widthD);
                base.parameters.Add("D4@Эскиз27", addwidth2 - 4.62);
                base.parameters.Add("D1@Эскиз27", 90);
                base.parameters.Add("D2@Эскиз27", 75 - 4.62);
                base.parameters.Add("D1@Эскиз24", ((RoofType_e)type == RoofType_e.Five || (RoofType_e)type == RoofType_e.Six) ? 149.53 : 9.53);
                base.parameters.Add("D1@Кривая2", weldW2 * 1000);
                base.parameters.Add("D1@Кривая1", weldW * 1000);

                EditPartParameters("15-001", NewPartPath);
               
            }
            #endregion

            //15-002 if type = 6
            #region
            if ((RoofType_e)type == RoofType_e.Six)
            {
               
                newPartName = $"15-06-02-{width}-{lenght}";

                //CheckExistPart(NewPartPath, RootFolder, out NewPartPath);
                if (base.IsPartExist)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    base.AssemblyDocument.ReplaceComponents(AssemblyName, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("15-002.SLDPRT");
                }
                else
                {
                    base.NewPartPath = $@"{RootFolder}\{SubjectDestinationFolder}\{newPartName}";

                    base.parameters.Add("D1@Эскиз1", 140 + lengthD + type4);
                    base.parameters.Add("D2@Эскиз1", widthD);

                    base.parameters.Add("D4@Эскиз27", addwidth2 - 4.62);
                    base.parameters.Add("D1@Эскиз27", 90);
                    base.parameters.Add("D2@Эскиз27", 75 - 4.62);

                    base.parameters.Add("D2@Эскиз23", 165);
                    base.parameters.Add("D1@Кривая2", weldW2 * 1000);
                    base.parameters.Add("D1@Кривая1", weldW * 1000);

                    EditPartParameters("15-002", NewPartPath);
                    
                }
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                SolidWorksDocument.Extension.SelectByID2("15-001-3@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2@15-000", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
            }
            else // если тип не == 6, удаляем деталь 15-002
            {
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("15-000.SLDASM");
                SolidWorksDocument.Extension.SelectByID2("15-002-1@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }
            #endregion


            switch ((RoofType_e)type)
            {
                case RoofType_e.Two:
                case RoofType_e.Six:
                    SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-33@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-40@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
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
                SolidWorksDocument.SaveAs2(AssemblyName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                ComponentsPathList.Add(base.NewPartPath);
                //SolidWorksAdapter.CloseAllDocumentsAndExit();
            }
            catch (Exception ex)
            {
                Patterns.Observer.MessageObserver.Instance.SetMessage(ex.ToString());
            }
            if (onlyPath) return base.NewPartPath;

            return base.NewPartPath;
        }

        protected override void DeleteComponents(int type)
        {
            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

            if ((RoofType_e)type == RoofType_e.One || (RoofType_e)type == RoofType_e.Five)
            {
                //15-001-1

                SolidWorksDocument.Extension.SelectByID2("Эскиз26@15-001-1@15-000", "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hole-M8@15-001-1@15-000", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть3@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть4@15-001-1@15-000", "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                //COMPONENT
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Fasteners - M8", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 11371_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Hex Bolt 7805_gost-2@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }
            else if ((RoofType_e)type == RoofType_e.Two || (RoofType_e)type == RoofType_e.Six)
            {
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.ClearSelection2(true);

                SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.ClearSelection2(true);

                SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern2", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.ClearSelection2(true);

                SolidWorksDocument.Extension.SelectByID2("Симметричный1", "MATE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                SolidWorksDocument.ClearSelection2(true);

                SolidWorksDocument.Extension.SelectByID2("Расстояние1", "MATE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.ClearSelection2(true);

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
            else if ((RoofType_e)type ==  RoofType_e.Three || (RoofType_e)type ==  RoofType_e.Four)
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