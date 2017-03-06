using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels {
    
    /// <summary>
    /// Describes types cut for panel 
    /// </summary>
    public enum CutType_e {
        Whole = 0,
        Horisontal = 1,
        Vertical = 2
    }

    public static class CutPanel {
        public const double LongSideMax = 2375;
        public const double ShortSideMax = 1050;

        /// <summary>
        /// Check is whole or double panel state
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool IsCut(Vector2 size) {
            if (size.X > ShortSideMax || size.Y > ShortSideMax) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns type cut of panel if is dual
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static CutType_e DeterminateCutPanel(Vector2 size) {
            if (IsCut(size)) {
                if (size.Y >= size.X) {
                    return CutType_e.Vertical;
                }
                else {
                    return CutType_e.Vertical;
                }
            }
            else {
                return CutType_e.Whole;
            }
        }
    }
}
