using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMWebService.Data.Solid.PartBuilders
{
    interface IPartBuilder
    {
        string Build(string type, string width, string height);
    }
}
