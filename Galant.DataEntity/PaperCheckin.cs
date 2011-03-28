using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace Galant.DataEntity
{
    /// <summary>
    /// 归班返回的金额类型
    /// </summary>
    [DataContract]
    public enum CheckinType
    {
        /// <summary>
        /// 现金
        /// </summary>
        [EnumMember]
        Cash = 0x00,
        /// <summary>
        /// 水票
        /// </summary>
        [EnumMember]
        Ticket = 0x01
    }

    /// <summary>
    /// 订单归班返回的钱/水票
    /// </summary>
    [DataContract]
    public class PaperCheckin : BaseData
    {

        #region Model
        private int _checkin_id;
        private string _paper_id;
        private decimal? _checkin_amount;
        private int? _product_id;
        private Product _product;
        private int? _product_count;
        private CheckinType _checkinType;
        /// <summary>
        /// 订单返回价值
        /// </summary>
        public CheckinType CheckinType
        {
            get { return _checkinType; }
            set { _checkinType = value; }
        }

        private int _checkin_type;

        
        [DataMember]
        public int Checkin_Type
        {
            get { return _checkin_type; }
            set { _checkin_type = value; }
        }

        [DataMember]
        public Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        [DataMember]
        public int? ProductCount
        {
            get { return _product_count; }
            set { _product_count = value; OnPropertyChanged("ProductCount"); }
        }

        [DataMember]
        public int CheckinId
        {
            set { _checkin_id = value; OnPropertyChanged("CheckinId"); }
            get { return _checkin_id; }
        }

        [DataMember]
        public int? ProductId
        {
            set { _product_id = value; OnPropertyChanged("ProductId"); }
            get { return _product_id; }
        }

        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public string PaperId
        {
            set { _paper_id = value; }
            get { return _paper_id; }
        }

        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public decimal? CheckinAmount
        {
            set { _checkin_amount = value; }
            get { return _checkin_amount; }
        }
        #endregion Model
    }
}
