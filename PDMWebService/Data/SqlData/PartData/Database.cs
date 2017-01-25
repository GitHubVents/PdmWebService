using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PDMWebService.Data.SqlData.PartData
{
    public class Database
    {
        public static void AddDxf(string varFilePath, int idPdm, string configuration, int version, out Exception exception)
        {
            byte[] file;
            exception = null;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            using (var con = new SqlConnection(ExportXmlSql.ConnectionString))
            {
                try
                {
                    con.Open();
                    var sqlCommand = new SqlCommand("ExportDXF", con) { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.Add("@DXF", SqlDbType.VarBinary, file.Length).Value = file;
                    sqlCommand.Parameters.Add("@IDPDM", SqlDbType.Int).Value = idPdm;
                    sqlCommand.Parameters.Add("@Configuration", SqlDbType.NVarChar).Value = configuration;
                    sqlCommand.Parameters.Add("@Version", SqlDbType.Int).Value = version;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    exception = e;
                    //MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n" + "Name - "+ varFilePath + $"\nIDPDM - {idPdm} config {configuration} ver - {version}");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void DeleteDxf(int idPdm, string configuration, int version, out Exception exc)
        {
            exc = null;
            using (var con = new SqlConnection(ExportXmlSql.ConnectionString))
            {
                try
                {
                    con.Open();
                    var sqlCommand = new SqlCommand("DXFDelete", con) { CommandType = CommandType.StoredProcedure };

                    sqlCommand.Parameters.Add("@IDPDM", SqlDbType.Int).Value = idPdm;
                    sqlCommand.Parameters.Add("@Configuration", SqlDbType.NVarChar).Value = configuration;
                    sqlCommand.Parameters.Add("@Version", SqlDbType.Int).Value = version;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    exc = e;
                    //  MessageBox.Show(e.Message + "\n" + e.StackTrace + "\n" + $"IDPDM - {idPdm} config {configuration} ver - {version}");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static void DatabaseFilePut(string varFilePath)
        {
            byte[] file;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlWrite = new SqlCommand("INSERT INTO Parts (DXF, WorkpieceX, WorkpieceY, Thickness, ConfigurationID, Version, PaintX, PaintY, PaintZ, NomenclatureID) Values (@File, @WorkpieceX, @WorkpieceY, @Thickness, @ConfigurationID, @Version, @PaintX, @PaintY, @PaintZ, @NomenclatureID)", varConnection))
            {
                varConnection.Open();
                sqlWrite.Parameters.Add("@WorkpieceX", SqlDbType.Float).Value = 1000;
                sqlWrite.Parameters.Add("@WorkpieceY", SqlDbType.Float).Value = 1250;
                sqlWrite.Parameters.Add("@Thickness", SqlDbType.Float).Value = 2;
                sqlWrite.Parameters.Add("@ConfigurationID", SqlDbType.Float).Value = 2;
                sqlWrite.Parameters.Add("@Version", SqlDbType.Float).Value = 3;

                sqlWrite.Parameters.Add("@PaintX", SqlDbType.Float).Value = 3;
                sqlWrite.Parameters.Add("@PaintY", SqlDbType.Float).Value = 4;
                sqlWrite.Parameters.Add("@PaintZ", SqlDbType.Float).Value = 5;

                sqlWrite.Parameters.Add("@NomenclatureID", SqlDbType.Float).Value = 5;

                sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
                sqlWrite.ExecuteNonQuery();
                varConnection.Close();
            }
        }

        public static SqlDataReader GetFile(string configuration, int idpdm, int version, out Exception exc)
        {
            exc = null;
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlQuery = new SqlCommand(@"select p.DXF from Parts p
                                                    inner join Nomenclature n on p.NomenclatureID = n.NomenclatureID
                                                    inner join [Vents-PDM].dbo.DocumentConfiguration dc on p.ConfigurationID = dc.ConfigurationID
                                                    where n.IDPDM = @IDPDM AND p.Version = @Version AND dc.ConfigurationName = @ConfigurationName",
                                                varConnection))
            {
                varConnection.Open();

                try
                {
                    sqlQuery.Parameters.AddWithValue("@IDPDM", idpdm);
                    sqlQuery.Parameters.AddWithValue("@Version", version);
                    sqlQuery.Parameters.AddWithValue("@ConfigurationName", configuration);

                    return sqlQuery.ExecuteReader();
                }
                catch (Exception e)
                {
                    exc = e;
                    return null;
                }
                finally
                {
                    varConnection.Close();
                }
            }
        }

        public static void SaveFile(byte[] blob, string varPathToNewLocation)
        {
            using (var fs = new FileStream(varPathToNewLocation, FileMode.Create, FileAccess.Write))
                fs.Write(blob, 0, blob.Length);
        }

        public static byte[] DatabaseFileRead(int idpdm, int version, string configuration, out string codeMaterial, out double? thikness, out Exception exc)
        {
            exc = null;
            codeMaterial = null;
            thikness = null;
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlQuery = new SqlCommand(
                                                    @"select p.DXF, m.CodeMaterial, p.Thickness
                                                    from Parts p
                                                    inner join Nomenclature n on p.NomenclatureID = n.NomenclatureID
                                                    Left JOIN MaterialsProp m on p.MaterialID = m.LevelID
                                                    inner join [Vents-PDM].dbo.DocumentConfiguration dc on p.ConfigurationID = dc.ConfigurationID                                                    
                                                    where n.IDPDM = @IDPDM AND p.Version = @Version AND dc.ConfigurationName = @ConfigurationName ",
                                                 varConnection))
            {
                varConnection.Open();

                sqlQuery.Parameters.AddWithValue("@IDPDM", idpdm);
                sqlQuery.Parameters.AddWithValue("@Version", version);
                sqlQuery.Parameters.AddWithValue("@ConfigurationName", configuration);

                try
                {
                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    {
                        try
                        {
                            sqlQueryResult.Read();
                            //MessageBox.Show("RecordsAffected" + sqlQueryResult.RecordsAffected);
                        }
                        catch (Exception ex)
                        {
                            //  MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace + "\n" + idpdm + "\n" + version + "\n" + configuration);
                        }

                        byte[] blob = null;

                        try
                        {
                            blob = new byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                        }
                        catch (Exception ex)
                        {
                            //       MessageBox.Show(ex.ToString() + "\n" + idpdm + "\n" + version + "\n" + configuration, "Нажмите ОК для продолжения");
                            //  MessageBox.Show(ex.ToString());
                            return null;
                        }

                        try
                        {
                            codeMaterial = sqlQueryResult.GetString(1);
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.ToString());
                        }
                        try
                        {
                            //Convert.ToDecimal()
                            thikness = Convert.ToDouble(sqlQueryResult["Thickness"]);
                            //thikness = (double?)sqlQueryResult.GetValue(2);
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.ToString());
                        }
                        return blob;
                    }
                }
                catch (Exception e)
                {
                    exc = e;
                    return null;
                }
                finally
                {
                    varConnection.Close();
                }
            }
        }

        public static void DatabaseFileRead(string varId, string varPathToNewLocation)
        {
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlQuery = new SqlCommand(@"SELECT [DXF] FROM [Parts] WHERE [PartID] = @varId", varConnection))

            {
                varConnection.Open();
                sqlQuery.Parameters.AddWithValue("@varId", varId);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                {
                    sqlQueryResult.Read();
                    var blob = new byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                    sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                    using (var fs = new FileStream(varPathToNewLocation, FileMode.Create, FileAccess.Write))
                        fs.Write(blob, 0, blob.Length);
                }
                varConnection.Close();
            }


        }

        public static MemoryStream DatabaseFileRead(string varId)
        {
            var memoryStream = new MemoryStream();
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlQuery = new SqlCommand(@"SELECT [RaportPlik] FROM [dbo].[Raporty] WHERE [RaportID] = @varID", varConnection))
            {
                sqlQuery.Parameters.AddWithValue("@varID", varId);
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                {
                    sqlQueryResult.Read();
                    var blob = new byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                    sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                    //using (var fs = new MemoryStream(memoryStream, FileMode.Create, FileAccess.Write)) {
                    memoryStream.Write(blob, 0, blob.Length);
                    //}
                }
            }
            return memoryStream;
        }

        public static int DatabaseFilePut(MemoryStream fileToPut)
        {
            var varId = 0;
            var file = fileToPut.ToArray();
            const string preparedCommand = @"
                    INSERT INTO [dbo].[Raporty]
                               ([RaportPlik])
                         VALUES
                               (@File)
                        SELECT [RaportID] FROM [dbo].[Raporty]
            WHERE [RaportID] = SCOPE_IDENTITY()
                    ";
            using (var varConnection = new SqlConnection(ExportXmlSql.ConnectionString))
            using (var sqlWrite = new SqlCommand(preparedCommand, varConnection))
            {
                sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;

                using (var sqlWriteQuery = sqlWrite.ExecuteReader())
                    while (sqlWriteQuery.Read())
                    {
                        varId = sqlWriteQuery["RaportID"] as int? ?? 0;
                    }
            }
            return varId;
        }

        void Temp()
        {
            byte[] data = null;
            var xml = Encoding.UTF8.GetString(data);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            // TODO: do something with the resulting XmlDocument


            DataTable dataTable = null;
            using (var stream = new MemoryStream(data))
            {
                dataTable.ReadXml(stream);
            }
        }

    }
}
