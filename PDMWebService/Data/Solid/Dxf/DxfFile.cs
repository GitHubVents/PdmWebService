using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.Solid.Dxf
{
  public  class DxfFile
    {
        public int IdPdm { get; set; }
        public int Version { get; set; }
        public string Configuration { get; set; }
        public string FilePath { get; set; }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("DxfFile [");
            stringBuilder.Append("IdPdm: " + IdPdm);
            stringBuilder.Append(", Version: " + Version);
            stringBuilder.Append(", Configuration: " + Configuration);
            stringBuilder.Append(", FilePath: " + FilePath + " ]");
            return stringBuilder.ToString();
        }
    }
}
