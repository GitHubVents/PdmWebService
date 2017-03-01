using ServiceConstants;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels
{

    public class FramelessPanel
    {
        public PanelType PanelType { get; set; }
        public Vector2 SizePanel { get; set; }
        public Vector2 WindowSize { get; set; }
        public Vector2 WindowsOffset { get; set; }

        public double innerHeight = 0;
        public double innerWeidht = 0;
        public double lenght = 0;
        public double deepInsulation = 0;
        public double widthHandle;
        public double outThickness;
        public double innerThickness;
        public double halfWidthPanel;
        public bool isOneHandle = false;

        public FramelessPanel(PanelType panelType, Vector2 sizePanel, Vector2 windowSize, Vector2 windowsOffset, ThermoStrip thermoStrip)
        {
            this.PanelType = panelType;
            this.SizePanel = sizePanel;
            this.WindowSize = windowSize;
            this.WindowsOffset = windowsOffset;
        }

    }

    public class FramelessPanelBuilder : ProductBuilderBehavior
    {
        private FramelessPanel framelessPanel;
        private List<FramelessPanel> framelessPanelList;
        public override event SetBendsHandler SetBends;
        public FramelessPanelBuilder(FramelessPanel framelessPanel, List<FramelessPanel> framelessPanelList) : base()
        {
            this.framelessPanel = framelessPanel;
            this.framelessPanelList = framelessPanelList;
            SetProperties("panel", "01 - Frameless Design 40mm");
        }


        public void Buid()
        {
            AssemblyName = "02-11-40-1";
            NewPartPath = System.IO.Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            SolidWorksAdapter.OpenDocument(NewPartPath, SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
        }



        private void SetSize()
        {
            if (CheckExistPart != null)
                CheckExistPart(base.PartName, out IsPartExist, out NewPartPath);
            PartName = "02-11-01-40-";
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2(PartName +"@02-11-01-40-1", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
            }
            else
            {

                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName); 
                if (SetBends != null)
                    SetBends((decimal)framelessPanel.outThickness, out KFactor, out BendRadius);

                Vector2 dimensions;
                if (framelessPanel.PanelType.Equals(PanelType.RemovablePanel))
                {
                    dimensions = new Vector2(framelessPanel.SizePanel.X - 42, framelessPanel.SizePanel.Y);

                }
                else
                {
                    dimensions = new Vector2(framelessPanel.SizePanel.X, framelessPanel.SizePanel.Y);
                }

                base.parameters.Add("D1@Эскиз1", dimensions.Y);
                base.parameters.Add("D2@Эскиз1", dimensions.X);
      
          

                base.parameters.Add("D7@Ребро-кромка1", framelessPanel.lenght);
                base.parameters.Add("Толщина@Листовой металл", framelessPanel.outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                if (framelessPanel.PanelType == PanelType.RemovablePanel && !framelessPanel.isOneHandle)
                {
                    base.parameters.Add("D4@Эскиз30", framelessPanel.widthHandle);
                }
                EditPartParameters(PartName, base.NewPartPath);

            }
        } 

        protected override void DeleteComponents(int type)
        {
            const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;


          


            switch (framelessPanel.PanelType)
            {

                    case PanelType.RemovablePanel:
                    


                    //    SolidWorksDocument.Extension.SelectByID2("Рамка", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.EditDelete();

                    //    foreach (var component in new List<string>
                    //{
                    //    "02-11-11-40--1", "02-11-11-40--2", "02-11-11-40--3","02-11-11-40--4",
                    //    "Threaded Rivets Increased-1", "Threaded Rivets Increased-2", "Threaded Rivets Increased-3", "Threaded Rivets Increased-4",
                    //    "Rivet Bralo-71", "Rivet Bralo-72", "Rivet Bralo-73", "Rivet Bralo-74", "Rivet Bralo-75", "Rivet Bralo-76",
                    //    "Rivet Bralo-83", "Rivet Bralo-84", "Rivet Bralo-91", "Rivet Bralo-92", "Rivet Bralo-93", "Rivet Bralo-94"
                    //})
                    //    {
                    //        SolidWorksDocument.Extension.SelectByID2(component + "@" + AssemblyName, "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    //        SolidWorksDocument.EditDelete();
                    //    }

                    //    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть21@" + "" /*nameUpPanel*/ + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                    //    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть22@" + "" /* nameDownPanel*/  + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                    //    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть1@02-11-03-40--1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                    //    SolidWorksDocument.Extension.SelectByID2("Рамка@" + "" /*nameUpPanel*/ + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.EditDelete();
                    //    SolidWorksDocument.Extension.SelectByID2("Рамка@" + ""/*nameUpPanel*/ + "-1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.EditDelete();
                    //    SolidWorksDocument.Extension.SelectByID2("Рамка@02-11-03-40--1@" + AssemblyName, "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.EditDelete();


                    //    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть26@" + ""/*nameUpPanel*/ + "-1@" + AssemblyName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                    //    SolidWorksDocument.Extension.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    //    SolidWorksDocument.EditDelete();
                    //    SolidWorksDocument.EditRebuild3();
                    break;

            }

            // else
            //{
            //    swDocExt.SelectByID2("Threaded Rivets-38@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

            //    if (config.Contains("01"))
            //    {
            //        swDocExt.SelectByID2("Вырез-Вытянуть25@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
            //        swDoc.EditRebuild3();
            //    }
            //    else if (config.Contains("02"))
            //    {
            //        swDocExt.SelectByID2("Вырез-Вытянуть25@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
            //        swDocExt.SelectByID2("Кривая3@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
            //        swDocExt.SelectByID2("Вырез-Вытянуть18@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditSuppress2();
            //    }

            //    swDocExt.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
            //    swDoc.Extension.SelectByID2("Вырез-Вытянуть26@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();
            //    swDoc.Extension.SelectByID2("Кривая6@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0); swDoc.EditUnsuppress2();


            //    swDoc.Extension.SelectByID2("D1@Кривая6@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Кривая6@" + nameUpPanel + ".Part"))).SystemValue = колСаморезВинтВысота / 1000; swDoc.EditRebuild3();

            //    swDoc.Extension.SelectByID2("D2@Эскиз74@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D2@Эскиз74@" + nameUpPanel + ".Part"))).SystemValue = 0.015;
            //    swDoc.EditRebuild3();

            //    swDocExt.SelectByID2("Threaded Rivets-37@" + nameAsm, "COMPONENT", 0, 0, 0, false, 0, null, 0); swDoc.EditDelete();

            //    swDoc.Extension.SelectByID2("D2@Эскиз68@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D2@Эскиз68@" + nameUpPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

            //    swDoc.Extension.SelectByID2("D1@Эскиз68@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Эскиз68@" + nameUpPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;


            //    swDoc.Extension.SelectByID2("D3@Эскиз68@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D3@Эскиз68@" + nameUpPanel + ".Part"))).SystemValue = BackProfils.ByWidth / 1000;




            //    //var additionToWindow = BackProfils.Flange30 ?
            //    ////    BackProfils.ByHeight / 1000 :  (BackProfils.ByHeight + 2) / 1000; 


            //    swDoc.Extension.SelectByID2("D4@Эскиз68@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D4@Эскиз68@" + nameUpPanel + ".Part"))).SystemValue = BackProfils.ByHeight / 1000;

            //    swDoc.Extension.SelectByID2("D1@Эскиз72@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Эскиз72@" + nameUpPanel + ".Part"))).SystemValue = (!BackProfils.Flange30) ? 0.01 : 0.02;
            //    swDoc.EditRebuild3();

            //    swDoc.Extension.SelectByID2("D3@Эскиз72@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D3@Эскиз72@" + nameUpPanel + ".Part"))).SystemValue = (!BackProfils.Flange30) ? 0.01 : 0.015;
            //    swDoc.EditRebuild3();

            //    var zaklWidth = Convert.ToInt32(Math.Truncate(HeightOfWindow / 100));
            //    swDoc.Extension.SelectByID2("D1@Кривая4@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Кривая4@" + nameUpPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;


            //    var zaklHeight = Convert.ToInt32(Math.Truncate(BackProfils.Height / 100));
            //    swDoc.Extension.SelectByID2("D1@Кривая5@" + nameUpPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Кривая5@" + nameUpPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;
            //    swDoc.EditRebuild3();


            //    swDoc.Extension.SelectByID2("D3@Эскиз95@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D3@Эскиз95@" + nameDownPanel + ".Part"))).SystemValue = HeightOfWindow / 1000;

            //    swDoc.Extension.SelectByID2("D4@Эскиз95@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D4@Эскиз95@" + nameDownPanel + ".Part"))).SystemValue = WidthOfWindow / 1000;

            //    swDoc.Extension.SelectByID2("D2@Эскиз95@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D2@Эскиз95@" + nameDownPanel + ".Part"))).SystemValue = BackProfils.ByHeight / 1000;

            //    swDoc.Extension.SelectByID2("D1@Эскиз95@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Эскиз95@" + nameDownPanel + ".Part"))).SystemValue = BackProfils.ByWidth / 1000;

            //    swDoc.Extension.SelectByID2("D1@Эскиз99@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Эскиз99@" + nameDownPanel + ".Part"))).SystemValue = (!BackProfils.Flange30) ? 0.01 : 0.02;
            //    swDoc.EditRebuild3();
            //    swDoc.Extension.SelectByID2("D3@Эскиз99@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D3@Эскиз99@" + nameDownPanel + ".Part"))).SystemValue = (!BackProfils.Flange30) ? 0.01 : 0.015;
            //    swDoc.EditRebuild3();


            //    swDoc.Extension.SelectByID2("D1@Кривая15@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Кривая15@" + nameDownPanel + ".Part"))).SystemValue = zaklWidth == 1 ? 2 : zaklWidth;

            //    swDoc.Extension.SelectByID2("D1@Кривая16@" + nameDownPanel + "-1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D1@Кривая16@" + nameDownPanel + ".Part"))).SystemValue = zaklHeight == 1 ? 2 : zaklHeight;
            //    swDoc.EditRebuild3();


            //    swDoc.Extension.SelectByID2("D5@Эскиз3@02-11-03-40--1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D5@Эскиз3@02-11-03-40-.Part"))).SystemValue = HeightOfWindow / 1000;

            //    swDoc.Extension.SelectByID2("D4@Эскиз3@02-11-03-40--1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D4@Эскиз3@02-11-03-40-.Part"))).SystemValue = WidthOfWindow / 1000;

            //    swDoc.Extension.SelectByID2("D2@Эскиз3@02-11-03-40--1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D2@Эскиз3@02-11-03-40-.Part"))).SystemValue = BackProfils.ByHeight / 1000;

            //    swDoc.Extension.SelectByID2("D3@Эскиз3@02-11-03-40--1@" + nameAsm, "DIMENSION", 0, 0, 0, false, 0, null, 0);
            //    ((Dimension)(swDoc.Parameter("D3@Эскиз3@02-11-03-40-.Part"))).SystemValue = BackProfils.ByWidth / 1000;
            //    swDoc.EditRebuild3();

            //    if (pType == "01")
            //    {
            //        swDoc.Extension.SelectByID2("Вырез-Вытянуть26@" + nameUpPanel + "-1@" + nameAsm, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            //        swDoc.Extension.DeleteSelection2(deleteOption);
            //        swDocExt.SelectByID2("Hole72", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
            //        swDoc.EditDelete();
            //    }


            }

    }
}
