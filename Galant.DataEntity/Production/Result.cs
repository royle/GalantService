using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Production
{
    public class Result:BaseData
    {
        public Result()
        {
            searchCondition = new Search();
        }
        private Search searchCondition;
        public Search SearchCondition
        {
            get { return searchCondition; }
            set { searchCondition = value; OnPropertyChanged("SearchCondition"); }
        }

        private List<Product> resultData;
        public List<Product> ResultData
        {
            set { resultData = value; OnPropertyChanged("ResultData"); }
            get { return resultData; }
        }
    }
}
