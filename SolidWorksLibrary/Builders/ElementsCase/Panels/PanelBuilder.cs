using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace SolidWorksLibrary.Builders.ElementsCase.Panels
{


    public class PanelBuilder : ProductBuilderBehavior
    {
        public override event SetBendsHandler SetBends;

        private Vector2 sizePanel { get; set; }
        private double innerHeight = 0;
        private double innerWeidht = 0;
        private double leght = 0;
        private double deepInsulation = 0;

        public PanelBuilder() : base()
        {
            base.SetProperties("panel", "source panel");
        }

        private void OpenTemplate(PanelType panelType)
        {
            if (panelType == PanelType.DualBlankPanel || panelType == PanelType.DualRemovablePanel)
            {
                documentlName = "02-01";
            }
            else
            {
                documentlName = "02-104-50";
            }

            string pathToPrototype = System.IO.Path.Combine(RootFolder, SourceFolder, documentlName + ".SLDASM");
            Console.WriteLine("pathToPrototype  " + pathToPrototype);
            SolidWorksAdapter.OpenDocument(pathToPrototype, swDocumentTypes_e.swDocASSEMBLY);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(documentlName + ".SLDASM");
            AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
        }

        public void Build(
            PanelType panelType, PanelProfile profile, Vector2 sizePanel,
            Materials OuterMaterial, Materials InnerMaterial,
            double outThickness, double innerThickness
            )
        {
            this.sizePanel = sizePanel;

            #region calculate panel dimention by profile

            switch (profile)
            {
                case PanelProfile.Profile_3_0:
                    innerHeight = sizePanel.Y - 7;
                    innerWeidht = sizePanel.X - 7;
                    leght = 27;
                    deepInsulation = 25;
                    break;

                case PanelProfile.Profile_5_0:
                    innerHeight = sizePanel.X - 10;
                    innerWeidht = sizePanel.Y - 10;
                    leght = 48;
                    deepInsulation = 45;
                    break;

                case PanelProfile.Profile_7_0:
                    innerHeight = sizePanel.X - 10;
                    innerWeidht = sizePanel.Y - 10;
                    leght = 50;
                    deepInsulation = 45;
                    break;
            }
            #endregion


            #region calculate step by  rivet
            double halfWidthPanel = Convert.ToDouble(sizePanel.X / 2);
            // Шаг заклепок
            const double step = 80;
            double rivetW = (Math.Truncate(sizePanel.X / step) + 1) * 1000;
            double rivetWd = (Math.Truncate(halfWidthPanel / step) + 1) * 1000;
            double rivetH = (Math.Truncate(sizePanel.Y / step) + 1) * 1000;
            if (Math.Abs(rivetW - 1000) < 1)
            {
                rivetW = 2000;
            }



            #endregion

            #region  calculate distance between the handles
            double widthHandle = sizePanel.X / 2;
            if (sizePanel.X < 1000)
            {
                widthHandle = sizePanel.X * 0.5;
            }
            if (sizePanel.X >= 1000)
            {
                widthHandle = sizePanel.X * 0.45;
            }
            if (sizePanel.X >= 1300)
            {
                widthHandle = sizePanel.X * 0.4;
            }
            if (sizePanel.X >= 1700)
            {
                widthHandle = sizePanel.X * 0.35;
            }
            #endregion


            OpenTemplate();
            DeleteComponents((int)panelType);


            if (panelType == PanelType.DualBlankPanel || panelType == PanelType.DualRemovablePanel)
            {

                DoublePanel(panelType);
            }
            else
            {
                SinglePanel(panelType);
            }
            Insulation();
            // ============================
            // Уплотнитель
            // шумотепло изоляция
            // сохранить сборку 
            // ===========================




            documentlName = "02-01-001"; // имя детали для внешней панели

            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, documentlName);
            Console.WriteLine(NewPartPath);
            //return;
            if (false)
            {
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(documentlName + ".SLDPRT");
                SolidWorksDocument.Extension.SelectByID2("02-01-001-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(NewPartPath, "", false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-001.SLDPRT");
            }
            else
            {

                // outer panel
                if (SetBends != null)
                    SetBends((decimal)outThickness, out KFactor, out BendRadius);
                base.parameters.Add("D1@Эскиз1", sizePanel.Y);
                base.parameters.Add("D2@Эскиз1", sizePanel.X);
                base.parameters.Add("D1@Кривая2", rivetH);
                base.parameters.Add("D1@Кривая1", rivetW);
                base.parameters.Add("D4@Эскиз30", widthHandle);
                base.parameters.Add("D7@Ребро-кромка1", leght);
                base.parameters.Add("Толщина@Листовой металл", outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                EditPartParameters("02-01-001", NewPartPath);


                documentlName = "02-01-002"; // имя детали для внутреней панели

                NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, documentlName);
                if (false)
                {
                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
                    SolidWorksDocument.Extension.SelectByID2("02-01-002-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(NewPartPath, "", false, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-002.SLDPRT");
                }
                else
                {
                    if (SetBends != null)
                        SetBends((decimal)innerThickness, out KFactor, out BendRadius);
                    base.parameters.Add("D1@Эскиз1", innerWeidht);
                    base.parameters.Add("D2@Эскиз1", innerHeight);
                    base.parameters.Add("D1@Кривая2", rivetW);
                    base.parameters.Add("D1@Кривая1", rivetH);
                    base.parameters.Add("Толщина@Листовой металл", innerThickness);
                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    EditPartParameters("02-01-002", NewPartPath);

                }



                documentlName = "sborka";
                ModelDoc2 asm = AssemblyDocument as ModelDoc2;
                NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, documentlName + ".SLDASM");
                asm.Extension.SaveAs(NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, null, ref errors, warnings);

                InitiatorSaveExeption(errors, warnings, NewPartPath);

            }
        }
        /// <summary>
        /// Build Insulation
        /// </summary>
        private void Insulation()
        {
            documentlName = "02-01-003"; // имя элемент - теплошумоизоляции 

            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, documentlName);

            if (false)
            {
                SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
                SolidWorksDocument.Extension.SelectByID2("02-01-003-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(NewPartPath, "", false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-003.SLDPRT");
            }
            else
            {
                base.parameters.Add("D1@Эскиз1", innerWeidht);
                base.parameters.Add("D2@Эскиз1", innerHeight);
                base.parameters.Add("D1@Бобышка-Вытянуть1", deepInsulation);
                // TO DO change lenght by profile, propertis
                EditPartParameters("02-01-003", NewPartPath);
            }

            documentlName = "02-01-004";
            // TO DO properties
            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, documentlName);
            if (false)
            {
                SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
                SolidWorksDocument.Extension.SelectByID2("02-01-004-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(NewPartPath, "", false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-004.SLDPRT");
            }
            else
            {
                base.parameters.Add("D6@Эскиз1", innerWeidht);
                base.parameters.Add("D3@Эскиз1", innerHeight);
                EditPartParameters("02-01-004", NewPartPath);

            }
        }
 

        private void SinglePanel (PanelType panelType)
        {
            documentlName = "02-01";
            
        }
        private void DoublePanel(PanelType panelType)
        {
            documentlName = "02-104-50";
          


           

            //    var currDestPath = typeOfPanel[1].Contains("несъемная") ? _destinationFolder : Panels0204;
             //   var curNumber = typeOfPanel[1].Contains("несъемная") ? "01" : "04";

                
                if (panelType == PanelType.DualBlankPanel)
                {
                    
                    
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-1@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-2@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    // Удаление ненужных элементов панели
                    const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                             (int)swDeleteSelectionOptions_e.swDelete_Children;
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@02-01-101-50-1@02-104-50", "BODYFEATURE", 0, 0, 0,
                        false, 0, null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                 
                }


              //  var newName =                    $"{modelName}-{curNumber}-{width}-{height}-{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}";

               var newPartPath =         $@"{RootFolder}{SubjectDestinationFolder}\{documentlName}.SLDPRT";

                 

                if (File.Exists(newPartPath))
                {
                    SolidWorksDocument = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    SolidWorksDocument.Extension.SelectByID2("02-01-101-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-101-50.SLDPRT");
                }
                else  
                {
                    SwPartParamsChangeWithNewName("02-01-101-50",
                        $@"{Settings.Default.DestinationFolder}{currDestPath}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightD).ToString()},
                            {"D2@Эскиз1", (widthD/2).ToString()},
                            {"D1@Кривая4", (rivetH).ToString()},
                            {"D1@Кривая3", (rivetWd).ToString()},
                            {"D1@Кривая5", (rivetH).ToString()},


                            {"D7@Ребро-кромка2", thicknessOfPanel == "50" ? "48" : "50"},


                            {"D2@Эскиз47", (wR/2).ToString()},

                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')},
                            {"D1@Листовой металл", (BendRadius).ToString()},
                            {"D2@Листовой металл", (KFactor*1000).ToString()}
                        },
                        false,
                        null);
                  
                   SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(documentlName);
                }
 

              //  modelName =                    $"{modelName}-02-{width}-{height}-50-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    SolidWorksDocument = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    SolidWorksDocument.Extension.SelectByID2("02-01-102-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-102-50.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-102-50",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightD - 10).ToString()},
                            {"D2@Эскиз1", ((widthD - 10)/2).ToString()},
                            {"D1@Кривая3", (rivetH).ToString()},
                            {"D1@Кривая2", (rivetH).ToString()},
                            {"D1@Кривая1", (rivetWd).ToString()},
                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},
                            {"D1@Листовой металл", (BendRadius).ToString()},
                            {"D2@Листовой металл", (KFactor*1000).ToString()}
                        },
                        false,
                        null);

                    
                  
                    _swApp.CloseDoc(newName);
                }

              
                newName =
                    $"{documentlName}-03-{width}-{height}-{thicknessOfPanel}-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    SolidWorksDocument = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    SolidWorksDocument.Extension.SelectByID2("02-01-103-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-103-50.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-103-50",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightD - 15).ToString()},
                            {"D1@Кривая1", (rivetH).ToString()},

                            {"D2@Эскиз1", thicknessOfPanel == "50" ? "46" : "48"},

                            {"Толщина@Листовой металл", thiknessStr},
                            {"D1@Листовой металл", (BendRadius).ToString()},
                            {"D2@Листовой металл", (KFactor*1000).ToString()}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }

                

               
             
        }
        protected override void DeleteComponents(int type)
        {
                PanelType eType = (PanelType)type;
                int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

                if (eType == PanelType.BlankPanel)
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    // Удаление ненужных элементов панели

                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
                        0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    // Удаление ненужных элементов панели
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
                        0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                }


            if (eType == PanelType.DualRemovablePanel || eType == PanelType.RemovablePanel)
            {
                if (sizePanel.X > 750)
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    // Удаление ненужных элементов панели
                     
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
                        null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                }
                else
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    // Удаление ненужных элементов панели
                    
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
                        null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                }
            }
        }
            
    }
}
















