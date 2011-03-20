using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity.Search
{
    public class FinishListCondition : BaseData
    {
        /// <summary>
        /// 发生站点
        /// </summary>
        [DataMember]
        public Entity Station
        {
            get;
            set;
        }
    }
}
