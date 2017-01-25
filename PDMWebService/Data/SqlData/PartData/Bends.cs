using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.SqlData.PartData
{
    public class Bends
    {
        public static void Fix(SldWorks swApp, out List<PartBendInfo> partBendInfos, bool makeFlat)
        {
            var solidWorksMacro = new SolidWorksFixPattern
            {
                SwApp = swApp ?? (SldWorks)Marshal.GetActiveObject("SldWorks.Application")
            };
            solidWorksMacro.FixFlatPattern(makeFlat);
            partBendInfos = solidWorksMacro.PartBendInfos;
        }

       
    }
}
