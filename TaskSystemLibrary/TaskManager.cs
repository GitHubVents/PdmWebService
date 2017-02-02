
using DataBaseDomian;
using Patterns;
using ServiceLibrary.TaskSystem.Constants;
using SolidWorksLibrary.Builders.Dxf;
using System;
using System.Collections.Generic;

namespace TaskSystemLibrary
{
    public class TaskManager : Singeton<TaskManager>
    { 
        private TaskManager() : base(){}

        /// <summary>
        ///  Add the new task at the which creates pdf  
        /// </summary>
        /// <param name="arrayDocumentId"></param>
        /// <param name="userId"></param>
        public void CreatePdf(int[] arrayDocumentId, int userId = 0)
        {
            TaskSystemDataRepository.Instance.CretaPdfTask(arrayDocumentId, userId);
            if (!TaskSystemDataRepository.Instance.ExistExecutingTask())
            {
                Execute();
            }
        }
        /// <summary>
        ///  Add the new task at the which creates dxf  
        /// </summary>
        /// <param name="arrayDocumentId"></param>
        /// <param name="userId"></param>
        public void CreateDxf(int[] arrayDocumentId, int userId = 0)
        {
            TaskSystemDataRepository.Instance.CreateDxfTask(arrayDocumentId, userId);
            if (!TaskSystemDataRepository.Instance.ExistExecutingTask())
            {
                Execute();
            }
        }

        /// <summary>
        /// Execute task waiting of the queue
        /// </summary>
        private void Execute()
        {
            DataBaseDomian.Orm.TaskInstance taskInstance = null;

            if (TaskSystemDataRepository.Instance.ExistWaitingTasks() &&
                !TaskSystemDataRepository.Instance.ExistExecutingTask())
            {
                taskInstance = TaskSystemDataRepository.Instance.GetWaitingTask();
                TaskSystemDataRepository.Instance.ApplyExecution(taskInstance.TaskInstanceID);
            }      

            if (taskInstance != null && taskInstance.TaskStatus == (int)TaskStatus.Execution)
            {
                switch (taskInstance.TaskID) // check task type
                {
                    #region dxf
                    case (int)TasksType.Dxf:
                        ExecuterFactory.ExecuteDxf(taskInstance);
                        break;
                    #endregion

                    #region pdf

                    case (int)TasksType.Pdf:
                        ExecuterFactory.ExecutePdf(taskInstance);
                        break;
                        #endregion
                }

            }
            taskInstance = null;
            if (TaskSystemDataRepository.Instance.ExistWaitingTasks())
            {
                Execute();
            }
        }



        private void Instance_ToSql(List<DxfFile> dxfList)
        {
            Exception exception;
            Console.WriteLine("Выгрузка данных в DXF");
            Console.WriteLine("Количество файлов для записив базу: " + dxfList.Count);
            foreach (var eachDxf in dxfList)
            {
                Console.WriteLine(eachDxf.ToString());
                //  PDMWebService.Data.SqlData.PartData.Database.AddDxf(eachDxf.FilePath, eachDxf.IdPdm, eachDxf.Configuration, eachDxf.Version, out exception);
            }
        }


        //public TaskData[] GetActiveTasksData()
        //{
        //    List<TaskData> TaskDataList = new List<TaskData>();
        //    var activeTasks = DataBaseModel.View_ActiveTasks;
        //    foreach (var task in activeTasks)
        //    {
        //        TaskDataList.Add(
        //            new TaskData()
        //            {
        //                TaskId = task.TaskInstanceID,
        //                Type = (int)task.TaskID,
        //                Status = task.TaskStatus,
        //                UserId = task.InitUserID
        //            });

        //    }
        //    return TaskDataList.ToArray();

        //}

        //public TaskData[] GetComplitedTasksData()
        //{
        //    List<TaskData> TaskDataList = new List<TaskData>();
        //    var completedTasks = DataBaseModel.View_CompletedTasks;
        //    Console.WriteLine("Count copmleted tasks" + completedTasks.Count());
        //    foreach (var task in completedTasks)
        //    {
        //        TaskDataList.Add(
        //           new TaskData()
        //           {
        //               TaskId = task.TaskInstanceID,
        //               Type = (int)task.TaskID,
        //               Status = task.TaskStatus,
        //               UserId = task.InitUserID
        //           });
        //        Console.WriteLine("Our iteration (GetComplitedTasksData)");
        //    }
        //    Console.WriteLine("We returns our completed tasks");
        //    return TaskDataList.ToArray();
        //}
    }
}