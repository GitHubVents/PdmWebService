using System.Runtime.Serialization;

namespace  ServiceLibrary.Models.DataContracts
{
    /// <summary>
    /// Wrapper for transfer file.
    /// </summary>
    [ DataContract] 
    public class TransmittableFile  
    {
        /// <summary>
        /// The file name and expansion
        /// </summary>
        [DataMember]        
        public string FileName { get; set; }

        /// <summary>
        /// The length of the serialized object
        /// </summary>
        [DataMember]
        public long Length { get; set; }

        /// <summary>
        /// Byte array serialized object.
        /// </summary>
        [DataMember]
        public byte[] SerealizeObject{ get; set; } 

        public TransmittableFile(string FileName, long Length, byte[] SerealizeObject)
        {
            this.FileName = FileName;
            this.Length = Length;
            this.SerealizeObject = SerealizeObject;
        }
    }
}
