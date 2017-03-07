using ServiceConstants;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless
{
     
        public class FramelessPanel
        {
            public PanelType_e PanelType { get; set; }
            public Vector2 SizePanel { get; set; }
            public Vector2 WindowSize { get; set; }
            public Vector2 WindowsOffset { get; set; }
          public bool усиление { get; set; }
            public double innerHeight = 0;
            public double innerWeidht = 0;
            public double lenght = 0;
            public double deepInsulation = 0;
            public double widthHandle;
            public double outThickness = 1;
            public double innerThickness = 1;
            public double halfWidthPanel;
            public bool isOneHandle = false;
            public Screws Screws { get; set; }
            //public bool isDoublePanal { get; set; }
            public ThermoStrip ThermoStrip { get; set; }

            public FramelessPanel(PanelType_e panelType, Vector2 sizePanel, Vector2 windowSize, Vector2 windowsOffset, ThermoStrip thermoStrip, Screws screws)
            {
                this.PanelType = panelType;
                this.SizePanel = sizePanel;
                this.WindowSize = windowSize;
                this.WindowsOffset = windowsOffset;
                this.Screws = screws;
                ThermoStrip = thermoStrip;

            }

       
    }
}
