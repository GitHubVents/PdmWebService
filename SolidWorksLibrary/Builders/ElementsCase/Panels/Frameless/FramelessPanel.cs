using ServiceTypes.Constants;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;
using System;
using System.IO;
using System.Collections.Generic;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless
{

    public class FramelessPanel : ProductBuilderBehavior
    {
        private Vector2 windowSize;
        public PanelType_e PanelType { get; set; }
        public Vector2 SizePanel { get; set; }
        public Vector2 WindowSize
        {
            get { return windowSize; }
            set
            {
                if (value == null)
                {
                    windowSize = new Vector2(0, 0);
                }
                else
                {
                    if (value.X < SizePanel.X-2*38 && value.Y < SizePanel.Y-2*38)
                    {
                        windowSize = value;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Размер выреза превышает размер панели!\n Value: " + value.X + ",  " + value.Y + "SizePanel: " + SizePanel.X.ToString() + ", " + SizePanel.Y.ToString());
                        
                        throw new Exception("Размер выреза превышает размер панели Exception.");
                    }
                    
                }
            }
        }
        public Vector2 WindowsOffset { get; set; }
        public Screws Screws { get; set; }
        public ThermoStrip_e ThermoStrip { get; set; }
        public double InnerThicknes { get; set; }
        public double OuterThicness { get; set; }

        public bool усиление { get; set; }
        public double innerHeight = 0;
        public double innerWeidht = 0;
        public double lenght = 0;
        public double deepInsulation = 0;
        public double widthHandle;
        public double halfWidthPanel;
        public bool isOneHandle = false;


        ////////////// my props
        public string fastenersTypeOfAmplPanel;//+
        public double RebroKromka1;//+
        public double осьСаморезВинт;//+
        public double осьОтверстийСаморезВинт;//+? pType ==05
        public const double осьПоперечныеОтверстия = 10.1;//++
        public double колЗаклепокВысота;
        public double диамЗаглушкаВинт;//+
        public double диамСаморезВинт;//+
        public double zaklByHeight;//+
        public double zaklByWidth;//+
        public double d1Эскиз52;//+;
        public bool isItTortsevaya;//+
        public double колСаморезВинтВысота;//+
        public double колСаморезВинтШирина;//+

        public double шагСаморезВинтВысота;//+
        public double шагСаморезВинтШирина;//+

        public double колЗаклепокШирина;//+
        double rivetCountByHeight;
        double D2Эскиз32;
        double D1Эскиз40;
        double zaklByHeightAirHole;
        double zaklByWidthAirHole;


        //
        double flangeType = 20;


        private Vector2 panelInnerSize { get; set; }


        private bool IsMirror { get; set; }  // Determines whether it is a mirror for dual panels


        public FramelessPanel(PanelType_e panelType, Vector2 sizePanel, Vector2 windowSize, Vector2 windowsOffset, ThermoStrip_e thermoStrip, double inThick, double outThick)//добавить толщину, материал и другую муть
        {
            SetProperties(@"Проекты\Панели", @"Библиотека проектирования\DriveWorks\01 - Frameless Design 40mm");

            try
            {
                this.PanelType = panelType;
                this.SizePanel = sizePanel;
                this.WindowSize = windowSize;
                this.WindowsOffset = windowsOffset;
                this.ThermoStrip = thermoStrip;
                this.InnerThicknes = inThick;
                this.OuterThicness = outThick;
            }
            catch 
            {
                
            }
        }


        //ширина ручек
        private double GetHandleWidth(FramelessPanel panel)
        {
            if (panel.SizePanel.X < 1000)
            {
                return panel.SizePanel.X * 0.5 * 0.5;
            }
            if (panel.SizePanel.X >= 1000)
            {
                return panel.SizePanel.X * 0.45 * 0.5;
            }
            if (panel.SizePanel.X >= 1300)
            {
                return panel.SizePanel.X * 0.4 * 0.5;
            }
            if (panel.SizePanel.X >= 1700)
            {
                return panel.SizePanel.X * 0.35 * 0.5;
            }
            return panel.SizePanel.X / 4;
        }
        //количество винтов
        private int QuantityRivets(double посадочнаяШирина)
        {
            if (посадочнаяШирина < 1100)
            {
                return 4000;
            }
            if (посадочнаяШирина < 700)
            {
                return 3000;
            }
            if (посадочнаяШирина < 365)
            {
                return 2000;
            }

            return 5000;
        }
        private void DefineSomeValues(FramelessPanel panel)
        {
            //сначала вызывать screws и GetRivetsStep

            //Задаем высоту панели
            panel.RebroKromka1 = panel.ThermoStrip == ThermoStrip_e.ThermoScotch ? 17.7 : 19.2;

            const double шагЗаклепокВысота = 125;


            panel.осьСаморезВинт = 9.0;
            panel.диамСаморезВинт = 3.3;
            panel.диамЗаглушкаВинт = 13.1;
            panel.осьОтверстийСаморезВинт = 10.3;
            panelInnerSize = new Vector2(panel.SizePanel.X - 40, panel.SizePanel.Y - 40);
            panel.колЗаклепокВысота = (panel.SizePanel.Y > 1000) ? (panel.колСаморезВинтВысота + 3000) : (panel.колСаморезВинтВысота + 1000);
            panel.колСаморезВинтШирина = (Math.Truncate(panel.SizePanel.X / panel.шагСаморезВинтШирина) + 1) * 1000;


            switch (panel.PanelType)
            {
                case PanelType_e.RemovablePanel:

                    this.SizePanel.X  = this.SizePanel.X - 2;
                    this.SizePanel.Y = this.SizePanel.Y - 2;
                    panelInnerSize = new Vector2(panel.SizePanel.X - 42, panel.SizePanel.Y - 42);

                    panel.осьСаморезВинт = 9.70;
                    panel.осьОтверстийСаморезВинт = 12.0;
                    panel.диамСаморезВинт = 7.0;
                    panel.диамЗаглушкаВинт = 11.0;
                    panel.isItTortsevaya = false;
                    panel.колЗаклепокВысота = (Math.Truncate(panel.SizePanel.Y / шагЗаклепокВысота) + 1) * 1000;
                    rivetCountByHeight = (Math.Truncate(panel.SizePanel.Y / шагЗаклепокВысота) + 1) * 1000;

                    break;
                case PanelType_e.ПодТорцевую:

                    panel.isItTortsevaya = true;
                    panel.d1Эскиз52 = 35;
                    // если это 05 тип панели
                    panel.колСаморезВинтШирина = QuantityRivets(panel.SizePanel.X);
                    break;
                case PanelType_e.BlankPanel:

                    D2Эскиз32 = 77.5;
                    D1Эскиз40 = 15;
                    break;
                default:

                    D2Эскиз32 = isItTortsevaya ? 77.5 : 158.1;
                    D1Эскиз40 = isItTortsevaya ? 15 : 138.1;

                    panel.isItTortsevaya = false;
                    panel.d1Эскиз52 = (panel.fastenersTypeOfAmplPanel != null) ? 30 : 20;
                    break;
            }


            panel.колЗаклепокШирина = panel.колСаморезВинтШирина + 1000;
            panel.zaklByHeight = panel.PanelType == PanelType_e.BlankPanel ||
                           panel.PanelType == PanelType_e.FrontPanel ||
                           panel.PanelType == PanelType_e.RemovablePanel
                           ? panel.колЗаклепокВысота
                           : panel.колЗаклепокВысота + 1000;

            panel.zaklByWidth = ((panel.колЗаклепокШирина / 1000) % 2 != 0) ? (panel.колЗаклепокШирина + 1000) : panel.колЗаклепокШирина;


            if (WindowSize.X != 0)
            {
                zaklByWidthAirHole = Math.Truncate((flangeType == 30 ? WindowSize.X : WindowSize.X + 2) / 100);
                zaklByHeightAirHole = (Math.Truncate(WindowSize.Y / 100));
            }


        }
        //саморезы
        private void GetRivetsStepandScrews(FramelessPanel panel)
        {
            panel.Screws = new Screws();
            panel.шагСаморезВинтШирина = (panel.SizePanel.X < 600) ? 150 : 200;
            panel.шагСаморезВинтВысота = (panel.SizePanel.Y < 600) ? 150 : panel.шагСаморезВинтШирина;
            panel.колСаморезВинтШирина = (Math.Truncate(panel.SizePanel.Y / panel.шагСаморезВинтШирина) + 1) * 1000;


            panel.Screws.ByHeightInnerUp = 10;

            panel.колСаморезВинтВысота = (panel.Screws.ByHeightInnerUp > 1000) ? panel.Screws.ByHeightInnerUp : (Math.Truncate(panel.SizePanel.Y / panel.шагСаморезВинтВысота) + 1) * 1000;


            panel.Screws.ByWidth = (panel.PanelType == PanelType_e.BlankPanel || panel.PanelType == PanelType_e.FrontPanel)
                                ? (panel.колСаморезВинтШирина - 1000 < 2000 ? 2000 : panel.колСаморезВинтШирина - 1000)
                                : (panel.колСаморезВинтШирина < 2000 ? 2000 : panel.колСаморезВинтШирина);

            panel.Screws.ByHeight = !panel.isItTortsevaya ? panel.колСаморезВинтВысота : panel.колСаморезВинтВысота - 1000;


            ////
            panel.Screws.ByWidthInner = panel.PanelType == PanelType_e.BlankPanel || panel.PanelType == PanelType_e.FrontPanel
                  ? (колСаморезВинтШирина - 1000 < 2000 ? 2000 : колСаморезВинтШирина - 1000)
                          : (колСаморезВинтШирина < 2000 ? 2000 : колСаморезВинтШирина);

            panel.Screws.ByHeightInner = panel.PanelType == PanelType_e.RemovablePanel || panel.PanelType == PanelType_e.BlankPanel
                ? (колСаморезВинтВысота)
                : (колСаморезВинтВысота - 1000);


            /////////////////////////////////
            panel.Screws.ByWidthInnerUp = 
                        panel.PanelType == PanelType_e.RemovablePanel || 
                        panel.PanelType == PanelType_e.BlankPanel || 
                        panel.PanelType == PanelType_e.ПодТорцевую ?

                Convert.ToInt32(panel.колСаморезВинтВысота / 1000) % 2 != 0 ?

                panel.колСаморезВинтВысота + 1000 : panel.колСаморезВинтВысота : panel.колСаморезВинтВысота;

        }
        



        //типы усиливающих панелей
        private string AmplificationType(PanelType_e pType)
        {
            switch (pType)
            {
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
        private string GetFastenersTypeOfAmplPanel(string amplType)
        {
            if (!string.IsNullOrEmpty(amplType))
            {

                if (amplType.Remove(0, 1).Contains("E"))
                {
                    return "E";
                }
                if (amplType.Remove(0, 1).Contains("D"))
                {
                    return "D";
                }
                if (amplType.Remove(0, 1).Contains("E"))
                {
                    return "E";
                }
                if (amplType.Remove(0, 1).Contains("Z"))
                {
                    return "Z";
                }
                return amplType.Remove(1).Contains("T") ? "T" : "E";
            }
            else
            {
                return string.Empty;
            }
        }
        public void CalculeteValues(FramelessPanel p)
        {
            p.fastenersTypeOfAmplPanel = GetFastenersTypeOfAmplPanel(AmplificationType(p.PanelType));//влияет на d1Эскиз52
            p.widthHandle = GetHandleWidth(p);
            GetRivetsStepandScrews(p);

            DefineSomeValues(p);
        }


        public string BuildPanel(FramelessPanel p)
        {
            string outer = "02-11-01-40-_new";
            string inner = "02-11-02-40-_new";



            AssemblyName = "02-11-40-1_new";
            NewPartPath = Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            SolidWorksDocument = SolidWorksAdapter.OpenDocument(NewPartPath, swDocumentTypes_e.swDocASSEMBLY);

            UnsupressInASM(p, AssemblyName);
            UnsupressInOuter(p, outer + "-1");
            UnsupressInInner(p, inner + "-1");

            //Изменяем размер привязки для панелей
            SolidWorksDocument.Extension.SelectByID2("D1@Расстояние1@" + AssemblyName + ".SLDASM", "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(SolidWorksDocument.Parameter("D1@Расстояние1"))).SystemValue = 0;

            

            InputHolesWrapper.StringValue(p.SizePanel.X.ToString());



            #region  02-11-01-40-   OUTER
            SetBends?.Invoke((decimal)this.OuterThicness, out KFactor, out BendRadius);
            PartPrototypeName = outer;
            base.PartName = "02-outer" + LittleRandomizer();
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);


            // Габариты
            base.parameters.Add("D1@Эскиз1", p.SizePanel.X);
            base.parameters.Add("D2@Эскиз1", p.SizePanel.Y);
            base.parameters.Add("D1@3-4", p.Screws.ByHeight);//отверстия под болты справа
            base.parameters.Add("D1@1-4", p.Screws.ByHeight);//...слева
            
            base.parameters.Add("D1@Эскиз50", p.диамСаморезВинт);
            base.parameters.Add("D1@2-3-1", p.диамСаморезВинт);
            base.parameters.Add("D1@Эскиз52", p.d1Эскиз52 * -1);// расстояние от края до края панели по ширине для |.....|
            base.parameters.Add("D2@Эскиз52", FramelessPanel.осьПоперечныеОтверстия);// константа
            base.parameters.Add("D3@1-1-1", !p.isItTortsevaya || p.PanelType == PanelType_e.BlankPanel ? 35 : 158.1);
            base.parameters.Add("D2@3-1-1", !p.isItTortsevaya || p.PanelType == PanelType_e.BlankPanel ? 35 : 158.1);
            base.parameters.Add("D3@2-1-1", p.диамЗаглушкаВинт);

            base.parameters.Add("D1@Кривая3", p.Screws.ByWidthInner);//+
            base.parameters.Add("D1@Кривая6", p.Screws.ByHeightInner);//+

            base.parameters.Add("D1@Эскиз49", p.диамЗаглушкаВинт);
            base.parameters.Add("D1@Кривая1", p.zaklByWidth);
            base.parameters.Add("D1@Кривая2", p.zaklByHeight);
            base.parameters.Add("D7@Ребро-кромка1", p.RebroKromka1);
            base.parameters.Add("Толщина@Листовой металл", this.OuterThicness);
            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);


            AdditionanlParametsForOuter(base.parameters, p);
            EditPartParameters(PartPrototypeName, base.NewPartPath, 2100);
            MessageObserver.Instance.SetMessage("Finished build part " + PartPrototypeName + ". Saved with path: " + NewPartPath);

           #endregion
         
            #region Panel 02-11-02-40- INNER

            SetBends?.Invoke((decimal)this.InnerThicknes, out KFactor, out BendRadius);
            base.PartName = "02-inner" + LittleRandomizer();
            PartPrototypeName = inner;
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
            //UnsupressInInner(p, PartPrototypeName);

            //Размеры для отверстий под клепальные гайки под съемные панели
            //         base.parameters.Add("D3@2-1-1", 55.0);
            base.parameters.Add("G0@Эскиз49", OutputHolesWrapper.G0);
            base.parameters.Add("G1@Эскиз49", OutputHolesWrapper.G1);
            base.parameters.Add("G2@Эскиз49", OutputHolesWrapper.G2);
            //         base.parameters.Add("G3@Эскиз49", OutputHolesWrapper.G0);
            base.parameters.Add("L1@Эскиз49", OutputHolesWrapper.L1);
            base.parameters.Add("L2@Эскиз49", OutputHolesWrapper.L2);
            base.parameters.Add("L3@Эскиз49", OutputHolesWrapper.L3);
            base.parameters.Add("D1@Кривая10", OutputHolesWrapper.D1);
            base.parameters.Add("D1@Кривая11", OutputHolesWrapper.D2);
            base.parameters.Add("D1@Кривая12", OutputHolesWrapper.D3);



            // Кол-во отверстий под заклепки сшивочных кронштейнов
            // base.parameters.Add("D1@CrvPatternW", колЗаклепокКронштейнаДвойнойПанели);
            // base.parameters.Add( "D1@CrvPatternH",  колЗаклепокКронштейнаДвойнойПанели);
            
            base.parameters.Add("D7@Ребро-кромка1", p.RebroKromka1);
            base.parameters.Add("Толщина@Листовой металл", this.InnerThicknes);
            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);


            //Размеры промежуточных профилей
            base.parameters.Add("Wp1@Эскиз59", Math.Abs(ValProfils.Wp1) < 1 ? 10 : ValProfils.Wp1);
            base.parameters.Add("Wp2@Эскиз59", Math.Abs(ValProfils.Wp2) < 1 ? 10 : ValProfils.Wp2);
            base.parameters.Add("Wp3@Эскиз59", Math.Abs(ValProfils.Wp3) < 1 ? 10 : ValProfils.Wp3);
            base.parameters.Add("Wp4@Эскиз59", Math.Abs(ValProfils.Wp4) < 1 ? 10 : ValProfils.Wp4);

            base.parameters.Add("D1@Эскиз1", panelInnerSize.X);
            base.parameters.Add("D2@Эскиз1", panelInnerSize.Y);
            base.parameters.Add("D1@1-3", p.Screws.ByWidth);
            //base.parameters.Add("D1@Кривая6", p.Screws.ByHeight);

            //base.parameters.Add("D1@1-4", p.колСаморезВинтВысота); //нету
            //base.parameters.Add("D1@Кривая5", p.Screws.ByWidthInner);
            //base.parameters.Add("D1@Кривая4", p.Screws.ByHeightInner);
            base.parameters.Add("D2@Эскиз32", p.D2Эскиз32);
            //base.parameters.Add("D1@Эскиз47", p.widthHandle); //D4
            base.parameters.Add("D1@Эскиз38", p.диамСаморезВинт);
            base.parameters.Add("D3@1-1-1", p.диамСаморезВинт);
            base.parameters.Add("D1@Эскиз40", p.диамСаморезВинт);
            base.parameters.Add("D2@1-2", p.осьОтверстийСаморезВинт);
            base.parameters.Add("D1@2-3", p.zaklByWidth);

            base.parameters.Add("D1@Кривая2", p.zaklByHeight);
            // Для промежуточной панели отверстия
            base.parameters.Add("D1@Кривая14", rivetCountByHeight * 2);

            ///////
            //base.parameters.Add("D1@Кривая3", p.Screws.ByWidthInner);//+
            base.parameters.Add("D1@Кривая6", p.Screws.ByHeightInner);//+

            AdditionanlParametsForInner(parameters, p);
            EditPartParameters(PartPrototypeName, base.NewPartPath, 2100);
            MessageObserver.Instance.SetMessage("Finished build part " + PartPrototypeName + ". Saved with path: " + NewPartPath);
            #endregion

            #region 02-11-04-40 Скотч
            if (p.ThermoStrip == ThermoStrip_e.ThermoScotch)
            {
                PartPrototypeName = "02-11-04-40-";
                base.PartName = "02-scotch" + LittleRandomizer();
                NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, PartName);

                base.parameters.Add("D2@Эскиз1", p.SizePanel.Y - 3);
                base.parameters.Add("D1@Эскиз1", p.SizePanel.X - 3);

                EditPartParameters(PartPrototypeName, base.NewPartPath, 0);
                MessageObserver.Instance.SetMessage("Finished build part " + PartPrototypeName + ". Saved with path: " + NewPartPath);
            }
            //else
            //{
                //лента уплотнительная Pes
                PartPrototypeName = "02-11-05-40-";
                base.PartName = "02-Pes" + LittleRandomizer();
                NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, PartName);

                base.parameters.Add("D2@Эскиз1", p.SizePanel.Y - 3);
                base.parameters.Add("D1@Эскиз1", p.SizePanel.X - 3);

                EditPartParameters(PartPrototypeName, base.NewPartPath, 0);
                MessageObserver.Instance.SetMessage("Finished build part " + PartPrototypeName + ". Saved with path: " + NewPartPath);
            //}
            #endregion


            #region Мин вата
            base.PartName = "02-vata" + LittleRandomizer();
            PartPrototypeName = "02-11-03-40-_new";
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);


            
            base.parameters.Add("D1@Эскиз1", p.SizePanel.X - 1);
            base.parameters.Add("D2@Эскиз1", p.SizePanel.Y - 2);

            if (WindowSize.X != 0)
            {
                base.parameters.Add("D2@Эскиз3", p.WindowsOffset.Y);
                base.parameters.Add("D3@Эскиз3", p.WindowsOffset.X);
                base.parameters.Add("D4@Эскиз3", p.WindowSize.Y);
                base.parameters.Add("D5@Эскиз3", p.WindowSize.X);
                UnsupressInsulation(PartPrototypeName);
            }
            EditPartParameters(PartPrototypeName, base.NewPartPath, 0);
            MessageObserver.Instance.SetMessage("Finished build part " + PartPrototypeName + ". Saved with path: " + NewPartPath);
            #endregion

            if (WindowSize.X != 0)
            {
                BuildAirHoleFrame();
            }




            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            SolidWorksDocument.ForceRebuild3(false);

            base.PartName = "Panel_ASM" + LittleRandomizer();
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

            SolidWorksDocument.Extension.SaveAs(NewPartPath + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                          + (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews +
                          (int)swSaveAsOptions_e.swSaveAsOptions_OverrideSaveEmodel, null, ref errors, warnings);

            return NewPartPath + ".SLDASM";
        }

        private void UnsupressInInner(FramelessPanel p, string InnerPartName)
        {
            //SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(InnerPartName + ".SLDPRT");

            if (p.ThermoStrip == ThermoStrip_e.Rivet)
            {
                SolidWorksDocument.Extension.SelectByID2("Hole2@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("Hole3@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@" + InnerPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }

            switch(p.PanelType)
            {
                case PanelType_e.RemovablePanel:

                    SolidWorksDocument.Extension.SelectByID2("Hole1@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();

                    break;
                case PanelType_e.РамаМонтажная:

                    if (p.ThermoStrip == ThermoStrip_e.Rivet)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Hole5@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                        SolidWorksDocument.Extension.SelectByID2("Hole6@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    break;

                case PanelType_e.BlankPanel:

                    if (p.ThermoStrip == ThermoStrip_e.Rivet)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Hole5@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                        SolidWorksDocument.Extension.SelectByID2("Hole6@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    else
                    {

                    }
                break;

                case PanelType_e.НожкиОпорные:

                    if (p.ThermoStrip == ThermoStrip_e.Rivet)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Hole5@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                        SolidWorksDocument.Extension.SelectByID2("Hole6@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                   
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    break;


                    //общий случай для крыши
                case PanelType_e.безКрыши:
                case PanelType_e.односкат:
                case PanelType_e.Двухскат:
                        if (p.ThermoStrip == ThermoStrip_e.Rivet)
                        {
                            SolidWorksDocument.Extension.SelectByID2("Hole5@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                            SolidWorksDocument.Extension.SelectByID2("Hole6@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                            SolidWorksDocument.EditUnsuppress2();
                        }
                break;
            }
            if (WindowSize.X != 0)
            {
                SolidWorksDocument.Extension.SelectByID2("AirHole@" + InnerPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            }
            SolidWorksDocument.EditUnsuppress2();
        }
        private void UnsupressInOuter(FramelessPanel p, string OuterPartName)
        {
            //SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(OuterPartName + ".SLDPRT");


            if (p.ThermoStrip == ThermoStrip_e.Rivet)
            {
                
                SolidWorksDocument.Extension.SelectByID2("Отверстия заклепки по ширине@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("Отверстия заклепки по высоте@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }


            switch (p.PanelType)
            {

                case PanelType_e.BlankPanel:

                    SolidWorksDocument.Extension.SelectByID2("Hole1@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    break;
                case PanelType_e.RemovablePanel:

                    if (p.ThermoStrip == ThermoStrip_e.Rivet)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }

                    if (p.SizePanel.X > 750)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть15@" + OuterPartName +"@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    }
                    else
                    {
                        SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть14@" + OuterPartName +"@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    }
                    break;
                case PanelType_e.безКрыши:
                    //Высвечиваем четыре отверствия
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    break;
                case PanelType_e.односкат:
                    SolidWorksDocument.Extension.SelectByID2("RoofFrameHoles@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole1@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole4@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditSuppress2();

                    break;
                case PanelType_e.Двухскат:
                    SolidWorksDocument.Extension.SelectByID2("RoofFrameHoles@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);

                    break;

                case PanelType_e.РамаМонтажная:
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);

                    SolidWorksDocument.Extension.SelectByID2("2-1@" + OuterPartName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("2-1-1@" + OuterPartName, "SKETCH", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("2-3@" + OuterPartName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("2-4@" + OuterPartName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("RoofFrameHoles@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.EditUnsuppress2();

                    SolidWorksDocument.Extension.SelectByID2("Hole4" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();

                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    break;
                case PanelType_e.НожкиОпорные:
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    SolidWorksDocument.Extension.SelectByID2("2-1@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("2-1-1@" + OuterPartName + "@" + AssemblyName, "SKETCH", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditSuppress2();
                    SolidWorksDocument.Extension.SelectByID2("2-3@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); //only for outer
                    SolidWorksDocument.EditSuppress2();

                    SolidWorksDocument.Extension.SelectByID2("Ножка@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole2@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Hole3@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    break;
                

                case PanelType_e.безОпор:
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть27@" + OuterPartName + "@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
                    break;
            }

            if (WindowSize.X != 0)
            {
                SolidWorksDocument.Extension.SelectByID2("AirHole@" + OuterPartName + "@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0); SolidWorksDocument.EditUnsuppress2();
            }
            SolidWorksDocument.EditUnsuppress2();
        }
        private void UnsupressInsulation(string PartPrototypeName)
        {
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            SolidWorksDocument.Extension.SelectByID2("AirHole@" + PartPrototypeName +"-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            SolidWorksDocument.EditUnsuppress2();
        }
        private void UnsupressInASM(FramelessPanel p, string AsmName)
        {

            if (p.ThermoStrip == ThermoStrip_e.Rivet)
            {
                SolidWorksDocument.Extension.SelectByID2("Rivets", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern4", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("DerivedCrvPattern7", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
            else
            {
                SolidWorksDocument.Extension.SelectByID2("02-11-04-40--1@" + AsmName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }

            //depends on type
            switch (p.PanelType)
            {
                case PanelType_e.RemovablePanel:

                    if (p.SizePanel.X > 800)
                    {
                        SolidWorksDocument.Extension.SelectByID2("Handel-2", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                        SolidWorksDocument.Extension.SelectByID2("Handel-2.1", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    else
                    {
                        SolidWorksDocument.Extension.SelectByID2("Handel-1", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    }
                    break;

                case PanelType_e.односкат:
                    SolidWorksDocument.Extension.SelectByID2("Roof", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    break;
                case PanelType_e.Двухскат:
                    SolidWorksDocument.Extension.SelectByID2("Roof", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    break;
                case PanelType_e.НожкиОпорные:
                    SolidWorksDocument.Extension.SelectByID2("Ножки", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    break;
                case PanelType_e.РамаМонтажная:
                    SolidWorksDocument.Extension.SelectByID2("Roof", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    break;
            }

            SolidWorksDocument.Extension.SelectByID2("02-11-03-40-_new", "COMPONENT", 0, 0, 0, false, 0, null, 0);

            if (WindowSize.X != 0)
            {
                SolidWorksDocument.Extension.SelectByID2("AirHole", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("AirHoleHorizontalPattern", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
                SolidWorksDocument.Extension.SelectByID2("AirHoleVerticalPattern", "COMPPATTERN", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
            SolidWorksDocument.EditUnsuppress2();
        }

        private void AdditionanlParametsForOuter(Dictionary<string, double> parameters, FramelessPanel panel)
        {

            switch (panel.PanelType)
            {
                case PanelType_e.RemovablePanel:

                    base.parameters.Add("D1@3-1-1", panel.осьОтверстийСаморезВинт);
                    base.parameters.Add("D2@1-1-1", panel.осьОтверстийСаморезВинт);

                    base.parameters.Add("D1@2-2", 65);
                    base.parameters.Add("D1@2-4", panel.Screws.ByWidth);
                    base.parameters.Add("D4@Эскиз47", panel.widthHandle);// расстояние между ручками
                    break;

                case PanelType_e.BlankPanel:
                    base.parameters.Add("D1@3-1-1", panel.осьОтверстийСаморезВинт);
                    base.parameters.Add("D2@1-1-1", panel.осьОтверстийСаморезВинт);
                    base.parameters.Add("D2@2-2", panel.осьСаморезВинт);
                    break;
            }

            if (WindowSize?.X != 0)
            {
                parameters.Add("D1@Эскиз68", panel.WindowSize.Y);
                parameters.Add("D2@Эскиз68", panel.WindowSize.X);
                parameters.Add("D3@Эскиз68", panel.WindowsOffset.X);
                parameters.Add("D4@Эскиз68", panel.WindowsOffset.Y);

                parameters.Add("D1@Кривая4", zaklByWidthAirHole == 1 ? 2000 : zaklByWidthAirHole * 1000);
                parameters.Add("D1@Кривая5", zaklByHeightAirHole == 1 ? 2000 : zaklByHeightAirHole * 1000);


                parameters.Add("D1@Эскиз72", flangeType == 20 ? 10 : 15);//по ширине
                parameters.Add("D3@Эскиз72", flangeType == 20 ? 10 : 20);// по высотеЫ
            }
        }

        private void AdditionanlParametsForInner(Dictionary<string, double> parameters, FramelessPanel panel)
        {

            switch (panel.PanelType)
            {
                case PanelType_e.RemovablePanel:

                    base.parameters.Add("D1@1-2", 44);
                    base.parameters.Add("D3@2-1-1", 54.0);
                    base.parameters.Add("D2@Эскиз29", 84.0);
                    //base.parameters.Add("D2@Эскиз43", 12.0);
                    base.parameters.Add("D1@Эскиз29", 11.3);
                    base.parameters.Add("D1@2-1-1", 11.3);
                    base.parameters.Add("D2@Эскиз39", 11.3);
                    base.parameters.Add("D1@Эскиз39", 5.0);

                break;
                default:
                    base.parameters.Add("D1@Кривая5", panel.Screws.ByWidthInner);
                    base.parameters.Add("D1@Кривая4", panel.Screws.ByHeightInner);
                    base.parameters.Add("D3@2-1-1", 55.0);
                    base.parameters.Add("D2@Эскиз29", 85.0);
                    //  base.parameters.Add("D2@Эскиз43", 11.0); 
                    base.parameters.Add("D1@Эскиз29", 10.3);
                    base.parameters.Add("D1@2-1-1", 10.3);
                    base.parameters.Add("D2@Эскиз39", 10.3);
                    base.parameters.Add("D1@Эскиз39", 4.0);

                break;
            }
            if (WindowSize?.X != 0)
            {
                parameters.Add("D1@Кривая15", zaklByWidthAirHole == 1 ? 2000 : zaklByWidthAirHole * 1000);
                parameters.Add("D1@Кривая16", zaklByHeightAirHole == 1 ? 2000 : zaklByHeightAirHole * 1000);

                parameters.Add("D1@Эскиз95", panel.WindowsOffset.X);
                parameters.Add("D2@Эскиз95", panel.WindowsOffset.Y);
                parameters.Add("D3@Эскиз95", panel.WindowSize.X);
                parameters.Add("D4@Эскиз95", panel.WindowSize.Y);
                parameters.Add("D1@Эскиз99", flangeType == 20 ? 10 : 15);
                parameters.Add("D3@Эскиз99", flangeType == 20 ? 10 : 20);
            }
        }

        private void BuildAirHoleFrame()
        {

            base.PartName = "AirHoleL" + LittleRandomizer();
            PartPrototypeName = "02-11-11-40-";
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

            base.parameters.Add("D2@Эскиз1", WindowSize.X + 60);
            base.parameters.Add("D1@Кривая1", zaklByWidthAirHole == 1 ? 2000 : zaklByWidthAirHole * 1000);
            EditPartParameters(PartPrototypeName, NewPartPath, 0);





            base.PartName = "AirHoleH" + LittleRandomizer();
            PartPrototypeName = "02-11-12-40-";
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

            base.parameters.Add("D2@Эскиз1", WindowSize.Y);
            base.parameters.Add("D1@Кривая1", zaklByHeightAirHole == 1 ? 2000 : zaklByHeightAirHole * 1000);

            EditPartParameters(PartPrototypeName, NewPartPath, 0);
        }




        static public string  LittleRandomizer()
        {
            return new Random().Next().ToString();
        }
    }
}