using PDMWebService.Properties;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VentsCadLibrary;
using VentsMaterials;
using static VentsCadLibrary.VaultSystem;

namespace PDMWebService.Data.Solid.PartBuilders
{
    public abstract class AbstractBuilder 
    {
        protected List<FileInfo> NewComponents;
        public List<VentsCadFile> NewComponentsFull;
        public AbstractBuilder() : base()
        {
            NewComponents = new List<FileInfo>();
            NewComponentsFull = new List<VentsCadFile>(); 
        }

        public static void DelEquations(int index, IModelDoc2 swModel)
        {
            try
            {
                //Логгер.Информация($"Удаление уравнения #{index} в модели {swModel.GetPathName()}", "", null, "DelEquations");
                var myEqu = swModel.GetEquationMgr();
                myEqu.Delete(index);
                swModel.EditRebuild3();
                //myEqu.Add2(index, "\"" + System.Convert.ToChar(index) + "\"=" + index, false);
            }
            catch (Exception e)
            {
                //Логгер.Ошибка($"Удаление уравнения #{index} в модели {swModel.GetPathName()}. {e.Message}", e.StackTrace, null, "DelEquations");
            }
        }


        protected  void GabaritsForPaintingCamera(IModelDoc2 swmodel)
        {
            try
            {
                const long valueset = 1000;
                const int swDocPart = 1;
                const int swDocAssembly = 2;

                for (var i = 0; i < swmodel.GetConfigurationCount(); i++)
                {
                    i = i + 1;
                    var configname = swmodel.IGetConfigurationNames(ref i);

                    //  //MessageBox.Show(configname, swmodel.GetConfigurationCount().ToString());

                    Configuration swConf = swmodel.GetConfigurationByName(configname);
                    if (swConf.IsDerived()) continue;
                    //swmodel.ShowConfiguration2(configname);
                    swmodel.EditRebuild3();

                    switch (swmodel.GetType())
                    {
                        case swDocPart:
                            {
                                //     //MessageBox.Show("swDocPart");
                                var part = (PartDoc)swmodel;
                                var box = part.GetPartBox(true);

                                swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                                // swmodel.AddCustomInfo3(configname, "Длина", , "");

                                swmodel.CustomInfo2[configname, "Длина"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[0] - box[3]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Ширина"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[1] - box[4]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Высота"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[2] - box[5]) * valueset)), 0));

                            }
                            break;
                        case swDocAssembly:
                            {
                                //    //MessageBox.Show("AssemblyDoc");

                                var swAssy = (AssemblyDoc)swmodel;

                                var boxAss = swAssy.GetBox((int)swBoundingBoxOptions_e.swBoundingBoxIncludeRefPlanes);

                                swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                                swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                                swmodel.CustomInfo2[configname, "Длина"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[0] - boxAss[3]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Ширина"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[1] - boxAss[4]) * valueset)), 0));
                                swmodel.CustomInfo2[configname, "Высота"] =
                                    Convert.ToString(
                                        Math.Round(Convert.ToDecimal((long)(Math.Abs(boxAss[2] - boxAss[5]) * valueset)), 0));
                            }
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                //MessageBox.Show($"{swmodel.GetTitle()}\n{exception.ToString()}\n{exception.StackTrace}", "GabaritsForPaintingCamera");
            }
        }

