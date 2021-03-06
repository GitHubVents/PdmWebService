﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase
{
    /// <summary>
    /// describes the delegate signature checkeExist part  
    /// </summary>
    /// <param name="partName"></param>
    /// <param name="isExesitPatrt"></param>
    /// <param name="pathToPart"></param>
    public delegate void CheckExistPartHandler(string partName, string rootFolder, out string pathToPart);
    /// <summary>
    ///  describes the delegate signature finished build 
    /// </summary>
    /// <param name="ComponentsPathList"></param>
    public delegate void FinishedBuildHandler(List<string> ComponentsPathList);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thickness"></param>
    /// <param name="kFactor"></param>
    /// <param name="bendRadius"></param>
    public delegate void SetBendsHandler(decimal thickness, out decimal kFactor, out decimal bendRadius);

    public interface IFeedbackBuilder
    {

        // ========================= README about CheckExistPart delegate =====================================================================  
        // When the event is fired, a check runs to find whether the part or assembly exists. It returnes the path to file if file exists 
        // and boolean flag using out operators.it allows not to be bound to file format such as PDM, IPS,SQL, explorer etc

        /// <summary>
        /// Provides notification and feedback to check for part
        /// </summary>
           CheckExistPartHandler CheckExistPart { get; set; }
        // ==================================================================================================================================

        /// <summary>
        /// Informing subscribers the completion of building 
        /// </summary>
          FinishedBuildHandler FinishedBuild{ get; set; }

        SetBendsHandler SetBends { get; set; }


    }
}
