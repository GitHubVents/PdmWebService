using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;

namespace VentsMaterials
{
    public class SetMaterials
    {
        public SetMaterials(SldWorks swApp)
        {
            if (swApp != null)
            {
                _swapp = swApp;
            }
        }
        private SldWorks _swapp;
        private ModelDoc2 _swmodel;
        private PartDoc _swPartDoc;
        public int CheckType { get; set; }
        readonly ToSQL _toSql = new ToSQL();

        public string Error { get; set; }

        public DataTable CoatingTypeDt()
        {
            // Here we create a DataTable with columns.
            var tablecoatin = new DataTable();
            tablecoatin.Columns.Add("Name", typeof(string));
            tablecoatin.Columns.Add("Code", typeof(string));

            // Here we add five DataRows.
            tablecoatin.Rows.Add("Шаргень", "WR");
            tablecoatin.Rows.Add("Глянец", "GL");

            return tablecoatin;
        }

        public List<string> CoatingListClass()
        {
            var newlistclass = new List<string>();

            newlistclass.Add("2");
            newlistclass.Add("3");

            return newlistclass;

        }

        public class MatName
        {
            public string Name { get; set; }
            public string ColorSQL { get; set; }
            public string CoatingType { get; set; }
            public string CoatingClass { get; set; }
        }

        #region " СРАВНЕНИЯ ТОЛЩИНЫ ЛИСТОГО МЕТАЛЛА С SQL "
        public string ThicknessValout { get; set; }
        public string BendRadiusValout { get; set; }
        public string KFactorValout { get; set; }

        public class SheetMetalProperty
        {
            public double BendRadius { get; set; }
            public double KFactor { get; set; }
            public double Thickness { get; set; }
        }

        public List<string> Sketches()
        {
            var sketchesString = new List<string>
            {"Листовой металл1", "Листовой металл", "Sheet-Metal1"};

            return sketchesString;
        }

