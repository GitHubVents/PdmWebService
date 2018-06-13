using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using System;
using ServiceTypes.Constants;
using SolidWorksLibrary.Builders.ElementsCase;
using System.IO;
using DataBaseDomian;

namespace PDMWebService.Data.Solid.ElementsCase
{
   public class FlapBuilder : ProductBuilderBehavior
   {
        int errores = 0, warnings = 0;
        public FlapBuilder() : base()
        { 
            base.SetProperties(@"Проекты\11 - Регулятор расхода воздуха", @"Библиотека проектирования\DriveWorks\11 - Damper");
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, @"11-");
        }
        

        public void Build(FlapTypes_e flapType, Vector2 flapSize, bool isOutDoor, int materialID, double thickness)
        {
            double addH = 0;
            switch (flapType)
            {
                case FlapTypes_e.Twenty_mm:
                    PartName = "11-20";
                    AssemblyName = "11 - Damper";
                    addH = 0;
                    break;
                case FlapTypes_e.Thirty_mm:
                    PartName = "11-30";
                    AssemblyName = "11-30";
                    addH = -8;
                    break;
            }

            string modelType = string.Empty;

            string drawingName = "11-20";
            if (PartName == "11-30") { drawingName = PartName; }

            string modelDRWPath = $@"{RootFolder}\{SourceFolder}\{drawingName}";
            string modelLamel = $@"{RootFolder}\{base.SourceFolder}\{"11-100"}.SLDDRW";
            string modelPath = $@"{RootFolder}\{base.SourceFolder}\{base.AssemblyName}.SLDASM";

            SolidWorksAdapter.OpenDocument(modelDRWPath + ".SLDDRW",  swDocumentTypes_e.swDocDRAWING);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(base.AssemblyName + ".SLDASM");


            int? ID = 0;
            SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, flapSize.X.ToInt(), flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 0, ref ID);
            string newDamperName = ID.ToString();



            #region // Габариты
            SetBends?.Invoke((decimal)thickness, out base.KFactor, out base.BendRadius);
            // Количество лопастей
            double countL = (Math.Truncate(flapSize.Y / 100)) * 1000;
            // Шаг заклепок
            const double step = 140;
            double rivetW = (Math.Truncate(flapSize.X / step)+ 1) * 1000;
            double rivetH = (Math.Truncate((flapSize.Y + addH) / step) + 1) * 1000;
            // Высота уголков
            double hC = Math.Truncate(7 + 5.02 + (flapSize.Y - countL / 10 - 10.04) / 2);
            #endregion



            #region typeOfFlange = "20"
            if (flapType == FlapTypes_e.Twenty_mm)
            {
                if ((countL / 1000) % 2 == 1) //нечетное
                {
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                }


                if (isOutDoor)
                {
                    //Delete Components
                    SolidWorksDocument.Extension.SelectByID2("Эскиз1", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть10@11-001-7@11 - Damper", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз34@11-001-7@11 - Damper", "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("ВНС-901.41.302-1@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-130@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-131@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-129@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-128@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-127@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-126@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    ///////
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-207@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-209@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-221@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-222@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-223@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-224@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-225@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-226@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-227@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-228@11-003@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();


                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть1@11-003@11 - Damper", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern3", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditSuppress2();


                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 5, ref ID);
                    //Replace Component
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-003-6@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString(), "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-003.SLDPRT");

                    // 11-005 
                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-005-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString() + ".SLDPRT", "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {
                        base.parameters.Add("D3@Эскиз1", flapSize.Y);
                        base.parameters.Add("D1@Кривая1", rivetH);
                        base.parameters.Add("D3@Эскиз37", Convert.ToInt32(countL / 1000) % 2 == 1 ? 0 : 50);

                        base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                        base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                        base.parameters.Add("Толщина@Листовой металл", thickness);

                        EditPartParameters("11-005", base.NewPartPath + ID.ToString(), materialID);
                    }


