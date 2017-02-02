using DataBaseDomian;
using DataBaseDomian.Orm;
using Patterns.Observer;
using PdmSolidWorksLibrary;
using SolidWorksLibrary.Builders.Dxf;
using System.Collections.Generic;

namespace TaskSystemLibrary
{
    internal abstract class ExecuterFactory
    {
        public static void ExecuteDxf(TaskInstance taskInstance)
        {
            try
            {
          
                IEnumerable<TaskSelection> taskSelections = TaskSystemDataRepository.Instance.
                    GetSelectionsTasks(taskInstance.TaskInstanceID);
                foreach (var eachTaskSelections in taskSelections)
                {
                    var fileModel = SolidWorksPdmAdapter.Instance.GetFileById((int)eachTaskSelections.DocumentID, true);
                    DxfBulder.Instance.Build(fileModel.Path, fileModel.IDPdm, fileModel.CurrentVersion);
                }
                TaskSystemDataRepository.Instance.ApplyCompleted(taskInstance.TaskInstanceID);
            }
            catch
            {
                TaskSystemDataRepository.Instance.ApplyError(taskInstance.TaskInstanceID);
            }
        }


        public static void ExecutePdf (TaskInstance taskInstance)
        {
            try
            {

                // Console.WriteLine("Execute pdf task");

                IEnumerable<DataBaseDomian.Orm.TaskSelection> taskSelections = TaskSystemDataRepository.Instance.
                  GetSelectionsTasks(taskInstance.TaskInstanceID);

                foreach (var taskSelection in taskSelections)
                {
                    //   var pdm = PdmFactory.CreateSolidWorksPdmAdapter();               // add conditions: which of pdm systems will be initialised. 
                    //recomended using  PdmType with namespace PDM { SolidWorksPdm, Ips }

                    //var fileModel = SolidWorksPdmAdapter.Instance.GetFileById((int)eachTaskSelections.DocumentID, true);


                    //string pathToTempFile = 

                    //  string pathToPdmFile = (pdm as SolidWorksPdmAdapter).AddToPdm(pathToTempFile, dataModel.FolderPath);

                    //  (pdm as SolidWorksPdmAdapter).CheckInOutPdm(pathToPdmFile, true);
                }

                TaskSystemDataRepository.Instance.ApplyCompleted(taskInstance.TaskInstanceID);
            }
            catch //(Exception exception)
            {
              //  Console.WriteLine(exception.ToString());
                TaskSystemDataRepository.Instance.ApplyError(taskInstance.TaskInstanceID);
            }
        }
    }
}
