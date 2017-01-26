using System.Runtime.Serialization;
using ServiceLibrary.TaskSystem.Constants;

namespace ServiceLibrary.DataContracts
{
    [ DataContract]
   public class TaskData
    {
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public int User { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string[] Designation { get; set; }

        [DataMember]
        public int type { get; set; }
    }
}
