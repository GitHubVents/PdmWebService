using Bullzip.PdfWriter;
using Patterns;
using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidWorksLibrary.Builders.Pdf
{
    public class PdfBuilder :  Singeton<PdfBuilder>
    {
        /// <summary>
        /// The path to final folder
        /// </summary>
        public string PdfFolder { get; set; }

        /// <summary>
        /// The path to temporary folder
        /// </summary>
        public string TempPdfFolder { get; set; }

        /// <summary>
        /// Name virtual printer
        /// </summary>
        public string PrinterName { get; set; } = "Bullzip PDF Printer";
      
        private PdfBuilder() : base()
        {
            MessageObserver.Instance.SetMessage("Create PdfBuilder", MessageType.System);

            // -------- default data --------------
            PdfFolder = @"D:\TEMP\PDF";
            TempPdfFolder = @"D:\TEMP\TEMP PDF";
            // -------------------------------------
        }

        /// <summary>
        /// Build pdf view on based SolidWorks drawwing
        /// </summary>
        /// <param name="pathToDrawing"></param>
        /// <returns></returns>
        public string Build(string pathToDrawing)
        {
            MessageObserver.Instance.SetMessage("Start build pdf file", MessageType.System);
            try
            { 
                return ConvertDrwToPdf(  pathToDrawing);
            }
            catch (Exception exception)
            {
                //Console.WriteLine("Failed open document    " + exception.ToString());
                throw new Exception("Failed open document: " + exception.ToString());
            }
        }

        /// <summary>
        /// Conert solid works drawing to pdf file.
        /// </summary>
        /// <param name="document">Document for building pdf</param>
        /// <param name="finalPath">Path to the build file</param>
        /// <returns></returns>
        private string ConvertDrwToPdf( string path)
        {
            int Errors = 0;
            ModelDoc2 document = SolidWorksAdapter.SldWoksAppExemplare.OpenDoc2(path, (int)swDocumentTypes_e.swDocDRAWING, false, false, true, Errors);
            try
            {
                ModelDocExtension swModelDocExt;
                PrintSpecification printSpec;
                Sheet sheet;

                swModelDocExt = document.Extension;
                // applied to Sheet or App, Doc...
                swModelDocExt.UsePageSetup = (int)swPageSetupInUse_e.swPageSetupInUse_DrawingSheet;
                IDrawingDoc drawingDoc = document as IDrawingDoc;
                PageSetup pageSetup;
                string[] sheetNames = drawingDoc.GetSheetNames();
                PdfSettings pdfSettings = new PdfSettings();
                ClearDirectory();
                int tempfileIndex = 0;
                foreach (var sheetName in sheetNames)
                {
                    //Console.WriteLine(sheetName);
                    drawingDoc.ActivateSheet(sheetName);
                    sheet = drawingDoc.GetCurrentSheet();
                    pageSetup = sheet.PageSetup;
                    var sheetPropertis = (double[])sheet.GetProperties2();
                    pdfSettings.PrinterName = PrinterName;
                    if (!Directory.Exists(TempPdfFolder))
                    {
                        Directory.CreateDirectory(TempPdfFolder);
                    }
                    pdfSettings.SetValue("Output", string.Format(TempPdfFolder + @"\" + tempfileIndex + " temp " + sheetName + " .pdf"));
                    tempfileIndex++;
                    pdfSettings.SetValue("ShowPDF", "no");
                    pdfSettings.SetValue("ShowSettings", "never");
                    pdfSettings.SetValue("ShowSaveAS", "never");
                    pdfSettings.SetValue("ShowProgress", "no");
                    pdfSettings.SetValue("ShowProgressFinished", "no");
                    pdfSettings.SetValue("ConfirmOverwrite", "yes");
                    pdfSettings.WriteSettings(PdfSettingsFileType.RunOnce);
                    pageSetup.DrawingColor = (int)swPageSetupDrawingColor_e.swPageSetup_AutomaticDrawingColor;
                    Orientation(pageSetup, sheetPropertis);
                    printSpec = (PrintSpecification)swModelDocExt.GetPrintSpecification();
                    printSpec.ScaleMethod = (int)swPrintSelectionScaleFactor_e.swPrintCurrentSheet;
                    printSpec.PrintToFile = false;
                    try
                    {
                        swModelDocExt.PrintOut4(PrinterName, "", printSpec);
                        System.Threading.Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Possibly not fount the Bullzip PDF Printer\n" + ex.ToString());
                    }
                }
                tempfileIndex = ResetIndex();
                 string NameAndExtension = System.IO.Path.GetFileName(path);
                string PathToPdfFile = PdfFolder.ToUpper() + @"\" + NameAndExtension.Replace("SLDDRW", "PDF");

              //  string PathToPdfFile = path.Replace("SLDDRW", "PDF");
                MergePdf(PathToPdfFile); // Path to temp file
                return PathToPdfFile;
            }
            catch (System.Exception exception)
            {
                throw new System.Exception("Filed buld pdf file. " + exception.ToString());
            }
        }

        private int ResetIndex()
        {
            return 0;
        }

        /// <summary>
        /// Determinate page orintation
        /// </summary>
        /// <param name="pagesetup"></param>
        /// <param name="propertis"></param>
        private void Orientation(PageSetup pagesetup, double[] propertis)
        {
            var width = propertis[5];
            var height = propertis[6];

            if (width < height)
            {
                pagesetup.Orientation = (int)swPageSetupOrientation_e.swPageSetupOrient_Portrait;
                SolidWorksAdapter.SldWoksAppExemplare.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swPageSetupPrinterOrientation, (int)swPageSetupOrientation_e.swPageSetupOrient_Portrait);
                if (width <= 0.21)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA4sizeVertical;
                }
                else if (width >= 0.21)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA3size;
                }
                else if (width >= 0.297)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA3size;
                }
                else if (width == 0.42)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA2size;
                }
                else if (width >= 0.594)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA1size;
                }
                if (width >= 0.841)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA0size;
                }

            }
            else if (width > height)
            {

                pagesetup.Orientation = (int)swPageSetupOrientation_e.swPageSetupOrient_Landscape;
                SolidWorksAdapter.SldWoksAppExemplare.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swPageSetupPrinterOrientation, (int)swPageSetupOrientation_e.swPageSetupOrient_Landscape);
                if (width < 0.297)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA4sizeVertical;

                }
                else if (width >= 0.297)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA3size;

                }
                else if (width == 0.42)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA3size;

                }
                else if (width >= 0.594)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA2size;

                }
                else if (width >= 0.841)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA1size;

                }
                else if (width >= 1.189)
                {
                    pagesetup.PrinterPaperSize = (int)swDwgPaperSizes_e.swDwgPaperA0size;

                }

            }

            else
            {

            }
        }

        private void ClearDirectory()
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(TempPdfFolder);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Merge the pdf files-pages.
        /// </summary>
        /// <param name="path"></param>
        private void MergePdf(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(TempPdfFolder);
            List<string> files = new List<string>();
            files.Sort();
            foreach (var item in dirInfo.GetFiles())
            {
                files.Add(item.FullName);
            }
            PdfUtil.Merge(files.ToArray(), path, PrinterName, 5000);
           // Logger.ToLog("================\n успешная конвертация. файл сохранен по пути" + path + "\n================");
        }
    }
}