//        /// <summary>
//        /// Panels50s the build string.
//        /// </summary>
//        /// <param name="typeOfPanel">The type of panel.</param>
//        /// <param name="width">The width.</param>
//        /// <param name="height">The lenght.</param>
//        /// <param name="materialP1">The material p1.</param>
//        /// <param name="materialP2">The material p2.</param>
//        /// <param name="coating">The coating.</param>
//        /// <param name="onlyPath">if set to <c>true</c> [only path].</param>
//        /// <returns></returns>
//        public void Panels50BuildStr(string[] typeOfPanel, PanelType panelType, double widthPanel, double heightPanel, string[] materialP1, string[] materialP2, string[] coating, bool onlyPath)
//        {
//            var thicknessOfPanel = typeOfPanel[2];
//            string modelPanelsPath;
//            string modelName;
//            string nameAsm;
//            var modelType =  $"{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}-MW";
             
//            switch (panelType)
//            {
//                case PanelType.NotRemovableBlankPanel:
//                    modelPanelsPath = Panel50Folder;
//                    nameAsm = "02-01";
//                    modelName = "02-01";
//                    _destinationFolder = Panels0201;
//                    break;

//                case PanelType.PanelRemovable:
//                    modelPanelsPath = Panel50Folder;
//                    nameAsm = "02-01";
//                    modelName = "02-04";
//                    _destinationFolder = Panels0204;
//                    break;
//                case PanelType.DualNotRemovablePanel:
//                    modelPanelsPath = DublePanel50Folder;
//                    nameAsm = "02-104-50";
//                    modelName = "02-01";
//                    _destinationFolder = Panels0201;
//                    break;
//                case PanelType.DualRemovablePpanel:
//                    modelPanelsPath = DublePanel50Folder;
//                    nameAsm = "02-104-50";
//                    modelName = "02-01";
//                    _destinationFold = Panels0201;
//                    break;

