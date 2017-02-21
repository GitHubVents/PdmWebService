using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataBaseDomian.XML
{
    public class ExportToXml
    {
        //private const string XML_PATH = @"\\Pdmsrv\xml\";
        private const string XML_DIRECTORY_PATH = @"D:\xml\";
        public static void ExportPartDataToXml(IEnumerable<Specification> dataToExport)
        {
            CheckAndCreateDirectory();


            foreach (var item in dataToExport)
            {


                try
                {
                    var myXml = new XmlTextWriter(XML_DIRECTORY_PATH + item.FileName.Replace(".SLDPRT", "") + ".xml", Encoding.UTF8);

                    myXml.WriteStartDocument();
                    myXml.Formatting = Formatting.Indented;
                    myXml.Indentation = 2;

                    // создаем элементы
                    myXml.WriteStartElement("xml");
                    myXml.WriteStartElement("transactions");
                    myXml.WriteStartElement("transaction");

                    myXml.WriteStartElement("document");

                    //foreach (var configData in dataToExport)
                    //{
                   

                    try
                    {
                        // Конфигурация
                        myXml.WriteStartElement("configuration");
                        myXml.WriteAttributeString("name", item.Configuration);

                        // Материал
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Материал");
                        myXml.WriteAttributeString("value", /*configData.Материал*/ string.Empty);
                        myXml.WriteEndElement();

                        // Наименование  -- Из таблицы свойств
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Наименование");
                        myXml.WriteAttributeString("value", item.Description);
                        myXml.WriteEndElement();

                        // Обозначение
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Обозначение");
                        myXml.WriteAttributeString("value", item.PartNumber);
                        myXml.WriteEndElement();

                        // Площадь покрытия
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Площадь покрытия");
                        myXml.WriteAttributeString("value", Convert.ToString(item.SurfaceArea).Replace(",", "."));
                        myXml.WriteEndElement();

                        // ERP code
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Код_Материала");
                        myXml.WriteAttributeString("value", item.CodeMaterial);
                        myXml.WriteEndElement();

                        // Длина граничной рамки

                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Длина граничной рамки");
                        myXml.WriteAttributeString("value", item.WorkpieceX);
                        myXml.WriteEndElement();

                        // Ширина граничной рамки
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Ширина граничной рамки");
                        myXml.WriteAttributeString("value", item.WorkpieceY);
                        myXml.WriteEndElement();

                        // Сгибы
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Сгибы");
                        myXml.WriteAttributeString("value", item.Bend);
                        myXml.WriteEndElement();

                        // Толщина листового металла
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Толщина листового металла");
                        myXml.WriteAttributeString("value", item.Thickness);
                        myXml.WriteEndElement();

                        // PaintX
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintX");
                        myXml.WriteAttributeString("value", Convert.ToString(item.PaintX).Replace(",", "."));
                        myXml.WriteEndElement();

                        // PaintY
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintY");
                        myXml.WriteAttributeString("value", Convert.ToString(item.PaintY).Replace(",", "."));
                        myXml.WriteEndElement();

                        // PaintZ
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "PaintZ");
                        myXml.WriteAttributeString("value", Convert.ToString(item.PaintZ).Replace(",", "."));
                        myXml.WriteEndElement();


                        // Версия последняя
                        myXml.WriteStartElement("attribute");
                        myXml.WriteAttributeString("name", "Версия");
                        myXml.WriteAttributeString("value", item.Version);
                        myXml.WriteEndElement();

                        myXml.WriteEndElement();  //configuration
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"{ex.ToString()}\n + {ex.StackTrace}\n (Name - {configData.FileName} ID - {configData.IdPdm} Conf - {configData.Config} Ver - {configData.Version})");

                        throw ex;
                    }
                    myXml.WriteEndElement();// ' элемент DOCUMENT
                    myXml.WriteEndElement();// ' элемент TRANSACTION
                    myXml.WriteEndElement();// ' элемент TRANSACTIONS
                    myXml.WriteEndElement();// ' элемент XML
                                            // заносим данные в myMemoryStream
                    myXml.Flush();

                    myXml.Close();

                }

                catch
                {

                }
            }
        }
        private static void CheckAndCreateDirectory()
        {
          
            if (!Directory.Exists(XML_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(XML_DIRECTORY_PATH);
            }
        }
    }
}
 


