using PDMWebService.Singleton;
using PDMWebService.TaskSystem;
using PDMWebService.TaskSystem.Data;
using ServiceLibrary.TaskSystem.Constants;
using System;
using System.Linq;

class TaskManager : AbstractSingeton<TaskManager>
{
    /// <summary>
    /// Data base classes model.
    /// </summary>
    private DbModelDataContext context { get; set; }


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

    private TaskManager() : base()
    {
        this.context = new DbModelDataContext();
    }
  
    public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId = 0)
    {  
        context.CreateTaskVibroInserion((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.VibroInsertion);
        context.SubmitChanges();    
         if (!ExistTaskToExecute())
            Execute() ;        
    }

    public void CreateRoof(int height, int wight, RoofTypes type, int userId = 0)
    {  
        context.CreateTaskRoof((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Roof);
        context.SubmitChanges();
       if (!ExistTaskToExecute())
            Execute();
    }

 
    public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
    { 
        context.CreateFlap((int)type, wight, height, userId, (int)TaskStatuses.Waiting, DateTime.Today, (int)TasksTypes.Flap, (int)material, Convert.ToByte(isOuter), thickness);
        context.SubmitChanges(); 
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
     
        context.CreatePanel
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

        context.SubmitChanges();

     //   if (!ExistTaskToExecute())
            Execute();
    }
    #endregion

    public void CreateMountingFrame()
    {
        //  AirCadExecutor.Instance.mou
    }


    public void CreatePdf (int idPdm, int userId = 0)
    {
        context.CreatePdf(idPdm, userId,(int)TaskStatuses.Waiting,  DateTime.Today, (int)TasksTypes.Pdf);
        context.SubmitChanges();
        if (!ExistTaskToExecute())
            Execute();
    }

    public void CreateDxf(int idPdm, int userId = 0)
    {
        Console.WriteLine("dxf");
        context.CreateDxf(idPdm, userId,  (int)TaskStatuses.Waiting , DateTime.Today, (int)TasksTypes.Dxf);
        context.SubmitChanges();
        if (!ExistTaskToExecute())
            Execute();
    }


    private void Execute(  )
    {
        TaskInstance taskInstance;
        if (!ExistTaskToExecute() && ExistWaitingTasks())
        {
            taskInstance = context.TaskInstances.First(  tskInst =>  tskInst.Status == (int)TaskStatuses.Waiting );
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
                case (int)TasksTypes.VibroInsertion:
                    VibroInsertion vibroInsert = context.VibroInsertions.First(eachVbrInsert => eachVbrInsert.Id == taskInstance.DataTaskId);
                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                    try
                    {

                        //  AirCadExecutor.Instance.Spigot(vibroInsert.TypeVibroInsert.ToString(), vibroInsert.Width.ToString(), vibroInsert.Height.ToString());
                     //   string str =   AirCadExecutor.Instance.SpigotStr(vibroInsert.TypeVibroInsert.ToString(), vibroInsert.Width.ToString(), vibroInsert.Height.ToString());
                       PDMWebService.Data.Solid.PartBuilders.VibroInsertionBuilder builder = new PDMWebService.Data.Solid.PartBuilders.VibroInsertionBuilder();
                        string str = builder.Build(vibroInsert.TypeVibroInsert.ToString(), vibroInsert.Width.ToString(), vibroInsert.Height.ToString());

                        try
                        {
                            //   path = newSpigot.GetPlace().Path.Clone() as string;                         
                            // newSpigot = null;
                            Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена...\nпуть к файлу: " + str + "\n");
                            ApplyCompleted(taskInstance.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Неудалось скопировать путь к сгенерированому файлу ");// + ex.ToString() + "\n");
                        }

                     
                        Console.WriteLine("task id: "+ taskInstance.Id + " type task: " + (TasksTypes)taskInstance.TypeTask + " data: [" + vibroInsert.Height + "," + vibroInsert.Width+"]"); 

                    }
                    catch (Exception ex)
                    {
                        ApplyError(taskInstance.Id);
                        // save the cause of the error in log & simplified in data base
                        Console.WriteLine("Error:\n" + ex.ToString());
                    }
                    break;

                case (int)TasksTypes.Roof:
                    Roof roof = context.Roofs.First(eachRoof => eachRoof.Id == taskInstance.DataTaskId);

                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
            
                    try
                    {
                        AirCadExecutor.Instance.Roof(roof.RoofType.ToString(), roof.Width.ToString(), roof.Height.ToString());
                        ApplyCompleted(taskInstance.Id);
                        Console.WriteLine(taskInstance.Id + " " + taskInstance.TypeTask + " " + roof.Height + " " + roof.Width);
                        Console.WriteLine("Генерация " + (TasksTypes)taskInstance.TypeTask + " завершена");
                    }
                    catch (Exception ex)
                    {
                        ApplyError(taskInstance.Id);
                        // save the cause of the error in log & simplified in data base
                        Console.WriteLine("Error:\n" + ex.ToString());
                    }
                    break;

                case (int)TasksTypes.Flap:

                    Flap flap = context.Flaps.First(each => each.Id == taskInstance.DataTaskId);
                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                 //   System.Threading.Thread.Sleep(5000);
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

                case (int)TasksTypes.Dxf:
                    try
                    {
                        DxfTarget dxfTarget = context.DxfTargets.First(each => each.Id == taskInstance.DataTaskId);
                        PDMWebService.Data.Solid.SolidWorksInstance.ConvertToDXF((int)dxfTarget.IpPdm);
                        ApplyCompleted(taskInstance.Id);
                    }
                    catch ( Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        ApplyError(taskInstance.Id);
                    }
                    break;

                case (int)TasksTypes.Pdf:
                    PdfTarget pdfTarget = context.PdfTargets.First(each => each.Id == taskInstance.DataTaskId);
                    
                    ApplyCompleted(taskInstance.Id);
                    break;
            }

        }

        if (  this.ExistWaitingTasks())
        {
          
            Execute();

        }
        }

    #region Apply statuses of the tasks
 
    private void ApplyError(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Error;
        context.SubmitChanges();
    }

 
    private void ApplyCompleted(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Completed;
        context.SubmitChanges();
    }
 
    private void ApplyWaiting(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Waiting;
       context.SubmitChanges();
    }

    
    private void ApplyExecution(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Execution;
        context.SubmitChanges();

    }
    #endregion

 
    private bool ExistTaskToExecute()
    {
        return this.context.ExistTaskToExecute() == 1 ? true : false;
    }

    private bool ExistWaitingTasks()
    {
        return this.context.ExistWaitingTasks() == 1 ? true : false;
    } 
}