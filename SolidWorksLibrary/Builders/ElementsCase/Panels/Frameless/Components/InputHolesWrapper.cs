using System;
using System.Text;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components {
    /// <summary>
    /// Входящие параметры для определения отверстий верхней и нижней панелей
    /// </summary>
    struct InputHolesWrapper
    {
        //Зазоры
        static public double G0;
        static public double G1;
        static public double G2;

        //Панели
        static public double B1;
        static public double B2;
        static public double B3;

        /// <summary>
        /// Преобразование строки расположение панелей в значения необходимые для построения
        /// </summary>
        /// <param name="values">The values.</param>
        public static void StringValue(string values)
        {
            G0 = 0;
            G1 = 0;
            G2 = 0;
            B1 = 0;
            B2 = 0;
            B3 = 0;

            try
            {
                var val = values.Split(';');
                var lenght = val.Length;

                if (lenght < 1) return;
                if (val[0] == "") return;
                G0 = Convert.ToDouble(val[0]);

                if (lenght < 2) return;
                if (val[1] == "") return;
                B1 = Convert.ToDouble(val[1]);

                if (lenght < 3) return;
                if (val[2] == "") return;
                G1 = Convert.ToDouble(val[2]);

                if (lenght < 4) return;
                if (val[3] == "") return;
                B2 = Convert.ToDouble(val[3]);

                if (lenght < 5) return;
                if (val[4] == "") return;
                G2 = Convert.ToDouble(val[4]);

                if (lenght < 6) return;
                if (val[5] == "") return;
                B3 = Convert.ToDouble(val[5]);
            }
            catch (Exception ex)
            {
                Patterns.Observer.MessageObserver.Instance.SetMessage(ex.ToString() + "\n" + ex.StackTrace, Patterns.Observer.MessageType.Error);
            }
            finally
            {
                CalculateHolesAtRemovablePanel();
            }
        }
        public static string InValUpDown()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"_({(G0 != 0 ? Convert.ToString(G0) : "")}");
            stringBuilder.Append($"{(B1 != 0 ? "-" + Convert.ToString(B1) : "")}");
            stringBuilder.Append($"{(G1 != 0 ? "_" + Convert.ToString(G1) : "")}");
            stringBuilder.Append($"{(B2 != 0 ? "-" + Convert.ToString(B2) : "")}");
            stringBuilder.Append($"{(G2 != 0 ? "_" + Convert.ToString(G2) : "")}");
            stringBuilder.Append($"{(B3 != 0 ? "-" + Convert.ToString(B3) : "")})");

            return stringBuilder.ToString();

                //$"_({(G0 != 0 ? Convert.ToString(G0) : "")}" +
                //$"{(B1 != 0 ? "-" + Convert.ToString(B1) : "")}" +
                //$"{( G1 != 0 ? "_" + Convert.ToString(G1) : "")}" +
                //$"{( B2  != 0 ? "-" + Convert.ToString(B2) : "")}" +
                //$"{( G2  != 0 ? "_" + Convert.ToString(G2) : "")}" +
                //$"{( B3  != 0 ? "-" + Convert.ToString(B3) : "")})";
        }

        static void CalculateHolesAtRemovablePanel()
        {
            #region Зазоры

            // Зазоры по умолчанию
            OutputHolesWrapper.G0 = 46;
            OutputHolesWrapper.G1 = 132;
            OutputHolesWrapper.G2 = 132;

            if (Math.Abs(G0) > 0)
            {
                OutputHolesWrapper.G0 = G0;
            }
            if (Math.Abs(G1) > 0)
            {
                OutputHolesWrapper.G1 = G1;
            }
            if (Math.Abs(G2) > 0)
            {
                OutputHolesWrapper.G2 = G2;
            }

            #endregion

            #region Отверстия под панель

            double widht;
            double height;
            double distance;
            double helixCount;

            СъемнаяПанель(B1, 0, out widht, out height, out distance, out helixCount);
            OutputHolesWrapper.L1 = distance;
            OutputHolesWrapper.D1 = helixCount;

            OutputHolesWrapper.L2 = 28;
            OutputHolesWrapper.D2 = 2000;
            OutputHolesWrapper.L3 = 28;
            OutputHolesWrapper.D3 = 2000;

            if (Math.Abs(InputHolesWrapper.B2) > 0)
            {
                СъемнаяПанель(B2, 0, out widht, out height, out distance, out helixCount);
                OutputHolesWrapper.L2 = distance;
                OutputHolesWrapper.D2 = helixCount;
            }

            if (!(Math.Abs( B3) > 0)) return;
            СъемнаяПанель(InputHolesWrapper.B3, 0, out widht, out height, out distance, out helixCount);
            OutputHolesWrapper.L3 = distance;
            OutputHolesWrapper.D3 = helixCount;

            #endregion
        }

        static void СъемнаяПанель(double landingWidht, double landingHeight,
                                  out double widht, out double height,
                                  out double distance, out double screwsCount)
        {
            widht = landingWidht - 2;
            height = landingHeight - 2;
            distance = landingWidht - 132;

            screwsCount = 5000;

            if (landingWidht < 1100)
            {
                screwsCount = 4000;
            }
            if (landingWidht < 700)
            {
                screwsCount = 3000;
            }
            if (landingWidht < 365)
            {
                screwsCount = 2000;
            }
        }
    }
}