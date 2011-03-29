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
        private int? _relation_entity;
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
        public int? RelationEntity
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

        ObservableDictionary<string, string> digestedEventData;
        public ObservableDictionary<string, string> DigestedEventData
        {
            get { EnsureEventDataDigested(); return digestedEventData; }
            set { /* Satisfy Invalidation Checker */ }
        }

        bool eventDataDigested;

        void EnsureEventDataDigested()
        {
            if (eventDataDigested) return;

            digestedEventData = new ObservableDictionary<string, string>();
            if (_event_data != null)
            {
                foreach (string line in _event_data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] tokens = line.Split(new char[] { '\t' }, 2);
                    if (tokens.Length >= 2)
                    {
                        if (!digestedEventData.ContainsKey(tokens[0]))
                        {
                            digestedEventData.Add(tokens[0], tokens[1]);
                        }
                    }
                    else { break; }
                }
            }
            digestedEventData.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(digestedEventData_CollectionChanged);
            eventDataDigested = true;
        }

        void digestedEventData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ObservableDictionary<string, string> data = sender as ObservableDictionary<string, string>;
                string v = "";
                foreach (string k in data.Keys)
                {
                    v += string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}\t{1}\n", k, data[k]);
                }
                v = v.TrimEnd(new char[] { '\n' });
                _event_data = v;
                OnPropertyChanged("EventData");
            }
        }
    }
}