                    // 11-006 
                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 6, ref ID);

                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-006-1@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath + ".SLDPRT", "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        base.parameters.Add("D3@Эскиз1", flapSize.Y);
                        base.parameters.Add("D1@Кривая1", rivetH);
                        base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                        base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                        base.parameters.Add("Толщина@Листовой металл", thickness);

                        EditPartParameters("11-006", base.NewPartPath + ID.ToString(), materialID);
                    }
                }
                else
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");

                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@11-001-7@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть12@11-001-7@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз35@11-001-7@11 - Damper", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз36@11-001-7@11 - Damper", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть10@11-003-6@11 - Damper", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.DeleteSelection(true);
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@11-003-6@11 - Damper", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.DeleteSelection(true);
                    SolidWorksDocument.Extension.SelectByID2("Эскиз34@11-003-6@11 - Damper", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз35@11-003-6@11 - Damper", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-005-1@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-006-1@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-187@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-188@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-189@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-190@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-191@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-192@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-193@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-194@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-195@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-196@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-197@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-198@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.EditDelete();
                }

                // 11-002
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, (flapSize.X - 0.96).ToInt(), 0, isOutDoor, materialID, (decimal)thickness, 2, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-002-4@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath + ".SLDPRT", "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-002.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.X - 0.96);
                    base.parameters.Add("D1@Кривая1", rivetW);

                    base.parameters.Add("D1@Листовой металл1", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл1", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл1", thickness);

                    EditPartParameters("11-002", base.NewPartPath + ID.ToString(), materialID);
                }

                // 11-003
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 7.94 + addH ).ToInt(), isOutDoor, materialID, (decimal)thickness, 3, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-003-6@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath + ".SLDPRT", "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-003.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.Y + 7.94);
                    base.parameters.Add("D1@Эскиз27", Math.Truncate(countL / 10 - 100));
                    base.parameters.Add("D1@Кривая1", countL);
                    if (isOutDoor){ base.parameters.Add("D1@Кривая2", rivetH);}

                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", thickness);

                    EditPartParameters("11-003", base.NewPartPath + ID.ToString(), materialID);
                }

                // 11-001
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 7.94 + addH ).ToInt(), isOutDoor, materialID, (decimal)thickness, 1, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-001-7@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath + ".SLDPRT", "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-001.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.Y + 7.94);
                    base.parameters.Add("D1@Эскиз27", (countL / 10 - 100));
                    base.parameters.Add("D1@Кривая1", countL);
                    if (isOutDoor) { base.parameters.Add("D1@Кривая2", rivetH); }

                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", thickness);

                    EditPartParameters("11-001", base.NewPartPath + ID.ToString(), materialID);
                }

                // 11-004
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, (flapSize.X - 24).ToInt(), hC.ToInt(), isOutDoor, materialID, (decimal)thickness, 4, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-004-1@11 - Damper", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath + ".SLDPRT", "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-004.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.X - 24);
                    base.parameters.Add("D7@Ребро-кромка1", hC);
                    base.parameters.Add("D1@Кривая1", rivetW);
                    base.parameters.Add("D1@Эскиз8", 18.5);
                    base.parameters.Add("D1@Эскиз9", 18.5);
                    base.parameters.Add("D1@Листовой металл1", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл1", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл1", thickness);

                    EditPartParameters("11-004", base.NewPartPath + ID.ToString(), materialID);
                }


                #region 11-100 Сборка лопасти

                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, (flapSize.X - 23).ToInt(), 0, isOutDoor, materialID, (decimal)thickness, 12, ref ID);
                string newNameAsm = ID.ToString(); //имя сборки лопасти

                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-100-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(NewPartPath + ID.ToString() + ".SLDASM", "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                }
                else
                {
                    #region  11-101 Профиль лопасти

                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, (flapSize.X - 23).ToInt(), 0, isOutDoor, materialID, (decimal)thickness, 11, ref ID);
                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.AssemblyName + ".SLDASM", "", true, true);
                    }
                    else
                    {
                        SolidWorksDocument = SolidWorksAdapter.OpenDocument(modelLamel, swDocumentTypes_e.swDocDRAWING, "");
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");

                        base.parameters.Add("D1@Вытянуть1", flapSize.X - 23);
                        EditPartParameters("11-101", base.NewPartPath + ID.ToString(), 0);

                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");// save Профиль лопасти
                        SolidWorksDocument.ForceRebuild3(false);
                        SolidWorksDocument.Extension.SaveAs(NewPartPath + newNameAsm + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                                         + (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, null, ref errors, warnings);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath + newNameAsm + ".SLDASM");
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDDRW");
                        
                        SolidWorksDocument.Extension.SaveAs(NewPartPath + newNameAsm + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                                         + (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, null, ref errors, warnings);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath + newNameAsm + ".SLDDRW");
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            #region typeOfFlange = "30"

            if (flapType == FlapTypes_e.Thirty_mm)
            {

                #region isOutDoor
                if (isOutDoor)
                {
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@11-30-002-1@11-30", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз27@11-30-002-1@11-30", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.Extension.SelectByID2("ВНС-902.49.283-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-323@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-322@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-321@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-320@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-314@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-315@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-316@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-317@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-318@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-319@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    // 11-005 
                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 5, ref ID);

                    if (false)
                    {
                        SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.AcativeteDoc("11-30.SLDASM")));
                        SolidWorksDocument.Extension.SelectByID2("11-005-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {
                        base.parameters.Add("D3@Эскиз1", flapSize.Y);
                        base.parameters.Add("D1@Кривая1", rivetH);
                        base.parameters.Add("D3@Эскиз37", ((countL / 1000) % 2 == 1) ? 0 : 50);
                        base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                        base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                        base.parameters.Add("Толщина@Листовой металл", thickness);

                        EditPartParameters("11-005", base.NewPartPath + ID.ToString(), materialID);
                    }

                    // 11-006 
                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 6, ref ID);

                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM" );
                        SolidWorksDocument.Extension.SelectByID2("11-006-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        base.parameters.Add("D3@Эскиз1", flapSize.Y);
                        base.parameters.Add("D1@Кривая1", rivetH);
                        base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                        base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                        base.parameters.Add("Толщина@Листовой металл", thickness);

                        EditPartParameters("11-006", base.NewPartPath + ID.ToString(), materialID);
                    }
                }
                else
                {
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@11-30-002-1@11-30", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть9@11-30-002-1@11-30", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз28@11-30-002-1@11-30", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз29@11-30-002-1@11-30", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();


                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@11-30-004-2@11-30", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@11-30-004-2@11-30", "BODYFEATURE", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз27@11-30-004-2@11-30", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Эскиз28@11-30-004-2@11-30", "SKETCH", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();

                    SolidWorksDocument.Extension.SelectByID2("11-005-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-006-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    


                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-346@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-347@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-348@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-349@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-350@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-351@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-356@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-357@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-358@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-359@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-360@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-361@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                }
                #endregion
                
                #region Кратность лопастей

                if (Convert.ToInt32(countL / 1000) % 2 == 0) //четное
                {
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                }
                #endregion

                double lp = flapSize.X - 50; // Размер профиля 640,5 при ширине двойного 1400 = 
                double lp2 = lp - 11.6; // Длина линии под заклепки профиля
                double lProfFileName = flapSize.X;
                double lProfNameLength = (flapSize.X - 23);//////////////////////////     /1000

                #region IsDouble

                bool isDouble = flapSize.X > 1000;

                if (!isDouble)
                {
                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                    SolidWorksDocument.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-100-13@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-30-003-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("11-30-003-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("ВНС-47.91.001-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент1", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern3", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-267@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-268@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-270@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-271@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-272@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-273@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-274@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-275@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-276@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-264@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-265@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-266@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-243@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-244@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-245@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-246@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-247@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-248@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-240@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-241@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-242@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-249@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-250@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-251@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();

                    ////////////////////////
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-334@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Rivet Bralo-332@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); SolidWorksDocument.EditDelete();
                }
                else
                {
                    lp = flapSize.X / 2 - 59.5;
                    lp2 = lp - 11.6;
                    lProfFileName = (int)Math.Truncate(Convert.ToDouble(flapSize.X) / 2 - 9);
                    lProfNameLength = (flapSize.X / 2 - 23);
                }

                #endregion

                #region Детали

                // 11-30-001 - верхняя/нижняя
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, (flapSize.X / 2 - 0.8).ToInt(), lp2.ToInt(), isOutDoor, materialID, (decimal)thickness, 7, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30.SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-30-001-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-001.SLDPRT");
                }
                else
                {
                    if (!isDouble)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30-001.SLDPRT");
                        SolidWorksDocument.Extension.SelectByID2("Эскиз17", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditSketch();
                        SolidWorksDocument.ClearSelection2(true);
                        SolidWorksDocument.Extension.SelectByID2("D22@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D21@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D20@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D19@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D18@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D17@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D4@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D3@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D2@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("D1@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();
                        SolidWorksDocument.SketchManager.InsertSketch(true);
                    }
                    
                    base.parameters.Add("D2@Эскиз1", flapSize.X / 2 - 0.8);
                    base.parameters.Add("D3@Эскиз18", lp2);//длина линии по которой разместяться заклепки
                    base.parameters.Add("D1@Кривая1", (Math.Truncate(lp2 / step) + 1)*1000);

                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", thickness);

                    EditPartParameters("11-30-001", base.NewPartPath + ID.ToString(), materialID);
                }
                // 11-30-002
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 2).ToInt(), isOutDoor, materialID, (decimal)thickness, 8, ref ID);

                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-30-002-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-002.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.Y + 10);//10
                    base.parameters.Add("D3@Эскиз23", countL/10 - 100);
                    // base.parameters.Add("D2@Эскиз23", 100 * Math.Truncate(countL/2000));
                    base.parameters.Add("D1@Кривая2", countL);
                    base.parameters.Add("D1@Кривая3", rivetH);

                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", thickness);

                    EditPartParameters("11-30-002", base.NewPartPath + ID.ToString(), materialID);
                }
                // 11-30-004 
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 2).ToInt(), isOutDoor, materialID, (decimal)thickness, 10, ref ID);

                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-30-004-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-004.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", flapSize.Y + 10);//10
                    base.parameters.Add("D3@Эскиз23",(countL/10 - 100));
                    base.parameters.Add("D2@Эскиз23", 100 * Math.Truncate(countL/2000));
                    base.parameters.Add("D1@Кривая2", countL);
                    base.parameters.Add("D1@Кривая5", rivetH);

                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", thickness);

                    EditPartParameters("11-30-004", base.NewPartPath + ID.ToString(), materialID);
                }
                // 11-30-003
                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, lp.ToInt(), isOutDoor, materialID, (decimal)thickness, 9, ref ID);
                if (false)
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
                    SolidWorksDocument.Extension.SelectByID2("11-30-003-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-003.SLDPRT");
                }
                else
                {
                    base.parameters.Add("D2@Эскиз1", lp);
                    base.parameters.Add("D7@Ребро-кромка1", hC);
                    base.parameters.Add("D1@Кривая1", (Math.Truncate(lp2/step) + 1)*1000);

                    base.parameters.Add("D1@Листовой металл1", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл1", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл1", thickness);

                    EditPartParameters("11-30-003", base.NewPartPath + ID.ToString(), materialID);
                }
                #endregion

                #region Сборки

                #region 11-100 Сборка лопасти

                SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, flapSize.X.ToInt(), flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 12, ref ID);

                string newNameAsm = ID.ToString();
                string NewAsmPath = NewPartPath + newNameAsm + ".SLDASM";


                if (isDouble)
                {
                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-100-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(NewAsmPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region 11-101 Профиль лопасти
                        SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, lProfNameLength.ToInt(), isOutDoor, materialID, (decimal)thickness, 11, ref ID);
                        
                        if (false)
                        {
                            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                            SolidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString(), "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                        }
                        else
                        {
                            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                            base.parameters.Add("D1@Вытянуть1", lProfNameLength);
                            EditPartParameters("11-101", base.NewPartPath + ID.ToString(), 0);
                        }
                        #endregion
                    }
                }
                else
                {
                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-100-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(NewAsmPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region  11-101  Профиль лопасти
                        
                        SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, lProfNameLength.ToInt(), isOutDoor, materialID, (decimal)thickness, 11, ref ID);
                        if (false)
                        {
                            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                            SolidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString(), "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-101.SLDPRT");
                        }
                        else
                        {
                            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");
                            base.parameters.Add("D1@Вытянуть1", lProfNameLength);
                            EditPartParameters("11-101", base.NewPartPath + ID.ToString(), 0);
                        }
                        #endregion
                    }
                }

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");

                if (isDouble)
                {
                    SolidWorksDocument.Extension.SelectByID2("Совпадение76", "MATE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Совпадение77", "MATE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("ВНС-47.91.101-2@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-100.SLDASM");

                SolidWorksDocument.SaveAs2(NewAsmPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewAsmPath);

                ModelDoc2 docDrw100 = SolidWorksAdapter.OpenDocument($@"{RootFolder}\{SourceFolder}\{"11-100"}.SLDDRW", swDocumentTypes_e.swDocDRAWING, "");
                docDrw100.ForceRebuild3(false);
                docDrw100.SaveAs2(NewPartPath + newNameAsm +".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewPartPath + newNameAsm + ".SLDDRW");


                #endregion

                #region 11-30-100 Сборка Перемычки
                
                if (isDouble)
                {
                    SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, flapSize.X.ToInt(), flapSize.Y.ToInt(), isOutDoor, materialID, (decimal)thickness, 15, ref ID);
                    newNameAsm = ID.ToString();
                    NewAsmPath = NewPartPath + newNameAsm + ".SLDASM";

                    if (false)
                    {
                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30-100.SLDASM");
                        SolidWorksDocument.Extension.SelectByID2("11-30-100-4@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(NewAsmPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                    }
                    else
                    {
                        #region  11-30-101  Профиль перемычки

                        SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 2).ToInt(), isOutDoor, materialID, (decimal)thickness, 16, ref ID);
                        if (false)
                        {
                            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30-100.SLDASM");
                            SolidWorksDocument.Extension.SelectByID2("11-30-101-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString(), "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            base.parameters.Add("D2@Эскиз1", flapSize.Y + 10);
                            base.parameters.Add("D3@Эскиз19",countL/10 - 100);
                            base.parameters.Add("D1@Кривая1", countL);
                            base.parameters.Add("D1@Кривая2", rivetH);

                            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                            base.parameters.Add("Толщина@Листовой металл", thickness);

                            EditPartParameters("11-30-101", base.NewPartPath + ID.ToString(), materialID);
                        }
                        #endregion

                        #region  11-30-102  Профиль перемычки

                        SwPlusRepository.Instance.AirVents_AddAssemblyFlap((int)flapType, 0, (flapSize.Y + 10).ToInt(), isOutDoor, materialID, (decimal)thickness, 17, ref ID);
                        if (false)
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("11-30-100", false, 0);
                            SolidWorksDocument.Extension.SelectByID2("11-30-102-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            AssemblyDocument.ReplaceComponents(base.NewPartPath + ID.ToString(), "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            base.parameters.Add("D2@Эскиз1", flapSize.Y + 10);
                            base.parameters.Add("D2@Эскиз19", countL / 10 - 100);
                            base.parameters.Add("D1@Кривая2", countL);
                            base.parameters.Add("D1@Кривая1", rivetH);

                            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                            base.parameters.Add("Толщина@Листовой металл", thickness);

                            EditPartParameters("11-30-102", base.NewPartPath + ID.ToString(), materialID);
                        }
                        #endregion

                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30-100.SLDASM");
                        //SolidWorksDocument.ForceRebuild3(false);
                        SolidWorksDocument.SaveAs2(NewAsmPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NewAsmPath);

                    }
                }

                #endregion

                #endregion
            }

            #endregion

            
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            SolidWorksDocument.ForceRebuild3(false);

            //Сохранение главной сборки
            SolidWorksDocument.Extension.SaveAs(NewPartPath + newDamperName + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref errores, ref warnings);
            //SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newDamperName + ".SLDASM");

            string nameDrw = $@"{RootFolder}\{SubjectDestinationFolder}\{newDamperName}";
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc( drawingName + ".SLDDRW");
            DrawingDoc drw = (DrawingDoc)SolidWorksDocument;
            drw.ActivateSheet("DRW1");
            
            int m = 5;

            if (Convert.ToInt32(flapSize.X) > 500 || Convert.ToInt32(flapSize.Y) > 500) { m = 10; }
            if (Convert.ToInt32(flapSize.X) > 850 || Convert.ToInt32(flapSize.Y) > 850) { m = 15; }
            if (Convert.ToInt32(flapSize.X) > 1250 || Convert.ToInt32(flapSize.Y) > 1250) { m = 20; }
            
            drw.SetupSheet5("DRW1", 12, 12, 1, m, true, nameDrw, 0.42, 0.297, "По умолчанию", false);
            //SolidWorksDocument.EditRebuild3();
            SolidWorksDocument.SaveAs2(NewPartPath + newDamperName + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksAdapter.CloseDocument(SolidWorksDocument);
                //.CloseDoc("11-" + newDamperName + ".SLDDRW");
        }
    } 
}