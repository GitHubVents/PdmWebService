using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using System.Collections.Generic;
using VentsMaterials;
using DataBaseDomian;

namespace SolidWorksLibrary.Builders.ElementsCase
{

    /// <summary>
    /// It abstract class describes the basic behavior of the builder
    /// </summary>
    public abstract partial class ProductBuilderBehavior : IFeedbackBuilder
    {
        ToSQL toSQL = new ToSQL();
        SetMaterials setMat;
        protected Dictionary<string, double> parameters { get; set; }

        protected string NewPartPath;
        #region properties

        /// <summary>
        /// Path list to built files
        /// </summary>
        public List<string> ComponentsPathList { get; protected set; }

        /// <summary>
        /// Sorce folder where there is the prototypes for the build
        /// </summary>
        protected string SourceFolder { get; set; }

        /// <summary>
        /// The folder containing files after they are built and save. 
        /// </summary>
        protected string SubjectDestinationFolder { get; set; }

        public string RootFolder { get; set; }

        protected AssemblyDoc AssemblyDocument {
            get {
                //SolidWorksAdapter.AcativeteDoc(AssemblyName);
                return SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
            }
        }

        protected ModelDoc2 SolidWorksDocument { get; set; }

        /// <summary>
        /// Working SolidWork document { DRW }
        /// </summary>
        protected DrawingDoc SolidWorksDRW { get; set; }

        protected string AssemblyName { get; set; }

        protected string PartName { get; set; }

        protected string PartPrototypeName { get; set; }

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
        public virtual SetBendsHandler SetBends { get; set; }




        protected bool IsPartExist;
        #endregion

        //Папка для сохранения моделей при тестировании
        protected string DebugRootFolder { get { return @"D:\Vents-PDM"; } }

        public ProductBuilderBehavior()
        {
            this.ComponentsPathList = new List<string>();
            parameters = new Dictionary<string, double>();
            SetBends += GetSetBends;
        }

        #region SetProperties

        /// <summary>
        /// Assigns necessary paths for build
        /// </summary>
        /// <param name="subjectDestinationFolder">The folder in which will be save the files after built</param>
        /// <param name="sourceFolder">Sorce folder where there is the prototypes for the build</param>
        protected void SetProperties(string subjectDestinationFolder, string sourceFolder)
        {
            this.RootFolder = @"D:\Test"; // test default value
            this.SubjectDestinationFolder = subjectDestinationFolder;
            this.SourceFolder = sourceFolder;
            CheckDestinationFolder(this.RootFolder, this.SubjectDestinationFolder);
        }
        protected void CheckDestinationFolder(string rootPath, string subjectDestinationFolder)
        {
            string pathDestination = Path.Combine(rootPath, subjectDestinationFolder);

            bool exist = Directory.Exists(pathDestination);

            if (!exist)
            {
                System.Windows.Forms.MessageBox.Show("Going to create " + pathDestination + " derictory");
                Directory.CreateDirectory(pathDestination);
            }
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
                    ("Failed save file with path" + path + "\n error code {" + error + "}, error description: " +
                    (swFileSaveError_e)error + ", warning code {" + warning +
                    "}, warning description: " + (swFileSaveWarning_e)warning);
                MessageObserver.Instance.SetMessage(exeption.ToString() + "\n" + exeption.Message, MessageType.Error);
            }
        }
        protected int errors = 0;
        protected int warnings = 0;