//        static void ВыгрузитьСборкуПеречень(List<Epdm.BomCells> спецификация, IEnumerable<string> списокКонфигурацийСборки, out List<Exception> exc)
//        {

//            exc = new List<Exception>();

//            var путь = спецификация[0].FilePath + "\\" + спецификация[0].FileName;

//            try
//            {

//                if (списокКонфигурацийСборки != null)
//                    foreach (var config in списокКонфигурацийСборки)
//                    {
//                        спецификация = Bom(путь, config, out exc);

//                        //TODO Проверка на наличие XML 
//                        //var exist = false;
//                        //try
//                        //{
//                        //    int? lastVerOfAsm = спецификация[0].ПоследняяВерсия;
//                        //    if (lastVerOfAsm != null) exist = ExistLastXml(путь, (int)lastVerOfAsm, true);
//                        //}
//                        //catch (Exception e)
//                        //{
//                        //    exc.Add(e);
//                        //}
//                        //if (exist) return;                               

//                        var myXml = new XmlTextWriter(xmlPath + Path.GetFileNameWithoutExtension(путь) + "-" + config + " Parts.xml", Encoding.UTF8);

//                        myXml.WriteStartDocument();
//                        myXml.Formatting = Formatting.Indented;
//                        myXml.Indentation = 2;

//                        // создаем элементы
//                        myXml.WriteStartElement("xml");
//                        myXml.WriteStartElement("transactions");
//                        myXml.WriteStartElement("transaction");

//                        myXml.WriteAttributeString("vaultName", "Vents-PDM");
//                        myXml.WriteAttributeString("type", "wf_export_document_attributes");
//                        myXml.WriteAttributeString("date", "1416475021");

//                        // document
//                        myXml.WriteStartElement("document");
//                        myXml.WriteAttributeString("pdmweid", "73592");
//                        myXml.WriteAttributeString("aliasset", "Export To ERP");

//                        foreach (var topAsm in спецификация.Where(x => x.Уровень == 0))
//                        {
//                            #region XML

//                            // Конфигурация
//                            myXml.WriteStartElement("configuration");
//                            myXml.WriteAttributeString("name", topAsm.Конфигурация);

//                            // Версия
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Версия");
//                            myXml.WriteAttributeString("value", topAsm.ПоследняяВерсия.ToString());
//                            myXml.WriteEndElement();

//                            // Масса
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Масса");
//                            myXml.WriteAttributeString("value", topAsm.Weight);
//                            myXml.WriteEndElement();

//                            // Наименование
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Наименование");
//                            myXml.WriteAttributeString("value", topAsm.Наименование);
//                            myXml.WriteEndElement();

//                            // Обозначение
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Обозначение");
//                            myXml.WriteAttributeString("value", topAsm.Раздел == "Материалы" ? "" : topAsm.Обозначение);
//                            // 1C Для раздела "материалов" вставляем ПУСТО в обозначение из-за конфликта с 1С 
//                            myXml.WriteEndElement();

//                            // Раздел
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Раздел");
//                            myXml.WriteAttributeString("value", topAsm.Раздел);
//                            myXml.WriteEndElement();

//                            // ERP code
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "ERP code");
//                            myXml.WriteAttributeString("value", topAsm.ErpCode);
//                            myXml.WriteEndElement();

//                            // Код_Материала
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Код_Материала");
//                            myXml.WriteAttributeString("value", topAsm.CodeMaterial);
//                            myXml.WriteEndElement();

//                            // Код документа
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Код документа");
//                            myXml.WriteAttributeString("value", ""); //topAsm..КодДокумента);
//                            myXml.WriteEndElement();

//                            // Кол. Материала
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Кол. Материала");
//                            myXml.WriteAttributeString("value", topAsm.SummMaterial);
//                            myXml.WriteEndElement();

//                            // Состояние
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Состояние");
//                            myXml.WriteAttributeString("value", topAsm.Состояние);
//                            myXml.WriteEndElement();

//                            // Подсчет ссылок
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Подсчет ссылок");
//                            myXml.WriteAttributeString("value", topAsm.Количество.ToString());
//                            myXml.WriteEndElement();

