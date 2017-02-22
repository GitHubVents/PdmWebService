using Patterns.Observer;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;

namespace SolidWorksLibrary.Builders.ElementsCase
{
    public abstract class AbstractBuilder
    {
        protected abstract void DeleteComponents(int type); 
         
        protected virtual void InitiatorSaveExeption(int error, int warning, string path = "")
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


        protected virtual void EditPartParameters(string partName, string newName, string[,] newParams  )
        {
            ModelDoc2 swDoc = null;
            swDoc = SolidWorksAdapter.AcativeteDoc(partName + ".SLDPRT");
            var modName = swDoc.GetPathName();
            for (var i = 0; i < newParams.Length / 2; i++)
            {
                Dimension myDimension = ((Dimension)(swDoc.Parameter(newParams[i, 0] + "@" + partName + ".SLDPRT")));
                var param = Convert.ToDouble(newParams[i, 1]);
                var swParametr = param;
                myDimension.SystemValue = swParametr / 1000;
            }
            swDoc.EditRebuild3();
            swDoc.ForceRebuild3(false);
            
            swDoc.SaveAs2(new FileInfo(newName + ".SLDPRT").FullName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, false, true);
     SolidWorksAdapter.SldWoksAppExemplare.CloseDoc(newName);
        }
    }
}
