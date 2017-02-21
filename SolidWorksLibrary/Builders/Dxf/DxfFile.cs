using System.Text;

namespace SolidWorksLibrary.Builders.Dxf
{
    /// <summary>
    /// Describes the Dxf data model
    /// </summary>
    public  class DxfFile
    {
        /// <summary>
        /// Id pdm sorce file
        /// </summary>
        public int IdPdm { get; set; }
        /// <summary>
        /// Version sorce file
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Configuration dxf view
        /// </summary>
        public string Configuration { get; set; }

        public string FilePath { get; set; }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("DxfFile [");
            stringBuilder.Append("IdPdm: " + IdPdm);
            stringBuilder.Append(", Version: " + Version);
            stringBuilder.Append(", Configuration: " + Configuration);
            stringBuilder.Append(", FilePath: " + FilePath + " ]");
            return stringBuilder.ToString();
        }
    }
}
