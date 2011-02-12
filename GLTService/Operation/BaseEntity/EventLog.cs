using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class EventLog:BaseOperator
    {
        public EventLog() : base() { }

        public override string SqlAddNewSql
        {
            get
            {
                return @"INSERT INTO event_logs(
                    Paper_id,Insert_Time,Relation_Entity,At_Station,Event_Type,Event_Data)
                    VALUES (
                    @Paper_id,@Insert_Time,@Relation_Entity,@At_Station,@Event_Type,@Event_Data)";
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