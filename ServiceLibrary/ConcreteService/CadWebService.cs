using DataBaseDomian.XML;
using Patterns.Observer;
using PdmSolidWorksLibrary;
using PDMWebService.Data.Solid.ElementsCase;
using ServiceTypes.Constants;
using ServiceLibrary.Models;
using ServiceLibrary.Models.DataContracts;
using ServiceLibrary.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskSystemLibrary;

namespace ServiceLibrary.ConcreteService
{
    public class CadWebService : ISolidWebService
    {
       
        private TaskManager taskManager;
        //public VentsManagingWebService()
        //{
        //    try
        //    {
        //        taskManager = TaskSystemLibrary.TaskManager.Instance;
                
        //        spigotBuilder = new SpigotBuilder();
        //        spigotBuilder.CheckExistPart = global_CheckExistPartEvent;
        //        spigotBuilder.FinishedBuild = SpigotBuilder_FinishedBuildEvent;

  
        //    }
        //    catch (Exception ex)
        //    {
        //       MessageObserver.Instance.SetMessage("Filed created TaskManager exemplar; message { " + ex + " }");
        //    }

        //}

        public string GetPathSelectFile(TransmittableFileModel dataSolidModel)
        {
            string UrlToSelectModel = " ";
            //string filePath = " ";
            //try
            //{
            //    SolidWorksPdmAdapter.Instance.DownLoadFile(DataConverter.ToFileModelPdm(dataSolidModel));
            //    filePath = Data.Explorer.ExplorerAdapter.FileCopyToVirtualFolder(@"D:\temp\".ToUpper(), dataSolidModel); //SolidWorksPdmAdapter.Instance.FileCopy(@"D:\temp\".ToUpper(), dataSolidModel);
            //    UrlToSelectModel = filePath.ToUpper().Replace(@"D:\temp\".ToUpper(), "http://pdmsrv/test/eDrawings/".ToUpper()); // need get from config
            //}
            //catch (Exception exception)
            //{
            //    Logger.ToLog("Невозможно вернуть путь к файлу " + exception.Message);
            //}
            return UrlToSelectModel;
        }

