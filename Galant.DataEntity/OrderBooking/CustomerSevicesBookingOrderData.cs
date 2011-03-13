using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity.OrderBooking
{
    /// <summary>
    /// 客服订水类
    /// </summary>
    [DataContract]
    public class CustomerSevicesBookingOrderData:BaseData
    {
        private Entity customer;
        /// <summary>
        /// 订水客户
        /// </summary>
        [DataMember]
        public Entity Customer
        {
            get { return customer; }
            set { customer = value; }
        }

        private Paper paper;
        /// <summary>
        /// 订单
        /// </summary>
        [DataMember]
        public Paper Paper
        {
            get { return paper; }
            set { paper = value; }
        }

        public List<Product> Products
        {
            get;
            set;
        }
    }
}