//            }


//            //var newPanel50Name = "02-" + typeOfPanel[0] + "-" + idAsm;

//            var newDestPath = !typeOfPanel[1].Contains("Панель двойная съемная") ? _destinationFolder : Panels0204;
//            var newModNumber = !typeOfPanel[1].Contains("Панель двойная съемная") ? modelName : "02-04";

//            var newPanel50Name = newModNumber + "-" + widthPanel + "-" + heightPanel + "-" + modelType;


//            var newPanel50Path = $@"{RootFolder}{newDestPath}\{newPanel50Name}.SLDASM";



//            #region modelPanelAsmbly        

//            var modelPanelAsmbly = $@"{RootFolder}{modelPanelsPath}\{nameAsm}.SLDASM";

//            var swDoc = SolidWorksAdapter.OpenDocument(modelPanelAsmbly, swDocumentTypes_e.swDocASSEMBLY);



//            var swAsm = SolidWorksAdapter.ToAssemblyDocument(swDoc);

//            // Габариты

//            double halfWidthPanel = Convert.ToDouble(widthPanel / 2);
//            // Шаг заклепок
//            const double step = 80;
//            double rivetW = (Math.Truncate(widthPanel / step) + 1) * 1000;
//            double rivetWd = (Math.Truncate(halfWidthPanel / step) + 1) * 1000;
//            double rivetH = (Math.Truncate(heightPanel / step) + 1) * 1000;
//            if (Math.Abs(rivetW - 1000) < 1) rivetW = 2000;
//            // Коэффициенты и радиусы гибов   
//            const decimal thiknessStr = 0.8m;
//            decimal BendRadius;
//            decimal KFactor;
//            SetBends(thiknessStr, out KFactor, out BendRadius);
//            #endregion