        public TransmittableFile SelectFile(TransmittableFileModel dataSolidModel)
        {

            SolidWorksPdmAdapter.Instance.GetFileById(DataConverter.ToFileModelPdm(dataSolidModel).IDPdm,true);

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
        public TransmittableFileModel[] Search(string nameSegment)
        {
           MessageObserver.Instance.SetMessage(nameSegment);
            return DataConverter.ToTransferFileModel(SolidWorksPdmAdapter.Instance.SearchDoc(nameSegment)).ToArray();
        }

        public string[] GetConfigigurations(string filePath)
        {
            return SolidWorksPdmAdapter.Instance.GetConfigigurations(filePath);
        }

        public TransmittableSpecification[] GetSpecifications(string filePath, string configuration)
        { 
                return DataConverter.GetSpecification(filePath,   configuration);
        }


        ////public void UploadDXF(string name, int idpdm, string configuration, int version)
        ////{

        ////    List<DataModel> datamodels = new List<DataModel>();
        ////    string n = name.Split('.')[0];
        ////    datamodels.AddRange(PDMAdapter.Instance.SearchDoc("%" + n + "%"));
        ////   MessageObserver.Instance.SetMessage("d m count " + datamodels);

        ////    foreach (var model in datamodels)
        ////    {
        ////       MessageObserver.Instance.SetMessage(model.FileName);

        ////        PDMAdapter.Instance.DownLoadFile(model);

        ////        string sldModelPath = PDMAdapter.Instance.CloneDowladFileTo(@"D:\TEMP\dxf\".ToUpper(), model);
        ////       MessageObserver.Instance.SetMessage(sldModelPath);
        ////        //  string dxfPath = SolidWorksInstance. SolidAdapter.ConvertToDXF(sldModelPath, configuration);
        ////       MessageObserver.Instance.SetMessage("Dxf сконвертирован");

        ////        //System.IO.FileInfo fileInfo = new System.IO.FileInfo(dxfPath);
        ////        //if (!fileInfo.Exists)
        ////        //    throw new System.IO.FileNotFoundException("File not found", dxfPath);

        ////        //System.IO.FileStream stream = new System.IO.FileStream(dxfPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        ////        //byte[] binaryDxf = ReadFully(stream);

        ////        //  SqlDataAdapder.Instance.UploadDxf(binaryDxf, idpdm, configuration, version);
        ////    }
        ////}

        //public void CreateRoof(int height, int wight, RoofTypes type, int userId)
        //{
        //    //   taskManager.CreateRoof(height, wight, type, userId);
        //}

        //public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId)
        //{
        //    //    taskManager.CreateVibroInsertion(height, wight, type, userId);
        //}



        //public void CreateFlap(FlapTypes type, int height, int wight, bool isOuter, int userId)
        //{
        //    //  taskManager.CreateFlap(type, height, wight, ServiceLibrary.TaskSystem.ServiceTypes.Constants.Meterials.Aluzinc_Az_150_07, isOuter, 0.7f, userId);
        //}

        //public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
        //{
        //    //  taskManager.CreateFlap(type, height, wight, material, isOuter, thickness, userId);
        //}

        ////public void CreateDxf(int FileId)
        ////{

        ////}

        ////public void CreatePdf(int FileId)
        ////{
        ////    taskManager.CreatePdf(FileId);
        ////}

        public void CreateDxf(int[] filesId)
        {
            try
            {
               MessageObserver.Instance.SetMessage("Input request on service: Create dxf");
                taskManager.CreateDxf(filesId);
            }
            catch (Exception ex)
            {
               MessageObserver.Instance.SetMessage("Exeption on seb service level; message { " + ex.ToString() + " }");
            }
        }

        public void CreatePdf(int[] filesId)
        {
            try
            {
               MessageObserver.Instance.SetMessage("Input request service: Create pdf");
                taskManager.CreatePdf(filesId);
            }
            catch (Exception ex)
            {
               MessageObserver.Instance.SetMessage("Exeption on seb service level; message { " + ex.ToString() + " }");
            }
        }

        public TransmittableTaskData[] GetActiveTasksData()
        {
           MessageObserver.Instance.SetMessage("Input request service: Create pdf");
            return DataConverter.ToTaskDataTransmittable(taskManager.GetActiveTasksData()).ToArray();

        }

        public TransmittableTaskData[] GetComplitedTasksData()
        {
          
            return DataConverter.ToTaskDataTransmittable(taskManager.GetComplitedTasksData()).ToArray();
        }

        public void ExportPartDataToXml(TransmittableSpecification[] specification)
        {
            List<Specification> list = new List<Specification>();
                 

            foreach (var item in specification)
            {
                list.Add(new Specification
                {
                    Bend = item.Bend,
                    CodeMaterial = item.CodeMaterial,
                    Configuration = item.Configuration,
                    Count = item.Count.ToString(),
                    Description = item.Description,
                    ERPCode = item.ERPCode,
                    FileName = item.FileName,
                    IDPDM = item.IDPDM,
                    isDxf = item.isDxf,
                    Level = item.Level,
                    PaintX = item.PaintX,
                    PaintY = item.PaintY,
                    PaintZ = item.PaintZ,
                    Partition= item.Partition,
                    PartNumber = item.PartNumber,
                    SummMaterial = item.SummMaterial,
                    SurfaceArea = item.SurfaceArea,
                    Thickness = item.Thickness,
                    Type = item.Type,
                    Version  = item.Version,
                    Weight = item.Weight,
                    WorkpieceX = item.WorkpieceX,
                    WorkpieceY = item.WorkpieceY 
                });
            }
                                                                                // only parts
            DataBaseDomian.XML.ExportToXml.ExportPartDataToXml(list.Where(each => each.Level == 2));
        }

        public bool isServiceWork()
        {
            return true;
        }

        //  ===================================    TEST CREATE SPIGOT REGION ======================================================
        // ========================================================================================================================

       private static  SpigotBuilder spigotBuilder;

        //                                                        TEST CONSTRUCTOR
        public CadWebService()
        {
            try
            {
             //   taskManager = TaskSystemLibrary.TaskManager.Instance;

                spigotBuilder = new SpigotBuilder();
                spigotBuilder.CheckExistPart = CheckExistPart_Handler;
                spigotBuilder.FinishedBuild = SpigotBuilder_FinishedBuildEvent;


            }
            catch (Exception ex)
            {
                MessageObserver.Instance.SetMessage("Filed created TaskManager exemplar; message { " + ex + " }");
            }

        }
        // ===================================           METHODS  ==========================================================================


        //TO DO
        public void CreateSpigot(SpigotType_e type, int width, int height)
        { 
            SpigotBuilder spigotBuilder = new SpigotBuilder( );
            spigotBuilder.CheckExistPart = CheckExistPart_Handler;
            try
            {
                spigotBuilder.Build((int)type, width, height);

                foreach (var item in spigotBuilder.ComponentsPathList)
                {
                    SolidWorksPdmAdapter.Instance.AddToPdm(item, @"D:\Test\Библиотека проектирования\DriveWorks\12 - Spigot");///////////////////////////////////////////////////////////////
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString( ));
            }
        }
        /// <summary>
        /// test method for handle finished build
        /// </summary>
        /// <param name="ComponentsPathList"></param>
        private static void SpigotBuilder_FinishedBuildEvent(List<string> ComponentsPathList)
        {
            MessageObserver.Instance.SetMessage("SpigotBuilder_FinishedBuildEvent");
            if (ComponentsPathList.Count > 0)
            {
                MessageObserver.Instance.SetMessage("Я закончил генерацию:");
                foreach (var item in ComponentsPathList)
                {
                    MessageObserver.Instance.SetMessage("\t" + item);
                }

            }
            else
            {
                MessageObserver.Instance.SetMessage("Я ничего не сгенерировал:");
            }
        }

        /// <summary>
        /// test method for handle checking exist part
        /// </summary>
        /// <param name="partName"></param>
        /// <param name="isExesitPatrt"></param>
        /// <param name="pathToPart"></param>
        private static void CheckExistPart_Handler(string partName, string RootFolder, out string pathToPart)
        {

            pathToPart = null;
            var model = DataBaseDomian.SwPlusRepository.Instance.ByName(partName);
            
            if (model != null)
            {
                pathToPart = $@"{RootFolder}{model.Path}{partName}";
                SolidWorksPdmAdapter.Instance.DownloadFile(pathToPart);
            }
        }


    }
}