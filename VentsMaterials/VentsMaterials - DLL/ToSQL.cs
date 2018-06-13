using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using SolidWorks.Interop.sldworks;

namespace VentsMaterials
{
    public class ToSQL
    {
        #region Connect to SQL

        static public string Conn
        { 
            get
            {
                return @"Data Source=pdmsrv;Initial Catalog=SWPlusDB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd;MultipleActiveResultSets=True";
            }
        }

        public string Error { get; set; }
    
        #endregion

        #region AddMaterialtoXML

        public DataTable MaterialsTable()
        {
            DataTable materialsTable = new DataTable();

            try
            {
                var query = "select * from MaterialsProp";

                var sqlConnection = new SqlConnection(Conn);
                var sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(materialsTable);

                sqlConnection.Close();

                sqlDataAdapter.Dispose();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return materialsTable;
        }

        // работает по имени материала или по коду материала
        internal string AddMaterialtoXml(int levelID)
        {
            //var ERP = "";
            var densValue = "";//Density
            //var SWProperty = "";
            //var description = "";
            //var CodeMaterial = "";
            var xhatchName = ""; //xhatch
            var xhatchAngle = ""; //angle
            var xhatchScale = ""; //scale
            var pwshader2 = "";
            var path = "";
            var rgb = "";
            var materialName = "";

            //var sqlBaseData = new SqlBaseData();

            var materialDataTable = MaterialsTable();

            foreach (DataRow dataRow in materialDataTable.Rows)
            {
                if (dataRow["LevelID"].ToString() == Convert.ToString(levelID))
                {
                    //ERP = dataRow["ERP"].ToString();
                    densValue = dataRow["Density"].ToString();
                    //SWProperty = dataRow["SWProperty"].ToString();
                    //description = dataRow["description"].ToString();
                    materialName = dataRow["MaterialsName"].ToString();
                    //CodeMaterial = dataRow["CodeMaterial"].ToString();
                    xhatchName = dataRow["xhatch"].ToString();
                    xhatchAngle = dataRow["angle"].ToString();
                    xhatchScale = dataRow["scale"].ToString();
                    pwshader2 = dataRow["pwshader2"].ToString();
                    path = dataRow["path"].ToString();
                    rgb = dataRow["RGB"].ToString();
                }
            }

            //System.IO.MemoryStream myMemoryStream;
            
           
            var myXml = new XmlTextWriter(GlobalPaths.PathToSwComplexMaterial, Encoding.UTF8);

            //создаем XML
            myXml.WriteStartDocument();

            // устанавливаем параметры форматирования xml-документа
            myXml.Formatting = Formatting.Indented;

            // длина отступа
            myXml.Indentation = 2;

            // создаем элементы
            myXml.WriteStartElement("mstns:materials");
            myXml.WriteAttributeString("xmlns:mstns", "http://www.solidworks.com/sldmaterials");
            myXml.WriteAttributeString("xmlns:msdata", "urn:schemas-microsoft-com:xml-msdata");
            myXml.WriteAttributeString("xmlns:sldcolorswatch", "http://www.solidworks.com/sldcolorswatch");
            myXml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            myXml.WriteAttributeString("version", "2008.03");

            //
            myXml.WriteStartElement("curves");
            myXml.WriteAttributeString("id", "curve0");
            //
            myXml.WriteStartElement("point");
            myXml.WriteAttributeString("x", "1.000000");
            myXml.WriteAttributeString("y", "1.000000");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("point");
            myXml.WriteAttributeString("x", "2.000000");
            myXml.WriteAttributeString("y", "1.000000");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("point");
            myXml.WriteAttributeString("x", "3.000000");
            myXml.WriteAttributeString("y", "1.000000");
            myXml.WriteEndElement();
            myXml.WriteEndElement();

            //
            myXml.WriteStartElement("classification");
            myXml.WriteAttributeString("name", "Металл");

            // Material name
            myXml.WriteStartElement("material");
            myXml.WriteAttributeString("name", materialName);  //MaterialName.Text
            myXml.WriteAttributeString("matid", "480");
            myXml.WriteAttributeString("description", "");
            myXml.WriteAttributeString("propertysource", "");
            myXml.WriteAttributeString("appdata", "");

            // xhatch name - штриховка
            myXml.WriteStartElement("xhatch");
            //myXml.WriteAttributeString("name", "ANSI32 (Сталь)")
            myXml.WriteAttributeString("name", xhatchName);  //CboHatch.Text
            //'myXml.WriteAttributeString("angle", "0.0");
            myXml.WriteAttributeString("angle", xhatchAngle);  //TextBox1.Text
            //'myXml.WriteAttributeString("scale", "1.0");
            myXml.WriteAttributeString("scale", xhatchScale);  //TxtDens.Text
            myXml.WriteEndElement();// '\ xhatch name

            // shaders
            myXml.WriteStartElement("shaders");
            // pwshader2
            myXml.WriteStartElement("pwshader2");
            myXml.WriteAttributeString("name", pwshader2);
            //myXml.WriteAttributeString("path", "\plastic\high gloss\" & TreeView1.SelectedNode.Text & ".p2m")
            myXml.WriteAttributeString("path", path);//@"\plastic\high gloss\cream high gloss plastic.p2m"
            myXml.WriteEndElement(); // pwshader2
            myXml.WriteEndElement(); // shaders

            // swatchcolor
            myXml.WriteStartElement("swatchcolor");
            myXml.WriteAttributeString("RGB", rgb); // "D7D0C0"
            myXml.WriteStartElement("sldcolorswatch:Optical");
            myXml.WriteAttributeString("Ambient", "1.000000");
            myXml.WriteAttributeString("Transparency", "0.000000");
            myXml.WriteAttributeString("Diffuse", "1.000000");
            myXml.WriteAttributeString("Specularity", "1.000000");
            myXml.WriteAttributeString("Shininess", "0.310000");
            myXml.WriteAttributeString("Emission", "0.000000");
            myXml.WriteEndElement(); // sldcolorswatch:Optical
            myXml.WriteEndElement(); // swatchcolor

            // physicalproperties
            myXml.WriteStartElement("physicalproperties");
            //
            myXml.WriteStartElement("EX");
            myXml.WriteAttributeString("displayname", "Модуль упругости");
            myXml.WriteAttributeString("value", "2E+011");
            myXml.WriteAttributeString("usepropertycurve", "0");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("NUXY");
            myXml.WriteAttributeString("displayname", "Коэффициент Пуассона");
            myXml.WriteAttributeString("value", "0.29");
            myXml.WriteAttributeString("usepropertycurve", "0");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("DENS");
            myXml.WriteAttributeString("displayname", "Массовая плотность");
            myXml.WriteAttributeString("value", densValue);//Plotnost.Text
            myXml.WriteAttributeString("usepropertycurve", "0");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("SIGXT");
            myXml.WriteAttributeString("displayname", "Предел прочности при растяжении");
            myXml.WriteAttributeString("value", "356901000");
            myXml.WriteAttributeString("usepropertycurve", "0");
            myXml.WriteEndElement();
            //
            myXml.WriteStartElement("SIGYLD");
            myXml.WriteAttributeString("displayname", "Предел текучести");
            myXml.WriteAttributeString("value", "203943000");
            myXml.WriteAttributeString("usepropertycurve", "0");
            myXml.WriteEndElement();
            myXml.WriteEndElement(); // physicalproperties

            // sustainability
            myXml.WriteStartElement("sustainability");
            myXml.WriteAttributeString("linkId", "");
            myXml.WriteAttributeString("dbName", "");
            myXml.WriteEndElement();
            // custom
            myXml.WriteStartElement("custom");
            myXml.WriteEndElement();

            myXml.WriteEndElement(); // Material name

            myXml.WriteEndElement(); // classification name
            myXml.WriteEndElement(); // mstns:materials
            
            // заносим данные в myMemoryStream 
            myXml.Flush();
            myXml.Close();

            return materialName;
        }

        #endregion

        // Применяем свойства из SQL
        public void AddCustomProperty(string configName, int materialID, SldWorks swApp)
        {
            int lvlId;
            string materialsNameEng;
            string codeErp;
            string swProp;
            string codeMateriala;
            string descriptCode;

            DataTable materialDataTable = MaterialsTable();
            SetMaterials setMat = new SetMaterials(swApp);
            foreach (DataRow dataRow in materialDataTable.Rows)
            {
                if ((int)dataRow["LevelID"] == materialID)
                {
                    string Thickness = dataRow["Thickness"].ToString();



                    if (Convert.ToBoolean(Thickness))
                    {
                        setMat.Thickness(configName, true);
                    }
                    else
                    {
                        setMat.Thickness(configName, false);
                    }

                    lvlId = (int)dataRow["LevelID"];
                    materialsNameEng = dataRow["MaterialsNameEng"].ToString();
                    codeErp = dataRow["ERP"].ToString();
                    swProp = dataRow["swProperty"].ToString();
                    codeMateriala = dataRow["CodeMaterial"].ToString();
                    descriptCode = dataRow["MaterialsName"].ToString();

                    setMat.CustomProperty(lvlId, configName, swProp, materialsNameEng, codeErp, codeMateriala, descriptCode, swApp);
                }
            }
            setMat.ApplyMaterial("", configName, materialID, swApp);
        }

        // Имена материалов
        public List<ColumnMatProps> GetAllMatName()
        {
            var SheetName = new List<ColumnMatProps>();
            try
            {
                var sqlconn = new SqlConnection(Conn);
                var sqlcmd = new SqlCommand();
                var sqlda = new SqlDataAdapter();
                sqlconn.Open();
                var ds = new DataSet();
                var dt = new DataTable();
                ds.Tables.Add(dt);
                sqlcmd.CommandText = "select * from MaterialsProp";

                sqlcmd.Connection = sqlconn;
                sqlda.SelectCommand = sqlcmd;
                sqlda.Fill(dt);

                foreach (DataRow datarow in dt.Rows)
                {
                    var ColumnValue = new ColumnMatProps
                    {
                        LevelID = datarow["LevelID"].ToString(),
                        GroupID = datarow["GroupID"].ToString(),
                        MatName = datarow["MaterialsName"].ToString()

                    };

                    SheetName.Add(ColumnValue);
                }

                sqlconn.Close();

            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return SheetName;
        }

        // Листовой металл
        public class ColumnMatProps
        {
            public string LevelID { get; set; }
            public string GroupID { get; set; }
            public string MatName { get; set; }
            
        }
        public List<ColumnMatProps> GetSheetMetalMaterialsName()
        {
            var sheetName = new List<ColumnMatProps>();
            try
            {
                var sqlconn = new SqlConnection(Conn);
                var sqlcmd = new SqlCommand();
                var sqlda = new SqlDataAdapter();

                sqlconn.Open();

                var ds = new DataSet();
                var dt = new DataTable();
                ds.Tables.Add(dt);

                sqlcmd.CommandText = "select LevelID, MaterialsName from MaterialsProp where Thickness =" + 1;

                sqlcmd.Connection = sqlconn;
                sqlda.SelectCommand = sqlcmd;
                sqlda.Fill(dt);

                foreach (DataRow datarow in dt.Rows)
                {
                    var ColumnValue = new ColumnMatProps
                    {
                        LevelID = datarow["LevelID"].ToString(),
                        MatName = datarow["MaterialsName"].ToString()
                    };

                    sheetName.Add(ColumnValue);
                }

                sqlconn.Close();

            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return sheetName;
        }

        // Берем Ral, Rus, ERPCode для применение в DataGrid
        //public class RalSQL
        //{
        //    public string Ral { get; set; }
        //    public string Rus { get; set; }
        //    public string ERPCode { get; set; }
            
        //}

        //public List<RalSQL> SetRalSql(string ConfigName, bool color)
        //{
        //    var RalSql = new List<RalSQL>();
        //    try
        //    {
        //        SqlConnection sqlconn = new SqlConnection(Conn);
        //        SqlCommand sqlcmd = new SqlCommand();
        //        SqlDataAdapter sqlda = new SqlDataAdapter();
        //        sqlconn.Open();
        //        DataSet ds = new DataSet();
        //        DataTable dt = new DataTable();
        //        ds.Tables.Add(dt);
        //        sqlcmd.CommandText = "SELECT ral, Rus, Hex, ERPCode FROM RAL where Applicability =" + 1;

        //        sqlcmd.Connection = sqlconn;
        //        sqlda.SelectCommand = sqlcmd;
        //        sqlda.Fill(dt);
        //        var SetMat = new SetMaterials();
        //        foreach (DataRow datarow in dt.Rows)
        //        {

        //            var ralsql = new RalSQL
        //            {
        //                Ral = datarow["Ral"].ToString(),
        //                Rus = datarow["Rus"].ToString(),
        //                ERPCode = datarow["ERPCode"].ToString()

        //            };

        //            RalSql.Add(ralsql);
        //        }

                
        //        sqlconn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        throw;
        //    }

      
        //    return RalSql;
        //}

        internal void SetRalSql(string configName, string hex, string coatingtype, string coatingclass, bool color, SldWorks swApp)
        {
            var raldatatable = RalTable();
            SetMaterials setMat = new SetMaterials(swApp);
            foreach (DataRow dataRow in raldatatable.Rows)
            {
                if (dataRow["hex"].ToString() == hex)
                {
                    var ral = dataRow["Ral"].ToString();
                    
                    var rus = dataRow["Rus"].ToString();
                    var erpCode = dataRow["ERPCode"].ToString();

                    setMat.DeleteOrAddPropertyColor(configName, ral, rus, erpCode, coatingtype, coatingclass, color, swApp);
                   
                }
            } 
        }

        #region COLOR
        public class ColorTableName
        {
            public string Ral { get; set; }
            public string Rus { get; set; }
            public string Hex { get; set; }
        }
        public List<ColorTableName> GetColorTable()
        {
            var ColorName = new List<ColorTableName>();
            try
            {
                var sqlconn = new SqlConnection(Conn);
                var sqlcmd = new SqlCommand();
                var sqlda = new SqlDataAdapter();

                sqlconn.Open();
                var ds = new DataSet();
                var dt = new DataTable();

                ds.Tables.Add(dt);

                sqlcmd.CommandText = "SELECT 'RAL ' + CAST(RAL AS CHAR(4)) as ral, Rus, Hex FROM RAL WHERE Applicability = 1";

                sqlcmd.Connection = sqlconn;
                sqlda.SelectCommand = sqlcmd;

                sqlda.Fill(dt);

                foreach (DataRow datarow in dt.Rows)
                {
                    var ColumnValue = new ColorTableName
                    {
                        Ral = datarow["RAL"].ToString(),
                        Rus = datarow["Rus"].ToString(),
                        Hex = datarow["HEX"].ToString()
                    };

                    ColorName.Add(ColumnValue);
                }

                sqlconn.Close();

            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            return ColorName;
        }

        public DataTable RalTable()
        {
            var raltable = new DataTable();

            try
            {
                var query = "SELECT RAL, Rus, Hex, ERPCode FROM RAL WHERE Applicability = 1 order by RAL desc";

                var sqlConnection = new SqlConnection(Conn);
                var sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(raltable);

                sqlConnection.Close();

                sqlDataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                //Error = ex.ToString();
                MessageBox.Show(ex.ToString());
            }

            return raltable;
        }

        // Сравнения Thickness
        public DataTable BendTable(string thickness)
        {
            var bendTable = new DataTable();
            try
            {
               // var query = "select * from [Materials].[BendTable] where Thickness = " + "0,5";
                var query = "select * from Materials.BendTable where Thickness = '" + thickness.Replace(",", ".") + "'";
                //var query = "SELECT Cast(Thickness AS decimal (2,1)), Cast([K-Factor]AS decimal (2,1)), Cast(BendRadius AS decimal (2,1)) FROM [Materials].[BendTable] where Thickness = " + thickness;

                var sqlConnection = new SqlConnection(Conn);
                var sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(bendTable);

                sqlConnection.Close();

                sqlDataAdapter.Dispose();
              

            }
            catch (Exception ex)
            {
                //Error = ex.ToString();
                MessageBox.Show(ex.ToString());
            }

            return bendTable;




        }

        #endregion
 
        public void PopulateTreeView(string id, string matname, TreeNode n, TreeView tree)
        {
            try
            {
                TreeNode NN;
                TreeView treeview = tree;

                //MessageBox.Show(Conn);

                var con = new SqlConnection(Conn);
                con.Open();

                if (treeview.Nodes.Count > 0)
                {
                    treeview.Nodes[0].Expand();
                }

                if (n == null)
                {
                    NN = treeview.Nodes.Add(id, matname);
                }
                else
                {
                    NN = n.Nodes.Add(id, matname);
                }


                var cmd = new SqlCommand("select levelID, groupname as MaterialName from MaterialsGroups where GroupID='" + id + 
                    "' union select levelID, MaterialsName from MaterialsProp  where GroupID='" + id + "' order by MaterialName", con);

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PopulateTreeView(dr["levelID"].ToString(), dr["MaterialName"].ToString(), NN, tree);
                }

                con.Close();
                dr.Close();
                cmd.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
    }
}