using ServiceTypes.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless {
    public enum ProfilType_e { VerticalProfil= 1, HorisontalProfil = 2}
     
    public  partial class FramelessPanelBuilder {

        //Profile for FrontPanel
        public void FrameProfil( double lenght, ProfilType_e type, bool flange30, string partName) {
            var width =  framelessPanel.ThermoStrip ==  ThermoStrip_e.ThermoScotch ? 38.5 : 40.0;
            string config = "00";

            switch (type) {
                case ProfilType_e.HorisontalProfil:
                    config = "00";
                    break;
                case ProfilType_e.VerticalProfil:
                    config = "01";
                    break;
            }
            NewPartPath = System.IO.Path.Combine(base.RootFolder, SubjectDestinationFolder, partName + ".SLDPRT");

            SolidWorksDocument  =   SolidWorksAdapter.AcativeteDoc("02-11-11-40-.SLDPRT");
            SolidWorksDocument.ShowConfiguration2(config);

            string[] configs = SolidWorksDocument.GetConfigurationNames();
            foreach (var s in configs) {
                if (!s.Equals(config)) {
                    SolidWorksDocument.DeleteConfiguration2(s);
                }             
            }
            // rename current configuration.
            SolidWorksDocument.ConfigurationManager.ActiveConfiguration.Name = "00";

            lenght = type ==  ProfilType_e.HorisontalProfil ? lenght - 60 : lenght;
            double rivetStep =  Math.Truncate(lenght / 100) * 1000;
            parameters.Add("D2@Эскиз1", lenght);
            parameters.Add("D1@Кривая1", rivetStep == 1000 ? 2000 : rivetStep);
            parameters.Add("D2@Эскиз23", (!flange30) ? 10.0 : 15.0);
            parameters.Add("D3@Эскиз23", (!flange30) ? 10.0 : 15.0);
            EditPartParameters("02-11-11-40-", NewPartPath, 0);
        }



        //Profiles for reinforcing panel
        #region Усиливающие рамки
        public void ReinforcingProfile (string testName) {
            const double thiknessF = 1;
            //var bendParams = sbSqlBaseData.BendTable(thiknessF);
            //var bendRadius = Convert.ToDouble(bendParams[0]);
            //var kFactor = Convert.ToDouble(bendParams[1]);
            const double heightF = 38.0; // Артурчик сказал не надо. 06.03.2017 16:45
            #region  Усиливающая рамка по ширине
            //newName = усиливающаяРамкаПоШирине.NewName;
            //newName = modelName + "-06-" + width + "-" + "40-" + materialP2[0] + скотч;
            //newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";



            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc(testName);// "02-11-07-40-"); // horisontal up
            
            if (false) { }
            else {
                parameters.Add("D2@Эскиз1", framelessPanel.PanelType == PanelType_e.RemovablePanel ? framelessPanel.SizePanel.X - 49.2 : framelessPanel.SizePanel.X - 47.2);
                //parameters.Add("D1@Эскиз1", heightF);
                parameters.Add("D1@Кривая3", ScrewsByWidthInner);
                parameters.Add("D1@Кривая2", колСаморезВинтШирина);
                //Размеры для отверсти под клепальные гайки под съемные панели
                parameters.Add("G0@Эскиз32", OutputHolesWrapper.G0 - 3.6);
                parameters.Add("G1@Эскиз32", OutputHolesWrapper.G1);
                parameters.Add("G2@Эскиз32", OutputHolesWrapper.G2);
                parameters.Add("G3@Эскиз32", OutputHolesWrapper.G0);
                //Convert.ToString(количествоВинтов)
                parameters.Add("L1@Эскиз32", OutputHolesWrapper.L1);
                parameters.Add("D1@Кривая4", OutputHolesWrapper.D1);
                parameters.Add("L2@Эскиз32", OutputHolesWrapper.L2);
                parameters.Add("D1@Кривая5", OutputHolesWrapper.D2);
                parameters.Add("L3@Эскиз32", OutputHolesWrapper.L3);
                parameters.Add("D1@Кривая6", OutputHolesWrapper.D3);
                parameters.Add("Толщина@Листовой металл", thiknessF);
                parameters.Add("D1@Листовой металл", (double)BendRadius);
                parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                EditPartParameters("02-11-06-40-", "", 0);
                //    $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}",
            }

                #endregion

                #region  Усиливающая рамка по ширине 2

                if (framelessPanel.PanelType == PanelType_e.BlankPanel) {
                    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
                    DocumentExtension.SelectByID2("02-11-06_2-40--4@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
              //  AssemblyDocument.ReplaceComponents(System.IO.Path.Combine(RootFolder, SubjectDestinationFolder, усиливающаяРамкаПоШирине.NewName + "SLDPRT"), String.Empty, true, true);
                                                        //($@"{RootFolder}\{SubjectDestinationFolder}\{усиливающаяРамкаПоШирине.NewName}.SLDPRT", "", true, true);

                    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-06_2-40-.SLDPRT");
                }
                else {
                    //newName = усиливающаяРамкаПоШирине2.NewName;
                    //newName = modelName + "-06-" + width + "-" + "40-" + materialP2[0] + скотч;
                    //newPartPath = $@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";

                    if (false){
                        SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
                        DocumentExtension.SelectByID2("02-11-06_2-40--4@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath, "", true, true);
                        SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-06_2-40-.SLDPRT");
                    }
                    else {
                    parameters.Add("D2@Эскиз1", framelessPanel.PanelType == PanelType_e.RemovablePanel ? framelessPanel.SizePanel.X - 49.2 : framelessPanel.SizePanel.X - 47.2);
                    parameters.Add("D1@Эскиз1", heightF);
                    parameters.Add("D1@Кривая3", ScrewsByWidthInner);
                    parameters.Add("D1@Кривая2", колСаморезВинтШирина);
                    parameters.Add("G0@Эскиз32", OutputHolesWrapper.G0 - 3.6);
                    parameters.Add("G1@Эскиз32", OutputHolesWrapper.G1);
                    parameters.Add("G2@Эскиз32", OutputHolesWrapper.G2);
                    parameters.Add("G3@Эскиз32", OutputHolesWrapper.G0);
                    parameters.Add("L1@Эскиз32", OutputHolesWrapper.L1);
                    parameters.Add("D1@Кривая4", OutputHolesWrapper.D1);
                    parameters.Add("L2@Эскиз32", OutputHolesWrapper.L2);
                    parameters.Add("D1@Кривая5", OutputHolesWrapper.D2);
                    parameters.Add("L3@Эскиз32", OutputHolesWrapper.L3);
                    parameters.Add("D1@Кривая6", OutputHolesWrapper.D3);
                    parameters.Add("Толщина@Листовой металл", thiknessF);
                    parameters.Add("D1@Листовой металл", (double)base.BendRadius);
                    parameters.Add("D2@Листовой металл", (double)base.KFactor * 1000);
                    EditPartParameters("02-11-06_2-40-", NewPartPath, 0);
                }
                }

                #endregion

                #region  Усиливающая рамка по высоте

               // PartName = усиливающаяРамкаПоВысоте.NewName;

            NewPartPath = "";// newName = modelName + "-07-" + lenght + "-" + "40-" + materialP2[0] + скотч;
            NewPartPath = System.IO.Path.Combine(RootFolder, SubjectDestinationFolder, NewPartPath + "SLDPRT");//$@"{Settings.Default.DestinationFolder}\{_destinationFolder}\{newName}.SLDPRT";

            //if (GetExistingFile(Path.GetFileNameWithoutExtension(NewPartPath), 1)) {
            //    SolidWorksDocument = ((ModelDoc2)(SolidWorksAdapter.SldWoksAppExemplare.ActivateDoc2(AssemblyName + ".SLDASM", true, 0)));
            //    DocumentExtension.SelectByID2("02-11-07-40--1@" + AssemblyName, "COMPONENT", 0, 0, 0, false, 0, null, 0);
            //    AssemblyDocument.ReplaceComponents(newPartPath, "", true, true);
            //    SolidWorksAdapter.SldWoksAppExemplare.CloseDoc("02-11-07-40-.SLDPRT");
            if (false) ;
              
                else {
                parameters.Add("D3@Эскиз1", framelessPanel.PanelType == PanelType_e.RemovablePanel ? framelessPanel.SizePanel.Y - 2 : framelessPanel.SizePanel.Y);
                parameters.Add("D1@Эскиз1", heightF);
                parameters.Add("D1@Эскиз23", framelessPanel.PanelType == PanelType_e.BlankPanel ? 44.4 : 125);
                parameters.Add("D1@Кривая2", ScrewsByHeightInner);
                parameters.Add("D1@Кривая1", колЗаклепокВысота);
                parameters.Add("Толщина@Листовой металл", thiknessF);
                parameters.Add("D1@Листовой металл", (double)base.BendRadius);
                parameters.Add("D2@Листовой металл", (double) base.KFactor  * 1000);

                EditPartParameters("02-11-07-40-", NewPartPath, 0);
                #endregion
            }

            #endregion
        }

}
}
