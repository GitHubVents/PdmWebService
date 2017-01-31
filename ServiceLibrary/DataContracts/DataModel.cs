using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceLibrary.DataContracts
{

    [DataContract]
    public class DataModel  
    {
        /// <summary>
        /// The file name and expansion
        /// </summary>
        [DataMember]
        public  string FileName { get; set; }
             
        [DataMember]
        public int FolderId { get; set; }

       
        [DataMember]
        public int Id { get; set; }

 
        [DataMember]
        public  string Path { get; set; }

        [DataMember]
        public string FolderPath { get; set; }
        [DataMember]
        public int CurrentVersion { get; set; }

        ///// <summary>
        ///// Convert VentsPDM_dll.DataModel to DataSolidModel
        ///// </summary>
        ///// <param name="other"></param>
        ///// <returns></returns>
        //public static TransmittableDataModel Convert(VentsPDM_dll.DataModel other)
        //{
        //    return new TransmittableDataModel
        //    {
        //        Id = other.Id,
        //        FileName = other.FileName,
        //        FolderId = other.FolderId,
        //        Path = other.Path
        //    };
        //}
        ///// <summary>
        ///// Convert VentsPDM_dll.DataModel list to DataSolidModel list
        ///// </summary>
        ///// <param name="OtherList"></param>
        ///// <returns></returns>
        //public static IEnumerable<TransmittableDataModel> ToSolidModelList(IEnumerable<VentsPDM_dll.DataModel> OtherList)
        //{
        //    List<TransmittableDataModel> dataSolidModelList = new List<TransmittableDataModel>();

        //    foreach (var item in OtherList)
        //    {
        //        dataSolidModelList.Add(Convert(item));
        //    }
        //    return dataSolidModelList ;
        //}


        //public static IEnumerable<VentsPDM_dll.DataModel> ToDataModelList(IEnumerable<TransmittableDataModel> OtherList)
        //{
        //    List<VentsPDM_dll.DataModel> dataSolidModelList = new List<VentsPDM_dll.DataModel>();

        //    foreach (var item in OtherList)
        //    {
        //        dataSolidModelList.Add(item.ToDataModel());
        //    }
        //    return dataSolidModelList;
        //}

        //public static IEnumerable<VentsPDM_dll.DataModel> ToDataModelList(TransmittableDataModel[] OtherArray)
        //{
        //    List<VentsPDM_dll.DataModel> dataSolidModelList = new List<VentsPDM_dll.DataModel>();

        //    foreach (var item in OtherArray)
        //    {
        //        dataSolidModelList.Add(item.ToDataModel());
        //    }
        //    return dataSolidModelList;
        //}
        ///// <summary>
        ///// Returns a VentsPDM_dll.DataModel that represents the current object. 
        ///// </summary>
        ///// <returns></returns>
        //public VentsPDM_dll.DataModel ToDataModel()
        //{
        //    return new VentsPDM_dll.DataModel
        //    {
        //        FileName = this.FileName,
        //        Id = this.Id,
        //        FolderId = this.FolderId,
        //        Path = this.Path
        //    };
        //}


        public override string ToString()
        {
            StringBuilder info = new  StringBuilder();
            info.AppendLine("DataModel[ ");
            info.AppendLine("Name: " + FileName);
            info.AppendLine("Name: " + FileName);
            info.AppendLine(", Id: " + Id);
            info.AppendLine(", Folder id: " + FolderId);
            info.AppendLine(", Path: " + Path);           
            info.AppendLine("] ");
            return info.ToString();
        }
    }

}