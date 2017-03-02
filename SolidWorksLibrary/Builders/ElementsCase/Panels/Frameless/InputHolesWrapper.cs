using System;
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
        static void CalculateHolesAtRemovablePanel()
        {
            #region Зазоры

            // Зазоры по умолчанию
            OutputHolesWrapper.G0 = 46;
            OutputHolesWrapper.G1 = 132;
            OutputHolesWrapper.G2 = 132;

            if (Math.Abs(InValPanels.G0) > 0)
            {
                OutputHolesWrapper.G0 = InValPanels.G0;
            }
            if (Math.Abs(InValPanels.G1) > 0)
            {
                OutputHolesWrapper.G1 = InValPanels.G1;
            }
            if (Math.Abs(InValPanels.G2) > 0)
            {
                OutputHolesWrapper.G2 = InValPanels.G2;
            }

            #endregion

            #region Отверстия под панель

            double ширина;
            double высота;
            double расстояниеL;
            double количествоВинтов;

            СъемнаяПанель(InValPanels.B1, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
            OutputHolesWrapper.L1 = расстояниеL;
            OutputHolesWrapper.D1 = количествоВинтов;

            OutputHolesWrapper.L2 = 28;
            OutputHolesWrapper.D2 = 2000;
            OutputHolesWrapper.L3 = 28;
            OutputHolesWrapper.D3 = 2000;

            if (Math.Abs(InValPanels.B2) > 0)
            {
                СъемнаяПанель(InValPanels.B2, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
                OutputHolesWrapper.L2 = расстояниеL;
                OutputHolesWrapper.D2 = количествоВинтов;
            }

            if (!(Math.Abs(InValPanels.B3) > 0)) return;
            СъемнаяПанель(InValPanels.B3, 0, out ширина, out высота, out расстояниеL, out количествоВинтов);
            OutputHolesWrapper.L3 = расстояниеL;
            OutputHolesWrapper.D3 = количествоВинтов;

            #endregion
        }
    }