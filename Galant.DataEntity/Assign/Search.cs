using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Assign
{
    public class Search:BaseData
    {
        public Search() : base("SearchCenterAssign") { }

        private bool needRoute;
        public bool NeedRoute
        {
            get { return needRoute; }
            set { needRoute = value; OnPropertyChanged("NeedRoute"); }
        }

        private bool affirmRoute;
        public bool AffirmRoute
        {
            get { return affirmRoute; }
            set { affirmRoute = value; OnPropertyChanged("AffirmRoute"); }
        }

        private bool unAccept;
        public bool UnAccept
        {
            get { return unAccept; }
            set { unAccept = value; OnPropertyChanged("UnAccept"); }
        }

        public Entity Station
        { get; set; }
    }
}
