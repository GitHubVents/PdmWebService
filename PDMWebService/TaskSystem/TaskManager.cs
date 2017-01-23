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

 
class TaskManager : AbstractSingeton<TaskManager>
 {
    
    private  DbModelDataContext context;
   
    /// <summary>
    /// Data base classes model.
    /// </summary>
    private DbModelDataContext Context { get
        {
            if(context == null)
             context = new DbModelDataContext();
            return context;
        }

    }


    #region isExecute... not need because replaced on ExistTaskToExecute method
    //private static Boolean _isExecute;
    // private static object lockObject = new object();
    ///// <summary>
    ///// Status executing.
    ///// </summary>
    //private  static Boolean isExecute
    //{
    //    get
    //    {
    //        bool test = false;
    //        lock (lockObject)
    //        {
    //            test = _isExecute;
    //        }
    //        return test;
    //    }
    //    set
    //    {
    //        lock (lockObject)
    //        {
    //            _isExecute = value;

    //            Console.WriteLine(" Изменение состояния _isExecute");
    //        }
    //    }
    //}
    #endregion;

   Thread threadExecute;
    private TaskManager() : base()
    {
        threadExecute = new   Thread(Execute);
        
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
        Console.WriteLine("Created dxf task")   ;

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
                #region dxf-pdf
                case (int)TasksTypes.Dxf:
                    try
                    {
                        IEnumerable<DxfTarget> dxfTargets = Context.DxfTargets.Where(each => each.TaskId == taskInstance.Id);
                        foreach (var eachDxfTarget in dxfTargets)
                        {
                            Console.WriteLine(eachDxfTarget.IpPdm);
                            DxfBulder.Instance.Build((int)eachDxfTarget.IpPdm);
                        }
                        ApplyCompleted(taskInstance.Id);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                        ApplyError(taskInstance.Id);
                    }
                    break;

                case (int)TasksTypes.Pdf:
                    try
                    {
                        
                        IEnumerable<PdfTarget> pdfTargets = Context.PdfTargets.Where(each => each.TaskId == taskInstance.Id);
                        Console.WriteLine("Bild pdf array");
                        foreach (var eachPdfTarget in pdfTargets)
                        {
                            Console.WriteLine("Bild pdf with id #" + eachPdfTarget.IpPdm);
                            PdfBuilder.Instance.PdfFolder = @"D:\TEMP\pdf";
                            PdfBuilder.Instance.Build((int)eachPdfTarget.IpPdm);
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
}