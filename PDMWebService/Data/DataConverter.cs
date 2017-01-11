using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data
{
 public abstract    class DataConverter
    {
        public static int GetInt(string param)
        {
            return Convert.ToInt32(param);
        }

        public static bool IsConvertToInt(IEnumerable<string> newStringParams)
        {
            foreach (var param in newStringParams)
            {
                try
                {
                    var y = Convert.ToInt32(param);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsConvertToDouble(IEnumerable<string> newStringParams)
        {
            foreach (var value in newStringParams)
            {
                try
                {
                    Convert.ToDouble(value);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString() + " Повторите ввод данных!");
                    return false;
                }
            }
            return true;
        }
    }
}
