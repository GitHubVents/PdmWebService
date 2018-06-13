using ServiceTypes.Constants;
using SolidWorks.Interop.swconst;
using SolidWorksLibrary.Builders.ElementsCase;
using System;
using System.IO;
using System.Windows.Forms;
using SolidWorksLibrary;
using DataBaseDomian;
using Patterns.Observer;
using System.Linq;

namespace PDMWebService.Data.Solid.ElementsCase
{
    public sealed class MountingFrameBuilder : ProductBuilderBehavior
    {

        bool internalCrossbeam;
        bool internalLongitudinalBeam;
        bool internalCrossbeamOffset;
        int tempOffset;

        public MountingFrameBuilder() : base()
        {
            SetProperties(@"Проекты\10 - Base Frame", @"Библиотека проектирования\DriveWorks\10 - Base Frame");
            internalCrossbeam = false;
            internalLongitudinalBeam = false;
            internalCrossbeamOffset = false;
            NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder);
        }

        public void BuildMountageFrame(bool cheched, int width, int lenght, decimal thikness, int FrameType, int offset, int materialID, int RALID, string CoatingType, int CoatingClass)
        {
            string hex = string.Empty;
            if (CoatingType != "0")
            {
                hex = SwPlusRepository.Instance.GetRAL().Where(x => x.RALID.Equals(RALID)).Select(x => x.HEX).First();
            }

            string assemblyName = "10-4";
            string modelMontageFramePath = $@"{base.RootFolder}\{base.SourceFolder}\{assemblyName}.SLDASM";

            SolidWorksDocument = SolidWorksAdapter.OpenDocument(modelMontageFramePath, swDocumentTypes_e.swDocASSEMBLY, "");
            if (SolidWorksDocument == null)
            {
                return;
            }
            MessageObserver.Instance.SetMessage("1)  Start to build mountage frame with " + SolidWorksDocument.GetTitle() + " model");
            

            if (cheched)
            {
                width = width - 40;
            }
            else
            {
                width = width - 20;
            }


            int? ID = 0;
            try
            {
                SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, width, lenght, thikness, offset, 0, materialID, RALID, CoatingType, CoatingClass, ref ID);
            }
            catch (Exception)
            {
                MessageObserver.Instance.SetMessage("Failed to get ID from dbo.AssemblyData");
            }

