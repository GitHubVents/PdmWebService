using System;

using System.Collections.Generic;
using System.IO;
using PDM_WebService.WcfServiceLibrary.DataContracts;
using System.Data;
using System.Linq;

using PDMWebService.Singleton;
using PDMWebService.Data.SqlData;
using EPDM.Interop.epdm;
using System.Threading;

namespace PDMWebService.Data.PDM
{
    public class PDMAdapter : AbstractSingeton<PDMAdapter>
    {

        private PDMAdapter():base()
        {
            PDMInitialize();
            try
            {
                this.sqlAdapter = SqlDataAdapder.Instance;
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
        /// <summary>
        /// PDM exemplar.
        /// </summary>
        private IEdmVault5 edmVault5 = null; // if   use insnace - apdater creater edmVault5

        /// <summary>
        /// Vents pdm name.
        /// </summary>
        private string vaultname { get; set; } = "Vents-PDM";

        private SqlDataAdapder sqlAdapter { get; set; }

      
        /// <summary>
        /// Search document by name.
        /// </summary>
        /// <param name="segmentName"></param>
        /// <returns></returns>
        public IEnumerable<DataModel> SearchDoc(string segmentName )
        {
            this.PDMInitialize();
            List<DataModel> searchResult = new List<DataModel>();
            try
            {
                var Search = (edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_Search);
                Search.FileName = segmentName ;
                Search.SetToken(EdmSearchToken.Edmstok_FindFolders, false);
                int count = 0;

                IEdmSearchResult5 Result = Search.GetFirstResult();
                while (Result != null)
                {

                    searchResult.Add(new DataModel
                    {
                        FileName = Result.Name,
                        Id = Result.ID,
                        FolderId = Result.ParentFolderID,
                        Path = Result.Path
                    });



                    Result = Search.GetNextResult();
                    count++;
                }
                Logger.ToLog("По запросу " + segmentName + " найдено " + count);
            }
            catch (Exception exception)
            {
                Logger.ToLog("По запросу " + segmentName + " не найдено ни одного файла\n Ошибка: " + exception);

            }
            return searchResult;
        }


        /// <summary>
        /// Download file in to local directory witch has fixed path
        /// </summary>
        /// <param name="dataModel"></param>
        public void DownLoadFile(DataModel dataModel)
        {
            this.PDMInitialize();
            try
            {
                var batchGetter = (IEdmBatchGet)(edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
                batchGetter.AddSelectionEx((EdmVault5)edmVault5, dataModel.Id, dataModel.FolderId, 0);
                if ((batchGetter != null))
                {
                    batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
                    batchGetter.GetFiles(0, null);
                }

                Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скачан с PDM системы по пути " + dataModel.Path);
            }
            catch (Exception exception)
            {
                Logger.ToLog("Ошибка при скачивании файла " + dataModel.FileName + " с id " + dataModel.Id + exception.ToString());

            }
        }


        /// <summary>
        ///  Get configuration by data model
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public string[] GetConfigigurations(DataModel dataModel)
        {
            IEdmFile9 fileModelInf;
            IEdmFolder5 ppoRetParentFolder;
            fileModelInf = (IEdmFile9)edmVault5.GetFileFromPath(dataModel.Path, out ppoRetParentFolder);

            EdmStrLst5 cfgList;
            cfgList = fileModelInf.GetConfigurations();

            IEdmPos5 pos;
            pos = cfgList.GetHeadPosition();

            List<string> cfgStringList = new List<string>();
            int id = 0;
            while (!pos.IsNull)
            {
                string buff = cfgList.GetNext(pos);
                if (buff.CompareTo("@") != 0)
                {
                    cfgStringList.Add(buff);
                }
                id++;
            }

            return cfgStringList.ToArray();
        }


        public   void GetLastVersionAsmPdm(string path)
        {
            try
            {
                IEdmFolder5 oFolder;
                GetEdmFile5(path, out oFolder).GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_RefsVerLatest);
            }
            catch (Exception exception)
            {
               // Logger.ToLog($"Message - {exception.ToString()}\nPath - {path}\nStackTrace - {exception.StackTrace}", null, "GetLastVersionOfFile", "SwEpdm");
            }
        }

        internal   IEdmFile5 GetEdmFile5(string path, out IEdmFolder5 folder)
        {
            folder = null;
            try
            {
                IEdmFolder5 oFolder;
                var edmFile5 =  edmVault5.GetFileFromPath(path, out oFolder);
                edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_RefsVerLatest);
                folder = oFolder;
                return edmFile5;
            }
            catch (Exception exception)
            {
                //Логгер.Ошибка($"Message - {exception.ToString()}\nPath - {path}\nStackTrace - {exception.StackTrace}", null, "GetEdmFile5", "SwEpdm");
                return null;
            }
        }


        /// <summary>
        /// Копирует файл в указаную директорию (не скачивает).
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public string CloneDowladFileTo(string directoryPath, DataModel dataModel)
        {
            string[] splitFileName = dataModel.FileName.Split('.');
            string fileExtension = splitFileName[splitFileName.Length - 1];
            string copiedFilePath = " ";

            copiedFilePath = directoryPath + dataModel.Id + "." + fileExtension;


            try
            {
                if (Directory.Exists(copiedFilePath) == false)
                {
                    Logger.ToLog("Файл " + copiedFilePath + " уже существует.");

                    try
                    {
                        File.SetAttributes(copiedFilePath, FileAttributes.Normal);
                        File.Delete(copiedFilePath);
                        Logger.ToLog("Файл " + copiedFilePath + "удален.");
                    }
                    catch (Exception exception)
                    {
                        Logger.ToLog("Неудалось удалить файл по имени " + copiedFilePath +
                            "\n по причине обозначеной в исключении " + exception);
                        Logger.ToLog("Дополнительная информация:\n" + "Копируемый файл " + dataModel.Path + "\nДиректория " + directoryPath + "\nРасширение файла " + fileExtension);
                    }
                }

                File.Copy(dataModel.Path, copiedFilePath, true);
                Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скопирован.");

            }
            catch (Exception exception)
            {
                Logger.ToLog("Неудалось скопировать файл по имени " + dataModel.FileName + " с Id " + dataModel.Id + "в директорию " + copiedFilePath +
                    "\n по причине обозначеной в исключении " + exception);
            }
            Logger.ToLog(copiedFilePath);
            return copiedFilePath;
        }


        #region
        ///// <summary>
        ///// Download file in to local directory.
        ///// </summary>
        ///// <param name="dataModel"></param>
        //public string GetPathAndDownLoadFile(DataModel dataModel, string directoryPath)
        //{          
        //    this.PDMInitialize(); 
        //    IEdmFolder5 srcFolder = null;
        //    IEdmFile5 file = edmVault5.GetFileFromPath(dataModel.Path,out srcFolder);
        //    IEdmVault7 vault2 = (IEdmVault7)this.edmVault5;
        //    IEdmFolder5 edmFolder5 = vault2.GetFolderFromPath(directoryPath);
        //    IEdmFolder8 edmFolder8 = (IEdmFolder8)edmFolder5;

        //    int copyFileStatus;
        //    edmFolder8.CopyFile2(file.ID, srcFolder.ID, 0, out copyFileStatus, "", (int)EdmCopyFlag.EdmCpy_Simple);

        //    //var batchGetter = (IEdmBatchGet)(edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
        //    // batchGetter.AddSelectionEx((EdmVault5)edmVault5, dataModel.Id, dataModel.FolderId, 0);
        //    // if ((batchGetter != null))
        //    // {
        //    //     batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
        //    //     batchGetter.GetFiles(0, null);
        //    // }
        //    return null;
        //}
        #endregion
        #region references
        ///// <summary>
        ///// Reference in to the components of assembly.  
        ///// </summary>
        ///// <param name="file"></param>
        ///// <param name="indent"></param>
        ///// <param name="projName"></param>
        ///// <returns></returns>
        //private string AddReferences(IEdmReference5 file, long indent, ref string projName)
        //{
        //    string filename = null;

        //    filename = filename + file.Name;

        //    const bool isTop = false;

        //    IEdmVault7 vault2 = null;

        //    vault2 = (IEdmVault7)edmVault5;

        //    IEdmPos5 pos = file.GetFirstChildPosition(projName, isTop, true, 0);

        //    IEdmFolder5 oFolder = null;


        //    while (!(pos.IsNull))
        //    {
        //        IEdmReference5 @ref = file.GetNextChild(pos);
        //        var oFile = (IEdmFile5)edmVault5.GetFileFromPath(@ref.FoundPath, out oFolder);

        //        filename = filename + AddReferences(@ref, indent, ref projName);

        //        // //MessageBox.Show(filename);
        //        // Последняя копия перечня в сборке
        //        oFile.GetFileCopy(0, "", @ref.FoundPath);
        //    }
        //    return filename;
        //}
        //public void ShowReferences(string filePath)
        //{
        //    // ERROR: Not supported in C#: OnErrorStatement
        //    string projName = null;
        //    IEdmFile5 file = default(IEdmFile5);
        //    IEdmFolder5 folder = default(IEdmFolder5);
        //    file = this.edmVault5.GetFileFromPath(filePath, out folder);

        //    IEdmReference5 @ref = default(IEdmReference5);
        //    @ref = file.GetReferenceTree(folder.ID, 0);
        //    AddReferences(@ref, 0, ref projName);

        //}
        #endregion

        /// <summary>
        /// Pdm initializes an instance of this object by creating and producing auto-login.
        /// </summary>
        private void PDMInitialize()
        {
            try
            {
                if (edmVault5 == null)
                {
                    edmVault5 = new EdmVault5();
                    Logger.ToLog("Создан экземпляр Vents-PDM");
                }

                if (!edmVault5.IsLoggedIn)
                {
                    edmVault5.LoginAuto(vaultname, 0);
                    Logger.ToLog("Автологин в системе Vents-PDM системного пользователя " + vaultname);
                }
            }
            catch (Exception exception)
            {
                Logger.ToLog("Невозможно создать экземпляр Vents-PDM - " + this.vaultname + "\n" + exception);
                throw new Exception("Невозможно создать экземпляр " + this.vaultname + "\n"+exception);
            }
            finally
            {
                Logger.ToLog("\n");
            }
        }


        private List<BomShell> GetBomShell(int bomId, string filePath, string bomConfiguration, EdmBomFlag bomFlag, out Exception exception)
        {
            exception = null;
            try
            {
                IEdmFolder5 oFolder;

                IEdmFile7 EdmFile7 = (IEdmFile7)edmVault5.GetFileFromPath(filePath, out oFolder);
                var bomView = EdmFile7.GetComputedBOM(Convert.ToInt32(bomId), Convert.ToInt32(-1), bomConfiguration, (int)bomFlag);

                if (bomView == null)
                    throw new Exception("Computed BOM it can not be null") ;
                object[] bomRows;
                EdmBomColumn[] bomColumns;
                bomView.GetRows(out bomRows);
                bomView.GetColumns(out bomColumns);
                var bomTable = new DataTable();

                foreach (EdmBomColumn bomColumn in bomColumns)
                {
                    bomTable.Columns.Add(new DataColumn { ColumnName = bomColumn.mbsCaption });
                }

                //bomTable.Columns.Add(new DataColumn { ColumnName = "Путь" });
                bomTable.Columns.Add(new DataColumn { ColumnName = "Уровень" });
                bomTable.Columns.Add(new DataColumn { ColumnName = "КонфГлавнойСборки" });
                bomTable.Columns.Add(new DataColumn { ColumnName = "ТипОбъекта" });
                bomTable.Columns.Add(new DataColumn { ColumnName = "GetPathName" });

                for (var i = 0; i < bomRows.Length; i++)
                {
                    var cell = (IEdmBomCell)bomRows.GetValue(i);

                    bomTable.Rows.Add();

                    for (var j = 0; j < bomColumns.Length; j++)
                    {
                        var column = (EdmBomColumn)bomColumns.GetValue(j);

                        object value;
                        object computedValue;
                        string config;
                        bool readOnly;

                        cell.GetVar(column.mlVariableID, column.meType, out value, out computedValue, out config, out readOnly);

                        if (value != null)
                        {
                            bomTable.Rows[i][j] = value;
                        }
                        else
                        {
                            bomTable.Rows[i][j] = null;
                        }
                        bomTable.Rows[i][j + 1] = cell.GetTreeLevel();

                        bomTable.Rows[i][j + 2] = bomConfiguration;
                        bomTable.Rows[i][j + 3] = config;
                        bomTable.Rows[i][j + 4] = cell.GetPathName();
                    }
                }
                exception = null;
                return BomTableToBomList(bomTable);

            }
            catch (Exception exception)
            {
                exception = exception;
                throw exception;
            }            
        }       

        private  List<BomShell> BomTableToBomList(DataTable table)
        {
            List<BomShell>   BoomShellList  = new List<BomShell>(table.Rows.Count);

            BoomShellList.AddRange(from DataRow row in table.Rows
                             select row.ItemArray into values
                             select new BomShell
                             {
                                 Partition = values[0].ToString(),
                                 Designation = values[1].ToString(),
                                 Name = values[2].ToString(),
                                 Material = values[3].ToString(),
                                 MaterialCmi = values[4].ToString(),
                                 SheetThickness = values[5].ToString(),
                                 Count = Convert.ToDecimal(values[6]),
                                 FileType = values[7].ToString(),
                                 Configuration = values[8].ToString(),
                                 LastVesion = Convert.ToInt32(values[9]),
                                 IdPdm = Convert.ToInt32(values[10]),
                                 FileName = values[11].ToString(),
                                 FilePath = values[12].ToString(),
                                 ErpCode = values[13].ToString(),
                                 SummMaterial = values[14].ToString(),
                                 Weight = values[15].ToString(),
                                 CodeMaterial = values[16].ToString(),
                                 Format = values[17].ToString(),
                                 Note = values[18].ToString(),
                                 Level = Convert.ToInt32(values[19]),
                                 ConfigurationMainAssembly = values[20].ToString(),
                                 TypeObject = values[21].ToString(),
                                 GetPathName = values[22].ToString()
                             });

            //LoggerInfo("Список из полученой таблицы успешно заполнен элементами в количестве" + bomList.Count);
            return BoomShellList;
        }
         
        public IEnumerable<Specification> GetSpecifications(string filePath, string config, bool asBuild, out Exception exception)
        {

            var bomFlag = asBuild ? EdmBomFlag.EdmBf_AsBuilt : EdmBomFlag.EdmBf_ShowSelected;
            var from = GetBomShell(BOM_ID, filePath, config, EdmBomFlag.EdmBf_ShowSelected, out exception);
            return this.sqlAdapter.GetSpecifications(from.ToArray());
        }
      private  const int BOM_ID = 8;



        /// <summary>
        /// Registration or unregistation files by their paths and registration status.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isRegistration"></param>
        public   void CheckInOutPdm(List<FileInfo> filesList, bool registration)
        {
            #region not working code
            //foreach (var file in filesList)
            //{
            //    try
            //    {
            //        IEdmFolder5 oFolder;
            //        IEdmFile5 edmFile5 = edmVault5.GetFileFromPath(file.FullName, out oFolder);

            //        var batchGetter = (IEdmBatchGet)(edmVault5 as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
            //        batchGetter.AddSelectionEx(edmVault5, edmFile5.ID, oFolder.ID, 0);
            //        if ((batchGetter != null))
            //        {
            //            batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
            //            batchGetter.GetFiles(0, null);
            //        }

            //        // Разрегистрировать
            //        if (!registration)
            //        {                    

            //            if (!edmFile5.IsLocked)
            //            {


            //                edmFile5.LockFile(oFolder.ID, 0);
            //                Thread.Sleep(50);
            //            }
            //        }
            //        else if (registration)
            //            if (edmFile5.IsLocked)
            //            {
            //                edmFile5.UnlockFile(oFolder.ID, "");
            //                Thread.Sleep(50);
            //            }
            //    }

            //    catch (Exception exception)
            //    {
            //        MessageBox.Show(exception.ToString() + "\n" + exception.StackTrace + "\n" + exception.Source);
            //    }
            //}



            //foreach (var eachFile in files)
            //{
            //    try
            //    {
            //        IEdmFolder5 folderPdm;
            //        IEdmFile5 filePdm = edmVault5.GetFileFromPath(eachFile.FullName, out folderPdm);

            //        if (filePdm == null)
            //        {
            //            filePdm.GetFileCopy(0, 0, folderPdm.ID, (int)EdmGetFlag.EdmGet_Simple);
            //        }

            //        // Разрегистрировать
            //        if (registration == false)
            //        {
            //            if (!filePdm.IsLocked)
            //            {
            //                filePdm.LockFile(folderPdm.ID, 0);
            //                Thread.Sleep(50);
            //            }
            //        }
            //        else
            //        {
            //            if (filePdm.IsLocked)
            //            {
            //                filePdm.UnlockFile(folderPdm.ID, "");
            //                Thread.Sleep(50);
            //            }
            //        }
            //    }

            //    catch (Exception exception)
            //    {
            //        MessageBox.Show(exception.ToString() + "\n" + exception.StackTrace + "\n" + exception.Source);
            //    }
            //}

            #endregion
            foreach (var file in filesList)
            {
                var retryCount = 2;
                var success = false;
                while (!success && retryCount > 0)
                {
                    try
                    {
                        IEdmFolder5 oFolder;
                        IEdmFile5 edmFile5 = edmVault5.GetFileFromPath(file.FullName, out oFolder);
                        edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_Simple);
                        // Разрегистрировать
                        if (registration == false)
                        {

                            m1:
                            edmFile5.LockFile(oFolder.ID, 0);
                            //MessageBox.Show(edmFile5.Name);
                            Thread.Sleep(50);
                            var j = 0;
                            if (!edmFile5.IsLocked)
                            {
                                j++;
                                if (j > 5)
                                {
                                    goto m3;
                                }
                                goto m1;
                            }
                        }
                        // Зарегистрировать
                        if (registration)
                        {
                            try
                            {
                                m2:
                                edmFile5.UnlockFile(oFolder.ID, "");
                                Thread.Sleep(50);
                                var i = 0;
                                if (edmFile5.IsLocked)
                                {
                                    i++;
                                    if (i > 5)
                                    {
                                        goto m4;
                                    }
                                    goto m2;
                                }
                            }
                            catch (Exception exception)
                            {
                               // MessageBox.Show(exception.ToString());
                            }
                        }
                        m3:
                        m4:
                        //LoggerInfo(string.Format("В базе PDM - {1}, зарегестрирован документ по пути {0}", file.FullName, vaultName), "", "CheckInOutPdm");
                        success = true;
                    }


                    catch (Exception exception)
                    {
                      //  Логгер.Ошибка($"Message - {exception.ToString()}\nfile.FullName - {file.FullName}\nStackTrace - {exception.StackTrace}", null, "CheckInOutPdm", "SwEpdm");
                        retryCount--;
                        Thread.Sleep(200);
                        if (retryCount == 0)
                        {
                            //
                        }
                    }
                }
                if (!success)
                {
                    //LoggerError($"Во время регистрации документа по пути {file.FullName} возникла ошибка\nБаза - {vaultName}. {exception.ToString()}", "", "CheckInOutPdm");
                }
            }
        }


        public IEdmFile5 GetFileById (int fileId)
        {
            return (IEdmFile5)edmVault5.GetObject(EdmObjectType.EdmObject_File, fileId);
        }

       

    }


}
