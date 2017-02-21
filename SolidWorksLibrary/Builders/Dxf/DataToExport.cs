using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.Dxf
{
    public class DataToExport
    {
        /// <summary>
        /// Id pdm sorce file
        /// </summary>
        public int IdPdm { get; set; }
        /// <summary>
        /// Version sorce file
        /// </summary>
        public int Version { get; set; } 

        public string ConfigurationName { get; set; }  
        public decimal SurfaceArea { get; set; }

        public decimal BoundingBoxLength { get; set; }
        public decimal BoundingBoxWidth { get; set; }
        public int Bend { get; set; }
        public decimal SheetMetalThickness { get; set; } 

        //public int? MaterialId;

        // public string FileName;

        public decimal PaintX { get; set; }
        public decimal PaintY { get; set; }
        public decimal PaintZ { get; set; }

        public byte  [] DxfByteCode { get; set; }

    }
}
