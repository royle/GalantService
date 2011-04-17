using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity.Result
{
    public class ResultPapersByID : BaseData
    {
        private List<Paper> papers = new List<Paper>();
        /// <summary>
        /// 符合条件的订单
        /// </summary>
        [DataMember]
        public List<Paper> Papers
        {
            get { return papers; }
            set { papers = value; }
        }

        private string paperId;
        [DataMember]
        public string PaperId
        {
            get { return paperId; }
            set { paperId = value.ToUpper(); }
        }

    }
}
