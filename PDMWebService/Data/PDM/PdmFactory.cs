using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.PDM
{
   public abstract class  PdmFactory
    {
        public static IPdmAdapter CreateSolidWorksPdmAdapter()
        {
            return SolidWorksPdmAdapter.Instance;
        }
        public static IPdmAdapter CreateIps()
        {
            return IpsAdapter.Instance;
        }
    }
}
