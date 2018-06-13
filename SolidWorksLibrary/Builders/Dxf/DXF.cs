using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using Patterns.Observer;

namespace SolidWorksLibrary.Builders.Dxf
{
    public class DXF
    {
        /// <summary>
        /// The path to the folder in which to save the built by Dxf file
        /// </summary>
        public string FolderToSaveDxf { get; set; }


        public DXF()
        {
            FolderToSaveDxf = @"C:\TEMP\dxf\";
        }
        public DXF(string folderToSaveDxf)
        {
            FolderToSaveDxf = folderToSaveDxf;
        }
        /// <summary>
        /// Convert to dxf input configuration of document 
        /// </summary>
        /// <param name="configuration">Building configuration</param>        
        /// <param name="swModel">Building document</param>
        /// <param name="dxfFilePath">Output path to file</param>
        /// <param name="isSheetmetal">Enable or disable well whether build no sheet metal parts</param>
        /// <returns></returns>
        public bool ConvertToDXF(string configuration,  IModelDoc2 swModel, out byte[] dxfByteCode,   bool isSheetmetal)
        {
            dxfByteCode = null;
            string dxfFilePath = string.Empty;
            try
            {            
                string sDxfName = DxfNameBuild(swModel.GetTitle(), configuration) + ".dxf";

                dxfFilePath = Path.Combine(FolderToSaveDxf, sDxfName);
                if (!Directory.Exists(FolderToSaveDxf))
                    Directory.CreateDirectory(FolderToSaveDxf);

                double[] dataAlignment = new double[12];

                dataAlignment[0] = 0.0;
                dataAlignment[1] = 0.0;
                dataAlignment[2] = 0.0;
                dataAlignment[3] = 1.0;
                dataAlignment[4] = 0.0;
                dataAlignment[5] = 0.0;
                dataAlignment[6] = 0.0;
                dataAlignment[7] = 1.0;
                dataAlignment[8] = 0.0;
                dataAlignment[9] = 0.0;
                dataAlignment[10] = 0.0;
                dataAlignment[11] = 1.0;
                object varAlignment = dataAlignment;

                IPartDoc swPart = (IPartDoc)swModel;

                int sheetmetalOptions = SheetMetalOptions(true, false, false, false, false, true, false);
                bool isExportToDWG2 = swPart.ExportToDWG2(dxfFilePath, swModel.GetPathName(), isSheetmetal ? (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal : (int)swExportToDWG_e.swExportToDWG_ExportSelectedFacesOrLoops,
                    true, varAlignment, false, false, sheetmetalOptions, isSheetmetal ? 0 : (int)swExportToDWG_e.swExportToDWG_ExportAnnotationViews);

                MessageObserver.Instance.SetMessage("\tCompleted building " + " with configuration \"" + configuration + "\"", MessageType.System);

                if (isExportToDWG2)
                {
                    dxfByteCode = DxfToByteCode(dxfFilePath);
                }
                             
                return isExportToDWG2;
            }
            catch (Exception exception)
            {
                string message = "\tFailed build dxf  " + swModel?.GetTitle() + " with configuration \"" + configuration + "\"" + exception.ToString();
                MessageObserver.Instance.SetMessage(message, MessageType.Error);
                return false;
            }
        }

        /// <summary>
        ///  Combinate fileName and config for new name dxf file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string DxfNameBuild(string fileName, string config)
        {
            string tempConf = config;

            string[] restricted = new string[] { ">", "<", "\"", "|"};


            if (tempConf.Contains("<") && tempConf.Contains(">"))
            {

                int firstBraket = tempConf.IndexOf("<") + 1;
                int secondBraket = tempConf.IndexOf(">");

                string subSTR = tempConf.Substring(firstBraket - 1, secondBraket - firstBraket + 2);
                tempConf = tempConf.Replace(subSTR, "");
            }


           

            foreach (string symbol in restricted)
            {
                if (tempConf.Contains(symbol))
                {
                    tempConf = tempConf.Replace(symbol, "");
                }
            }

            return $"{fileName.Replace("ВНС-", "").ToLower().Replace(".sldprt", "")}-{tempConf}";
        }


        private byte [] DxfToByteCode (string path)
        {
            byte[] dxfByteCode = null;
             
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    dxfByteCode = reader.ReadBytes((int)stream.Length);
                }
            }
            
            return dxfByteCode;
        }


        /// <summary>
        ///  Compute sheet metal options
        /// </summary>
        /// <param name="ExportGeometry"></param>
        /// <param name="IcnludeHiddenEdges"></param>
        /// <param name="ExportBendLines"></param>
        /// <param name="IncludeScetches"></param>
        /// <param name="MergeCoplanarFaces"></param>
        /// <param name="ExportLibraryFeatures"></param>
        /// <param name="ExportFirmingTools"></param>
        /// <returns></returns>
        private int SheetMetalOptions(bool ExportGeometry, bool IcnludeHiddenEdges, bool ExportBendLines, bool IncludeScetches, bool MergeCoplanarFaces, bool ExportLibraryFeatures, bool ExportFirmingTools)
        {
            return SheetMetalOptions(
                ExportGeometry ? 1 : 0,
                IcnludeHiddenEdges ? 1 : 0,
                ExportBendLines ? 1 : 0,
                IncludeScetches ? 1 : 0,
                MergeCoplanarFaces ? 1 : 0,
                ExportLibraryFeatures ? 1 : 0,
                ExportFirmingTools ? 1 : 0);
        }
        /// <summary>
        /// Compute sheet metal options
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="p5"></param>
        /// <param name="p6"></param>
        /// <returns></returns>
        private int SheetMetalOptions(int p0, int p1, int p2, int p3, int p4, int p5, int p6)
        {
            return p0 * 1 + p1 * 2 + p2 * 4 + p3 * 8 + p4 * 16 + p5 * 32 + p6 * 64;
        }
    }
}
