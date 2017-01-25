using PDMWebService.Properties;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentsCadLibrary;

namespace PDMWebService.Data.Solid.PartBuilders
{
    class FlapBuilder : AbstractBuilder
    {
        /// <summary>
        /// Папка с исходной моделью "Регулятора расхода воздуха". 
        /// </summary>
        private string DamperFolder = @"\Библиотека проектирования\DriveWorks\11 - Damper";
        /// <summary>
        /// Папка для сохранения компонентов "Регулятора расхода воздуха". 
        /// </summary>
        private string DamperDestinationFolder = @"\Проекты\Blauberg\11 - Регулятор расхода воздуха";

        /// <summary>
        /// Dumpers the s.
        /// </summary>
        /// <param name="typeOfFlange">The typeOfFlange.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isOutDoor"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        public string Build(string typeOfFlange, string width, string height, bool isOutDoor, string[] material)
        {
            if (!DataConverter.IsConvertToInt(new[] { width, height })) { return ""; }

            string modelName = null;
            string modelDamperPath = null;
            string nameAsm = null;

            var replaced = VersionsFileInfo.Replaced.Instance;
            replaced.Clear();

            var repList = replaced.List;

            DateTime? lastChangedDate = null;

            switch (typeOfFlange)
            {
                case "20":
                    modelName = "11-20";
                    modelDamperPath = DamperFolder;
                    nameAsm = "11 - Damper";
                    lastChangedDate = new DateTime(2016, 5, 20);
                    break;
                case "30":
                    modelName = "11-30";
                    modelDamperPath = DamperFolder;
                    nameAsm = "11-30";
                    break;
            }

            if (string.IsNullOrEmpty(modelName)) return null;

            var modelType = $"{(material[3] == "AZ" ? "" : "-" + material[3])}{(material[3] == "AZ" ? "" : material[1])}";

            var drawing = "11-20";
            if (modelName == "11-30")
            { drawing = modelName; }
            var newDamperName = modelName + "-" + width + "-" + height + modelType + (isOutDoor ? "-O" : "");
            // //MessageBox.Show(newDamperName); return null;
            var newDamperPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newDamperName}.SLDDRW";
            var newDamperAsmPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newDamperName}.SLDASM";


            if (OpenIfExist(newDamperPath, VaultSystem.VentsCadFile.Type.Drawing, lastChangedDate)) return null;
            VersionsFileInfo.Replaced.ExistLatestVersion(newDamperAsmPath, VaultSystem.VentsCadFile.Type.Assembly, lastChangedDate, Settings.Default.PdmBaseName);

            if (replaced.List.Count > 0)
            {
                VaultSystem.CheckInOutPdm(new List<FileInfo> { new FileInfo(newDamperPath), }, false);
            }

            var modelDamperDrw = $@"{Settings.Default.SourceFolder}{modelDamperPath}\{drawing}.SLDDRW";
            var modelLamel = $@"{Settings.Default.SourceFolder}{modelDamperPath}\{"11-100"}.SLDDRW";

            foreach (string item in new[] { new FileInfo(modelDamperDrw).FullName, new FileInfo(modelLamel).FullName })
            {
                PDMWebService.Data.PDM.SolidWorksPdmAdapter.Instance.GetLastVersionAsmPdm(item);

            }
         
 
            var swDocDrw =SolidWorksInstance.SldWoksApp.OpenDoc6(@modelDamperDrw, (int)swDocumentTypes_e.swDocDRAWING,
                (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

            ModelDoc2 swDoc = SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm, false, 0);
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);

             

            // Габариты
            var widthD = Convert.ToDouble(width);
            var heightD = Convert.ToDouble(height);
            // Количество лопастей
            var countL = (Math.Truncate(heightD / 100)) * 1000;

            // Шаг заклепок
            const double step = 140;
            var rivetW = (Math.Truncate(widthD / step) + 1) * 1000;
            var rivetH = (Math.Truncate(heightD / step) + 1) * 1000;

