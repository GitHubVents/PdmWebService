
//using ServiceLibrary.DataContracts;
//using System;
//using System.IO;

//namespace PDMWebService.Data.Explorer
//{
//    abstract class ExplorerAdapter
//    {

       
//     // ===============================================================================
//        // Данный метод необходим для копирования файлов в виртуальную папку. используеться для веб простмотрщика. переписать в связи с неочевидностью использования 
   
//        /// <summary>
//        /// Копирует файл в указаную директорию (не скачивает).
//        /// </summary>
//        /// <param name="directory"></param>
//        /// <param name="dataModel"></param>
//        /// <returns></returns>
//        public static string FileCopyToVirtualFolder(string directory, DataModel dataModel)
//        {
//            string fileExtension = Path.GetExtension(dataModel.FileName).Replace(".","");
//            string copiedFilePath = " ";

//            copiedFilePath = directory +@"\"+ dataModel.Id + "." + fileExtension;


//            try
//            {
//                if (Directory.Exists(copiedFilePath) == false)
//                {
//                    Logger.ToLog("Файл " + copiedFilePath + " уже существует.");

//                    try
//                    {
//                        File.SetAttributes(copiedFilePath, FileAttributes.Normal);
//                        File.Delete(copiedFilePath);
//                        Logger.ToLog("Файл " + copiedFilePath + "удален.");
//                    }
//                    catch (Exception exception)
//                    {
//                        Logger.ToLog("Неудалось удалить файл по имени " + copiedFilePath +
//                            "\n по причине обозначеной в исключении " + exception);
//                        Logger.ToLog("Дополнительная информация:\n" + "Копируемый файл " + dataModel.Path + "\nДиректория " + directory + "\nРасширение файла " + fileExtension);
//                    }
//                }

//                File.Copy(dataModel.Path, copiedFilePath, true);
//                Logger.ToLog("Файл " + dataModel.FileName + " с id " + dataModel.Id + " успешно скопирован.");

//            }
//            catch (Exception exception)
//            {
//                Logger.ToLog("Неудалось скопировать файл по имени " + dataModel.FileName + " с Id " + dataModel.Id + "в директорию " + copiedFilePath +
//                    "\n по причине обозначеной в исключении " + exception);
//            }
//            Logger.ToLog(copiedFilePath);
//            return copiedFilePath;
//        }
//        // ======================================================================================================


//        public static string FileCopy (string path, string directory)
//        {

//            string copiedFilePath = directory + @"\" + Path.GetFileName(path);
//            try
//            {
//                if (Directory.Exists(copiedFilePath) == false)
//                {
//                    Logger.ToLog("Файл " + copiedFilePath + " уже существует.");

//                    try
//                    {
//                        File.SetAttributes(copiedFilePath, FileAttributes.Normal);
//                        File.Delete(copiedFilePath);
//                        Logger.ToLog("Файл " + copiedFilePath + "удален.");
//                    }
//                    catch (Exception exception)
//                    {
//                        Logger.ToLog("Неудалось удалить файл по имени " + copiedFilePath +
//                            "\n по причине обозначеной в исключении " + exception);
                       

//                    }
//                }
//                Console.WriteLine("FileCopyToVirtualFolder " + copiedFilePath);
//                File.Copy(path, copiedFilePath, true);
//                Logger.ToLog("Файл " + path + " успешно скопирован в " + directory);
//                return copiedFilePath;
               

//            }
//            catch (Exception exception)
//            {
//                Logger.ToLog("Неудалось скопировать файл по имени " +path +  "в директорию " + copiedFilePath +
//                    "\n по причине обозначеной в исключении " + exception);
//                throw exception;
//            } 
       
//        }
//    }
//}
