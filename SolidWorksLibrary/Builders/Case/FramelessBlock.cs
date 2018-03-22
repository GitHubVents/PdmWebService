using SolidWorksLibrary.Builders.ElementsCase;
using ServiceTypes.Constants;
using System.Collections.Generic;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;
using System.IO;
using SolidWorks.Interop.swconst;
using System;

namespace SolidWorksLibrary.Builders.Case
{
    public class FramelessBlock : ProductBuilderBehavior
    {
        #region Propertyis
        static public int OrderNumber { get; set; }
        static public string Section { get; set; }
        static public string TradeMark { get; set; }

        /// <summary>
        /// Стандартный или не стандартный типоразмер
        /// </summary>
        static public bool Standart { get; set; }

        static public int BlockSizeX { get; set; }
        static public int BlockSizeY { get; set; }
        static public int BlockLenght { get; set; }

        /// <summary>
        /// Сторона обслуживания
        /// </summary>
        static public int Side { get; set; }

        /// <summary>
        /// Типоразмер установки
        /// </summary>
        static public int TypeSize { get; set; }
        static public int ThermoStrip { get; set; }
        static public Materials_e Inner { get; set; }
        static public int InnerThickness { get; set; }
        static public Materials_e Outer { get; set; }
        static public int OuterThickness { get; set; }


        static public int Ustanovka { get; set;}
        static public int Amplification { get; set;}
        static public int TortsevayaPanel { get; set; }
        static public int TortsevayaType { get; set; }


        //крыша
        static public bool WithRoof { get; set;}
        static public int RoofType { get; set;}
        static public int RoofDimension { get; set;}

        // крыша с вырезом
        static public int OffsetTypeX{get;set;}
        static public int OffsetTypeY { get; set; }
        static public int OffsetX { get; set; }
        static public int OffsetY { get; set; }
        /// <summary>
        /// Ширина сечения
        /// </summary>
        static public int OffsetSizeX { get; set; }
        /// <summary>
        /// Длинна сечения
        /// </summary>
        static public int OffsetSizeY { get; set; }
        /// <summary>
        /// Фланец 20/30
        /// </summary>
        static public int TypeOfCutout { get; set; }



        //опорная часть
        static public int Support { get; set; }
        static public int SupportType { get; set; }


        static public int Insulation;
        static public int PanelThickness;
        static public int TopPanel;
        static public int ButtonPanel;
        static public int BlankPanel;

        private Vector3 XYZ { get; set; }
        #endregion

        FramelessPanel p;
        private static double widthHandle;//????????????????????????????????????????????????????????????

        public List<FramelessPanel> listOfPanels { get; set; }


        private FramelessBlock(List<FramelessPanel> listOfPanels) : base()
        {
            //FramelessPanelBuilder builder;
            SetProperties(@"Проекты\Панели", @"Библиотека проектирования\DriveWorks\01 - Frameless Design 40mm");
           
            foreach (var panel in listOfPanels)
            {
                CalculeteValues(panel);
                Build2();
                //builder = new FramelessPanelBuilder(p, );
            }
        }

        private void ChechHowManyPanels()
        {
            //количество
            if (Ustanovka == 1)
            {
               // p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
                listOfPanels.Add(p);
            }
            else if (Ustanovka == 2)
            {
                //p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
                listOfPanels.Add(p);

                //p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
                listOfPanels.Add(p);
            }
            else if (Ustanovka == 3)
            {

            }

            //усиливающие
            if (Amplification == 0)
            {

            }
            else if (Amplification == 1)
            {

            }
            else if (Amplification == 2)
            {

            }
            else if (Amplification == 3)
            {

            }

            /*
            //торцевые
            if ()
            {
                
            }
            */

            #region Roof
            // крыша
            if (WithRoof == true)
            {

                Vector2 windowSize = null;
                Vector2 windowOffset = null;

                if (RoofType == (int)RoofType_e.One) //прямоугольный вырез??????????????????????
                {

                    if (OffsetTypeX != (int)OffsetType_.Center)
                    {
                        windowOffset.X = OffsetX;
                    }
                    if (OffsetTypeY != (int)OffsetType_.Center)
                    {
                        windowOffset.Y = OffsetY;
                    }
                    windowOffset = new Vector2(OffsetX, OffsetY);
                    windowSize = new Vector2(OffsetSizeX, OffsetSizeY);
                }
                if (RoofType == (int)RoofType_e.Two) //сшитая????????????????????????????????????
                {
                    // use RoofDimesion
                }

                p = new FramelessPanel((PanelType_e)11, new Vector2(XYZ.X, XYZ.Y), windowSize, windowOffset, (ThermoStrip_e)ThermoStrip, new ElementsCase.Panels.Frameless.Components.Screws() {ByHeight = 1, ByHeightInner = 1, ByHeightInnerUp = 1, ByWidth = 2, ByWidthInner = 2, ByWidthInnerUp = 2} );/////////////////////////
                listOfPanels.Add(p);
            }
            #endregion

            //опорная часть
            if (Support == (int)PanelType_e.РамаМонтажная)
            {

            }
            else if (Support == (int)PanelType_e.НожкиОпорные)
            {

            }
            else if (Support == (int)PanelType_e.безОпор)//
            {

            }


            switch (SupportType)
            {
                case (int)PanelProfile_e.Profile_3_0:
                
                    break;
                case (int)PanelProfile_e.Profile_5_0:
               
                    break;
            }
        }

