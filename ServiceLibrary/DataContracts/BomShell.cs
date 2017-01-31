using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.DataContracts
{
    /// <summary>
    ///  Bill of Materials (BOM) view.
    /// </summary>
    [DataContract]
    public class BomShell
    {
        /// <summary>
        /// Количество
        /// </summary>
        [DataMember]
        public decimal? Count { get; set; } // +
        /// <summary>
        /// Тип файла
        /// </summary>
        [DataMember]
        public string FileType { get; set; } // -
        /// <summary>
        /// Конфигурация
        /// </summary>
        [DataMember]
        public string Configuration { get; set; } // -
        /// <summary>
        /// Последняя версия
        /// </summary>
        [DataMember]
        public int? LastVesion { get; set; } // -
        /// <summary>
        /// Уровень
        /// </summary>
        [DataMember]
        public int? Level { get; set; } // -
        /// <summary>
        /// Состояние
        /// </summary>
        [DataMember]
        public string State { get; set; } // -
        /// <summary>
        /// Раздел
        /// </summary>
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
        /// <summary>
        /// Материал
        /// </summary>
        [DataMember]
        public string Material { get; set; } // -
        /// <summary>
        /// Материал Цми
        /// </summary>
        [DataMember]
        public string MaterialCmi { get; set; } // -
        /// <summary>
        /// Толщина листа
        /// </summary>
        [DataMember]
        public string SheetThickness { get; set; } //-

        [DataMember]
        public int? IdPdm { get; set; } // -
        /// <summary>
        /// Имя файла
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }
        /// <summary>
        /// Erp код
        /// </summary>
        [DataMember]
        public string ErpCode { get; set; } // +
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SummMaterial { get; set; } //+
        [DataMember]
        public string Weight { get; set; }
        [DataMember]
        public string CodeMaterial { get; set; } //+
        [DataMember]
        public string Format { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int? Position { get; set; }
        /// <summary>
        /// Количество по конфигурации
        /// </summary>
        [DataMember]
        public List<decimal> CountByConfiguration { get; set; }
        /// <summary>
        /// Конфигурация главной сборки
        /// </summary>
        [DataMember]
        public string ConfigurationMainAssembly { get; set; }
        /// <summary>
        /// Тип объекта
        /// </summary>
        [DataMember]
        public string TypeObject { get; set; }
        [DataMember]
        public string GetPathName { get; set; }
    }
}
