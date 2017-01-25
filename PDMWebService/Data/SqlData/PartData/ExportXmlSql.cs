using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PDMWebService.Data.SqlData.PartData
{
    public class ExportXmlSql
    {
        private static int CurrentVersion { get; set; }


        public static bool ExistXml(string modelName, int documentVersion, out string message)
        {
            message = "";

            #region To Delete

            //var name = new FileInfo(modelName).Name;
            //message = message +"\nName"+ name;

            //if (name != null)
            //{
            //    modelName = Path.GetFileNameWithoutExtension(name).ToUpper();
            //}

            #endregion

            bool exist = false;

            try
            {
                var xmlPartPath = new FileInfo(XmlPath + modelName + ".xml");
                if (!xmlPartPath.Exists) return false;

                var xmlPartVersion = GetXmlVersion(xmlPartPath.FullName);
                exist = Equals(xmlPartVersion, documentVersion);
            }
            catch (Exception e)
            {
                message = $"Exception 1: {e.Message}";
                exist = false;
            }
            if (exist)
            {
                return true;
            }
            else if (!exist)
                try
                {
                    if (File.Exists(XmlPath + modelName + ".xml"))
                    {
                        var xmlPartVer = GetXmlVersion(XmlPath + modelName + ".xml");
                        exist = Equals(xmlPartVer, documentVersion);
                    }
                }
                catch (Exception e)
                {
                    message = $"Exception 2: {e.Message}";
                }

            if (exist)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        static int? GetXmlVersion(string xmlPath)
        {
            if (!xmlPath.EndsWith("xml")) return null;

            int? version = null;

            try
            {
                var coordinates = XDocument.Load(xmlPath);

                var enumerable = coordinates.Descendants("attribute")
                    .Select(
                        element =>
                            new
                            {
                                Number = element.FirstAttribute.Value,
                                Values = element.Attribute("value")
                            });
                foreach (var obj in enumerable)
                {
                    if (obj.Number != "Версия") continue;
                    version = Convert.ToInt32(obj.Values.Value);
                    goto m1;
                }
            }
            catch (Exception)
            {
                return 0;
            }

            m1:

            return version;
        }

        public static string XmlPath { get; set; } = @"\\pdmsrv\XML\"; //  @"\\pdmsrv\SolidWorks Admin\XML\"; // @"C:\Temp\"; //

        public static string ConnectionString { get; } = "Data Source=pdmsrv;Initial Catalog=SWPlusDB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd;MultipleActiveResultSets=True";

        public static void Export(SldWorks swApp, int lastVer, int idPdm, out Exception exception)
        {
            Export(swApp, lastVer, idPdm, false, out exception);
        }

        public static void GetCurrentConfigPartData(SldWorks swApp, int lastVer, int idPdm, bool closeDoc, bool fixBends, out List<DataToExport> dataList, out Exception exception)
        //public static void GetCurrentConfigPartData(SldWorks swApp, bool closeDoc, bool fixBends, out List<DataToExport> dataList, out Exception exception)
        {
            // Проход по всем родительским конфигурациям

            exception = null;
            dataList = new List<DataToExport>();

            var swModel = swApp.IActiveDoc2;

            if (swModel == null) return;

            var configName = ((Configuration)swModel.GetActiveConfiguration()).Name;

            swModel.ShowConfiguration2(configName);
            swModel.EditRebuild3();
            var swModelDocExt = swModel.Extension;

            var fileName = swModel.GetTitle().ToUpper().Replace(".SLDPRT", "");

            AddDimentions(swModel, configName, out exception);

            var confiData = new DataToExport
            {
                Config = configName,
                FileName = fileName,
                IdPdm = idPdm,
                Version = lastVer
            };

            #region Разгибание всех сгибов
            fixBends = true;
            if (fixBends)
            {
                swModel.EditRebuild3();
                List<PartBendInfo> list;
                Bends.Fix(swApp, out list, false);
            }


            #endregion

            swModel.ForceRebuild3(false);

            var swCustProp = swModelDocExt.CustomPropertyManager[configName];
            string valOut;
            string materialId;

            // TO DO LOOK

            swCustProp.Get4("MaterialID", true, out valOut, out materialId);
            if (string.IsNullOrEmpty(materialId))
            {
                confiData.MaterialId = null;
            }
            else
            {
                confiData.MaterialId = int.Parse(materialId);
            }

            string paintX;
            swCustProp.Get4("Длина", true, out valOut, out paintX);
            if (string.IsNullOrEmpty(paintX))
            {
                confiData.PaintX = null;
            }
            else
            {
                confiData.PaintX = double.Parse(paintX);
            }

            string paintY;
            swCustProp.Get4("Ширина", true, out valOut, out paintY);
            if (string.IsNullOrEmpty(paintY))
            {
                confiData.PaintY = null;
            }
            else
            {
                confiData.PaintY = double.Parse(paintY);
            }

            string paintZ;
            swCustProp.Get4("Высота", true, out valOut, out paintZ);
            if (string.IsNullOrEmpty(paintZ))
            {
                confiData.PaintZ = null;
            }
            else
            {
                confiData.PaintZ = double.Parse(paintZ);
            }

            string codMaterial;
            swCustProp.Get4("Код материала", true, out valOut, out codMaterial);
            confiData.КодМатериала = codMaterial;

            string материал;
            swCustProp.Get4("Материал", true, out valOut, out материал);
            confiData.Материал = материал;

            string обозначение;
            swCustProp.Get4("Обозначение", true, out valOut, out обозначение);
            confiData.Обозначение = обозначение;

            var swCustPropForDescription = swModelDocExt.CustomPropertyManager[""];
            string наименование;
            swCustPropForDescription.Get4("Наименование", true, out valOut, out наименование);
            confiData.Наименование = наименование;

            //UpdateCustomPropertyListFromCutList
            const string длинаГраничнойРамкиName = @"Длина граничной рамки";
            const string длинаГраничнойРамкиName2 = @"Bounding Box Length";
            const string ширинаГраничнойРамкиName = @"Ширина граничной рамки";
            const string ширинаГраничнойРамкиName2 = @"Bounding Box Width";
            const string толщинаЛистовогоМеталлаNAme = @"Толщина листового металла";
            const string толщинаЛистовогоМеталлаNAme2 = @"Sheet Metal Thickness";
            const string сгибыName = @"Сгибы";
            const string сгибыName2 = @"Bends";
            const string площадьПокрытияName = @"Площадь покрытия";//const string площадьПокрытияName2 = @"Bounding Box Area";

            Feature swFeat2 = swModel.FirstFeature();
            while (swFeat2 != null)
            {
                if (swFeat2.GetTypeName2() == "SolidBodyFolder")
                {
                    BodyFolder swBodyFolder = swFeat2.GetSpecificFeature2();
                    swFeat2.Select2(false, -1);
                    swBodyFolder.SetAutomaticCutList(true);
                    swBodyFolder.UpdateCutList();

                    Feature swSubFeat = swFeat2.GetFirstSubFeature();
                    while (swSubFeat != null)
                    {
                        if (swSubFeat.GetTypeName2() == "CutListFolder")
                        {

                            BodyFolder bodyFolder = swSubFeat.GetSpecificFeature2();

                            if (bodyFolder.GetCutListType() != (int)swCutListType_e.swSheetmetalCutlist)
                            {
                                goto m1;
                            }

                            swSubFeat.Select2(false, -1);
                            bodyFolder.SetAutomaticCutList(true);
                            bodyFolder.UpdateCutList();
                            var swCustPrpMgr = swSubFeat.CustomPropertyManager;
                            swCustPrpMgr.Add("Площадь поверхности", "Текст",
                                "\"SW-SurfaceArea@@@Элемент списка вырезов1@" +
                                Path.GetFileName(swModel.GetPathName()) + "\"");

                            string длинаГраничнойРамки;
                            swCustPrpMgr.Get4(длинаГраничнойРамкиName, true, out valOut,
                                out длинаГраничнойРамки);
                            if (string.IsNullOrEmpty(длинаГраничнойРамки))
                            {
                                swCustPrpMgr.Get4(длинаГраничнойРамкиName2, true, out valOut,
                                    out длинаГраничнойРамки);
                            }
                            swCustProp.Set(длинаГраничнойРамкиName, длинаГраничнойРамки);
                            confiData.ДлинаГраничнойРамки = длинаГраничнойРамки;

                            string ширинаГраничнойРамки;
                            swCustPrpMgr.Get4(ширинаГраничнойРамкиName, true, out valOut,
                                out ширинаГраничнойРамки);
                            if (string.IsNullOrEmpty(ширинаГраничнойРамки))
                            {
                                swCustPrpMgr.Get4(ширинаГраничнойРамкиName2, true, out valOut,
                                    out ширинаГраничнойРамки);
                            }
                            swCustProp.Set(ширинаГраничнойРамкиName, ширинаГраничнойРамки);
                            confiData.ШиринаГраничнойРамки = ширинаГраничнойРамки;

                            string толщинаЛистовогоМеталла;
                            swCustPrpMgr.Get4(толщинаЛистовогоМеталлаNAme, true, out valOut,
                                out толщинаЛистовогоМеталла);
                            if (string.IsNullOrEmpty(толщинаЛистовогоМеталла))
                            {
                                swCustPrpMgr.Get4(толщинаЛистовогоМеталлаNAme2, true, out valOut,
                                out толщинаЛистовогоМеталла);
                            }
                            swCustProp.Set(толщинаЛистовогоМеталлаNAme, толщинаЛистовогоМеталла);
                            confiData.ТолщинаЛистовогоМеталла = толщинаЛистовогоМеталла;

                            string сгибы;
                            swCustPrpMgr.Get4(сгибыName, true, out valOut, out сгибы);
                            if (string.IsNullOrEmpty(сгибы))
                            {
                                swCustPrpMgr.Get4(сгибыName2, true, out valOut, out сгибы);
                            }
                            swCustProp.Set(сгибыName, сгибы);
                            confiData.Сгибы = сгибы;

                            var myMassProp = swModel.Extension.CreateMassProperty();
                            var площадьПоверхности =
                                Convert.ToString(Math.Round(myMassProp.SurfaceArea * 1000) / 1000);

                            swCustProp.Set(площадьПокрытияName, площадьПоверхности);
                            try
                            {
                                confiData.ПлощадьПокрытия =
                                    double.Parse(площадьПоверхности.Replace(".", ","));
                            }
                            catch (Exception e)
                            {
                                exception = e;
                            }
                        }
                        m1:
                        swSubFeat = swSubFeat.GetNextFeature();
                    }
                }
                swFeat2 = swFeat2.GetNextFeature();
            }
            dataList.Add(confiData);

            if (!closeDoc)
            {
                return;
            }
            var namePrt = swApp.IActiveDoc2.GetTitle().ToLower().Contains(".sldprt")
                ? swApp.IActiveDoc2.GetTitle()
                : swApp.IActiveDoc2.GetTitle() + ".sldprt";
            swApp.CloseDoc(namePrt);

        }

        public static void Export(SldWorks swApp, int verToExport, int idPdm, bool closeDoc, out Exception exception)
        {
            exception = null;
            CurrentVersion = verToExport;
            //verToExport;

            #region Сбор информации по детали и сохранение разверток

            if (swApp == null)
            {
                try
                {
                    swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }
                catch (Exception)
                {
                    swApp = new SldWorks { Visible = false };
                }
            }

            IModelDoc2 swModel;

            try
            {
                swModel = swApp.IActiveDoc2;
            }
            catch (Exception)
            {
                swModel = swApp.IActiveDoc2;
            }

            if (swModel == null) return;

            var modelName = swModel.GetTitle();

            try
            {


                IPartDoc partDoc;
                try
                {
                    partDoc = (IPartDoc)((ModelDoc2)swModel);
                }
                catch (Exception)
                {
                    return;
                }

                bool sheetMetal = false;

                try
                {
                    sheetMetal = Part.IsSheetMetal(partDoc);
                }
                catch (Exception)
                {

                }

                if (!sheetMetal)
                {
                    //swApp.CloseDoc(Path.GetFileName(swModel.GetPathName()));

                    //13.10.2016

                    swApp.CloseAllDocuments(true);

                    //swApp.ExitApp();
                    return;
                }

                var activeconfiguration = (Configuration)swModel.GetActiveConfiguration();
                var swModelConfNames = (string[])swModel.GetConfigurationNames();

                foreach (var name in from name in swModelConfNames
                                     let config = (Configuration)swModel.GetConfigurationByName(name)
                                     where config.IsDerived()
                                     select name)
                {
                    swModel.DeleteConfiguration(name);
                }

                var swModelDocExt = swModel.Extension;
                var swModelConfNames2 = (string[])swModel.GetConfigurationNames();

                // Проход по всем родительским конфигурациям

                var dataList = new List<DataToExport>();

                var filePath = swModel.GetPathName();

                foreach (var configName in from name in swModelConfNames2
                                           let config = (Configuration)swModel.GetConfigurationByName(name)
                                           where !config.IsDerived()
                                           select name)
                {
                    // swModel.ShowConfiguration2(configName);
                    swModel.EditRebuild3();

                    AddDimentions(swModel, configName, out exception);

                    var confiData = new DataToExport
                    {
                        Config = configName,
                        FileName = filePath.Substring(filePath.LastIndexOf('\\') + 1),
                        IdPdm = idPdm
                    };

                    #region Разгибание всех сгибов

                    try
                    {
                        swModel.EditRebuild3();
                        List<PartBendInfo> list;
                        Bends.Fix(swApp, out list, false);
                    }
                    catch (Exception)
                    {
                        //
                    }

                    #endregion

                    swModel.ForceRebuild3(false);

                    var swCustProp = swModelDocExt.CustomPropertyManager[configName];
                    string valOut;
                    string materialId;

                    swCustProp.Get4("MaterialID", true, out valOut, out materialId);
                    if (string.IsNullOrEmpty(materialId))
                    {
                        confiData.MaterialId = null;
                    }
                    else
                    {
                        confiData.MaterialId = int.Parse(materialId);
                    }

                    string paintX;
                    swCustProp.Get4("Длина", true, out valOut, out paintX);
                    if (string.IsNullOrEmpty(paintX))
                    {
                        confiData.PaintX = null;
                    }
                    else
                    {
                        confiData.PaintX = double.Parse(paintX);
                    }

                    string paintY;
                    swCustProp.Get4("Ширина", true, out valOut, out paintY);
                    if (string.IsNullOrEmpty(paintY))
                    {
                        confiData.PaintY = null;
                    }
                    else
                    {
                        confiData.PaintY = double.Parse(paintY);
                    }

                    string paintZ;
                    swCustProp.Get4("Высота", true, out valOut, out paintZ);
                    if (string.IsNullOrEmpty(paintZ))
                    {
                        confiData.PaintZ = null;
                    }
                    else
                    {
                        confiData.PaintZ = double.Parse(paintZ);
                    }

                    string codMaterial;
                    swCustProp.Get4("Код материала", true, out valOut, out codMaterial);
                    confiData.КодМатериала = codMaterial;

                    string материал;
                    swCustProp.Get4("Материал", true, out valOut, out материал);
                    confiData.Материал = материал;

                    string обозначение;
                    swCustProp.Get4("Обозначение", true, out valOut, out обозначение);
                    confiData.Обозначение = обозначение;

                    var swCustPropForDescription = swModelDocExt.CustomPropertyManager[""];
                    string наименование;
                    swCustPropForDescription.Get4("Наименование", true, out valOut, out наименование);
                    confiData.Наименование = наименование;

                    //UpdateCustomPropertyListFromCutList
                    const string длинаГраничнойРамкиName = @"Длина граничной рамки";
                    const string длинаГраничнойРамкиName2 = @"Bounding Box Length";
                    const string ширинаГраничнойРамкиName = @"Ширина граничной рамки";
                    const string ширинаГраничнойРамкиName2 = @"Bounding Box Width";
                    const string толщинаЛистовогоМеталлаNAme = @"Толщина листового металла";
                    const string толщинаЛистовогоМеталлаNAme2 = @"Sheet Metal Thickness";//Sheet Metal Thickness
                    const string сгибыName = @"Сгибы";
                    const string сгибыName2 = @"Bends";
                    const string площадьПокрытияName = @"Площадь покрытия";
                    const string площадьПокрытияName2 = @"Bounding Box Area";

                    Feature swFeat2 = swModel.FirstFeature();
                    while (swFeat2 != null)
                    {
                        if (swFeat2.GetTypeName2() == "SolidBodyFolder")
                        {
                            //  List<Bends.SolidWorksFixPattern.PartBendInfo> list;
                            //  Bends.Fix(swApp, out list, false);
                            BodyFolder swBodyFolder = swFeat2.GetSpecificFeature2();
                            swFeat2.Select2(false, -1);
                            swBodyFolder.SetAutomaticCutList(true);
                            swBodyFolder.UpdateCutList();

                            Feature swSubFeat = swFeat2.GetFirstSubFeature();
                            while (swSubFeat != null)
                            {
                                if (swSubFeat.GetTypeName2() == "CutListFolder")
                                {

                                    BodyFolder bodyFolder = swSubFeat.GetSpecificFeature2();

                                    if (bodyFolder.GetCutListType() != (int)swCutListType_e.swSheetmetalCutlist)
                                    {
                                        goto m1;
                                    }

                                    swSubFeat.Select2(false, -1);
                                    bodyFolder.SetAutomaticCutList(true);
                                    bodyFolder.UpdateCutList();
                                    var swCustPrpMgr = swSubFeat.CustomPropertyManager;
                                    swCustPrpMgr.Add("Площадь поверхности", "Текст",
                                        "\"SW-SurfaceArea@@@Элемент списка вырезов1@" +
                                        Path.GetFileName(swModel.GetPathName()) + "\"");

                                    string длинаГраничнойРамки;
                                    swCustPrpMgr.Get4(длинаГраничнойРамкиName, true, out valOut,
                                        out длинаГраничнойРамки);
                                    if (string.IsNullOrEmpty(длинаГраничнойРамки))
                                    {
                                        swCustPrpMgr.Get4(длинаГраничнойРамкиName2, true, out valOut,
                                            out длинаГраничнойРамки);
                                    }
                                    swCustProp.Set(длинаГраничнойРамкиName, длинаГраничнойРамки);
                                    confiData.ДлинаГраничнойРамки = длинаГраничнойРамки;

                                    string ширинаГраничнойРамки;
                                    swCustPrpMgr.Get4(ширинаГраничнойРамкиName, true, out valOut,
                                        out ширинаГраничнойРамки);
                                    if (string.IsNullOrEmpty(ширинаГраничнойРамки))
                                    {
                                        swCustPrpMgr.Get4(ширинаГраничнойРамкиName2, true, out valOut,
                                            out ширинаГраничнойРамки);
                                    }
                                    swCustProp.Set(ширинаГраничнойРамкиName, ширинаГраничнойРамки);
                                    confiData.ШиринаГраничнойРамки = ширинаГраничнойРамки;

                                    string толщинаЛистовогоМеталла;
                                    swCustPrpMgr.Get4(толщинаЛистовогоМеталлаNAme, true, out valOut,
                                        out толщинаЛистовогоМеталла);
                                    if (string.IsNullOrEmpty(толщинаЛистовогоМеталла))
                                    {
                                        swCustPrpMgr.Get4(толщинаЛистовогоМеталлаNAme2, true, out valOut,
                                        out толщинаЛистовогоМеталла);
                                    }
                                    swCustProp.Set(толщинаЛистовогоМеталлаNAme, толщинаЛистовогоМеталла);
                                    confiData.ТолщинаЛистовогоМеталла = толщинаЛистовогоМеталла;

                                    string сгибы;
                                    swCustPrpMgr.Get4(сгибыName, true, out valOut, out сгибы);
                                    if (string.IsNullOrEmpty(сгибы))
                                    {
                                        swCustPrpMgr.Get4(сгибыName2, true, out valOut, out сгибы);
                                    }
                                    swCustProp.Set(сгибыName, сгибы);
                                    confiData.Сгибы = сгибы;

                                    var myMassProp = swModel.Extension.CreateMassProperty();
                                    var площадьПоверхности =
                                        Convert.ToString(Math.Round(myMassProp.SurfaceArea * 1000) / 1000);

                                    swCustProp.Set(площадьПокрытияName, площадьПоверхности);
                                    try
                                    {
                                        confiData.ПлощадьПокрытия =
                                            double.Parse(площадьПоверхности.Replace(".", ","));
                                    }
                                    catch (Exception e)
                                    {
                                        exception = e;
                                    }
                                }
                                m1:
                                swSubFeat = swSubFeat.GetNextFeature();
                            }
                        }
                        swFeat2 = swFeat2.GetNextFeature();
                    }
                    dataList.Add(confiData);
                }

                swModel.ShowConfiguration2(activeconfiguration.Name);

                ExportDataToXmlSql(swModel.GetTitle().ToUpper().Replace(".SLDPRT", ""), dataList, out exception);

                #endregion

                if (!closeDoc) return;
                var namePrt = swApp.IActiveDoc2.GetTitle().ToLower().Contains(".sldprt")
                    ? swApp.IActiveDoc2.GetTitle()
                    : swApp.IActiveDoc2.GetTitle() + ".sldprt";
                swApp.CloseDoc(namePrt);
            }

            catch (Exception e)
            {
                exception = e;
            }

            finally
            {
                swApp.CloseDoc(Path.GetFileName(swModel.GetPathName()));
            }
        }

       

        public static void ExportDataToXmlSql(string fileName, IEnumerable<DataToExport> dataToExport, out Exception exception)
        {
            exception = null;
            if (fileName == null || dataToExport == null) return;
            try
            {
                var myXml = new XmlTextWriter(XmlPath + fileName + ".xml", Encoding.UTF8);

                myXml.WriteStartDocument();
                myXml.Formatting = Formatting.Indented;
                myXml.Indentation = 2;

                // создаем элементы
                myXml.WriteStartElement("xml");
                myXml.WriteStartElement("transactions");
                myXml.WriteStartElement("transaction");

                myXml.WriteStartElement("document");

                foreach (var configData in dataToExport)
                {
                    #region XML

                    try
                    {
                        // Конфигурация
                        myXml.WriteStartElement("configuration");
                        myXml.WriteAttributeString("name", configData.Config);

                        // Материал
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Материал");
                        myXml.WriteAttributeString("value", configData.Материал);
                        myXml.WriteEndElement();

                        // Наименование  -- Из таблицы свойств
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Наименование");
                        myXml.WriteAttributeString("value", configData.Наименование);
                        myXml.WriteEndElement();

                        // Обозначение
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Обозначение");
                        myXml.WriteAttributeString("value", configData.Обозначение);
                        myXml.WriteEndElement();

                        // Площадь покрытия
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Площадь покрытия");
                        myXml.WriteAttributeString("value", Convert.ToString(configData.ПлощадьПокрытия).Replace(",", "."));
                        myXml.WriteEndElement();

                        // ERP code
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Код_Материала");
                        myXml.WriteAttributeString("value", configData.КодМатериала);
                        myXml.WriteEndElement();

                        // Длина граничной рамки

                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Длина граничной рамки");
                        myXml.WriteAttributeString("value", configData.ДлинаГраничнойРамки);
                        myXml.WriteEndElement();

                        // Ширина граничной рамки
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Ширина граничной рамки");
                        myXml.WriteAttributeString("value", configData.ШиринаГраничнойРамки);
                        myXml.WriteEndElement();

                        // Сгибы
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Сгибы");
                        myXml.WriteAttributeString("value", configData.Сгибы);
                        myXml.WriteEndElement();

                        // Толщина листового металла
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Толщина листового металла");
                        myXml.WriteAttributeString("value", configData.ТолщинаЛистовогоМеталла);
                        myXml.WriteEndElement();

                        // PaintX
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintX");
                        myXml.WriteAttributeString("value", Convert.ToString(configData.PaintX).Replace(",", "."));
                        myXml.WriteEndElement();

                        // PaintY
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintY");
                        myXml.WriteAttributeString("value", Convert.ToString(configData.PaintY).Replace(",", "."));
                        myXml.WriteEndElement();

                        // PaintZ
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintZ");
                        myXml.WriteAttributeString("value", Convert.ToString(configData.PaintZ).Replace(",", "."));
                        myXml.WriteEndElement();


                        // Версия последняя
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Версия");
                        myXml.WriteAttributeString("value", configData.Version != 0 ? configData.Version.ToString() : Convert.ToString(CurrentVersion));
                        myXml.WriteEndElement();

                        myXml.WriteEndElement();  //configuration
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"{ex.ToString()}\n + {ex.StackTrace}\n (Name - {configData.FileName} ID - {configData.IdPdm} Conf - {configData.Config} Ver - {configData.Version})");
                        exception = ex;
                    }

                    #endregion

                    #region SQL                   

                    try
                    {
                        using (var sqlConnection = new SqlConnection(ConnectionString))
                        {
                            sqlConnection.Open();

                            var spcmd = new SqlCommand("UpDateCutList", sqlConnection)
                            {
                                CommandType = CommandType.StoredProcedure
                            };

                            double workpieceX;
                            double.TryParse(configData.ДлинаГраничнойРамки.Replace('.', ','), out workpieceX);
                            double workpieceY;
                            double.TryParse(configData.ШиринаГраничнойРамки.Replace('.', ','), out workpieceY);
                            int bend;
                            int.TryParse(configData.Сгибы, out bend);
                            double thickness;
                            double.TryParse(configData.ТолщинаЛистовогоМеталла.Replace('.', ','), out thickness);

                            var configuration = configData.Config;

                            var materialId = configData.MaterialId;

                            if (materialId == null)
                            {
                                spcmd.Parameters.AddWithValue("@MaterialID", DBNull.Value);
                            }
                            else
                            {
                                spcmd.Parameters.AddWithValue("@MaterialID", materialId);
                            }

                            spcmd.Parameters.AddWithValue("@PaintX", configData.PaintX);
                            spcmd.Parameters.AddWithValue("@PaintY", configData.PaintY);
                            spcmd.Parameters.AddWithValue("@PaintZ", configData.PaintZ);

                            spcmd.Parameters.AddWithValue("@FILENAME", configData.FileName);
                            spcmd.Parameters.AddWithValue("@IDPDM", configData.IdPdm);


                            spcmd.Parameters.AddWithValue("@SurfaceArea", ParseDouble(configData.ПлощадьПокрытия.ToString()));


                            spcmd.Parameters.Add("@WorkpieceX", SqlDbType.Float).Value = workpieceX;
                            spcmd.Parameters.Add("@WorkpieceY", SqlDbType.Float).Value = workpieceY;

                            spcmd.Parameters.Add("@Bend", SqlDbType.Int).Value = bend;
                            spcmd.Parameters.Add("@Thickness", SqlDbType.Float).Value = thickness;
                            spcmd.Parameters.Add("@Version", SqlDbType.Int).Value = configData.Version != 0 ? configData.Version : CurrentVersion;
                            spcmd.Parameters.AddWithValue("@configuration", configuration);

                            #region
                            //spcmd.Parameters.Add("@configuration", SqlDbType.NVarChar).Value = configuration;
                            //query = $"MaterialID- {materialId}\nPaintX- {configData.PaintX}\nFILENAME- {configData.FileName}\nIDPDM- {configData.IdPdm}\nSurfaceArea- {ParseDouble(configData.ПлощадьПокрытия.ToString())}\nWorkpieceX- {workpieceX}\nConfiguration- {configuration}";

                            //MessageBox.Show($"MaterialID- {materialId}\nPaintX- {configData.PaintX}\nFILENAME- {configData.FileName}\nIDPDM- {configData.IdPdm}\nSurfaceArea- {ParseDouble(configData.ПлощадьПокрытия.ToString())}\nWorkpieceX- {workpieceX}\nConfiguration- {configuration}");
                            #endregion

                            spcmd.ExecuteNonQuery();

                            sqlConnection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"{ex.ToString()}\n + {ex.StackTrace}\n (Name - {configData.FileName} ID - {configData.IdPdm} Conf - {configData.Config} Ver - {configData.Version})");
                        exception = ex;
                        // MessageBox.Show(query);

                    }

                    #endregion
                }

                myXml.WriteEndElement();// ' элемент DOCUMENT
                myXml.WriteEndElement();// ' элемент TRANSACTION
                myXml.WriteEndElement();// ' элемент TRANSACTIONS
                myXml.WriteEndElement();// ' элемент XML
                // заносим данные в myMemoryStream
                myXml.Flush();

                myXml.Close();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        private static double ParseDouble(string value)
        {
            value = value.Replace(" ", "");
            value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," ? value.Replace(".", ",") : value.Replace(",", ".");
            var splited = value.Split(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]);
            if (splited.Length <= 2) return double.Parse(value);
            var r = "";
            for (var i = 0; i < splited.Length; i++)
            {
                if (i == splited.Length - 1)
                    r += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                r += splited[i];
            }
            value = r;
            return double.Parse(value);
        }

        private static void AddDimentions(IModelDoc2 swmodel, string configname, out Exception exception)
        {
            const long valueset = 1000;
            exception = null;

            try
            {
                swmodel.GetConfigurationByName(configname);
                swmodel.EditRebuild3();

                var part = (PartDoc)swmodel;
                var box = part.GetPartBox(true);

                swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                swmodel.CustomInfo2[configname, "Длина"] =
                    Convert.ToString(
                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[0] - box[3]) * valueset)), 0),
                        CultureInfo.InvariantCulture);
                swmodel.CustomInfo2[configname, "Ширина"] =
                    Convert.ToString(
                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[1] - box[4]) * valueset)), 0),
                        CultureInfo.InvariantCulture);
                swmodel.CustomInfo2[configname, "Высота"] =
                    Convert.ToString(
                        Math.Round(Convert.ToDecimal((long)(Math.Abs(box[2] - box[5]) * valueset)), 0),
                        CultureInfo.InvariantCulture);

                swmodel.EditRebuild3();

            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

    }
}
