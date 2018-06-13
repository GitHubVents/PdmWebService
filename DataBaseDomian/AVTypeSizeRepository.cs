using System.Collections.Generic;
using System.Linq;
using Patterns;
using DataBaseDomian.Orm;

namespace DataBaseDomian
{
    class AVTypeSizeRepository : Singeton<AVTypeSizeRepository>
    {
        public AVTypeSizeRepository() : base()
        {

        }

        private AVTypeSizeRepository dtContext = null;
        
        public AVTypeSizeRepository DTContext
        {
            get
            {
                if (dtContext == null)
                {
                    dtContext = new AVTypeSizeRepository();
                }
                return dtContext;
            }
        }

        public IEnumerable<StandardSize> ormStandartSize
        {
            get
            {
                return DTContext.ormStandartSize.OrderBy(x=>x.Type);
            }
        }
    }
}
