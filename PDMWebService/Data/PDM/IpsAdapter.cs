using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPDM.Interop.epdm;
using PDM_WebService.WcfServiceLibrary.DataContracts;

namespace PDMWebService.Data.PDM
{
  public  class IpsAdapter : Singleton.AbstractSingeton<IpsAdapter>, IPdmAdapter
    {

        private IpsAdapter() :base()
        {

        }
        public void DownLoadFile(DataModel dataModel)
        {
            throw new NotImplementedException();
        }

        public string[] GetConfigigurations(DataModel dataModel)
        {
            throw new NotImplementedException();
        }

        public DataModel GetFileById(int fileId, bool isDowload)
        {
            throw new NotImplementedException();
        }

        public void GetLastVersionAsmPdm(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataModel> SearchDoc(string segmentName)
        {
            throw new NotImplementedException();
        }
    }
}
