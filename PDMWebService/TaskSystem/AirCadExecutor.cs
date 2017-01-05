using System.Diagnostics;
using PDMWebService.TaskSystem.AirCad;
namespace PDMWebService.TaskSystem
{
    public static class AirCadExecutor
    {
        private static ModelSw executorModelSw { get; set; }

        public static ModelSw Instance
        {
                get{
                    if (executorModelSw == null)
                    {
                        executorModelSw = new ModelSw();
                    }


                    return executorModelSw;
                }
            }
        }
    }

