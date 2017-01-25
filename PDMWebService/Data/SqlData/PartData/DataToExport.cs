using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.SqlData.PartData
{
    public class DataToExport
    {
        public string Config;
        public string Материал;
        public string Обозначение;
        public double ПлощадьПокрытия;
        public string КодМатериала;

        public string ДлинаГраничнойРамки;
        public string ШиринаГраничнойРамки;
        public string Сгибы;
        public string ТолщинаЛистовогоМеталла;
        public string Наименование;

        public int? MaterialId;

        public string FileName;

        public double? PaintX;
        public double? PaintY;
        public double? PaintZ;

        public int IdPdm;
        public int Version;
    }
}
