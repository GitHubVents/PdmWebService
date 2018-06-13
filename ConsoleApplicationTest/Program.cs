using SolidWorksLibrary.Builders.ElementsCase;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;
using System.Collections.Generic;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {

            FramelessPanel panel = new FramelessPanel
                (ServiceTypes.Constants.PanelType_e.BlankPanel,
                new Vector2(600, 600), new Vector2(0, 0), new Vector2(0, 0), ServiceTypes.Constants.ThermoStrip_e.Rivet,
                new SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components.Screws()
                                                            { ByHeight = 1, ByHeightInner = 1, ByHeightInnerUp = 1, ByWidth = 2, ByWidthInner = 2, ByWidthInnerUp = 2 });
            FramelessPanelBuilder builder = new FramelessPanelBuilder(panel, new List<FramelessPanel>(), "00");
            builder.Build2();
        }
    }
}