        private void GetXYZ()
        {
            XYZ.X = BlockSizeX;
            XYZ.Z = BlockSizeY;
            XYZ.Y = BlockLenght;
        }

        //на клиента
        private static void GetWritedValues() //заполнить свойства из текстбоксов
        {
            
        }


        public void Build2()
        {
            AssemblyName = "02-11-40-1";
            NewPartPath = Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            SolidWorksDocument = SolidWorksAdapter.OpenDocument(NewPartPath, swDocumentTypes_e.swDocASSEMBLY);


            InputHolesWrapper.StringValue(XYZ.X.ToString());

            #region  02-11-01-40-

            PartPrototypeName = "02-11-01-40-";
            base.PartName = "02-0" + (int)p.PanelType + "-01-" + 13131313;
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);

            


            //типДвойнойРазрез


            // Габариты
            base.parameters.Add("D1@Эскиз1", p.SizePanel.X);
            base.parameters.Add("D2@Эскиз1", p.SizePanel.Y);
            base.parameters.Add("D1@3-4", p.Screws.ByHeight);//отверстия под болты справа
            base.parameters.Add("D1@1-4", p.Screws.ByHeight); //...слева
            base.parameters.Add("D1@2-4", p.Screws.ByWidth);
            base.parameters.Add("D2@2-2", осьСаморезВинт);
            base.parameters.Add("D4@Эскиз47", p.widthHandle); // расстояние между ручками
            base.parameters.Add("D1@Эскиз50", диамСаморезВинт);
            base.parameters.Add("D1@2-3-1", диамСаморезВинт);
            base.parameters.Add("D1@Эскиз52", d1Эскиз52); // расстояние от края до края панели по ширине для |.....|
            base.parameters.Add("D2@Эскиз52", осьПоперечныеОтверстия); // константа
            //         base.parameters.Add("D1@Кривая3", d1Кривая3);
            base.parameters.Add("D3@1-1-1", string.IsNullOrEmpty(типТорцевой) || p.PanelType == PanelType_e.BlankPanel ? 35 : 158.1);
            base.parameters.Add("D2@3-1-1", string.IsNullOrEmpty(типТорцевой) || p.PanelType == PanelType_e.BlankPanel ? 35 : 158.1);
            base.parameters.Add("D3@2-1-1", диамЗаглушкаВинт);
            base.parameters.Add("D1@Эскиз49", диамЗаглушкаВинт);
            base.parameters.Add("D1@Кривая1", zaklByWidth);
            base.parameters.Add("D1@Кривая2", zaklByHeight);
            base.parameters.Add("D7@Ребро-кромка1", (ThermoStrip_e)FramelessBlock.ThermoStrip == ThermoStrip_e.ThermoScotch ? 17.7 : 19.2);
            //         base.parameters.Add("Толщина@Листовой металл", materialP1[1].Replace('.', ',');
            //         base.parameters.Add("D1@CrvPatternW", колЗаклепокКронштейнаДвойнойПанели);
            //         base.parameters.Add("D1@CrvPatternH", колЗаклепокКронштейнаДвойнойПанели);
            EditPartParameters(PartPrototypeName, base.NewPartPath, 0);
            #endregion

            SolidWorksDocument.EditRebuild3();
        }