//                            // Конфигурация
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Конфигурация");
//                            myXml.WriteAttributeString("value", topAsm.Конфигурация);
//                            myXml.WriteEndElement();

//                            // Идентификатор
//                            myXml.WriteStartElement("attribute");
//                            myXml.WriteAttributeString("name", "Идентификатор");
//                            myXml.WriteAttributeString("value", "");
//                            myXml.WriteEndElement();

//                            // references
//                            myXml.WriteStartElement("references");

//                            // document
//                            myXml.WriteStartElement("document");
//                            myXml.WriteAttributeString("pdmweid", "73592");
//                            myXml.WriteAttributeString("aliasset", "Export To ERP");

//                            var allParts =
//                                спецификация.Where(x => x.ТипФайла.ToLower() == "sldprt")
//                                .Where(x => x.Раздел == "Детали" || x.Раздел == "").OrderBy(x => x.FileName).ToList();


//                            foreach (var part in allParts)
//                            {
//                                #region XML

//                                // Конфигурация
//                                myXml.WriteStartElement("configuration");
//                                myXml.WriteAttributeString("name", part.Конфигурация);

//                                // Версия
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Версия");
//                                myXml.WriteAttributeString("value", part.ПоследняяВерсия.ToString());
//                                myXml.WriteEndElement();

//                                // Масса
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Масса");
//                                myXml.WriteAttributeString("value", part.Weight);
//                                myXml.WriteEndElement();

//                                // Наименование
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Наименование");
//                                myXml.WriteAttributeString("value", part.Наименование);
//                                myXml.WriteEndElement();

//                                // Обозначение
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Обозначение");
//                                myXml.WriteAttributeString("value", part.Раздел == "Материалы" ? "" : part.Обозначение);
//                                myXml.WriteEndElement();

//                                // Раздел
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Раздел");
//                                myXml.WriteAttributeString("value", part.Раздел);
//                                myXml.WriteEndElement();

//                                // ERP code
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "ERP code");
//                                myXml.WriteAttributeString("value", part.ErpCode);
//                                myXml.WriteEndElement();

//                                // Код_Материала
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Код_Материала");
//                                myXml.WriteAttributeString("value", part.CodeMaterial);
//                                myXml.WriteEndElement();

//                                // Код документа
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Код документа");
//                                myXml.WriteAttributeString("value", ""); // topLevel.КодДокумента);
//                                myXml.WriteEndElement();

//                                // Кол. Материала
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Кол. Материала");
//                                myXml.WriteAttributeString("value", part.SummMaterial);
//                                myXml.WriteEndElement();

//                                // Состояние
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Состояние");
//                                myXml.WriteAttributeString("value", part.Состояние);
//                                myXml.WriteEndElement();

//                                // Подсчет ссылок
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Подсчет ссылок");
//                                myXml.WriteAttributeString("value", part.Количество.ToString());
//                                myXml.WriteEndElement();

//                                // Конфигурация
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Конфигурация");
//                                myXml.WriteAttributeString("value", part.Конфигурация);
//                                myXml.WriteEndElement();

//                                // Идентификатор
//                                myXml.WriteStartElement("attribute");
//                                myXml.WriteAttributeString("name", "Идентификатор");
//                                myXml.WriteAttributeString("value", "");
//                                myXml.WriteEndElement();

//                                myXml.WriteEndElement(); //configuration

//                                #endregion
//                            }

//                            myXml.WriteEndElement(); // document
//                            myXml.WriteEndElement(); // элемент references
//                            myXml.WriteEndElement(); // configuration

//                            #endregion
//                        }



//                        myXml.WriteEndElement(); // ' элемент DOCUMENT
//                        myXml.WriteEndElement(); // ' элемент TRANSACTION
//                        myXml.WriteEndElement(); // ' элемент TRANSACTIONS
//                        myXml.WriteEndElement(); // ' элемент XML
//                                                 // заносим данные в myMemoryStream
//                        myXml.Flush();

//                        myXml.Close();
//                    }
//            }
//            catch (Exception e)
//            {
//                exc.Add(e);
//            }
//        }
//    }
//}
