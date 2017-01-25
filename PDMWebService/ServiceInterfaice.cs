
using PDM_WebService.WcfServiceLibrary;
using PDM_WebService.WcfServiceLibrary.DataContracts;
using PDMWebService.Data.PDM;
using PDMWebService.Data.Solid;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceLibrary.TaskSystem.Constants;
using EPDM.Interop.epdm;
using System.Threading;

namespace PDMWebService
{
    class ServiceInterfaice : ISolidWebService
    { 
        private TaskManager taskManager;
        public ServiceInterfaice()
        {
           

            Thread t = new Thread(InitTaskManager);
            t.Start();
        }

        private void InitTaskManager ()
        {
            try
            {
                taskManager = TaskManager.Instance;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }



        public string GetPathSelectFile(DataModel dataSolidModel)
        {
            string UrlToSelectModel = " ";
            string filePath = " ";
            try
            {
                SolidWorksPdmAdapter.Instance.DownLoadFile(dataSolidModel);
                filePath = Data.Explorer.ExplorerAdapter.FileCopyToVirtualFolder(@"D:\temp\".ToUpper(), dataSolidModel); //SolidWorksPdmAdapter.Instance.FileCopy(@"D:\temp\".ToUpper(), dataSolidModel);
                UrlToSelectModel = filePath.ToUpper().Replace(@"D:\temp\".ToUpper(), "http://pdmsrv/test/eDrawings/".ToUpper()); // need get from config
            }
            catch (Exception exception)
            {
                Logger.ToLog("Невозможно вернуть путь к файлу " + exception.Message);
            }
            return UrlToSelectModel;

           
        }

        public TransmittableFile SelectFile(DataModel dataSolidModel)
        {

            SolidWorksPdmAdapter.Instance.DownLoadFile(dataSolidModel);
            TransmittableFile remoteInfo;
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(dataSolidModel.Path);

                if (!fileInfo.Exists)
                    throw new System.IO.FileNotFoundException("File not found", dataSolidModel.Path);
                System.IO.FileStream stream = new System.IO.FileStream(dataSolidModel.Path,
                          System.IO.FileMode.Open, System.IO.FileAccess.Read);
                remoteInfo = new TransmittableFile(dataSolidModel.FileName, fileInfo.Length, ReadFully(stream));

            }
            catch (Exception exception)
            {
                throw exception;
            }
            return remoteInfo;

           
        }

        public static byte[] ReadFully(System.IO.Stream input)
        {
            byte[] buffer = new byte[input.Length];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
             
        }
        DataModel[] ISolidWebService.Search(string nameSegment)
        {
            Console.WriteLine(nameSegment);
            return SolidWorksPdmAdapter.Instance.SearchDoc(nameSegment).ToArray();
          
        }

        public string[] GetConfigiguration(DataModel dataSolidModel)
        {
            return null; // dpmAdapter.GetConfigiguration(dataSolidModel);
        }

        public Specification[] GetSpecifications(DataModel dataSolidModel, string configuration)
        {
            Exception exception = new Exception();
            try
            {
                Specification[] specifications = SolidWorksPdmAdapter.Instance.GetSpecifications(dataSolidModel.Path, configuration, true, out exception).ToArray();
                Logger.ToLog("Спецификации по модели " + dataSolidModel.FileName + " с конфигурацией " + configuration + " успешно получена");
                return specifications;
            }
            catch
            {
                Logger.ToLog("Ошобка при попытке получить список спецификаций по модели" + dataSolidModel.FileName + " с конфигурацией " + configuration + "\n" + exception.Message);
                return null;
            }
           
        }


        //public void UploadDXF(string name, int idpdm, string configuration, int version)
        //{

        //    List<DataModel> datamodels = new List<DataModel>();
        //    string n = name.Split('.')[0];
        //    datamodels.AddRange(PDMAdapter.Instance.SearchDoc("%" + n + "%"));
        //    Console.WriteLine("d m count " + datamodels);

        //    foreach (var model in datamodels)
        //    {
        //        Console.WriteLine(model.FileName);

        //        PDMAdapter.Instance.DownLoadFile(model);

        //        string sldModelPath = PDMAdapter.Instance.CloneDowladFileTo(@"D:\TEMP\dxf\".ToUpper(), model);
        //        Console.WriteLine(sldModelPath);
        //        //  string dxfPath = SolidWorksInstance. SolidAdapter.ConvertToDXF(sldModelPath, configuration);
        //        Console.WriteLine("Dxf сконвертирован");

        //        //System.IO.FileInfo fileInfo = new System.IO.FileInfo(dxfPath);
        //        //if (!fileInfo.Exists)
        //        //    throw new System.IO.FileNotFoundException("File not found", dxfPath);

        //        //System.IO.FileStream stream = new System.IO.FileStream(dxfPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //        //byte[] binaryDxf = ReadFully(stream);

        //        //  SqlDataAdapder.Instance.UploadDxf(binaryDxf, idpdm, configuration, version);
        //    }
        //}

        public void CreateRoof(int height, int wight, RoofTypes type, int userId)
        {
            taskManager.CreateRoof(height, wight, type, userId);
        }

        public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId)
        {
            taskManager.CreateVibroInsertion(height, wight, type, userId);
        }

        public void OpenSolidWorks()
        {

            Console.WriteLine("Пришел запрос на запуск Solid Works application");
            try
            {
                SolidWorksInstance.SldWoksApp.Visible = true;
                SolidWorks.Interop.sldworks.SldWorks sld = new SolidWorks.Interop.sldworks.SldWorks()
                {
                    Visible = true
                };
            }
            catch (Exception exc)
            {
                Console.WriteLine("Неудалось запустить Solid Works... \n\n" + exc.ToString());
            }
        }

        public void CreateFlap(FlapTypes type, int height, int wight, bool isOuter, int userId)
        {
            taskManager.CreateFlap(type, height, wight, ServiceLibrary.TaskSystem.Constants.Meterials.Aluzinc_Az_150_07, isOuter, 0.7f, userId);
        }

        public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
        {
            taskManager.CreateFlap(type, height, wight, material, isOuter, thickness, userId);
        }

        //public void CreateDxf(int FileId)
        //{
           
        //}

        //public void CreatePdf(int FileId)
        //{
        //    taskManager.CreatePdf(FileId);
        //}

        public void CreateDxf(int[] filesId)
        {
            Console.WriteLine("пришел такс дхф");
                taskManager.CreateDxf(filesId);
            Console.WriteLine("Таск выполняеться. клиенту ответили что то...");
        }

        public void CreatePdf(int[] filesId)
        {
            Console.WriteLine("пришел такс pdf");
            taskManager.CreatePdf(filesId);
            Console.WriteLine("Таск выполняеться. клиенту ответили что то...");
        }

//        int nWorkerThreads;
//        int nCompletionThreads;
//        ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionThreads);
//            foreach (var item in filesId)
//            {
//                ThreadPool.QueueUserWorkItem(delegate (object state)
//                {
//                    Console.WriteLine("пришел такс дхф");
//                    taskManager.CreateDxf(filesId);
//                });

//            }

//Console.WriteLine("Таск выполняеться. клиенту ответили что то...");
    }
}
