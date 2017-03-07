using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components {
    struct WindowProfils
    {
        //Поперечное сечение окна и смещения от центра
        public static double Width;
        public static double Height;
        public static double ByWidth;
        public static double ByHeight;

        public static bool Flange30;

        private static double _typeOfPanel;

        public static void StringValue(string values)
        {
            if (string.IsNullOrEmpty(values)) return;
            try
            {
                var val = values.Split(';');

                var p1 = val[0].Split('_');

                double.TryParse(p1[0], out Width);
                double.TryParse(p1[1], out Height);
                double.TryParse(p1[2], out ByWidth);
                double.TryParse(p1[3], out ByHeight);
                bool.TryParse(p1[4], out Flange30);
                double.TryParse(p1[5], out _typeOfPanel);

                ////MessageBox.Show(p1[0] + "\n" + p1[1] + "\n" + p1[2] + "\n" + p1[3] + "\n" + p1[4] + " - " + Flange30 + "\n" + TypeOfPanel);
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
