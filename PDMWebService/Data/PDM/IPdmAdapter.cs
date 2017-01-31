using ServiceLibrary.DataContracts;
using System.Collections.Generic;

namespace PDMWebService.Data.PDM
{

    public interface IPdmAdapter
    {
      IEnumerable<DataModel> SearchDoc(string segmentName);
        void DownLoadFile(DataModel dataModel);
        string[] GetConfigigurations(DataModel dataModel);
        void GetLastVersionAsmPdm(string path);

        // need change!!!!!!!!!!!
        DataModel GetFileById(int fileId, bool isDowload);

       
    }
}
