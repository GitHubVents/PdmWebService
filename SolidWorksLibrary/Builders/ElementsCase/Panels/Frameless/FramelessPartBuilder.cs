using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless {

    public partial class FramelessPanelBuilder : ProductBuilderBehavior {
        protected FramelessPanel framelessPanel;
        private List<FramelessPanel> framelessPanelList;
        public override event SetBendsHandler SetBends;
        protected double колСаморезВинтШирина { get; set; }
        protected double screwsByWidthInner { get; set; }
        protected string NameUpPanel { get; set; } = "02-11-01-40-";
        protected string NameDownPanel { get; set; } = "02-11-02-40-";
        protected ModelDocExtension DocumentExtension { get {return this.SolidWorksDocument.Extension } }
        protected string configuration { get; set; }
        protected double колЗаклепокВысота;
        protected double screwsByHeightInner;
        public FramelessPanelBuilder(FramelessPanel framelessPanel, List<FramelessPanel> framelessPanelList, string config) : base() {

            this.framelessPanel = framelessPanel;
            this.framelessPanelList = framelessPanelList;
            SetProperties("panel", "01 - Frameless Design 40mm");
            this.configuration = config;
        }


        public void Build() {
            Patterns.Observer.MessageObserver.Instance.SetMessage("Opened solid works");
            AssemblyName = "02-11-40-1";
            NewPartPath = System.IO.Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            Patterns.Observer.MessageObserver.Instance.SetMessage(NewPartPath);
            SolidWorksDocument = SolidWorksAdapter.OpenDocument(NewPartPath, swDocumentTypes_e.swDocASSEMBLY);
            //  SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);

            Patterns.Observer.MessageObserver.Instance.SetMessage("calc data. press any key");
            CalculateHandle();

            Patterns.Observer.MessageObserver.Instance.SetMessage("delete data.");

            //   DeleteComponents();
            //Patterns.Observer.MessageObserver.Instance.SetMessage("set data. ");// press any key");

            //   //Console.ReadKey();
            //    SetSize();
        }

        void CalculateHandle() {
            framelessPanel.widthHandle = framelessPanel.SizePanel.X / 4;

            if (framelessPanel.SizePanel.X < 1000) {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.5 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1000) {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.45 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1300) {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.4 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1700) {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.35 * 0.5;
            }
        }

        double количествоВинтов = 2000;

        void CalculateRemovablePanel(Vector2 panelSize,
                                   out Vector2 removablePanelSize,
                                    out double расстояниеL, out int количествоВинтов) {
            removablePanelSize = Vector2.Zero;

            removablePanelSize.X = panelSize.X - 2;
            removablePanelSize.Y = panelSize.Y - 2;
            расстояниеL = panelSize.X - 132;

            количествоВинтов = 5000;

            if (panelSize.X < 1100) {
                количествоВинтов = 4000;
            }
            if (panelSize.X < 700) {
                количествоВинтов = 3000;
            }
            if (panelSize.X < 365) {
                количествоВинтов = 2000;
            }
        }
        private void CalculateRivetCount() {
       


            if (framelessPanel.PanelType == PanelType_e.RemovablePanel) {
                Vector2 removebalePanelSize;
                double distanceL;
                int countScrews;
                CalculateRemovablePanel(framelessPanel.SizePanel, out removebalePanelSize, out distanceL, out countScrews);
                const double шагЗаклепокВысота = 125;
                колЗаклепокВысота = (Math.Truncate(removebalePanelSize.Y / шагЗаклепокВысота) + 1) * 1000;

            }
            else {
                // TO DO

                //    колЗаклепокВысота = колСаморезВинтВысота + 1000;
                if (Convert.ToInt32(framelessPanel.SizePanel.Y) > 1000) {
                    //  колЗаклепокВысота = колСаморезВинтВысота + 3000;
                }
            }
        }

        private void CalculateDoublePanel() {
            //double колЗаклепокКронштейнаДвойнойПанели = 2000;

            //string типДвойнойВерхней = "0";
            //string типДвойнойНижней = "0";
            //string типДвойнойРазрез = null;



            //if (типДвойной != "00")
            //{
            //    //AssemblyName = "02-11-40-2";

            //    NameUpPanel = "02-11-01-40-2-";
            //    NameDownPanel = "02-11-02-40-2-";

            //    типДвойнойВерхней = типДвойной.Remove(1, 1);
            //    типДвойнойНижней = типДвойной.Remove(0, 1);

            //    if (типДвойной.Contains("1"))
            //    {
            //        типДвойнойРазрез = "W";
            //    }
            //    if (типДвойной.Contains("2"))
            //    {
            //        типДвойнойРазрез = "H";
            //    }
            //}

            //// Кронштейны двойной панели            


            //if (!string.IsNullOrEmpty(типДвойнойРазрез))
            //{
            //    var idToDelete = "-2";
            //    var idToChange = "-1";

            //    var lenghtOfProfil = framelessPanel.SizePanel.Y;

            //    var nameOfProfil = усиление ? "02-11-13-40-" : "02-11-14-40-";
            //    var nameOfProfilToDelete = !усиление ? "02-11-13-40-" : "02-11-14-40-";

            //    // todo учет толщины
            //    var deltaForLenght = усиление ? 48.0 : 3.5;
            //    var newNameP = nameOfProfil + framelessPanel.SizePanel.Y;

            //    var cut = типДвойнойРазрез == "H" ? " по высоте H " : " по ширине W ";

            //    if (типДвойнойРазрез == "H")
            //    {
            //        idToDelete = "-1";
            //        idToChange = "-2";
            //        newNameP = nameOfProfil + framelessPanel.SizePanel.X;
            //        lenghtOfProfil = framelessPanel.SizePanel.X;
            //    }
            //  }


            // TO DO 
            //DocumentExtension.SelectByID2(/*nameOfProfil + idToDelete + "@" + AssemblyName */ "", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
            //DocumentExtension.SelectByID2(/*nameOfProfilToDelete + "-1@" + AssemblyName*/"", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
            //DocumentExtension.SelectByID2(/* nameOfProfilToDelete + "-2@" + AssemblyName*/"", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
        }

        /// <summary>
        ///  Расчет шага для саморезов и заклепок
        /// </summary>
        /// <param name="screwsCountByWidht"></param>
        /// <param name="screwscountByHeight"></param>
        private void GetCountScrews(ref double screwsCountByWidht, ref double screwscountByHeight) {
            double stepScrewsByWeidht = 200;
            double stepScrewsByHeight = 200;

            if (framelessPanel.SizePanel.X < 600) {
                stepScrewsByWeidht = 150;
            }
            if (framelessPanel.SizePanel.Y < 600) {
                stepScrewsByHeight = 150;
            }
            screwsCountByWidht = (Math.Truncate(framelessPanel.SizePanel.X / stepScrewsByWeidht) + 1) * 1000;
            screwscountByHeight = (Math.Truncate(framelessPanel.SizePanel.Y / stepScrewsByHeight) + 1) * 1000;

            if (framelessPanel.Screws?.ByHeightInnerUp > 1000) {
                screwscountByHeight = framelessPanel.Screws.ByHeightInnerUp;
            }
        }





        private void SetSize() {

            // Расчет шага для саморезов и заклепок
            double rivetCountByHeight;


            //switch (framelessPanel.PanelType)
            //{
            //    case PanelType.RemovablePanel:
            //    case "05":
            //        колСаморезВинтШирина = количествоВинтов;
            //        break;
            //}


            #region Оступы для отверстий заклепок, саморезов и винтов
            var отступОтветныхОтверстийШирина = 8;
            var осьСаморезВинт = 9.0;
            var осьОтверстийСаморезВинт = framelessPanel.PanelType == PanelType_e.RemovablePanel ? 12.0 : 11.0;
            var осьПоперечныеОтверстия = 10.1;

            if (framelessPanel.ThermoStrip == ThermoStrip.ThermoScotch) {
                осьПоперечныеОтверстия = 10.1;
            }

            if (framelessPanel.PanelType != PanelType_e.RemovablePanel) {
                отступОтветныхОтверстийШирина = 47;
                осьСаморезВинт = 9.70;
                осьОтверстийСаморезВинт = 10.3;
            }
            #endregion

            #region Диаметры отверстий
            var диамЗаглушкаВинт = 13.1;
            var диамСаморезВинт = 3.3;

            if (framelessPanel.PanelType == PanelType_e.RemovablePanel) {
                диамЗаглушкаВинт = 11;
                диамСаморезВинт = 7;
            }
            #endregion

            if (CheckExistPart != null)
                CheckExistPart(base.PartName, out IsPartExist, out NewPartPath);

            PartName = "02-11-02-40-";
            if (IsPartExist) {
                DocumentExtension.SelectByID2(PartName + "@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
            }
            else {

                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                if (SetBends != null)
                    SetBends((decimal)framelessPanel.outThickness, out KFactor, out BendRadius);

                Vector2 dimensions; // габариты
                if (framelessPanel.PanelType.Equals(PanelType_e.RemovablePanel)) {

                    const double шагЗаклепокВысота = 125;
                    rivetCountByHeight = (Math.Truncate(framelessPanel.SizePanel.Y / шагЗаклепокВысота) + 1) * 1000;

                    dimensions = new Vector2(framelessPanel.SizePanel.X - 42, framelessPanel.SizePanel.Y - 42);

                    base.parameters.Add("D3@2-1-1", 54.0);
                    base.parameters.Add("D2@Эскиз29", 84.0);
                    //base.parameters.Add("D2@Эскиз43", 12.0);
                    base.parameters.Add("D1@Эскиз29", 11.3);
                    base.parameters.Add("D1@2-1-1", 11.3);
                    base.parameters.Add("D2@Эскиз39", 11.3);
                    base.parameters.Add("D1@Эскиз39", 5.0);


                }
                else // others
                {
                    //     rivetCountByHeight = screwscountByHeight + 1000;
                    if (Convert.ToInt32(framelessPanel.SizePanel.Y) > 1000) {
                        //      rivetCountByHeight = screwscountByHeight + 3000;
                    }

                    dimensions = new Vector2(framelessPanel.SizePanel.X - 40, framelessPanel.SizePanel.Y - 40);
                    base.parameters.Add("D3@2-1-1", 55.0);
                    //Console.WriteLine("1");
                    base.parameters.Add("D2@Эскиз29", 85.0);
                    //Console.WriteLine("2");
                    //  base.parameters.Add("D2@Эскиз43", 11.0);
                    //Console.WriteLine("3");
                    base.parameters.Add("D1@Эскиз29", 10.3);
                    //Console.WriteLine("4");
                    base.parameters.Add("D1@2-1-1", 10.3);
                    //Console.WriteLine("5");
                    base.parameters.Add("D2@Эскиз39", 10.3);
                    //Console.WriteLine("6");
                    base.parameters.Add("D1@Эскиз39", 4.0);
                    //Console.WriteLine("7");
                }

                base.parameters.Add("D1@Эскиз1", dimensions.X);
                //Console.WriteLine("8");
                base.parameters.Add("D2@Эскиз1", dimensions.Y);
                //Console.WriteLine("9");
                if (framelessPanel.PanelType == PanelType_e.RemovablePanel && !framelessPanel.isOneHandle) {
                    base.parameters.Add("D4@Эскиз47", framelessPanel.widthHandle);
                    //Console.WriteLine("10");

                }

                //Размеры для отверсти под клепальные гайки под съемные панели

                //   base.parameters.Add("D3@2-1-1", 55.0);
                //Console.WriteLine("11");
                base.parameters.Add("G0@Эскиз49", OutputHolesWrapper.G0);
                //Console.WriteLine("12");
                base.parameters.Add("G1@Эскиз49", OutputHolesWrapper.G1);
                //Console.WriteLine("13");
                base.parameters.Add("G2@Эскиз49", OutputHolesWrapper.G2);
                //Console.WriteLine("14");
                // base.parameters.Add("G3@Эскиз49", OutputHolesWrapper.G0);
                //Console.WriteLine("15");
                base.parameters.Add("L1@Эскиз49", OutputHolesWrapper.L1);
                //Console.WriteLine("16");
                base.parameters.Add("L2@Эскиз49", OutputHolesWrapper.L2);
                //Console.WriteLine("17");
                base.parameters.Add("L3@Эскиз49", OutputHolesWrapper.L3);
                //Console.WriteLine("18");

                base.parameters.Add("D1@Кривая10", OutputHolesWrapper.D1);
                //Console.WriteLine("19");
                base.parameters.Add("D1@Кривая11", OutputHolesWrapper.D2);
                //Console.WriteLine("20");
                base.parameters.Add("D1@Кривая12", OutputHolesWrapper.D3);
                //Console.WriteLine("21");

                //Размеры промежуточных профилей
                base.parameters.Add("Wp1@Эскиз59", Math.Abs(ValProfils.Wp1) < 1 ? 10 : ValProfils.Wp1);
                //Console.WriteLine("22");
                base.parameters.Add("Wp2@Эскиз59", Math.Abs(ValProfils.Wp2) < 1 ? 10 : ValProfils.Wp2);
                //Console.WriteLine("23");
                base.parameters.Add("Wp3@Эскиз59", Math.Abs(ValProfils.Wp3) < 1 ? 10 : ValProfils.Wp3);
                //Console.WriteLine("24");
                base.parameters.Add("Wp4@Эскиз59", Math.Abs(ValProfils.Wp4) < 1 ? 10 : ValProfils.Wp4);
                //Console.WriteLine("25");

                // Для промежуточной панели отверстия
                //   base.parameters.Add("D1@Кривая14", rivetCountByHeight * 2),



                // Кол-во отверстий под заклепки сшивочных кронштейнов
                // base.parameters.Add("D1@CrvPatternW", колЗаклепокКронштейнаДвойнойПанели);
                // { "D1@CrvPatternH",  колЗаклепокКронштейнаДвойнойПанели}


                base.parameters.Add("D7@Ребро-кромка1", framelessPanel.lenght);
                //Console.WriteLine("26");
                base.parameters.Add("Толщина@Листовой металл", framelessPanel.outThickness);
                //Console.WriteLine("27");
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                //Console.WriteLine("28");
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                //Console.WriteLine("29");

                //Console.WriteLine("Количество параметров " + parameters.Count);
                EditPartParameters(PartName, base.NewPartPath);

            }
        }


        //private void DeleteFeatures(IEnumerable<FeatureBox> featureBoxList) {
        //    FeatureManager featureManager = SolidWorksDocument.FeatureManager;
        //    var selMgr = (SelectionMgr)SolidWorksDocument.SelectionManager;

        //    var selData = (SelectData)selMgr.CreateSelectData();


        //    var features = featureManager.GetFeatures(true);
        //    int count = featureManager.GetFeatureCount(false);


        //    foreach (var eachFeatureBox in featureBoxList.Distinct()) {
        //        SolidWorksAdapter.AcativeteDoc(eachFeatureBox.FileName);

        //        foreach (var item in featureBoxList.Where(each => each.FileName == eachFeatureBox.FileName)) {
        //            foreach (var feature in features) {

        //                if (featureBoxList.Contains(item.feature)) {

        //                    selMgr.AddSelectionListObject(item, selData);
        //                }
        //            }
        //        }
        //    }




        //    model.EditDelete();
        //}

        protected override void DeleteComponents(int type = 0) {
            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;
            DocumentExtension = SolidWorksDocument.Extension;
            double HeightOfWindow = WindowProfils.Flange30 ? WindowProfils.Width : (WindowProfils.Width + 2);
            double WidthOfWindow = WindowProfils.Height;
            CompType_e bodyfeat = CompType_e.BODYFEATURE;
            CompType_e sketch = CompType_e.SKETCH;


            //   List<FeatureBox> FeatureBoxList = new List<FeatureBox>();


            //var ftrfolder = VentsCad.CompType.FTRFOLDER;
            //var dimension = VentsCad.CompType.DIMENSION;           

            //var HeightOfWindow = BackProfils.Flange30 ?
            //        BackProfils.ByHeight / 1000 : (BackProfils.ByHeight + 2) / 1000;

            //MessageBox.Show(HeightOfWindow.ToString());



            //var supress = VentsCad.Act.Suppress; 
            //   var unSupress = VentsCad.Act.Unsuppress;
            // var doNothing = VentsCad.Act.DoNothing;


            #region FrontPanel
            if (framelessPanel.PanelType != PanelType_e.FrontPanel) {

                // FeatureBoxList.Add(new FeatureBox {ComponentName = "Рамка", FileName = AssemblyName, IsOptions = false });

                //  Удаление компонентов из сборки
                DocumentExtension.SelectByID2("Рамка", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                foreach (var component in new List<string>
                {
                                        "02-11-11-40--1", "02-11-11-40--2", "02-11-11-40--3","02-11-11-40--4",
                                        "Threaded Rivets Increased-1", "Threaded Rivets Increased-2", "Threaded Rivets Increased-3", "Threaded Rivets Increased-4",
                                        "Rivet Bralo-71", "Rivet Bralo-72", "Rivet Bralo-73", "Rivet Bralo-74", "Rivet Bralo-75", "Rivet Bralo-76",
                                        "Rivet Bralo-83", "Rivet Bralo-84", "Rivet Bralo-91", "Rivet Bralo-92", "Rivet Bralo-93", "Rivet Bralo-94"
                                    }) {
                    DocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    // FeatureBoxList.Add(new FeatureBox { ComponentName = component, FileName = AssemblyName, IsOptions = false });
                }



                DocumentExtension.SelectByID2("Вырез-Вытянуть21@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Рамка@" + NameUpPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Вырез-Вытянуть22@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Рамка@" + NameDownPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Вырез-Вытянуть1@02-11-03-40-@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Рамка@02-11-03-40-@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.EditRebuild3();
            }
            #endregion
            #region Fro others panels
            else {
                DocumentExtension.SelectByID2("Threaded Rivets-38@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.EditRebuild3();
                }
                else if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.SelectByID2("Кривая3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                }

                DocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                DocumentExtension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                DocumentExtension.SelectByID2("Кривая6@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();


                // DocumentExtension.SelectByID2("D1@Кривая6@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                // ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая6@" + NameUpPanel + ".Part"))).SystemValue = колСаморезВинтВысота / 1000;

                DocumentExtension.SelectByID2("D2@Эскиз74@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз74@" + NameUpPanel + ".Part"))).SystemValue = 0.015;

                DocumentExtension.SelectByID2("Threaded Rivets-37@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("D2@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

                DocumentExtension.SelectByID2("D1@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;


                DocumentExtension.SelectByID2("D3@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WindowProfils.ByWidth / 1000;

                DocumentExtension.SelectByID2("D4@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                DocumentExtension.SelectByID2("D1@Эскиз72@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз72@" + NameUpPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.02;

                DocumentExtension.SelectByID2("D3@Эскиз72@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз72@" + NameUpPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.015;

                var zaklWidth = Convert.ToInt32(Math.Truncate(HeightOfWindow / 100));
                DocumentExtension.SelectByID2("D1@Кривая4@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая4@" + NameUpPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;

                var zaklHeight = Convert.ToInt32(Math.Truncate(WindowProfils.Height / 100));
                DocumentExtension.SelectByID2("D1@Кривая5@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая5@" + NameUpPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;

                DocumentExtension.SelectByID2("D3@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

                DocumentExtension.SelectByID2("D4@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;

                DocumentExtension.SelectByID2("D2@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                DocumentExtension.SelectByID2("D1@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WindowProfils.ByWidth / 1000;

                DocumentExtension.SelectByID2("D1@Эскиз99@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз99@" + NameDownPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.02;

                DocumentExtension.SelectByID2("D3@Эскиз99@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз99@" + NameDownPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.015;

                DocumentExtension.SelectByID2("D1@Кривая15@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая15@" + NameDownPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;

                DocumentExtension.SelectByID2("D1@Кривая16@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая16@" + NameDownPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;

                DocumentExtension.SelectByID2("D5@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D5@Эскиз3@02-11-03-40-.Part"))).SystemValue = HeightOfWindow / 1000;

                DocumentExtension.SelectByID2("D4@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз3@02-11-03-40-.Part"))).SystemValue = WidthOfWindow / 1000;

                DocumentExtension.SelectByID2("D2@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз3@02-11-03-40-.Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                DocumentExtension.SelectByID2("D3@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз3@02-11-03-40-.Part"))).SystemValue = WindowProfils.ByWidth / 1000;
                SolidWorksDocument.EditRebuild3();

                if (framelessPanel.PanelType == PanelType_e.BlankPanel) {
                    DocumentExtension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);
                    DocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }

            }
            #endregion

            #region 04 05 - Съемные панели
            switch (framelessPanel.PanelType) {
                case PanelType_e.RemovablePanel:

                    if (framelessPanel.SizePanel.X > 750) {
                        #region when widht panel larger 750
                        DocumentExtension.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Threaded Rivets-25@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Ручка MLA 120-3@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("SC GOST 17475_gost-5@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("SC GOST 17475_gost-6@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Вырез-Вытянуть14@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        #endregion
                    }


                    else {
                        #region #region when widht panel less 750 
                        DocumentExtension.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Threaded Rivets-21@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Threaded Rivets-22@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Ручка MLA 120-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("SC GOST 17475_gost-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("SC GOST 17475_gost-2@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        #endregion
                    }
                    #region
                    DocumentExtension.SelectByID2("Threaded Rivets-60@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Threaded Rivets-61@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-2@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-2@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("1-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("3-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Hole1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.065;
                    DocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                    ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = framelessPanel.PanelType == PanelType_e.RemovablePanel ? 0.044 : 0.045;

                    DocumentExtension.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Вырез-Вытянуть1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Эскиз27@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Кривая1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Кривая1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Эскиз26@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Hole3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Вырез-Вытянуть3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Эскиз31@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Кривая3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Кривая3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Эскиз30@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    DocumentExtension.SelectByID2("Эскиз30@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    #endregion

                    #region Удаление торцевых отверстий под саморезы
                    DocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);
                    #endregion

                    #region Удаление торцевых отверстий под клепальные гайки
                    DocumentExtension.SelectByID2("Под клепальные гайки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.SelectByID2("Эскиз49@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Панель1", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Панель2", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.SelectByID2("Эскиз32@" + "02-11-06-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Эскиз32@" + "02-11-06_2-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    #endregion

                    #region Удаление отверстий под монтажную рамку
                    DocumentExtension.SelectByID2("Вырез-Вытянуть9@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть10@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть5@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);
                    #endregion

                    #region Одна ручка
                    if (framelessPanel.SizePanel.Y < 825) {
                        DocumentExtension.SelectByID2("SC GOST 17473_gost-12@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        DocumentExtension.SelectByID2("SC GOST 17473_gost-13@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.SelectByID2("Вырез-Вытянуть11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                    }
                    #endregion
                    SolidWorksDocument.EditRebuild3();
                    break;

                #endregion

                #region Удаление элементов съемной панели

                case PanelType_e.BlankPanel:
                case PanelType_e.безКрыши:
                case PanelType_e.односкат:

                case PanelType_e.безОпор:
                case PanelType_e.РамаМонтажная:
                case PanelType_e.НожкиОпорные:
                case PanelType_e.FrontPanel:

                    DocumentExtension.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Threaded Rivets-25@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Ручка MLA 120-3@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17475_gost-5@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17475_gost-6@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Threaded Rivets-21@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Threaded Rivets-22@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Ручка MLA 120-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17475_gost-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17475_gost-2@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17473_gost-12@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("SC GOST 17473_gost-13@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();

                    DocumentExtension.SelectByID2("Вырез-Вытянуть14@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);

                    DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);

                    DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);

                    DocumentExtension.SelectByID2("Вырез-Вытянуть11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    DocumentExtension.DeleteSelection2(deleteOption);

                    #endregion

                    #region Удаление элементов глухой панели

                    if (framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel) {
                        DocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = 0.047;
                        SolidWorksDocument.EditRebuild3();

                        DocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.067;
                        SolidWorksDocument.EditRebuild3();

                        if (framelessPanel.PanelType == PanelType_e.FrontPanel) {
                            DocumentExtension.SelectByID2("Вырез-Вытянуть12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Вырез-Вытянуть4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Вырез-Вытянуть5@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                        }

                        if (!string.IsNullOrEmpty(ТипУсиливающей())) // TO DO
                        {
                            DocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                            ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.03;
                            SolidWorksDocument.EditRebuild3();

                            DocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                            ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = 0.01;
                            SolidWorksDocument.EditRebuild3();
                        }
                    }

                    break;
            }


            if (/*Тип торцевой*/ framelessPanel.PanelType != PanelType_e.BlankPanel) {
                DocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.0975;
                SolidWorksDocument.EditRebuild3();

                DocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = 0.0775;
                SolidWorksDocument.EditRebuild3();
            }

            if (framelessPanel.PanelType != PanelType_e.BlankPanel && framelessPanel.PanelType != PanelType_e.FrontPanel) {
                DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                DocumentExtension.DeleteSelection2(deleteOption);
                DocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                DocumentExtension.DeleteSelection2(deleteOption);
            }

            if (framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel) {
                //Удаление торцевых отверстий под клепальные гайки
                DocumentExtension.SelectByID2("Под клепальные гайки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Эскиз49@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Панель1", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Панель2", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Эскиз32@" + "02-11-06-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Эскиз32@" + "02-11-06_2-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }

            if (framelessPanel.PanelType != PanelType_e.НожкиОпорные) {

                for (byte i = 5; i <= 20; i++) // TO DO what is 20
                {
                    DocumentExtension.SelectByID2("Threaded Rivets-" + i + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }

                DocumentExtension.SelectByID2("Ножка", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Вырез-Вытянуть11@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);

                DocumentExtension.DeleteSelection2(deleteOption);

                DocumentExtension.SelectByID2("Зеркальное отражение2@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }


            if (framelessPanel.PanelType != PanelType_e.односкат && framelessPanel.PanelType != PanelType_e.РамаМонтажная) {
                DocumentExtension.SelectByID2("Threaded Rivets-33@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-34@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-35@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-36@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Up", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                DocumentExtension.DeleteSelection2(deleteOption);
                SolidWorksDocument.ClearSelection2(true);
            }

            if (framelessPanel.PanelType != PanelType_e.RemovablePanel) {
                DocumentExtension.SelectByID2("SC GOST 17473_gost-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("SC GOST 17473_gost-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Threaded Rivets-59@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Threaded Rivets-60@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Threaded Rivets-61@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                ;
                DocumentExtension.SelectByID2("Threaded Rivets-62@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.ClearSelection2(true);
            }
            if (configuration.Contains("00")) {
                foreach (var i in new[] { 37, 38, 47, 48, 51, 52 })  // delete Threaded Rivet by name: Threaded Rivets- + number
                {
                    DocumentExtension.SelectByID2("Threaded Rivets-" + i + "@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
            }

            if (configuration.Contains("01")) {
                DocumentExtension.SelectByID2("Threaded Rivets-38@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-48@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-51@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("4-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                SolidWorksDocument.EditRebuild3();
                DocumentExtension.SelectByID2("1-0@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                SolidWorksDocument.EditRebuild3();

                // Погашение отверстий под клепальные гайки
                DocumentExtension.SelectByID2("Вырез-Вытянуть20@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                DocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                // В усиливающих профилях
                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                SolidWorksDocument.ClearSelection2(true);
            }


            if (configuration.Contains("02")) {
                DocumentExtension.SelectByID2("Threaded Rivets-37@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-47@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-52@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("2-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                SolidWorksDocument.EditRebuild3();
                DocumentExtension.SelectByID2("1-1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                SolidWorksDocument.EditRebuild3();

                // Погашение отверстий под клепальные гайки
                DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                DocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                // В усиливающих профилях
                DocumentExtension.SelectByID2("Вырез-Вытянуть15@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress2();

                SolidWorksDocument.ClearSelection2(true);

            }

            #endregion

            #region Удаление усиления для типоразмера меньше AV09

            if (!framelessPanel.усиление) {
                foreach (var component in new[] { "02-11-06-40--1", "02-11-06_2-40--4", "02-11-07-40--1", "02-11-07-40--2" }) {
                    DocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }

                foreach (var number in new[]
                {
                                        "37", "38", "39", "40",
                                        "41", "42", "43", "44",
                                        "45", "46", "47", "48",
                                        "49", "50", "51", "52",
                                        "53", "54", "55", "56"
                                    }) {
                    DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }

                SolidWorksDocument.ShowConfiguration2("Вытяжная заклепка 3,0х6 (ст ст. с пл. гол.)");
                DocumentExtension.SelectByID2("Усиление", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Hole7@" + NameDownPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Эскиз45@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Вырез-Вытянуть13@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Кривая7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Hole8@" + NameDownPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Эскиз47@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Вырез-Вытянуть14@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Кривая9@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                SolidWorksDocument.ClearSelection2(true);
            }

            #endregion

            #region Отверстия под панели L2 L3

            if (Convert.ToInt32(OutputHolesWrapper.L2) == 28) {

                DocumentExtension.SelectByID2("Threaded Rivets-47@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-48@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);

                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая5@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);

                DocumentExtension.SelectByID2("Вырез-Вытянуть16@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая5@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);
            }


            if (Convert.ToInt32(OutputHolesWrapper.L3) == 28) {
                DocumentExtension.SelectByID2("Threaded Rivets-51@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Threaded Rivets-52@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Вырез-Вытянуть19@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-Вытянуть20@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);

                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая6@" + "02-11-06-40-" + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);

                DocumentExtension.SelectByID2("Вырез-Вытянуть17@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Кривая6@" + "02-11-06_2-40-" + "-4@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                SolidWorksDocument.ClearSelection2(true);
            }
            #endregion

            #region Панели усиливающие

            string типКрепежнойЧастиУсиливающейПанели = null;
            var типТорцевойЧастиУсиливающейПанели = "T";

            if (!string.IsNullOrEmpty(ТипУсиливающей())) {

                типТорцевойЧастиУсиливающейПанели = ТипУсиливающей().Remove(1).Contains("T") ? "T" : "E";
                if (ТипУсиливающей().Remove(0, 1).Contains("E")) {
                    типКрепежнойЧастиУсиливающейПанели = "E";
                }
                if (ТипУсиливающей().Remove(0, 1).Contains("D")) {
                    типКрепежнойЧастиУсиливающейПанели = "D";
                }
                if (ТипУсиливающей().Remove(0, 1).Contains("E")) {
                    типКрепежнойЧастиУсиливающейПанели = "E";
                }
                if (ТипУсиливающей().Remove(0, 1).Contains("Z")) {
                    типКрепежнойЧастиУсиливающейПанели = "Z";
                }
            }

            if (framelessPanel.SizePanel.Y < 825) {
                DocumentExtension.SelectByID2("UpperAV09@02-11-09-40--1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
            }

            if (типТорцевойЧастиУсиливающейПанели == "E") {
                DocumentExtension.SelectByID2("1-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("1-3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("1-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Эскиз42@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Hole1@" + NameUpPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
            }

            if (типКрепежнойЧастиУсиливающейПанели != "D") {
                foreach (var component in new[]
                {
                                        "02-11-09-40--1",
                                        "Threaded Rivets с насечкой-1", "Threaded Rivets с насечкой-2",
                                        "Threaded Rivets с насечкой-3", "Threaded Rivets с насечкой-4"
                                    }) {
                    DocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                DocumentExtension.SelectByID2("Кронштейн", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("U10@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
            }

            if (типКрепежнойЧастиУсиливающейПанели != "Z") {
                DocumentExtension.SelectByID2("U20@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                foreach (var component in new[]
                {
                                        "Threaded Rivets с насечкой-5", "Threaded Rivets с насечкой-6"
                                    }) {
                    DocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
            }

            if (типКрепежнойЧастиУсиливающейПанели == "Z" || типКрепежнойЧастиУсиливающейПанели == "D" || типКрепежнойЧастиУсиливающейПанели == "E") {
                DocumentExtension.SelectByID2("3-2@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("3-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2("3-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Эскиз41@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Hole3@" + NameUpPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                DocumentExtension.SelectByID2("Эскиз56@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditSuppress();
            }


            #endregion

            #region Вставки внутренние

            if (ValProfils.Tp1 == "01" || ValProfils.Tp1 == "00") {
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз80@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз81@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
            }
            else {
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp1R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp1L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp2 == "01" || ValProfils.Tp2 == "00") {
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз61@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp2R@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз61@" + NameDownPanel + "-1", supress);
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз62@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp2L@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз62@" + NameDownPanel + "-1", supress);
                }
            }
            else {
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp2R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp2L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp3 == "01" || ValProfils.Tp3 == "00") {
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз63" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp3R@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз63@" + NameDownPanel + "-1", supress);
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз64@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp3L@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз64@" + NameDownPanel + "-1", supress);
                }
            }
            else {
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp3R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp3L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp4 == "01" || ValProfils.Tp4 == "00") {
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз82" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp4R@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз82@" + NameDownPanel + "-1", supress);
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Вырез-ВытянутьTp4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                    DocumentExtension.SelectByID2("Эскиз83" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp4L@" + NameDownPanel + "-1", supress);
                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, sketch, "Эскиз83@" + NameDownPanel + "-1", supress);
                }
            }
            else {
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Вырез-ВытянутьTp4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp4R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Вырез-ВытянутьTp4L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp1 == "02") {
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "1R" : "1L")}@{NameDownPanel}-1", supress);

                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Тип-02-1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-1R@" + NameDownPanel + "-1", supress);
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Тип-02-1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-1L@" + NameDownPanel + "-1", supress);
                }
            }
            else {
                DocumentExtension.SelectByID2("Тип-02-1R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Тип-02-1L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-1R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-1L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp2 == "02") {
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Тип-02-2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Тип-02-2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat,   $"Тип-02-{(isLeftSide ? "2R" : "2L")}@{NameDownPanel}-1", supress);
            }
            else {
                DocumentExtension.SelectByID2("Тип-02-2R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Тип-02-2L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-2R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-2L@" + NameDownPanel + "-1", supress);
            }

            if (ValProfils.Tp3 == "02") {
                // VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "3R" : "3L")}@{NameDownPanel}-1", supress);
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Тип-02-3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-3R@" + NameDownPanel + "-1", supress);
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Тип-02-3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();

                    //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-3L@" + NameDownPanel + "-1", supress);
                }
            }
            else {
                DocumentExtension.SelectByID2("Тип-02-3R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Тип-02-3L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-3R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-3L@" + NameDownPanel + "-1", supress);
            }


            if (ValProfils.Tp4 == "02") {
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, $"Тип-02-{(isLeftSide ? "4R" : "4L")}@{NameDownPanel}-1", supress);
                if (configuration.Contains("01")) {
                    DocumentExtension.SelectByID2("Тип-02-4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
                if (configuration.Contains("02")) {
                    DocumentExtension.SelectByID2("Тип-02-4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress();
                }
            }
            else {
                DocumentExtension.SelectByID2("Тип-02-4R@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Тип-02-4L@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-4R@" + NameDownPanel + "-1", supress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, bodyfeat, "Тип-02-4L@" + NameDownPanel + "-1", supress);
            }


            #region To Delete

            if (ValProfils.Tp1 == "05") {
            }
            else {
                DocumentExtension.SelectByID2("Тип-05-1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Эскиз88@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Тип-05-1@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Эскиз88@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
            }

            if (ValProfils.Tp2 == "05") {
            }
            else {
                DocumentExtension.SelectByID2("Тип-05-2@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Эскиз66@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Тип-05-2@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Эскиз66@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
            }

            if (ValProfils.Tp3 == "05") {
            }
            else {
                DocumentExtension.SelectByID2("Тип-05-3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Эскиз67@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Тип-05-3@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Эскиз67@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
            }

            if (ValProfils.Tp4 == "05") {
            }
            else {
                DocumentExtension.SelectByID2("Тип-05-4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();
                DocumentExtension.SelectByID2("Эскиз89@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress();

                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Тип-05-4@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
                //VentsCad.DoWithSwDoc(SolidWorksAdapter.SldWoksAppExemplare, VentsCad.CompType.BODYFEATURE, "Эскиз89@" + NameDownPanel + "-1", VentsCad.Act.Suppress);
            }

            #endregion

            #endregion

            #region Отверстия под усиливающие панели

            if (framelessPanel.PanelType == PanelType_e.безКрыши || framelessPanel.PanelType == PanelType_e.односкат || framelessPanel.PanelType == PanelType_e.Двухскат) {

                DocumentExtension.SelectByID2("Эскиз59@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                DocumentExtension.SelectByID2("Эскиз73@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();

                if (ValProfils.Tp1 != "-") {
                    if (configuration.Contains("02")) {
                        foreach (var name in new[] { "U32", "U31" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U33", "U34" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                    if (configuration.Contains("01")) {
                        foreach (var name in new[] { "U52", "U51" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U53", "U54" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                }

                if (ValProfils.Tp4 != "-") {
                    if (configuration.Contains("02")) {
                        foreach (var name in new[] { "U42", "U41" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U43", "U44" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                    if (configuration.Contains("01")) {
                        foreach (var name in new[] { "U62", "U61" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U63", "U64" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                }
            }

            if (framelessPanel.PanelType == PanelType_e.безОпор || framelessPanel.PanelType == PanelType_e.РамаМонтажная) {

                DocumentExtension.SelectByID2("Эскиз59@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                DocumentExtension.SelectByID2("Эскиз73@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();

                if (ValProfils.Tp1 != "-") {
                    if (configuration.Contains("02")) {
                        foreach (var name in new[] { "U32", "U31" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U33", "U34" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                    if (configuration.Contains("01")) {
                        foreach (var name in new[] { "U52", "U51" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U53", "U54" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                }
                if (ValProfils.Tp4 != "-") {
                    if (configuration.Contains("02")) {
                        foreach (var name in new[] { "U42", "U41" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U43", "U44" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                    if (configuration.Contains("01")) {
                        foreach (var name in new[] { "U62", "U61" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                        foreach (var name in new[] { "U63", "U64" }) {
                            DocumentExtension.SelectByID2(name + "@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                    }
                }
            }

            #endregion

            #region На заклепках (удаляем скотч)

            if (framelessPanel.ThermoStrip != ThermoStrip.ThermoScotch) // На заклепках
            {
                DocumentExtension.SelectByID2("02-11-04-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                //Задаем растояние между двумя деталями
                DocumentExtension.SelectByID2("D1@Расстояние1@" + AssemblyName + ".SLDASM", "DIMENSION", 0, 0, 0, true, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Расстояние1"))).SystemValue = 0;
            }

            #endregion

            #region Изменение деталей

            #region Кронштейны двойной панели            

            double колЗаклепокКронштейнаДвойнойПанели = 2000;

            CutType_e cutType = CutPanel.DeterminateCutPanel(framelessPanel.SizePanel);

            //  if (!string.IsNullOrEmpty(типДвойнойРазрез)) {
            if (cutType != CutType_e.Whole) {
                var idToDelete = "-2";
                var idToChange = "-1";

                var lenghtOfProfil = Convert.ToDouble(height);

                var nameOfProfil = усиление ? "02-11-13-40-" : "02-11-14-40-";
                var nameOfProfilToDelete = !усиление ? "02-11-13-40-" : "02-11-14-40-";

                // todo учет толщины
                var deltaForLenght = усиление ? 48.0 : 3.5;
                var newNameP = nameOfProfil + height;

                var cut = типДвойнойРазрез == "H" ? " по высоте H " : " по ширине W ";

                if (типДвойнойРазрез == "H") {
                    idToDelete = "-1";
                    idToChange = "-2";
                    newNameP = nameOfProfil + width;
                    lenghtOfProfil = width;
                }

                DocumentExtension.SelectByID2(nameOfProfil + idToDelete + "@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2(nameOfProfilToDelete + "-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                DocumentExtension.SelectByID2(nameOfProfilToDelete + "-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                #region

                switch (типДвойнойРазрез) {
                    case "H":
                        foreach (var number in new[]
                        { "105", "106", "113", "114", "137", "138", "139", "140", "141", "142", "143", "144", "145", "146"}) {
                            DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            SolidWorksDocument.EditDelete();
                        }

                        DocumentExtension.SelectByID2("Cut-ExtrudeWP1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeW@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeWP1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeW@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("WP1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();
                        DocumentExtension.SelectByID2("Cut-ExtrudeWC@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);

                        if (типДвойнойВерхней == "0") {
                            foreach (var number in new[] { "123", "124", "160", "161", "157", "158", "159" }) {
                                DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                                SolidWorksDocument.EditDelete();
                            }
                            DocumentExtension.SelectByID2("Cut-ExtrudeHP1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Cut-ExtrudeH@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                        }
                        if (типДвойнойНижней == "0") {
                            foreach (var number in new[] { "121", "122", "162", "163", "164", "165", "166" }) {
                                DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                                SolidWorksDocument.EditDelete();
                            }
                            DocumentExtension.SelectByID2("Cut-ExtrudeHP1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Cut-ExtrudeH@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                        }
                        break;

                    case "W":
                        foreach (var number in new[]
                        {
                                                "121", "122", "123", "124", "162", "163", "164", "165", "166",
                                                "157", "158", "159", "160", "161"
                                            }) {
                            DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            SolidWorksDocument.EditDelete();
                        }

                        DocumentExtension.SelectByID2("HP1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();
                        DocumentExtension.SelectByID2("Cut-ExtrudeHP1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeH@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeHP1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeH@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);
                        DocumentExtension.SelectByID2("Cut-ExtrudeHC@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        DocumentExtension.DeleteSelection2(deleteOption);

                        if (типДвойнойВерхней == "0") {
                            foreach (var number in new[] { "113", "114", "137", "138", "139", "140", "141" }) {
                                DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                                SolidWorksDocument.EditDelete();
                            }
                            DocumentExtension.SelectByID2("Cut-ExtrudeWP1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Cut-ExtrudeW@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Cut-ExtrudeWC@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                        }
                        if (типДвойнойНижней == "0") {
                            foreach (var number in new[] { "105", "106", "142", "143", "144", "145", "146" }) {
                                DocumentExtension.SelectByID2("Rivet Bralo-" + number + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                                SolidWorksDocument.EditDelete();
                            }
                            DocumentExtension.SelectByID2("Cut-ExtrudeWP1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                            DocumentExtension.SelectByID2("Cut-ExtrudeW@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            DocumentExtension.DeleteSelection2(deleteOption);
                        }
                        break;
                }

                #endregion

                //var newPartPathP = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newNameP}.SLDPRT";

                колЗаклепокКронштейнаДвойнойПанели = (Math.Truncate((lenghtOfProfil - 45.0) / 125) + 1) * 1000;

                if (false)
                    ;
                //GetExistingFile(Path.GetFileNameWithoutExtension(newPartPathP), 1)) {
                //    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
                //    DocumentExtension.SelectByID2(nameOfProfil + idToChange + "@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //    swAsm.ReplaceComponents(newPartPathP, "", false, true);
                //SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(nameOfProfil + ".SLDPRT");}
                else {
                    //EditPartParameters();
                    //SwPartParamsChangeWithNewName(nameOfProfil, $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newNameP}",
                    //    new[,]
                    //    {
                    //            {"D2@Эскиз1", Convert.ToString(lenghtOfProfil - deltaForLenght)},
                    //            {"D1@CrvPattern1", Convert.ToString(колЗаклепокКронштейнаДвойнойПанели)}
                    //    },
                    //    false, null);
                }


                #endregion

                #region  Панель внешняя

                var newName = панельВнешняя.NewName;
                var newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";

                var outerPanel = newPartPath;

                //todo вынести кол-во в метод модели

                var screwsByHeight =
                string.IsNullOrEmpty(типТорцевой)
                    ? колСаморезВинтВысота
                    : колСаморезВинтВысота - 1000;

                var zaklByHeight =
                     framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel || framelessPanel.PanelType == PanelType_e.RemovablePanel
                     ? колЗаклепокВысота
                     : колЗаклепокВысота + 1000;

                if (типДвойнойРазрез == "H") {
                    if ((screwsByHeight / 1000) % 2 != 0) {
                        screwsByHeight = screwsByHeight + 1000;
                    }

                    if ((zaklByHeight / 1000) % 2 != 0) {
                        zaklByHeight = zaklByHeight + 1000;
                    }
                }

                var screwsByWidth = framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel ? (колСаморезВинтШирина - 1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000)
                            : (колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина);

                var zaklByWidth = колЗаклепокШирина;

                if (типДвойнойРазрез == "W") {
                    if ((screwsByWidth / 1000) % 2 != 0) {
                        screwsByWidth = screwsByWidth + 1000;
                    }

                    if ((zaklByWidth / 1000) % 2 != 0) {
                        zaklByWidth = zaklByWidth + 1000;
                    }
                }

                if (screws.ByWidth > 0 & pType.Contains("3")) {
                    screwsByWidth = screws.ByWidth;
                }

                if (screws?.ByHeight > 0) {
                    screwsByHeight = screws.ByHeight;
                }
                if (screws?.ByWidth > 0) {
                    screwsByWidth = screws.ByWidth;
                }

                screwsByWidthInner =
               framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel
                  ? (колСаморезВинтШирина - 1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000)
                          : (колСаморезВинтШирина2 < 2000 ? 2000 : колСаморезВинтШирина);

                  screwsByHeightInner = pType == "04" || pType == "05"
                    ? (колСаморезВинтВысота)
                    : (колСаморезВинтВысота - 1000);


                if (screws?.ByHeightInner > 0) {
                    screwsByHeightInner = screws.ByHeightInner < 2000 ? 2000 : screws.ByHeightInner;
                }
                if (screws?.ByWidthInner > 0) {
                    screwsByWidthInner = screws.ByWidthInner < 2000 ? 2000 : screws.ByWidthInner;
                }

                //                if (GetExistingFile(newPartPath, 1))//   (Path.GetFileNameWithoutExtension(newPartPath), 1))
                //                {
                //                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
                //                    DocumentExtension.SelectByID2(NameUpPanel + "-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
                //                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NameUpPanel + ".SLDPRT");
                //                }
                //                else {
                //                    var d1Кривая3 = pType == "35"
                //                        ? (колСаморезВинтШирина - 1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000)
                //                        : (колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина);

                //                    var d1Эскиз52 = типКрепежнойЧастиУсиливающейПанели == null ? Convert.ToString(30) : Convert.ToString(20);

                //                    if (!string.IsNullOrEmpty(типТорцевой)) {
                //                        d1Кривая3 = колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина;
                //                        d1Эскиз52 = Convert.ToString(35);
                //                    }

                //                    if (screws?.ByWidthInnerUp > 0) {
                //                        d1Кривая3 = screws.ByWidthInnerUp;
                //                    }

                //типДвойнойРазрез

                SwPartParamsChangeWithNewName(NameUpPanel,
                    $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}",
                    new[,]
                    {
                                            // Габариты
                                            parameters.Add

                                            parameters.Add("D1@Эскиз1", Convert.ToString(ширинаПанели));
                parameters.Add("D2@Эскиз1", Convert.ToString(высотаПанели));
                parameters.Add("D1@3-4", Convert.ToString(screwsByHeight));
                parameters.Add("D1@1-4", Convert.ToString(screwsByHeight));
                parameters.Add("D1@2-4", Convert.ToString(screwsByWidth));
                parameters.Add("D2@2-2", Convert.ToString(осьСаморезВинт));
                parameters.Add("D4@Эскиз47", Convert.ToString(растояниеМеждуРучками));
                parameters.Add("D1@Эскиз50", Convert.ToString(диамСаморезВинт));
                parameters.Add("D1@2-3-1", Convert.ToString(диамСаморезВинт));
                parameters.Add("D1@Эскиз52", d1Эскиз52);
                parameters.Add("D2@Эскиз52", Convert.ToString(осьПоперечныеОтверстия));
                parameters.Add("D1@Кривая3", Convert.ToString(d1Кривая3));
                parameters.Add("D3@1-1-1", string.IsNullOrEmpty(типТорцевой) || pType == "01" ? Convert.ToString(35) : Convert.ToString(158.1));
                parameters.Add("D2@3-1-1", string.IsNullOrEmpty(типТорцевой) || pType == "01" ? Convert.ToString(35) : Convert.ToString(158.1));
                parameters.Add("D3@2-1-1", Convert.ToString(диамЗаглушкаВинт));
                parameters.Add("D1@Эскиз49", Convert.ToString(диамЗаглушкаВинт));
                parameters.Add("D1@Кривая1", Convert.ToString(zaklByWidth));
                parameters.Add("D1@Кривая2", Convert.ToString(zaklByHeight));
                parameters.Add("D7@Ребро-кромка1", framelessPanel.ThermoStrip == ThermoStrip.ThermoScotch ? Convert.ToString(17.7) : Convert.ToString(19.2));
                parameters.Add("Толщина@Листовой металл", materialP1[1].Replace('.', ','));
                parameters.Add("D1@CrvPatternW", Convert.ToString(колЗаклепокКронштейнаДвойнойПанели));
                //      parameters.Add("D1@CrvPatternH", Convert.ToString(колЗаклепокКронштейнаДвойнойПанели));

                типДвойнойВерхней != "0" ? new[]
                {
                                            $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойВерхней1}",
                                            $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойВерхней2}"

                        EditPartParameters(PartName,"");

            }

            //                #endregion

            //                #region  Панель внутреняя

            //                newName = панельВнутренняя.NewName;
            //                //newName = modelname2 + "-02-" + width + "-" + lenght + "-" + "40-" + materialP2[0] + strenghtP + panelsUpDownConfigString;
            //                newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";
            //                var innerPanel = newPartPath;

            //                if (false) ;
            //                //    (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1)) {
            //                //SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //                //DocumentExtension.SelectByID2(NameDownPanel + "-1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                //swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                //SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(NameDownPanel + ".SLDPRT");
            //            }
            //            else {
            //                //EditPartParameters();
            //                SwPartParamsChangeWithNewName(NameDownPanel,
            //                    $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}",
            //                    new[,]
            //                    {
            //                        {"D1@Эскиз1", pType == "04" || pType == "05"
            //                                ? Convert.ToString(ширинаПанели - 42)
            //                                : Convert.ToString(ширинаПанели - 40)},

            //                        {"D2@Эскиз1", pType == "04" || pType == "05"
            //                                ? Convert.ToString(высотаПанели - 42)
            //                                : Convert.ToString(высотаПанели - 40)},

            //                        {"D1@1-3", Convert.ToString(screwsByWidth)},
            //                        {"D1@Кривая6", Convert.ToString(screwsByHeight)},

            //                        {"D1@1-4", Convert.ToString(колСаморезВинтВысота)},

            //                        {"D1@Кривая5", Convert.ToString(screwsByWidthInner)},

            //                        {"D1@Кривая4", Convert.ToString(screwsByHeightInner)},

            //                        {"D2@Эскиз32", pType == "01" || pType == "35"
            //                                ? Convert.ToString(77.5)
            //                                : Convert.ToString(158.1)},

            //                        {"D4@Эскиз47", Convert.ToString(растояниеМеждуРучками)},

            //                        {"D1@Эскиз38", Convert.ToString(диамСаморезВинт)},
            //                        {"D3@1-1-1", Convert.ToString(диамСаморезВинт)},

            //                        {"D1@Эскиз40", string.IsNullOrEmpty(типТорцевой) || pType == "01"
            //                                ? Convert.ToString(15)
            //                                : Convert.ToString(138.1)},

            //                        {"D2@1-2", Convert.ToString(осьОтверстийСаморезВинт)},

            //                        {"D1@2-3", Convert.ToString(zaklByWidth)},
            //                        {"D1@Кривая1", Convert.ToString(zaklByWidth)},

            //                        {"D1@Кривая2", Convert.ToString(zaklByHeight)},

            //                        {"D3@2-1-1", pType == "04" || pType == "05"
            //                                ? Convert.ToString(54.0)
            //                                : Convert.ToString(55.0)},

            //                        {"D2@Эскиз29", pType == "04" || pType == "05"
            //                                ? Convert.ToString(84.0)
            //                                : Convert.ToString(85.0)},

            //                        {"D2@Эскиз43", pType == "04" || pType == "05"
            //                                ? Convert.ToString(12.0)
            //                                : Convert.ToString(11.0)},

            //                        {"D1@Эскиз29", pType == "04" || pType == "05"
            //                                ? Convert.ToString(11.3)
            //                                : Convert.ToString(10.3)},

            //                        {"D1@2-1-1", pType == "04" || pType == "05"
            //                                ? Convert.ToString(11.3)
            //                                : Convert.ToString(10.3)},

            //                        {"D2@Эскиз39", pType == "04" || pType == "05"
            //                                ? Convert.ToString(11.3)
            //                                : Convert.ToString(10.3)},

            //                        {"D1@Эскиз39", pType == "04" || pType == "05"
            //                                ? Convert.ToString(5.0)
            //                                : Convert.ToString(4.0)},

            //                        //Рамка усиливающая
            //                        {"D1@Кривая9", pType == "01" || pType == "35"
            //                                ? Convert.ToString(колСаморезВинтШирина - 1000)
            //                                : Convert.ToString(колСаморезВинтШирина)},

            //                        {"D1@Кривая7", Convert.ToString(колЗаклепокВысота)},

            //                        {"D3@Эскиз56", Convert.ToString(отступОтветныхОтверстийШирина)},

            //                        //Размеры для отверсти под клепальные гайки под съемные панели
            //                        {"G0@Эскиз49", Convert.ToString(OutputHolesWrapper.G0)},
            //                        {"G1@Эскиз49", Convert.ToString(OutputHolesWrapper.G1)},
            //                        {"G2@Эскиз49", Convert.ToString(OutputHolesWrapper.G2)},
            //                        {"G3@Эскиз49", Convert.ToString(OutputHolesWrapper.G0)},

            //                        //Convert.ToString(количествоВинтов)
            //                        {"L1@Эскиз49", Convert.ToString(OutputHolesWrapper.L1)},
            //                        {"D1@Кривая10", Convert.ToString(OutputHolesWrapper.D1)},
            //                        {"L2@Эскиз49", Convert.ToString(OutputHolesWrapper.L2)},
            //                        {"D1@Кривая11", Convert.ToString(OutputHolesWrapper.D2)},
            //                        {"L3@Эскиз49", Convert.ToString(OutputHolesWrapper.L3)},
            //                        {"D1@Кривая12", Convert.ToString(OutputHolesWrapper.D3)},

            //                        //Размеры промежуточных профилей
            //                        {"Wp1@Эскиз59", Math.Abs(ValProfils.Wp1) < 1 ? "10" : Convert.ToString(ValProfils.Wp1)},
            //                        {"Wp2@Эскиз59", Math.Abs(ValProfils.Wp2) < 1 ? "10" : Convert.ToString(ValProfils.Wp2)},
            //                        {"Wp3@Эскиз59", Math.Abs(ValProfils.Wp3) < 1 ? "10" : Convert.ToString(ValProfils.Wp3)},
            //                        {"Wp4@Эскиз59", Math.Abs(ValProfils.Wp4) < 1 ? "10" : Convert.ToString(ValProfils.Wp4)},

            //                        //todo Для промежуточной панели отверстия
            //                        {"D1@Кривая14", Convert.ToString(колЗаклепокВысота*2)},

            //                        {"Толщина@Листовой металл", materialP2[1].Replace('.', ',')},

            //                        // Кол-во отверстий под заклепки сшивочных кронштейнов
            //                        {"D1@CrvPatternW", Convert.ToString(колЗаклепокКронштейнаДвойнойПанели)},
            //                        {"D1@CrvPatternH", Convert.ToString(колЗаклепокКронштейнаДвойнойПанели)}
            //                    },
            //                    true,
            //                    типДвойнойНижней != "0"
            //                        ? new[]
            //                        {
            //                            $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойНижней1}",
            //                            $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойНижней2}"
            //                        }
            //                        : null);

            //                VentsMatdll(materialP2, new[] { покрытие[7], покрытие[4], покрытие[5] }, newName);

            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            //            }

            //            #endregion



            //            #region Теплоизоляция

            //            #region наименование теплоизоляции

            //            //6700  Лента уплотнительная Pes20x3/25 A/AT-B
            //            //14800  Лента двохсторонняя акриловая HSA 19х2
            //            //4900  Материал теплоизол. Сlassik TWIN50

            //            //newName = modelName + "-03-" + width + "-" + lenght + "-" + "40";

            //            #endregion

            //            newName = теплоизоляция.NewName;
            //            newPartPath = $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}.SLDPRT";
            //            if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1)) {
            //                SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //                DocumentExtension.SelectByID2("02-11-03-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-03-40-.SLDPRT");
            //            }
            //            else {
            //                SwPartParamsChangeWithNewName("02-11-03-40-",
            //                    $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}",
            //                        new[,]
            //                        {
            //                            {"D2@Эскиз1", Convert.ToString(высотаПанели-1)},
            //                            {"D1@Эскиз1", Convert.ToString(ширинаПанели-2)}
            //                        },
            //                        true,
            //                        null);
            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            //            }

            //            #endregion

            //            #region Скотч

            //            const double rizn = 3;

            //            if (скотч) {
            //                //Скотч

            //                newName = cкотч.NewName;
            //                newPartPath = $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}.SLDPRT";
            //                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1)) {
            //                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //                    DocumentExtension.SelectByID2("02-11-04-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-04-40-.SLDPRT");
            //                }
            //                else {
            //                    SwPartParamsChangeWithNewName("02-11-04-40-",
            //                        $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}",
            //                        new[,]
            //                        {
            //                            {"D2@Эскиз1", Convert.ToString(высотаПанели - rizn)},
            //                            {"D1@Эскиз1", Convert.ToString(ширинаПанели - rizn)}
            //                        },
            //                        true,
            //                        null);
            //                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            //                }
            //            }

            //            #endregion

            //            #region  Pes 20x3/25 A/AT-BT 538x768

            //            newName = pes.NewName;
            //            newPartPath = $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}.SLDPRT";

            //            if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1)) {
            //                SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //                DocumentExtension.SelectByID2("02-11-05-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-05-40-.SLDPRT");
            //            }
            //            else {
            //                SwPartParamsChangeWithNewName("02-11-05-40-",
            //                    $@"{Settings.Default.DestinationFolder}\{Panels0201}\Materials\{newName}",
            //                        new[,]
            //                        {
            //                            {"D2@Эскиз1", Convert.ToString(высотаПанели - rizn)},
            //                            {"D1@Эскиз1", Convert.ToString(ширинаПанели - rizn)}
            //                        },
            //                        true,
            //                        null);
            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            //            }

            //            #endregion

            //            #region Кронштейн усиливающей панели

            //            if (типКрепежнойЧастиУсиливающейПанели == "D") {
            //                if (кронштейнДверной == null)
            //                    goto m1;
            //                newName = кронштейнДверной.NewName;
            //                //newName = "02-11-09-40-" + lenght;
            //                newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";

            //                if (GetExistingFile(Path.GetFileNameWithoutExtension(newPartPath), 1)) {
            //                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //                    DocumentExtension.SelectByID2("02-11-09-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-09-40-.SLDPRT");
            //                }
            //                else {
            //                    SwPartParamsChangeWithNewName("02-11-09-40-",
            //                        $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}",
            //                            new[,]
            //                        {
            //                            {"D2@Эскиз1", Convert.ToString(высотаПанели - 45)},
            //                            {"D1@Эскиз1", скотч ? Convert.ToString(16.0) : Convert.ToString(17.5)},
            //                            {"D1@Кривая1", Convert.ToString(колЗаклепокВысота)}
            //                        },
            //                        true,
            //                        null);
            //                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            //                }
            //            }

            //m1:

            //#endregion

            //#region Разрезные части

            //            if (!string.IsNullOrEmpty(типДвойнойРазрез)) {
            //                #region to delete

            //                //var имяДвойнойВерхней1 = панельВнешняя.NewName + "-" + типДвойнойВерхней + "1";
            //                //var имяДвойнойВерхней2 = панельВнешняя.NewName + "-" + типДвойнойВерхней + "2";
            //                //var имяДвойнойНижней1 = панельВнутренняя.NewName + "-" + типДвойнойНижней + "1";
            //                //var имяДвойнойНижней2 = панельВнутренняя.NewName + "-" + типДвойнойНижней + "2";

            //                //MessageBox.Show("имяДвойнойВерхней1 - " + имяДвойнойВерхней1 + "\nимяДвойнойВерхней2 - " +
            //                //                имяДвойнойВерхней2 + "\nимяДвойнойНижней1 - " + имяДвойнойНижней1 +
            //                //                "\nимяДвойнойНижней2 - " + имяДвойнойНижней1);

            //                #endregion

            //                if (типДвойнойВерхней != "0") {
            //                    partsToDeleteList.Add(outerPanel);

            //                    newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойВерхней1}.SLDPRT";
            //                    DocumentExtension.SelectByID2(панельВнешняя.NewName + "-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                    newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойВерхней2}.SLDPRT";
            //                    DocumentExtension.SelectByID2(панельВнешняя.NewName + "-3@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                }

            //                if (типДвойнойНижней != "0") {
            //                    partsToDeleteList.Add(innerPanel);

            //                    newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойНижней1}.SLDPRT";
            //                    DocumentExtension.SelectByID2(панельВнутренняя.NewName + "-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                    newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{имяДвойнойНижней2}.SLDPRT";
            //                    DocumentExtension.SelectByID2(панельВнутренняя.NewName + "-3@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                    swAsm.ReplaceComponents(newPartPath, "", false, true);
            //                }

            //                switch (типДвойнойВерхней) {
            //                    case "1":
            //                        DocumentExtension.SelectByID2("Cut-ExtrudeW1@" + имяДвойнойВерхней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditUnsuppress2();
            //                        DocumentExtension.SelectByID2("Cut-ExtrudeW2@" + имяДвойнойВерхней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditUnsuppress2();
            //                        DocumentExtension.SelectByID2("Rivet Bralo-185@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        break;

            //                    case "2":
            //                        DocumentExtension.SelectByID2("Cut-ExtrudeH1@" + имяДвойнойВерхней1 + "-2@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditUnsuppress2();
            //                        DocumentExtension.SelectByID2("Cut-ExtrudeH2@" + имяДвойнойВерхней2 + "-3@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditUnsuppress2();
            //                        DocumentExtension.SelectByID2("Rivet Bralo-186@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        break;

            //                    case "0":
            //                        DocumentExtension.SelectByID2(панельВнешняя.NewName + "-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        DocumentExtension.SelectByID2(панельВнешняя.NewName + "-3@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();

            //                        DocumentExtension.SelectByID2("Rivet Bralo-185@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        DocumentExtension.SelectByID2("Rivet Bralo-186@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
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
            //                        DocumentExtension.SelectByID2(панельВнутренняя.NewName + "-2@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        DocumentExtension.SelectByID2(панельВнутренняя.NewName + "-3@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //                        SolidWorksDocument.EditDelete();
            //                        break;
            //                }

            //                DocumentExtension.SelectByID2("D1@PLANE1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //                ((Dimension)(SolidWorksDocument.Parameter("D1@PLANE1"))).SystemValue = 40.0 / 1000;
            //                SolidWorksDocument.EditRebuild3();

            //                foreach (var component in new[] { "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" }) {
            //                    DocumentExtension.SelectByID2("DerivedCrvPattern" + component + "@" + AssemblyName, "COMPPATTERN", 0, 0, 0, true, 0, null, 0);
            //                    swAsm.DissolveComponentPattern();
            //                }
            //            }

            //            #endregion

            //            #endregion

            //            #region Задание имени сборки (description Наименование)

            //            //switch (pType) {
            //            //    case "Несъемная":
            //            //    case "Съемная":
            //            //        pType = pType + " панель";
            //            //        break;
            //            //}

            //            SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName, true, 0)));
            //            //GabaritsForPaintingCamera(SolidWorksDocument);

            //            #endregion

            //            #region Сохранение и регистрация сборки в базе

            //            SolidWorksDocument.EditRebuild3();
            //            SolidWorksDocument.ForceRebuild3(true);
            //            SolidWorksDocument.SaveAs2(newFramelessPanelPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);

            //            NewComponentsFull.Add(new VentsCadFile {
            //                LocalPartFileInfo = new FileInfo(newFramelessPanelPath).FullName,
            //                PartIdSql = idAsm
            //            });

            //            try {
            //                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(new FileInfo(newFramelessPanelPath).Name);
            //            }
            //            catch (Exception) {
            //                //
            //            }

            //            //  List<VentsCadFile> outList;



            //            foreach (var item in outList) {
            //                var typeFile = 0;
            //                if (item.LocalPartFileInfo.ToUpper().Contains(".SLDASM")) { typeFile = 2; }
            //                if (item.LocalPartFileInfo.ToUpper().Contains(".SLDPRT")) { typeFile = 1; }

            //                //MessageBox.Show("typeFile - " + typeFile + "\n PartIdPdm - " + item.PartIdPdm + "\n PartIdSql - " + item.PartIdSql);

            //                if (item.PartIdPdm != 0) {
            //                    //    sqlBaseData.AirVents_SetPDMID(typeFile, item.PartIdPdm, item.PartIdSql);
            //                }
            //            }


            //            #region Auto Export to XML

            //        //    foreach (var newComponent in NewComponents) {
            //                //MessageBox.Show(newComponent.Name);
            //                // todo open for users
            //                //PartInfoToXml(newComponent.FullName);
            //          //  }

            //            #endregion

            //            #endregion


            //            #region Начальные проверки и пути



            //            bool needToAddStepInsertionAndStep;

            //          //  var pType = typeOfPanel[0];

            //          //string типУсиливающей = ТипУсиливающей();

            //        //   bool усилисвающя = Усиливающая(); 

            //            //if (усилисвающя) {
            //            //    pType = "01";
            //            //}

            //            switch (framelessPanel.PanelType) {
            //                case  PanelType_e.BlankPanel:
            //                case  PanelType_e.RemovablePanel:
            //                //case "05":
            //                case  PanelType_e.FrontPanel:
            //                    needToAddStepInsertionAndStep = false;
            //                    break;
            //                default:
            //                    needToAddStepInsertionAndStep = true;
            //                    break;
            //            }

            //            if (Усиливающая()) {
            //                needToAddStepInsertionAndStep = true;
            //            }

            //            var panelsUpDownConfigString =
            //                (framelessPanel.PanelType !=  PanelType_e.RemovablePanel   & framelessPanel.PanelType !=  PanelType_e.BlankPanel & framelessPanel.PanelType !=  PanelType_e.FrontPanel)
            //                    ?InputHolesWrapper.InValUpDown() : "";

            //            #region Обозначение ДЕТАЛЕЙ и СБОРКИ из БАЗЫ


            //            #region Задание наименований

            //            //var sqlBaseData = new SqlBaseData();
            //          //  var newId = sqlBaseData.PanelNumber() + 1;
            //          ///  var partIds = new List<KeyValuePair<int, int>>();

            //            #region панельВнешняя, панельВнутренняя            

            //         //   var панельВнешняя =
            //         //       new AddingPanel {
            //         //           PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //           ElementType = 1,
            //         //           Width = width,
            //         //           Height = height,
            //         //           PartThick = 40,
            //         //           PartMat = materialP1[0],
            //         //           PartMatThick = materialP1[1],
            //         //           Reinforcing = усиление,
            //         //           Ral = null,
            //         //           CoatingType = null,
            //         //           CoatingClass = null,
            //         //           Mirror = config.Contains("01"),
            //         //           StickyTape = скотч,
            //         //           StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //           AirHole = типТорцевой
            //         //       };
            //         //  // var id = панельВнешняя.AddPart();

            //         ////   панельВнешняя.PartQuery =
            //         //   //    $"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}, ElementType = {1}\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)},\nPartThick = 40, PartMat = {Convert.ToInt32(materialP1[0])}, PartMatThick = {Convert.ToDouble(materialP1[1].Replace('.', ','))}\nReinforcing = {усиление}, Ral = {покрытие[0]}, CoatingClass = {Convert.ToInt32(покрытие[2])}\nMirror = {config.Contains("01")}, StickyTape = {скотч}\nStepInsertion = {(needToAddStepInsertionAndStep ? расположениеВставок : null)}, AirHole = {типТорцевой}";



            //         //   //панельВнешняя.NewName = "02-" + typeOfPanel[0] + "-1-" + id;

            //         //   var панельВнутренняя =
            //         //       new AddingPanel {
            //         //           PanelTypeId = усилисвающя ? Convert.ToInt32(первыйТип[2]) : Convert.ToInt32(typeOfPanel[2]),
            //         //           ElementType = 2,
            //         //           Width = Convert.ToInt32(width),
            //         //           Height = Convert.ToInt32(height),
            //         //           PartThick = 40,
            //         //           PartMat = Convert.ToInt32(materialP2[0]),
            //         //           PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //         //           Reinforcing = усиление,
            //         //           Ral = покрытие[3],
            //         //           CoatingType = покрытие[4],
            //         //           CoatingClass = Convert.ToInt32(покрытие[5]),
            //         //           Mirror = config.Contains("01"),
            //         //           Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //           StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //           AirHole = типТорцевой
            //         //       };
            //         //   //id = панельВнутренняя.AddPart();

            //         //   //if (типДвойной == "00") {
            //         //   //    //   partIds.Add(new KeyValuePair<int, int>(2, id));
            //         //   //}

            //         //   //панельВнутренняя.NewName = "02-" + pType + "-2-" + id;
            //         //   //панельВнутренняя.PartQuery =
            //         //   //    $" PanelTypeId = {Convert.ToInt32(typeOfPanel[2])},ElementType = 2\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)}\nPartThick = {40}, PartMat = {Convert.ToInt32(materialP2[0])}, PartMatThick = {Convert.ToDouble(materialP2[1].Replace('.', ','))}\nReinforcing = {усиление}, Ral = {покрытие[3]}, CoatingType = {покрытие[4]}, CoatingClass = {Convert.ToInt32(покрытие[5])}\nMirror = {config.Contains("01")}, Step = {(needToAddStepInsertionAndStep ? расположениеПанелей : null)}, StickyTape = {скотч}\nStepInsertion = {(needToAddStepInsertionAndStep ? расположениеВставок : null)}, AirHole = {типТорцевой}";

            //         //   // Сшитые панели Внешняя тип + первая/вторая = 11, 12 или 21, 22 тоже и с нижней

            //         //   var имяДвойнойВерхней1 = "";
            //         //   var имяДвойнойВерхней2 = "";
            //         //   var имяДвойнойНижней1 = "";
            //         //   var имяДвойнойНижней2 = "";

            //         //   AddingPanel панельВнешняяДвойная1 = null;
            //         //   AddingPanel панельВнешняяДвойная2 = null;
            //         //   AddingPanel панельВнутренняяДвойная1 = null;
            //         //   AddingPanel панельВнутренняяДвойная2 = null;

            //         //   if (типДвойной != "00") {
            //         //       панельВнешняяДвойная1 =
            //         //           new AddingPanel {
            //         //               PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //               ElementType = Convert.ToInt32(типДвойной.Remove(1, 1) + "1"),
            //         //               Width = Convert.ToInt32(width),
            //         //               Height = Convert.ToInt32(height),
            //         //               PartThick = 40,
            //         //               PartMat = Convert.ToInt32(materialP1[0]),
            //         //               PartMatThick = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //         //               Reinforcing = усиление,
            //         //               Ral = покрытие[0],
            //         //               CoatingType = покрытие[1],
            //         //               CoatingClass = Convert.ToInt32(покрытие[2]),
            //         //               Mirror = config.Contains("01"),
            //         //               StickyTape = скотч,//.Contains("Со скотчем"),
            //         //               StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //               AirHole = типТорцевой
            //         //           };
            //         //     //  id = панельВнешняяДвойная1.AddPart();

            //         //    //   partIds.Add(new KeyValuePair<int, int>(Convert.ToInt32(типДвойной.Remove(1, 1) + "1"), id));
            //         //    //   панельВнешняяДвойная1.NewName = "02-" + pType + "-1-" + id;
            //         //    //   имяДвойнойВерхней1 = панельВнешняяДвойная1.NewName;

            //         //    //   панельВнешняяДвойная1.PartQuery =
            //         //   //    $"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}, ElementType = {Convert.ToInt32(типДвойной.Remove(1, 1) + "1")}\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)},\nPartThick = 40, PartMat = {Convert.ToInt32(materialP1[0])}, PartMatThick = {Convert.ToDouble(materialP1[1].Replace('.', ','))}\nReinforcing = {усиление}, Ral = {покрытие[0]}, CoatingClass = {Convert.ToInt32(покрытие[2])}\nMirror = {config.Contains("01")}, StickyTape = {скотч}\nStepInsertion = {(needToAddStepInsertionAndStep ? расположениеВставок : null)}, AirHole = {типТорцевой}";

            //         //       панельВнешняяДвойная2 =
            //         //           new AddingPanel {
            //         //               PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //               ElementType = Convert.ToInt32(типДвойной.Remove(1, 1) + "2"),
            //         //               Width = Convert.ToInt32(width),
            //         //               Height = Convert.ToInt32(height),
            //         //               PartThick = 40,
            //         //               PartMat = Convert.ToInt32(materialP1[0]),
            //         //               PartMatThick = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //         //               Reinforcing = усиление,
            //         //               Ral = покрытие[0],
            //         //               CoatingType = покрытие[1],
            //         //               CoatingClass = Convert.ToInt32(покрытие[2]),
            //         //               Mirror = config.Contains("01"),
            //         //               StickyTape = скотч,//.Contains("Со скотчем"),
            //         //               StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //               AirHole = типТорцевой
            //         //           };
            //         //    //   id = панельВнешняяДвойная2.AddPart();
            //         //    //   partIds.Add(new KeyValuePair<int, int>(Convert.ToInt32(типДвойной.Remove(1, 1) + "2"), id));
            //         //    //   панельВнешняяДвойная2.NewName = "02-" + pType + "-1-" + id;
            //         //    //   имяДвойнойВерхней2 = панельВнешняяДвойная2.NewName;
            //         //    //   панельВнешняяДвойная2.PartQuery =
            //         //    //       $"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}, ElementType = {Convert.ToInt32(типДвойной.Remove(1, 1) + "2")}\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)},\nPartThick = 40, PartMat = {Convert.ToInt32(materialP1[0])}, PartMatThick = {Convert.ToDouble(materialP1[1].Replace('.', ','))}\nReinforcing = {усиление}, Ral = {покрытие[0]}, CoatingClass = {Convert.ToInt32(покрытие[2])}\nMirror = {config.Contains("01")} StickyTape = {скотч}\nStepInsertion = {(needToAddStepInsertionAndStep ? расположениеВставок : null)}, AirHole = {типТорцевой}";


            //         //       if (типДвойной.Remove(0, 1) != "0") {
            //         //           панельВнутренняяДвойная1 =
            //         //               new AddingPanel {
            //         //                   PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //                   ElementType = Convert.ToInt32(типДвойной.Remove(0, 1) + "1"),
            //         //                   Width = Convert.ToInt32(width),
            //         //                   Height = Convert.ToInt32(height),
            //         //                   PartThick = 40,
            //         //                   PartMat = Convert.ToInt32(materialP2[0]),
            //         //                   PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //         //                   Reinforcing = усиление,
            //         //                   Ral = покрытие[3],
            //         //                   CoatingType = покрытие[4],
            //         //                   CoatingClass = Convert.ToInt32(покрытие[5]),
            //         //                   Mirror = config.Contains("01"),
            //         //                   Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //                   StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //                   AirHole = типТорцевой
            //         //               };
            //         //       //    id = панельВнутренняяДвойная1.AddPart();
            //         //      //     partIds.Add(new KeyValuePair<int, int>(Convert.ToInt32(типДвойной.Remove(0, 1) + "1"), id));
            //         //      //     панельВнутренняяДвойная1.NewName = "02-" + pType + "-2-" + id;
            //         //     //      имяДвойнойНижней1 = панельВнутренняяДвойная1.NewName;

            //         //           панельВнутренняяДвойная2 =
            //         //               new AddingPanel {
            //         //                   PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //                   ElementType = Convert.ToInt32(типДвойной.Remove(0, 1) + "2"),
            //         //                   Width = Convert.ToInt32(width),
            //         //                   Height = Convert.ToInt32(height),
            //         //                   PartThick = 40,
            //         //                   PartMat = Convert.ToInt32(materialP2[0]),
            //         //                   PartMatThick = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //         //                   Reinforcing = усиление,
            //         //                   Ral = покрытие[3],
            //         //                   CoatingType = покрытие[4],
            //         //                   CoatingClass = Convert.ToInt32(покрытие[5]),
            //         //                   Mirror = config.Contains("01"),
            //         //                   Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //                   StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //         //                   AirHole = типТорцевой
            //         //               };
            //         //      //     id = панельВнутренняяДвойная2.AddPart();
            //         //     //      partIds.Add(new KeyValuePair<int, int>(Convert.ToInt32(типДвойной.Remove(0, 1) + "2"), id));
            //         //    //       панельВнутренняяДвойная2.NewName = "02-" + pType + "-2-" + id;
            //         //     //      имяДвойнойНижней2 = панельВнутренняяДвойная2.NewName;
            //         //       }
            //         //   }

            //         //   #endregion

            //         //   #region теплоизоляция, cкотч, усиливающаяРамкаПоШирине, усиливающаяРамкаПоВысоте

            //         //   //var теплоизоляция =
            //         //   //    new AddingPanel {
            //         //   //        PanelTypeId = typeOfPanel[2],
            //         //   //        ElementType = 3,
            //         //   //        Width = width,
            //         //   //        Height = height,
            //         //   //        PartThick = 40,
            //         //   //        PartMat = 4900,

            //         //   //        PartMatThick = 1,
            //         //   //        Ral = "Без покрытия",
            //         //   //        CoatingType = "0",
            //         //   //        CoatingClass = 0,
            //         //   //        AirHole = типТорцевой
            //         //   //    };
            //         // //  id = теплоизоляция.AddPart();
            //         ////   partIds.Add(new KeyValuePair<int, int>(3, id));
            //         ////   теплоизоляция.NewName = "02-" + id;
            //         ////   теплоизоляция.PartQuery =
            //         // //      $"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}, ElementType = 3\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)}\nPartThick = 40, PartMat = 4900, PartMatThick = 1\nRal = Без покрытия, CoatingType = 0, CoatingClass = {Convert.ToInt32("0")}\nAirHole = {типТорцевой}";

            //         //   //AddingPanel cкотч = null;

            //         //   //if (скотч)// Со скотчем
            //         //   //{
            //         //   //    cкотч =
            //         //   //        new AddingPanel {
            //         //   //            PanelTypeId = 14,
            //         //   //            ElementType = 4,
            //         //   //            Width = width,
            //         //   //            Height = height,
            //         //   //            PartThick = 40,
            //         //   //            PartMat = 14800,
            //         //   //            PartMatThick = 1,
            //         //   //            Ral = "Без покрытия",
            //         //   //            CoatingType = "0",
            //         //   //            CoatingClass = Convert.ToInt32("0")
            //         //   //        };
            //         //  //     id = cкотч.AddPart();
            //         //  //     partIds.Add(new KeyValuePair<int, int>(4, id));
            //         //   //    cкотч.NewName = "02-" + id;
            //         //   //    cкотч.PartQuery =
            //         //  //         $"PanelTypeId = 14, ElementType = 4\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)}\nPartThick = 40, PartMat = 14800, PartMatThick = 1, Ral = Без покрытия\nCoatingType = 0, CoatingClass = {Convert.ToInt32("0")}";
            //         //   }

            //         //   var pes =
            //         //       new AddingPanel {
            //         //           PanelTypeId = 15,
            //         //           ElementType = 5,
            //         //           Width = Convert.ToInt32(width),
            //         //           Height = Convert.ToInt32(height),
            //         //           PartThick = 40,
            //         //           PartMat = 6700,

            //         //           PartMatThick = 1,
            //         //           Ral = "Без покрытия",
            //         //           CoatingType = "0",
            //         //           CoatingClass = Convert.ToInt32("0")
            //         //       };
            //         // //  id = pes.AddPart();
            //         ////   partIds.Add(new KeyValuePair<int, int>(5, id));
            //         ////   pes.NewName = "02-" + id;
            //         ////   pes.PartQuery =
            //         ////       $"PanelTypeId = 15, ElementType = 5\nWidth = {Convert.ToInt32(width)}, Height = {Convert.ToInt32(height)}\nPartThick = 40, PartMat = 6700, PartMatThick = 1\nRal = Без покрытия, CoatingType = 0\nCoatingClass = {Convert.ToInt32("0")}";

            //         //   AddingPanel усиливающаяРамкаПоШирине = null;
            //         //   AddingPanel усиливающаяРамкаПоШирине2 = null;
            //         //   AddingPanel усиливающаяРамкаПоВысоте = null;

            //         //   if (усиление) {
            //         //       усиливающаяРамкаПоШирине =
            //         //           new AddingPanel {
            //         //               PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //               ElementType = 6,
            //         //               Height = 40,
            //         //               Width = Convert.ToInt32(width),
            //         //               PartThick = 40,
            //         //               PartMat = 1800,
            //         //               PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //               Mirror = config.Contains("01"),
            //         //               Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //               StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,

            //         //               Ral = "Без покрытия",
            //         //               CoatingType = "0",
            //         //               CoatingClass = Convert.ToInt32("0")
            //         //           };
            //         //   //    id = усиливающаяРамкаПоШирине.AddPart();
            //         //    //   partIds.Add(new KeyValuePair<int, int>(6, id));
            //         //    //   усиливающаяРамкаПоШирине.NewName = "02-" + id;

            //         //       if (pType != "01") {
            //         //           усиливающаяРамкаПоШирине2 =
            //         //               new AddingPanel {
            //         //                   PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //                   ElementType = 62,
            //         //                   Height = 40,
            //         //                   Width = Convert.ToInt32(width),
            //         //                   PartThick = 40,
            //         //                   PartMat = 1800,
            //         //                   PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //                   Mirror = config.Contains("01"),
            //         //                   Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //                   StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,

            //         //                   Ral = "Без покрытия",
            //         //                   CoatingType = "0",
            //         //                   CoatingClass = Convert.ToInt32("0")
            //         //               };
            //         //      //     id = усиливающаяРамкаПоШирине2.AddPart();
            //         //       //    partIds.Add(new KeyValuePair<int, int>(62, id));
            //         //       //    усиливающаяРамкаПоШирине2.NewName = "02-" + id;
            //         //       }

            //         //       усиливающаяРамкаПоВысоте =
            //         //           new AddingPanel {
            //         //               PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //               ElementType = 7,
            //         //               Height = Convert.ToInt32(height),
            //         //               Width = 40,
            //         //               PartThick = 40,
            //         //               PartMat = 1800,
            //         //               PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //               Mirror = config.Contains("01"),
            //         //               Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //         //               StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,

            //         //               Ral = "Без покрытия",
            //         //               CoatingType = "0",
            //         //               CoatingClass = Convert.ToInt32("0")
            //         //           };
            //         //    //   id = усиливающаяРамкаПоВысоте.AddPart();
            //         //    //   partIds.Add(new KeyValuePair<int, int>(7, id));
            //         //    //   усиливающаяРамкаПоВысоте.NewName = "02-" + id;
            //         //   }

            //         //   AddingPanel кронштейнДверной = null;

            //         //   if (усилисвающя) {
            //         //       if (ТипУсиливающей().Remove(0, 1).Contains("D")) {
            //         //           //MessageBox.Show(Усиливающая(pType) + "\n" + типУсиливающей.Remove(0, 1).Contains("D"));
            //         //           кронштейнДверной = new AddingPanel {
            //         //               PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //               ElementType = 9,
            //         //               Height = Convert.ToInt32(height),
            //         //               Width = 20,
            //         //               PartThick = 40,
            //         //               PartMat = 1800,
            //         //               PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //               Mirror = config.Contains("01"),

            //         //               Ral = "Без покрытия",
            //         //               CoatingType = "0",
            //         //               CoatingClass = Convert.ToInt32("0")
            //         //           };
            //         //     //      id = кронштейнДверной.AddPart();
            //         //     //      partIds.Add(new KeyValuePair<int, int>(9, id));
            //         //     //      кронштейнДверной.NewName = "02-" + id;
            //         //    //       кронштейнДверной.PartQuery = $"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}, ElementType = 9\nHeight = {Convert.ToInt32(height)}, Width = 20\nPartThick = 40, PartMat = 1800\nPartMatThick = {Convert.ToDouble("1".Replace('.', ','))}, Mirror = {config.Contains("01")}\n,Ral = Без покрытия, CoatingType = 0\nCoatingClass = {Convert.ToInt32("0")}";
            //         //       }
            //         //   }

            //         //   AddingPanel профильТорцевойРамкиВертикальный = null;
            //         //   AddingPanel профильТорцевойРамкиГоризонтальный = null;

            //         //   if (типТорцевой != null) {
            //         //       профильТорцевойРамкиВертикальный = new AddingPanel {
            //         //           PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //           ElementType = 11,
            //         //           Height = (int)HeightOfWindow,
            //         //           Width = 40,
            //         //           PartThick = 40,
            //         //           PartMat = 1800,
            //         //           PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //           Mirror = config.Contains("01"),

            //         //           Ral = "Без покрытия",
            //         //           CoatingType = "0",
            //         //           CoatingClass = Convert.ToInt32("0"),

            //         //           AirHole = типТорцевой
            //         //       };
            //         //  //     id = профильТорцевойРамкиВертикальный.AddPart();
            //         //   //    partIds.Add(new KeyValuePair<int, int>(11, id));
            //         //   //    профильТорцевойРамкиВертикальный.NewName = "02-" + id;


            //         //       профильТорцевойРамкиГоризонтальный = new AddingPanel {
            //         //           PanelTypeId = Convert.ToInt32(typeOfPanel[2]),
            //         //           ElementType = 12,
            //         //           Height = (int)WidthOfWindow,//BackProfils.Height,
            //         //           Width = 40,
            //         //           PartThick = 40,
            //         //           PartMat = 1800,
            //         //           PartMatThick = Convert.ToDouble("1".Replace('.', ',')),
            //         //           Mirror = config.Contains("01"),

            //         //           Ral = "Без покрытия",
            //         //           CoatingType = "0",
            //         //           CoatingClass = Convert.ToInt32("0"),

            //         //           AirHole = типТорцевой
            //         //       };
            //         //   //    id = профильТорцевойРамкиГоризонтальный.AddPart();
            //         //    //   partIds.Add(new KeyValuePair<int, int>(12, id));
            //         //    //   профильТорцевойРамкиГоризонтальный.NewName = "02-" + id;
            //         //   }

            //            #endregion

            //            #region Сборка панели


            //            //var iDs = "";

            //            //var idAsm = 0;
            //            //foreach (var сборка in partIds.Select(partId => new AddingPanel {
            //            //    PartId = partId.Value,

            //            //    PanelTypeId = Convert.ToInt32(typeOfPanel[2]),

            //            //    ElementType = partId.Key,

            //            //    Width = Convert.ToInt32(width),
            //            //    Height = Convert.ToInt32(height),

            //            //    PanelMatOut = Convert.ToInt32(materialP1[0]),
            //            //    PanelMatIn = Convert.ToInt32(materialP2[0]),
            //            //    PanelThick = 40,
            //            //    PanelMatThickOut = Convert.ToDouble(materialP1[1].Replace('.', ',')),
            //            //    PanelMatThickIn = Convert.ToDouble(materialP2[1].Replace('.', ',')),
            //            //    RalOut = покрытие[0],
            //            //    RalIn = покрытие[0],
            //            //    CoatingTypeOut = покрытие[1],
            //            //    CoatingTypeIn = покрытие[1],
            //            //    CoatingClassOut = Convert.ToInt32(покрытие[2]),
            //            //    CoatingClassIn = Convert.ToInt32(покрытие[2]),

            //            //    Mirror = config.Contains("01"),
            //            //    Step = needToAddStepInsertionAndStep ? расположениеПанелей : null,
            //            //    StepInsertion = needToAddStepInsertionAndStep ? расположениеВставок : null,
            //            //    Reinforcing = усиление,
            //            //    StickyTape = скотч,//.Contains("Со скотчем"),

            //            //    AirHole = типТорцевой,

            //            //    PanelNumber = newId
            //            //})) {
            //            //    idAsm = сборка.Add();
            //            //    iDs = iDs + "\n" + idAsm;
            //            //}

            //            //MessageBox.Show(iDs);
            //            //return null;

            //            #endregion


            //          //  var обозначениеНовойПанели = "02-" + typeOfPanel[0] + "-" + idAsm;

            //            //existingAsmsAndParts.AddRange(new List<ExistingAsmsAndParts>
            //            //    {
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = обозначениеНовойПанели,
            //            //            Comment = "Сборка панели",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = $@"PanelTypeId = {Convert.ToInt32(typeOfPanel[2])}
            //            //            Width = {Convert.ToInt32(width)} Height = {Convert.ToInt32(height)}" +

            //            //            #region to delete
            //            //            //PanelMatOut = {Convert.ToInt32(materialP1[0])}
            //            //            //PanelMatIn = {Convert.ToInt32(materialP2[0])}
            //            //            //PanelMatThickOut = {Convert.ToDouble(materialP1[1].Replace('.', ','))}
            //            //            //PanelMatThickIn = {Convert.ToDouble(materialP2[1].Replace('.', ','))}
            //            //            //RalOut = {покрытие[0]}
            //            //            //RalIn = {покрытие[0]}
            //            //            //CoatingTypeOut = {покрытие[1]}
            //            //            //CoatingTypeIn = {покрытие[1]}
            //            //            //CoatingClassOut = {Convert.ToInt32(покрытие[2])}
            //            //            //CoatingClassIn = {Convert.ToInt32(покрытие[2])}
            //            //            #endregion

            //            //            $@" Mirror = {config.Contains("01")}
            //            //            Step = {(needToAddStepInsertionAndStep ? расположениеПанелей : null)}
            //            //            StepInsertion = {(needToAddStepInsertionAndStep ? расположениеВставок : null)}
            //            //            Reinforcing = {усиление}
            //            //            StickyTape = {скотч}
            //            //            AirHole = {типТорцевой}
            //            //            PanelNumber = {newId}"
            //            //        }
            //            //    });


            //            //if (типДвойной == "00") {
            //            //    existingAsmsAndParts.AddRange(new List<ExistingAsmsAndParts>
            //            //    {
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнешняя.NewName,
            //            //            Comment = "Панель Внешняя",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнешняя.PartQuery
            //            //        }
            //            //    });
            //            //}
            //            //else {
            //            //    existingAsmsAndParts.AddRange(new List<ExistingAsmsAndParts>
            //            //    {
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнешняяДвойная1.NewName,
            //            //            Comment = "Панель Внутренняя 1",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнешняяДвойная1.PartQuery
            //            //        },
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнешняяДвойная2.NewName,
            //            //            Comment = "Панель Внутренняя 2",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнешняяДвойная2.PartQuery
            //            //        }
            //            //    });
            //            //}

            //            //if (типДвойной == "00") {
            //            //    existingAsmsAndParts.AddRange(new List<ExistingAsmsAndParts>
            //            //    {
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнутренняя.NewName,
            //            //            Comment = "Панель Внутренняя",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнутренняя.PartQuery
            //            //        }
            //            //    });
            //            //}
            //            //else {
            //            //    existingAsmsAndParts.AddRange(new List<ExistingAsmsAndParts>
            //            //    {
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнутренняяДвойная1.NewName,
            //            //            Comment = "Панель Внутренняя 1",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнутренняяДвойная1.PartQuery
            //            //        },
            //            //        new ExistingAsmsAndParts
            //            //        {
            //            //            PartName = панельВнутренняяДвойная2.NewName,
            //            //            Comment = "Панель Внутренняя 2",
            //            //            IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //            PartQuery = панельВнутренняяДвойная2.PartQuery
            //            //        }
            //            //    });
            //            //}

            //            //if (кронштейнДверной != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = кронштейнДверной.NewName,
            //            //        Comment = "Кронштейн дверной",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //        PartQuery = кронштейнДверной.PartQuery
            //            //    });
            //            //}
            //            //if (cкотч != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = cкотч?.NewName,
            //            //        Comment = "Скотч",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1],
            //            //        PartQuery = cкотч?.PartQuery
            //            //    });
            //            //}
            //            //if (усиливающаяРамкаПоВысоте != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = усиливающаяРамкаПоВысоте?.NewName,
            //            //        Comment = "Усиливающая рамка по высоте",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1]
            //            //    });
            //            //}
            //            //if (усиливающаяРамкаПоШирине != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = усиливающаяРамкаПоШирине?.NewName,
            //            //        Comment = "Усиливающая рамка по ширине",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1]
            //            //    });
            //            //}
            //            //if (профильТорцевойРамкиВертикальный != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = профильТорцевойРамкиВертикальный?.NewName,
            //            //        Comment = "Профиль торцевой рамки вертикальный",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1]
            //            //    });
            //            //}
            //            //if (профильТорцевойРамкиГоризонтальный != null) {
            //            //    existingAsmsAndParts.Add(new ExistingAsmsAndParts {
            //            //        PartName = профильТорцевойРамкиГоризонтальный?.NewName,
            //            //        Comment = "Профиль торцевой рамки горизонтальный",
            //            //        IdAsm = обозначениеНовойПанели + " - " + typeOfPanel[1]
            //            //    });
            //            //}

            //            //if (onlySearch)
            //            //    return null;

            //            #endregion

            //            #endregion

            //            string[] frameProfils = null;

            //            if (типТорцевой != null) {

            //                    frameProfils = new[]
            //                    { 
            //                        FrameProfil(framelessPanel.WindowSize.Y + 60, framelessPanel.ThermoStrip,//.Contains("Со скотчем"),
            //                        "00",
            //                            BackProfils.Flange30, профильТорцевойРамкиВертикальный.NewName),
            //                        FrameProfil( framelessPanel.WindowSize.X+ 0, framelessPanel.ThermoStrip,//.Contains("Со скотчем"), 
            //                        "01",
            //                            BackProfils.Flange30, профильТорцевойРамкиГоризонтальный.NewName)
            //                    };
            //                }              


            //            switch (framelessPanel.PanelType) {
            //                case PanelType_e.RemovablePanel:
            //                    _destinationFolder = Panels0204; // Folder - 02-04-Removable Panels
            //                    break;
            //                default:
            //                    _destinationFolder = Panels0201; // Folder - 02-01-Panels
            //                    break;
            //            }


            //            #region Двойная

            //            var типДвойнойВерхней = "0";
            //            var типДвойнойНижней = "0";
            //            string типДвойнойРазрез = null;

            //            if (типДвойной != "00") {
            //                nameAsm = "02-11-40-2";

            //                NameUpPanel = "02-11-01-40-2-";
            //                NameDownPanel = "02-11-02-40-2-";

            //                типДвойнойВерхней = типДвойной.Remove(1, 1);
            //                типДвойнойНижней = типДвойной.Remove(0, 1);

            //                if (типДвойной.Contains("1")) {
            //                    типДвойнойРазрез = "W";
            //                }
            //                if (типДвойной.Contains("2")) {
            //                    типДвойнойРазрез = "H";
            //                }
            //            }

            //            #endregion

            //            //var modelPanelAsmbly = new FileInfo($@"{sourceFolder}{modelPanelsPath}\{nameAsm}.SLDASM").FullName;

            //            #endregion
        }


        string ТипУсиливающей() {
            switch (framelessPanel.PanelType) {
                case PanelType_e.ПростаяУсилПанель:
                    return "EE";
                case PanelType_e.ПодДвериНаПетлях:
                    return "ED";
                case PanelType_e.ПоДвериНаЗажимах:
                    return "EZ";
                case PanelType_e.ПодТорцевую:
                    return "TE";
                case PanelType_e.ПодТорцевуюИДвериНаЗажимах:
                    return "TZ";
                case PanelType_e.ПодТорцевуюИДвериНаПетлях:
                    return "TD";
                default:
                    return null;
            }
        }
        bool Усиливающая() {
            switch (framelessPanel.PanelType) {
                case PanelType_e.ПростаяУсилПанель:
                case PanelType_e.ПодДвериНаПетлях:
                case PanelType_e.ПоДвериНаЗажимах:
                case PanelType_e.ПодТорцевую:
                case PanelType_e.ПодТорцевуюИДвериНаЗажимах:
                case PanelType_e.ПодТорцевуюИДвериНаПетлях:
                    return true;
                default:
                    return false;
            }
        }




    }
}
 