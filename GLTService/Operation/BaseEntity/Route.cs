using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public class Route:BaseOperator
    {
        public Route(DataOperator data)
            : base(data) { }

        public Route()
            : base()
        { }

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
            TableName = "routes";
        }

        public override string KeyId
        {
            get
            {
                return "Route_ID";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("RouteId", "Route_ID");
            DicDataMapping.Add("RountName", "Route_Name");
            DicDataMapping.Add("FromEntityId", "from_entity");
            DicDataMapping.Add("ToEntityId", "to_entity");
            DicDataMapping.Add("IsFinally", "Is_finally");
        }

        /// <summary>
        /// 获取所有路线
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Galant.DataEntity.Route> GetAllRoutes(DataOperator data)
        {
            string SqlSearch = this.BuildSearchSQL();
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Entity entity = new Entity(data);
                List<Galant.DataEntity.Entity> routeEntitys = entity.GetRoutedEntitys(data);
                List<Galant.DataEntity.Route> routes = new List<Galant.DataEntity.Route>();
                foreach (DataRow dr in dt.Rows)
                {
                    routes.Add(MappingRow(dr, data));
                }
                return routes;
            }
            return null;
        }

        public Galant.DataEntity.Route GetRouteByID(string routeID,DataOperator data)
        {
            string SqlSearch = this.BuildSearchSQLByKey(routeID);
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                return MappingRow(dr, data);
            }
            return null;
        }

        String sqlFindRouteIdByEntities = @"SELECT {0} FROM {1} WHERE {2}";//from_entity = {2} AND to_entity = {3} AND Is_finally = {4}
        public String GetRouteIdByEnities(string fromEntityId, string toEntityId)
        {
            return GetRouteIdByEnities(fromEntityId, toEntityId, false);
        }

        public string GetRouteIdByEnities(string fromEntityId, string toEntityId, bool isFinally)
        {
            List<String> condition = new List<string>();
            if (!string.IsNullOrEmpty(fromEntityId)) condition.Add(" from_entity = " + fromEntityId);
            if (!string.IsNullOrEmpty(toEntityId)) condition.Add(" to_entity = " + toEntityId);
            condition.Add(" Is_finally = " + fromEntityId);

            string cdtion = string.Join(" AND ", condition);
            string sqlText = string.Format(sqlFindRouteIdByEntities, KeyId, TableName, cdtion);
            object obj = MySql.Data.MySqlClient.MySqlHelper.ExecuteScalar(this.Operator.myConnection, sqlText);
            return obj == null ? string.Empty : obj.ToString();
        }

        private Galant.DataEntity.Route MappingRow(DataRow dr, DataOperator data)
        {
            Entity entity = new Entity(data);
            List<Galant.DataEntity.Entity> routeEntitys = entity.GetRoutedEntitys(data);
            Galant.DataEntity.Route route = new Galant.DataEntity.Route();
            route.RouteId = Convert.ToInt32(dr["Route_ID"]);
            route.RountName = dr["Route_Name"].ToString();
            if (!string.IsNullOrEmpty(dr["to_entity"].ToString()))
            {
                route.ToEntity = routeEntitys.Where(e => e.EntityId == Convert.ToInt32(dr["to_entity"])).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(dr["from_entity"].ToString()))
            {
                route.FromEntity = routeEntitys.Where(e => e.EntityId == Convert.ToInt32(dr["from_entity"])).FirstOrDefault();
            }
            route.IsFinally = Convert.ToBoolean(dr["Is_finally"]);
            return route;
        }
    }
}