using DataBaseDomian.Orm;
using Patterns;
using Patterns.Observer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataBaseDomian
{
    public class SwPlusRepository : Singeton<SwPlusRepository> , IUpdater
    {
        protected SwPlusRepository() : base()
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

                    MessageObserver.Instance.SetMessage("Opened new connection to SwPlus data base", MessageType.System);
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
        public int AddPartOfPanel(int panelType, int elementType, int widht, int height, int partThick, int parMat, int partMatThick, bool mirror, bool stickyTape, string step, string stepInsertion, bool reinfocung, string airHole)
        {
            int? partId = 0;
            partId = DataContext.AirVents_AddPartOfPanel_test(panelType, elementType, widht, height, partThick, parMat, partMatThick, mirror, stickyTape, step, stepInsertion, reinfocung, airHole, ref partId);
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

        public GetPath ByName (string fileName)
        {
            return  DataContext.GetPaths.Where(each => each.Filename == fileName).FirstOrDefault();            
        }
        
        public GetPath ById(int idPdm)
        {
            return DataContext.GetPaths.Where(each => each.DocumentID == idPdm).First( );
        }

        public Dictionary<int, string> MaterialsForRoof()
        {
            return DataContext.MaterialsProps.Where(x => x.LevelID == 1700 || (x.LevelID == 1800)).Select(x => new { x.LevelID, x.MaterialsName }).ToDictionary(x => x.LevelID, x => x.MaterialsName);
        }
        public Dictionary<int, string> MaterialsForFrame()
        {
            return DataContext.MaterialsProps.Where(x => (x.LevelID == 1800) || (x.LevelID == 2100) || (x.LevelID == 18400)).Select(x => new { x.LevelID, x.MaterialsName }).ToDictionary(x => x.LevelID, x => x.MaterialsName);
        }
        public Dictionary<int, string> MaterialsForFlap()
        {
            return DataContext.MaterialsProps.Where(x => (x.LevelID == 1700) || (x.LevelID == 1800) || (x.LevelID == 2100) || (x.LevelID == 2300) || (x.LevelID == 2400) || (x.LevelID == 2500) || (x.LevelID == 18400) || (x.LevelID == 33883) || (x.LevelID == 34079) || (x.LevelID == 34101)).Select(x => new { x.LevelID, x.MaterialsName }).ToDictionary(x => x.LevelID, x => x.MaterialsName);
        }


        public void AirVents_AddAssemblyRoof(int type, int width, int lenght, int element, int materialID, decimal thickness, int RALID, string CoatingType, int CoatingClass, ref int? ID)
        {
            DataContext.AirVents_AddAssemblyRoof(type, width, lenght, element, thickness, RALID, CoatingType, CoatingClass, materialID, ref ID);
        }
        public void AirVents_AddAssemblyFrame(int? type, int? width, int? lenght, decimal? thikness, int? offset, int element, int? materialID, int RALID, string CoatingType, int CoatingClass, ref int? ID)
        {
            DataContext.AirVents_AddAssemblyFrame(type, width, lenght, thikness, offset, element,  RALID, CoatingType, CoatingClass, materialID, ref ID);
        }
        public void AirVents_AddAssemblySpigot(int? type, int? width, int? lenght, int element, ref int? ID)
        {
            DataContext.AirVents_AddAssemblySpigot(type, width, lenght, element, ref ID);
        }
        public void AirVents_AddAssemblyFlap(int type, int width, int height, bool outer, int materialID, decimal thickness, int element, ref int? ID)
        {
            DataContext.AirVents_AddAssemblyFlap(type, width, height, materialID, thickness, outer, element, ref ID);
        }


        //покрытие
        public string[] CoatingType()
        {
            return new string[] {"Шагрень", "Глянец", "E/P-PARLAK(Муар)"};
        }
        public int[] CoatingClass()
        {
            return new int[] {2, 3};
        }
        public List<RAL> GetRAL()
        {
            return DataContext.RALs.Where(x => x.Applicability.Equals(true)).OrderByDescending(x => x.RALName).ToList();
        }
    }
    
}
