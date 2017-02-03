using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using EPDM.Interop.epdm;
using System.Runtime.InteropServices;

using Patterns;
using PdmSolidWorksLibrary.Models;
using System.Threading;

namespace PdmSolidWorksLibrary
{
    public class SolidWorksPdmAdapter :Singeton<SolidWorksPdmAdapter>
    {

        private SolidWorksPdmAdapter() : base(){}

        /// <summary>
        /// PDM exemplar.
        /// </summary>
        private static IEdmVault5 edmVeult5 = null;
        private IEdmVault5 PdmExemplar
        {
            get
            {

                try
                {
                    if (edmVeult5 == null)
                    {
                        KillProcsses("ViewServer");
                        KillProcsses("AddInSrv");

                        edmVeult5 = new EdmVault5();
                     //   Logger.ToLog("Создан экземпляр Vents-PDM");
                        if (!edmVeult5.IsLoggedIn)
                        {
                            edmVeult5.LoginAuto(vaultname, 0);
                            //ogger.ToLog("Автологин в системе Vents-PDM системного пользователя " + vaultname);
                        }
                    }

                    return edmVeult5;
                }
                catch (Exception exception)
                {
                   // Logger.ToLog("Невозможно создать экземпляр Vents-PDM - " + this.vaultname + "\n" + exception);
                    throw new Exception("Невозможно создать экземпляр " + this.vaultname + "\n" + exception);
                }
                finally
                {
                 //   Logger.ToLog("\n");
                }
            }
        }

        /// <summary>
        /// Vents pdm name.
        /// </summary>
        private string vaultname { get; set; } = "Vents-PDM";
         


        /// <summary>
        /// Search document by name.
        /// </summary>
        /// <param name="segmentName"></param>
        /// <returns></returns>
        public IEnumerable < FileModelPdm> SearchDoc(string segmentName)
        { 
            List<FileModelPdm> searchResult = new List<FileModelPdm>();
            try
            {
                var Search = (PdmExemplar as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_Search);
                Search.FileName = segmentName;
                Search.SetToken(EdmSearchToken.Edmstok_FindFolders, false);
                int count = 0;

                IEdmSearchResult5 Result = Search.GetFirstResult();
                while (Result != null)
                {
                    

                    
                    searchResult.Add(new FileModelPdm
                    {
                        FileName = Result.Name,
                        IDPdm = Result.ID,
                        FolderId = Result.ParentFolderID,
                        Path = Result.Path,
                        FolderPath = Path.GetDirectoryName(Result.Path),
                        CurrentVersion = Result.Version
                    });



                    Result = Search.GetNextResult();
                    count++;
                }
                //Logger.ToLog("По запросу " + segmentName + " найдено " + count);

                Console.WriteLine("Succsess search: return result model");

            }
            catch (Exception exception)
            {
              //  Logger.ToLog("По запросу " + segmentName + " не найдено ни одного файла\n Ошибка: " + exception);
                Console.WriteLine(exception.ToString());
                throw exception;

            }
            return searchResult;
        }


        /// <summary>
        /// Download file in to local directory witch has fixed path
        /// </summary>
        /// <param name="fileModel"></param>
        public void DownLoadFile(FileModelPdm fileModel)
        {
            
            try
            {
                var batchGetter = (IEdmBatchGet)(PdmExemplar as IEdmVault7).CreateUtility(EdmUtility.EdmUtil_BatchGet);
                batchGetter.AddSelectionEx((EdmVault5)PdmExemplar, fileModel.IDPdm, fileModel.FolderId, 0);
                if ((batchGetter != null))
                {
                    batchGetter.CreateTree(0, (int)EdmGetCmdFlags.Egcf_SkipUnlockedWritable);
                    batchGetter.GetFiles(0, null);
                }
                Console.WriteLine("Download file");
              //  Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скачан с PDM системы по пути " + dataModel.Path);
            }
            catch (Exception exception)
            {
               // Logger.ToLog("Ошибка при скачивании файла " + dataModel.FileName + " с id " + dataModel.Id + exception.ToString());

                Console.WriteLine(exception.ToString());
                throw exception;

            }
        }