        private void GetHandleWidth(FramelessPanel panel)
        {
            panel.widthHandle = XYZ.X / 4;

            if (XYZ.X < 1000)
            {
                panel.widthHandle = XYZ.X * 0.5 * 0.5;
            }
            if (XYZ.X >= 1000)
            {
                panel.widthHandle = XYZ.X * 0.45 * 0.5;
            }
            if (XYZ.X >= 1300)
            {
                panel.widthHandle = XYZ.X * 0.4 * 0.5;
            }
            if (XYZ.X >= 1700)
            {
                panel.widthHandle = XYZ.X * 0.35 * 0.5;
            }
        }

        private void GetRivetsAndScrewsStep(FramelessPanel panel, ref double колСаморезВинтВысота, ref double колСаморезВинтШирина)
        {
            колСаморезВинтШирина = 200;
            колСаморезВинтВысота = 200;

            if (panel.SizePanel.X < 600)
            {
                колСаморезВинтШирина = 150;
            }
            if (panel.SizePanel.Y < 600)
            {
                колСаморезВинтВысота = 150;
            }
            колСаморезВинтШирина = (Math.Truncate(panel.SizePanel.X / колСаморезВинтШирина) + 1) * 1000;
            колСаморезВинтВысота = (Math.Truncate(panel.SizePanel.Y / колСаморезВинтВысота) + 1) * 1000;

            if (panel.Screws?.ByHeightInnerUp > 1000)
            {
                колСаморезВинтВысота = panel.Screws.ByHeightInnerUp;
            }
        }


        int отступОтветныхОтверстийШирина = 8;

        double осьСаморезВинт;//+
        double осьОтверстийСаморезВинт;//+? pType ==05
        const double осьПоперечныеОтверстия = 10.1;//++

        double диамЗаглушкаВинт;//+
        double диамСаморезВинт;//+
        double zaklByHeight;
        double zaklByWidth;
        double колСаморезВинтШирина2;
        double колСаморезВинтВысота2;
        string  fastenersTypeOfAmplPanel;
        double d1Эскиз52;//+; //= fastenersTypeOfAmplPanel == null ? 30 : 20);


        private void DefineSomeValues(PanelType_e pType)
        {
           

            switch (pType)
            {
                case PanelType_e.RemovablePanel:
                    осьСаморезВинт = 9.0;
                    осьОтверстийСаморезВинт = 12.0;
                    диамСаморезВинт = 7.0;
                    диамЗаглушкаВинт = 11.0;

                    break;
                case PanelType_e.ПодТорцевую:
                    d1Эскиз52 = 35;
                    осьСаморезВинт = 9.70;
                    осьОтверстийСаморезВинт = 10.3;
                    диамСаморезВинт = 3.3;
                    диамЗаглушкаВинт = 13.1;

                    break;
                default:
                    осьСаморезВинт = 9.70;
                    осьОтверстийСаморезВинт = 10.3;
                    диамСаморезВинт = 3.3;
                    диамЗаглушкаВинт = 13.1;
                    d1Эскиз52 = (fastenersTypeOfAmplPanel!=null) ? 30 : 20;

                    break;
            }
        }
        private void DefineScrews(FramelessPanel panel)
        {
            Screws screws = new Screws();


        }

        //типы усиливающих панелей
        static string AmplificationType(PanelType_e pType)
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
        private void Define(string amplType)
        {
            if (!string.IsNullOrEmpty(amplType))
            {

                fastenersTypeOfAmplPanel = amplType.Remove(1).Contains("T") ? "T" : "E";
                if (amplType.Remove(0, 1).Contains("E"))
                {
                    fastenersTypeOfAmplPanel = "E";
                }
                if (amplType.Remove(0, 1).Contains("D"))
                {
                    fastenersTypeOfAmplPanel = "D";
                }
                if (amplType.Remove(0, 1).Contains("E"))
                {
                    fastenersTypeOfAmplPanel = "E";
                }
                if (amplType.Remove(0, 1).Contains("Z"))
                {
                    fastenersTypeOfAmplPanel = "Z";
                }
            }
        }



        private void CalculeteValues(FramelessPanel p)
        {
            GetHandleWidth(p);
            DefineScrews(p);
            DefineSomeValues(p.PanelType);
            GetRivetsAndScrewsStep(p, ref колСаморезВинтВысота2, ref колСаморезВинтШирина2);
        }
    }
}