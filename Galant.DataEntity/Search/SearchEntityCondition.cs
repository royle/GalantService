using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Search
{
    public class SearchEntityCondition : Galant.DataEntity.BaseData
    {
        private Galant.DataEntity.EntityType? type;
        private string alias;
        private string phone;
        private string name;
        private bool isStop;

        public SearchEntityCondition() : base("SearchEntity") { }


        public Galant.DataEntity.EntityType? Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsStop
        {
            get { return isStop; }
            set { isStop = value; }
        }
    }
}