        protected virtual void EditPartParameters(string partName, string newPath, int materialID)
        {
            
            MessageObserver.Instance.SetMessage("4)  SolidWorksDocument when start EditParams: " + SolidWorksDocument?.GetTitle());

            foreach (var eachParameter in parameters)
            {
                try
                {
                    Dimension myDimension = (SolidWorksDocument?.Parameter(eachParameter.Key + "@" + partName + ".SLDPRT")) as Dimension;
                    myDimension.SystemValue = eachParameter.Value / 1000;
                }
                catch (Exception)
                {
                    SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser(eachParameter.Key + "@" + partName);
                    MessageObserver.Instance.SetMessage("Failed to set value to: " + eachParameter.Key + "@" + partName);
                }
            }
            this.parameters.Clear();

            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(partName);
            if (SolidWorksDocument != null)
            {
                //применение материала
                if (materialID != 0)
                {
                    toSQL.AddCustomProperty("", materialID, SolidWorksAdapter.SldWoksAppExemplare);
                }

                bool res = SolidWorksDocument.Extension.SaveAs(newPath + ".SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                          + (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews +
                                          (int)swSaveAsOptions_e.swSaveAsOptions_OverrideSaveEmodel, null, ref errors, warnings);

                MessageObserver.Instance.SetMessage("Trying to save file with path " + newPath + "\n error code {" + errors + "}, error description: " +
                                                   (swFileSaveError_e)errors + ", warning code {" + warnings + "}, warning description: " + (swFileSaveWarning_e)warnings);

                if (!res)
                {
                    SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser("Документ не был сохранен по пути " + newPath + ".SLDPRT");
                }

                //InitiatorSaveExeption(errors, warnings, newPath);
                SolidWorksAdapter.CloseDocument(SolidWorksDocument);
                MessageObserver.Instance.SetMessage("7)   After editing detail the SolidWorksDocument is: " + SolidWorksDocument?.GetTitle());
            }
            else
            {
                MessageObserver.Instance.SetMessage("SolidWorksDocument is NULL");
                SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser("Не удалось преобразовать деталь " + partName);
            }

            //SolidWorksAdapter.SldWoksAppExemplare.DocumentVisible(false, 2);
            //SolidWorksAdapter.SldWoksAppExemplare.DocumentVisible(false, 1);
        }
        protected virtual void EditPartParameters(string partName, string newPath, int materialID, string hex, string CoatingType, string CoatingClass)
        {
            MessageObserver.Instance.SetMessage("4)  SolidWorksDocument when start EditParams: " + SolidWorksDocument?.GetTitle());

            foreach (var eachParameter in parameters)
            {
                try
                {
                    Dimension myDimension = (SolidWorksDocument?.Parameter(eachParameter.Key + "@" + partName + ".SLDPRT")) as Dimension;
                    myDimension.SystemValue = eachParameter.Value / 1000;
                }
                catch (Exception)
                {
                    SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser(eachParameter.Key + "@" + partName);
                    MessageObserver.Instance.SetMessage("Failed to set value to: " + eachParameter.Key + "@" + partName);
                }
            }
            this.parameters.Clear();

            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(partName);
            if (SolidWorksDocument != null)
            { 
                //SolidWorksDocument.ForceRebuild3(true);

                //применение материала
                if (materialID != 0)
                {
                    toSQL.AddCustomProperty("", materialID, SolidWorksAdapter.SldWoksAppExemplare);
                }
                //применение цвета
                if (hex!=string.Empty)
                {
                    try
                    {
                        setMat = new SetMaterials(SolidWorksAdapter.SldWoksAppExemplare);
                        setMat.SetColor(SolidWorksDocument.GetActiveConfiguration().Name, hex, CoatingType, CoatingClass.ToString(), SolidWorksAdapter.SldWoksAppExemplare);
                    }
                    catch (Exception ex)
                    {
                        MessageObserver.Instance.SetMessage("Failed to set color: \n\t" + ex.Message);
                        throw ex;
                    }
                }

                bool res = SolidWorksDocument.Extension.SaveAs(newPath + ".SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                          + (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews + 
                                          (int)swSaveAsOptions_e.swSaveAsOptions_OverrideSaveEmodel, null, ref errors, warnings);
                
                MessageObserver.Instance.SetMessage("Trying to save file with path " + newPath + "\n error code {" + errors + "}, error description: " +
                                                   (swFileSaveError_e)errors + ", warning code {" + warnings + "}, warning description: " + (swFileSaveWarning_e)warnings);

                if (!res)
                {
                    SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser("Документ не был сохранен по пути " + newPath + ".SLDPRT");
                }

                //InitiatorSaveExeption(errors, warnings, newPath);
                SolidWorksAdapter.CloseDocument(SolidWorksDocument);
                MessageObserver.Instance.SetMessage("7)   After editing detail the SolidWorksDocument is: " + SolidWorksDocument?.GetTitle());
            }
            else
            {
                MessageObserver.Instance.SetMessage("SolidWorksDocument is NULL");
                SolidWorksAdapter.SldWoksAppExemplare.SendMsgToUser("Не удалось преобразовать деталь " + partName);
            }

            //SolidWorksAdapter.SldWoksAppExemplare.DocumentVisible(false, 2);
            //SolidWorksAdapter.SldWoksAppExemplare.DocumentVisible(false, 1);
        }


        private void GetSetBends(decimal thikness, out decimal KFactor, out decimal BendRadius)
        {
            KFactor = this.KFactor;
            BendRadius = this.BendRadius;

            try
            {
                foreach (var item in SwPlusRepository.Instance.Bends)
                {
                    if (item.Thickness == thikness)
                    {
                        this.KFactor = item.K_Factor;
                        this.BendRadius = item.BendRadius;
                    }
                }
            }
            catch (Exception)
            {
                MessageObserver.Instance.SetMessage("Faild to GetSetBends");//////////////////////////////////////////////////
            }
        }

    }
}