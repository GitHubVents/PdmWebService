using SolidWorks.Interop.sldworks;
using System;


namespace SolidWorksLibrary.Builders.Dxf
{
    public class Bends
    {
        ModelDoc2 modelDoc;
        public string ConfigurationName;

        public static Bends Create(ModelDoc2 modelDoc, string configurationName)
        {
            try
            {
                return new Bends
                {
                    modelDoc = modelDoc,
                    ConfigurationName = configurationName
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public void Fix()
        //{
        //    FixOneBand(ConfigurationName);
        //}
        //private Feature _swFeat;
        //public class PartBendInfo
        //{
        //    public string Config { get; set; }
        //    public string EdgeFlange { get; set; }
        //    public string OneBend { get; set; }
        //    public bool IsSupressed { get; set; }
        //}
        //public List<PartBendInfo> PartBendInfos = new List<PartBendInfo>();
        //private void FixOneBand(string config)
        //{
        //    try
        //    {
        //        IPartDoc swPart = (IPartDoc)modelDoc;

        //        _swFeat = (Feature)modelDoc.FirstFeature();
        //        Feature flatPattern = null;

        //        while ((_swFeat != null))
        //        {

        //            if (_swFeat.GetTypeName() == "FlatPattern")
        //            {
        //                flatPattern = _swFeat;
        //                flatPattern.Select(true);
        //                swPart.EditUnsuppress();
        //                flatPattern.DeSelect();

        //                _swSubFeat = (Feature)flatPattern.GetFirstSubFeature();

        //                while ((_swSubFeat != null))
        //                {
        //                    if (_swSubFeat.GetTypeName() == "UiBend")
        //                    {
        //                        object[] fisrtParent = _swSubFeat.GetParents();
        //                        if (fisrtParent != null)
        //                        {
        //                            foreach (var item in fisrtParent)
        //                            {
        //                                Feature swFirstParentFeat = (Feature)item;
        //                                bool SuppressedEdgeFlange = IsSuppressedEdgeFlange(swFirstParentFeat.GetOwnerFeature().Name);

        //                                PartBendInfos.Add
        //                                (
        //                                    new PartBendInfo
        //                                    {
        //                                        EdgeFlange = _swSubFeat.Name,
        //                                        OneBend = swFirstParentFeat.GetOwnerFeature().Name,
        //                                        IsSupressed = SuppressedEdgeFlange,
        //                                        Config = config
        //                                    }
        //                                );
        //                            }
        //                        }
        //                    }
        //                    _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
        //                }
        //            }

        //            _swFeat = (Feature)_swFeat.GetNextFeature();

        //            foreach (var item in PartBendInfos)
        //            {
                        
        //                if (!item.IsSupressed)
        //                {
        //                    modelDoc.Extension.SelectByID2(item.EdgeFlange, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
        //                    SelectionMgr swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
        //                    Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
        //                    swPart.EditUnsuppress();
        //                }
        //            }
        //        }
        //        //modelDoc.EditRebuild3();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message + "\t StackTrace " + ex.StackTrace);
        //    }
        //}
        //bool IsSuppressedEdgeFlange(string featureName)
        //{
        //    modelDoc.Extension.SelectByID2(featureName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
        //    var swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
        //    var swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
        //    bool[] states = swFeat.IsSuppressed2(2, modelDoc.GetConfigurationNames());
        //    bool stat = states[0];
        //    modelDoc.ClearSelection2(true);
        //    return stat;
        //}




        Feature __swFeat;
        Feature _swSubFeat;
        SelectionMgr swSelMgr;

        public bool FixEachBend()
        {

            swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
            __swFeat = (Feature)modelDoc.FirstFeature();

            bool sheetMetal = false;
            bool supessed = false;


            while (__swFeat != null)
            {
                // Process top-level sheet metal features
                switch (__swFeat.GetTypeName())
                {
                    case "SMBaseFlange":
                        //Process_SMBaseFlange(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "SheetMetal":
                        //Process_SheetMetal(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "SM3dBend":
                        //Process_SM3dBend(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "SMMiteredFlange":
                        //Process_SMMiteredFlange(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "ProcessBends":
                        //Process_ProcessBends(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "FlattenBends":
                        //Process_FlattenBends(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "EdgeFlange":
                        //Process_EdgeFlange(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "FlatPattern":
                        //Process_FlatPattern(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "Hem":
                        //Process_Hem(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "Jog":
                        //Process_Jog(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "LoftedBend":
                        //Process_LoftedBend(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "Rip":
                        //Process_Rip(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    case "CornerFeat":
                        //Process_CornerFeat(sldWorks, modelDoc, __swFeat);
                        sheetMetal = true;
                        break;
                    default:
                        sheetMetal = false;
                        break;
                        // Probably not a sheet metal feature
                }

                if (sheetMetal)
                {

                    supessed = __swFeat.IsSuppressed();

                    if (!supessed || __swFeat.GetTypeName2() == "FlatPattern")
                    {

                        if (__swFeat.GetTypeName2() == "FlatPattern" && __swFeat.IsSuppressed())
                        {
                            __swFeat.SetSuppression(2);
                        }


                        // process sheet metal sub-features
                        _swSubFeat = (Feature)__swFeat.GetFirstSubFeature();
                        while ((_swSubFeat != null))
                        {
                           
                            if (_swSubFeat.IsSuppressed())
                            {
                                _swSubFeat.SetSuppression(2);
                            }

                            _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
                        }
                    }
                }
                __swFeat = (Feature)__swFeat.GetNextFeature();
            }
            return sheetMetal;
        }
    }
}