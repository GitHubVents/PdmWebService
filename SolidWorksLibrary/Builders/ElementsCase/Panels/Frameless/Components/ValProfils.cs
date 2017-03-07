using System;
namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components {
    
        struct ValProfils
        {
            // sizes for intermediate profiles
            static public double Wp1;
            static public double Wp2;
            static public double Wp3;
            static public double Wp4;

            // Types  intermediate profiles
            static public string Tp1;
            static public string Tp2;
            static public string Tp3;
            static public string Tp4;

            //static public string PsTy1;
            // static public string PsTy2;

            public static void StringValue(string values)
            {
                try
                {
                    if (!string.IsNullOrEmpty(values))
                    {
                        var val = values.Split(';');

                        var p1 = val[0]?.Split('_');
                        Tp1 = p1[0];
                        double.TryParse(p1[1], out Wp1);

                        var p2 = val[1].Split('_');
                        Tp2 = p2[0];
                        double.TryParse(p2[1], out Wp2);

                        var p3 = val[2].Split('_');
                        Tp3 = p3[0];
                        double.TryParse(p3[1], out Wp3);

                        var p4 = val[3].Split('_');
                        Tp4 = p4[0];
                        double.TryParse(p4[1], out Wp4);
                    }

                }
                catch (Exception ex)
                {
                    Patterns.Observer.MessageObserver.Instance.SetMessage(ex.ToString() + "\n" + ex.StackTrace, Patterns.Observer.MessageType.Error);
                }
            }
        }
    }
 