        protected void SwPartParamsChangeWithNewName(string partName, string newName, string[,] newParams,
            bool newFuncOfAdding, IReadOnlyList<string> copies)
        {

            ModelDoc2 swDoc = null;
            try
            {
                //Логгер.Отладка($"Начало изменения детали {partName}", "", "", "SwPartParamsChangeWithNewName");
                int error = 0;
                int warnings = 0;
             
                swDoc = ((ModelDoc2)(SolidWorksInstance.SldWoksApp.ActivateDoc2(partName + ".SLDPRT", true, 0)));
                var modName = swDoc.GetPathName();
                for (var i = 0; i < newParams.Length / 2; i++)
                {
                    try
                    {
                        var myDimension = ((Dimension)(swDoc.Parameter(newParams[i, 0] + "@" + partName + ".SLDPRT")));
                        var param = Convert.ToDouble(newParams[i, 1]);
                        var swParametr = param;
                        myDimension.SystemValue = swParametr / 1000;
                        swDoc.EditRebuild3();
                    }
                    catch (Exception e)
                    {
                        //Логгер.Отладка(
                        //string.Format(
                        //      "Во время изменения детали {4} произошла ошибка при изменении параметров {0}={1}. {2} {3}",
                        //      newParams[i, 0], newParams[i, 1], e.TargetSite, e.Message,
                        //       Path.GetFileNameWithoutExtension(modName)),
                        //    "", "", "SwPartParamsChangeWithNewName");
                    }
                }
                if (newName == "")
                {
                    return;
                }

                GabaritsForPaintingCamera(swDoc);

                swDoc.EditRebuild3();
                swDoc.ForceRebuild3(false);

                if (!newFuncOfAdding)
                {
                    NewComponents.Add(new FileInfo(newName + ".SLDPRT"));
                }

                if (newFuncOfAdding)
                {
                    // todo проверить
                    //if (!string.IsNullOrEmpty(copies)) return;
                    NewComponentsFull.Add(new VaultSystem.VentsCadFile
                    {
                        LocalPartFileInfo = new FileInfo(newName + ".SLDPRT").FullName,
                        PartIdSql = Convert.ToInt32(newName.Substring(newName.LastIndexOf('-') + 1))
                    });
                }


                swDoc.SaveAs2(new FileInfo(newName + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    false, true);

                if (copies != null)
                {
                    // //MessageBox.Show("copies - " + copies + "  addingInName - " + addingInName);
                    swDoc.SaveAs2(new FileInfo(copies[0] + ".SLDPRT").FullName,
                        (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
                    swDoc.SaveAs2(new FileInfo(copies[1] + ".SLDPRT").FullName,
                        (int)swSaveAsVersion_e.swSaveAsCurrentVersion, true, true);
                }


                SolidWorksInstance.SldWoksApp.CloseDoc(newName + ".SLDPRT");
                //Логгер.Отладка($"Деталь {partName} изменена и сохранена по пути {new FileInfo(newName).FullName}", "",
                //    "", "SwPartParamsChangeWithNewName");
                //  //MessageBox.Show("Все хорошо... же.");
            }
            catch (Exception e)
            {
               
                string param = "с параметрами ";
                foreach (var item in newParams)
                {
                    param += item + ", ";
                }
                //MessageBox.Show(e.ToString() + isNullStr + " для детали " + partName + param);
            }
        }

        protected void VentsMatdll(IList<string> materialP1, IList<string> покрытие, string newName)
        {
            ModelDoc2 model = SolidWorksInstance.SldWoksApp.ActivateDoc2(newName, true, 0);
            if (model == null) throw new NullReferenceException("Модель ненайдена");

            try
            {
                // //MessageBox.Show(newName);

                var setMaterials = new SetMaterials();
                ToSQL.Conn = Settings.Default.ConnectionToSQL;
                var toSql = new ToSQL();

                // //MessageBox.Show($"Conn - {ToSQL.Conn} SetMaterials {setMaterials == null} toSql - {toSql == null} _swApp {_swApp == null} levelId - {Convert.ToInt32(materialP1[0])}");

                setMaterials?.ApplyMaterial("", "00", Convert.ToInt32(materialP1[0]), SolidWorksInstance.SldWoksApp);
                model?.Save();

                foreach (var confname in setMaterials.GetConfigurationNames(SolidWorksInstance.SldWoksApp))
                {
                    foreach (var matname in setMaterials.GetCustomProperty(confname, SolidWorksInstance.SldWoksApp))
                    {
                        toSql.AddCustomProperty(confname, matname.Name, SolidWorksInstance.SldWoksApp);
                    }
                }

                if (покрытие != null)
                {
                    if (покрытие[1] != "0")
                    {
                        setMaterials.SetColor("00", покрытие[0], покрытие[1], покрытие[2], SolidWorksInstance.SldWoksApp);
                    }
                    SolidWorksInstance.SldWoksApp.IActiveDoc2.Save();
                }

                try
                {
                    string message;
                    setMaterials.CheckSheetMetalProperty("00", SolidWorksInstance.SldWoksApp, out message);
                    if (message != null)
                    {
                        //  //MessageBox.Show(message, newName + " 858 ");
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show($"{newName}\n{e.ToString()}\n{e.StackTrace}", "VentsMatdll");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show($"{newName}\n{e.ToString()}\n{e.StackTrace}\n{newName}", "VentsMatdll 2");
            }

            GabaritsForPaintingCamera(model);

            model?.Save();
        }

       protected bool GetExistingFile(string fileName, int type)
        {
            if (new FileInfo(fileName).Exists)
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }

            List<VaultSystem.SearchInVault.FindedDocuments> найденныеФайлы;
            switch (type)

            {
                case 0:
                    VaultSystem.SearchInVault.SearchDoc(fileName, VaultSystem.SearchInVault.SwDocType.SwDocAssembly, out найденныеФайлы, Settings.Default.PdmBaseName);
                    if (найденныеФайлы?.Count > 0) goto m1;
                    break;
                case 1:
                    VaultSystem.SearchInVault.SearchDoc(fileName, VaultSystem.SearchInVault.SwDocType.SwDocPart, out найденныеФайлы, Settings.Default.PdmBaseName);
                    if (найденныеФайлы?.Count > 0) goto m1;
                    break;
            }

            goto m2;
            m1:
            try
            {
              PDMWebService.Data.PDM.PDMAdapter.Instance.GetLastVersionAsmPdm(найденныеФайлы[0].Path );
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(найденныеФайлы[0].Path);
                return fileNameWithoutExtension != null && string.Equals(fileNameWithoutExtension, fileName, StringComparison.CurrentCultureIgnoreCase);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());return false;
            }
            m2:
            return false;
        }

        static bool Усиливающая(string pType)
        {
            switch (pType)
            {
                case "24":
                case "25":
                case "26":
                case "27":
                case "28":
                case "29":
                    return true;
                default:
                    return false;
            }
        }
        public   bool OpenIfExist(string FilePath, VaultSystem.VentsCadFile.Type type, DateTime? lastChange)
        {
            var open = false;
            //Получение файлов в хранилище
            if (string.IsNullOrEmpty(FilePath)) return true;
            var fileName = Path.GetFileNameWithoutExtension(FilePath);

            var cadFiles = VaultSystem.VentsCadFile.Get(fileName, type, Settings.Default.PdmBaseName);
            if (cadFiles == null) return false;

            VaultSystem.VentsCadFile findedFile = null;

            try
            {
                findedFile = cadFiles.Single(x => Path.GetFileNameWithoutExtension(x.Path).ToLower() == fileName.ToLower());
            }
            catch (Exception exception)
            {
                // //MessageBox.Show(exception.ToString(), "findedFile");
            }

            if (findedFile == null)
            {
                return false;
            }

            #region To delete

        
            #endregion

            //Определение и получение данных в объект -olderFile- если файл записан раньше чем изменен шаблон (findedFile.Time < lastChange)
            var olderFile = VersionsFileInfo.Replaced.GetIfOlder(findedFile.Path, lastChange, findedFile.Time);

            bool result;
            if (olderFile == null)
            {
                result = true;  //MessageBox.Show(fileName + " уже есть в базе. Открыть? ", "", MessageBoxButton.YesNo);
            }
            else
            {
                result = true;  //MessageBox.Show(fileName + " уже есть в базе, но конструкция устарела.\nЧтобы обновить нажмите \"Да\",\nоткрыть существующую без обновления -  \"Отмена\"? ", "Обновить?", MessageBoxButton.YesNoCancel);
            }

            if (result == false)
            {
                open = true;
            }
            else if (result != false)
            {
                if (result == true)
                {
                    if (olderFile == null)
                    {
                        var processes = Process.GetProcessesByName("SLDWORKS");
                        if (processes?.Length > 0)
                        {
                            VentsCad.OpenSwDoc(findedFile.Path);
                        }
                        else
                        {
                            Process.Start("conisio://" + Settings.Default.TestPdmBaseName + "/open?projectid=" + findedFile.ProjectId +
                                  "&documentid=" + findedFile.PartIdPdm + "&objecttype=1");
                        }
                        open = true;
                    }
                    else
                    {
                       
                      PDMWebService.Data.PDM.PDMAdapter.Instance.CheckInOutPdm(new List<FileInfo> { new FileInfo(findedFile.Path) }, false);
                        open = false;
                    }
                }
                if (result == false)
                {
                    var processes = Process.GetProcessesByName("SLDWORKS");
                    if (processes?.Length > 0)
                    {
                        VentsCad.OpenSwDoc(findedFile.Path);
                    }
                    else
                    {
                        Process.Start("conisio://" + Settings.Default.TestPdmBaseName + "/open?projectid=" + findedFile.ProjectId +
                              "&documentid=" + findedFile.PartIdPdm + "&objecttype=1");
                    }
                    open = true; 
                }
            }
            return open;
        }
    }
}
