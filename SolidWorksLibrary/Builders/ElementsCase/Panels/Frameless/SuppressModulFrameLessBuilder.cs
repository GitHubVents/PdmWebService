//using ServiceConstants;
//using SolidWorks.Interop.sldworks;
//using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;
//using System;

//namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless {
//    public partial class FramelessPanelBuilder {
//        private void Suppress() {         
             

//            #region For others panels
//            else {

//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress2();
//                    SolidWorksDocument.EditRebuild3();
//                }
//                else if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    DocumentExtension.SelectByID2("Кривая3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress2();
//                }

//                DocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
//                DocumentExtension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                DocumentExtension.SelectByID2("Кривая6@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditUnsuppress2(); 

//            }
//            #endregion

//            #region 04 05 - Съемные панели            

//            if (configuration.Contains("01")) {


//                DocumentExtension.SelectByID2("4-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                SolidWorksDocument.EditRebuild3();
//                DocumentExtension.SelectByID2("1-0@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                SolidWorksDocument.EditRebuild3();

//                // Погашение отверстий под клепальные гайки
//                DocumentExtension.SelectByID2("Вырез-Вытянуть20@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                DocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                // В усиливающих профилях
//                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                SolidWorksDocument.ClearSelection2(true);
//            }


//            if (configuration.Contains("02")) {
//                DocumentExtension.SelectByID2("Threaded Rivets-37@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditDelete();
//                DocumentExtension.SelectByID2("Threaded Rivets-47@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditDelete();
//                DocumentExtension.SelectByID2("Threaded Rivets-52@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditDelete();

//                DocumentExtension.SelectByID2("2-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                SolidWorksDocument.EditRebuild3();
//                DocumentExtension.SelectByID2("1-1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                SolidWorksDocument.EditRebuild3();

//                // Погашение отверстий под клепальные гайки
//                DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                DocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                // В усиливающих профилях
//                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress2();

//                SolidWorksDocument.ClearSelection2(true);

//            }

//            #endregion

//            #region Удаление усиления для типоразмера меньше AV09

//            if (!framelessPanel.усиление) {

//            }

//            #endregion

//            #region Отверстия под панели L2 L3

//            if (Convert.ToInt32(OutputHolesWrapper.L2) == 28) {


//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);

//                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая5@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);

//                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая5@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);
//            }


//            if (Convert.ToInt32(OutputHolesWrapper.L3) == 28) {


//                DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-Вытянуть20@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);

//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая6@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);

//                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Кривая6@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                SolidWorksDocument.ClearSelection2(true);
//            }
//            #endregion

//            #region Панели усиливающие

//            string типКрепежнойЧастиУсиливающейПанели = null;
//            var типТорцевойЧастиУсиливающейПанели = "T";

//            if (!string.IsNullOrEmpty(ТипУсиливающей())) {

//                типТорцевойЧастиУсиливающейПанели = ТипУсиливающей().Remove(1).Contains("T") ? "T" : "E";
//                if (ТипУсиливающей().Remove(0, 1).Contains("E")) {
//                    типКрепежнойЧастиУсиливающейПанели = "E";
//                }
//                if (ТипУсиливающей().Remove(0, 1).Contains("D")) {
//                    типКрепежнойЧастиУсиливающейПанели = "D";
//                }
//                if (ТипУсиливающей().Remove(0, 1).Contains("E")) {
//                    типКрепежнойЧастиУсиливающейПанели = "E";
//                }
//                if (ТипУсиливающей().Remove(0, 1).Contains("Z")) {
//                    типКрепежнойЧастиУсиливающейПанели = "Z";
//                }
//            }

//            if (framelessPanel.SizePanel.Y < 825) {
//                DocumentExtension.SelectByID2("UpperAV09@02-11-09-40--1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (типТорцевойЧастиУсиливающейПанели == "E") {

//                DocumentExtension.SelectByID2("Эскиз42@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();

//            }

//            if (типКрепежнойЧастиУсиливающейПанели != "D") {
//                foreach (var component in new[]
//                {
//                                "02-11-09-40--1",
//                                "Threaded Rivets с насечкой-1", "Threaded Rivets с насечкой-2",
//                                "Threaded Rivets с насечкой-3", "Threaded Rivets с насечкой-4"
//                            }) {
//                    DocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
//                    SolidWorksDocument.EditDelete();
//                }
//                DocumentExtension.SelectByID2("Кронштейн", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditDelete();
//                DocumentExtension.SelectByID2("U10@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (типКрепежнойЧастиУсиливающейПанели != "Z") {
//                DocumentExtension.SelectByID2("U20@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();


//            }

//            if (типКрепежнойЧастиУсиливающейПанели == "Z" || типКрепежнойЧастиУсиливающейПанели == "D" || типКрепежнойЧастиУсиливающейПанели == "E") {


//                DocumentExtension.SelectByID2("Эскиз41@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();

//                DocumentExtension.SelectByID2("Эскиз56@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }


//            #endregion

//            #region Вставки внутренние

//            if (ValProfils.Tp1 == "01" || ValProfils.Tp1 == "00") {
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз80@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз81@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp2 == "01" || ValProfils.Tp2 == "00") {
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз61@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз62@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp3 == "01" || ValProfils.Tp3 == "00") {
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз63" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз64@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp4 == "01" || ValProfils.Tp4 == "00") {
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз82" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                    DocumentExtension.SelectByID2("Эскиз83" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Вырез-ВытянутьTp4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp1 == "02") {
//                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "1R" : "1L")}@{NameDownPanel}-1", supress);

//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Тип-02-1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Тип-02-1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-02-1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Тип-02-1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp2 == "02") {
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Тип-02-2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Тип-02-2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }

//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-02-2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Тип-02-2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp3 == "02") {
//                // VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "3R" : "3L")}@{NameDownPanel}-1", supress);
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Тип-02-3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Тип-02-3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-02-3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Тип-02-3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }


//            if (ValProfils.Tp4 == "02") {
//                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "4R" : "4L")}@{NameDownPanel}-1", supress);
//                if (configuration.Contains("01")) {
//                    DocumentExtension.SelectByID2("Тип-02-4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//                if (configuration.Contains("02")) {
//                    DocumentExtension.SelectByID2("Тип-02-4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    SolidWorksDocument.EditSuppress();
//                }
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-02-4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Тип-02-4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }
//            #endregion

//            #region To Delete

//            if (ValProfils.Tp1 == "05") {
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-05-1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Эскиз88@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp2 == "05") {
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-05-2@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Эскиз66@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp3 == "05") {
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-05-3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Эскиз67@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            if (ValProfils.Tp4 == "05") {
//            }
//            else {
//                DocumentExtension.SelectByID2("Тип-05-4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//                DocumentExtension.SelectByID2("Эскиз89@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
//                SolidWorksDocument.EditSuppress();
//            }

//            #endregion

//            #region Отверстия под усиливающие панели

//            if (framelessPanel.PanelType == PanelType_e.безКрыши || framelessPanel.PanelType == PanelType_e.односкат || framelessPanel.PanelType == PanelType_e.Двухскат) {

//                DocumentExtension.SelectByID2("Эскиз59@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditUnsuppress2();
//                DocumentExtension.SelectByID2("Эскиз73@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditUnsuppress2();

//                if (ValProfils.Tp1 != "-") {
//                    if (configuration.Contains("02")) {
//                        foreach (var name in new[] { "U32", "U31" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U33", "U34" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                    if (configuration.Contains("01")) {
//                        foreach (var name in new[] { "U52", "U51" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U53", "U54" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                }

//                if (ValProfils.Tp4 != "-") {
//                    if (configuration.Contains("02")) {
//                        foreach (var name in new[] { "U42", "U41" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U43", "U44" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                    if (configuration.Contains("01")) {
//                        foreach (var name in new[] { "U62", "U61" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U63", "U64" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                }
//            }

//            if (framelessPanel.PanelType == PanelType_e.безОпор || framelessPanel.PanelType == PanelType_e.РамаМонтажная) {

//                DocumentExtension.SelectByID2("Эскиз59@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditUnsuppress2();
//                DocumentExtension.SelectByID2("Эскиз73@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
//                SolidWorksDocument.EditUnsuppress2();

//                if (ValProfils.Tp1 != "-") {
//                    if (configuration.Contains("02")) {
//                        foreach (var name in new[] { "U32", "U31" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U33", "U34" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                    if (configuration.Contains("01")) {
//                        foreach (var name in new[] { "U52", "U51" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U53", "U54" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                }
//                if (ValProfils.Tp4 != "-") {
//                    if (configuration.Contains("02")) {
//                        foreach (var name in new[] { "U42", "U41" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U43", "U44" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                    if (configuration.Contains("01")) {
//                        foreach (var name in new[] { "U62", "U61" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                        foreach (var name in new[] { "U63", "U64" }) {
//                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                            SolidWorksDocument.EditUnsuppress2();
//                        }
//                    }
//                }
//            }

//            #endregion

//            #region Разрезные части

//            if (!string.IsNullOrEmpty(типДвойнойРазрез)) {
//                #region to delete

//                //var имяДвойнойВерхней1 = панельВнешняя.NewName + "-" + типДвойнойВерхней + "1";
//                //var имяДвойнойВерхней2 = панельВнешняя.NewName + "-" + типДвойнойВерхней + "2";
//                // var имяДвойнойНижней1 = панельВнутренняя.NewName + "-" + типДвойнойНижней + "1";
//                // var имяДвойнойНижней2 = панельВнутренняя.NewName + "-" + типДвойнойНижней + "2";

//                #endregion


//                switch (типДвойнойВерхней) {
//                    case "1":
//                        DocumentExtension.SelectByID2("Cut-ExtrudeW1@" + имяДвойнойВерхней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        DocumentExtension.SelectByID2("Cut-ExtrudeW2@" + имяДвойнойВерхней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();

//                        break;

//                    case "2":
//                        DocumentExtension.SelectByID2("Cut-ExtrudeH1@" + имяДвойнойВерхней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        DocumentExtension.SelectByID2("Cut-ExtrudeH2@" + имяДвойнойВерхней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();

//                        break;

//                    case "0":

//                        break;
//                }

//                switch (типДвойнойНижней) {
//                    case "1":
//                        DocumentExtension.SelectByID2("Cut-ExtrudeW1@" + имяДвойнойНижней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        DocumentExtension.SelectByID2("Cut-ExtrudeW2@" + имяДвойнойНижней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        break;

//                    case "2":
//                        DocumentExtension.SelectByID2("Cut-ExtrudeH1@" + имяДвойнойНижней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        DocumentExtension.SelectByID2("Cut-ExtrudeH2@" + имяДвойнойНижней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                        SolidWorksDocument.EditUnsuppress2();
//                        break;

//                    case "0":

//                        break;
//                }

//                foreach (var component in new[] { "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" }) {
//                    DocumentExtension.SelectByID2("DerivedCrvPattern" + component + "@" + AssemblyName, "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
//                    AssemblyDocument.DissolveComponentPattern();
//                }
//            }

//            #endregion

//        }
//    }
//}
 
                 

 