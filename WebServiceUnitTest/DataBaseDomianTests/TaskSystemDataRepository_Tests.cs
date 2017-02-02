using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataBaseDomian;

namespace WebServiceUnitTest.DataBaseDomianTests
{
    /// <summary>
    /// Summary description for TaskSystemDataRepository_Tests
    /// </summary>
    [TestClass]
    public class TaskSystemDataRepository_Tests
    {




        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateDxfTask_TestMethod_withOneDocId()
        {

            long countTasks = TaskSystemDataRepository.Instance.CountTasks;
            System.Diagnostics.Debug.WriteLine("количество тасков до добавления " + countTasks);
            int TaskInstance_id =   TaskSystemDataRepository.Instance.CreateDxfTask(new int[] { 99282 }, 0);
            System.Diagnostics.Debug.WriteLine("количество тасков после добавления " + TaskSystemDataRepository.Instance.CountTasks);

            Assert.AreNotEqual<long>(countTasks, TaskSystemDataRepository.Instance.CountTasks);
            System.Diagnostics.Debug.WriteLine("Таск добавлен: изменилось количество");
            Assert.AreEqual<long>(countTasks+1, TaskSystemDataRepository.Instance.CountTasks);
            System.Diagnostics.Debug.WriteLine("    количество соответствует прогнозам");
            Assert.AreEqual<long>(TaskSystemDataRepository.Instance.CountSelectionsTask(TaskInstance_id)+1, TaskSystemDataRepository.Instance.CountSelectionsTask(TaskInstance_id));
            System.Diagnostics.Debug.WriteLine("    добавлен SelectionsTask: количество соотвествует прогнозам");
        }


        //[TestMethod]
        //public void CreateDxfTask_TestMethod_withManyDocId()
        //{
        //    int[] documentsId = new int[] { 99281, 137138, 330060, 330243 };
        //    int countNewDocuments = documentsId.Length;

        //    TaskSystemDataRepository.Instance.CreateDxfTask(documentsId, 0);
        //}
    }
}
