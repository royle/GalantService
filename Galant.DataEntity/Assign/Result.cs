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

        private List<Galant.DataEntity.Paper> resultData;
        public List<Galant.DataEntity.Paper> ResultData
        {
            get { return resultData; }
            set { resultData = value; OnPropertyChanged("ResultData"); }
        }

    }
}
