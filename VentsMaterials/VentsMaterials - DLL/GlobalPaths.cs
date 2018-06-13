using System;
using System.IO;

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
                //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Local\Temp");
                //return Path.GetTempPath();
                return System.IO.Path.GetTempPath();
            }
        }
    }
}