            // Высота уголков
            var hC = Math.Truncate(7 + 5.02 + (heightD - countL / 10 - 10.04) / 2);

            // Коэффициенты и радиусы гибов   
            var thiknessStr = material?[1].Replace(".", ",") ?? "0,8";
            // //MessageBox.Show(thiknessStr);  // obs

            #region typeOfFlange = "20"

            if (typeOfFlange == "20")
            {
                if (Convert.ToInt32(countL / 1000) % 2 == 1) //нечетное
                {
                    swDoc.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditUnsuppress2();
                }

                string newName;
                string newPartPath;

                if (isOutDoor)
                {
                    swDoc.Extension.SelectByID2("Эскиз1", "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();

                    swDoc.Extension.SelectByID2("Вырез-Вытянуть10@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз34@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("ВНС-901.41.302-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Rivet Bralo-130@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-131@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-129@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-128@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-127@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-126@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();


                    // 11-005 
                    newName = "11-05-" + height + modelType;
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                    if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {

                        SwPartParamsChangeWithNewName("11-005",
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"D3@Эскиз37", (Convert.ToInt32(countL / 1000) % 2 == 1) ? "0" : "50"},
                                {"Толщина@Листовой металл", thiknessStr}
                            },
                            false,
                            null);
                        AddMaterial(material, newName);
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }

                    // 11-006 
                    newName = "11-06-" + height + modelType;
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                    if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-006",
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            },
                            false,
                            null);

