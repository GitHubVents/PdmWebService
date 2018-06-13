using SolidWorks.Interop.swconst;
using System;
using ServiceTypes.Constants;
using Patterns.Observer;
using DataBaseDomian;
using VentsMaterials;
using System.Linq;

namespace SolidWorksLibrary.Builders.ElementsCase
{
    /// <summary>
    /// Build the roof and included in its parts
    /// </summary>
    public class RoofBuilder : ProductBuilderBehavior
    {
        
        public  RoofBuilder() 
        {
            SetProperties(@"Проекты\15 - Крыша", @"Библиотека проектирования\DriveWorks\15 - Roof");
            base.NewPartPath = System.IO.Path.Combine(RootFolder, SubjectDestinationFolder, "15-");
        }

        public void Build(int type, int width, int lenght, int materialID, int RALID, string CoatingType, int CoatingClass)
        {
            #region Make calculations

            string hex = string.Empty;
            if (CoatingType != "0")
            {
                hex = SwPlusRepository.Instance.GetRAL().Where(x => x.RALID.Equals(RALID)).Select(x => x.HEX).First();
            }
             

            var addwidth = 100;
            var addwidth2 = 75;
            var type4 = 0;
            var divwidth = 1;
            double lengthD = lenght - 28.5;
            double sketch24 = 9.53;
            double D1Эскиз1 = lengthD + type4;

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
            if ((RoofType_e)type == RoofType_e.Five || (RoofType_e)type == RoofType_e.Six)
            {
                D1Эскиз1 = 140 + lengthD + type4;
                sketch24 = 149.53;
            }

            double widthD = width / divwidth + addwidth;
            
            const double step = 200;
            const double step2 = 150;
            double weldW = Math.Truncate(lenght / step) + 1;
            double weldW2 = Math.Truncate(lenght / step2) + 1;
            double thickness = materialID.Equals(1700) ? 0.7 : 0.8;
           
            SetBends?.Invoke((decimal)thickness, out KFactor, out BendRadius);

            #endregion

            int? ID = 0;
            SwPlusRepository.Instance.AirVents_AddAssemblyRoof(type, width, lenght, 0, materialID, (decimal)thickness, RALID, CoatingType, CoatingClass, ref ID);
            string ASMName = ID.ToString();
            

            base.PartName = "15-000";
            string modelRoofPath = $@"{RootFolder}\{SourceFolder}\{base.PartName}.SLDASM";

            SolidWorksAdapter.OpenDocument(modelRoofPath, swDocumentTypes_e.swDocASSEMBLY, "00");
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(base.PartName + ".SLDASM");
            MessageObserver.Instance.SetMessage("Start to build roof, open model " + SolidWorksDocument?.GetTitle());
            SetMaterials setMat = new SetMaterials(SolidWorksAdapter.SldWoksAppExemplare);



            #region Сохранение и изменение элементов

            DeleteComponents(type);
            //15-001
            #region 

            SwPlusRepository.Instance.AirVents_AddAssemblyRoof(type, widthD.ToInt(), D1Эскиз1.ToInt(), 1, materialID, (decimal)thickness, RALID, CoatingType, CoatingClass, ref ID);

            base.parameters.Add("D1@Эскиз1", D1Эскиз1);
            base.parameters.Add("D2@Эскиз1", widthD);
            base.parameters.Add("Толщина@Листовой металл1", thickness);
            base.parameters.Add("D1@Листовой металл1", (double)BendRadius);
            base.parameters.Add("D2@Листовой металл1", (double)KFactor * 1000);

            

            switch ((RoofType_e)type)
            {
                case RoofType_e.One:
                    base.parameters.Add("D1@Кривая1", weldW * 1000);
                    base.parameters.Add("D1@Эскиз24", sketch24);
                    break;
                case RoofType_e.Two:
                    base.parameters.Add("D1@Кривая1", weldW * 1000);
                    base.parameters.Add("D1@Кривая2", weldW2 * 1000);
                    base.parameters.Add("D1@Эскиз24", sketch24);
                    break;
                case RoofType_e.Three:
                    base.parameters.Add("D4@Эскиз27", addwidth2 - 4.62);
                    base.parameters.Add("D1@Эскиз27", 90);
                    base.parameters.Add("D2@Эскиз27", 75 - 4.62);
                    break;
                case RoofType_e.Four:
                    base.parameters.Add("D4@Эскиз27", addwidth2 - 4.62);
                    base.parameters.Add("D1@Эскиз27", 90);
                    base.parameters.Add("D2@Эскиз27", 75 - 4.62);
                    break;
                case RoofType_e.Five:
                    base.parameters.Add("D1@Кривая1", weldW * 1000);
                    base.parameters.Add("D1@Эскиз24", sketch24);
                    break;
                case RoofType_e.Six:
                    base.parameters.Add("D1@Кривая1", weldW * 1000);
                    base.parameters.Add("D1@Кривая2", weldW2 * 1000);
                    base.parameters.Add("D1@Эскиз24", sketch24);
                    break;
            }

            EditPartParameters("15-001", NewPartPath + ID.ToString() , materialID, hex, CoatingType, CoatingClass.ToString());
            MessageObserver.Instance.SetMessage("Saved 15-001 with new name " + NewPartPath + ID.ToString());
            #endregion

            //15-002 if type = 6
            #region
            if ((RoofType_e)type == RoofType_e.Six)
            {

                SwPlusRepository.Instance.AirVents_AddAssemblyRoof(type, widthD.ToInt(), D1Эскиз1.ToInt(), 2, materialID, (decimal)thickness, RALID, CoatingType, CoatingClass, ref ID);

                base.parameters.Add("D1@Эскиз1", D1Эскиз1);
                base.parameters.Add("D2@Эскиз1", widthD);
                base.parameters.Add("D4@Эскиз27", addwidth2 - 4.62);
                base.parameters.Add("D1@Эскиз27", 90);
                base.parameters.Add("D2@Эскиз27", 75 - 4.62);
                base.parameters.Add("D2@Эскиз23", 165);
                base.parameters.Add("D1@Кривая2", weldW2 * 1000);
                base.parameters.Add("D1@Кривая1", weldW * 1000);
                base.parameters.Add("Толщина@Листовой металл1", thickness);


                EditPartParameters("15-002", NewPartPath + ID.ToString(), materialID, hex,CoatingType, CoatingClass.ToString());
                MessageObserver.Instance.SetMessage("Saved 15-002 with new name " + NewPartPath + ID.ToString());


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
                    //SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-40@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    //SolidWorksDocument.EditDelete();
                    break;
                default:
                    SolidWorksDocument.Extension.SelectByID2("Винт самосверл 6-гр.гол с шайбой-26@15-000", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    break;
            }
            #endregion             
            
            SolidWorksDocument.ForceRebuild3(false);
            int res = SolidWorksDocument.SaveAs2(NewPartPath + ASMName + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            if (res != 0)
            {
                MessageObserver.Instance.SetMessage("ERROR to save asm: " + ((swFileSaveError_e)res).ToString());
            }
            MessageObserver.Instance.SetMessage("\t AssemblyDocument saved with name " + NewPartPath + ASMName + ".SLDASM");
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
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
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
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-1@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-2@15-000", "COMPONENT", 0, 0, 0, true, 0, null, 0);
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


        private void GetSetBends(decimal thikness, out decimal KFactor, out decimal BendRadius)
        {
            KFactor = base.KFactor;
            BendRadius = base.BendRadius;

            try
            {
                var bendList = SwPlusRepository.Instance.Bends;//.Where(x => x.Thickness == thikness).Select(x => new { x.K_Factor, x.BendRadius }).ToList();

                foreach (var item in bendList)
                {
                    if (item.Thickness == thikness)
                    {
                        base.KFactor = item.K_Factor;
                        base.BendRadius = item.BendRadius;
                    }
                }
                MessageObserver.Instance.SetMessage("KFactor: " + KFactor);
            }
            catch (Exception)
            {
                MessageObserver.Instance.SetMessage("Faild to GetSetBends");//////////////////////////////////////////////////
            }
        }

    }
}