
//using ServiceLibrary.DataContracts;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PDMWebService.Data
//{
//    public abstract class DataConverter
//    {
//        public static int ToInt(string param)
//        {
//            return Convert.ToInt32(param);
//        }

//        public static bool IsConvertToInt(IEnumerable<string> newStringParams)
//        {
//            foreach (var param in newStringParams)
//            {
//                try
//                {
//                    var y = Convert.ToInt32(param);
//                }
//                catch (Exception)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }

//        public static bool IsConvertToDouble(IEnumerable<string> newStringParams)
//        {
//            foreach (var value in newStringParams)
//            {
//                try
//                {
//                    Convert.ToDouble(value);
//                }
//                catch (Exception exception)
//                {
//                    //MessageBox.Show(exception.ToString() + " Повторите ввод данных!");
//                    return false;
//                }
//            }
//            return true;
//        }


//        public static List<BomShell> BomTableToBomList(DataTable table)
//        {
//            List<BomShell> BoomShellList = new List<BomShell>(table.Rows.Count);

//            BoomShellList.AddRange(from DataRow row in table.Rows
//                                   select row.ItemArray into values
//                                   select new BomShell
//                                   {
//                                       Partition = values[0].ToString(),
//                                       Designation = values[1].ToString(),
//                                       Name = values[2].ToString(),
//                                       Material = values[3].ToString(),
//                                       MaterialCmi = values[4].ToString(),
//                                       SheetThickness = values[5].ToString(),
//                                       Count = Convert.ToDecimal(values[6]),
//                                       FileType = values[7].ToString(),
//                                       Configuration = values[8].ToString(),
//                                       LastVesion = Convert.ToInt32(values[9]),
//                                       IdPdm = Convert.ToInt32(values[10]),
//                                       FileName = values[11].ToString(),
//                                       FilePath = values[12].ToString(),
//                                       ErpCode = values[13].ToString(),
//                                       SummMaterial = values[14].ToString(),
//                                       Weight = values[15].ToString(),
//                                       CodeMaterial = values[16].ToString(),
//                                       Format = values[17].ToString(),
//                                       Note = values[18].ToString(),
//                                       Level = Convert.ToInt32(values[19]),
//                                       ConfigurationMainAssembly = values[20].ToString(),
//                                       TypeObject = values[21].ToString(),
//                                       GetPathName = values[22].ToString()
//                                   });

//            //LoggerInfo("Список из полученой таблицы успешно заполнен элементами в количестве" + bomList.Count);
//            return BoomShellList;
//        }
//    }
//}
