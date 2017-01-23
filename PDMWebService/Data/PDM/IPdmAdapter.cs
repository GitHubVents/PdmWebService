using EPDM.Interop.epdm;
using PDM_WebService.WcfServiceLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.PDM
{

   public interface IPdmAdapter
    {
      IEnumerable<DataModel> SearchDoc(string segmentName);
        void DownLoadFile(DataModel dataModel);
        string[] GetConfigigurations(DataModel dataModel);
        void GetLastVersionAsmPdm(string path);

        // need change!!!!!!!!!!!
        IEdmFile5 GetFileById(int fileId);
    }
}
