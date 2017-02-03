using DataBaseDomian.Orm;
using Patterns;
using Patterns.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseDomian
{
  public  class SwPlusRepository :Singeton<SwPlusRepository>
    {
        protected SwPlusRepository()
        {

        }
        /// <summary>
        /// Exemplar of an object oriented database model
        /// </summary>
        private SwPlusDataContext _dataContext;

        /// <summary>
        /// Property-getter for exemplar of an object oriented database model 
        /// </summary>
        private SwPlusDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new SwPlusDataContext();

                    MessageObserver.Instance.SetMessage("Open connection to SwPlus data base", MessageType.System);
                }
                return _dataContext;
            }
        }


        public IEnumerable< View_Part> Parts
        {
            get
            {
                return DataContext.View_Parts;
            }
        } 
    }
}
