using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.SqlData.PartData
{
    public class Dxf
    {
        public static bool ExistDxf(int idPdm, int currentVersion, string configuration, out Exception exception)
        {
            exception = null;
            var exist = false;
            string res = "";
            try
            {
                using (var sqlConnection = new SqlConnection(ExportXmlSql.ConnectionString))
                using (var command = new SqlCommand("DXFCheck", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("IDPDM", idPdm);
                    command.Parameters.AddWithValue("Configuration", configuration);
                    command.Parameters.AddWithValue("Version", currentVersion);

                    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    sqlConnection.Open();
                    command.ExecuteNonQuery();

                    var result = returnParameter.Value;
                    res = result.ToString();
                    exist = result.ToString() == "1";
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                // MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace);
                exist = false;
            }

            // MessageBox.Show(exist.ToString(), res);

            return exist;

        }

        public static void SaveByteToFile(byte[] blob, string varPathToNewLocation)
        {
            Database.SaveFile(blob, varPathToNewLocation);
        }

        public static bool Save(string partPath, string configuration, out Exception exception, bool fixBends, bool closeAfterSave, string folderToSave = null, bool includeNonSheetParts = false)
        {
            List<DxfFile> dxfList;
            return Save(partPath, folderToSave, configuration, null, out exception, 0, 0, out dxfList, fixBends, closeAfterSave, includeNonSheetParts);
        }

        public static bool Save(SldWorks swApp, out Exception exception, int idPdm, int version, out List<DxfFile> dxfList, bool fixBends, bool closeAfterSave, string configuration = null, bool includeNonSheetParts = false)
        {
            return Save(null, null, configuration, swApp, out exception, idPdm, version, out dxfList, fixBends, closeAfterSave, includeNonSheetParts);
        }

        public static bool Save(string partPath, string folderToSave, string configuration, out Exception exception, int idPdm, int version, out List<DxfFile> dxfList, bool fixBends, bool closeAfterSave, bool includeNonSheetParts)
        {
            return Save(partPath, folderToSave, configuration, null, out exception, idPdm, version, out dxfList, fixBends, closeAfterSave, includeNonSheetParts);
        }

        public static bool Save(string folderToSave, string configuration, SldWorks swApp, out Exception exception, int idPdm, int version, out List<DxfFile> dxfList, bool fixBends, bool closeAfterSave, bool includeNonSheetParts = false)
        {
            return Save(null, folderToSave, configuration, swApp, out exception, idPdm, version, out dxfList, fixBends, closeAfterSave, includeNonSheetParts);
        }

     

        static void GetDxf(int idPdm, int version, string configuration)
        {

        }

        public static string TempDxfFolder { get; set; } = @"C:\DXF\";

        static bool Save(string partPath, string folderToSave, string configuration, SldWorks swApp, out Exception exception, int idPdm, int version, out List<DxfFile> dxfList, bool fixBends, bool closeAfterSave, bool includeNonSheetParts)
        {
            var save = false;

            exception = null;
            dxfList = new List<DxfFile>();

            if (string.IsNullOrEmpty(folderToSave)) folderToSave = TempDxfFolder;

            try
            {
                if (swApp == null)
                {
                    try
                    {
                        swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                    }
                    catch (Exception)
                    {

                        swApp = new SldWorks { Visible = true };
                    }
                }

                var thisProc = Process.GetProcessesByName("SLDWORKS")[0];
                thisProc.PriorityClass = ProcessPriorityClass.RealTime;

                IModelDoc2 swModel = null;

                if (!string.IsNullOrEmpty(partPath))
                {
                    try
                    {
                        swModel = swApp.OpenDoc6(partPath, (int)swDocumentTypes_e.swDocPART,
                            (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", 0, 0);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }
                else
                {
                    swModel = swApp.IActiveDoc2;
                }

                if (swModel == null) return save;

                bool sheetmetal = true;

                if (!IsSheetMetalPart((IPartDoc)swModel))
                {
                    sheetmetal = false;
                    if (!includeNonSheetParts)
                    {
                        CloseModel(swModel, swApp);
                        return save;
                    }
                }


                if (!string.IsNullOrEmpty(configuration))
                {
                    try
                    {
                        string filePath;
                        save = SaveThisConfigDxf(folderToSave, configuration, fixBends ? swApp : null, swModel, out filePath, sheetmetal);
                        dxfList.Add(new DxfFile
                        {
                            Configuration = configuration,
                            FilePath = filePath,
                            IdPdm = idPdm,
                            Version = version
                        });
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    if (closeAfterSave) { CloseModel(swModel, swApp); }
                }
                else
                {

                    var swModelConfNames2 = (string[])swModel.GetConfigurationNames();

                    foreach (var configName in from name in swModelConfNames2
                                               let config = (Configuration)swModel.GetConfigurationByName(name)
                                               where !config.IsDerived()
                                               select name)
                    {
                        string filePath;
                        //  MessageBox.Show(swModel.GetTitle() + "\nsheetmetal-" + sheetmetal.ToString());
                        save = SaveThisConfigDxf(folderToSave, configName, fixBends ? swApp : null, swModel, out filePath, sheetmetal);
                        dxfList.Add(new DxfFile
                        {
                            Configuration = configName,
                            FilePath = filePath,
                            IdPdm = idPdm,
                            Version = version
                        });
                    }
                    if (closeAfterSave) { CloseModel(swModel, swApp); }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return save;

        }

        public static bool SaveThisConfigDxf(string folderToSave, string configuration, SldWorks swApp, IModelDoc2 swModel, out string dxfFilePath, bool sheetmetal)
        {

            swModel.ShowConfiguration2(configuration);
            swModel.EditRebuild3();

            if (swApp != null && sheetmetal)
            {
                List<PartBendInfo> list;
                Bends.Fix(swApp, out list, true);
            }

            var sDxfName = DxfName(swModel.GetTitle(), configuration) + ".dxf";

            dxfFilePath = Path.Combine(folderToSave, sDxfName);
            //  dxfFilePath = Path.Combine(@"C:\DXF", sDxfName);

            Directory.CreateDirectory(folderToSave);

            var dataAlignment = new double[12];

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

            var swPart = (IPartDoc)swModel;
            int sheetmetalOptions = SheetMetalOptions(true, sheetmetal, false, false, false, true, false);

            //MessageBox.Show(sheetmetalOptions.ToString());                               

            if (sheetmetal)
            {
                return swPart.ExportToDWG(dxfFilePath, swModel.GetPathName(), sheetmetal ? 1 : 2, true, varAlignment, false, false, sheetmetalOptions, sheetmetal ? 0 : 3);
            }
            else
            {
                return swModel.SaveAs4(dxfFilePath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
            }
        }

        static int SheetMetalOptions(bool ExportGeometry, bool IcnludeHiddenEdges, bool ExportBendLines, bool IncludeScetches, bool MergeCoplanarFaces, bool ExportLibraryFeatures, bool ExportFirmingTools)
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
        static int SheetMetalOptions(int p0, int p1, int p2, int p3, int p4, int p5, int p6)
        {
            return p0 * 1 + p1 * 2 + p2 * 4 + p3 * 8 + p4 * 16 + p5 * 32 + p6 * 64;
        }


        static void CloseModel(IModelDoc2 swModel, ISldWorks swApp)
        {
            swApp.CloseDoc(swModel.GetTitle().ToLower().Contains(".sldprt") ? swModel.GetTitle() : swModel.GetTitle() + ".sldprt");
        }

        static string GetFromCutlist(IModelDoc2 swModel, string property)
        {
            string propertyValue = null;

            try
            {
                Feature swFeat2 = swModel.FirstFeature();
                while (swFeat2 != null)
                {
                    if (swFeat2.GetTypeName2() == "SolidBodyFolder")
                    {
                        BodyFolder swBodyFolder = swFeat2.GetSpecificFeature2();
                        swFeat2.Select2(false, -1);
                        swBodyFolder.SetAutomaticCutList(true);
                        swBodyFolder.UpdateCutList();

                        Feature swSubFeat = swFeat2.GetFirstSubFeature();
                        while (swSubFeat != null)
                        {
                            if (swSubFeat.GetTypeName2() == "CutListFolder")
                            {
                                BodyFolder bodyFolder = swSubFeat.GetSpecificFeature2();
                                swSubFeat.Select2(false, -1);
                                bodyFolder.SetAutomaticCutList(true);
                                bodyFolder.UpdateCutList();
                                var swCustPrpMgr = swSubFeat.CustomPropertyManager;
                                string valOut;
                                swCustPrpMgr.Get4(property, true, out valOut, out propertyValue);
                            }
                            swSubFeat = swSubFeat.GetNextFeature();
                        }
                    }
                    swFeat2 = swFeat2.GetNextFeature();
                }
            }
            catch (Exception)
            {
                //
            }

            return propertyValue;
        }

        public static bool IsSheetMetalPart(IPartDoc swPart)
        {
            var isSheetMetal = false;
            try
            {
                var vBodies = swPart.GetBodies2((int)swBodyType_e.swSolidBody, false);

                foreach (Body2 vBody in vBodies)
                {
                    isSheetMetal = vBody.IsSheetMetal();
                    if (isSheetMetal)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return isSheetMetal;
        }

        public static string DxfName(string fileName, string config)
        {
            //return $"{fileName.ToLower().Replace(".sldprt", "").ToUpper()}-{config}";
            return $"{fileName.Replace("ВНС-", "").ToLower().Replace(".sldprt", "")}-{config}";
        }

       

        public static void AddToSql(List<DxfFile> dxfList, bool deleteFiles, out List<ResultList> resultList)
        {
            resultList = new List<ResultList>();

            foreach (var file in dxfList)
            {
                Exception ex;
                Database.AddDxf(file.FilePath, file.IdPdm, file.Configuration, file.Version, out ex);
                if (ex != null)
                {
                    resultList.Add(new ResultList { exc = ex, dxfFile = file });
                }
            }

            if (!deleteFiles) return;

            FileInfo fileInf;
            var files = new DirectoryInfo(TempDxfFolder).GetFiles();
            foreach (var fileInfo in files)
            {
                fileInf = fileInfo;
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception ex)
                {
                    resultList.Add(new ResultList { exc = ex, dxfFile = new DxfFile { FilePath = fileInf.FullName } });
                }
            }
        }

    }
}
