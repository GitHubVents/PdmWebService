using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseDomian
{
    /// <summary>
    /// Provides the interface for refresh the data repositories
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// Refresh all entities to actual data
        /// </summary>
        void RefreshRepositoryStatus();
    }
}
