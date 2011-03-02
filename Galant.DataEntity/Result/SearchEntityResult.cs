using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Result
{
    public class SearchEntityResult : BaseData
    {
        private Galant.DataEntity.Search.SearchEntityCondition searchCondition = new Search.SearchEntityCondition();
        public Galant.DataEntity.Search.SearchEntityCondition SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }

        private List<Galant.DataEntity.Entity> resultData;
        public List<Galant.DataEntity.Entity> ResultData
        {
            get { return resultData; }
            set { resultData = value; OnPropertyChanged("ResultData"); }
        }
    }
}
