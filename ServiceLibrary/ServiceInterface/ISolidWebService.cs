using ServiceConstants;
using ServiceLibrary.Models.DataContracts;
using System.ServiceModel;


namespace ServiceLibrary.ServiceInterface
{

    [ServiceContract]
    public interface ISolidWebService
    {
        /// <summary>
        /// Get list a data solid models by name. 
        /// </summary>
        /// <param name="nameSegment"></param>
        /// <returns></returns>
        [OperationContract]
        TransmittableFileModel[] Search(string nameSegment);

        /// <summary>
        /// Get serialize file fro selected model. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        TransmittableFile SelectFile(TransmittableFileModel dataSolidModel);
        /// <summary>
        /// Get path to virtual folder for selected model
        /// </summary>
        /// <param name="dataSolidModel"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPathSelectFile(TransmittableFileModel dataSolidModel);
        /// <summary>
        /// Get configurations for selected model 
        /// </summary>
        /// <param name="dataSolidModel"></param>
        /// <returns></returns>
        [OperationContract]
        string[] GetConfigigurations(string filePath);
        /// <summary>
        ///  Get specifications by all parts selected model 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        [OperationContract]
        TransmittableSpecification[] GetSpecifications(string filePath, string configuration);

        /// <summary>
        /// Check dxf data
        /// </summary>
        /// <param name="idPDM"></param>
        /// <param name="configuration"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        //[OperationContract]
        //bool CheckDEF(int idPDM, string configuration, int version);



        //[OperationContract]
        //void CreateRoof(int height, int wight,  RoofTypes type, int userId);
        //[OperationContract]
        //void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId);

        ///// <summary>
        ///// Create a flap with the custom material and thickness.
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="height"></param>
        ///// <param name="wight"></param>
        ///// <param name="material"></param>
        ///// <param name="isOuter"></param>
        ///// <param name="thickness"></param>
        ///// <param name="userId"></param>
        //[OperationContract]
        //void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0);
        ///// <summary>
        ///// test method...
        ///// </summary>
        //[OperationContract]
        //void OpenSolidWorks();


        [OperationContract(IsOneWay = true)]
        void CreateDxf(int[] filesId);

        [OperationContract(IsOneWay = true)]
        void CreatePdf(int[] filesId);

        /// <summary>
        /// Returned data about active tasks
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        TransmittableTaskData[] GetActiveTasksData();

        /// <summary>
        /// Returned data about complited tasks
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        TransmittableTaskData[] GetComplitedTasksData();

        [OperationContract(IsOneWay = true)]
        void ExportPartDataToXml(TransmittableSpecification[] specification);


        [OperationContract]
        bool isServiceWork();

        [OperationContract(IsOneWay =true)]
        void CreateSpigot(SpigotType_e type, int width, int height);

    }
}
