using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.StationAssign
{
    public class Search:BaseData
    {
        public Search() : base("SearchStationAssign") { }

        public Entity Station
        { get; set; }

        public Entity Opeartor { get; set; }
    }
}
