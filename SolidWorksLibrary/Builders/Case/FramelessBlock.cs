using SolidWorksLibrary.Builders.ElementsCase;
using System.Collections.Generic;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;
using SolidWorks.Interop.swconst;
using Patterns.Observer;
using ServiceTypes.Constants;
using System.IO;
using SolidWorks.Interop.sldworks;
using SolidWorksLibrary.Builders.ElementsCase.Panels;

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
        static public int BlockSizeZ { get; set; }

        /// <summary>
        /// Сторона обслуживания
        /// </summary>
        static public int Side { get; set; }

        /// <summary>
        /// Типоразмер установки
        /// </summary>
        static public string TypeSize { get; set; }
        static public ThermoStrip_e ThermoStrip { get; set; }
        static public int MaterialInner { get; set; }
        static public double InnerThickness { get; set; }
        static public int MaterialOuter { get; set; }
        static public double OuterThickness { get; set; }


        static public int Ustanovka { get; set; }
        static public int Amplification { get; set; }
        static public bool FrontPanel { get; set; }
        static public bool BackPanel { get; set; }



        //крыша
        static public bool WithRoof { get; set; }
        static public int RoofType { get; set; }
        static public int RoofDimension { get; set; }

        // крыша с вырезом
        static public int OffsetTypeX { get; set; }
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
        #endregion



        private FramelessPanel p;
        public static Dictionary<FramelessPanel, PanelDestination> DictionaryOfPanels;

        Dictionary<string, PanelDestination> pathesAndIndexex = new Dictionary<string, PanelDestination>();



        public FramelessBlock() : base()
        {
            DictionaryOfPanels = new Dictionary<FramelessPanel, PanelDestination>();
            SetProperties(@"Проекты\Панели", @"Библиотека проектирования\DriveWorks\01 - Frameless Design 40mm");
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder);
        }



        public void GenerateBlock(Dictionary<FramelessPanel, PanelDestination> listOfPanels, List<ServiceSide> serviceSidePanels)
        {

            string pathToBuildedPanel;
            string modelPartName;
            string blockName = @"Frameless Design 40mm_new";
            string blockPath = Path.Combine(RootFolder, SourceFolder, blockName + @".SLDASM");

            foreach (var panel in listOfPanels)
            {
                panel.Key.CalculeteValues(panel.Key);
                pathToBuildedPanel = panel.Key.BuildPanel(panel.Key);
                pathesAndIndexex.Add(pathToBuildedPanel, panel.Value);
            }
            MessageObserver.Instance.SetMessage("Panels were build");

            SolidWorksDocument = SolidWorksAdapter.OpenDocument(blockPath, swDocumentTypes_e.swDocASSEMBLY);
            UnsupressInBLOCK(blockName);


            foreach (KeyValuePair<string, PanelDestination> item in pathesAndIndexex)
            {
                GetPartName(out modelPartName, item.Value);

                SolidWorksDocument.Extension.SelectByID2(modelPartName + "@" + blockName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(item.Key, "", false, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(modelPartName + ".SLDPRT");
            }

            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(blockName + ".SLDASM");
            System.Windows.Forms.MessageBox.Show("Get ready for making dimensions");
            
            //ADDITIONAL PARAMETERS FOR BLOCK
            bool res = SolidWorksDocument.Extension.SelectByID2("D1@LF@" + blockName, "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(SolidWorksDocument.Parameter("D1@LF"))).SystemValue = FramelessBlock.BlockSizeX/2;
            res = SolidWorksDocument.Extension.SelectByID2("D1@LB@" + blockName, "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(SolidWorksDocument.Parameter("D1@LB"))).SystemValue = FramelessBlock.BlockSizeX / 2000;

                




            res = SolidWorksDocument.Extension.SelectByID2("D1@HD@" + blockName, "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(SolidWorksDocument.Parameter("D1@HD"))).SystemValue = FramelessBlock.BlockSizeY / 2;
            res = SolidWorksDocument.Extension.SelectByID2("D1@HU@" + blockName, "DIMENSION", 0, 0, 0, true, 0, null, 0);
            ((Dimension)(SolidWorksDocument.Parameter("D1@HU"))).SystemValue = FramelessBlock.BlockSizeY / 2;
           

            //SAVING BLOCK
            SolidWorksDocument.Extension.SaveAs(base.NewPartPath + @"\BLOCK-" + FramelessPanel.LittleRandomizer() + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                              + (int)swSaveAsOptions_e.swSaveAsOptions_Copy, null, ref errors, warnings);
        }
        private void GetPartName(out string partName, PanelDestination ds)
        {
            partName = string.Empty;
            switch (ds)
            {
                case PanelDestination.Top:
                    partName = @"02-11-40-1_new-17";
                    break;
                case PanelDestination.Down:
                    partName = @"02-11-40-1_new-16";
                    break;
                case PanelDestination.Left:
                    partName = @"02-11-40-1_new-23";
                    break;
                case PanelDestination.Right:
                    partName = @"02-11-40-1_new-21";
                    break;
                case PanelDestination.Front:
                    partName = @"02-11-40-1_new-15";
                    break;
                case PanelDestination.Back:
                    partName = @"02-11-40-1_new-14";
                    break;

            }
        }

        //private Dictionary<FramelessPanel, PanelDestination> ChechHowManyPanels()
        //{
        //    //количество
        //    if (Ustanovka == 1)
        //    {
        //        //p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
        //        DictionaryOfPanels.Add(p, PanelDestination.Right);

        //    }
        //    else if (Ustanovka == 2)
        //    {
        //        //p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
        //        DictionaryOfPanels.Add(p, PanelDestination.Right);

        //        //p = new FramelessPanel(, new Vector2(XYZ.X, XYZ.Y), null, null, ThermoStrip);
        //        DictionaryOfPanels.Add(p, PanelDestination.Right);
        //    }
        //    else if (Ustanovka == 3)
        //    {

        //    }

        //    //усиливающие
        //    if (Amplification == 0)
        //    {

        //    }
        //    else if (Amplification == 1)
        //    {

        //    }
        //    else if (Amplification == 2)
        //    {

        //    }
        //    else if (Amplification == 3)
        //    {

        //    }

        //    /*
        //    //торцевые
        //    if ()
        //    {

        //    }
        //    */

        //    #region Roof
        //    // крыша
        //    if (WithRoof == true)
        //    {

        //        Vector2 windowSize = null;
        //        Vector2 windowOffset = null;

        //        if (RoofType == (int)RoofType_e.One) //прямоугольный вырез??????????????????????
        //        {

        //            if (OffsetTypeX != (int)OffsetType_.Center)
        //            {
        //                windowOffset.X = OffsetX;
        //            }
        //            if (OffsetTypeY != (int)OffsetType_.Center)
        //            {
        //                windowOffset.Y = OffsetY;
        //            }
        //            windowOffset = new Vector2(OffsetX, OffsetY);
        //            windowSize = new Vector2(OffsetSizeX, OffsetSizeY);
        //        }
        //        if (RoofType == (int)RoofType_e.Two) //сшитая????????????????????????????????????
        //        {
        //            // use RoofDimesion
        //        }

        //        //p = new FramelessPanel((PanelType_e)11, new Vector2(XYZ.X, XYZ.Y), windowSize, windowOffset, (ThermoStrip_e)ThermoStrip, new ElementsCase.Panels.Frameless.Components.Screws() {ByHeight = 1, ByHeightInner = 1, ByHeightInnerUp = 1, ByWidth = 2, ByWidthInner = 2, ByWidthInnerUp = 2} );/////////////////////////
        //        DictionaryOfPanels.Add(p, PanelDestination.Top);
        //    }
        //    #endregion

        //    //опорная часть
        //    if (Support == (int)PanelType_e.РамаМонтажная)
        //    {

        //    }
        //    else if (Support == (int)PanelType_e.НожкиОпорные)
        //    {

        //    }
        //    else if (Support == (int)PanelType_e.безОпор)//
        //    {

        //    }


        //    switch (SupportType)
        //    {
        //        case (int)PanelProfile_e.Profile_3_0:

        //            break;
        //        case (int)PanelProfile_e.Profile_5_0:

        //            break;
        //    }


        //    return DictionaryOfPanels;

        //}




        
        private void UnsupressInBLOCK(string blockName)
        {
            if (SupportType == (int)PanelType_e.НожкиОпорные)
            {
                SolidWorksDocument.Extension.SelectByID2("FOOTS@" + blockName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
            if (FramelessBlock.FrontPanel)
            {
                SolidWorksDocument.Extension.SelectByID2("Панель торцевая входа@" + blockName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
            if (FramelessBlock.BackPanel)
            {
                SolidWorksDocument.Extension.SelectByID2("Панель торцевая выхода@" + blockName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();
            }
        }
    }
    public enum PanelDestination
    {
        Top,
        Down,
        Left,
        Right,
        Front,
        Back
    }
}