using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary.Builders.Case;
using SolidWorksLibrary.Builders.ElementsCase;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;


namespace ConsoleApplicationTest
{
    class Program
    {

        static void Main(string[] args)
        {
            MessageObserver.Instance.ReceivedMessage += Instance_ReceivedMessage;

            
            FramelessPanel panelRemoveb = new FramelessPanel(
                        ServiceTypes.Constants.PanelType_e.RemovablePanel,
                        sizePanel: new Vector2(1200, 580), 
                        windowSize: new Vector2(0,0),
                        windowsOffset: new Vector2(0, 0),
                        thermoStrip: ServiceTypes.Constants.ThermoStrip_e.Rivet, inThick: 1, outThick: 1);


            FramelessPanel panelWithLegs = new FramelessPanel(
                        ServiceTypes.Constants.PanelType_e.НожкиОпорные,
                        sizePanel: new Vector2(1200, 580),
                        windowSize: new Vector2(0, 0),
                        windowsOffset: new Vector2(0, 0),
                        thermoStrip: ServiceTypes.Constants.ThermoStrip_e.Rivet, inThick: 1, outThick: 1);

            //верхняя
            FramelessPanel panelRoofWithoutRoof = new FramelessPanel(
                        ServiceTypes.Constants.PanelType_e.односкат,
                        sizePanel: new Vector2(1200, 580),
                        windowSize: new Vector2(0, 0),
                        windowsOffset: new Vector2(0, 0),
                        thermoStrip: ServiceTypes.Constants.ThermoStrip_e.Rivet, inThick: 1, outThick: 0.8);

            //panelRoofWithoutRoof.CalculeteValues(panelRoofWithoutRoof);
            //panelRoofWithoutRoof.BuildPanel(panelRoofWithoutRoof);

            //левая
            FramelessPanel panelDeaf = new FramelessPanel(
                        ServiceTypes.Constants.PanelType_e.BlankPanel,
                        sizePanel: new Vector2(1200, 580),
                        windowSize: new Vector2(530, 440),
                        windowsOffset: new Vector2(0, 0),
                        thermoStrip: ServiceTypes.Constants.ThermoStrip_e.Rivet, inThick: 1, outThick: 1);
            
            //panelRemoveb.CalculeteValues(panelRemoveb);
            //panelRemoveb.BuildPanel(panelRemoveb);
           

            FramelessBlock block = new FramelessBlock();

            Dictionary<FramelessPanel, PanelDestination> pannelsArray = new Dictionary<FramelessPanel, PanelDestination>();
            pannelsArray.Add(panelRemoveb, PanelDestination.Right);
            pannelsArray.Add(panelDeaf, PanelDestination.Left);
            pannelsArray.Add(panelRoofWithoutRoof, PanelDestination.Top);
            pannelsArray.Add(panelWithLegs, PanelDestination.Down);

            block.GenerateBlock(pannelsArray, null);
            //Program p = new Program();
            //p.MakeServiceSide(770, 400, 310, 500);

        }
        private static void Instance_ReceivedMessage(Patterns.Observer.MessageEventArgs massage)
        {
            Logger.Instance.ToLog($"Time:{massage.time} Message: {massage.Message}");
        }

        private void MakeServiceSide(int length, params int[] pannelCount)
        {
            FeatureManager swFeatureManager = default(FeatureManager);
            int errors = 0, warnings = 0;
            string fileName = @"D:\Test\Библиотека проектирования\DriveWorks\01 - Frameless Design 40mm\Frameless Design 40mm_new.SLDASM";

            SldWorks swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            ModelDoc2 swModel = (ModelDoc2)swApp.OpenDoc6(fileName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warnings);
            swFeatureManager = swModel.FeatureManager;

            bool res = swModel.Extension.SelectByID2("Справа", "PLANE", 0, 0, 0, false, 0, null, 0);

            int center = length / 2;
            int commonLenght = 0;

            //foreach (var eachPLenth in pannelCount)
            //{
            //    commonLenght += eachPLenth;


            //  if(commonLenght > center)

                //RefPlane plane = swFeatureManager?.InsertRefPlane(1, (length * 0.5 - eachPLenth * 0.5) / 1000, 0, 0, 0, 0);
                swModel.CreatePlaneAtOffset((length * 0.5 - pannelCount[0] * 0.5) / 1000, false);
           // }


            

            Console.WriteLine("End");

        }
    }
}