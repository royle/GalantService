using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public class EventLog: BaseData
    {
        #region Model
        private int _event_id;
        private string _paper_id;
        private DateTime _insert_time;
        private int _relation_entity;
        private int? _entity_id;
        private int? _at_station;
        private string _event_type;
        private string _event_data;

        [DataMember]
        /// <summary>
        /// auto_increment
        /// </summary>
        public int EventId
        {
            set { _event_id = value; OnPropertyChanged("EventId"); }
            get { return _event_id; }
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
        public DateTime InsertTime
        {
            set { _insert_time = value; }
            get { return _insert_time; }
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
        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public int? AtStation
        {
            set { _at_station = value; }
            get { return _at_station; }
        }
        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public string EventType
        {
            set { _event_type = value; }
            get { return _event_type; }
        }
        [DataMember]
        /// <summary>
        /// 
        /// </summary>
        public string EventData
        {
            set { _event_data = value; }
            get { return _event_data; }
        }
        #endregion Model
    }
}
