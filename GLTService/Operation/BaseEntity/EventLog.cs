using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class EventLog:BaseOperator
    {
        public EventLog(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return SqlInsertDataSql;
            }
        }
        public override string SqlUpdateSql
        {
            get
            {
                return SqlUpdateDataSql;
            }
        }


        protected override void SetTableName()
        {
            TableName = "event_logs";
        }

        public override string KeyId
        {
            get
            {
                return "Event_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("EventId", "Event_id");
            DicDataMapping.Add("PaperId", "Paper_id");
            DicDataMapping.Add("InsertTime", "Insert_Time");
            DicDataMapping.Add("RelationEntity", "Relation_Entity");
            DicDataMapping.Add("AtStation", "At_Station");
            DicDataMapping.Add("EventType", "Event_Type");
            DicDataMapping.Add("EventData", "Event_Data");
        }
    }
}