//using System.Data;
//using PDMWebService.TaskSystem.AirCad;
//namespace PDMWebService.TaskSystem.Data
//{
//    public abstract class PanelBuilder
//    {
//        public static void Build(Panel panel)
//        {
//            string mat1Code = "";
//            string mat2Code = "";

//            int OuterMaterial = panel.OuterMaterial;
//            var row1 = OuterMaterial.Row;
//            if (row1 != null)
//                mat1Code = row1.Field<string>("CodeMaterial");
//            var viewRowMat2 = (DataRowView)MaterialP2.SelectedItem;
//            var row2 = viewRowMat2.Row;
//            if (row2 != null)
//                mat2Code = row2.Field<string>("CodeMaterial");


//            var materialP1 = new[] { panel.OuterMaterial, panel., (ServiceLibrary.TaskSystem.Constants.Meterials)panel.OuterMaterial, mat1Code };
//            var materialP2 = new[] { MaterialP2.SelectedValue.ToString(), ТолщинаВннутренней.Text, MaterialP2.Text, mat2Code };


//            var thicknessOfPanel = ((ComboBoxItem)TypeOfPanel.SelectedItem).Content.ToString().Remove(2);

//            var sw = new ModelSw();

//            switch (thicknessOfPanel)
//            {
//                case "30":
//                    string path;
//                    sw.Panels30Build(
//                        typeOfPanel:
//                            new[] { TypeOfPanel50.SelectedValue.ToString(), TypeOfPanel50.Text, thicknessOfPanel },
//                        width: WidthPanel.Text,
//                        height: HeightPanel.Text,
//                        materialP1: materialP1,
//                        materialP2: materialP2,
//                        покрытие: null,
//                        path: out path);

//                    break;
//                case "50":
//                case "70":
//                    sw.Panels50Build(
//                        typeOfPanel: new[] { TypeOfPanel50.SelectedValue.ToString(), TypeOfPanel50.Text, thicknessOfPanel },
//                        width: WidthPanel.Text,
//                        height: HeightPanel.Text,
//                        materialP1: materialP1,
//                        meterialP2: materialP2,
//                        покрытие: null
//                        );
//                    break;
//            }
//        }
//    }
//}