//            // Переменные панели с ручками

//            #region   Расстояние межу ручками
//            double widthHandle = widthPanel / 2;
//            if (widthPanel < 1000)
//            {
//                widthHandle = widthPanel * 0.5;
//            }
//            if (widthPanel >= 1000)
//            {
//                widthHandle = widthPanel * 0.45;
//            }
//            if (widthPanel >= 1300)
//            {
//                widthHandle = widthPanel * 0.4;
//            }
//            if (widthPanel >= 1700)
//            {
//                widthHandle = widthPanel * 0.35;
//            }
//            #endregion

//            #region typeOfPanel != "Панель двойная"
//            int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
//                                         (int)swDeleteSelectionOptions_e.swDelete_Children; ;
//            // Тип панели
//            if (modelName == "02-01" & !typeOfPanel[1].Contains("Панель двойная"))
//            {
//                swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();

//                // Удаление ненужных элементов панели

//                swDoc.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
//                    0);
//                swDoc.Extension.DeleteSelection2(deleteOption);

//                swDoc.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                swDoc.EditDelete();
//                swDoc.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                swDoc.EditDelete();
//                // Удаление ненужных элементов панели
//                swDoc.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
//                    0);
//                swDoc.Extension.DeleteSelection2(deleteOption);
//            }

//            if (modelName == "02-04" || modelName == "02-05")
//            {
//                if (Convert.ToInt32(widthPanel) > 750)
//                {
//                    swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    // Удаление ненужных элементов панели

//                    swDoc.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
//                        null, 0);
//                    swDoc.Extension.DeleteSelection2(deleteOption);
//                }
//                else
//                {
//                    swDoc.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                    swDoc.EditDelete();
//                    swDoc.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swDoc.EditDelete();
//                    // Удаление ненужных элементов панели

//                    swDoc.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
//                        null, 0);
//                    swDoc.Extension.DeleteSelection2(deleteOption);
//                }
//            }

//            if (!typeOfPanel[1].Contains("Панель двойная"))
//            {
//                // Панель внешняя 


//                var newName =
//                    $"{modelName}-01-{width}-{height}-{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}";


//                var newPartPath = $@"{RootFolder}{_destinationFolder}\{newName}.SLDPRT";
//                if (File.Exists(newPartPath))
//                {
//                    swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
//                    swDoc.Extension.SelectByID2("02-01-001-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                    swAsm.ReplaceComponents(newPartPath, "", false, true);
//                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-001.SLDPRT");
//                }
//                else if (File.Exists(newPartPath) != true)
//                {

//                    EditPartParameters("02-01-001",
//                        $@"{RootFolder}{_destinationFolder}\{newName}",
//                        new[,]
//                        {
//                            {"D1@Эскиз1", (heightPanel).ToString()},
//                            {"D2@Эскиз1", (widthPanel).ToString()},
//                            {"D1@Кривая2", (rivetH).ToString()},
//                            {"D1@Кривая1", (rivetW).ToString()},
//                            {"D4@Эскиз30", (widthHandle).ToString()},

