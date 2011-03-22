using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.Operation.BaseEntity;
using Galant.DataEntity.Assign;
using System.Data;
using MySql;
using MySql.Data.MySqlClient;
using Galant.DataEntity;
using GLTService.DBConnector;

namespace GLTService.Operation.Assign
{
    public class CenterAssign:BaseOperator
    {
        public CenterAssign(DataOperator data) : base(data) { }

        public readonly string sqlSelect = @"SELECT PAPER_ID,SUBSTATE,HOLDER,BOUND,CONTACT_A,CONTACT_B,`TYPE`,NEXT_ROUTE FROM PAPERS
WHERE `TYPE`= 1 AND HOLDER = 0 AND SUBSTATE <= 36 ";
        public readonly string sqlUpdatePaper = @"UPDATE papers SET holder = {0}, SubState={1}, Next_Route = {2} WHERE paper_id = '{3}'";

        public void UpdatePaper(List<Galant.DataEntity.Assign.CenterAssignData> data)
        {
            foreach(Galant.DataEntity.Assign.CenterAssignData assign in data)
            {
                if (!assign.HasNewRoute) continue;
                string routeid = assign.NextRoute.RouteId.ToString();
                if (assign.NextRoute.RouteId == null)
                {
                    GLTService.Operation.BaseEntity.Route opera = new BaseEntity.Route(this.Operator);
                    routeid = opera.GetRouteIdByEnities("0", assign.NextEntity.EntityId.ToString());
                    if (string.IsNullOrEmpty(routeid))
                    {
                        Galant.DataEntity.Route route = new Galant.DataEntity.Route()
                        {
                            FromEntity = new Galant.DataEntity.Entity() { EntityId = 0 },
                            ToEntity = assign.NextEntity,
                            IsFinally = false,
                            RountName = "hq to " + assign.NextEntity.Alias
                        };
                        opera.AddNewData(route);
                        routeid = ReadLastInsertId();
                    }
                }
                String sqlText = string.Format(sqlUpdatePaper, assign.Holder.EntityId, (int)assign.NewSubStatus, routeid, assign.PaperId);
                SqlHelper.ExecuteNonQuery(this.Operator.mytransaction, CommandType.Text, sqlText);
            }
        }

        public List<CenterAssignData> ReadCenterAssgin()
        {
            DataTable dt = MySqlHelper.ExecuteDataset(Operator.myConnection, sqlSelect).Tables[0];
            List<CenterAssignData> data = MappingTable(dt);
            return data;
        }

        public List<Galant.DataEntity.Entity> ReadAllStationEntity()
        {
            GLTService.Operation.BaseEntity.Entity entity = new BaseEntity.Entity(this.Operator);
            return entity.GetEntitysByConditions(this.Operator, new Galant.DataEntity.Search.SearchEntityCondition() { Type = Galant.DataEntity.EntityType.Station });
        }

        List<CenterAssignData> MappingTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<CenterAssignData> data = new List<CenterAssignData>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(MappingRow(row));
            }
            return data;
        }
        
        CenterAssignData MappingRow(DataRow row)
        {
            GLTService.Operation.BaseEntity.Entity entity = new BaseEntity.Entity(this.Operator);
            GLTService.Operation.BaseEntity.Route route = new BaseEntity.Route(this.Operator);

            CenterAssignData data = new CenterAssignData();
            if (!string.IsNullOrWhiteSpace(row["PAPER_ID"].ToString()))
            {
                data.PaperId = row["PAPER_ID"].ToString();
            }
             if (!string.IsNullOrWhiteSpace(row["SUBSTATE"].ToString()))
            {
                data.PaperSubStatus = (PaperSubState)int.Parse(row["SUBSTATE"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["BOUND"].ToString()))
            {
                data.Bound = (PaperBound)int.Parse(row["BOUND"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["TYPE"].ToString()))
            {
                data.PaperType = (PaperType)int.Parse(row["TYPE"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["HOLDER"].ToString()))
            {
                data.Holder = (Galant.DataEntity.Entity)entity.SearchById(row["HOLDER"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["CONTACT_B"].ToString()))
            {
                data.ContactB = (Galant.DataEntity.Entity)entity.SearchById(row["CONTACT_B"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["CONTACT_A"].ToString()))
            {
                data.ContactA = (Galant.DataEntity.Entity)entity.SearchById(row["CONTACT_A"].ToString());
            }
             if (!string.IsNullOrWhiteSpace(row["NEXT_ROUTE"].ToString()))
            {
                data.Routes = route.GetRouteByID(row["NEXT_ROUTE"].ToString(), this.Operator);
            }

            route = null;
            entity = null;

            return data;
        }
    }
}