//using SolidWorks.Interop.sldworks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PDMWebService.Data.Solid.Dxf
//{
//    public class Bends
//    {
//        public static void Fix(SldWorks swApp, out List<SolidWorksFixPattern.PartBendInfo> partBendInfos, bool makeFlat)
//        {
//            var solidWorksMacro = new SolidWorksFixPattern
//            {
//                SwApp = swApp
//            };
//            solidWorksMacro.FixFlatPattern(makeFlat);
//            partBendInfos = solidWorksMacro.PartBendInfos;
//        }

//        public class SolidWorksFixPattern
//        {
//            public void FixFlatPattern(bool makeDxf)
//            {
//                try
//                {
//                    _swModel = (ModelDoc2)SwApp.ActiveDoc;
//                    var getActiveConfig = (Configuration)_swModel.GetActiveConfiguration();
//                    _swModel.EditRebuild3();
//                    GetBendsInfo(getActiveConfig.Name);
//                    FixOneBand(getActiveConfig.Name, makeDxf);
//                }
//                catch (Exception exception)
//                {
//                    //MessageBox.Show(exception.StackTrace);
//                }
//            }

//            public SldWorks SwApp;
//            ModelDoc2 _swModel;
//            Feature _swFeat;
//            Feature _swSubFeat;

//            public class PartBendInfo
//            {
//                public string Config { get; set; }
//                public string EdgeFlange { get; set; }
//                public string OneBend { get; set; }
//                public bool IsSupressed { get; set; }
//            }

//            public List<PartBendInfo> PartBendInfos = new List<PartBendInfo>();

//            static bool IsSheetFeature(string name)
//            {
//                switch (name)
//                {
//                    case "EdgeFlange":
//                    case "FlattenBends":
//                    case "SMBaseFlange":
//                    case "SheetMetal":
//                    case "SM3dBend":
//                    case "SMMiteredFlange":
//                    case "ProcessBends":
//                    case "FlatPattern":
//                    case "Hem":
//                    case "Jog":
//                    case "LoftedBend":
//                    case "Rip":
//                    case "CornerFeat":
//                        return true;
//                    default: return false;

//                }
//            }

//            private void GetBendsInfo(string config)
//            {
//                var swPart = (IPartDoc)_swModel;
//                _swFeat = (Feature)_swModel.FirstFeature();
//                while ((_swFeat != null))
//                {
//                    if (IsSheetFeature(_swFeat.GetTypeName()))
//                    {
//                        var parentFeatureName = _swFeat.Name;
//                        var stateOfEdgeFlange = IsSuppressedEdgeFlange(parentFeatureName);
//                        _swSubFeat = _swFeat.IGetFirstSubFeature();

//                        while ((_swSubFeat != null))
//                        {
//                            if (_swSubFeat.GetTypeName() == "OneBend" || _swSubFeat.GetTypeName() == "SketchBend")
//                            {
//                                PartBendInfos.Add(new PartBendInfo
//                                {
//                                    Config = config,
//                                    EdgeFlange = parentFeatureName,
//                                    OneBend = _swSubFeat.Name,
//                                    IsSupressed = stateOfEdgeFlange
//                                });

//                                _swSubFeat.Select(false);

//                                if (stateOfEdgeFlange)
//                                {
//                                    swPart.EditSuppress();
//                                }
//                                else
//                                {
//                                    swPart.EditUnsuppress();
//                                }

//                                _swSubFeat.DeSelect();


//                            }
//                            _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
//                        }
//                    }
//                    _swFeat = (Feature)_swFeat.GetNextFeature();
//                }
//            }

//            private void FixOneBand(string config, bool makeDxf)
//            {
//                var swPart = (IPartDoc)_swModel;

//                _swFeat = (Feature)_swModel.FirstFeature();
//                Feature flatPattern = null;

//                while ((_swFeat != null))
//                {
//                    if (_swFeat.GetTypeName() == "FlatPattern")
//                    {
//                        flatPattern = _swFeat;
//                        flatPattern.Select(true);
//                        swPart.EditUnsuppress();
//                        flatPattern.DeSelect();

//                        _swSubFeat = (Feature)flatPattern.GetFirstSubFeature();

//                        while ((_swSubFeat != null))
//                        {
//                            if (_swSubFeat.GetTypeName() == "UiBend")
//                            {
//                                try
//                                {

//                                    var supression =
//                                        PartBendInfos.Where(x => x.OneBend == GetOneBandName(_swSubFeat.Name))
//                                        .Single(x => x.Config == config)
//                                        .IsSupressed;

//                                    _swSubFeat.Select(false);

//                                    if (supression)
//                                    {
//                                        swPart.EditSuppress();
//                                    }
//                                    else
//                                    {
//                                        swPart.EditUnsuppress();
//                                    }

//                                    _swSubFeat.DeSelect();


//                                }
//                                catch (Exception)
//                                {
//                                    //MessageBox.Show(exception.StackTrace);
//                                }
//                            }

//                            _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
//                        }
//                    }

//                    _swFeat = (Feature)_swFeat.GetNextFeature();
//                }

//                if (makeDxf)
//                {
//                    flatPattern?.Select(true);
//                    swPart.EditUnsuppress();
//                    flatPattern?.DeSelect();
//                }
//                else
//                {
//                    flatPattern?.Select(true);
//                    swPart.EditSuppress();
//                    flatPattern?.DeSelect();
//                }

//                _swModel.EditRebuild3();
//            }

//            static string GetOneBandName(string uiName)
//            {
//                if (!string.IsNullOrEmpty(uiName))
//                    return uiName.Substring(
//                        uiName.IndexOf('<') + 1,
//                        uiName.IndexOf('>') - uiName.IndexOf('<') - 1);
//                return null;
//            }

//            bool IsSuppressedEdgeFlange(string featureName)
//            {
//                var state = false;
//                try
//                {
//                    _swModel.Extension.SelectByID2(featureName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
//                    var swSelMgr = (SelectionMgr)_swModel.SelectionManager;
//                    var swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
//                    var states = (bool[])swFeat.IsSuppressed2(1, _swModel.GetConfigurationNames());
//                    state = states[0];
//                    _swModel.ClearSelection2(true);
//                }
//                catch (Exception)
//                {

//                }
//                return state;
//            }

//        }
//    }
//}