//                            {"D7@Ребро-кромка1", thicknessOfPanel == "50" ? "48" : "50"},

//                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')}
//                        });


//                    //Панель внутреняя

//                    var modelnewname = modelName;
//                    var modelPath = _destinationFolder;
//                    if (modelName == "02-04")
//                    {
//                        modelnewname = "02-01";
//                        modelPath = Panels0201;
//                    }

//                    newName =
//                        $"{modelnewname}-02-{width}-{height}-50-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";



//                    newPartPath = $@"{RootFolder}{modelPath}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-002-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-002.SLDPRT");
//                    }
//                    else if (!File.Exists(newPartPath))
//                    {



//                        EditPartParameters("02-01-002",
//                        $@"{RootFolder}{modelPath}\{newName}",
//                        new[,]
//                        {
//                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
//                            {"D2@Эскиз1", (widthPanel - 10).ToString()},
//                            {"D1@Кривая2", (rivetH).ToString()},
//                            {"D1@Кривая1", (rivetW).ToString()},
//                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},
//                            {"D1@Листовой металл", (BendRadius).ToString()},
//                            {"D2@Листовой металл", (KFactor*1000).ToString()}
//                        });

//                    }

//                    //Панель теплошумоизоляции

//                    if (modelName == "02-05")
//                    {
//                        modelPath = Panels0201;
//                    }

//                    newName = "02-03-" + width + "-" + height; //newName = теплоизоляция.NewName;
//                    newPartPath = $@"{RootFolder}{modelPath}\{newName}.SLDPRT";

//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-003-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-003.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-003",
//                        $@"{RootFolder}{_destinationFolder}\{newName}",
//                        new[,]
//                        {
//                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
//                            {"D2@Эскиз1", (widthPanel - 10).ToString()}
//                        });

//                    }

//                    //Уплотнитель

//                    newName = "02-04-" + width + "-" + height; //newName = уплотнитель.NewName;

//                    newPartPath = $@"{RootFolder}{modelPath}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-01.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-004-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-004.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-004",
//                        $@"{RootFolder}{_destinationFolder}\{newName}",
//                        new[,]
//                        {
//                            {"D6@Эскиз1", (heightPanel - 10).ToString()},
//                            {"D3@Эскиз1", (widthPanel - 10).ToString()}
//                        });

//                    }
//                }

//                #endregion

//                #region typeOfPanel == "Панель двойная несъемная"

//                if (typeOfPanel[1].Contains("Панель двойная"))
//                {

//                    var currDestPath = typeOfPanel[1].Contains("несъемная") ? _destinationFolder : Panels0204;
//                    var curNumber = typeOfPanel[1].Contains("несъемная") ? "01" : "04";



//                    if (typeOfPanel[1].Contains("несъемная"))
//                    {


//                        swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swDoc.EditDelete();
//                        swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null,
//                            0);
//                        swDoc.EditDelete();
//                        swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                        swDoc.EditDelete();
//                        swDoc.Extension.SelectByID2("Threaded Rivets-1@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                        swDoc.EditDelete();
//                        swDoc.Extension.SelectByID2("Threaded Rivets-2@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swDoc.EditDelete();
//                        // Удаление ненужных элементов панели

//                        swDoc.Extension.SelectByID2("Вырез-Вытянуть11@02-01-101-50-1@02-104-50", "BODYFEATURE", 0, 0, 0,
//                            false, 0, null, 0);
//                        swDoc.Extension.DeleteSelection2(deleteOption);

//                    }


//                    var newName =
//                        $"{modelName}-{curNumber}-{width}-{height}-{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}";

//                    var newPartPath =
//                        $@"{RootFolder}{currDestPath}\{newName}.SLDPRT";



//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-104-50.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-101-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-101-50.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-101-50",
//                            $@"{RootFolder}{currDestPath}\{newName}",
//                            new[,]
//                            {
//                            {"D1@Эскиз1", (heightPanel).ToString()},
//                            {"D2@Эскиз1", (widthPanel/2).ToString()},
//                            {"D1@Кривая4", (rivetH).ToString()},
//                            {"D1@Кривая3", (rivetWd).ToString()},
//                            {"D1@Кривая5", (rivetH).ToString()},


