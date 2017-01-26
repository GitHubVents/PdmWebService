using ServiceLibrary.DataContracts;
using ServiceLibrary.TaskSystem.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text; 

namespace PDM_WebService.WcfServiceLibrary
{
    [ServiceContract]
    public  interface ITaskSystemMonitor
    {
        [OperationContract]
        TaskData[] GetTasksData(int userId = 0, TasksTypes type = TasksTypes.None, TaskStatuses status = TaskStatuses.All); 
    }
}
