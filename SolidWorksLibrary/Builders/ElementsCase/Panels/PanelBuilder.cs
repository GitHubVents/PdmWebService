﻿using ServiceConstants;
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
    public class PanelBuilder : ProductBuilderBehavior
    {
        #region fields
        public override event SetBendsHandler SetBends;
        private Vector2 sizePanel { get; set; }
        private double innerHeight = 0;
        private double innerWeidht = 0;
        private double lenght = 0;
        private double deepInsulation = 0;
        private double widthHandle;
        private double outThickness;
        private double innerThickness;
        private double halfWidthPanel;
        // Шаг заклепок
        private const double step = 80;
        private double rivetW;
        private double rivetWd;
        private double rivetH;
        bool isOneHandle = false;
        #endregion

        public PanelBuilder() : base()
        {
            base.SetProperties("panel", "source panel");
        }

        private void OpenTemplate(PanelType panelType)
        {
            if (panelType == PanelType.DualBlankPanel || panelType == PanelType.DualRemovablePanel)
            {
               AssemblyName = "02-104-50";
            }
            else
            {
                 AssemblyName = "02-01";
            }

            string pathToPrototype = System.IO.Path.Combine(RootFolder, SourceFolder, AssemblyName + ".SLDASM");
            SolidWorksAdapter.OpenDocument(pathToPrototype, swDocumentTypes_e.swDocASSEMBLY);
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(AssemblyName + ".SLDASM");
            AssemblyDocument = SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
        }

        public void Build(
            PanelType panelType, PanelProfile profile, Vector2 sizePanel,
            Materials OuterMaterial, Materials InnerMaterial,
            double outThickness, double innerThickness
            )
        {
            this.sizePanel = sizePanel;
            this.innerThickness = innerThickness;
            this.outThickness = outThickness;

            #region calculate panel dimention by profile

            switch (profile)
            {
                case PanelProfile.Profile_3_0:
                    innerHeight = sizePanel.X - 7;
                    innerWeidht = sizePanel.Y - 7;
                    lenght = 27;
                    deepInsulation = 20;
                    break;

                case PanelProfile.Profile_5_0:
                    innerHeight = sizePanel.X - 10;
                    innerWeidht = sizePanel.Y - 10;
                    lenght = 48;
                    deepInsulation = 45;
                    break;

                case PanelProfile.Profile_7_0:
                    innerHeight = sizePanel.X - 10;
                    innerWeidht = sizePanel.Y - 10;
                    lenght = 50;
                    deepInsulation = 45;
                    break;
            }
            #endregion

            #region  calculate distance between the handles
            widthHandle = sizePanel.X / 2;
            if (sizePanel.X < 1000)
            {
                widthHandle = sizePanel.X * 0.5;
            }
            if (sizePanel.X >= 1000)
            {
                widthHandle = sizePanel.X * 0.45;
            }
            if (sizePanel.X >= 1300)
            {
                widthHandle = sizePanel.X * 0.4;
            }
            if (sizePanel.X >= 1700)
            {
                widthHandle = sizePanel.X * 0.35;
            }
            isOneHandle = sizePanel.X > 750 ? false : true;
            #endregion

            OpenTemplate(panelType);
            DeleteComponents((int)panelType);
            CalculateRivetStep();
            if (panelType == PanelType.DualBlankPanel || panelType == PanelType.DualRemovablePanel)
            {
                DoublePanel(panelType);
            }
            else
            {
                SinglePanel(panelType);
            }
            Insulation(profile);
            AssemblyName = "02-" + (int)panelType + sizePanel.X + "-" + sizePanel.Y + "-" + OuterMaterial + "-" + InnerMaterial + "-" + (int)profile ;
            ModelDoc2 asm = AssemblyDocument as ModelDoc2;
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, AssemblyName + ".SLDASM");
            asm.ForceRebuild3(false);
            asm.Extension.SaveAs(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion,/* (int)swSaveAsOptions_e.swSaveAsOptions_Silent +*/ (int)swSaveAsOptions_e.swSaveAsOptions_SaveReferenced + (int)swSaveAsOptions_e.swSaveAsOptions_UpdateInactiveViews, null, ref errors, warnings);
            InitiatorSaveExeption(errors, warnings, base.NewPartPath);
    

        }

        private void CalculateRivetStep()
        {
            halfWidthPanel = Convert.ToDouble(sizePanel.X / 2);
            // Шаг заклепок
            rivetW = (Math.Truncate(sizePanel.X / step) + 1) * 1000;
            rivetWd = (Math.Truncate(halfWidthPanel / step) + 1) * 1000;
            rivetH = (Math.Truncate(sizePanel.Y / step) + 1) * 1000;
            if (Math.Abs(rivetW - 1000) < 1)
            {
                rivetW = 2000;
            }

        }

        private void SinglePanel(PanelType panelType)
        {
            base.PartName = "02-01-001"; // имя детали для внешней панели
            if (CheckExistPart != null)
            CheckExistPart(base.PartName , out IsPartExist, out NewPartPath);
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2("02-01-001-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
             
            }
            else
            {
                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                // outer panel
                if (SetBends != null)
                    SetBends((decimal)outThickness, out KFactor, out BendRadius);
                base.parameters.Add("D1@Эскиз1", sizePanel.Y);
                base.parameters.Add("D2@Эскиз1", sizePanel.X);
                base.parameters.Add("D1@Кривая2", rivetH);
                base.parameters.Add("D1@Кривая1", rivetW);
               
                base.parameters.Add("D7@Ребро-кромка1", lenght);
                base.parameters.Add("Толщина@Листовой металл", outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                if (panelType == PanelType.RemovablePanel && !isOneHandle)
                {
                    base.parameters.Add("D4@Эскиз30", widthHandle);
                }
                EditPartParameters("02-01-001", base.NewPartPath);


                base.PartName = "02-01-002"; // имя детали для внутреней панели
                if (CheckExistPart != null)
                    CheckExistPart(base.PartName, out IsPartExist, out NewPartPath);
             
                if (false)
                {                   
                    SolidWorksDocument.Extension.SelectByID2("02-01-002-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
        
                }
                else
                {
                    base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                    if (SetBends != null)
                        SetBends((decimal)innerThickness, out KFactor, out BendRadius);
                    base.parameters.Add("D1@Эскиз1", innerWeidht);
                    base.parameters.Add("D2@Эскиз1", innerHeight);
                    base.parameters.Add("D1@Кривая2", rivetW);
                    base.parameters.Add("D1@Кривая1", rivetH);
                    base.parameters.Add("Толщина@Листовой металл", innerThickness);
                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    EditPartParameters("02-01-002", base.NewPartPath);
                }
            }
        }

        private void DoublePanel(PanelType panelType)
        {
            base.PartName = "02-01-101-50";
            if (CheckExistPart != null)
                CheckExistPart(PartName, out IsPartExist, out NewPartPath);        
            if (IsPartExist)
            {                
                SolidWorksDocument.Extension.SelectByID2("02-01-101-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true); 
            }
            else
            {
                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                if (SetBends != null)
                    SetBends((decimal)outThickness, out KFactor, out BendRadius);
                base.parameters.Add("D1@Эскиз1", sizePanel.Y);
                base.parameters.Add("D2@Эскиз1", sizePanel.X / 2);
                base.parameters.Add("D1@Кривая4", rivetH);
                base.parameters.Add("D1@Кривая3", rivetWd);
                base.parameters.Add("D1@Кривая5", rivetH);
                base.parameters.Add("D7@Ребро-кромка2", lenght);
                base.parameters.Add("D2@Эскиз47", widthHandle / 2);
                base.parameters.Add("Толщина@Листовой металл", outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                EditPartParameters("02-01-101-50", base.NewPartPath); 
            }


            base.PartName = "02-01-102-50";

            if (CheckExistPart != null)
                CheckExistPart(PartName, out IsPartExist, out NewPartPath);
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2("02-01-102-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);         
            }
            else  
            {
                base.NewPartPath = base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                if (SetBends != null)
                    SetBends((decimal)outThickness, out KFactor, out BendRadius);
                base.parameters.Add("D1@Эскиз1", sizePanel.Y - 10);
                base.parameters.Add("D2@Эскиз1", (sizePanel.X - 10) / 2);
                base.parameters.Add("D1@Кривая3", rivetH);
                base.parameters.Add("D1@Кривая2", rivetH);
                base.parameters.Add("D1@Кривая1", rivetWd);
                base.parameters.Add("Толщина@Листовой металл", outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                EditPartParameters("02-01-102-50", base.NewPartPath);
            }             

            base.PartName = "02-01-103-50";
        

            if (CheckExistPart != null)
                CheckExistPart(PartName, out IsPartExist, out NewPartPath);
            if (false)
            { 

                SolidWorksDocument.Extension.SelectByID2("02-01-103-50-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
              
            }
            else
            {
                base.NewPartPath = base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
                if (SetBends != null)
                    SetBends((decimal)outThickness, out KFactor, out BendRadius);
                base.parameters.Add("D1@Эскиз1", sizePanel.Y - 15);
                
                base.parameters.Add("D1@Кривая1", rivetH);
                base.parameters.Add("D2@Эскиз1", lenght - innerThickness - outThickness - 1);
                base.parameters.Add("Толщина@Листовой металл", outThickness);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                EditPartParameters("02-01-103-50", base.NewPartPath);
            }

        }

        /// <summary>
        /// Build Insulation
        /// </summary>
        private void Insulation(PanelProfile profile)
        {
            string MaterialsFolder = "Materials";
            PartName = "02-03-"  + (int)profile + sizePanel.X + "-" + sizePanel.Y;
            if (CheckExistPart != null)
                CheckExistPart(PartName, out IsPartExist, out NewPartPath);
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2("02-01-003@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
            }
            else
            {
                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, MaterialsFolder, PartName);
                base.parameters.Add("D1@Эскиз1", innerWeidht);
                base.parameters.Add("D2@Эскиз1", innerHeight);
                base.parameters.Add("D1@Бобышка-Вытянуть1", deepInsulation);
                // TO DO change lenght by profile, propertis
                EditPartParameters("02-01-003", base.NewPartPath);
            }

            PartName = "02-04-" + (int)profile + sizePanel.X + "-" + sizePanel.Y;
            if (CheckExistPart != null)
                CheckExistPart(PartName, out IsPartExist, out NewPartPath);
            if (IsPartExist)
            {
                SolidWorksDocument.Extension.SelectByID2("02-01-004@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath, "", false, true);
            }
            else
            {
                base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, MaterialsFolder, PartName);
                base.parameters.Add("D6@Эскиз1", innerWeidht);
                base.parameters.Add("D3@Эскиз1", innerHeight);
                EditPartParameters("02-01-004", base.NewPartPath);
            }
        }

        protected override void DeleteComponents(int type)
        {
            PanelType eType = (PanelType)type;
            int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;

            if (eType == PanelType.BlankPanel)
            {
                SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }


            if (eType == PanelType.DualRemovablePanel || eType == PanelType.RemovablePanel)
            {
                if (!isOneHandle)
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-5@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-6@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть7@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,   null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                }
                else
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-5@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-9@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-10@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-13@02-01", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-14@02-01", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@02-01-001-1@02-01", "BODYFEATURE", 0, 0, 0, false, 0,null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                }

                if (eType == PanelType.DualBlankPanel)
                {
                    SolidWorksDocument.Extension.SelectByID2("Ручка MLA 120-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-1@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("SC GOST 17475_gost-2@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-1@02-104-50", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Threaded Rivets-2@02-104-50", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                    SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть11@02-01-101-50-1@02-104-50", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.Extension.DeleteSelection2(deleteOption);

                }
            }
        }

    }
}