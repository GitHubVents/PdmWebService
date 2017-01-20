using Bullzip.PdfWriter;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.IO;

namespace PDMWebService.Data.Solid.Pdf
{
   public class PdfBuilder : Singleton.AbstractSingeton<PdfBuilder>
    {
        private SldWorks solidWorksApp;
        public PdfBuilder()
        {
           solidWorksApp =  SolidWorksInstance.SldWoksApp; 
          
        }
        /// <summary>
        /// Builds pdf from solid works drawing
        /// </summary>
        /// <param name="document">Document for building pdf</param>
        /// <param name="finalPath">Path to the build file</param>
        /// <returns></returns>
      public  void ConvertDrwToPdf( ModelDoc2 document, string finalPath)
        {
       
            try
            {
                ModelDocExtension swModelDocExt;
                PrintSpecification printSpec;
                Sheet sheet;
                string PRINTERNAME = "Bullzip PDF Printer";

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
                    drawingDoc.ActivateSheet(sheetName);
                    sheet = drawingDoc.GetCurrentSheet();
                    pageSetup = sheet.PageSetup;
                    var sheetPropertis = (double[])sheet.GetProperties2();
                    pdfSettings.PrinterName = PRINTERNAME;
                    if (!Directory.Exists(@"C:\pdf"))
                    {
                        Directory.CreateDirectory(@"C:\pdf");
                    }
                    pdfSettings.SetValue("Output", string.Format(@"C:\pdf\" + tempfileIndex + " temp " + sheetName + " .pdf"));
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
                    swModelDocExt.PrintOut4(PRINTERNAME, "", printSpec);
                    System.Threading.Thread.Sleep(2000);
                }

                tempfileIndex = 0;
                MergePdf(finalPath.ToUpper().Replace("SLDDRW", "PDF"));
              
            }
            catch (System.Exception exception)
            {
                throw new System.Exception("Filed buld pdf file. " + exception.ToString());
            }
           
        }

        /// <summary>
        /// Determinate page orintation
        /// </summary>
        /// <param name="pagesetup"></param>
        /// <param name="propertis"></param>
     private   void Orientation(PageSetup pagesetup, double[] propertis)
        {
            var width = propertis[5];
            var height = propertis[6];

            if (width < height)
            {
                pagesetup.Orientation = (int)swPageSetupOrientation_e.swPageSetupOrient_Portrait;
                SolidWorksInstance.SldWoksApp.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swPageSetupPrinterOrientation, (int)swPageSetupOrientation_e.swPageSetupOrient_Portrait);
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
                SolidWorksInstance.SldWoksApp.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swPageSetupPrinterOrientation, (int)swPageSetupOrientation_e.swPageSetupOrient_Landscape);
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
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\pdf");

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
        }
        /// <summary>
        /// Merge the pdf files-pages.
        /// </summary>
        /// <param name="path"></param>
        private void MergePdf(string path)
        {
            string PRINTERNAME = "Bullzip PDF Printer";
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\pdf");
            List<string> files = new List<string>();

            files.Sort();

            foreach (var item in dirInfo.GetFiles())
            {
                files.Add(item.FullName);
            }

            PdfUtil.Merge(files.ToArray(), path, PRINTERNAME, 5000);
            Logger.ToLog("================\n успешная конвертация. файл сохранен по пути" + path + "\n================");

        }
    }
}
