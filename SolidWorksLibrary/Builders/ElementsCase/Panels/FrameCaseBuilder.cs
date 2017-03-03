using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels
{
    public enum ServiceSide
    {
        Left = 1,
        Right = 2
    }

    public class FrameCaseBuilder : ProductBuilderBehavior
    {
        public FrameCaseBuilder() : base()
        {
            base.SetProperties(@"01-Frame", @"01 - Frame 50mm");

        }
        private const double step = 100;
        const string modelName = "01-001-50.SLDASM";

        public string GetFrameCasePath(string modelName)
        {
            string path = Path.Combine(RootFolder, SubjectDestinationFolder, modelName); 
            return path;
        }

        public void Build(Vector3 frameSize, int profileType, ServiceSide serviceSide)
        {
            string caseAssemblyPath = Path.Combine(RootFolder, SourceFolder, modelName);
            Patterns.Observer.MessageObserver.Instance.SetMessage("\n" + caseAssemblyPath + "\n");
            ModelDoc2 SolidWorksDocument = SolidWorksAdapter.OpenDocument(caseAssemblyPath, SolidWorksDocumentumentTypes_e.SolidWorksDocumentASSEMBLY);
            Patterns.Observer.MessageObserver.Instance.SetMessage("открылась сборка");
             AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument( SolidWorksDocument); 
            double rivetL;
            string newName = "01-P150-45-" + (frameSize.Z - 140);
            string newPartPath = GetFrameCasePath(newName);


            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                SolidWorksDocument.Extension.SelectByID2("01-P150-45-1640-27@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                    0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P150-45-1640.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true) // TO DO delegate
            {
                rivetL = (Math.Truncate((frameSize.Z - 170) / step) + 1) * 1000;
                parameters.Add("D1@Вытянуть1", frameSize.Z - 140);
                parameters.Add("D1@Кривая1", rivetL);
                EditPartParameters("01-P150-45-1640", newPartPath);

                //frameSize.X

                newName = "01-P150-45-" + (frameSize.X - 140);
                newPartPath = GetFrameCasePath(newName);
                if (File.Exists(new FileInfo(newPartPath).FullName))
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                    SolidWorksDocument.Extension.SelectByID2("01-003-50-22@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                        false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-003-50.SLDPRT");
                }
                else
                {
                    rivetL = (Math.Truncate((frameSize.X - 170) / step) + 1) * 1000;
                    parameters.Add("D1@Вытянуть1", frameSize.X - 140);
                    parameters.Add("D1@Кривая1", rivetL);
                    EditPartParameters("01-P150-45-1640", newPartPath);
                }

                //01-P252-45-770
                newName = "01-P252-45-" + (frameSize.X - 100);
                newPartPath = newPartPath = GetFrameCasePath(newName);
                if (File.Exists(new FileInfo(newPartPath).FullName))
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                    SolidWorksDocument.Extension.SelectByID2("01-P252-45-770-6@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                        false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P252-45-770.SLDPRT");
                }
                else
                {
                    parameters.Add("D1@Вытянуть1", frameSize.X - 100);
                    EditPartParameters("01-P150-45-1640", newPartPath);
                }

                //frameSize.Y

                newName = "01-P150-45-" + (frameSize.Y - 140);
                newPartPath = newPartPath = GetFrameCasePath(newName);
                if (File.Exists(new FileInfo(newPartPath).FullName))
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                    SolidWorksDocument.Extension.SelectByID2("01-P150-45-510-23@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                        0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P150-45-510.SLDPRT");
                }
                else  
                {
                    rivetL = (Math.Truncate((frameSize.Y - 170) / step) + 1) * 1000;

                    parameters.Add("D1@Вытянуть1", (frameSize.Y - 140));
                    parameters.Add("D1@Кривая1", rivetL);
                    EditPartParameters("01-P150-45-1640", newPartPath);
                }

                //  01-P252-45-550
                newName = "01-P252-45-" + (frameSize.Y - 100);
                newPartPath = newPartPath = GetFrameCasePath(newName);
                if (File.Exists(new FileInfo(newPartPath).FullName))
                {
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                    SolidWorksDocument.Extension.SelectByID2("01-P252-45-550-10@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                        0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P252-45-550.SLDPRT");
                }
                else
                {
                    parameters.Add("D1@Вытянуть1", frameSize.Y - 100);
                    EditPartParameters("01-P252-45-550", newPartPath);
                }

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(modelName);
                SolidWorksDocument.EditRebuild3();
                SolidWorksDocument.ForceRebuild3(true);
                AssemblyDocument = (AssemblyDoc)SolidWorksDocument;

                if (serviceSide == ServiceSide.Left)
                {
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-1@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                        0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-6@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                        0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-7@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                        0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-10@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-4@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-12@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-12@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                else if (serviceSide == ServiceSide.Right)
                {
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-16@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-21@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-22@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-17@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets с насечкой-23@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-18@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("M8-Panel block-one side-18@" + modelName.Replace(".SLDASM", ""),
                        "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
            }
        }       
    }
}
 
