using ServiceTypes.Constants;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SolidWorksLibrary;
using DataBaseDomian;

namespace PDMWebService.Data.Solid.ElementsCase
{
    public sealed class MountingFrameBuilder : ProductBuilderBehavior
    {
        bool internalCrossbeam; // Погашение внутренней поперечной балки
        bool internalLongitudinalBeam; // Погашение внутренней продольной балки
        int tempOffset;

        public MountingFrameBuilder() : base()
        {
            SetProperties(@"Проекты\10 - Base Frame", @"Библиотека проектирования\DriveWorks\10 - Base Frame");
            internalCrossbeam = false;
            internalLongitudinalBeam = false;
            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder);
        }
        
        public void BuildMountageFrame(bool cheched, int width, int lenght, int thikness, int FrameType, int offset, int materialID)
        {
            string assemblyName = "10-4";
            string modelMontageFramePath = $@"{base.RootFolder}\{base.SourceFolder}\{assemblyName}.SLDASM";

            SolidWorksDocument = SolidWorksAdapter.OpenDocument(modelMontageFramePath, swDocumentTypes_e.swDocASSEMBLY, "");
            tbAssemblyNameDataContext bildFrame = new tbAssemblyNameDataContext();
            SolidWorksAdapter.ToAssemblyDocument(SolidWorksDocument);
            int? ID = 0;

            ID = bildFrame.AirVents_AddAssemblyFrame(FrameType, width, lenght, thikness, offset, 0, ref ID);
            string assmblName = ID?.ToString();

            if (cheched)
            {
                width = width - 20;
            }

            tempOffset = offset;
            if (offset > (lenght - 125) * 10)
            {
                offset = (lenght - 250) * 10;
                MessageBox.Show("Смещение превышает допустимое значение! Программой установлено - " + (offset / 10));
            }
            else
            {
                SetBends = GetSetBends;//получаем значения KFactor, BendRadius из SWPlusBD Materials.BendTable

                GetFrameType((MontageFrameType_e)FrameType, lenght, out offset);//определяем по типу рамы, какие балки удалять

                SetBends?.Invoke((decimal)thikness, out KFactor, out BendRadius);

                if (lenght > 1000)
                {
                    SolidWorksDocument.Extension.SelectByID2("Middle@10-01-01-4-1@10-4", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Middle", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                }

            #region Удаление поперечной балки
            //Тип рамы 2
            if (internalCrossbeam == false)
            {

                SolidWorksDocument.Extension.SelectByID2("10-03-01-4-1@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-39@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-40@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-19@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-41@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-42@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-20@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-43@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-44@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-21@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-45@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-46@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-26@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                    // Удаление ненужных элементов продольной балки
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть8@10-01-01-4-1@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditSuppress2();
            }

            #endregion

            #region Удаление продольной балки

            // Погашение внутренней продольной балки
            if (internalLongitudinalBeam == false)
            {
                foreach (var s in new[] { "5", "6", "7", "8" })
                {
                    SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                foreach (var s in new[] { "6", "7", "8", "9", "37", "38", "39", "40" })
                {
                    SolidWorksDocument.Extension.SelectByID2("Washer 6402_gost-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                foreach (var s in new[] { "17", "18", "19", "20", "21", "22", "23", "24", "57", "58", "59", "60" })
                {
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }
                SolidWorksDocument.Extension.SelectByID2("10-04-4-2@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                SolidWorksDocument.EditDelete();

                    //// Удаление ненужных элементов поперечной балки
                SolidWorksDocument.Extension.SelectByID2("Регулируемая ножка-10@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Регулируемая ножка-11@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();
                SolidWorksDocument.Extension.SelectByID2("Регулируемая ножка-002@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditDelete();

                foreach (var s in new[] { "10", "11", "12", "13", "40", "41", "42", "43" })
                {
                    SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditDelete();
                }

                int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть5@10-03-01-4@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
                SolidWorksDocument.Extension.SelectByID2("Вырез-Вытянуть4@10-03-01-4@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.Extension.DeleteSelection2(deleteOption);
            }
            else
            {
                //Продольные балки (Длина установки)
                ID = 0;
                ID = bildFrame.AirVents_AddAssemblyFrame(FrameType, width, 0, thikness, 0, 4, ref ID);

                SolidWorksDocument.Extension.SelectByID2("10-04-4-2@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                SolidWorksDocument.EditUnsuppress2();

                base.parameters.Add("D1@Эскиз1", lenght - 140);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", thikness);
                
                EditPartParameters("10-04-4", base.NewPartPath + @"\10-" + ID, materialID);
            }

            #endregion
            }

            #region Изменение параметров
            if (internalCrossbeam == true)
            {
                ID = 0;
                ID = bildFrame.AirVents_AddAssemblyFrame(FrameType, width, 0, thikness, offset, 2, ref ID);
                bildFrame.SubmitChanges();

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-01-01-4");

                SolidWorksDocument.Extension.SaveAs(base.NewPartPath +@"\10-" + ID + ".SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                                              + (int)swSaveAsOptions_e.swSaveAsOptions_Copy, null, ref errors, warnings);
                SolidWorksAdapter.CloseDocument(SolidWorksDocument);

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
                

                SolidWorksDocument.Extension.SelectByID2("10-01-01-4-2@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                AssemblyDocument.ReplaceComponents(base.NewPartPath + @"\10-" + ID + ".SLDPRT", "", false, true);

                base.parameters.Add("D1@Эскиз1", lenght);
                base.parameters.Add("D3@Эскиз25", lenght - offset);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", thikness);
                EditPartParameters(@"10-" + ID, base.NewPartPath + @"\10 -" + ID, materialID);
            }

            //Продольные балки (Длина установки)
            ID = 0;
            ID = bildFrame.AirVents_AddAssemblyFrame(FrameType, lenght, 0, thikness, offset, 1, ref ID);
            bildFrame.SubmitChanges();

            base.parameters.Add("D1@Эскиз1", lenght);
            base.parameters.Add("D3@Эскиз25", offset);
            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
            base.parameters.Add("Толщина@Листовой металл", thikness);
            EditPartParameters("10-01-01-4", base.NewPartPath + @"\10-" + ID, materialID);

            // Поперечная балка (Ширина установки)
            ID = 0;
            ID = bildFrame.AirVents_AddAssemblyFrame(FrameType, Convert.ToInt32(width - 0.12), 0, thikness, 0, 3, ref ID);
            bildFrame.SubmitChanges();


            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
            SolidWorksDocument.Extension.SelectByID2("10-03-01-4-1@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
            SolidWorksDocument.EditUnsuppress2();

            base.parameters.Add("D2@Эскиз1", width - 0.12);
            base.parameters.Add("D1@Листовой металл", (double)BendRadius);
            base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
            base.parameters.Add("Толщина@Листовой металл", thikness);
            EditPartParameters("10-03-01-4", base.NewPartPath + @"\10-" + ID, materialID);
            #endregion

            SaveDoc(assmblName);
        }

        private void SaveDoc(string name)
        {
            //Сохранение
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
            SolidWorksDocument.Visible = true;
            SolidWorksDocument.SaveAs2(base.NewPartPath + @"\" + name + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
        }

        private void GetFrameType(MontageFrameType_e type, int length, out int offset)
        {
            offset = tempOffset;

            switch (type)
            {
                case MontageFrameType_e.Zero:
                    break;
                case MontageFrameType_e.One:
                    //Погашение внутренней поперечной балки
                    internalCrossbeam = true;
                    offset = length / 2;
                    break;
                case MontageFrameType_e.Two:
                    //Погашение внутренней продольной балки
                    internalLongitudinalBeam = true;
                    break;
                case MontageFrameType_e.Three:
                    //Погашение внутренней поперечной балки
                    internalCrossbeam = true;
                    break;
            }
        }
        private void GetSetBends(decimal thikness, out decimal KFactor, out decimal BendRadius)
        {
            var bendList = SwPlusRepository.Instance.Bends;
            KFactor = base.KFactor;
            BendRadius = base.BendRadius;

            try
            {
                foreach (var item in bendList)
                {
                    if (item.Thickness == thikness)
                    {
                        base.KFactor = item.K_Factor;
                        base.BendRadius = item.BendRadius;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}