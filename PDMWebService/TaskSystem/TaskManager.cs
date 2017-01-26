using PDMWebService.Data.PDM;
using PDMWebService.Data.Solid.Dxf;
using PDMWebService.Data.Solid.PartBuilders;
using PDMWebService.Data.Solid.Pdf;
using PDMWebService.Singleton;
using PDMWebService.TaskSystem;
using PDMWebService.TaskSystem.Data;
using ServiceLibrary.TaskSystem.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PDM_WebService.WcfServiceLibrary;
using ServiceLibrary.DataContracts;

class TaskManager : AbstractSingeton<TaskManager>, ITaskSystemMonitor
{

    private DbModelDataContext context;

    /// <summary>
    /// Data base classes model.
    /// </summary>
    private DbModelDataContext Context
    {
        get
        {
            if (context == null)
                context = new DbModelDataContext();
            return context;
        }

    }

    private TaskManager() : base()
    {

    }

    #region create task of generetion
    public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId = 0)
    {
        Context.CreateTaskVibroInserion((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.VibroInsertion);
        Context.SubmitChanges();
        if (!ExistTaskToExecute())
            Execute();
    }
    public void CreateRoof(int height, int wight, RoofTypes type, int userId = 0)
    {
        Context.CreateTaskRoof((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Roof);
        Context.SubmitChanges();
        if (!ExistTaskToExecute())
            Execute();
    }
    public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
    {
        Context.CreateFlap((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Flap, (int)material, Convert.ToByte(isOuter), thickness);
        Context.SubmitChanges();
        if (!ExistTaskToExecute())
            Execute();
    }
    #region not ported panel builder..
    public void CreatePanel(
        PanelProfiles profil,
        PanelTypes type,
        int height,
        int wight,
        Meterials outerMaterial,
        Meterials innerMaterial,
        double thicknessInnerMaterial = 0.7f,
        double thicknessOterMaterial = 0.7f,
        int userId = 0
        )
    {

        Context.CreatePanel
            (
                (int)TaskStatuses.Waiting,
                (int)TasksTypes.Panel,
                userId,
                DateTime.Today,
                (int)profil,
                (int)type, wight,
                height,
                (int)outerMaterial,
                (int)innerMaterial,
                thicknessInnerMaterial,
                thicknessOterMaterial
            );

        Context.SubmitChanges();

        //   if (!ExistTaskToExecute())
        Execute();
    }
    #endregion
    public void CreateMountingFrame()
    {
        //  AirCadExecutor.Instance.mou
    }
    #endregion
    public void CreatePdf(int[] idPdmArr, int userId = 0)
    {

        Console.WriteLine("Created pdf task");

        int TaskId = Context.CreateTaskInstance(userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Pdf);
        foreach (var idPdm in idPdmArr)
        {
            Context.CreatePdf(idPdm, TaskId);
        }

        Context.SubmitChanges();
        if (!ExistTaskToExecute())
        {
            //threadExecute.Start();
            Execute();
        }
    }


    public void CreateDxf(int[] idPdmArr, int userId = 0)
    {
        Console.WriteLine("Created dxf task");

        int TaskId = Context.CreateTaskInstance(userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Dxf);
        foreach (var idPdm in idPdmArr)
        {
            Context.CreateDxf(idPdm, TaskId);
        }

        Context.SubmitChanges();
        if (!ExistTaskToExecute())
        {
            //threadExecute.Start();
            Execute();
        }
    }

    private void Execute()
    {
        Console.WriteLine("Created == PDF == task");
        TaskInstance taskInstance;
        if (!ExistTaskToExecute() && ExistWaitingTasks())
        {
            taskInstance = Context.TaskInstances.First(tskInst => tskInst.Status == (int)TaskStatuses.Waiting);
            ApplyExecution(taskInstance.Id);
        }
        else
        {
            throw new Exception("In queue is not  tasks to perform and waiting. Possibly incorrect saving a task");
        }

        if (taskInstance != null && taskInstance.Status == (int)TaskStatuses.Execution)
        {
            switch (taskInstance.TypeTask)
            {
                #region generetion
                case (int)TasksTypes.VibroInsertion:
                    VibroInsertion vibroInsertion = Context.VibroInsertions.First(eachVbrInsert => eachVbrInsert.Id == taskInstance.DataTaskId);
                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                    try
                    {
                        VibroInsertionBuilder builder = new VibroInsertionBuilder();
                        var pathToConveredFile = builder.Build(vibroInsertion.TypeVibroInsert.ToString(), vibroInsertion.Width.ToString(), vibroInsertion.Height.ToString());
                        try
                        {
                            Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена...\nпуть к файлу: " + pathToConveredFile + "\n");
                            ApplyCompleted(taskInstance.Id);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("Неудалось скопировать путь к сгенерированому файлу " + exception.ToString() + "\n");
                        }


                        Console.WriteLine("task id: " + taskInstance.Id + " type task: " + (TasksTypes)taskInstance.TypeTask + " data: [" + vibroInsertion.Height + "," + vibroInsertion.Width + "]");

                    }
                    catch (Exception exception)
                    {
                        ApplyError(taskInstance.Id);
                        // save the cause of the error in log & simplified in data base
                        Console.WriteLine("Error:\n" + exception.ToString());
                    }
                    break;

                case (int)TasksTypes.Roof:
                    Roof roof = Context.Roofs.First(eachRoof => eachRoof.Id == taskInstance.DataTaskId);

                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);

                    try
                    {
                        AirCadExecutor.Instance.Roof(roof.RoofType.ToString(), roof.Width.ToString(), roof.Height.ToString());
                        ApplyCompleted(taskInstance.Id);
                        Console.WriteLine(taskInstance.Id + " " + taskInstance.TypeTask + " " + roof.Height + " " + roof.Width);
                        Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена");
                    }
                    catch (Exception exception)
                    {
                        ApplyError(taskInstance.Id);
                        // save the cause of the error in log & simplified in data base
                        Console.WriteLine("Error:\n" + exception.ToString());
                    }
                    break;

                case (int)TasksTypes.Flap:

                    Flap flap = Context.Flaps.First(each => each.Id == taskInstance.DataTaskId);
                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                    AirCadExecutor.Instance.Dumper(
                        flap.FlapType.ToString(),
                        flap.Width.ToString(),
                        flap.Height.ToString(),
                         flap.isOuter == 1 ? true : false,
                         new string[] { flap.MaterialId.ToString(), flap.Thickness.ToString() }
                         );
                    Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена");
                    ApplyCompleted(taskInstance.Id);
                    break;

                case (int)TasksTypes.Panel:
                    //    AirCadExecutor.Instance.Panels30Build();
                    Console.WriteLine("Эмуляция генерирования панели. Time out: 5 seconds");
                    System.Threading.Thread.Sleep(5000);
                    break;
                #endregion

                #region dxf
                case (int)TasksTypes.Dxf:
                    try
                    {
                        IEnumerable<DxfTarget> dxfTargets = Context.DxfTargets.Where(each => each.TaskId == taskInstance.Id);
                        foreach (var eachDxfTarget in dxfTargets)
                        {
                            var pdm = PdmFactory.CreateSolidWorksPdmAdapter(); // add conditions: which of pdm systems will be initialised.                          
                            var dataModel = pdm.GetFileById(eachDxfTarget.IpPdm, true); // get file data and download                           
                            var configrations = pdm.GetConfigigurations(dataModel);
                            DxfBulder.Instance.ToSql += Instance_ToSql;

                            DxfBulder.Instance.Build(dataModel, configrations);
                        }
                        ApplyCompleted(taskInstance.Id);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        ApplyError(taskInstance.Id);
                    }
                    break;
                #endregion

                #region pdf

                case (int)TasksTypes.Pdf:
                    try
                    {
                        IEnumerable<PdfTarget> pdfTargets = Context.PdfTargets.Where(each => each.TaskId == taskInstance.Id);
                        foreach (var eachPdfTarget in pdfTargets)
                        {
                            //PdfBuilder.Instance.PdfFolder = @"D:\TEMP\pdf";
                            var pdm = PdmFactory.CreateSolidWorksPdmAdapter();               // add conditions: which of pdm systems will be initialised. 
                                                                                             //recomended using  PdmType with namespace PDM { SolidWorksPdm, Ips }

                            var dataModel = pdm.GetFileById((int)eachPdfTarget.IpPdm, true); // get file data and download


                            string pathToTempFile = PdfBuilder.Instance.Build(dataModel);

                            string pathToPdmFile = (pdm as SolidWorksPdmAdapter).AddToPdm(pathToTempFile, dataModel.FolderPath);

                            (pdm as SolidWorksPdmAdapter).CheckInOutPdm(pathToPdmFile, true);
                        }

                        ApplyCompleted(taskInstance.Id);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        ApplyError(taskInstance.Id);
                    }
                    break;
                    #endregion
            }

        }

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

    private void ApplyError(int id)
    {
        Context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Error;
        Context.SubmitChanges();
    }


    private void ApplyCompleted(int id)
    {
        Context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Completed;
        Context.SubmitChanges();
    }

    private void ApplyWaiting(int id)
    {
        Context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Waiting;
        Context.SubmitChanges();
    }


    private void ApplyExecution(int id)
    {
        Context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Execution;
        Context.SubmitChanges();

    }
    #endregion

    #region exist 
    private bool ExistTaskToExecute()
    {
        return this.Context.ExistTaskToExecute() == 1 ? true : false;
    }

    private bool ExistWaitingTasks()
    {
        return this.Context.ExistWaitingTasks() == 1 ? true : false;
    }


    #endregion

    public TaskData[] GetTasksData(int userId, TasksTypes type, TaskStatuses status)
    {
        List<TaskData> TaskDataList = new List<TaskData>();
        int countTasks = context.TaskInstances.Count();
        var tasks = context.TaskInstances.Where(eachTask => eachTask.Id >= countTasks - 10);
        var pdm = PdmFactory.CreateSolidWorksPdmAdapter() as SolidWorksPdmAdapter; // temp

        foreach (var task in tasks)
        {
            switch (task.TypeTask)
            {
                case (int)TasksTypes.Dxf:
                    var dxfDataList = context.DxfTargets;
                    var taskData = new TaskData()
                    {
                        TaskId = task.Id,
                        Status = (TaskStatuses)task.Status,
                        type = (TasksTypes)task.TypeTask,
                        User = task.UserId,

                        //  Designation = pdm.GetFileById(eachDxf.IpPdm, false).FileName;
                    };
                    List<string> dxf_designations = new List<string>();
                    foreach (var eachDxf in dxfDataList)
                    {
                        dxf_designations.Add(pdm.GetFileById(eachDxf.IpPdm, false).FileName);
                    }
                    taskData.Designation = dxf_designations.ToArray();

                    break;
                case (int)TasksTypes.Pdf:
                    var pdfDataList = context.PdfTargets;
                    var pdf_taskData = new TaskData()
                    {
                        TaskId = task.Id,
                        Status = (TaskStatuses)task.Status,
                        type = (TasksTypes)task.TypeTask,
                        User = task.UserId,

                        //  Designation = pdm.GetFileById(eachDxf.IpPdm, false).FileName;
                    };
                    List<string> pdf_designations = new List<string>();
                    foreach (var eachPdf in pdfDataList)
                    {
                        pdf_designations.Add(pdm.GetFileById((int)eachPdf.IpPdm, false).FileName);
                    }
                    pdf_taskData.Designation = pdf_designations.ToArray();
                    break;
            }
        }

        return TaskDataList.ToArray();
    }
}