using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace Galant.DataEntity.Result
{
    public class FinishingListResult:BaseData
    {
        private Galant.DataEntity.Search.FinishListCondition searchCondition = new Search.FinishListCondition();        
        [DataMember]
        public Galant.DataEntity.Search.FinishListCondition SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }

        private List<Galant.DataEntity.Paper> resultData = new List<Paper>();
        [DataMember]
        public List<Galant.DataEntity.Paper> ResultData
        {
            get { return resultData; }
            set { resultData = value; OnPropertyChanged("ResultData"); OnPropertyChanged("HolderResultData"); }
        }

        private List<HolderPapers> holderResultData;
        [IgnoreDataMember]
        public List<HolderPapers> HolderResultData
        {
            get
            {
                if (holderResultData == null)
                {
                    holderResultData = this.ResultData.GroupBy(c => c.Holder).Select(
                        g => new HolderPapers { Holder = g.Key, Papers = g.ToList() }
                        ).OrderBy(g=>g.Holder).ToList();
                }
                return holderResultData;
            }
        }
        
    }

    public class HolderPapers
    {
        public Galant.DataEntity.Entity Holder { get; set; }
        public List<Galant.DataEntity.Paper> Papers { get; set; }

        public int PackagesCount
        {
            get { return Papers == null ? 0 : Papers.Select(s => s.ChildsCount).Sum() ?? 0; }
        }
    }
}
