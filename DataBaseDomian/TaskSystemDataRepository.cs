using Patterns;
using System;
using DataBaseDomian.Orm;
using Patterns.Observer;
using System.Linq;
using System.Collections.Generic;
using ServiceConstants;

namespace DataBaseDomian
{
    /// <summary>
    /// Provides acces to tasks data base tasks 
    /// </summary>
    public class TaskSystemDataRepository : Singeton<TaskSystemDataRepository>, IUpdater
    {
        /// <summary>
        /// Exemplar of an object oriented database model
        /// </summary>
        private TasksSystemDataContext _dataContext;

        /// <summary>
        /// Property-getter for exemplar of an object oriented database model 
        /// </summary>
        private TasksSystemDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new TasksSystemDataContext();

                    MessageObserver.Instance.SetMessage("Open connection to TasksSystem data base", MessageType.System);
                }
                return _dataContext;
            }
        }
        /// <summary>
        /// Returns the active tasks that is to tasks with status waiting or execute 
        /// </summary>
        public IEnumerable< View_ActiveTask > ActiveTasks
        {
            get
            {
               
                return DataContext.View_ActiveTasks;
            }
        }
        /// <summary>
        /// Returns the completed tasks that is to tasks with status completed or error 
        /// </summary>
        public IEnumerable<View_CompletedTask> CompletedTasks
        {
            get
            {
             
                return DataContext.View_CompletedTasks;
            }
        }

        /// <summary>
        /// Returns the summary count of tasks instance
        /// </summary>
        public long CountTasks
        {
            get
            {
             
                return DataContext.TaskInstances.LongCount();
            }
        }
        /// <summary>
        /// Returns the count of selections tasks by taskInstance Id
        /// </summary>
        /// <param name="taskInstanceId"> Task Instance id</param>
        /// <returns></returns>
        public long CountSelectionsTasks(int taskInstanceId)
        {
            
            return DataContext.TaskSelections.Where(eachSelectionTask => eachSelectionTask.TaskInstanceID == taskInstanceId).LongCount();

        }


        protected TaskSystemDataRepository() : base() { }

        /// <summary>
        /// Adds new pdf task to data base. 
        /// </summary>
        /// <param name="arrayDocumentId"></param>
        /// <param name="userId"></param>
        public int CretaPdfTask(int[] arrayDocumentId, int userId = 0)
        {
            

          //  MessageObserver.Instance.SetMessage("Add new task to data base { " + TasksType.Pdf + " }", MessageType.System);

            int taskInstanceId = 0;
            try
            {
                taskInstanceId = this.DataContext.Tasks_SetTaskInstance((int)TasksType_e.Pdf, userId);
                MessageObserver.Instance.SetMessage("Created task instance with id " + taskInstanceId, MessageType.System);

                foreach (var eachDocumentId in arrayDocumentId)
                {
                    int taskSelectionId = this.DataContext.Tasks_SetTaskSelection(taskInstanceId, eachDocumentId);
                    MessageObserver.Instance.SetMessage("Created TaskSelection with id " + taskSelectionId, MessageType.System);
                }
                this.DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                if (taskInstanceId != 0)
                {
                    MessageObserver.Instance.SetMessage("Failed added new task with id " + taskInstanceId + "; message { " + ex + " }", MessageType.Error);
                    ApplyError(taskInstanceId);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("Failed added new task; message { " + ex + " }", MessageType.Error);
                }
            }
            return taskInstanceId;
        }
        /// <summary>
        /// Adds new dxf task to data base. 
        /// </summary>
        /// <param name="arrayDocumentId"></param>
        /// <param name="userId"></param>
        public int CreateDxfTask(int[] arrayDocumentId, int userId = 0)
        {
           

            MessageObserver.Instance.SetMessage("Add new task to data base { " + TasksType_e.Dxf + " }", MessageType.System);

            int taskInstanceId = 0;
            try
            {
                taskInstanceId = this.DataContext.Tasks_SetTaskInstance((int)TasksType_e.Dxf, 100500);
                MessageObserver.Instance.SetMessage("Created task instance with id " + taskInstanceId, MessageType.System);

                foreach (var eachDocumentId in arrayDocumentId)
                {
                     this.DataContext.Tasks_SetTaskSelection(taskInstanceId, eachDocumentId);
                    MessageObserver.Instance.SetMessage("Created TaskSelection with. Document id " + eachDocumentId , MessageType.System);
                }
                this.DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                if (taskInstanceId != 0)
                {
                    MessageObserver.Instance.SetMessage("Failed added new task with id " + taskInstanceId + "; message { " + ex + " }", MessageType.Error);
                    ApplyError(taskInstanceId);
                }
                else
                {
                    MessageObserver.Instance.SetMessage("Failed added new task; message { " + ex + " }", MessageType.Error);
                }
            }
            return taskInstanceId;
        }

        /// <summary>
        /// Returns task instance with is waiting status 
        /// </summary>
        /// <returns></returns>
        public TaskInstance GetWaitingTask()
        {
            try
            {
                RefreshRepositoryStatus();
                return DataContext.TaskInstances.First(eachTaskInstance => eachTaskInstance.TaskStatus == (int)TaskStatus_e.Waiting);
            }
            catch
            {
                MessageObserver.Instance.SetMessage("In queue is not exist tasks to execute and waiting. Possibly incorrect saving a task instance", MessageType.Warning);
                return null;
            }
        }
        /// <summary>
        /// Returns selections Tasks by task instance id
        /// </summary>
        /// <param name="taskInstancesID"></param>
        /// <returns></returns>
        public IEnumerable<TaskSelection> GetSelectionsTasks(int taskInstancesID)
        {
            try
            {
                 
                return DataContext.TaskSelections.Where(eachTaskSelection => eachTaskSelection.TaskInstanceID == taskInstancesID);
            }
            catch
            {
                MessageObserver.Instance.SetMessage("In queue is not exist selections tasks to execute and waiting. Possibly incorrect saving a selections task or task instance", MessageType.Error);
                return null;
            }
        }

        #region Apply statuses of the tasks
        /// <summary>
        /// Applly error status for TaskInstance
        /// </summary>
        /// <param name="taskInstanceId"></param>
        public void ApplyError(int taskInstanceId)
        {
            this.DataContext.Tasks_SetTaskStatus((int)TaskStatus_e.Error, taskInstanceId);
            DataContext.SubmitChanges();

            RefreshRepositoryStatus();
            MessageObserver.Instance.SetMessage("Apply Error for task instance by id " + taskInstanceId);
        }

        /// <summary>
        /// Applly competed status for TaskInstance
        /// </summary>
        /// <param name="taskInstanceId"></param>
        public void ApplyCompleted(int taskInstanceId)
        {
            this.DataContext.Tasks_SetTaskStatus((int)TaskStatus_e.Completed, taskInstanceId);
            DataContext.SubmitChanges();

            RefreshRepositoryStatus();
            MessageObserver.Instance.SetMessage("Apply Completed for task instance by id " + taskInstanceId);
        }

        /// <summary>
        /// Applly waiting status for TaskInstance
        /// </summary>
        /// <param name="taskInstanceId"></param>
        public void ApplyWaiting(int taskInstanceId)
        {
            this.DataContext.Tasks_SetTaskStatus((int)TaskStatus_e.Waiting, taskInstanceId);
            DataContext.SubmitChanges();

            RefreshRepositoryStatus();
            MessageObserver.Instance.SetMessage("Apply Waiting for task instance by id " + taskInstanceId);
        }

        /// <summary>
        /// Applly execution status for TaskInstance
        /// </summary>
        /// <param name="taskInstanceId"></param>
        public void ApplyExecution(int taskInstanceId)
        {
            this.DataContext.Tasks_SetTaskStatus((int)TaskStatus_e.Execution, taskInstanceId);
            DataContext.SubmitChanges();

            RefreshRepositoryStatus();
            MessageObserver.Instance.SetMessage("Apply Execution for task instance by id " + taskInstanceId);
        }
        #endregion

        #region exist  
        /// <summary>
        /// Check on exist task with execution status
        /// </summary>
        /// <returns></returns>
        public bool ExistExecutingTask()
        {

            RefreshRepositoryStatus();

            int counExecutingTasks = DataContext.View_ActiveTasks.Where(eachTask => eachTask.TaskStatus == (int)TaskStatus_e.Execution).Count();

            return counExecutingTasks > 0 ? true : false;
        }

        /// <summary>
        /// Check on exist task with waiting status
        /// </summary>
        /// <returns></returns>
        public bool ExistWaitingTasks()
        {

            RefreshRepositoryStatus();
            int countWaitingTasks = DataContext.View_ActiveTasks.Where(eachTask => eachTask.TaskStatus == (int)TaskStatus_e.Waiting).Count();
            return countWaitingTasks > 0 ? true : false;
        }
        #endregion

        public void UpDateCutList(string configuration, byte[] DXFByte, decimal? workpieceX = null, decimal? workpieceY = null, int? bend = null,
                                 decimal? thickness = null, int? version = null,
                                 decimal? paintX = null, decimal? paintY = null, decimal? paintZ = null,
                                  int? IdPdm = null, decimal? surfaceArea = null)
        {
            DataContext.UpDateCutList
                (
                    workpieceX,
                    workpieceY,
                    bend,
                    thickness,
                    configuration,
                    version,
                    paintX,
                    paintY,
                    paintZ,
                    IdPdm,
                    surfaceArea,
                    DXFByte
               );
        }

  
        public void RefreshRepositoryStatus()
        {
            try
            {
                DataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DataContext.TaskInstances);
                DataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DataContext.TaskSelections);               
          
            }
            catch(Exception exception)
            {
               MessageObserver.Instance.SetMessage("Failed refresh entities { " + exception.ToString() + " }");
            }
        }
    }
}