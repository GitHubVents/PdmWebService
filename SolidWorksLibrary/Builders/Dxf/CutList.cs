using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Globalization;

namespace SolidWorksLibrary.Builders.Dxf
{
    public static   class CutList
    {

     private static   ModelDoc2 SwModel;
    

        /// <summary>
        /// Returns sheet metal cut list  by configuration name
        /// </summary>
        /// <param name="configuratuinName"></param>
        /// <param name="SwModel"></param>
        /// <returns></returns>
        public static   DataToExport GetDataToExport(   ModelDoc2 swModel)
        {
            SwModel = swModel;
            var dataToExport = new DataToExport();

          //  var swCustProp = swModel.Extension.CustomPropertyManager[configuratuinName];
            string valOut;        

             
            const string BoundingBoxLengthRu = @"Длина граничной рамки"; // rename, change number to eng/rus
            const string BoundingBoxLengthEng = @"Bounding Box Length";
            const string BoundingBoxWidthRu = @"Ширина граничной рамки";
            const string BoundingBoxWidthEng = @"Bounding Box Width";
            const string SheetMetalThicknessRu = @"Толщина листового металла";
            const string SheetMetalThicknessEng = @"Sheet Metal Thickness";
            const string BendsRu = @"Сгибы";
            const string BendsEng = @"Bends";
             
            Feature swFeat2 = SwModel.FirstFeature();
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
                            MessageObserver.Instance.SetMessage("GetTypeName2: " + swSubFeat.GetTypeName2() + "; swSubFeat.Name " + swSubFeat.Name);
                            BodyFolder bodyFolder = swSubFeat.GetSpecificFeature2();

                            if (bodyFolder.GetCutListType() != (int)swCutListType_e.swSheetmetalCutlist)
                            {
                                goto m1;
                            }

                            swSubFeat.Select2(false, -1);
                            bodyFolder.SetAutomaticCutList(true);
                            bodyFolder.UpdateCutList();

                            var swCustProp = swSubFeat.CustomPropertyManager;
                            string tempOutBoundingBoxLength;
                            swCustProp.Get4(BoundingBoxLengthRu, true, out valOut,
                                out tempOutBoundingBoxLength);
                            if (string.IsNullOrEmpty(tempOutBoundingBoxLength))
                            {
                                swCustProp.Get4(BoundingBoxLengthEng, true, out valOut,
                                    out tempOutBoundingBoxLength);
                               
                            }
                          
                         MessageObserver.Instance.SetMessage("\n line 75 cut list: " +tempOutBoundingBoxLength + "\n");
                            dataToExport.BoundingBoxLength =  Convert.ToDecimal(tempOutBoundingBoxLength.Replace(".",","));

                            string ширинаГраничнойРамки;
                            swCustProp.Get4(BoundingBoxWidthRu, true, out valOut,
                                out ширинаГраничнойРамки);
                            if (string.IsNullOrEmpty(ширинаГраничнойРамки))
                            {
                                swCustProp.Get4(BoundingBoxWidthEng, true, out valOut,
                                    out ширинаГраничнойРамки);
                            }
                            //swCustProp.Set(BoundingBoxWidthRu, ширинаГраничнойРамки);
                            dataToExport.BoundingBoxWidth = Convert.ToDecimal( ширинаГраничнойРамки.Replace(".", ","));

                            string толщинаЛистовогоМеталла;
                            swCustProp.Get4(SheetMetalThicknessRu, true, out valOut,
                                out толщинаЛистовогоМеталла);
                            if (string.IsNullOrEmpty(толщинаЛистовогоМеталла))
                            {
                                swCustProp.Get4(SheetMetalThicknessEng, true, out valOut,
                                out толщинаЛистовогоМеталла);
                            }
                           // swCustProp.Set(SheetMetalThicknessRu, толщинаЛистовогоМеталла);
                            dataToExport.Thickness = (float)Convert.ToDouble( толщинаЛистовогоМеталла.Replace(".", ","));

                            string сгибы;
                            swCustProp.Get4(BendsRu, true, out valOut, out сгибы);
                            if (string.IsNullOrEmpty(сгибы))
                            {
                                swCustProp.Get4(BendsEng, true, out valOut, out сгибы);
                            }
                          //  swCustProp.Set(BendsRu, сгибы);
                            dataToExport.Bend = Convert.ToInt32( сгибы );
                         
                            dataToExport.PaintX =  (int)GetDimentions( )[0];
                            dataToExport.PaintY = (int)GetDimentions(  )[1];
                            dataToExport.PaintZ = (int)GetDimentions(   )[2];
                            dataToExport.SurfaceArea = (float)GetSurfaceArea ( );
                        }
                        m1:
                        swSubFeat = swSubFeat.GetNextFeature();
                    }
                }
                swFeat2 = swFeat2.GetNextFeature();
            }
            SwModel = null;
            return dataToExport; 
        }

        private  static decimal GetSurfaceArea ( )
        {
            var myMassProp = SwModel.Extension.CreateMassProperty();
            return Convert.ToDecimal (Math.Round(myMassProp.SurfaceArea * 1000) / 1000); 
        }


        private static decimal[] GetDimentions()
        {
            decimal[] dimentions = new decimal[3];
            const long valueset = 1000;

            var part = (PartDoc)SwModel;
            var box = part.GetPartBox(true);
            dimentions[0] = Convert.ToDecimal(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[0] - box[3]) * valueset)), 0), CultureInfo.InvariantCulture);
            dimentions[1] = Convert.ToDecimal(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[1] - box[4]) * valueset)), 0), CultureInfo.InvariantCulture);
            dimentions[2] = Convert.ToDecimal(Math.Round(Convert.ToDecimal((long)(Math.Abs(box[2] - box[5]) * valueset)), 0), CultureInfo.InvariantCulture);
            return dimentions;
        }
    }
}
