using SolidWorksLibrary.Builders.ElementsCase;
using ServiceTypes.Constants;
using System.Collections.Generic;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;

namespace SolidWorksLibrary.Builders.Case
{
    public class FramelessBlock
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
        static public int SizeType { get; set; }
        static public int ThermoStrip { get; set; }
        static public Materials_e Inner { get; set; }
        static public Materials_e Outer { get; set; }


        static public int Ustanovka { get; set;}
        static public int Amplification { get; set;}
        
        


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
        


        public List<FramelessPanel> listOfPanels { get; set; }


        private FramelessBlock(List<FramelessPanel> listOfPanels)
        {
            FramelessPanelBuilder builder = null;
            foreach (var panel in listOfPanels)
            {
                //builder = new FramelessPanelBuilder(p, );
            }

            listOfPanels.Add(p);
            builder?.Build();
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

                p = new FramelessPanel((PanelType_e)11, new Vector2(XYZ.X, XYZ.Y), windowSize, windowOffset, (ThermoStrip_e)ThermoStrip, new ElementsCase.Panels.Frameless.Components.Screws() );/////////////////////////
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


        public void Build()
        {
            GetXYZ();
        }
    }
}