            string assmblName = ID?.ToString();
            tempOffset = offset;
            if (offset > (lenght - 125) * 10)
            {
                offset = (lenght - 250) * 10;
                MessageBox.Show("Смещение превышает допустимое значение! Программой установлено - " + (offset / 10));
            }
            else
            {
                SetBends?.Invoke((decimal)thikness, out KFactor, out BendRadius);

                GetFrameType((MontageFrameType_e)FrameType, lenght, out offset);//определяем по типу рамы, какие балки удалять

                MessageObserver.Instance.SetMessage("3)  SetBends delegat return KFactor:   " + KFactor.ToString());

                if (lenght > 1000)
                {
                    SolidWorksDocument.Extension.SelectByID2("Middle@10-01-01-4-1@10-4", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                    SolidWorksDocument.Extension.SelectByID2("Middle", "FTRFOLDER", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                }

                #region Рама с продольной балкой
                if (internalLongitudinalBeam)
                {
                    foreach (var s in new[] { "5", "6", "7", "8" })
                    {
                        SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    foreach (var s in new[] { "6", "7", "8", "9", "37", "38", "39", "40" })
                    {
                        SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    foreach (var s in new[] { "17", "18", "19", "20", "21", "22", "23", "24" })
                    {
                        SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    foreach (var s in new[] { "10", "11", "12", "13"})
                    {
                        SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        SolidWorksDocument.EditUnsuppress2();
                    }
                    SolidWorksDocument.Extension.SelectByID2("10-04-4-2@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();


                    SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, lenght - 140, 0, thikness, 0, 3, materialID, RALID, CoatingType, CoatingClass, ref ID);


                    SolidWorksDocument.Extension.SelectByID2("10-04-4-2@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();

                    base.parameters.Add("D1@Эскиз1", lenght - 140);
                    base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                    base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                    base.parameters.Add("Толщина@Листовой металл", (double)thikness);

                    EditPartParameters("10-04-4", base.NewPartPath + @"\10-" + ID?.ToString(), materialID, hex, CoatingType, CoatingClass.ToString());
                    MessageObserver.Instance.SetMessage("8)  Finished build internal longtitudinal beam 10-04-4 with new name " + base.NewPartPath + @"\10-" + ID);//////////////////////////////////////////////////
                }
                #endregion

                #region Рама с поперечной балкой
                if (internalCrossbeam)
                {   
                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
                    SolidWorksDocument.Extension.SelectByID2("10-03-01-4-1@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();

                    if (internalCrossbeamOffset)
                    {
                        SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, width, 0, thikness, offset, 5, materialID, RALID, CoatingType, CoatingClass, ref ID);

                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-01-01-4");

                        SolidWorksDocument.Extension.SaveAs(base.NewPartPath + @"\10-" + ID + ".SLDPRT", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent
                                                                      + (int)swSaveAsOptions_e.swSaveAsOptions_Copy, null, ref errors, warnings);
                        SolidWorksAdapter.CloseDocument(SolidWorksDocument);

                        SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
                        SolidWorksDocument.Extension.SelectByID2("10-01-01-4-2@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        AssemblyDocument.ReplaceComponents(base.NewPartPath + @"\10-" + ID + ".SLDPRT", "", false, true);
                        


                        base.parameters.Add("D1@Эскиз1", lenght);
                        base.parameters.Add("D3@Эскиз25", lenght - offset);
                        base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                        base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                        base.parameters.Add("Толщина@Листовой металл", (double)thikness);
                        EditPartParameters(@"10-" + ID, base.NewPartPath + @"\10-" + ID?.ToString(), materialID, hex, CoatingType, CoatingClass.ToString());
                        MessageObserver.Instance.SetMessage("Finished build internal crossbeam with offset 10-01-01-4 with new name " + base.NewPartPath + @"\10-" + ID);
                    }

                    SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
                    SolidWorksDocument.Extension.SelectByID2("10-03-01-4-1@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-39@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-40@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-19@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-41@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-42@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-20@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-43@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-44@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-21@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-45@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 125-46@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-53@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-54@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-55@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-56@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Шайба DIN 7980-26@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Гайка DIN 934-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.Extension.SelectByID2("Болт DIN 933-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    SolidWorksDocument.EditUnsuppress2();
                }
                #endregion

                #region Просто рама
                //Продольные балки (Длина установки)
                
                if (internalCrossbeamOffset)
                {
                    SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, lenght, 0, thikness, offset, 4, materialID, RALID, CoatingType, CoatingClass, ref ID);
                }
                else
                {
                    SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, lenght, 0, thikness, offset, 1, materialID, RALID, CoatingType, CoatingClass, ref ID);
                }
                MessageObserver.Instance.SetMessage("9)  Просто рама ID 10-01-01-4 : " + ID.ToString());


                base.parameters.Add("D1@Эскиз1", lenght);
                base.parameters.Add("D3@Эскиз25", offset);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", (double)thikness);
                EditPartParameters("10-01-01-4", base.NewPartPath + @"\10-" + ID?.ToString(), materialID, hex, CoatingType, CoatingClass.ToString());

                // Поперечная балка (Ширина установки)
                SwPlusRepository.Instance.AirVents_AddAssemblyFrame(FrameType, width - 100, 0, thikness, 0, 2, materialID, RALID, CoatingType, CoatingClass, ref ID);
                MessageObserver.Instance.SetMessage("10)  Просто рама ID 10-03-01-4 : " + ID.ToString());

                base.parameters.Add("D2@Эскиз1", width - 100);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", (double)thikness);
                EditPartParameters("10-03-01-4", base.NewPartPath + @"\10-" + ID?.ToString(), materialID, hex, CoatingType, CoatingClass.ToString());
                #endregion

                SaveDoc(assmblName);
            }
        }

        private void SaveDoc(string name)
        {
            //Сохранение
            SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4");
            SolidWorksDocument.Visible = true;
            SolidWorksDocument.SaveAs2(base.NewPartPath + @"\10-" + name + ".SLDASM", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
            MessageObserver.Instance.SetMessage("\t " + SolidWorksDocument.GetTitle() + " is saved.");
        }

        private void GetFrameType(MontageFrameType_e type, int length, out int offset)
        {
            offset = tempOffset;

            switch (type)
            {
                case MontageFrameType_e.Zero:
                    offset = length / 2;
                    break;
                case MontageFrameType_e.One:
                    //Внутреняя поперечна балка
                    internalCrossbeam = true;
                    offset = length / 2;
                    break;
                case MontageFrameType_e.Two:
                    //Внутреняя продольная балка
                    offset = length / 2;
                    internalLongitudinalBeam = true;
                    break;
                case MontageFrameType_e.Three:
                    //Внутреняя поперечная балка со смещением
                    internalCrossbeam = true;
                    internalCrossbeamOffset = true;
                    break;
            }
        }
    }
}