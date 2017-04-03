using ServiceTypes.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels.Frameless.Components {

    public delegate int AddPartOfPanelHandler
(
 int panelType,
 int elementType,
 Vector2 panelsize,
 int partThick,
 int parMat,
 int partMatThick,
 bool mirror,
 bool stickyTape,
 string step,
 string stepInsertion,
 bool reinfocung,
 string airHole
);
     
    public class ExistPartsChecker {
        /// <summary>
        /// Occurs when someone wants to add or check a entity.
        /// </summary>
        public event AddPartOfPanelHandler addPartOfPanelEvent;

        /// <summary>
        /// Working subject
        /// </summary>
        private FramelessPanel framelessPanel { get; set; }

        /// <summary>
        /// ExistPartsChecker construct. 
        /// </summary>
        /// <param name="framelessPanel">Working subject</param>
        public ExistPartsChecker(FramelessPanel framelessPanel) {
            this.framelessPanel = framelessPanel;
        }

        /// <summary>
        /// Check entity by input parameters. If entity esixt returns it is id, otherwise will be create new entity by input parameters and return it is id.
        /// </summary>
        /// <param name="panelType">Panel type (can be one of system types equal to 0)</param>
        /// <param name="elementType">Panel element type</param>
        /// <param name="partThick"></param>
        /// <param name="partMat"></param>
        /// <param name="partMatThick"></param>
        /// <param name="mirror"></param>
        /// <param name="stickyTape"></param>
        /// <param name="step"></param>
        /// <param name="stepInsertion"></param>
        /// <param name="airHole"></param>
        /// <returns></returns>
        public int GetId(PanelType_e panelType ,  ElemetPanelType_e elemetType, int partThick, int partMat, int partMatThick, bool mirror, bool stickyTape, string step, string stepInsertion, string airHole) {

            if (this.addPartOfPanelEvent != null) {
                return this.addPartOfPanelEvent((int)panelType, (int)elemetType, framelessPanel.SizePanel, partMatThick, partMat, partMatThick, mirror, stickyTape, step, stepInsertion, framelessPanel.усиление, airHole);
            }
            throw new Exception("Failed add or find part of panel in the data base");
        }

    }
}
