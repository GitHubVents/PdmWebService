using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.Dxf
{
    public class DataToExport
    {
        //public decimal BoundingBoxLength { get; set; }
        // public decimal BoundingBoxWidth { get; set; }
         
        public string Configuration { get; set; }
        public byte[] DXFByte { get; set; }
        public decimal WorkpieceX { get; set; }
        public decimal WorkpieceY { get; set; }
        public int Bend { get; set; }
        public decimal Thickness { get; set; }
        public int Version { get; set; }
        public int PaintX { get; set; }
        public int PaintY { get; set; }
        public int PaintZ { get; set; }
        public int IdPdm { get; set; }
        public decimal SurfaceArea { get; set; }
        public int? MaterialID { get; set; }

    }
}
