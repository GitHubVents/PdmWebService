using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