        /// <summary>
        ///  Get configuration by data model
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        public string[] GetConfigigurations(FileModelPdm fileModel)
        {
            IEdmFile9 fileModelInf;
            IEdmFolder5 ppoRetParentFolder;
            fileModelInf = (IEdmFile9)PdmExemplar.GetFileFromPath(fileModel.Path, out ppoRetParentFolder);

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


        public void GetLastVersionAsmPdm(string path)
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

        internal IEdmFile5 GetEdmFile5(string path, out IEdmFolder5 folder)
        {
            folder = null;
            try
            {
                IEdmFolder5 oFolder;
                var edmFile5 = PdmExemplar.GetFileFromPath(path, out oFolder);
                edmFile5.GetFileCopy(0, 0, oFolder.ID, (int)EdmGetFlag.EdmGet_RefsVerLatest);
                folder = oFolder;
                return edmFile5;
            }
            catch (Exception exception)
            {
                //Логгер.Ошибка($"Message - {exception.ToString()}\nPath - {path}\nStackTrace - {exception.StackTrace}", null, "GetEdmFile5", "SwEpdm");
                throw exception;
            }
        } 
        private void KillProcsses (string name)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(name );
            foreach (var process in processes)
            {               
                process.Kill();
                Console.WriteLine("\nFind proccess and kill: " + process);
            }
          
        }
     const   int BOM_ID = 8;
        #region bom
        public IEnumerable<BomShell> GetBomShell(string filePath, string bomConfiguration )
        {
          
            try
            {
                IEdmFolder5 oFolder;

                IEdmFile7 EdmFile7 = (IEdmFile7)PdmExemplar.GetFileFromPath(filePath, out oFolder);
                var bomView = EdmFile7.GetComputedBOM(Convert.ToInt32(BOM_ID), Convert.ToInt32(-1), bomConfiguration, (int)EdmBomFlag.EdmBf_ShowSelected);

                if (bomView == null)
                    throw new Exception("Computed BOM it can not be null");
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
           
                return  BomTableToBomList(bomTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private   IEnumerable<BomShell> BomTableToBomList(DataTable table)
        {
            List<BomShell> BoomShellList = new List<BomShell>(table.Rows.Count);

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
                                       TypeObject = values[9].ToString(),
                                       GetPathName = values[22].ToString()
                                   });

            //LoggerInfo("Список из полученой таблицы успешно заполнен элементами в количестве" + bomList.Count);
            return BoomShellList;
        }



        public void CheckInOutPdm (IEnumerable<string>   pathToFiles, bool registration)
        {
            foreach (var eachFile in pathToFiles)
            {
                CheckInOutPdm(eachFile,registration);
            }
        }



        /// <summary>
        /// Registration or unregistation files by their paths and registration status.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isRegistration"></param>
        public void CheckInOutPdm(string pathToFile, bool registration)
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


            Console.WriteLine(pathToFile);
            var retryCount = 2;
            var success = false;
            while (!success && retryCount > 0)
            {
                try
                {
                    IEdmFolder5 oFolder;
                    IEdmFile5 edmFile5 = PdmExemplar.GetFileFromPath(pathToFile, out oFolder);
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
                    throw exception;
                }
            }
            if (!success)
            {
                //LoggerError($"Во время регистрации документа по пути {file.FullName} возникла ошибка\nБаза - {vaultName}. {exception.ToString()}", "", "CheckInOutPdm");
            }

        }

        /// <summary>
        /// Adds file to pdm. File must the locate in local directory pdm.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="folder"></param>
        public string AddToPdm(string pathToFile, string folder)
        {
            
            string msg = "";
            try
            {
                //if (File.Exists(pathToFile))
                //{
                //   File.SetAttributes(pathToFile, FileAttributes.Normal);
                //    File.Delete(pathToFile);
                //}
                var edmFolder = PdmExemplar.GetFolderFromPath(folder);
               
                edmFolder.AddFile(0, pathToFile);

             

            //    Logger.ToLog("Файлы добавлены в PDM");

             return   Path.Combine(folder, Path.GetFileName(pathToFile));

            }
            catch (COMException ex)
            {
                //  Logger.ToLog("ERROR BatchAddFiles " + msg + ", file: " + fileNameErr + " HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.ToString());   
                throw ex;           
            }
        }

        public void SetVariable(FileModelPdm fileModel, string pathToTempPdf)
        {
            try
            {


                var filePath = fileModel.FolderPath + "\\" + pathToTempPdf;

                IEdmFolder5 folder;
                var aFile = PdmExemplar.GetFileFromPath(filePath, out folder);
                var pEnumVar = (IEdmEnumeratorVariable8)aFile.GetEnumeratorVariable(); ;
                pEnumVar.SetVar("Revision", "", fileModel.CurrentVersion);
           //     Logger.ToLog("Файлу: " + fileModel.FileName + @"\" + pathToTempPdf + " добавлены переменные");
                pEnumVar.CloseFile(true);




            }
            catch (COMException ex)
            {
                // Logger.ToLog("ERROR BatchSetVariable файл: " + fileNameErr + ", " + ex.Message);
            }
        }


        public FileModelPdm GetFileById(int fileId, bool isDownload)
        {
            try
            {
                IEdmFile5 pdmFile = (IEdmFile5)PdmExemplar.GetObject(EdmObjectType.EdmObject_File, fileId);
                Console.WriteLine(SearchDoc(pdmFile.Name).Count());
                FileModelPdm ileModel = SearchDoc(pdmFile.Name).First();
                Console.WriteLine("GetFileById");
                if (isDownload)
                {
                    Console.WriteLine("\t\tGetFileById: start DownLoadFile");
                    DownLoadFile(ileModel);
                    Console.WriteLine("\t\tGetFileById: end DownLoadFile");
                }
                
                return ileModel;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw exception; 
            }
        }

    }
}
