using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidWorksLibrary.Builders.ElementsCase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="thickness"></param>
    /// <param name="kFactor"></param>
    /// <param name="bendRadius"></param>
    public delegate void SetBendsHandler(decimal thickness, out decimal kFactor, out decimal bendRadius);


    /// <summary>
    /// It abstract class describes the basic behavior of the builder
    /// </summary>
    public abstract class ProductBuilderBehavior : IFeedbackBuilder
    {
        protected Dictionary<string, double> parameters { get; set; }

        protected string NewPartPath {get;set;}
        #region properties
        /// <summary>
        /// Path list to built files
        /// </summary>
        protected List<string> ComponentsPathList { get; set; }
        /// <summary>
        /// Sorce folder where there is the prototypes for the build
        /// </summary>
        protected string SourceFolder { get; set; }
        /// <summary>
        /// The folder containing files after they are built and save. 
        /// </summary>
        protected string SubjectDestinationFolder { get; set; }
        /// <summary>
        /// Root folder file system
        /// </summary>
        protected string RootFolder { get; set; }
        /// <summary>
        /// Working assembly document
        /// </summary>
        protected AssemblyDoc AssemblyDocument { get; set; }
        /// <summary>
        /// Working SolidWork document { asm, prt }
        /// </summary>
        protected ModelDoc2 SolidWorksDocument { get; set; }
        /// <summary>
        /// Working document name
        /// </summary>
        protected string documentlName { get; set; }
        /// <summary>
        /// Sheetmetal bend radius 
        /// </summary>
        protected decimal BendRadius   = 5; // default value
        /// <summary>
        /// Gets or sets the K-factor
        /// </summary>
        protected decimal KFactor  = 5; // default value
        /// <summary>
        /// Provides notification and feedback to set bends for part
        /// </summary>
        public virtual event SetBendsHandler SetBends;

        #endregion

        public ProductBuilderBehavior()
        {
            this.ComponentsPathList = new List<string>();
            parameters = new Dictionary<string, double>();
        }

        #region SetProperties

        /// <summary>
        /// Assigns necessary paths for build
        /// </summary>
        /// <param name="subjectDestinationFolder">The folder in which will be save the files after built</param>
        /// <param name="sourceFolder">Sorce folder where there is the prototypes for the build</param>
        protected void SetProperties(string subjectDestinationFolder, string sourceFolder)
        {
            this.RootFolder = @"C:\TestPDM"; // test default value
            this.SubjectDestinationFolder = subjectDestinationFolder;
            this.SourceFolder = sourceFolder;
        }
        /// <summary>
        /// Assigns necessary paths for build
        /// </summary>
        /// <param name="subjectDestinationFolder">The folder in which will be save the files after built</param>
        /// <param name="sourceFolder">Sorce folder where there is the prototypes for the build</param>
        /// <param name="rootFolder">The root folder working file system</param>
        protected void SetProperties(string subjectDestinationFolder, string sourceFolder, string rootFolder)
        {
            this.RootFolder = rootFolder;
            this.SubjectDestinationFolder = subjectDestinationFolder;
            this.SourceFolder = sourceFolder;
        }
        #endregion
        // ========================= README about CheckExistPart delegate =====================================================================  
        // When the event is fired, a check runs to find whether the part or assembly exists. It returnes the path to file if file exists 
        // and boolean flag using out operators.it allows not to be bound to file format such as PDM, IPS,SQL, explorer etc

        /// <summary>
        /// Provides notification and feedback to check for part
        /// </summary>
        public CheckExistPartHandler CheckExistPart { get; set; }        

        /// <summary>
        /// Informing subscribers the completion of building 
        /// </summary>
        public FinishedBuildHandler FinishedBuild { get; set; }


        protected virtual void DeleteComponents(int type)
        {
            throw new NotImplementedException("Not implemented DeleteComponents method");
        }

        protected virtual void InitiatorSaveExeption(int error, int warning, string path = "")
        {
            if (error != 0 || warning != 0)
            {
                var exeption = new Exception
                    ("Failed save file " + " error code {" + error + "}, error description: " +
                    (swFileSaveError_e)error + ", warning code {" + warning +
                    "}, warning description: " + (swFileSaveWarning_e)warning);
                MessageObserver.Instance.SetMessage(exeption.ToString(), MessageType.Error);
            }
        }
        protected int errors   = 0;

        protected int warnings   = 0;
        protected virtual void EditPartParameters(string partName, string newPath)
        {
            ModelDoc2 solidWorksDocument = null;
            solidWorksDocument = SolidWorksAdapter.AcativeteDoc(partName + ".SLDPRT");
            foreach (var item in parameters)
            {
                //boolstatus = Part.Extension.SelectByID2("D2@Ýñêèç1@02-01-101-50-1@02-104-50", "DIMENSION", 0, 0, 0, False, 0, Nothing, 0)
                Dimension myDimension = ((Dimension)(solidWorksDocument.Parameter(item.Key + "@" + partName + ".SLDPRT")));
                myDimension.SystemValue = item.Value / 1000;
            }
            solidWorksDocument.EditRebuild3();
            solidWorksDocument.ForceRebuild3(false);
            solidWorksDocument.Extension.SaveAs(newPath + ".SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, null, ref errors, warnings);
            InitiatorSaveExeption(errors, warnings, newPath);
            SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newPath);
            this.parameters.Clear();
        }

                       // if u will be use abstract build method, u must override constructor in all product_builder[s]
        // /// <summary>
        //   /// Build prduct
        //   /// </summary>
        //public abstract Build();  
    }
}
