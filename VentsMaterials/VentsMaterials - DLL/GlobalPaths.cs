using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VentsMaterials
{
    abstract class GlobalPaths
    {
        public static string PathToSwComplexMaterial
        {
            get
            {
                
                return Path.Combine(PathToSwComplexFolder,"vents-materials.sldmat");
            }
        }

        public static string PathToSwComplexFolder
        {
            get
            {
                
                return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "sw-complex");
            }
        }
    }
}
