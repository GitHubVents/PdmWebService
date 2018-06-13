using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;


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
            FixOneBand(ConfigurationName);
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


        public List<PartBendInfo> PartBendInfos = new List<PartBendInfo>();

        private void FixOneBand(string config)
        {
            try
            {
                IPartDoc swPart = (IPartDoc)modelDoc;

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
                                object[] fisrtParent = _swSubFeat.GetParents();
                                if (fisrtParent != null)
                                {
                                    foreach (var item in fisrtParent)
                                    {
                                        Feature swFirstParentFeat = (Feature)item;
                                        bool SuppressedEdgeFlange = IsSuppressedEdgeFlange(swFirstParentFeat.GetOwnerFeature().Name);

                                        PartBendInfos.Add
                                        (
                                            new PartBendInfo
                                            {
                                                EdgeFlange = _swSubFeat.Name,
                                                OneBend = swFirstParentFeat.GetOwnerFeature().Name,
                                                IsSupressed = SuppressedEdgeFlange,
                                                Config = config
                                            }
                                        );
                                    }
                                }
                            }
                            _swSubFeat = (Feature)_swSubFeat.GetNextSubFeature();
                        }
                    }

                    _swFeat = (Feature)_swFeat.GetNextFeature();

                    foreach (var item in PartBendInfos)
                    {
                        
                        if (!item.IsSupressed)
                        {
                            modelDoc.Extension.SelectByID2(item.EdgeFlange, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                            SelectionMgr swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
                            Feature swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
                            swPart.EditUnsuppress();
                        }
                    }
                }
                //modelDoc.EditRebuild3();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\t StackTrace " + ex.StackTrace);
            }
        }

        bool IsSuppressedEdgeFlange(string featureName)
        {
            modelDoc.Extension.SelectByID2(featureName, "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
            var swSelMgr = (SelectionMgr)modelDoc.SelectionManager;
            var swFeat = (Feature)swSelMgr.GetSelectedObject6(1, -1);
            bool[] states = swFeat.IsSuppressed2(2, modelDoc.GetConfigurationNames());
            bool stat = states[0];
            modelDoc.ClearSelection2(true);
            return stat;
        }
    }
}