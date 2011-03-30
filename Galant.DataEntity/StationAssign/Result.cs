using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.StationAssign
{
    public class Result:BaseData
    {
        private Galant.DataEntity.StationAssign.Search searchCondition
            = new Search();
        public Search SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }

        private List<Paper> resultData;
        public List<Paper> ResultData
        { 
            get{return resultData;}
            set { resultData = value; OnPropertyChanged("ResultData"); }
        }

        private List<Entity> entity;
        public List<Entity> Entity
        {
            get { return entity; }
            set { entity = value; OnPropertyChanged("Entity"); }
        }
    }

    public class StationAssignData : Paper
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
                OnPropertyChanged("IsMarked");
                OnPropertyChanged("MarkMode");
            }
        }

        private PaperSubState? newPaperSubStatus;
        public PaperSubState? NewSubStatus
        {
            get { return newPaperSubStatus; }
            set { newPaperSubStatus = value; }
        }

        public enum MarkModes { None, Standby, Confirm }
        public MarkModes MarkMode
        {
            get { return IsMarked ? MarkModes.Standby : MarkModes.None; }
        }
    }
}
