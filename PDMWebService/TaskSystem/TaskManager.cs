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

    /// <summary>
    /// Generate Vibro panel without registering.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="wight"></param>
    /// <param name="typeVibroInsert"></param>
    /// <param name="userId"></param>
    public void CreateVibroInsertion(int height, int wight, VibroInsertionTypes type, int userId = 0)
    {
        int status = (int)TaskStatuses.Waiting;// (int)DetermineStatusOfTask();
        Console.WriteLine("Пришел новый таск и стутс ему леги... " + DetermineStatusOfTask().ToString());
     int createdTaskId = context.CreateTaskVibroInserion((int)type, wight, height, userId, status, DateTime.Today, (int)TasksTypes.VibroInsertion);
 
        Console.WriteLine("Id last adding task " + createdTaskId);
        context.SubmitChanges();
    
        if (!ExistTaskToExecute())
            Execute() ;
        
    }

    public void CreateRoof(int height, int wight, RoofTypes type, int userId = 0)
    {
        int status = (int)DetermineStatusOfTask();
        context.CreateTaskRoof((int)type, wight, height, userId, status, DateTime.Today, (int)TasksTypes.Roof);
        context.SubmitChanges();

      //  if (ExistTaskToExecute())
          //  Execute(0);
    }

    public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, int userId = 0)
    {
        int status = (int)DetermineStatusOfTask();
        context.CreateFlap((int)type, wight, height, userId, 0, DateTime.Today, (int)TasksTypes.Flap, (int)material, Convert.ToByte(isOuter), null);
        context.SubmitChanges();

     //   if (ExistTaskToExecute()) ;
        //    Execute();

    }

    public void CreateFlap(FlapTypes type, int height, int wight, Meterials material, bool isOuter, float thickness, int userId = 0)
    {
        int status = (int)DetermineStatusOfTask();
        context.CreateFlap((int)type, wight, height, userId, 0, DateTime.Today, (int)TasksTypes.Flap, (int)material, Convert.ToByte(isOuter), thickness);
        context.SubmitChanges();

      //  if (ExistTaskToExecute()) ;
          //  Execute();
    }

    public void CreatePanel(
        PanelProfiles profil,
        PanelTypes type,
        int height,
        int wight,
        Meterials outerMaterial,
        Meterials innerMaterial,
        double thicknessInnerMaterial = 0.7f,
        double thicknessOterMaterial = 0.7f
        )
    {
        int status = (int)DetermineStatusOfTask();
        context.CreatePanel
            (
                status,
                (int)TasksTypes.Panel,
                0,
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

        if (ExistTaskToExecute()) ; 
           // Execute();
    }

    public void CreateMountingFrame()
    {
        //  AirCadExecutor.Instance.mou
    }

    private void Execute(  )
    {
        TaskInstance taskInstance;
        if (!ExistTaskToExecute() && ExistWaitingTasks())
        {
            taskInstance = context.TaskInstances.First(  tskInst =>  tskInst.Status == (int)TaskStatuses.Waiting );
            ApplyExecution(taskInstance.Id);
        }
        //else if (ExistWaitingTasks() == true)
        //{
        //    taskInstance = context.TaskInstances.First(tskInst => tskInst.Status == (int)TaskStatuses.Waiting);
        //    this.ApplyExecution(taskInstance.Id);
        //}
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
                    try
                    {
                        //using (var ventsCad = new VentsCadLibrary.VentsCad())
                        //{
                        //    VentsCadLibrary.VentsCad.Spigot newSpigot = new VentsCadLibrary.VentsCad.Spigot(vibroInsert.TypeVibroInsert.ToString(), vibroInsert.Width.ToString(), vibroInsert.Height.ToString());
                        //    if (!newSpigot.Exist)
                        //    {
                        //        newSpigot.Build();
                        //    }
                        //    VentsCadLibrary.VentsCad.ProductPlace place2 = newSpigot.GetPlace();
                        //}

                        Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                        System.Threading.Thread.Sleep(5000);

                        ApplyCompleted(taskInstance.Id);
                        Console.WriteLine(taskInstance.Id + " " + taskInstance.TypeTask + " " + vibroInsert.Height + " " + vibroInsert.Width);

                        //System.IO.File.Create(@"C:\"+taskInstance.Id + " " + taskInstance.TypeTask + " " + vibroInsert.Height + " " + vibroInsert.Width + ".txt" );


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
                    System.Threading.Thread.Sleep(5000);

                    //AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw modelSw = new AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw();
                    //try
                    //{
                    //    AirCadExecutor.Instance.Roof(roof.RoofType.ToString(), roof.Width.ToString(), roof.Height.ToString());
                    //    System.Threading.Thread.Sleep(5000);
                    //    ApplyCompleted(taskInstance.Id);

                    //    Console.WriteLine(taskInstance.Id + " " + taskInstance.TypeTask + " " + roof.Height + " " + roof.Width);
                    //    //System.IO.File.Create(@"C:\" + taskInstance.Id + " " + taskInstance.TypeTask + " " + roof.Height + " " + roof.Width + ".txt");
                    //}
                    //catch (Exception ex)
                    //{
                    //    ApplyError(taskInstance.Id);
                    //    // save the cause of the error in log & simplified in data base
                    //    Console.WriteLine("Error:\n" + ex.ToString());
                    //}
                    break;

                case (int)TasksTypes.Flap:

                    Flap flap = context.Flaps.First(each => each.Id == taskInstance.DataTaskId);
                    Console.WriteLine("Выполнение таска: " + (TasksTypes)taskInstance.TypeTask);
                    System.Threading.Thread.Sleep(5000);
                    //AirCadExecutor.Instance.Dumper(
                    //    flap.FlapType.ToString(),
                    //    flap.Width.ToString(),
                    //    flap.Height.ToString(),
                    //     flap.isOuter == 1 ? true : false,
                    //     new string[] { flap.MaterialId.ToString(), flap.Thickness.ToString() }
                    //     );
                    break;

                    //case (int)TasksTypes.Panel:
                    //    AirCadExecutor.Instance.Panels30Build
                    //break;
            }

        }

        if (  this.ExistWaitingTasks())
        {
          
            Execute();

        }
        }

    #region Apply statuses of the tasks

    /// <summary>
    ///  Apply status error for task by id  
    /// </summary>
    /// <param name="id"></param>
    private void ApplyError(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Error;
        context.SubmitChanges();
    }

    /// <summary>
    ///  Apply status completed for task by id  
    /// </summary>
    private void ApplyCompleted(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Completed;
        context.SubmitChanges();
    }

    /// <summary>
    ///  Apply status waiting for task by id  
    /// </summary>
    private void ApplyWaiting(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Waiting;
       context.SubmitChanges();
    }

    /// <summary>
    /// Apply status execution for task by id  
    /// </summary>
    /// <param name="id"></param>
    private void ApplyExecution(int id)
    {
        context.TaskInstances.First(eachTask => eachTask.Id == id).Status = (int)TaskStatuses.Execution;
        context.SubmitChanges();

    }
    #endregion

    /// <summary>
    /// Check whether there are tasks to execute and returns true or false
    /// </summary>
    /// <returns></returns>
    private bool ExistTaskToExecute()
    {
        return this.context.ExistTaskToExecute() == 1 ? true : false;
    }

    private bool ExistWaitingTasks()
    {
        return this.context.ExistWaitingTasks() == 1 ? true : false;
    }

    private TaskStatuses DetermineStatusOfTask()
    {
         if (this.ExistTaskToExecute() == false)
        {
           return TaskStatuses.Execution;
        } 
        else  
        {
            return TaskStatuses.Waiting;
        }      

       
    }

}