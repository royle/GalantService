using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

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

        private List<StationAssignData> resultData;
        public List<StationAssignData> ResultData
        { 
            get{return resultData;}
            set { resultData = value; OnPropertyChanged("ResultData"); }
        }

        private List<Entity> entities;
        [IgnoreDataMember]
        public List<Entity> Entities
        {
            get { return entities; }
            set { entities = value; OnPropertyChanged("Entities"); }
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
        public PaperSubState? NewPaperSubStatus
        {
            get { return newPaperSubStatus; }
            set { newPaperSubStatus = value; }
        }

        public enum MarkModes { None, Confirm }
        public MarkModes MarkMode
        {
            get { return IsMarked ? MarkModes.Confirm: MarkModes.None; }
        }
    }
}
