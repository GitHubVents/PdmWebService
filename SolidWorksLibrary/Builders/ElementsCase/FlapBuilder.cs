using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ServiceConstants; 
using SolidWorksLibrary.Builders.ElementsCase;

namespace PDMWebService.Data.Solid.ElementsCase
{
   public class FlapBuilder : ProductBuilderBehavior
    { 
        public FlapBuilder()
        { 
            base.SetProperties(@"\11 - Регулятор расхода воздуха\", @"\11 - Damper\" );
        }

        /// <summary>
        /// Dumpers the s.
        /// </summary>
        /// <param name="flapType">The typeOfFlange.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isOutDoor"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        public string Build(FlapTypes flapType, int width, int height, bool isOutDoor, string[] material)
        {
            string modelName = null;
            string modelDamperPath = null;
            string nameAsm = null;

            switch (flapType)
            {
                case FlapTypes.Twenty_mm:
                    modelName = "11-20";
                    modelDamperPath = SourceFolder;
                    nameAsm = "11 - Damper";
                    break;
                case FlapTypes.Thirty_mm:
                    modelName = "11-30";
                    modelDamperPath = SourceFolder;
                    nameAsm = "11-30";
                    break;
            }

            var modelType = $"{(material[3] == "AZ" ? "" : "-" + material[3])}{(material[3] == "AZ" ? "" : material[1])}";

            string drawingName = "11-20";
            if (modelName == "11-30")
            { drawingName = modelName; }
            string newDamperName = modelName + "-" + width + "-" + height + modelType + (isOutDoor ? "-O" : "");
            string newDamperPath = $@"{RootFolder}{SubjectDestinationFolder}{newDamperName}.SLDDRW";
            string newDamperAsmPath = $@"{RootFolder}{SubjectDestinationFolder}{newDamperName}.SLDASM";
            string modelDamperDrw = $@"{RootFolder}{modelDamperPath}{drawingName}.SLDDRW";
            string modelLamel = $@"{RootFolder}{modelDamperPath}{"11-100"}.SLDDRW";
           ModelDoc2 swDocDrw = SolidWorksAdapter.OpenDocument(modelDamperDrw, swDocumentTypes_e.swDocDRAWING); 

            ModelDoc2 solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm);   // TO DO
            AssemblyDoc sldWorksAsm = (AssemblyDoc)solidWorksDocument;
            sldWorksAsm.ResolveAllLightWeightComponents(false);
             
            // Габариты
            double widthD = width;
            double heightD = height;
            // Количество лопастей
            double countL = (Math.Truncate(heightD / 100)) * 1000;

            // Шаг заклепок
            const double step = 140;
            double rivetW = (Math.Truncate(widthD / step) + 1) * 1000;
            double rivetH = (Math.Truncate(heightD / step) + 1) * 1000;

            // Высота уголков
            double hC = Math.Truncate(7 + 5.02 + (heightD - countL / 10 - 10.04) / 2);

            // Коэффициенты и радиусы гибов   
         var thiknessStr =/* material?[1].Replace(".", ",") ?? */ "3";
            // //MessageBox.Show(thiknessStr);  // obs

            #region typeOfFlange = "20"

            if (flapType == FlapTypes.Twenty_mm)
            {
                if ((countL / 1000) % 2 == 1) //нечетное
                {
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.EditSuppress2();
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.EditUnsuppress2();
                }

                string newName;
                string newPartPath;
                #region isOutDoor
                if (isOutDoor)
                {
                    solidWorksDocument.Extension.SelectByID2("Эскиз1", "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditSuppress2();

                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть10@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз34@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("ВНС-901.41.302-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-130@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-131@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-129@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-128@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-127@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-126@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();


                    // 11-005 
                    newName = "11-05-" + height + modelType;
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";

                    // TO DO change exists file...
                    //  if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                    if (false)
                    {
                        solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {

                        SwPartParamsChangeWithNewName("11-005",
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"D3@Эскиз37", (Convert.ToInt32(countL / 1000) % 2 == 1) ? "0" : "50"},
                                {"Толщина@Листовой металл", thiknessStr}
                            });
                       
                        AddMaterial(material, newName);
                      //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }

                    // 11-006 
                    newName = "11-06-" + height + modelType;
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                    if (/*VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName)*/ false)
                    {
                        solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-006",
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            });

                        AddMaterial(material, newName);
                      //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }
                }
                # endregion

                else
                {
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть12@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз35@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз36@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть10@11-003-6@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@11-003-6@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз34@11-003-6@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз35@11-003-6@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();


                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-187@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-188@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-189@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-190@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-191@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-192@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-193@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-194@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-195@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-196@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-197@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-198@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                }

                
                // 11-001 
                newName = "11-01-" + height + modelType + (isOutDoor ? "-O" : "");

                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                if (/*VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName)*/ false)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-001-7@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-001.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-001", 
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 7.94)}, {"D1@Эскиз27", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз27", Convert.ToString(100*Math.Truncate(countL/2000))}, {"D1@Кривая1", Convert.ToString(countL)},
                            //{"D1@Кривая2", Convert.ToString(rivetH)},
                            {"Толщина@Листовой металл", thiknessStr}
                        });
                    AddMaterial(material, newName);
                  //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName + ".SLDPRT");
                }

