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
  public  class FrameCaseBuilder
    {
        /// <summary>
        /// Destination folder to save spigot
        /// </summary>
        public string FrameDestinationFolder { get; set; } = @"01-Frame";
        /// <summary>
        ///  Spigot source folder
        /// </summary>
        public string FeametFolder { get; set; } = @"01 - Frame 50mm";
        /// <summary>
        /// Root folder file system
        /// </summary>
        public string RootFolder { get; set; } = @"C:\TestPDM";


        enum ServiceSide { Left = 1, Right=2}

        private const double step = 100;
        const string modelName = "01-001-50.SLDASM";
        public   void Build(int width, int  height, int lenght , int profileType, ServiceSide serviceSide)
        {



            string caseAssemblyPath = Path.Combine(new string[] { RootFolder, FrameDestinationFolder, modelName });
            ModelDoc2 swDoc = SolidWorksAdapter.OpenDocument(caseAssemblyPath, swDocumentTypes_e.swDocASSEMBLY);

            SolidWorksAdapter.SldWoksAppExemplare.Visible = true;
            var swAsm = (AssemblyDoc)swDoc;
            swAsm.ResolveAllLightWeightComponents(false);          
          
           
            double rivetL;

            //Lenght

            string newName = "01-P150-45-" + (lenght - 140);
            var newPartPath = $@"{RootFolder}{FeametFolder}{newName}.SLDPRT";
            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
                swDoc.Extension.SelectByID2("01-P150-45-1640-27@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                    0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", true, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P150-45-1640.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                rivetL = (Math.Truncate((lenght - 170) / step) + 1) * 1000;
                SwPartParamsChangeWithNewName("01-P150-45-1640",
                    $@"{RootFolder}{FeametFolder}{newName}",
                    new[,]
                    {
                        {"D1@Вытянуть1", Convert.ToString(lenght - 140)},
                        {"D1@Кривая1", Convert.ToString(rivetL)}
                    },
                    false,
                    null);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            }

            //Width

            newName = "01-P150-45-" + (width - 140);
            newPartPath = $@"{RootFolder}{FeametFolder}{newName}.SLDPRT";
            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
                swDoc.Extension.SelectByID2("01-003-50-22@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", true, true);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-003-50.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                rivetL = (Math.Truncate((width - 170) / step) + 1) * 1000;
                SwPartParamsChangeWithNewName("01-003-50",
                    $@"{RootFolder}{FeametFolder}{newName}",
                    new[,]
                    {
                        {"D1@Вытянуть1", Convert.ToString(width - 140)},
                        {"D1@Кривая1", Convert.ToString(rivetL)}
                    },
                    false,
                    null);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName); 
            }

            //01-P252-45-770
            newName = "01-P252-45-" + (width - 100);
            newPartPath = $@"{RootFolder}{FeametFolder}{newName}.SLDPRT";
            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
                swDoc.Extension.SelectByID2("01-P252-45-770-6@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0, 0,
                    false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", true, true);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P252-45-770.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                SwPartParamsChangeWithNewName("01-P252-45-770",
                    $@"{RootFolder}{FeametFolder}{newName}",
                    new[,] { { "D1@Вытянуть1", Convert.ToString(width - 100) } },
                    false,
                    null);
                SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            }

            //Height

            newName = "01-P150-45-" + (height - 140);
            newPartPath = $@"{RootFolder}{FeametFolder}{newName}.SLDPRT";
            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
                swDoc.Extension.SelectByID2("01-P150-45-510-23@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                    0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", true, true);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P150-45-510.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                rivetL = (Math.Truncate((height - 170) / step) + 1) * 1000;
                SwPartParamsChangeWithNewName("01-P150-45-510",
                    $@"{RootFolder}{FeametFolder}{newName}",
                    new[,]
                    {
                        {"D1@Вытянуть1", Convert.ToString(height - 140)},
                        {"D1@Кривая1", Convert.ToString(rivetL)}
                    },
                    false,
                    null);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            }

            //  01-P252-45-550
            newName = "01-P252-45-" + (height - 100);
            newPartPath = $@"{RootFolder}{FeametFolder}{newName}.SLDPRT";
            if (File.Exists(new FileInfo(newPartPath).FullName))
            {
                swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
                swDoc.Extension.SelectByID2("01-P252-45-550-10@" + modelName.Replace(".SLDASM", ""), "COMPONENT", 0, 0,
                    0, false, 0, null, 0);
                swAsm.ReplaceComponents(newPartPath, "", true, true);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("01-P252-45-550.SLDPRT");
            }
            else if (File.Exists(newPartPath) != true)
            {
                SwPartParamsChangeWithNewName("01-P252-45-550",
                    $@"{RootFolder}{FeametFolder}{newName}",
                    new[,] { { "D1@Вытянуть1", Convert.ToString(height - 100) } },
                    false,
                    null);
              SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
            }

            swDoc = SolidWorksAdapter.AcativeteDoc(modelName);
            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(true);
            swAsm = (AssemblyDoc)swDoc;

            if (serviceSide == ServiceSide.Left)
            {
                swDoc.Extension.SelectByID2("M8-Panel block-one side-1@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                    0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("M8-Panel block-one side-6@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                    0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("M8-Panel block-one side-7@" + modelName.Replace(".SLDASM", ""), "COMPONENT",
                    0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-10@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-4@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-12@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-12@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }
            else if (serviceSide ==  ServiceSide.Right)
            {
                swDoc.Extension.SelectByID2("M8-Panel block-one side-16@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-21@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-22@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("M8-Panel block-one side-17@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("Threaded Rivets с насечкой-23@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("M8-Panel block-one side-18@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, true, 0, null, 0);
                swDoc.Extension.SelectByID2("M8-Panel block-one side-18@" + modelName.Replace(".SLDASM", ""),
                    "COMPONENT", 0, 0, 0, false, 0, null, 0);
                swDoc.EditDelete();
            }

        }
    }
}
