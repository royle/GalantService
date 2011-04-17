using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using GLTService.DBConnector;

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

        public override Galant.DataEntity.BaseData MappingRow(DataRow row)
        {
            Galant.DataEntity.EventLog e = base.MappingRow(row) as Galant.DataEntity.EventLog;
            if (e.EntityID.HasValue)
            {
                Entity enti = new Entity(this.Operator);
                e.AddEntity = enti.SearchById(e.EntityID.Value.ToString()) as Galant.DataEntity.Entity;
            }
            return e;
        }

        /// <summary>
        /// 获取订单事件列表
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.EventLog> GetEventByPaperID(string paperID)
        {
            string SqlSearch = @"select * from event_logs where paper_id = @paper_id";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", paperID));
            DataTable dt = SqlHelper.ExecuteDataset(this.Operator.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];
            List<Galant.DataEntity.EventLog> logs = new List<Galant.DataEntity.EventLog>();
            foreach (DataRow dr in dt.Rows)
            {
                logs.Add(MappingRow(dr) as Galant.DataEntity.EventLog);
            }
            return logs;
        }
    }
}