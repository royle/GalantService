using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Assign
{
    public class Result:BaseData
    {
        private Galant.DataEntity.Assign.Search searchCondition
            = new Search();
        public Search SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }

        private List<CenterAssignData> resultData;
        public List<CenterAssignData> ResultData
        {
            get { return resultData; }
            set { resultData = value; OnPropertyChanged("ResultData"); }
        }

    }

    public class CenterAssignData:Paper
    {

        public override bool IsMarked
        {
            get
            {
                return base.IsMarked;
            }
            set
            {
                base.IsMarked = value;
                OnPropertyChanged("MarkMode");
            }
        }

        bool hasNewRoute;
        public bool HasNewRoute
        {
            get { return hasNewRoute; }
            set { hasNewRoute = value; OnPropertyChanged("HasNewRoute"); OnPropertyChanged("MarkMode"); OnPropertyChanged("NewRouteTo"); }
        }

        public enum MarkModes { None, Standby, Confirm }
        public MarkModes MarkMode
        {
            get { return IsMarked ? MarkModes.Standby : HasNewRoute ? MarkModes.Confirm : MarkModes.None; }
        }
    }
}
