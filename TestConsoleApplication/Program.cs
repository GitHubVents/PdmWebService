//using Patterns;
using PDMWebService.Data.Solid.ElementsCase;
//using PDMWebService.Data.Solid.PartBuilders;
using SolidWorksLibrary.Builders.Dxf;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test//ConsoleApplication
{
    class Program 
    {
        static void Main(string[] args)
        {
            FramelessPanel panel = new FramelessPanel(ServiceTypes.Constants.PanelType_e.BlankPanel, new Vector2(600, 600), new Vector2(0, 0), new Vector2(0, 0), ServiceTypes.Constants.ThermoStrip_e.Rivet, null);
            FramelessPanelBuilder builder = new FramelessPanelBuilder(panel, new List<FramelessPanel>(), "00" );
            builder.Build();
                
        }

    }
}
