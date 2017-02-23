using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels
{ public delegate void SetBendsHandler(decimal _Thickness, out decimal KFactor, out decimal BendRadius); 
    class PanelBuilder : AbstractBuilder
    {
      public SetBendsHandler SetBends { get; set; }
        public PanelBuilder()
        {


        }

        /// <summary>
        /// Panels50s the build string.
        /// </summary>
        /// <param name="typeOfPanel">The type of panel.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The lenght.</param>
        /// <param name="materialP1">The material p1.</param>
        /// <param name="materialP2">The material p2.</param>
        /// <param name="покрытие">The ПОКРЫТИЕ.</param>
        /// <param name="onlyPath">if set to <c>true</c> [only path].</param>
        /// <returns></returns>
        public void Panels50BuildStr(string[] typeOfPanel , PanelTypes panelType , double widthPanel, double heightPanel, string[] materialP1, string[] materialP2, string[] покрытие, bool onlyPath)
        {
             
            var thicknessOfPanel = typeOfPanel[2];
             
            string modelPanelsPath;
            string modelName;
            string nameAsm;
            var modelType =
                $"{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}-MW";


            switch (panelType)
            {
                case PanelTypes.NotRemovableBlankPanel:
                    modelPanelsPath = Panel50Folder;
                    nameAsm = "02-01";
                    modelName = "02-01";
                    _destinationFolder = Panels0201;
                    break;

                case PanelTypes.PanelRemovable:
            }

            switch (typeOfPanel[1])
            {
                // add constants
                case "Панель несъемная глухая":
                    modelPanelsPath = Panel50Folder;
                    nameAsm = "02-01";
                    modelName = "02-01";
                    _destinationFolder = Panels0201;
                    break;
                case "Панель съемная с ручками":
                    modelPanelsPath = Panel50Folder;
                    nameAsm = "02-01";
                    modelName = "02-04";
                    _destinationFolder = Panels0204;
                    break;
                case "Панель теплообменника":
                    modelPanelsPath = Panel50Folder;
                    nameAsm = "02-01";
                    modelName = "02-05";
                    _destinationFolder = Panels0205;
                    break;
                case "Панель двойная несъемная":
                    modelPanelsPath = DublePanel50Folder;
                    nameAsm = "02-104-50";
                    modelName = "02-01";
                    _destinationFolder = Panels0201;
                    break;

                case "Панель двойная съемная":
                    modelPanelsPath = DublePanel50Folder;
                    nameAsm = "02-104-50";
                    modelName = "02-01";
                    _destinationFolder = Panels0201;
                    break;
                default:
                    modelPanelsPath = Panel50Folder;
                    nameAsm = "02-01";
                    modelName = "02-01";
                    break;
            }

            #region Обозначения и Хранилище

           

            #endregion

            //var newPanel50Name = "02-" + typeOfPanel[0] + "-" + idAsm;

            var newDestPath = !typeOfPanel[1].Contains("Панель двойная съемная") ? _destinationFolder : Panels0204;
            var newModNumber = !typeOfPanel[1].Contains("Панель двойная съемная") ? modelName : "02-04";

            var newPanel50Name = newModNumber + "-" + width + "-" + height + "-" + modelType;
             

            var newPanel50Path = $@"{Settings.Default.DestinationFolder}{newDestPath}\{newPanel50Name}.SLDASM";

       

            #region modelPanelAsmbly        

            var modelPanelAsmbly = $@"{Settings.Default.SourceFolder}{modelPanelsPath}\{nameAsm}.SLDASM";
            
            var swDoc = SolidWorksAdapter.OpenDocument(modelPanelAsmbly, swDocumentTypes_e.swDocASSEMBLY);

            

            var swAsm = SolidWorksAdapter.GetAssembly(swDoc);

            // Габариты
            
            double halfWidthPanel = Convert.ToDouble(widthPanel / 2);
            // Шаг заклепок
            const double step = 80;
            double rivetW = (Math.Truncate(widthPanel / step) + 1) * 1000;
            double rivetWd = (Math.Truncate(halfWidthPanel / step) + 1) * 1000;
            double rivetH = (Math.Truncate(heightPanel / step) + 1) * 1000;
            if (Math.Abs(rivetW - 1000) < 1) rivetW = 2000;
            // Коэффициенты и радиусы гибов   
            const decimal thiknessStr = 0.8m;
            decimal bendRadius;
            decimal kFactor;
            SetBends(thiknessStr, out kFactor, out bendRadius);
            #endregion

            // Переменные панели с ручками

            var widthHandle = widthPanel / 2; // Расстояние межу ручками
            if (widthPanel < 1000)
            {
                widthHandle = widthPanel * 0.5;
            }
            if (widthPanel >= 1000)
            {
                widthHandle = widthPanel * 0.45;
            }
            if (widthPanel >= 1300)
            {
                widthHandle = widthPanel * 0.4;
            }
            if (widthPanel >= 1700)
            {
                widthHandle = widthPanel * 0.35;
            }

            #region typeOfPanel != "Панель двойная"

            // Тип панели
            if (modelName == "02-01" & !typeOfPanel[1].Contains("Панель двойная"))
            {
                swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();

                // Удаление ненужных элементов панели
                const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                         (int)swDeleteSelectionOptions_e.swDelete_Children;
                swDoc.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
                    0);
                swDoc.Extension.DeleteSelection2(deleteOption);

                swDoc.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.EditDelete();
                swDoc.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
                // Удаление ненужных элементов панели
                swDoc.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null,
                    0);
                swDoc.Extension.DeleteSelection2(deleteOption);
            }

            if (modelName == "02-04" || modelName == "02-05")
            {
                if (Convert.ToInt32(width) > 750)
                {
                    swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    // Удаление ненужных элементов панели
                    const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                             (int)swDeleteSelectionOptions_e.swDelete_Children;
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
                        null, 0);
                    swDoc.Extension.DeleteSelection2(deleteOption);
                }
                else
                {
                    swDoc.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    // Удаление ненужных элементов панели
                    const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                             (int)swDeleteSelectionOptions_e.swDelete_Children;
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,
                        null, 0);
                    swDoc.Extension.DeleteSelection2(deleteOption);
                }
            }

            if (!typeOfPanel[1].Contains("Панель двойная"))
            {
                // Панель внешняя 

                #region

                //var newName = панельВнешняя.NewName;
                //var newName =modelName + "-01-" + width + "-" + lenght + "-" + "50-" + materialP1[3] + materialP1[3] == "AZ" ? "" : materialP1[1];

                #endregion

                var newName =
                    $"{modelName}-01-{width}-{height}-{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}";

                #region

                //var newName = String.Format("{0}{1}{2}{3}",
                //    modelName + "-01-" + width + "-" + lenght + "-" + "50-" + materialP1
                //    //string.IsNullOrEmpty(покрытие[6]) ? "" : "-" + покрытие[6],
                //    //string.IsNullOrEmpty(покрытие[1]) ? "" : "-" + покрытие[1],
                //    //string.IsNullOrEmpty(покрытие[2]) ? "" : "-" + покрытие[2]
                //    );

                #endregion

                var newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-01.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-001-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-001.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {

                    #region before

                    //swApp.SendMsgToUser("Изменяем компонент " + @NewPartPath);
                    //SetMeterial(materialP1[0], _swApp.ActivateDoc2("02-01-001.SLDPRT", true, 0), "");// _swApp.ActivateDoc2("02-01-001.SLDPRT"
                    //try
                    //{
                    //    var setMaterials = new SetMaterials();
                    //    _swApp.ActivateDoc2("02-11-01-40-.SLDPRT", true, 0);
                    //    setMaterials.SetColor("00", покрытие[6], покрытие[1], покрытие[2], _swApp);// setMaterials.SetColor("00", "F6F6F6", "Шаргень", "2", _swApp);
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show(e.StackTrace);
                    //}

                    #endregion

                    SwPartParamsChangeWithNewName("02-01-001",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel).ToString()},
                            {"D2@Эскиз1", (widthPanel).ToString()},
                            {"D1@Кривая2", (rivetH).ToString()},
                            {"D1@Кривая1", (rivetW).ToString()},
                            {"D4@Эскиз30", (widthHandle).ToString()},

                            {"D7@Ребро-кромка1", thicknessOfPanel == "50" ? "48" : "50"},

                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')}
                        },
                        false,
                        null);
                    try
                    {

                        VentsMatdll(materialP1, new[] { покрытие[6], покрытие[1], покрытие[2] }, newName);
                    }
                    catch (Exception e)
                    {
                       

                    _swApp.CloseDoc(newName);
                }

                //Панель внутреняя

                var modelnewname = modelName;
                var modelPath = _destinationFolder;
                if (modelName == "02-04")
                {
                    modelnewname = "02-01";
                    modelPath = Panels0201;
                }

                #region

                //newName = String.Format("{0}{1}{2}{3}",
                //modelnewname + "-02-" + width + "-" + lenght + "-" + "50-" + materialP2[0];
                //   //string.IsNullOrEmpty(покрытие[6]) ? "" : "-" + покрытие[6],
                //   //string.IsNullOrEmpty(покрытие[1]) ? "" : "-" + покрытие[1],
                //   //string.IsNullOrEmpty(покрытие[2]) ? "" : "-" + покрытие[2]
                //   );

                //newName = панельВнутренняя.NewName;

                #endregion

                newName =
                    $"{modelnewname}-02-{width}-{height}-50-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

                //newName = modelnewname + "-02-" + width + "-" + lenght + "-" + "50-" + materialP2[3] + materialP2[3] == "AZ" ? "" : materialP2[1];

                newPartPath = $@"{Settings.Default.DestinationFolder}{modelPath}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-01.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-002-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-002.SLDPRT");
                }
                else if (!File.Exists(newPartPath))
                {

                    #region before

                    //SetMeterial(materialP2[0], _swApp.ActivateDoc2("02-01-002.SLDPRT", true, 0), "");
                    //try
                    //{
                    //    var setMaterials = new SetMaterials();
                    //    _swApp.ActivateDoc2("02-11-01-40-.SLDPRT", true, 0);
                    //    setMaterials.SetColor("00", покрытие[7], покрытие[4], покрытие[5], _swApp);// setMaterials.SetColor("00", "F6F6F6", "Шаргень", "2", _swApp);
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show(e.StackTrace);
                    //}

                    #endregion

                    SwPartParamsChangeWithNewName("02-01-002",
                        $@"{Settings.Default.DestinationFolder}{modelPath}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
                            {"D2@Эскиз1", (widthPanel - 10).ToString()},
                            {"D1@Кривая2", (rivetH).ToString()},
                            {"D1@Кривая1", (rivetW).ToString()},
                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},
                            {"D1@Листовой металл", (bendRadius).ToString()},
                            {"D2@Листовой металл", (kFactor*1000).ToString()}
                        },
                        false,
                        null);
                    try
                    {
                        VentsMatdll(materialP2, new[] { покрытие[7], покрытие[4], покрытие[5] }, newName);
                    }
                    catch (Exception e)
                    {
                       
                    }
                    _swApp.CloseDoc(newName);
                }

                //Панель теплошумоизоляции

                if (modelName == "02-05")
                {
                    modelPath = Panels0201;
                }

                newName = "02-03-" + width + "-" + height; //newName = теплоизоляция.NewName;
                newPartPath = $@"{Settings.Default.DestinationFolder}{modelPath}\{newName}.SLDPRT";

                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-01.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-003-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-003.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-003",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
                            {"D2@Эскиз1", (widthPanel - 10).ToString()}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }

                //Уплотнитель

                newName = "02-04-" + width + "-" + height; //newName = уплотнитель.NewName;

                newPartPath = $@"{Settings.Default.DestinationFolder}{modelPath}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-01.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-004-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-004.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-004",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D6@Эскиз1", (heightPanel - 10).ToString()},
                            {"D3@Эскиз1", (widthPanel - 10).ToString()}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }
            }

            #endregion

            #region typeOfPanel == "Панель двойная несъемная"

            if (typeOfPanel[1].Contains("Панель двойная"))
            {

                #region before

                // Панель внешняя 
                //var newName = String.Format("{0}{1}{2}{3}",
                //    modelName + "-01-" + width + "-" + lenght + "-" + "50-" + materialP1
                //    //string.IsNullOrEmpty(покрытие[6]) ? "" : "-" + покрытие[6],
                //    //string.IsNullOrEmpty(покрытие[1]) ? "" : "-" + покрытие[1],
                //    //string.IsNullOrEmpty(покрытие[2]) ? "" : "-" + покрытие[2]
                //    );

                //var newName = панельВнешняя.NewName;
                //var newName = modelName + "-01-" + width + "-" + lenght + "-" + "50-" + materialP1[3] + materialP1[3] == "AZ" ? "" : materialP1[1];

                #endregion

                var currDestPath = typeOfPanel[1].Contains("несъемная") ? _destinationFolder : Panels0204;
                var curNumber = typeOfPanel[1].Contains("несъемная") ? "01" : "04";

                //MessageBox.Show("currDestPath - " + currDestPath + "   curNumber -" + curNumber + " -- " + typeOfPanel[1].Contains("несъемная"));

                if (typeOfPanel[1].Contains("несъемная"))
                {
                    //MessageBox.Show("Не съемная - " + typeOfPanel[1]);

                    swDoc.Extension.SelectByID2("Ручка MLA 120-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null,
                        0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("SC GOST 17475_gost-2@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-1@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDoc.EditDelete();
                    swDoc.Extension.SelectByID2("Threaded Rivets-2@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDoc.EditDelete();
                    // Удаление ненужных элементов панели
                    const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed +
                                             (int)swDeleteSelectionOptions_e.swDelete_Children;
                    swDoc.Extension.SelectByID2("Вырез-Вытянуть11@02-01-101-50-1@02-104-50", "BODYFEATURE", 0, 0, 0,
                        false, 0, null, 0);
                    swDoc.Extension.DeleteSelection2(deleteOption);

                    //MessageBox.Show("Удалилось");
                }


                var newName =
                    $"{modelName}-{curNumber}-{width}-{height}-{thicknessOfPanel}-{materialP1[3]}{(materialP1[3] == "AZ" ? "" : materialP1[1])}";

                var newPartPath =
                    $@"{Settings.Default.DestinationFolder}{currDestPath}\{newName}.SLDPRT";

                //MessageBox.Show("newPartPath -" + newPartPath);

                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-101-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-101-50.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-101-50",
                        $@"{Settings.Default.DestinationFolder}{currDestPath}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel).ToString()},
                            {"D2@Эскиз1", (widthPanel/2).ToString()},
                            {"D1@Кривая4", (rivetH).ToString()},
                            {"D1@Кривая3", (rivetWd).ToString()},
                            {"D1@Кривая5", (rivetH).ToString()},


                            {"D7@Ребро-кромка2", thicknessOfPanel == "50" ? "48" : "50"},


                            {"D2@Эскиз47", (widthHandle/2).ToString()},

                            {"Толщина@Листовой металл", materialP1[1].Replace('.', ',')},
                            {"D1@Листовой металл", (bendRadius).ToString()},
                            {"D2@Листовой металл", (kFactor*1000).ToString()}
                        },
                        false,
                        null);
                    try
                    {
                        VentsMatdll(materialP1, new[] { покрытие[6], покрытие[1], покрытие[2] }, newName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    _swApp.CloseDoc(newName);
                }

                #region Name before

                //Панель внутреняя
                //newName = String.Format("{0}{1}{2}{3}",
                //   modelName + "-02-" + width + "-" + lenght + "-" + "50-" + materialP1
                //   //string.IsNullOrEmpty(покрытие[7]) ? "" : "-" + покрытие[7],
                //   //string.IsNullOrEmpty(покрытие[3]) ? "" : "-" + покрытие[3],
                //   //string.IsNullOrEmpty(покрытие[4]) ? "" : "-" + покрытие[4]
                //   );

                //newName = панельВнутренняя.NewName;

                //newName = modelName + "-02-" + width + "-" + lenght + "-" + "50-" + materialP2[3] + materialP2[3] == "AZ" ? "" : materialP2[1];

                #endregion

                newName =
                    $"{modelName}-02-{width}-{height}-50-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-102-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-102-50.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-102-50",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel - 10).ToString()},
                            {"D2@Эскиз1", ((widthPanel - 10)/2).ToString()},
                            {"D1@Кривая3", (rivetH).ToString()},
                            {"D1@Кривая2", (rivetH).ToString()},
                            {"D1@Кривая1", (rivetWd).ToString()},
                            {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},
                            {"D1@Листовой металл", (bendRadius).ToString()},
                            {"D2@Листовой металл", (kFactor*1000).ToString()}
                        },
                        false,
                        null);

                    try
                    {
                        // MessageBox.Show(materialP1+" " +   покрытие[7] + " " + покрытие[4] + " " + покрытие[5] +" " + newName);
                        VentsMatdll(materialP1, new[] { покрытие[7], покрытие[4], покрытие[5] }, newName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }

                    _swApp.CloseDoc(newName);
                }

                #region

                // Профиль 02-01-103-50
                //newName = деталь103.NewName;
                // modelName + "-03-" + width + "-" + lenght + "-" + "50-" + materialP2[3] + materialP2[3] == "AZ" ? "" : materialP2[1];

                #endregion

                newName =
                    $"{modelName}-03-{width}-{height}-{thicknessOfPanel}-{materialP2[3]}{(materialP2[3] == "AZ" ? "" : materialP2[1])}";

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-103-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-103-50.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-103-50",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", (heightPanel - 15).ToString()},
                            {"D1@Кривая1", (rivetH).ToString()},

                            {"D2@Эскиз1", thicknessOfPanel == "50" ? "46" : "48"},

                            {"Толщина@Листовой металл", thiknessStr},
                            {"D1@Листовой металл", (bendRadius).ToString()},
                            {"D2@Листовой металл", (kFactor*1000).ToString()}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }

                //Панель теплошумоизоляции

                //newName = теплоизоляция.NewName;

                newName = "02-03-" + width + "-" + height;

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-003-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-003.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-003",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D1@Эскиз1", Convert.ToString(heightPanel - 10)},
                            {"D2@Эскиз1", Convert.ToString(widthPanel - 10)}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }

                //Уплотнитель

                //newName = уплотнитель.NewName;

                newName = "02-04-" + width + "-" + height;

                newPartPath = $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}.SLDPRT";
                if (File.Exists(newPartPath))
                {
                    swDoc = ((ModelDoc2)(_swApp.ActivateDoc2("02-104-50.SLDASM", true, 0)));
                    swDoc.Extension.SelectByID2("02-01-004-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                    _swApp.CloseDoc("02-01-004.SLDPRT");
                }
                else if (File.Exists(newPartPath) != true)
                {
                    SwPartParamsChangeWithNewName("02-01-004",
                        $@"{Settings.Default.DestinationFolder}{_destinationFolder}\{newName}",
                        new[,]
                        {
                            {"D6@Эскиз1", Convert.ToString(heightPanel - 10)},
                            {"D3@Эскиз1", Convert.ToString(widthPanel - 10)}
                        },
                        false,
                        null);
                    _swApp.CloseDoc(newName);
                }
            }

            #endregion

            #region Несъемная

            //switch (typeOfPanel[1])
            //{
            //    case "Несъемная":
            //    case "Съемная":
            //        typeOfPanel[1] = typeOfPanel[1] + " панель";
            //        break;
            //    case "Панель теплообменника":
            //    case "Панель двойная":
            //        break;
            //}

            #endregion

            swDoc = ((ModelDoc2)(_swApp.ActivateDoc2(nameAsm, true, 0)));
            var swModelDocExt = swDoc.Extension;
            var swCustPropForDescription = swModelDocExt.CustomPropertyManager[""];
            swCustPropForDescription.Set("Наименование", typeOfPanel[1]);
            swCustPropForDescription.Set("Description", typeOfPanel[1]);

            //GabaritsForPaintingCamera(swDoc);

            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(true);
            swDoc.SaveAs2(new FileInfo(newPanel50Path).FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false,
                true);
            NewComponents.Add(new FileInfo(newPanel50Path));
            _swApp.CloseDoc(new FileInfo(newPanel50Path).Name);
            _swApp.Visible = true;
            CheckInOutPdm(NewComponents, true, Settings.Default.TestPdmBaseName);

            foreach (var newComponent in NewComponents)
            {
                // ExportXmlSql.Export(newComponent.FullName);
            }

            if (onlyPath) return newPanel50Path;
            MessageBox.Show(newPanel50Path, "Модель построена");

            return newPanel50Path;
        }

        protected override void DeleteComponents(int type)
        {
            base.DeleteComponents(type);
        }
    }
}