        public List<SheetMetalProperty> GetSheetMetalProperty(ModelDoc2 swModel, Feature swFeat)
        {
            var getsheetmetalproperty = new List<SheetMetalProperty>();
            try
            {

                //var str = (from value in Sketches() where value == "Листовой металл1" | value ==  "Листовой металл" | value ==  "Sheet-Metal1" select value.ToList());

                //foreach (var sketch in Sketches())
                //{

                //MessageBox.Show(sketch);

                var canSelect = swModel.Extension.SelectByID2("Листовой металл1" + "@" + swModel.GetTitle(), "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                //var canselect = swModel.Extension.SelectByID2(sketch + "@" + swModel.GetTitle(), "BODYFEATURE", 0, 0, 0, false, 0, null, 0);

                if (!canSelect)
                {
                    var canselect2 = swModel.Extension.SelectByID2("Листовой металл" + "@" + swModel.GetTitle(), "BODYFEATURE", 0, 0, 0, false, 0, null, 0);

                    if (!canselect2)
                    {
                        swModel.Extension.SelectByID2("Sheet-Metal1" + "@" + swModel.GetTitle(), "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    }

                }

                swFeat.Select(true);

                var swSelMgr = swModel.ISelectionManager;

                swFeat = swSelMgr.GetSelectedObject6(1, -1);


                //if (swFeat != null)
                //{

                SheetMetalFeatureData swSheetMetal = swFeat.GetDefinition();

                swFeat.ModifyDefinition(swSheetMetal, swModel, null);

                var sheetMetalValout = new SheetMetalProperty()
                {
                    BendRadius = Math.Abs(swSheetMetal.BendRadius * 1000),
                    KFactor = swSheetMetal.KFactor,
                    Thickness = Math.Abs(swSheetMetal.Thickness * 1000)
                };

                getsheetmetalproperty.Add(sheetMetalValout);

                //}

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            return getsheetmetalproperty;
        }

        public void SetSheetMetalProperty(ModelDoc2 swModel, Feature swFeat, double BendRadius, double KFactor)
        {

            //var sketches = new List<string> { "Листовой металл1", "Листовой металл", "Sheet-Metal1"};

            var dimensions = new List<string> { "D1", "D2" };
            var sketchName = "";

            foreach (var sketch in Sketches().Where(sketch => swModel.Extension.SelectByID2(sketch, "BODYFEATURE", 0, 0, 0, false, 0, null, 0)))
            {
                sketchName = sketch;
            }

            var swSelMgr = swModel.ISelectionManager;
            swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);

            foreach (
                var dimenison in
                    dimensions.Where(
                        sketch =>
                            swModel.Extension.SelectByID2(
                                sketch + "@" + sketchName + "@" + Path.GetFileName(swModel.GetPathName()), "DIMENSION",
                                0, 0, 0, false, 0, null, 0)).
                        Select(dimension => (DisplayDimension)swSelMgr.GetSelectedObject6(1, 0)).
                        Select(swDisplayDimension => swDisplayDimension.GetDimension2(0).
                            SetSystemValue3(0.1, (int)swSetValueInConfiguration_e.swSetValue_InThisConfiguration,
                                swModel.ConfigurationManager.ActiveConfiguration.Name))) { }


            SheetMetalFeatureData swSheetMetal = swFeat.GetDefinition();
            swSheetMetal.BendRadius = BendRadius;
            swSheetMetal.KFactor = KFactor;
            swFeat.ModifyDefinition(swSheetMetal, swModel, null);

        }

        public void GetSheetMetalPropertyVoid(SldWorks swapp)
        {
            try
            {

                if (swapp == null)
                {
                    swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }

                _swmodel = (ModelDoc2)swapp.ActiveDoc;

                _swPartDoc = (PartDoc)_swmodel;

                Feature swFeat = _swPartDoc.FirstFeature();

                var SheetMetalProp = GetSheetMetalProperty(_swmodel, swFeat);

                foreach (var sheetMetalProperty in SheetMetalProp)
                {
                    while (swFeat != null)
                    {
                        string nameTypeFeature = swFeat.GetTypeName2();

                        if (nameTypeFeature == "SheetMetal")
                        {
                            BendRadiusValout = Convert.ToString(sheetMetalProperty.BendRadius);
                            KFactorValout = Convert.ToString(sheetMetalProperty.KFactor);
                            ThicknessValout = Convert.ToString(sheetMetalProperty.Thickness);
                        }
                        swFeat = swFeat.GetNextFeature();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void CheckSheetMetalProperty(string configname, SldWorks swapp, out string SheetMetalMessage)
        {
            SheetMetalMessage = null;
            try
            {


                if (swapp == null)
                {
                    swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }

                _swmodel = (ModelDoc2)swapp.ActiveDoc;

                var swPart = (PartDoc)_swmodel;
                Feature swFeat = swPart.FirstFeature();

                GetSheetMetalPropertyVoid(swapp);

                var bendTable = _toSql.BendTable(ThicknessValout);

                if (bendTable.Rows.Count == 1)
                {

                    foreach (DataRow row in bendTable.Rows)
                    {
                        var colKFactor = row["K-Factor"].ToString();
                        var colBendRadius = row["BendRadius"].ToString();
                        if (colKFactor != KFactorValout || Convert.ToDouble(colBendRadius).ToString() != BendRadiusValout)
                        {
                            var kfactordouble = Convert.ToDouble(colKFactor);
                            var radiusdouble = Math.Abs(Convert.ToDouble(colBendRadius) / 1000);


                            SetSheetMetalProperty(_swmodel, swFeat, radiusdouble, kfactordouble);
                        }
                    }
                }
                else
                {
                    SheetMetalMessage = "Для конфигурации \"" + configname + "\" установлена нестандартная толщина листового металла. Параметры листового металла не будут установлены";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        //
        public List<MatName> GetCustomProperty(string confname, SldWorks swApp)
        {
            try
            {

                var customnamelist = new List<MatName>();

                customnamelist.Clear();

                if (swApp == null)
                {
                    swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }

                _swmodel = swApp.ActiveDoc;

                _swmodel.GetConfigurationByName(confname);

                var swpart = (PartDoc)_swmodel;

                var sMatDb = "";

                var valout = _swmodel.CustomInfo2[confname, "RAL"];
                var valout1 = _swmodel.CustomInfo2[confname, "Тип покрытия"];
                var valout2 = _swmodel.CustomInfo2[confname, "Класс покрытия"];

                var matname = new MatName()
                {
                    Name = swpart.GetMaterialPropertyName2(confname, out sMatDb),

                    ColorSQL = valout,
                    CoatingType = valout1,
                    CoatingClass = valout2

                };

                customnamelist.Add(matname);

                return customnamelist;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        // Получаем все конфигурации
        public List<string> GetConfigurationNames()
        {
            _swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");

            var ConfigNames = new List<string>();
            try
            {
                _swmodel = (ModelDoc2)_swapp.ActiveDoc;

                object[] confignamearray = _swmodel.GetConfigurationNames();

                //ConfigNames.AddRange(from string cfgName in confignamearray let swConf = swmodel.GetConfigurationByName(cfgName) where ((Configuration)swConf).IsDerived() select cfgName);

                foreach (string cfgName in confignamearray)
                {

                    // только верхний уровень конфигураций
                    Configuration swConf = _swmodel.GetConfigurationByName(cfgName);

                    if (swConf.IsDerived() == false)
                    {
                        ConfigNames.Add(cfgName);
                    }

                }

            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return ConfigNames;
        }

        public List<string> GetConfigurationNames(SldWorks swApp)
        {
            var ConfigNames = new List<string>();
            try
            {

                if (swApp == null)
                {
                    swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }

                _swmodel = (ModelDoc2)swApp.ActiveDoc;

                object[] confignamearray = _swmodel.GetConfigurationNames();

                foreach (string cfgName in confignamearray)
                {

                    // только верхний уровень конфигураций
                    Configuration swConf = _swmodel.GetConfigurationByName(cfgName);

                    if (swConf.IsDerived() == false)
                    {
                        ConfigNames.Add(cfgName);
                    }

                }

                //ConfigNames.AddRange(from string cfgName in confignamearray let swConf = _swmodel.GetConfigurationByName(cfgName) where ((Configuration) swConf).IsDerived() == false select cfgName);
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return ConfigNames;
        }

        public string GetActiveConfigurationName()
        {
            _swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            _swmodel = _swapp.ActiveDoc;

            var activeConfName = _swmodel.ConfigurationManager.ActiveConfiguration.Name;
            //DataGridConfig.Rows[Convert.ToInt32(ActiveConfName)].Cells[0].Value = true;
            return activeConfName;
        }

        // Толщина листового металла
        internal void Thickness(string configName, bool sheet)
        {
            //_swapp = new SldWorks { Visible = true };
            _swmodel = (ModelDoc2)_swapp.ActiveDoc;
                if (sheet)
                {
                    _swmodel.DeleteCustomInfo2(configName, "Толщина листового металла");

                    var titleName = _swmodel.GetTitle();

                    _swmodel.AddCustomInfo3(configName, "Толщина листового металла", 30, "\"Толщина@" + configName + "@" + titleName + ".sldprt" + "\"");
                   // _swmodel.ShowConfiguration(configName);
                }
                else
                {
                    _swmodel.DeleteCustomInfo2(configName, "Толщина листового металла");
                }

        }

        internal void GetPropertyBox(string configname)
        {
            const long valueset = 1000; ;
            const int swDocPart = 1;
            const int swDocAssembly = 2;

            _swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            _swmodel = _swapp.ActiveDoc;

            Configuration swConf = _swmodel.GetConfigurationByName(configname);
            if (swConf.IsDerived() == false)
            {
                //swmodel.ShowConfiguration2(configname);
                _swmodel.EditRebuild3();

                if (_swmodel.GetType() == swDocPart)
                {
                    var part = (PartDoc)_swmodel;
                    var box = part.GetPartBox(true);

                    _swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                    _swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                    _swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                    _swmodel.CustomInfo2[configname, "Длина"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(box[0] - box[3]) * valueset), 0));
                    _swmodel.CustomInfo2[configname, "Ширина"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(box[1] - box[4]) * valueset), 0));
                    _swmodel.CustomInfo2[configname, "Высота"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(box[2] - box[5]) * valueset), 0));

                }
                else if (_swmodel.GetType() == swDocAssembly)
                {
                    var swAssy = (AssemblyDoc)_swmodel;

                    var boxAss = swAssy.GetBox((int)swBoundingBoxOptions_e.swBoundingBoxIncludeRefPlanes);

                    _swmodel.AddCustomInfo3(configname, "Длина", 30, "");
                    _swmodel.AddCustomInfo3(configname, "Ширина", 30, "");
                    _swmodel.AddCustomInfo3(configname, "Высота", 30, "");

                    _swmodel.CustomInfo2[configname, "Длина"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(boxAss[0] - boxAss[3]) * valueset), 0));
                    _swmodel.CustomInfo2[configname, "Ширина"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(boxAss[1] - boxAss[4]) * valueset), 0));
                    _swmodel.CustomInfo2[configname, "Высота"] = Convert.ToString(Math.Round(Convert.ToDecimal(Math.Abs(boxAss[2] - boxAss[5]) * valueset), 0));
                }
            }
        }

        internal void DeleteOrAddPropertyColor(string ConfigName, string RAL, string ralRus, string ralErp, string coatingtype, string coatingclass, bool color, SldWorks swApp)
        {

            if (swApp == null)
            {
                //_swapp = new SldWorks { Visible = true };
                swApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }


            _swmodel = (ModelDoc2)swApp.ActiveDoc;

            try
            {
                if (color)
                {

                    _swmodel.DeleteCustomInfo2(ConfigName, "Площадь покрытия");
                    _swmodel.DeleteCustomInfo2(ConfigName, "RAL");
                    _swmodel.DeleteCustomInfo2(ConfigName, "RALRus");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Ral_ERP");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Тип покрытия");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Класс покрытия");

                    var titleName = _swmodel.GetTitle();

                    _swmodel.AddCustomInfo3(ConfigName, "Площадь покрытия", 30, "\"SW-SurfaceArea@@" + ConfigName + "@" + titleName + ".sldprt" + "\"");
                    _swmodel.AddCustomInfo3(ConfigName, "RAL", 30, RAL);
                    _swmodel.AddCustomInfo3(ConfigName, "RALRus", 30, ralRus);
                    _swmodel.AddCustomInfo3(ConfigName, "Ral_ERP", 30, ralErp);

                    _swmodel.AddCustomInfo3(ConfigName, "Тип покрытия", 30, coatingtype);
                    _swmodel.AddCustomInfo3(ConfigName, "Класс покрытия", 30, coatingclass);

                    _swmodel.ShowConfiguration(ConfigName);

                    GetPropertyBox(ConfigName);

                }
                else
                // Если не красим
                {
                    _swmodel.DeleteCustomInfo2(ConfigName, "Площадь покрытия");
                    _swmodel.DeleteCustomInfo2(ConfigName, "RAL");
                    _swmodel.DeleteCustomInfo2(ConfigName, "RALRus");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Ral_ERP");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Длина");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Ширина");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Высота");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Тип покрытия");
                    _swmodel.DeleteCustomInfo2(ConfigName, "Класс покрытия");
                }
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
        }

        // GetMaterialsNameList
        public List<string> GetMaterialsNameList(SldWorks swApp)
        {
            try
            {

                var listMatName = new List<string>();

                SetMaterials matDll = new SetMaterials(swApp);

                var confarray2 = matDll.GetConfigurationNames(swApp);

                foreach (var confname in confarray2)
                {
                    var matname = matDll.GetCustomProperty(confname, swApp);

                    foreach (var customProperty in matname)
                    {
                        listMatName.Add(customProperty.Name);
                    }
                }

                //var matDll = new SetMaterials();

                //var confarray2 = matDll.GetConfigurationNames(swApp);

                //return (from confname in confarray2 from customProperty in matDll.GetCustomProperty(confname, swApp) select customProperty.Name).ToList();

                return listMatName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        // Set Custom Property
        internal void CustomProperty(int lvlId, string configName, string swProp, string materialsNameEng, string codeErp, string CodeMaterial, string descriptCode, SldWorks swApp)
        {
            _swmodel = swApp.ActiveDoc;

            _swmodel.DeleteCustomInfo2(configName, "MaterialID");
            _swmodel.DeleteCustomInfo2(configName, "Материал_ФБ");
            _swmodel.DeleteCustomInfo2(configName, "Код материала");
            _swmodel.DeleteCustomInfo2(configName, "Код");
            _swmodel.DeleteCustomInfo2(configName, "Материал");
            _swmodel.DeleteCustomInfo2(configName, "Материал_Таблица");
            _swmodel.DeleteCustomInfo2(configName, "Material");

            // Имя материала на английском
            _swmodel.AddCustomInfo3(configName, "Material", 30, materialsNameEng);

            var grouped = GetMaterialsNameList(swApp).GroupBy(s => s).Select(group => new { Word = group.Key, Count = group.Count() });

            if (grouped.Count() == 1)
            {
                _swmodel.AddCustomInfo3(configName, "Материал_ФБ", 30, swProp);

                if (swProp == "")
                {
                    _swmodel.DeleteCustomInfo2(configName, "Материал_ФБ");
                }
            }
            else
            {
                _swmodel.AddCustomInfo3(configName, "Материал_ФБ", 30, "<FONT size=1.8> " + Convert.ToChar(10) + "<FONT size=3.5>" + "См. таблицу");

                if (swProp == "")
                {
                    _swmodel.DeleteCustomInfo2(configName, "Материал_ФБ");
                }

                _swmodel.AddCustomInfo3(configName, "Материал_Таблица", 30, swProp);

                if (swProp == "")
                {
                    _swmodel.DeleteCustomInfo2(configName, "Материал_Таблица");
                }
            }

            _swmodel.AddCustomInfo3(configName, "MaterialID", 3, lvlId.ToString());

            _swmodel.AddCustomInfo3(configName, "Код материала", 30, codeErp);
            if (codeErp == "")
            {
                _swmodel.DeleteCustomInfo2(configName, "Код материала");
            }

            _swmodel.AddCustomInfo3(configName, "Код", 30, CodeMaterial);
            if (CodeMaterial == "")
            {
                _swmodel.DeleteCustomInfo2(configName, "Код");
            }

            _swmodel.AddCustomInfo3(configName, "Материал", 30, descriptCode);
            if (descriptCode == "")
            {
                _swmodel.DeleteCustomInfo2(configName, "Материал");
            }

        }

        // Set Materials
        public void ApplyMaterial(string partPath, string confName, int materialID, SldWorks swapp)
        {
            _swmodel = swapp.ActiveDoc;
            swapp.SetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swFileLocationsMaterialDatabases, GlobalPaths.PathToSwComplexFolder); // задаем базы данных материалов

            _swPartDoc = ((PartDoc)(_swmodel));

            const string dbMatName = "vents-materials.sldmat";

            ToSQL addMatXml = new ToSQL();

            if (_swmodel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                Component2 comp = _swmodel.ISelectionManager.GetSelectedObject3(1);
                PartDoc swPartAssem = comp.GetModelDoc();

                //// удаляем материал
                //swPartAssem.SetMaterialPropertyName("", "");

                // применяем материал
                swPartAssem.SetMaterialPropertyName2(confName, dbMatName, addMatXml.AddMaterialtoXml(materialID));
                _swmodel.ClearSelection2(true);
            }
            else if (_swmodel.GetType() == (int)swDocumentTypes_e.swDocPART)
            {
                // Если имя не пустое
                if (partPath != "")
                {
                    //// удаляем материал
                    //swPartDoc.SetMaterialPropertyName("", "");

                    // применяем материал
                    _swPartDoc.SetMaterialPropertyName2(confName, dbMatName, addMatXml.AddMaterialtoXml(materialID));
                    

                    //_swmodel.ForceRebuild3(false);
                    //_swmodel.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, 0);
                    //swapp.CloseDoc(partPath);
                    //swapp.ExitApp();
                    //swapp = null;
                }
                else
                {
                    //// удаляем материал
                    //swPartDoc.SetMaterialPropertyName(confName, "");

                    // применяем материал
                    _swPartDoc.SetMaterialPropertyName2(confName, dbMatName, addMatXml.AddMaterialtoXml(materialID));
                }

            }
        
        
            //catch (Exception ex)
            //{
            //    //swapp.SendMsgToUser(ex.ToString());
            //    MessageBox.Show(ex.Message);
            //    Error = ex.ToString();
            //}
        }

        #region COLOR
        // Set COLOR
        public void SetColor(string configname, string hex, string coatingtype, string coatingclass, SldWorks swapp)
        {
            var colorsql = new ToSQL();

            if (hex == "")
            {
                colorsql.SetRalSql(configname, "", "", "", false, swapp);
                //return;
            }

            else
            {

            if (swapp == null)
            {
                swapp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
            }

            _swmodel = swapp.ActiveDoc;

            ModelDocExtension swModelDocExt = null;
  
            DisplayStateSetting swDisplayStateSetting = null;
            var swComponents = new Component2[1];
            object[] displayStateNames = null;
            object appearances = null;
            object[] appearancesArray = null;
            var swAppearanceSetting = default(AppearanceSetting);
            var newAppearanceSetting = new AppearanceSetting[1];
            ConfigurationManager swConfigMgr = default(ConfigurationManager);
            Configuration swConfig = default(Configuration);

            CheckedDisplayStatesToConfig(swapp);

            int nbrDisplayStates = 0;

            swModelDocExt = (ModelDocExtension)_swmodel.Extension;
            swConfigMgr = (ConfigurationManager)_swmodel.ConfigurationManager;
            swConfig = (Configuration)swConfigMgr.ActiveConfiguration;

            swComponents[0] = null;
            swComponents[0] = swConfig.GetRootComponent3(true);

            _swmodel.ClearSelection2(true);
           
            //Get display state
            swDisplayStateSetting = (DisplayStateSetting)swModelDocExt.GetDisplayStateSetting((int)swDisplayStateOpts_e.swAllDisplayState);
            swDisplayStateSetting.Entities = swComponents;
            swDisplayStateSetting.Option = (int)swDisplayStateOpts_e.swSpecifyDisplayState;
            // Get the names of display states 
            displayStateNames = (object[])swConfig.GetDisplayStates();

            nbrDisplayStates = swConfig.GetDisplayStatesCount();

            for (var i = 0; i <= (nbrDisplayStates - 1); i++)
            {
                var displayStateName = (string)displayStateNames[i];

                displayStateNames[0] = displayStateName; //"<Default>_Состояние отображения 1";
                swDisplayStateSetting.Names = displayStateNames;
            }

            //Change color of selected component in specified display state
            //from default red to green; this is the overriding color
            appearances = swModelDocExt.DisplayStateSpecMaterialPropertyValues[swDisplayStateSetting];
            appearancesArray = (object[])appearances;
            swAppearanceSetting = (AppearanceSetting)appearancesArray[0];

            //Color myColor = Color.FromArgb(0xBEBD7F);
            var myColor =  ColorTranslator.FromHtml("#" + hex);

            int redRgb = myColor.R;
            int greenRgb = myColor.G;
            int blueRgb = myColor.B;
   
            int newColor = Math.Max(Math.Min(redRgb, 255), 0) + Math.Max(Math.Min(greenRgb, 255), 0) * 16 * 16 + Math.Max(Math.Min(blueRgb, 255), 0) * 16 * 16 * 16 * 16;

            swAppearanceSetting.Color = newColor;
            newAppearanceSetting[0] = swAppearanceSetting;
            _swmodel.ClearSelection2(true);
            swModelDocExt.DisplayStateSpecMaterialPropertyValues[swDisplayStateSetting] = newAppearanceSetting;

            colorsql.SetRalSql(configname, hex, coatingtype, coatingclass, true, swapp);
    
            }

        }

        internal void CheckedDisplayStatesToConfig(SldWorks swapp)
        {
            ModelDoc2 swModel = default(ModelDoc2);
            ConfigurationManager swConfigMgr = default(ConfigurationManager);
            string assemblyName = null;

            swModel = swapp.ActiveDoc;
    
            swConfigMgr = (ConfigurationManager)swModel.ConfigurationManager;
            //swConfigMgr.LinkDisplayStatesToConfigurations = false;

            swConfigMgr.LinkDisplayStatesToConfigurations = true;

        }
        #endregion

    }
}