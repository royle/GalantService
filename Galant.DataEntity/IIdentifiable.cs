using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    interface IIdentifiable
    {
        String QueryId
        {
            get;
        }

        String CurrentQueryVersion
        {
            get;
        }
    }
}
