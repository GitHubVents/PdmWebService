using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.SqlData.PartData
{
    public class PartBendInfo
    {
        public string Config { get; set; }
        public string EdgeFlange { get; set; }
        public string OneBend { get; set; }
        public bool IsSupressed { get; set; }
    }
}
