using ServiceConstants;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless
{

    public class FramelessPanelBuilder : ProductBuilderBehavior
    {
        private FramelessPanel framelessPanel;
        private List<FramelessPanel> framelessPanelList;
        public override event SetBendsHandler SetBends;


        string NameUpPanel = "02-11-01-40-";
        string NameDownPanel = "02-11-02-40-";

        string config;
        public FramelessPanelBuilder(FramelessPanel framelessPanel, List<FramelessPanel> framelessPanelList, string config) : base()
        {

            this.framelessPanel = framelessPanel;
            this.framelessPanelList = framelessPanelList;
            SetProperties("panel", "01 - Frameless Design 40mm");
            this.config = config;
        }


        public void Buid()
        { Patterns.Observer.MessageObserver.Instance.SetMessage("Opened solid works");
            AssemblyName = "02-11-40-1";
            NewPartPath = System.IO.Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            Patterns.Observer.MessageObserver.Instance.SetMessage(NewPartPath);
            

           SolidWorksDocument =  SolidWorksAdapter.OpenDocument(NewPartPath, SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY);
           
          //  SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);

            Patterns.Observer.MessageObserver.Instance.SetMessage("calc data. press any key");
            CalculateHandle();

            Patterns.Observer.MessageObserver.Instance.SetMessage("delete data.");// press any key");
          //  //Console.ReadKey();
            DeleteComponents();
            Patterns.Observer.MessageObserver.Instance.SetMessage("set data. ");// press any key");

            //   //Console.ReadKey();
           // SetSize();
        }

        void CalculateHandle()
        {
            framelessPanel.widthHandle = framelessPanel.SizePanel.X / 4;

            if (framelessPanel.SizePanel.X < 1000)
            {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.5 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1000)
            {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.45 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1300)
            {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.4 * 0.5;
            }
            if (framelessPanel.SizePanel.X >= 1700)
            {
                framelessPanel.widthHandle = framelessPanel.SizePanel.X * 0.35 * 0.5;
            }
        }

        double количествоВинтов = 2000;

        void CalculateRemovablePanel(Vector2 panelSize,
                                   out Vector2 removablePanelSize,
                                    out double расстояниеL, out int количествоВинтов)
        {
            removablePanelSize = Vector2.Zero;

            removablePanelSize.X = panelSize.X - 2;
            removablePanelSize.Y = panelSize.Y - 2;
            расстояниеL = panelSize.X - 132;

            количествоВинтов = 5000;

            if (panelSize.X < 1100)
            {
                количествоВинтов = 4000;
            }
            if (panelSize.X < 700)
            {
                количествоВинтов = 3000;
            }
            if (panelSize.X < 365)
            {
                количествоВинтов = 2000;
            }
        }
        private void CalculateRivetCount() {
            double колЗаклепокВысота;


            if (framelessPanel.PanelType == PanelType_e.RemovablePanel) {
                Vector2 removebalePanelSize;
                double distanceL;
                int countScrews;
                CalculateRemovablePanel(framelessPanel.SizePanel, out removebalePanelSize, out distanceL, out countScrews);
                const double шагЗаклепокВысота = 125;
                колЗаклепокВысота = (Math.Truncate(removebalePanelSize.Y / шагЗаклепокВысота) + 1) * 1000;

            }
            else
            {
                // TO DO

                //    колЗаклепокВысота = колСаморезВинтВысота + 1000;
                if (Convert.ToInt32(framelessPanel.SizePanel.Y) > 1000)
                {
                    //  колЗаклепокВысота = колСаморезВинтВысота + 3000;
                }
            }
        }

        private void CalculateDoublePanel()
        {
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
            //SolidWorksDocument.Extension.SelectByID2(/*nameOfProfil + idToDelete + "@" + AssemblyName */ "", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
            //SolidWorksDocument.Extension.SelectByID2(/*nameOfProfilToDelete + "-1@" + AssemblyName*/"", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
            //SolidWorksDocument.Extension.SelectByID2(/* nameOfProfilToDelete + "-2@" + AssemblyName*/"", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //SolidWorksDocument.EditDelete();
        }

        /// <summary>
        ///  Расчет шага для саморезов и заклепок
        /// </summary>
        /// <param name="screwsCountByWidht"></param>
        /// <param name="screwscountByHeight"></param>
        private void GetCountScrews(ref double screwsCountByWidht, ref double screwscountByHeight)
        {
            double stepScrewsByWeidht = 200;
            double stepScrewsByHeight = 200;

            if (framelessPanel.SizePanel.X < 600)
            {
                stepScrewsByWeidht = 150;
            }
            if (framelessPanel.SizePanel.Y < 600)
            {
                stepScrewsByHeight = 150;
            }
            screwsCountByWidht = (Math.Truncate(framelessPanel.SizePanel.X / stepScrewsByWeidht) + 1) * 1000;
            screwscountByHeight = (Math.Truncate(framelessPanel.SizePanel.Y / stepScrewsByHeight) + 1) * 1000;

            if (framelessPanel.Screws?.ByHeightInnerUp > 1000)
            {
                screwscountByHeight = framelessPanel.Screws.ByHeightInnerUp;
            }
        }





        private void SetSize()
        {

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

            if (framelessPanel.ThermoStrip == ThermoStrip.ThermoScotch)
            {
                осьПоперечныеОтверстия = 10.1;
            }

            if (framelessPanel.PanelType != PanelType_e.RemovablePanel)
            {
                отступОтветныхОтверстийШирина = 47;
                осьСаморезВинт = 9.70;
                осьОтверстийСаморезВинт = 10.3;
            }
            #endregion

            #region Диаметры отверстий
            var диамЗаглушкаВинт = 13.1;
            var диамСаморезВинт = 3.3;

            if (framelessPanel.PanelType == PanelType_e.RemovablePanel)
            {
                диамЗаглушкаВинт = 11;
                диамСаморезВинт = 7;
            }
            #endregion

            if (CheckExistPart != null)
                CheckExistPart(base.PartName, out IsPartExist, out NewPartPath);

            PartName = "02-11-02-40-";
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2(PartName + "@02-11-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
            }
            else
            {

                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                if (SetBends != null)
                    SetBends((decimal)framelessPanel.outThickness, out KFactor, out BendRadius);

                Vector2 dimensions; // габариты
                if (framelessPanel.PanelType.Equals(PanelType_e.RemovablePanel))
                {

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
                    if (Convert.ToInt32(framelessPanel.SizePanel.Y) > 1000)
                    {
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
                if (framelessPanel.PanelType == PanelType_e.RemovablePanel && !framelessPanel.isOneHandle)
                {  base.parameters.Add("D4@Эскиз47", framelessPanel.widthHandle);
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

        protected override void DeleteComponents(int type=0)
        {
            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;


            ModelDocExtension solidWorksDocumentExtension = SolidWorksDocument.Extension;
            double HeightOfWindow = WindowProfils.Flange30 ? WindowProfils.Width : (WindowProfils.Width + 2);
            double WidthOfWindow = WindowProfils.Height;
            CompType_e bodyfeat = CompType_e.BODYFEATURE;
            CompType_e sketch = CompType_e.SKETCH;


            //var ftrfolder = VentsCad.CompType.FTRFOLDER;
            //var dimension = VentsCad.CompType.DIMENSION;           

            //var HeightOfWindow = BackProfils.Flange30 ?
            //        BackProfils.ByHeight / 1000 : (BackProfils.ByHeight + 2) / 1000;

            //MessageBox.Show(HeightOfWindow.ToString());



            //var supress = VentsCad.Act.Suppress; 
            //   var unSupress = VentsCad.Act.Unsuppress;
            // var doNothing = VentsCad.Act.DoNothing;



            if (framelessPanel.PanelType != PanelType_e.FrontPanel)
            {

                Console.WriteLine("framelessPanel.PanelType != PanelType_e.FrontPanel");
                //Удаление компонентов из сборки
                //solidWorksDocumentExtension.SelectByID2("Рамка", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                //SolidWorksDocument.EditDelete();

                //foreach (var component in new List<string>
                //{
                //    "02-11-11-40--1", "02-11-11-40--2", "02-11-11-40--3","02-11-11-40--4",
                //    "Threaded Rivets Increased-1", "Threaded Rivets Increased-2", "Threaded Rivets Increased-3", "Threaded Rivets Increased-4",
                //    "Rivet Bralo-71", "Rivet Bralo-72", "Rivet Bralo-73", "Rivet Bralo-74", "Rivet Bralo-75", "Rivet Bralo-76",
                //    "Rivet Bralo-83", "Rivet Bralo-84", "Rivet Bralo-91", "Rivet Bralo-92", "Rivet Bralo-93", "Rivet Bralo-94"
                //})
                //{
                //    solidWorksDocumentExtension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                //    SolidWorksDocument.EditDelete();
                //}

                //List<System.Runtime.InteropServices.DispatchWrapper> arrObjIn = new List<System.Runtime.InteropServices.DispatchWrapper>();

                //List<Feature> selObjs = new List<Feature>();
                //     SelectionMgr selectionMgr = (SelectionMgr)SolidWorksDocument.SelectionManager;
                //SelectData selectData = (SelectData)selectionMgr.CreateSelectData();

            //    SolidWorksAdapter.AcativeteDoc(NameUpPanel);

                FeatureManager featureManager = SolidWorksDocument.FeatureManager;



              var features =  featureManager.GetFeatures(false);

                foreach (var item in features)
                {
                  if (item.GetTypeName2().ToLower() == "cut")
                    {
                        Console.WriteLine(item.Name + " - "  + item.GetTypeName2().ToLower());
                   }
                }
                return;

             // var feature =  features.First(eachFeature => eachFeature.Name == "Вырез-Вытянуть21");

              ///  Console.WriteLine();

                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть21@" + NameUpPanel +"-1@" +AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                solidWorksDocumentExtension.SelectByID2("Рамка@" + NameUpPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть22@"+NameDownPanel+"-1@"+AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                solidWorksDocumentExtension.SelectByID2("Рамка@" + NameDownPanel + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть1@02-11-03-40-@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
               
               
                solidWorksDocumentExtension.SelectByID2("Рамка@02-11-03-40-@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
               
                SolidWorksDocument.EditDelete();

                solidWorksDocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.EditRebuild3();
            }
            else
            {
                solidWorksDocumentExtension.SelectByID2("Threaded Rivets-38@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                if (config.Contains("01"))
                {
                    solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.EditRebuild3();
                }
                else if (config.Contains("02"))
                {
                    solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть25@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocumentExtension.SelectByID2("Кривая3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть18@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();
                }

                solidWorksDocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.SelectByID2("Кривая6@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();


                // SolidWorksDocument.Extension.SelectByID2("D1@Кривая6@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                // ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая6@" + NameUpPanel + ".Part"))).SystemValue = колСаморезВинтВысота / 1000;

                SolidWorksDocument.Extension.SelectByID2("D2@Эскиз74@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз74@" + NameUpPanel + ".Part"))).SystemValue = 0.015;

                solidWorksDocumentExtension.SelectByID2("Threaded Rivets-37@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                SolidWorksDocument.Extension.SelectByID2("D2@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

                SolidWorksDocument.Extension.SelectByID2("D1@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;


                SolidWorksDocument.Extension.SelectByID2("D3@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WindowProfils.ByWidth / 1000;

                SolidWorksDocument.Extension.SelectByID2("D4@Эскиз68@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз68@" + NameUpPanel + ".Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                SolidWorksDocument.Extension.SelectByID2("D1@Эскиз72@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз72@" + NameUpPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.02;

                SolidWorksDocument.Extension.SelectByID2("D3@Эскиз72@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз72@" + NameUpPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.015;

                var zaklWidth = Convert.ToInt32(Math.Truncate(HeightOfWindow / 100));
                SolidWorksDocument.Extension.SelectByID2("D1@Кривая4@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая4@" + NameUpPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;

                var zaklHeight = Convert.ToInt32(Math.Truncate(WindowProfils.Height / 100));
                SolidWorksDocument.Extension.SelectByID2("D1@Кривая5@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая5@" + NameUpPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;

                SolidWorksDocument.Extension.SelectByID2("D3@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

                SolidWorksDocument.Extension.SelectByID2("D4@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;

                SolidWorksDocument.Extension.SelectByID2("D2@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                SolidWorksDocument.Extension.SelectByID2("D1@Эскиз95@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз95@" + NameDownPanel + ".Part"))).SystemValue = WindowProfils.ByWidth / 1000;

                SolidWorksDocument.Extension.SelectByID2("D1@Эскиз99@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Эскиз99@" + NameDownPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.02;

                SolidWorksDocument.Extension.SelectByID2("D3@Эскиз99@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз99@" + NameDownPanel + ".Part"))).SystemValue = (!WindowProfils.Flange30) ? 0.01 : 0.015;

                SolidWorksDocument.Extension.SelectByID2("D1@Кривая15@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая15@" + NameDownPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;

                SolidWorksDocument.Extension.SelectByID2("D1@Кривая16@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D1@Кривая16@" + NameDownPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;

                SolidWorksDocument.Extension.SelectByID2("D5@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D5@Эскиз3@02-11-03-40-.Part"))).SystemValue = HeightOfWindow / 1000;

                SolidWorksDocument.Extension.SelectByID2("D4@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D4@Эскиз3@02-11-03-40-.Part"))).SystemValue = WidthOfWindow / 1000;

                SolidWorksDocument.Extension.SelectByID2("D2@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D2@Эскиз3@02-11-03-40-.Part"))).SystemValue = WindowProfils.ByHeight / 1000;

                SolidWorksDocument.Extension.SelectByID2("D3@Эскиз3@02-11-03-40--1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                ((Dimension)(SolidWorksDocument.Parameter("D3@Эскиз3@02-11-03-40-.Part"))).SystemValue = WindowProfils.ByWidth / 1000;
                SolidWorksDocument.EditRebuild3();

                if (framelessPanel.PanelType == PanelType_e.BlankPanel)
                {
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть26@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                    solidWorksDocumentExtension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }


                switch (framelessPanel.PanelType)
                {
                    #region 04 05 - Съемные панели

                    case PanelType_e.RemovablePanel:


                        if (framelessPanel.SizePanel.X > 750)
                        {

                            solidWorksDocumentExtension.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Threaded Rivets-25@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Ручка MLA 120-3@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-5@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-6@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditDelete();

                            SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть14@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                        }
                        else
                        {
                            solidWorksDocumentExtension.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Threaded Rivets-21@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Threaded Rivets-22@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("Ручка MLA 120-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-2@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            SolidWorksDocument.EditDelete();

                            solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            solidWorksDocumentExtension.DeleteSelection2(deleteOption);
                        }

                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-60@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-61@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-2@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-1@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-2@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-3@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-4@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("1-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("3-1-1@" + NameUpPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Hole1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        SolidWorksDocument.Extension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.065;


                        solidWorksDocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                        ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = framelessPanel.PanelType == PanelType_e.RemovablePanel ? 0.044 : 0.045;


                        solidWorksDocumentExtension.SelectByID2("Hole2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Эскиз27@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Кривая1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Кривая1@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Эскиз26@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        solidWorksDocumentExtension.SelectByID2("Hole3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Эскиз31@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Кривая3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Кривая3@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Эскиз30@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Эскиз30@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        // Удаление торцевых отверстий под саморезы
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.DeleteSelection2(deleteOption);

                        // Удаление торцевых отверстий под клепальные гайки
                        SolidWorksDocument.Extension.SelectByID2("Под клепальные гайки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Эскиз49@" + NameDownPanel + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Панель1", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Панель2", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Панель3", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Эскиз32@" + "02-11-06-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Эскиз32@" + "02-11-06_2-40-" + "-1@" + AssemblyName, "SKETCH", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();


                        // Удаление отверстий под монтажную рамку
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть9@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть10@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть5@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.DeleteSelection2(deleteOption);

                        // Одна ручка
                        if (framelessPanel.SizePanel.Y < 825)
                        {
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17473_gost-12@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            solidWorksDocumentExtension.SelectByID2("SC GOST 17473_gost-13@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                            SolidWorksDocument.EditDelete();

                            SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть19@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                        }
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

                        solidWorksDocumentExtension.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditDelete();
                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-25@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Ручка MLA 120-3@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-5@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-6@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-26@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-21@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Threaded Rivets-22@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Ручка MLA 120-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-1@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17475_gost-2@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17473_gost-12@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("SC GOST 17473_gost-13@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditDelete();

                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть14@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть15@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть19@" + NameUpPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                        #endregion

                        if (framelessPanel.PanelType == PanelType_e.BlankPanel || framelessPanel.PanelType == PanelType_e.FrontPanel)
                        {
                            solidWorksDocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0, null, 0);
                            ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = 0.047;
                            SolidWorksDocument.EditRebuild3();

                            solidWorksDocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false, 0,
                                null, 0);
                            ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.067;
                            SolidWorksDocument.EditRebuild3();

                            if (framelessPanel.PanelType == PanelType_e.FrontPanel)
                            {
                                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть12@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть4@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть5@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть6@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                                solidWorksDocumentExtension.SelectByID2("Вырез-Вытянуть7@" + NameDownPanel + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, true, 0, null, 0);
                                solidWorksDocumentExtension.DeleteSelection2(deleteOption);
                            }

                            //if (типУсиливающей != null)
                            //{
                            //    solidWorksDocumentExtension.SelectByID2("D1@2-2@" + NameUpPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0, false,
                            //        0, null, 0);
                            //    ((Dimension)(SolidWorksDocument.Parameter("D1@2-2@" + NameUpPanel + ".Part"))).SystemValue = 0.03;
                            //    SolidWorksDocument.EditRebuild3();

                            //    solidWorksDocumentExtension.SelectByID2("D1@1-2@" + NameDownPanel + "-1@" + AssemblyName, "DIMENSION", 0, 0, 0,
                            //        false, 0, null, 0);
                            //    ((Dimension)(SolidWorksDocument.Parameter("D1@1-2@" + NameDownPanel + ".Part"))).SystemValue = 0.01;
                            //    SolidWorksDocument.EditRebuild3();
                            //}
                        }

                        break;
                }


            }
        //    static string ТипУсиливающей()
        //{
        //        switch (framelessPanel.PanelType)
        //        {
        //            case "24":
        //                return "EE";
        //            case "25":
        //                return "ED";
        //            case "26":
        //                return "EZ";
        //            case "27":
        //                return "TE";
        //            case "28":
        //                return "TZ";
        //            case "29":
        //                return "TD";
        //            default:
        //                return null;
        //        }
        //    }
        }
    }
}