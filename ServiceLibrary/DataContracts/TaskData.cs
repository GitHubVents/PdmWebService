using System.Runtime.Serialization;
using ServiceLibrary.TaskSystem.Constants;

namespace ServiceLibrary.DataContracts
{
    /// <summary>
    /// This model describes the information abaut task 
    /// </summary>
    [ DataContract]
   public class TaskData
    {
        /// <summary>
        /// Task id in schedule.
        /// </summary>
        [DataMember]   
        public int TaskId { get; set; }
        /// <summary>
        /// User id which the created task.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
        /// <summary>
        /// Current status task { Waiting, Execute, Completed, Error }
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string[] Designation { get; set; }

        /// <summary>
        /// Type task
        /// </summary>
        [DataMember]
        public int Type { get; set; }
    }
}
