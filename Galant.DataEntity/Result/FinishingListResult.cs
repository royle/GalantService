using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Result
{
    public class FinishingListResult:BaseData
    {
        private Galant.DataEntity.Search.FinishListCondition searchCondition = new Search.FinishListCondition();
        public Galant.DataEntity.Search.FinishListCondition SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }
    }
}