//                            {"D7@Ребро-кромка2", thicknessOfPanel == "50" ? "48" : "50"},


//                            {"D2@Эскиз47", (widthHandle/2).ToString()},

//                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')},
//                            {"D1@Листовой металл", (BendRadius).ToString()},
//                            {"D2@Листовой металл", (KFactor*1000).ToString()}
//                            });


//                    }



//                    newName =
//                        $"{modelName}-02-{width}-{height}-50-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

//                    newPartPath = $@"{RootFolder}{_destinationFolder}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-104-50.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-102-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-102-50.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-102-50",
//                            $@"{RootFolder}{_destinationFolder}\{newName}",
//                            new[,]
//                            {
//                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
//                            {"D2@Эскиз1", ((widthPanel - 10)/2).ToString()},
//                            {"D1@Кривая3", (rivetH).ToString()},
//                            {"D1@Кривая2", (rivetH).ToString()},
//                            {"D1@Кривая1", (rivetWd).ToString()},
//                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},
//                            {"D1@Листовой металл", (BendRadius).ToString()},
//                            {"D2@Листовой металл", (KFactor*1000).ToString()}
//                            });


//                    }


//                    newName =
//                        $"{modelName}-03-{width}-{height}-{thicknessOfPanel}-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

//                    newPartPath = $@"{RootFolder}{_destinationFolder}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-104-50.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-103-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-103-50.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-103-50",
//                            $@"{RootFolder}{_destinationFolder}\{newName}",
//                            new string[,]
//                            {
//                            {"D1@Эскиз1", (heightPanel - 15).ToString()},
//                            {"D1@Кривая1", (rivetH).ToString()},

//                            {"D2@Эскиз1", thicknessOfPanel == "50" ? "46" : "48"},

//                            {"Толщина@Листовой металл", thiknessStr},
//                            {"D1@Листовой металл", (BendRadius).ToString()},
//                            {"D2@Листовой металл", (KFactor*1000).ToString()}
//                            });
//                    }

//                    //Панель теплошумоизоляции

//                    //newName = теплоизоляция.NewName;

//                    newName = "02-03-" + widthPanel + "-" + heightPanel;

//                    newPartPath = $@"{RootFolder}{_destinationFolder}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-104-50.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-003-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-003.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-003",
//                            $@"{RootFolder}{_destinationFolder}\{newName}",
//                            new[,]
//                            {
//                            {"D1@Эскиз1", Convert.ToString(heightPanel - 10)},
//                            {"D2@Эскиз1", Convert.ToString(widthPanel - 10)}
//                            });

//                    }

//                    //Уплотнитель

//                    //newName = уплотнитель.NewName;

//                    newName = "02-04-" + widthPanel + "-" + heightPanel;

//                    newPartPath = $@"{RootFolder}{_destinationFolder}\{newName}.SLDPRT";
//                    if (File.Exists(newPartPath))
//                    {
//                        swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2("02-104-50.SLDASM", true, 0)));
//                        swDoc.Extension.SelectByID2("02-01-004-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                        swAsm.ReplaceComponents(newPartPath, "", false, true);
//                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-01-004.SLDPRT");
//                    }
//                    else if (File.Exists(newPartPath) != true)
//                    {
//                        EditPartParameters("02-01-004",
//                            $@"{RootFolder}{_destinationFolder}\{newName}",
//                            new[,]
//                            {
//                            {"D6@Эскиз1", Convert.ToString(heightPanel - 10)},
//                            {"D3@Эскиз1", Convert.ToString(widthPanel - 10)}
//                            });

//                    }
//                }


//                swDoc = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(nameAsm, true, 0)));
//                var swModelDocExt = swDoc.Extension;
//                var swCustPropForDescription = swModelDocExt.CustomPropertyManager[""];
//                swCustPropForDescription.Set("Наименование", typeOfPanel[1]);
//                swCustPropForDescription.Set("Description", typeOfPanel[1]);



//                swDoc.EditRebuild3();
//                swDoc.ForceRebuild3(true);
//                swDoc.SaveAs2(System.IO.Path.GetFileName( newPanel50Path)), (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false,
//                    true);
//                ComponentsPathList.Add(newPanel50Path);
//                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(new FileInfo(newPanel50Path).Name);
//                SolidWorksAdapter.SldWoksAppExemplare.Visible = true;

//            }
//        }

       
//    }
//}
