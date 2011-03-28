using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

namespace Galant.DataEntity
{
    [DataContract]
    public class Store : BaseData
    {

        #region Model
        private int _store_id;
        private string _product_id;
        private int _relation_entity;
        private int? _entity_id;
        private int _product_count;
        private int _bound;

        /// <summary>
        /// 送实
        /// </summary>
        [DataMember]
        public int Bound
        {
            get { return _bound; }
            set { _bound = value; }
        }

        [DataMember]
        public int ProductCount
        {
            get { return _product_count; }
            set { _product_count = value; }
        }

        [DataMember]
        /// <summary>
        /// auto_increment
        /// </summary>
        public int StoreId
        {
            set { _store_id = value; OnPropertyChanged("StoreId"); }
            get { return _store_id; }
        }
        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public string ProductID
        {
            set { _product_id = value; }
            get { return _product_id; }
        }

        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public int RelationEntity
        {
            set { _relation_entity = value; }
            get { return _relation_entity; }
        }
        [DataMember]
        public int? EntityID
        {
            get { return _entity_id; }
            set { _entity_id = value; }
        }
        
        #endregion Model

    }
}