                        AddMaterial(material, newName);
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }
                }
                else
                {
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть11@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть12@11-001-7@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз35@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз36@11-001-7@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть10@11-003-6@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть11@11-003-6@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз34@11-003-6@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз35@11-003-6@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();


                    swDoc.Extension.SelectByID2("Rivet Bralo-187@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-188@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-189@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-190@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-191@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-192@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-193@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-194@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-195@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-196@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-197@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-198@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                }


                // 11-001 
                newName = "11-01-" + height + modelType + (isOutDoor ? "-O" : "");

                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-001-7@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-001.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-001",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 7.94)}, {"D1@Эскиз27", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз27", Convert.ToString(100*Math.Truncate(countL/2000))}, {"D1@Кривая1", Convert.ToString(countL)},
                            {"D1@Кривая2", Convert.ToString(rivetH)},  {"Толщина@Листовой металл", thiknessStr}
                        }, false, null);
                    AddMaterial(material, newName);
                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    SolidWorksInstance.SldWoksApp.CloseDoc(newName + ".SLDPRT");
                }

                #region OutDoor

                if (isOutDoor)
                {
                    try
                    {
                        swDoc
                           = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-003-6@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-003.SLDPRT");
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-003"); // obs
                    }
                }

                #endregion

                // 11-002
                newName = "11-03-" + width + modelType;
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";

                if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-002-4@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-002.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-002",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD - 0.96)},
                            {"D1@Кривая1", Convert.ToString(rivetW)},

                            {"D1@Кривая3", Convert.ToString(rivetH)},

                            {"Толщина@Листовой металл1", thiknessStr}
                        },
                        false,
                        null);

                    AddMaterial(material, newName);
                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // todo sdgvr СПИСКИ оформить для регистрации                
                if (!isOutDoor)
                {
                    // 11-003 
                    newName = "11-02-" + height + modelType + (isOutDoor ? "-O" : "");
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";

                    if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-003-6@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-003.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-003",
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                            new[,]
                            {
                                {"D2@Эскиз1", Convert.ToString(heightD + 7.94)},
                                {"D1@Эскиз27", Convert.ToString(countL/10 - 100)},
                                {"D1@Кривая1", Convert.ToString(countL)},
                                {"D1@Кривая2", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            },
                            false,
                            null);

                        AddMaterial(material, newName);
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile
                        {
                            LocalPartFileInfo = new FileInfo(newPartPath).FullName
                        });
                    }
                }

                // 11-004
                newName = "11-04-" + width + "-" + hC + modelType;
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";

                if (VersionsFileInfo.Replaced.ExistLatestVersion(newPartPath, VaultSystem.VentsCadFile.Type.Part, lastChangedDate, Settings.Default.PdmBaseName))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-004-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-004.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-004",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD - 24)},
                            {"D7@Ребро-кромка1", Convert.ToString(hC)},
                            {"D1@Кривая1", Convert.ToString(rivetW)},

                            {"D1@Эскиз8", Convert.ToString(18.5)},
                            {"D1@Эскиз9", Convert.ToString(18.5)},

                            {"D1@Кривая5", Convert.ToString(rivetH)},
                            {"Толщина@Листовой металл1", thiknessStr}
                        },
                        false,
                        null);

                    AddMaterial(material, newName);
                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                //11-100 Сборка лопасти
                var newNameAsm = "11-" + width;
                var newPartPathAsm =
                    $@"{Settings.Default.DestinationFolder}{DamperDestinationFolder}\{newNameAsm}.SLDASM";
                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPathAsm), 0))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                }
                else
                {
                    #region  11-101  Профиль лопасти

                    newName = "11-" + (Math.Truncate(widthD - 23)) + "-01" + modelType;
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                    {
                        SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                        swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                        swDoc.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", true, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                        swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                        swDoc.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        var myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@11-101.Part")));
                        myDimension.SystemValue = (widthD - 23) / 1000;
                        SolidWorksInstance.SldWoksApp.ActivateDoc2("11-101", false, 0);
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                        swDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

                        // ToDo Delete
                        SolidWorksInstance.SldWoksApp.CloseDoc(newName + ".sldasm");

                        NewComponentsFull.Add(new VaultSystem.VentsCadFile
                        {
                            LocalPartFileInfo = new FileInfo(
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.sldasm").FullName
                        });
                    }

                    #endregion

                    SolidWorksInstance.SldWoksApp.ActivateDoc2("11-100", false, 0);
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                    swDoc.ForceRebuild3(false);

                    var docDrw100 = SolidWorksInstance.SldWoksApp.OpenDoc6(modelLamel, (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
                    swDoc.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

                    try
                    {
                        //_swApp.CloseDoc(newNameAsm);                    
                        SolidWorksInstance.SldWoksApp.ActivateDoc2(docDrw100?.GetTitle(), false, 0);
                        docDrw100?.ForceRebuild3(true);

                        docDrw100.SaveAs2(
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW").FullName) + " - DRW1");

                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW").FullName });

                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-101  Профиль лопасти");                    // obs     
                    }
                }
            }

            #endregion

            #region typeOfFlange = "30"

            if (typeOfFlange == "30")
            {
                string newName;
                string newPartPath;

                if (isOutDoor)
                {
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть7@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз27@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("ВНС-902.49.283-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Rivet Bralo-314@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-323@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-322@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-321@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-320@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("Rivet Bralo-315@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-316@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-317@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-318@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-319@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    // 11-005 
                    newName = "11-05-" + height + modelType;
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-005.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-005",
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},

                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"D3@Эскиз37", (Convert.ToInt32(countL / 1000) % 2 == 1) ? "0" : "50"},

                                {"Толщина@Листовой металл", thiknessStr}
                            },
                            false,
                            null);
                        AddMaterial(material, newName);

                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }

                    // 11-006 
                    newName = "11-06-" + height + modelType;
                    newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPath, "", false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-006.SLDPRT");
                    }
                    else
                    {
                        SwPartParamsChangeWithNewName("11-006",
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                            new[,]
                            {
                                {"D3@Эскиз1", Convert.ToString(heightD)},
                                {"D1@Кривая1", Convert.ToString(rivetH)},
                                {"Толщина@Листовой металл", thiknessStr}
                            },
                            false,
                            null);
                        AddMaterial(material, newName);
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                    }
                }
                else
                {
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть8@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть9@11-30-002-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз28@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз29@11-30-002-1@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();


                    swDoc.Extension.SelectByID2("Вырез-Вытянуть7@11-30-004-2@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть8@11-30-004-2@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз27@11-30-004-2@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Эскиз28@11-30-004-2@" + nameAsm, "SKETCH", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

                    swDoc.Extension.SelectByID2("11-005-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-006-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();


                    swDoc.Extension.SelectByID2("Rivet Bralo-346@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-347@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-348@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-349@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-350@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-351@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-356@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-357@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-358@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-359@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-360@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-361@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                }


                #region Кратность лопастей

                if (Convert.ToInt32(countL / 1000) % 2 == 0) //четное
                {
                    swDoc.Extension.SelectByID2("Совпадение5918344", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Совпадение5918345", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("Совпадение5918355", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Совпадение5918353", "MATE", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditUnsuppress2();
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
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                    swDoc.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.Extension.SelectByID2("11-30-100-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-100-13@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-30-003-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("11-30-003-4@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ВНС-47.91.001-1@11-30", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("ЗеркальныйКомпонент1", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("DerivedCrvPattern3", "COMPPATTERN", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-267@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-268@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-270@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-271@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-272@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-273@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-274@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-275@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-276@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-264@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-265@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-266@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-243@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-244@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-245@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-246@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-247@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-248@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Rivet Bralo-240@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-241@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-242@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-249@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-250@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.Extension.SelectByID2("Rivet Bralo-251@11-30", "COMPONENT", 0, 0, 0, true, 0, null, 0); swDoc.EditDelete();
                }

                if (isdouble)
                {
                    lp = widthD / 2 - 59.5;
                    lp2 = lp - 11.6;
                    lProfName = Convert.ToString(Math.Truncate(Convert.ToDouble(Convert.ToDouble(width)) / 2 - 9));
                    lProfNameLength = (widthD / 2 - 23) / 1000;
                }

                #endregion

                #region Детали

                // 11-30-001 
                newName = "11-30-03-" + width + modelType;
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-30-001-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-30-001.SLDPRT");
                }
                else
                {
                    if (!isdouble)
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2("11-30-001.SLDPRT", true, 0)));
                        swDoc.Extension.SelectByID2("Эскиз17", "SKETCH", 0, 0, 0, false, 0, null, 0);
                        swDoc.EditSketch();
                        swDoc.ClearSelection2(true);
                        swDoc.Extension.SelectByID2("D22@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D21@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D20@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D19@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D18@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D17@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D4@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D3@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D2@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.Extension.SelectByID2("D1@Эскиз17@11-30-001.SLDPRT", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                        swDoc.EditDelete();
                        swDoc.SketchManager.InsertSketch(true);
                    }

                    SwPartParamsChangeWithNewName("11-30-001",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(widthD/2 - 0.8)},
                            {"D3@Эскиз18", Convert.ToString(lp2)},
                            {"D1@Кривая1", Convert.ToString((Math.Truncate(lp2/step) + 1)*1000)},
                            {"Толщина@Листовой металл", thiknessStr}
                        },
                        false,
                        null);
                    AddMaterial(material, newName);

                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-002 
                newName = "11-30-01-" + height + modelType + (isOutDoor ? "-O" : "");
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-30-002-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-30-002.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-002",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                            {"D3@Эскиз23", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз23", Convert.ToString(100*Math.Truncate(countL/2000))},

                            {"D1@Кривая2", Convert.ToString(countL)},
                            {"D1@Кривая3", Convert.ToString(rivetH)},

                            {"Толщина@Листовой металл", thiknessStr}
                        },
                        false,
                        null);

                    AddMaterial(material, newName);

                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-004 
                newName = "11-30-02-" + height + modelType + (isOutDoor ? "-O" : "");
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-30-004-2@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-30-004.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-004",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                            {"D3@Эскиз23", Convert.ToString(countL/10 - 100)},
                            {"D2@Эскиз23", Convert.ToString(100*Math.Truncate(countL/2000))},
                            {"D1@Кривая2", Convert.ToString(countL)},

                            {"D1@Кривая5", Convert.ToString(rivetH)},

                            {"Толщина@Листовой металл", thiknessStr}
                        },
                        false,
                        null);

                    AddMaterial(material, newName);

                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                // 11-30-003 
                newName = "11-30-04-" + Math.Truncate(lp) + "-" + hC + modelType;
                newPartPath = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                {
                    swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("11-30-003-2@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksInstance.SldWoksApp.CloseDoc("11-30-003.SLDPRT");
                }
                else
                {
                    SwPartParamsChangeWithNewName("11-30-003",
                        $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                        new[,]
                        {
                            {"D2@Эскиз1", Convert.ToString(lp)},
                            {"D7@Ребро-кромка1", Convert.ToString(hC)},
                            {"D1@Кривая1", Convert.ToString((Math.Truncate(lp2/step) + 1)*1000)},
                            {"Толщина@Листовой металл1", thiknessStr}
                        },
                        false,
                        null);

                    AddMaterial(material, newName);

                    NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                }

                #endregion

                #region Сборки

                #region 11-100 Сборка лопасти

                var newNameAsm = "11-2-" + lProfName;
                string newPartPathAsm =
                    $@"{Settings.Default.DestinationFolder}{DamperDestinationFolder}\{newNameAsm}.SLDASM";

                if (isdouble)
                {
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPathAsm), 0))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region  11-101  Профиль лопасти

                        newName = "11-" + (Math.Truncate(lProfNameLength * 1000)) + "-01";
                        newPartPath =
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                        if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                        }
                        else
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0,
                                null, 0);
                            var myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@11-101.Part")));
                            myDimension.SystemValue = lProfNameLength;
                            SolidWorksInstance.SldWoksApp.ActivateDoc2("11-101", false, 0);
                            swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc(newName);
                            NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                        }

                        #endregion
                    }
                }

                if (!isdouble)
                {
                    newNameAsm = "11-" + lProfName;
                    newPartPathAsm =
                        $@"{Settings.Default.DestinationFolder}{DamperDestinationFolder}\{newNameAsm}.SLDASM";
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPathAsm), 0))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-100-1@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                    }
                    else
                    {
                        #region  11-101  Профиль лопасти

                        newName = "11-" + (Math.Truncate(lProfNameLength * 1000)) + "-01";
                        newPartPath =
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                        if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1))
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("11-101-1@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc("11-100.SLDASM");
                        }
                        else
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("D1@Вытянуть1@11-101-1@11-100", "DIMENSION", 0, 0, 0, false, 0,
                                null, 0);
                            var myDimension = ((Dimension)(swDoc.Parameter("D1@Вытянуть1@11-101.Part")));
                            myDimension.SystemValue = lProfNameLength;
                            SolidWorksInstance.SldWoksApp.ActivateDoc2("11-101", false, 0);
                            swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.SaveAs2(newPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc(newName);
                            NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPath).FullName });
                        }

                        #endregion
                    }
                }

                SolidWorksInstance.SldWoksApp.ActivateDoc2("11-100", false, 0);
                swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                if (isdouble)
                {
                    swDoc.Extension.SelectByID2("Совпадение76", "MATE", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
                    swDoc.Extension.SelectByID2("Совпадение77", "MATE", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
                    swDoc.Extension.SelectByID2("ВНС-47.91.101-2@11-100", "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();
                }

                var docDrw100 = SolidWorksInstance.SldWoksApp.OpenDoc6($@"{Settings.Default.SourceFolder}{modelDamperPath}\{"11-100"}.SLDDRW", (int)swDocumentTypes_e.swDocDRAWING, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);

                SolidWorksInstance.SldWoksApp.ActivateDoc2(Path.GetFileNameWithoutExtension(newPartPathAsm), false, 0);
                swDoc.ForceRebuild3(false);
                swDoc.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksInstance.SldWoksApp.CloseDoc(newNameAsm);
                docDrw100.ForceRebuild3(false);
                docDrw100.SaveAs2(
                    $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                SolidWorksInstance.SldWoksApp.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(
                    $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW").FullName) + " - DRW1");

                NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(newPartPathAsm).FullName });
                NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}.SLDDRW").FullName });

                #endregion

                #region 11-30-100 Сборка Перемычки

                newNameAsm = "11-30-100-" + height + modelType;
                newPartPathAsm = $@"{Settings.Default.DestinationFolder}{DamperDestinationFolder}\{newNameAsm}.SLDASM";

                if (isdouble)
                {
                    if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPathAsm), 0))
                    {
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm + ".SLDASM", true, 0)));
                        swDoc.Extension.SelectByID2("11-30-100-4@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAsm.ReplaceComponents(newPartPathAsm, "", true, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc("11-30-100.SLDASM");
                    }
                    else
                    {
                        #region  11-30-101  Профиль перемычки

                        newName = "11-30-101-" + height + modelType;
                        newPartPath =
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                        if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 0))
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-30-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("11-30-101-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            SwPartParamsChangeWithNewName("11-30-101",
                                $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                                new[,]
                                {
                                    {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                                    {"D3@Эскиз19", Convert.ToString(countL/10 - 100)},
                                    {"D1@Кривая1", Convert.ToString(countL)},
                                    {"D1@Кривая2", Convert.ToString(rivetH)},
                                    {"Толщина@Листовой металл", thiknessStr}
                                },
                                false,
                                null);
                            AddMaterial(material, newName);

                            SolidWorksInstance.SldWoksApp.CloseDoc(newName);
                            NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}").FullName });
                        }

                        #endregion

                        SolidWorksInstance.SldWoksApp.ActivateDoc2("11-30-100", false, 0);
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                        swDoc.ForceRebuild3(true);

                        #region  11-30-102  Профиль перемычки

                        newName = "11-30-102-" + height + modelType;
                        newPartPath =
                            $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}.SLDPRT";
                        if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 0))
                        {
                            SolidWorksInstance.SldWoksApp.IActivateDoc2("10-30-100", false, 0);
                            swDoc = (ModelDoc2)((IModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                            swDoc.Extension.SelectByID2("11-30-102-2@11-30-100", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            swAsm.ReplaceComponents(newPartPath, "", true, true);
                            SolidWorksInstance.SldWoksApp.CloseDoc("11-30-100.SLDASM");
                        }
                        else
                        {
                            SwPartParamsChangeWithNewName("11-30-102",
                                $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newName}",
                                new[,]
                                {
                                    {"D2@Эскиз1", Convert.ToString(heightD + 10)},
                                    {"D2@Эскиз19", Convert.ToString(countL/10 - 100)},
                                    {"D1@Кривая2", Convert.ToString(countL)},
                                    {"D1@Кривая1", Convert.ToString(rivetH)},
                                    {"Толщина@Листовой металл", thiknessStr}
                                },
                                false,
                                null);
                            try
                            {
                                VentsMatdll(material, null, newName);
                            }
                            catch (Exception e)
                            {
                                //MessageBox.Show($"{newName}\n{e.Message}\n{e.StackTrace}", "11-30-102  Профиль перемычки");                                
                            }

                            SolidWorksInstance.SldWoksApp.CloseDoc(newName);
                            NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}").FullName });
                        }

                        #endregion

                        SolidWorksInstance.SldWoksApp.ActivateDoc2("11-30-100", false, 0);
                        swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActiveDoc));
                        swDoc.ForceRebuild3(false); swDoc.ForceRebuild3(true);
                        swDoc.SaveAs2(newPartPathAsm, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
                        SolidWorksInstance.SldWoksApp.CloseDoc(newNameAsm);
                        NewComponentsFull.Add(new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo($@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newNameAsm}").FullName });

                        #endregion

                    }
                }

                #endregion
            }

            #endregion

            swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(nameAsm, true, 0)));

            GabaritsForPaintingCamera(swDoc);
            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(true);
            var name = $@"{Settings.Default.DestinationFolder}\{DamperDestinationFolder}\{newDamperName}";
            swDoc.SaveAs2(name + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksInstance.SldWoksApp.CloseDoc(Path.GetFileNameWithoutExtension(new FileInfo(name + ".SLDASM").FullName));
            swDocDrw.Extension.SelectByID2("DRW1", "SHEET", 0, 0, 0, false, 0, null, 0);
            var drw = (DrawingDoc)(SolidWorksInstance.SldWoksApp.IActivateDoc3(drawing + ".SLDDRW", true, 0));
            drw.ActivateSheet("DRW1");
            var m = 5;
            if (Convert.ToInt32(width) > 500 || Convert.ToInt32(height) > 500) { m = 10; }
            if (Convert.ToInt32(width) > 850 || Convert.ToInt32(height) > 850) { m = 15; }
            if (Convert.ToInt32(width) > 1250 || Convert.ToInt32(height) > 1250) { m = 20; }

            //drw.SetupSheet5("DRW1", 12, 12, 1, m, true, Settings.Default.DestinationFolder + @"\\srvkb\SolidWorks Admin\Templates\Основные надписи\A2-A-1.slddrt", 0.42, 0.297, "По умолчанию", false);

            swDocDrw.SaveAs2(name + ".SLDDRW", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            SolidWorksInstance.SldWoksApp.CloseDoc(newDamperPath);
            SolidWorksInstance.SldWoksApp.ExitApp();

            NewComponentsFull.AddRange(new List<VaultSystem.VentsCadFile>
                        {
                            new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(name + ".SLDASM").FullName },
                            new VaultSystem.VentsCadFile { LocalPartFileInfo = new FileInfo(name + ".SLDDRW").FullName },
                         });

            

            string message = null;

            for (int i = 0; i < NewComponentsFull.Count; i++)
            {
                if (repList.Exists(x => x.FilePath.ToLower() == NewComponentsFull[i].LocalPartFileInfo.ToLower()))
                {

                    NewComponentsFull[i].MessageForCheckOut = repList.Single(x => x.FilePath.ToLower().Contains((NewComponentsFull[i].LocalPartFileInfo.ToLower()))).MessageForCheckOut;
                    if (!string.IsNullOrEmpty(NewComponentsFull[i].MessageForCheckOut))
                    {
                        if (string.IsNullOrEmpty(message))
                        {
                            message = NewComponentsFull[i].MessageForCheckOut;
                        }
                    }
                }
            }

            #region To Delete

            //var asdfwe = "";
            //foreach (var item in NewComponentsFull)
            //{                
            //    asdfwe = asdfwe + $"\nPath - {item.LocalPartFileInfo} Message - {item.MessageForCheckOut}";
            //}
            // //MessageBox.Show(asdfwe, $" newList {NewComponentsFull.Count()} listToCheckOut - {NewComponentsFull.Count()}");

            #endregion

            List<VaultSystem.VentsCadFile> outList;
            VaultSystem.CheckInOutPdmNew(NewComponentsFull, true, out outList);

            if (!string.IsNullOrEmpty(message))
            {
                var list = "";
                foreach (var item in NewComponentsFull.Where(x => !string.IsNullOrEmpty(x.MessageForCheckOut)))
                {

                    list = list + $"\nPath - {Path.GetFileName(item.LocalPartFileInfo)}";
                }
                //MessageBox.Show(list, message);// $" Следующие файлы были обновлены {newList.Count()} listToCheckOut - {listToCheckOut.Count()}"); // obs
            }

            SolidWorksInstance.CloseAllDocumentsAndExit();
            return newDamperPath;
        }

        private void AddMaterial(IList<string> material, string newName)
        {
            try
            {
                VentsMatdll(material, null, newName);
            }
            catch (Exception e)
            {
                //MessageBox.Show($"{e}\n{material}\n{newName}{e.StackTrace}" , "Adding Material");
            }
        }
    }
}
