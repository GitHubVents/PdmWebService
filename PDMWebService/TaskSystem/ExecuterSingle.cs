using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.TaskSystem
{

    #region ari vents vad executor  ===== A temporary solution, which should be replaced by porting classes ====
    protected abstract class ExecuterSingle
    {
        private static AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw Executor { get; set; }

        public static AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw Instance()
        {
            if (Executor == null)
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("AirVentsCad.exe");
                foreach (var process in processes)
                {
                    process.Kill();
                }

                Executor = new AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw();
            }
            return Executor;
        }
    }
}
