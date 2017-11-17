using ServiceTypes.Constants;
using SolidWorks.Interop.sldworks;
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
        private SldWorks _swApp;
        bool internalCrossbeam; // Погашение внутренней поперечной балки
        bool internalLongitudinalBeam; // Погашение внутренней продольной балки
        double tempOffset;

        

        ModelDoc2 swDocMontageFrame;

        //#region fields and property
        //private string newPartName;
        //private string typeOfMfs;
        //  private string frameOffset;
        //private string width;
        //private string lenght;
        //private string addMatName;
        //private string thikness;

        //public string NewPartName
        //{
        //    get
        //    {
        //        return newPartName;
        //    }

        //    set
        //    {
        //        newPartName = value;
        //    }
        //}
        //public string TypeOfMfs
        //{
        //    get
        //    {
        //        return typeOfMfs;
        //    }

        //    set
        //    {
        //        typeOfMfs = value;
        //    }
        //}
        //public string FrameOffset //смещение
        //{
        //    get
        //    {
        //        return frameOffset;
        //    }

        //    set
        //    {
        //        frameOffset = value;
        //    }
        //}
        //public string Width
        //{
        //    get
        //    {
        //        return width;
        //    }

        //    set
        //    {
        //        width = value;
        //    }
        //}
        //public string Lenght
        //{
        //    get
        //    {
        //        return lenght;
        //    }

        //    set
        //    {
        //        lenght = value;
        //    }
        //}
        //public string AddMatName
        //{
        //    get
        //    {
        //        return addMatName;
        //    }

        //    set
        //    {
        //        addMatName = value;
        //    }
        //}
        //public string Thikness
        //{
        //    get
        //    {
        //        return thikness;
        //    }

        //    set
        //    {
        //        thikness = value;
        //    }
        //}
        //#endregion

        public MountingFrameBuilder() : base()
        {
            SetProperties(@"Проекты\10 - Base Frame", @"Библиотека проектирования\DriveWorks\10 - Base Frame");
            internalCrossbeam = false; 
            internalLongitudinalBeam = false;
        }


       
        private void MountingFrameNameAndPath(double thikness, double lenght, int type, double offset)
        {
            base.PartName = string.Format("10-01-01-{0}{4}-{1}{2}{3}.SLDPRT", thikness, lenght, offset, type, "");//AddMatName вместо кавычек
            base.NewPartPath = Path.Combine(RootFolder, SubjectDestinationFolder, base.PartName);
        }


        public void BuildMountageFrame(double width, double lenght, double thikness, int type, double offset, string material, IList<string> покрытие, bool onlyPath)
        {
            tempOffset = offset;
            if (offset > (lenght - 125) * 10)
            {
                offset = (lenght - 250) * 10;
                MessageBox.Show("Смещение превышает допустимое значение! Программой установлено - " + (offset / 10));
            }
            else
            {
                SetBends = GetSetBends;//получаем значения KFactor, BendRadius из SWPlusBD Materials.BendTable

                MountingFrameNameAndPath(thikness, lenght, type, offset);//определяем имя новой модели и путь
                
                GetFrameType((MontageFrameType_e)type, lenght, out offset);//определяем по типу рамы, какие балки удалять

                SetBends?.Invoke((decimal)thikness, out KFactor, out BendRadius);

                SolidWorksDocument = SolidWorksAdapter.AcativeteDoc("10-4.SLDASM");

                #region Изменение параметров
                //Продольные балки (Длина установки)

                base.parameters.Add("D1@Эскиз1", lenght);
                base.parameters.Add("D3@Эскиз25", offset);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", thikness);
                
                EditPartParameters("10-01-01-4", base.NewPartPath);


                //Продольные балки (Длина установки)

                base.parameters.Add("D1@Эскиз1", lenght - 140);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", thikness);

                EditPartParameters("10-04-4", base.NewPartPath);


                //Поперечная балка (Ширина установки)

                base.parameters.Add("D2@Эскиз1", width - 0.12);
                base.parameters.Add("D1@Листовой металл", (double)BendRadius);
                base.parameters.Add("D2@Листовой металл", (double)KFactor * 1000);
                base.parameters.Add("Толщина@Листовой металл", thikness);
                EditPartParameters("10-03-01-4", base.NewPartPath);

                #endregion

                #region Удаление поперечной балки
                //Тип рамы 2
                if (internalCrossbeam == false)
                {
                    swDocMontageFrame.Extension.SelectByID2("10-03-01-4-1@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-39@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-40@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 6402_gost-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Nut 5915_gost-19@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Bolt 7805_gost-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-41@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-42@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 6402_gost-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Nut 5915_gost-20@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Bolt 7805_gost-23@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-43@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-44@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 6402_gost-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Nut 5915_gost-21@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Bolt 7805_gost-24@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-45@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-46@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Washer 6402_gost-26@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Nut 5915_gost-22@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.Extension.SelectByID2("Hex Bolt 7805_gost-25@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.EditDelete();

                    // Удаление ненужных элементов продольной балки
                    int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;
                    swDocMontageFrame.Extension.SelectByID2("Вырез-Вытянуть8@10-01-01@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.Extension.DeleteSelection2(deleteOption);
                }

                #endregion
                
                #region Удаление продольной балки

                // Погашение внутренней продольной балки
                if (internalLongitudinalBeam == false)
                {
                    foreach (var s in new[] { "5", "6", "7", "8"})
                    {
                        swDocMontageFrame.Extension.SelectByID2("Hex Bolt 7805_gost-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocMontageFrame.EditDelete();
                    }
                    foreach (var s in new[] { "6", "7", "8", "9", "37", "38", "39", "40" })
                    {
                        swDocMontageFrame.Extension.SelectByID2("Washer 6402_gost-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocMontageFrame.EditDelete();
                    }
                    foreach (var s in new[] { "17", "18", "19", "20", "21", "22", "23", "24", "57", "58", "59", "60" })
                    {
                        swDocMontageFrame.Extension.SelectByID2("Washer 11371_gost-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocMontageFrame.EditDelete();
                    }
                    swDocMontageFrame.Extension.SelectByID2("10-04-4-2@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                    swDocMontageFrame.EditDelete();
                    // Удаление ненужных элементов поперечной балки
                    swDocMontageFrame.Extension.SelectByID2("Регулируемая ножка-10@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.EditDelete();
                    swDocMontageFrame.Extension.SelectByID2("Регулируемая ножка-11@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.EditDelete();
                    swDocMontageFrame.Extension.SelectByID2("Регулируемая ножка-002@10-4", "COMPONENT", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.EditDelete();

                    foreach (var s in new[] { "10", "11", "12", "13", "40", "41", "42", "43" })
                    {
                        swDocMontageFrame.Extension.SelectByID2("Hex Nut 5915_gost-" + s + "@10-4", "COMPONENT", 0, 0, 0, true, 0, null, 0);
                        swDocMontageFrame.EditDelete();
                    }

                    const int deleteOption = (int)swDeleteSelectionOptions_e.swDelete_Absorbed + (int)swDeleteSelectionOptions_e.swDelete_Children;
                    swDocMontageFrame.Extension.SelectByID2("Вырез-Вытянуть5@10-03-01-4@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.Extension.DeleteSelection2(deleteOption);
                    swDocMontageFrame.Extension.SelectByID2("Вырез-Вытянуть4@10-03-01-4@10-4", "BODYFEATURE", 0, 0, 0, false, 0, null, 0);
                    swDocMontageFrame.Extension.DeleteSelection2(deleteOption);
                }

                #endregion
            }
        }

        public void OpenDoc()
        {
            var modelMontageFramePath = $@"{base.RootFolder}\{base.SourceFolder}\{"10-4"}.SLDASM";
            _swApp = new SldWorks();
            //_swApp.Visible = true;

            swDocMontageFrame = _swApp.OpenDoc6(modelMontageFramePath, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_LoadModel, "00", 0, 0);
            
            var swAsm = (AssemblyDoc)swDocMontageFrame;
            swAsm.ResolveAllLightWeightComponents(false);
        }

        public void SaveDoc()
        {
            //Сохранение
            _swApp.IActivateDoc2("10-4.SLDASM", false, 0);
            swDocMontageFrame = ((ModelDoc2)(_swApp.ActiveDoc));
            swDocMontageFrame.ForceRebuild3(true);
            swDocMontageFrame.SaveAs2(base.NewPartPath, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
        }

        private void GetFrameType(MontageFrameType_e type, double length, out double offset)
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