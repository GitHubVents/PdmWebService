using System;
using System.Runtime.Serialization;

namespace ServiceLibrary.Models.DataContracts
{
    [DataContract]
   public class TransmittableSpecification
    {
        // from bom [t]
      //  [DataMember]
    //public    string FileName { get; set; }

        // from Nomenclature [t] 
        //[DataMember]
        //public string Nomenclature { get; set; }
        //[DataMember]
        //public int NomenclatureGroupID { get; set; }
        [DataMember]
        public string ERPCode { get; set; }
        [DataMember] 
        public string IDPDM { get; set; }
        //[DataMember]
        //public bool Deleted { get; set; }

        // from part [t]

        //[DataMember]
        //public int MaterialID { get; set; } //change on material name

        [DataMember]
        public string WorkpieceX { get; set; }

        [DataMember]

        public string WorkpieceY { get; set; }
        [DataMember]
        public string Bend { get; set; }
        [DataMember]
       public string Thickness { get; set; }
        [DataMember]
        public string Configuration { get; set; }  
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string PaintX { get; set; }
        [DataMember]
        public string PaintY { get; set; }
        [DataMember]
        public string PaintZ { get; set; }

        [DataMember]
        public string SurfaceArea { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        [DataMember]
        public string Count { get; set; } // +
 
         
        [DataMember]
        public string Partition { get; set; } // +
        /// <summary>
        /// Обозначение
        /// </summary>
        [DataMember]
        public string Designation { get; set; } // +
        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        public string Name { get; set; } // +     
    
      
        [DataMember]
        public string SummMaterial { get; set; }  
        [DataMember]
        public string Weight { get; set; }

        [DataMember]
        public string CodeMaterial { get; set; } 
         
        /// <summary>
        /// Is entry have dxf data.
        /// </summary>
        [DataMember]
        public string isDxf {get; set;}
        /// <summary>
        /// Item type {sldprt or sldasm}
        /// </summary>
        [DataMember]
        public string Type { get; set; }
    }
}
