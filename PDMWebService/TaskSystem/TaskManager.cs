using PDMWebService.Data.PDM;
//using PDMWebService.Data.Solid.Dxf;
using PDMWebService.Data.Solid.Pdf;
using PDMWebService.TaskSystem.Data;
using ServiceLibrary.TaskSystem.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceLibrary.DataContracts;
using SolidWorksLibrary.Builders.Dxf;

class TaskManager : PDMWebService.Singleton.AbstractSingeton<TaskManager>
{
    private DbModelDataContext _dataBaseNodel;

    /// <summary>
    /// Data base classes model.
    /// </summary>
    private DbModelDataContext DataBaseModel
    {
        get
        {
            if (_dataBaseNodel == null)
                _dataBaseNodel = new DbModelDataContext();
            return _dataBaseNodel;
        }

    }
    IPdmAdapter _pdm;
    IPdmAdapter pdm
    {
        get
        {
            if (_pdm == null)
                _pdm = PdmFactory.CreateSolidWorksPdmAdapter(); // add conditions: which of pdm systems will be initialised.  
            return _pdm;

        }
    }
    private TaskManager() : base()
    {
      //  DxfBulder.Instance.ToSql += Instance_ToSql;

      
    }

    #region create task of generetion
    public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId = 0)
    {
        //Context.CreateTaskVibroInserion((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.VibroInsertion);
        //Context.SubmitChanges();
        //if (!ExistTaskToExecute())
        //    Execute();
    }
    public void CreateRoof(int height, int wight, RoofTypes type, int userId = 0)
    {
        //Context.CreateTaskRoof((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Roof);
        //Context.SubmitChanges();
        //if (!ExistTaskToExecute())
        //    Execute();
    }
    public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
    {
        //Context.CreateFlap((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Flap, (int)material, Convert.ToByte(isOuter), thickness);
        //Context.SubmitChanges();
        //if (!ExistTaskToExecute())
        //    Execute();
    }

    #region not ported panel builder..
    public void CreatePanel() { 
    //    PanelProfiles profil,
    //    PanelTypes type,
    //    int height,
    //    int wight,
    //    Meterials outerMaterial,
    //    Meterials innerMaterial,
    //    double thicknessInnerMaterial = 0.7f,
    //    double thicknessOterMaterial = 0.7f,
    //    int userId = 0
    //    )
    //{

    //    Context.CreatePanel
    //        (
    //            (int)TaskStatuses.Waiting,
    //            (int)TasksTypes.Panel,
    //            userId,
    //            DateTime.Today,
    //            (int)profil,
    //            (int)type, wight,
    //            height,
    //            (int)outerMaterial,
    //            (int)innerMaterial,
    //            thicknessInnerMaterial,
    //            thicknessOterMaterial
    //        );

    //    Context.SubmitChanges();

    //    //   if (!ExistTaskToExecute())
    //    Execute();
    }
    #endregion
    public void CreateMountingFrame()
    {
        //  AirCadExecutor.Instance.mou
    }
    #endregion

    public void CreatePdf(int[] arrayDocumentId, int userId = 0)
    {
        int taskInstanceId = 0;
        try
        {
         taskInstanceId = this.DataBaseModel.Tasks_SetTaskInstance((int)TasksType.Pdf, 100500);
            foreach (var eachDocumentId in arrayDocumentId)
            {
                this.DataBaseModel.Tasks_SetTaskSelection(taskInstanceId, eachDocumentId);
            }
            this.DataBaseModel.SubmitChanges();
        }
        catch(Exception ex)
        {
            if (taskInstanceId != 0)
                ApplyError(taskInstanceId);
            Console.WriteLine("Filed Create PDF Task on TaskManager level: " + ex.ToString());
        }
        if (!ExistExecutingTask())
        {
            Execute();
        }
    }


    public void CreateDxf(int[] arrayDocumentId, int userId = 0)
    {
        int taskInstanceId = 0;
        try
        {
             taskInstanceId = this.DataBaseModel.Tasks_SetTaskInstance((int)TasksType.Dxf, 100500);
            foreach (var eachDocumentId in arrayDocumentId)
            {
                this.DataBaseModel.Tasks_SetTaskSelection(taskInstanceId, eachDocumentId);
            }
            this.DataBaseModel.SubmitChanges();
        }
        catch(Exception ex)
        {
            if (taskInstanceId != 0)
                ApplyError(taskInstanceId);
            Console.WriteLine("Filed Create PDF Task on TaskManager level: " + ex.ToString());
        }
        if (!ExistExecutingTask())
        {
            Console.WriteLine("Exist tasks in waiting status");
            Execute();
        }
    }

    private void Execute()
    { 
        TaskInstance taskInstance;
        if (ExistWaitingTasks() && !ExistExecutingTask())
        {

            Console.WriteLine("Not exists task in execution");
            taskInstance = DataBaseModel.TaskInstances.First(tskInst => tskInst.TaskStatus == (int)TaskStatus.Waiting);
            Console.WriteLine("Get tasks with waiting status");
            ApplyExecution(taskInstance.TaskInstanceID);
           // taskInstance = DataBaseModel.TaskInstances.First(each => each.TaskInstanceID == taskInstance.TaskInstanceID);
        }
        else  
        {
            Console.WriteLine("In queue is not  tasks to perform and waiting. Possibly incorrect saving a task");
            throw new Exception("In queue is not  tasks to perform and waiting. Possibly incorrect saving a task");             
        }
        Console.WriteLine("Got task instance: " + taskInstance.TaskInstanceID + "; STATUS " + taskInstance.TaskStatus + " ~ " + (TaskStatus)taskInstance.TaskStatus);

        if (taskInstance != null && taskInstance.TaskStatus == (int)TaskStatus.Execution)
        { 
            switch (taskInstance.TaskID) // check task type
            {
                #region generetion
                //case (int)TasksTypes.VibroInsertion:
                //    VibroInsertion vibroInsertion = Context.VibroInsertions.First(eachVbrInsert => eachVbrInsert.Id == taskInstance.DataTaskId);
                //    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                //    try
                //    {
                //        VibroInsertionBuilder builder = new VibroInsertionBuilder();
                //        var pathToConveredFile = builder.Build(vibroInsertion.TypeVibroInsert.ToString(), vibroInsertion.Width.ToString(), vibroInsertion.Height.ToString());
                //        try
                //        {
                //            Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена...\nпуть к файлу: " + pathToConveredFile + "\n");
                //            ApplyCompleted(taskInstance.Id);
                //        }
                //        catch (Exception exception)
                //        {
                //            Console.WriteLine("Неудалось скопировать путь к сгенерированому файлу " + exception.ToString() + "\n");
                //        }


                //        Console.WriteLine("task id: " + taskInstance.Id + " type task: " + (TasksTypes)taskInstance.TypeTask + " data: [" + vibroInsertion.Height + "," + vibroInsertion.Width + "]");

                //    }
                //    catch (Exception exception)
                //    {
                //        ApplyError(taskInstance.Id);
                //        // save the cause of the error in log & simplified in data base
                //        Console.WriteLine("Error:\n" + exception.ToString());
                //    }
                //    break;

                //case (int)TasksTypes.Roof:
                //    Roof roof = Context.Roofs.First(eachRoof => eachRoof.Id == taskInstance.DataTaskId);

                //    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);

                //    try
                //    {
                //        AirCadExecutor.Instance.Roof(roof.RoofType.ToString(), roof.Width.ToString(), roof.Height.ToString());
                //        ApplyCompleted(taskInstance.Id);
                //        Console.WriteLine(taskInstance.Id + " " + taskInstance.TypeTask + " " + roof.Height + " " + roof.Width);
                //        Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена");
                //    }
                //    catch (Exception exception)
                //    {
                //        ApplyError(taskInstance.Id);
                //        // save the cause of the error in log & simplified in data base
                //        Console.WriteLine("Error:\n" + exception.ToString());
                //    }
                //    break;

                //case (int)TasksTypes.Flap:

                //    Flap flap = Context.Flaps.First(each => each.Id == taskInstance.DataTaskId);
                //    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                //    AirCadExecutor.Instance.Dumper(
                //        flap.FlapType.ToString(),
                //        flap.Width.ToString(),
                //        flap.Height.ToString(),
                //         flap.isOuter == 1 ? true : false,
                //         new string[] { flap.MaterialId.ToString(), flap.Thickness.ToString() }
                //         );
                //    Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена");
                //    ApplyCompleted(taskInstance.Id);
                //    break;

                //case (int)TasksTypes.Panel:
                //    //    AirCadExecutor.Instance.Panels30Build();
                //    Console.WriteLine("Эмуляция генерирования панели. Time out: 5 seconds");
                //    System.Threading.Thread.Sleep(5000);
                //    break;
                #endregion

                #region dxf
                case (int)TasksType.Dxf:
                    try
                    {

                        Console.WriteLine("Execute dxf task");
                        IEnumerable<TaskSelection> taskSelections = DataBaseModel.TaskSelections
                            .Where(eachTaskSelection => eachTaskSelection.TaskInstanceID == taskInstance.TaskInstanceID); 
                        foreach (var eachTaskSelections in taskSelections)
                        {                  
                            var dataModel = pdm.GetFileById((int)eachTaskSelections.DocumentID, true); // get file data and download    
                              //   DxfBulder.Instance.Build(dataModel );
                            Patterns.Observer.MessageObserver.Instance.ReceivedMessage += delegate (Patterns.Observer.MessageEventArgs e) { if (e.Type == Patterns.Observer.MessageType.Error) ApplyError(taskInstance.TaskInstanceID); Console.WriteLine(e.Message); };
                            DxfBulder.Instance.Build(dataModel.Path,dataModel.Id,dataModel.CurrentVersion);

                        }
                        ApplyCompleted(taskInstance.TaskInstanceID);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                     ApplyError(taskInstance.TaskInstanceID);
                    }
                    break;
                #endregion

                #region pdf

                case (int)TasksType.Pdf:
                    try
                    {

                        Console.WriteLine("Execute pdf task");

                        IEnumerable<TaskSelection> taskSelections = DataBaseModel.TaskSelections.Where(eachTaskSelection => eachTaskSelection.TaskInstanceID == taskInstance.TaskInstanceID);
                        foreach (var taskSelection in taskSelections)
                        { 
                            var pdm = PdmFactory.CreateSolidWorksPdmAdapter();               // add conditions: which of pdm systems will be initialised. 
                                                                                             //recomended using  PdmType with namespace PDM { SolidWorksPdm, Ips }

                            var dataModel = pdm.GetFileById((int)taskSelection.DocumentID, true); // get file data and download


                            string pathToTempFile = PdfBuilder.Instance.Build(dataModel);

                            string pathToPdmFile = (pdm as SolidWorksPdmAdapter).AddToPdm(pathToTempFile, dataModel.FolderPath);

                            (pdm as SolidWorksPdmAdapter).CheckInOutPdm(pathToPdmFile, true);
                        }

                        ApplyCompleted(taskInstance.TaskInstanceID);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        ApplyError(taskInstance.TaskInstanceID);
                    }
                    break;
                    #endregion
            }

        }
        taskInstance = null;
        if (this.ExistWaitingTasks())
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
            PDMWebService.Data.SqlData.PartData.Database.AddDxf(eachDxf.FilePath, eachDxf.IdPdm, eachDxf.Configuration, eachDxf.Version, out exception);
        }
    }







    #region Apply statuses of the tasks

    private void ApplyError(int taskInstanceId)
    {
        Console.WriteLine("Apply Error " + taskInstanceId);
        this.DataBaseModel.Tasks_SetTaskStatus((int)TaskStatus.Error, taskInstanceId);
        DataBaseModel.SubmitChanges();
    }


    private void ApplyCompleted(int taskInstanceId)
    {
        Console.WriteLine("Apply Completed" + taskInstanceId);
        this.DataBaseModel.Tasks_SetTaskStatus((int)TaskStatus.Completed, taskInstanceId);
        DataBaseModel.SubmitChanges();
    }

    private void ApplyWaiting(int taskInstanceId)
    {
        Console.WriteLine("Apply Waiting " + taskInstanceId);
        this.DataBaseModel.Tasks_SetTaskStatus((int)TaskStatus.Waiting, taskInstanceId);
        DataBaseModel.SubmitChanges();
    }


    private void ApplyExecution(int taskInstanceId)
    {
        Console.WriteLine("Apply Execution " + taskInstanceId);

        this.DataBaseModel.Tasks_SetTaskStatus((int)TaskStatus.Execution, taskInstanceId);
        DataBaseModel.SubmitChanges();
        DataBaseModel.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DataBaseModel.TaskInstances);
        Console.WriteLine("status after update  "+ DataBaseModel.TaskInstances.First(x => x.TaskInstanceID == taskInstanceId).TaskStatus);
    
    }
    #endregion

    #region exist  
    private bool ExistExecutingTask()
    {
        int counExecutingTasks = DataBaseModel.View_ActiveTasks.Where(eachTask => eachTask.TaskStatus == (int)TaskStatus.Execution).Count();
     
        return counExecutingTasks > 0 ? true : false;
    }

    private bool ExistWaitingTasks()
    {
      int countWaitingTasks =  DataBaseModel.View_ActiveTasks.Where(eachTask => eachTask.TaskStatus == (int)TaskStatus.Waiting).Count();
        return countWaitingTasks > 0 ? true : false;
    }


    #endregion

    public TaskData[] GetActiveTasksData( )
    {         
        List<TaskData> TaskDataList = new List<TaskData>();   
        var activeTasks = DataBaseModel.View_ActiveTasks; 
        foreach (var task in activeTasks)
        { 
            TaskDataList.Add(
                new TaskData()
                {
                    TaskId = task.TaskInstanceID,
                    Type = (int)task.TaskID,
                    Status = task.TaskStatus,
                    UserId = task.InitUserID
                });
 
        }
        return TaskDataList.ToArray();
      
    }

    public TaskData[] GetComplitedTasksData()
    {
        List<TaskData> TaskDataList = new List<TaskData>();
        var completedTasks = DataBaseModel.View_CompletedTasks;
        Console.WriteLine("Count copmleted tasks"+completedTasks.Count());
        foreach (var task in completedTasks)
        { 
            TaskDataList.Add(
               new TaskData()
               {
                   TaskId = task.TaskInstanceID,
                   Type = (int)task.TaskID,
                   Status = task.TaskStatus,
                   UserId = task.InitUserID
               });
            Console.WriteLine("Our iteration (GetComplitedTasksData)");
        }
        Console.WriteLine("We returns our completed tasks");
        return TaskDataList.ToArray();
    }
}