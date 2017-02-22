using Patterns.Observer;
using SolidWorks.Interop.swconst;
using System;

namespace SolidWorksLibrary.Builders.Parts
{
    public abstract class AbstractBuilder
    {
        protected abstract void DeleteComponents(int type);
        public abstract void DeterminateModelName(int type);

        protected virtual void SaveExeptionInitiator(int error, int warning, string path = "")
        {
            if (error != 0 || warning != 0)
            {
                var exeption = new Exception
                    ("Failed save file " + " error code {" + error + "}, error description: " +
                    (swFileSaveError_e)error + ", warning code {" + warning +
                    "}, warning description: " + (swFileSaveWarning_e)warning);
                MessageObserver.Instance.SetMessage(exeption.ToString(), MessageType.Error);
            }
        }
    }
}
