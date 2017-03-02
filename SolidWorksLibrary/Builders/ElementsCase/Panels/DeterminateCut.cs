using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWorksLibrary.Builders.ElementsCase.Panels
{
   
   public static class DeterminateCutPanel
    {
        public const double LongSideMax = 2375;
        public const double ShortSideMax = 1050; 

        public static bool IsCut (Vector2 size)
        {
            if (size.X > ShortSideMax || size.Y > ShortSideMax)
            {
                return true;
            }
            return false;
        }

        // TO DO determinate cut type
    }
}
