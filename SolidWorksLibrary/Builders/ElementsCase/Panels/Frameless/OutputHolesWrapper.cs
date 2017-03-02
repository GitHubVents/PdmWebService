using System;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless
{
    /// <summary>
    /// Исходящие параметры для определения отверстий верхней и нижней панелей
    /// </summary>
    struct OutputHolesWrapper
    {
        //Зазоры
        static public double G0;
        static public double G1;
        static public double G2;
        // Панель1
        public static double L1;
        public static double D1;
        // Панель2
        static public double L2;
        public static double D2;
        // Панель3
        static public double L3;
        public static double D3;

        //public static string OutValUpDown()
        //{
        //    return String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
        //        Convert.ToInt32(G0) != 0 ? Convert.ToString(G0) : "",
        //        Convert.ToInt32(L1) != 0 ? ";" + Convert.ToString(L1) : "",
        //        Convert.ToInt32(D1) != 0 ? ";" + Convert.ToString(D1) : "",
        //        Convert.ToInt32(G1) != 0 ? ";" + Convert.ToString(G1) : "",
        //        Convert.ToInt32(L2) != 0 ? ";" + Convert.ToString(L2) : "",
        //        Convert.ToInt32(D2) != 0 ? ";" + Convert.ToString(D2) : "",
        //        Convert.ToInt32(G2) != 0 ? ";" + Convert.ToString(G2) : "",
        //        Convert.ToInt32(L3) != 0 ? ";" + Convert.ToString(L3) : "",
        //        Convert.ToInt32(D3) != 0 ? ";" + Convert.ToString(D3) : "");
        //}

      
    }
}