                #region OutDoor

                if (isOutDoor)
                {
                    try
                    {
                        solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM" );
                        solidWorksDocument.Extension.SelectByID2("11-003-6@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-003.SLDPRT");
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-003"); // obs
                    }
                }

                #endregion

                // 11-002
                newName = "11-03-" + width + modelType;
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";

                if (/*VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName)*/false)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                   
                    solidWorksDocument.Extension.SelectByID2("11-002-4@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-002.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-002",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD - 0.96)},
                            {"D1@Кривая1", Convert.ToString(rivetW)},
                        //    {"D1@Кривая3", Convert.ToString(rivetH)},  удален эскиз, хз
                            {"Толщина@Листовой металл1", thiknessStr}
                        });

                    AddMaterial(material, newName);
                 //   NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // todo sdgvr СПИСКИ оформить для регистрации                
                if (!isOutDoor)
                {
                    // 11-003 
                    newName = "11-02-" + height + modelType + (isOutDoor ? "-O" : "");
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";

                    if (/*VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName)*/ false)
                    {
                        solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-003-6@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-003.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-003",
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                            new[,]
                            {
                                {"D2@Эскиз1", Convert.ToString(heightD + 7.94)},
                                {"D1@Эскиз27", Convert.ToString(countL/10 - 100)},
                                {"D1@Кривая1", Convert.ToString(countL)},
                                //{"D1@Кривая2", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            });

                        AddMaterial(material, newName);
                        //NewComponentsFull.Add(new VaultSystem.VentsCadFile
                        //{
                        //    LocalPartFileInfo = new FileInfo(newPartPath).FullName
                        //});
                    }
                }

                // 11-004
                newName = "11-04-" + width + "-" + hC + modelType;
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";

                if (/*VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName)*/false)
                {
                    solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-004-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-004.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-004",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD - 24)},
                            {"D7@Ребро-кромка1", Convert.ToString(hC)},
                            {"D1@Кривая1", Convert.ToString(rivetW)},
                            {"D1@Эскиз8", Convert.ToString(18.5)},
                            {"D1@Эскиз9", Convert.ToString(18.5)},
                          //  {"D1@Кривая5", Convert.ToString(rivetH)}, удален эскиз, хз
                            {"Толщина@Листовой металл1", thiknessStr}
                        });
                    AddMaterial(material, newName);
                //    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                //11-100 Сборка лопасти
                var newNameAsm = "11-" + width;
                var newPartPathAsm =
                    $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDASM";
                if (false)
                {
                    solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                }
                else
                {
                    #region  11-101  Профиль лопасти

                    newName = "11-" + (Math.Truncate(widthD - 23)) + "-01" + modelType;
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                    if (false)
                    {
                        SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                        solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                        solidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                        solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                        solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        var myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@11-101.Part")));
                        myDimension.SystemValue = (widthD - 23) / 1000;
                        SolidWorksAdapter.AcativeteDoc("11-101" );
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                        solidWorksDocument.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

                        // ToDo Delete
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName + ".sldasm");

                      //  NewComponentsFull.Add(new VaultSystem.VentsCadFile
                        //{
                        //    LocalPartFileInfo = new FileInfo(
                        //    $@"{RootFolder}{DamperDestinationFolder}{newName}.sldasm").FullName
                        //});
                    }

                    #endregion

                    SolidWorksAdapter.AcativeteDoc("11-100");
                    solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                    solidWorksDocument.ForceRebuild3(false);

                    var docDrw100 = SolidWorksAdapter.SldWoksAppExemplare.OpenDoc6(modelLamel, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
                    solidWorksDocument.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

                    try
                    {
                        //_swApp.CloseDoc(newNameAsm);                    
                        SolidWorksAdapter.AcativeteDoc(docDrw100?.GetTitle());
                        docDrw100?.ForceRebuild3(true);

                        docDrw100.SaveAs2(
                            $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(
                            $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDDRW").FullName) + " - DRW1");

                      ///  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                      //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{RootFolder}{DamperDestinationFolder}{newNameAsm}.SLDDRW").FullName });

                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-101  Профиль лопасти");                    // obs     
                    }
                }
            }

            #endregion

            #region typeOfFlange = "30"

            if (flapType ==  FlapTypes.Thirty_mm)
            {
                string newName;
                string newPartPath;

                if (isOutDoor)
                {
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз27@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("ВНС-902.49.283-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-314@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-323@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-322@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-321@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-320@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-315@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-316@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-317@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-318@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-319@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    // 11-005 
                    newName = "11-05-" + height + modelType;
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                    if (false)
                    {
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM" )));
                        solidWorksDocument.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-005",
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},

                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"D3@Эскиз37", (Convert.ToInt32(countL / 1000) % 2 == 1) ? "0" : "50"},

                                {"Толщина@Листовой металл", thiknessStr}
                            });
                        AddMaterial(material, newName);

                       // NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }

                    // 11-006 
                    newName = "11-06-" + height + modelType;
                    newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                    if (false)
                    {
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM" )));
                        solidWorksDocument.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-006",
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            });
                        AddMaterial(material, newName);
                        //NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }
                }
                else
                {
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть9@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз28@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз29@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();


                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@11-30-004-2@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@11-30-004-2@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз27@11-30-004-2@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Эскиз28@11-30-004-2@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();

                    solidWorksDocument.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();


                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-346@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-347@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-348@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-349@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-350@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-351@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-356@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-357@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-358@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-359@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-360@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-361@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                }


                #region Кратность лопастей

                if (Convert.ToInt32(countL / 1000) % 2 == 0) //четное
                {
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.EditSuppress2();
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.EditUnsuppress2();
                }

                #endregion

                var lp = widthD - 50; // Размер профиля 640,5 при ширине двойного 1400 = 
                var lp2 = lp - 11.6; // Длина линии под заклепки профиля
                var lProfName = width; //
                var lProfNameLength = (widthD - 23) / 1000;

                #region IsDouble

                var isdouble = widthD > 1000;

                if (!isdouble)
                {
                    solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                    solidWorksDocument.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-100-13@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-30-003-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("11-30-003-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("ВНС-47.91.001-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("ЗеркальныйКомпонент1", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("DerivedCrvPattern3", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-267@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-268@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-270@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-271@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-272@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-273@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-274@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-275@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-276@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-264@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-265@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-266@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-243@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-244@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-245@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-246@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-247@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-248@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-240@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-241@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-242@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-249@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-250@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    solidWorksDocument.Extension.SelectByID2("Rivet Bralo-251@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); solidWorksDocument.EditDelete();
                }

                if (isdouble)
                {
                    lp = widthD / 2 - 59.5;
                    lp2 = lp - 11.6;
                    lProfName = (int)Math.Truncate(Convert.ToDouble(width) / 2 - 9);
                    lProfNameLength = (widthD / 2 - 23) / 1000;
                }

                #endregion

                #region Детали

                // 11-30-001 
                newName = "11-30-03-" + width + modelType;
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                if (false)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-30-001-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-001.SLDPRT");
                }
                else
                {
                    if (!isdouble)
                    {
                        solidWorksDocument = SolidWorksAdapter.AcativeteDoc("11-30-001.SLDPRT");
                        solidWorksDocument.Extension.SelectByID2("Эскиз17", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocument.EditSketch();
                        solidWorksDocument.ClearSelection2(true);
                        solidWorksDocument.Extension.SelectByID2("D22@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D21@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D20@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D19@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D18@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D17@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D4@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D3@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D2@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.Extension.SelectByID2("D1@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocument.EditDelete();
                        solidWorksDocument.SketchManager.InsertSketch(true);
                    }

                    SwPartParamsChangeWithNewName("11-30-001",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD/2 - 0.8)},
                            {"D3@Эскиз18", Convert.ToString(lp2)},
                            {"D1@Кривая1", Convert.ToString((Math.Truncate(lp2/step) + 1)*1000)},
                            {"Толщина@Листовой металл", thiknessStr}
                        });
                    AddMaterial(material, newName);

                  //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-002 
                newName = "11-30-01-" + height + modelType + (isOutDoor ? "-O" : "");
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                if (false)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-30-002-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-002.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-002",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                            {"D3@Эскиз23", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз23", Convert.ToString(100*Math.Truncate(countL/2000))},

                            {"D1@Кривая2", Convert.ToString(countL)},
                           // {"D1@Кривая3", Convert.ToString(rivetH)},

                            {"Толщина@Листовой металл", thiknessStr}
                        });

                    AddMaterial(material, newName);

                    //NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-004 
                newName = "11-30-02-" + height + modelType + (isOutDoor ? "-O" : "");
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                if (false)
                {
                    solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-30-004-2@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-004.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-004",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                            {"D3@Эскиз23", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз23", Convert.ToString(100*Math.Truncate(countL/2000))},
                            {"D1@Кривая2", Convert.ToString(countL)},
                            // {"D1@Кривая5", Convert.ToString(rivetH)},
                            {"Толщина@Листовой металл", thiknessStr}
                        });

                    AddMaterial(material, newName);

                    //NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-003 
                newName = "11-30-04-" + Math.Truncate(lp) + "-" + hC + modelType;
                newPartPath = $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                if (false)
                {
                    solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                    solidWorksDocument.Extension.SelectByID2("11-30-003-2@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-003.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-003",
                        $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(lp)},
                            {"D7@Ребро-кромка1", Convert.ToString(hC)},
                            {"D1@Кривая1", Convert.ToString((Math.Truncate(lp2/step) + 1)*1000)},
                            {"Толщина@Листовой металл1", thiknessStr}
                        });

                    AddMaterial(material, newName);

                  //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                #endregion

                #region Сборки

                #region 11-100 Сборка лопасти

                var newNameAsm = "11-2-" + lProfName;
                string newPartPathAsm =
                    $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDASM";

                if (isdouble)
                {
                    if (false)
                    {
                        solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region  11-101  Профиль лопасти

                        newName = "11-" + (Math.Truncate(lProfNameLength * 1000)) + "-01";
                        newPartPath =
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                        if (false)
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                            solidWorksDocument =  SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc;
                            solidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                        }
                        else
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                            solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0,
                                null, 0);
                            var myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@11-101.Part")));
                            myDimension.SystemValue = lProfNameLength;
                            SolidWorksAdapter.AcativeteDoc("11-101");
                            solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
                         //   NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                        }

                        #endregion
                    }
                }

                if (!isdouble)
                {
                    newNameAsm = "11-" + lProfName;
                    newPartPathAsm =
                        $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDASM";
                    if (false)
                    {
                        solidWorksDocument =SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region  11-101  Профиль лопасти

                        newName = "11-" + (Math.Truncate(lProfNameLength * 1000)) + "-01";
                        newPartPath =
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                        if (false)
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                            solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-100.SLDASM");
                        }
                        else
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-100", false, 0);
                            solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0,
                                null, 0);
                            var myDimension = ((Dimension)(solidWorksDocument.Parameter("D1@Вытянуть1@11-101.Part")));
                            myDimension.SystemValue = lProfNameLength;
                            SolidWorksAdapter.AcativeteDoc("11-101");
                            solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
                       //     NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                        }

                        #endregion
                    }
                }

                SolidWorksAdapter.AcativeteDoc("11-100" );
                solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                if (isdouble)
                {
                    solidWorksDocument.Extension.SelectByID2("Совпадение76", "MATE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditSuppress2();
                    solidWorksDocument.Extension.SelectByID2("Совпадение77", "MATE", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditUnsuppress2();
                    solidWorksDocument.Extension.SelectByID2("ВНС-47.91.101-2@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0); solidWorksDocument.EditDelete();
                }

                var docDrw100 = SolidWorksAdapter.SldWoksAppExemplare.OpenDoc6($@"{RootFolder}{modelDamperPath}{"11-100"}.SLDDRW", (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

                SolidWorksAdapter.AcativeteDoc(Path.GetFileNameWithoutExtension(newPartPathAsm) );
                solidWorksDocument.ForceRebuild3(false);
                solidWorksDocument.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newNameAsm);
                docDrw100.ForceRebuild3(false);
                docDrw100.SaveAs2(
                    $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(
                    $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDDRW").FullName) + " - DRW1");

               // NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPathAsm).FullName });
             //   NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{RootFolder}{DamperDestinationFolder}{newNameAsm}.SLDDRW").FullName });

                #endregion

                #region 11-30-100 Сборка Перемычки

                newNameAsm = "11-30-100-" + height + modelType;
                newPartPathAsm = $@"{RootFolder}{SubjectDestinationFolder}{newNameAsm}.SLDASM";

                if (isdouble)
                {
                    if (false)
                    {
                        solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm + ".SLDASM");
                        solidWorksDocument.Extension.SelectByID2("11-30-100-4@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        sldWorksAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                    }
                    else
                    {
                        #region  11-30-101  Профиль перемычки

                        newName = "11-30-101-" + height + modelType;
                        newPartPath =
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                        if (false)
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-30-100", false, 0);
                            solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.Extension.SelectByID2("11-30-101-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            SwPartParamsChangeWithNewName("11-30-101",
                                $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                                new[,]
                                {
                                    {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                                    {"D3@Эскиз19", Convert.ToString(countL/10 - 100)},
                                    {"D1@Кривая1", Convert.ToString(countL)},
                                    {"D1@Кривая2", Convert.ToString(rivetH)},
                                    {"Толщина@Листовой металл", thiknessStr}
                                });
                            AddMaterial(material, newName);

                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
                            //NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{RootFolder}{DamperDestinationFolder}{newNameAsm}").FullName });
                        }

                        #endregion

                        SolidWorksAdapter.AcativeteDoc("11-30-100");
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                        solidWorksDocument.ForceRebuild3(true);

                        #region  11-30-102  Профиль перемычки

                        newName = "11-30-102-" + height + modelType;
                        newPartPath =
                            $@"{RootFolder}{SubjectDestinationFolder}{newName}.SLDPRT";
                        if (false)
                        {
                            SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc2("10-30-100", false, 0);
                            solidWorksDocument = (ModelDoc2)((IModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                            solidWorksDocument.Extension.SelectByID2("11-30-102-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            sldWorksAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            SwPartParamsChangeWithNewName("11-30-102",                                $@"{RootFolder}{SubjectDestinationFolder}{newName}",
                                new[,]
                                {
                                    {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                                    {"D2@Эскиз19", Convert.ToString(countL/10 - 100)},
                                    {"D1@Кривая2", Convert.ToString(countL)},
                                    {"D1@Кривая1", Convert.ToString(rivetH)},
                                    {"Толщина@Листовой металл", thiknessStr}
                                } );
                            try
                            {
                               // VentsMatdll(material, null, newName);
                            }
                            catch (Exception e)
                            {
                                //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-30-102  Профиль перемычки");                                
                            }

                            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
                          //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{RootFolder}{DamperDestinationFolder}{newNameAsm}").FullName });
                        }

                        #endregion

                        SolidWorksAdapter.AcativeteDoc("11-30-100");
                        solidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActiveDoc));
                        solidWorksDocument.ForceRebuild3(false); solidWorksDocument.ForceRebuild3(true);
                        solidWorksDocument.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newNameAsm);
                      //  NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{RootFolder}{DamperDestinationFolder}{newNameAsm}").FullName });

                        #endregion

                    }
                }

                #endregion
            }

            #endregion

            solidWorksDocument = SolidWorksAdapter.AcativeteDoc(nameAsm);

         //   GabaritsForPaintingCamera(solidWorksDocument);
            solidWorksDocument.EditRebuild3();
            solidWorksDocument.ForceRebuild3(true);
            var name = $@"{RootFolder}{SubjectDestinationFolder}{newDamperName}";
            solidWorksDocument.SaveAs2(name + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(name + ".SLDASM").FullName));
            swDocDrw.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)(SolidWorksAdapter.SldWoksAppExemplare.IActivateDoc3(drawingName + ".SLDDRW", true, 0));
            drw.ActivateSheet("DRW1");
            var m = 5;
            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500) { m = 10; }
            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850) { m = 15; }
            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250) { m = 20; }

            //drw.SetupSheet5("DRW1", 12, 12, 1, m, true, RootFolder + @"\\srvkb\SolidWorks Admin\Templates\Основные надписи\A2-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);

            swDocDrw.SaveAs2(name + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newDamperPath);
            SolidWorksAdapter.SldWoksAppExemplare.ExitApp();

            //NewComponentsFull.AddRange(new List<VaultSystem.VentsCadFile>
            //            {
            //                new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(name + ".SLDASM").FullName },
            //                new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(name + ".SLDDRW").FullName },
            //             });



            string message = null;

            //for (int i = 0; i < NewComponentsFull.Count; i++)
            //{
            //    if (/*repList.Exists(x => x.FilePath.ToLower() == NewComponentsFull[i].LocalPartFileInfo.ToLower())*/ true)
            //    {

            //    //    NewComponentsFull[i].MessageForCheckOut = repList.Single(x => x.FilePath.ToLower().Contains((NewComponentsFull[i].LocalPartFileInfo.ToLower()))).MessageForCheckOut;
            //        //if (!string.IsNullOrEmpty(NewComponentsFull[i].MessageForCheckOut))
            //        //{
            //        //    if (string.IsNullOrEmpty(message))
            //        //    {
            //        //        message = NewComponentsFull[i].MessageForCheckOut;
            //        //    }
            //        //}
            //    }
            //}

            #region To Delete

            //var asdfwe = "";
            //foreach (var item in NewComponentsFull)
            //{                
            //    asdfwe = asdfwe + $"\nPath - {item.LocalPartFileInfo} Message - {item.MessageForCheckOut}";
            //}
            // //MessageBox.Show(asdfwe, $" newList {NewComponentsFull.Count()} listToCheckOut - {NewComponentsFull.Count()}");

            #endregion

          //  List<VaultSystem.VentsCadFile> outList;
          //  VaultSystem.CheckInOutPdmNew(NewComponentsFull, true, out outList);

            //if (!string.IsNullOrEmpty(message))
            //{
            //    var list = "";
            //    foreach (var item in NewComponentsFull.Where(x => !string.IsNullOrEmpty(x.MessageForCheckOut)))
            //    {

            //        list = list + $"\nPath - {Path.GetFileName(item.LocalPartFileInfo)}";
            //    }
            //    //MessageBox.Show(list, message);// $" Следующие файлы были обновлены {newList.Count()} listToCheckOut - {listToCheckOut.Count()}"); // obs
            //}

            SolidWorksAdapter.CloseAllDocumentsAndExit();
            return newDamperPath;
        }


        private void SwPartParamsChangeWithNewName(string partName, string newName, string[,] newParams /*, bool newFuncOfAdding, IReadOnlyList<string> copies*/)
        {
            ModelDoc2 swDoc = null;
            swDoc = SolidWorksAdapter.AcativeteDoc(partName + ".SLDPRT");
            var modName = swDoc.GetPathName();
            for (var i = 0; i < newParams.Length / 2; i++)
            {
                Dimension myDimension = ((Dimension)(swDoc.Parameter(newParams[i, 0] + "@" + partName + ".SLDPRT")));
                var param = Convert.ToDouble(newParams[i, 1]);
                var swParametr = param;
                myDimension.SystemValue = swParametr/1000;
            }
            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(false);
            //if (!newFuncOfAdding)
            //{
            //    //TO DO
            //    // list for add file to pdm
            //    NewComponents.Add(new FileInfo(newName + ".SLDPRT"));
            //}

            //if (newFuncOfAdding)
            //{
            //    // todo проверить
            //    //if (!string.IsNullOrEmpty(copies)) return;

            // // TO DO
            //    //NewComponentsFull.Add(new VaultSystem.VentsCadFile
            //    //{
            //    //    LocalPartFileInfo = new FileInfo(newName + ".SLDPRT").FullName,
            //    //    PartIdSql = Convert.ToInt32(newName.Substring(newName.LastIndexOf('-') + 1))
            //    //});
            //}

            // to do
            swDoc.SaveAs2(new FileInfo(newName + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

            //if (copies != null)
            //{
            //    //MessageBox.Show("copies - " + copies + "  addingInName - " + addingInName);
            //    swDoc.SaveAs2(new FileInfo(copies[0] + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
            //    swDoc.SaveAs2(new FileInfo(copies[1] + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
            //}
            // _swApp.CloseDoc(newName + ".SLDPRT");

            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
        }



        private void AddMaterial(IList<string> material, string newName)
        {
            try
            {
                //VentsMatdll(material, null, newName);
            }
            catch (Exception e)
            {
                //MessageBox.Show($"{e}\n{material}\n{newName}{e.StackTrace}" , "Adding Material");
            }
            
        }
    } 

}

