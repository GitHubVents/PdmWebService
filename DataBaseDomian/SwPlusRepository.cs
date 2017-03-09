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
    public class SwPlusRepository : Singeton<SwPlusRepository> , IUpdater
    {
        protected SwPlusRepository():base()
        {

        }



        /// <summary>
        /// Exemplar of an object oriented database model
        /// </summary>
        private SwPlusDataContext _dataContext;



        public IEnumerable<BendTable> Bends { get { return DataContext.BendTables; } }

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


        public IEnumerable<View_Part> Parts
        {
            get
            {
                
                return DataContext.View_Parts;
            }
        }

        /// <summary>
        /// Add part of panel and returns it's id
        /// </summary>
        /// <returns></returns>
        public int  AddPartOfPanel(int panelType, int elementType, int widht, int height, int partThick, int parMat, int partMatThick, bool mirror, bool stickyTape, string step, string stepInsertion,bool reinfocung,string airHole ) {
            int? partId = 0;
            partId = DataContext.AirVents_AddPartOfPanel_test(panelType, elementType, widht, height, partThick, parMat, partMatThick, mirror, stickyTape, step, stepInsertion, reinfocung, airHole,ref partId);
            return (int)partId;
        }

        public void RefreshRepositoryStatus()
        {
            try
            {
                // only tables
            //    DataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, new object[] { DataContext.View_Parts });
            }
            catch (Exception exception)
            {
                throw new Exception("Failed refresh entities { " + exception.ToString() + " }");
            }
        }
    }
}
