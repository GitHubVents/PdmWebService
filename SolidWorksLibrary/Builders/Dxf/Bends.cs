using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;


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


        public void Fix()
        {
            //    MessageObserver.Instance.SetMessage("Bust features ");
            GetBendsInfo(ConfigurationName);
            //    MessageObserver.Instance.SetMessage("FixOneBand");
            FixOneBand(ConfigurationName, true);

        }


        private Feature _swFeat;
        private Feature _swSubFeat;

        public class PartBendInfo
        {
            public string Config { get; set; }
            public string EdgeFlange { get; set; }
            public string OneBend { get; set; }
            public bool IsSupressed { get; set; }
        }

        bool IsSheetFeature(string name)
        {
            switch (name)
            {
                case "EdgeFlange":
                case "FlattenBends":
                case "SMBaseFlange":
                case "SheetMetal":
                case "SM3dBend":
                case "SMMiteredFlange":
                case "ProcessBends":
                case "FlatPattern":
                case "Hem":
                case "Jog":
                case "LoftedBend":
                case "Rip":
                case "CornerFeat":

                case "MirrorSolid": 
                    return true;
                default: return false;
            }
        }

        public List<PartBendInfo> PartBendInfos = new List<PartBendInfo>();

        private void GetBendsInfo(string config)
        {
            var swPart = (IPartDoc)modelDoc;
            _swFeat = (Feature)modelDoc.FirstFeature();
            while ((_swFeat != null))
            {
                if (IsSheetFeature(_swFeat.GetTypeName()))
                {
                    var parentFeatureName = _swFeat.Name;
                    var stateOfEdgeFlange = IsSuppressedEdgeFlange(parentFeatureName);
                    _swSubFeat = _swFeat.IGetFirstSubFeature();

                    while ((_swSubFeat != null))
                    {
                        if (_swSubFeat.GetTypeName() == "OneBend" || _swSubFeat.GetTypeName() == "SketchBend")
                        {
                            PartBendInfos.Add(new PartBendInfo
                            {
                                Config = config,
                                EdgeFlange = parentFeatureName,
                                OneBend = _swSubFeat.Name,
                                IsSupressed = stateOfEdgeFlange
                            });

                            _swSubFeat.Select(false);

                            //if (stateOfEdgeFlange)
                            //{
                            //    swPart.EditSuppress();
                            //}
                            //else
                            //{
                            //    swPart.EditUnsuppress();
                            //}

                            _swSubFeat.DeSelect();


                        }
                        _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
                    }
                }
                _swFeat = (Feature)_swFeat.GetNextFeature();
            }
        }

        private void FixOneBand(string config, bool makeDxf)
        {
            var swPart = (IPartDoc)modelDoc;

            _swFeat = (Feature)modelDoc.FirstFeature();
            Feature flatPattern = null;

            while ((_swFeat != null))
            {
                if (_swFeat.GetTypeName() == "FlatPattern")
                {
                    flatPattern = _swFeat;
                    flatPattern.Select(true);
                    swPart.EditUnsuppress();
                    flatPattern.DeSelect();

                    _swSubFeat = (Feature)flatPattern.GetFirstSubFeature();

                    while ((_swSubFeat != null))
                    {
                        if (_swSubFeat.GetTypeName() == "UiBend")
                        {
                            var supression =
                                PartBendInfos.Where(x => x.OneBend == GetOneBandName(_swSubFeat.Name))
                                .Single(x => x.Config == config)
                                .IsSupressed;

                            _swSubFeat.Select(false);

                            if (supression)
                            {
                                swPart.EditSuppress();
                            }
                            //else
                            //{
                            //    swPart.EditUnsuppress();
                            //}

                            _swSubFeat.DeSelect();

                        }

                        _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
                    }
                }

                _swFeat = (Feature)_swFeat.GetNextFeature();
            }

            if (makeDxf)
            {
                flatPattern?.Select(true);
                swPart.EditUnsuppress();
                flatPattern?.DeSelect();
            }
            else
            {
                flatPattern?.Select(true);
                swPart.EditSuppress();
                flatPattern?.DeSelect();
            }

            modelDoc.EditRebuild3();
        }

        static string GetOneBandName(string uiName)
        {
            if (!string.IsNullOrEmpty(uiName))
                return uiName.Substring(
                    uiName.IndexOf('<') + 1,
                    uiName.IndexOf('>') - uiName.IndexOf('<') - 1);
            return null;
        }


        bool IsSuppressedEdgeFlange(string featureName)
        {
            var state = false;
            modelDoc.Extension.SelectByID2(featureName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            var swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
            var swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
            var states = (bool[])swFeat.IsSuppressed2(1, modelDoc.GetConfigurationNames());
            state = states[0];
            modelDoc.ClearSelection2(true);
            return state;
        }
    }
}

