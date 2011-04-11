using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.Operation.BaseEntity;
using Galant.DataEntity.StationAssign;
using System.Data;
using MySql.Data.MySqlClient;
using Galant.DataEntity;
using GLTService.DBConnector;

namespace GLTService.Operation
{
    public class StationAssign:BaseOperator
    {
        public StationAssign(DataOperator data) : base(data) { }

        public readonly string sqlSelect = @"SELECT PAPER_ID,SUBSTATE,HOLDER,BOUND,CONTACT_A,CONTACT_B,`TYPE`,`COMMENT`, NEXT_ROUTE FROM PAPERS
WHERE `TYPE`= 1 AND HOLDER = '{0}' AND SUBSTATE <= 36 AND IS_COLLECTION = 0";

        public readonly string sqlUpdatePaper = @"UPDATE papers SET holder = {0}, SubState={1}, Next_Route = {2} WHERE paper_id = '{3}'";

        public void UpdatePaper(Galant.DataEntity.StationAssign.Result result)
        {
            Galant.DataEntity.Entity station = result.SearchCondition.Station;
            List<StationAssignData> data = result.ResultData;
            List<Galant.DataEntity.Paper> paperList = new List<Galant.DataEntity.Paper>();
            foreach (StationAssignData assign in data)
            {
                if (!assign.NewPaperSubStatus.HasValue) continue;

                paperList.Add((Galant.DataEntity.Paper)assign);
                string routeid = string.Empty;
                if (assign.NextRoute == null || assign.NextRoute.RouteId == null)
                {
                    GLTService.Operation.BaseEntity.Route opera = new BaseEntity.Route(this.Operator);
                    routeid = opera.GetRouteIdByEnities(station.EntityId.ToString(), null, true);
                    if (string.IsNullOrEmpty(routeid))
                    {
                        Galant.DataEntity.Route route = new Galant.DataEntity.Route();

                        route.FromEntity = station;
                        route.IsFinally = true;
                        route.RountName = station.Alias + "-";
                        opera.AddNewData(route);
                        routeid = ReadLastInsertId();
                    }
                }
                String sqlText = string.Format(sqlUpdatePaper, assign.Holder.EntityId, (int)assign.NewPaperSubStatus, routeid, assign.PaperId);
                SqlHelper.ExecuteNonQuery(this.Operator.mytransaction, CommandType.Text, sqlText);

                this.AddEvent(new Galant.DataEntity.EventLog() { RelationEntity = assign.Holder.EntityId, AtStation = station.EntityId, EventType = "CKO-B", EventData = "CKO-B", InsertTime = DateTime.Now, PaperId = assign.PaperId });
            }
            Galant.DataEntity.Paper collection = new Galant.DataEntity.Paper();
            collection.Holder = station;
            collection.ContactB = station;
            collection.ContactC = collection.ContactA = new Galant.DataEntity.Entity() { EntityId = 0 };
            collection.StartTime = DateTime.Now;
            collection.IsCollection = true;
            collection.PaperType = PaperType.Product;
            collection.PaperStatus = PaperStatus.Transfer;
            collection.PaperSubStatus = PaperSubState.InTransit;
            collection.Bound = PaperBound.ToB;
            collection.ChildPapers = paperList;
            GLTService.Operation.BaseEntity.Paper op = new BaseEntity.Paper(this.Operator);
            op.AddNewCollectionData(collection);

            PaperLinks link = new PaperLinks(this.Operator);
            link.BuildPaperTree(collection);
            this.AddEvent(new Galant.DataEntity.EventLog() { RelationEntity = station.EntityId, AtStation = station.EntityId, EventType = "S-Create", InsertTime = DateTime.Now, PaperId = collection.PaperId });
            this.AddEvent(new Galant.DataEntity.EventLog() { RelationEntity = station.EntityId, AtStation = station.EntityId, EventType = "CKO-B", InsertTime = DateTime.Now, PaperId = collection.PaperId });
        }

        public List<StationAssignData> ReadStationAssign(Galant.DataEntity.StationAssign.Search searchData)
        {
            String sqlText = string.Format(sqlSelect, searchData.Station.EntityId);
            DataTable dt = MySqlHelper.ExecuteDataset(Operator.myConnection, sqlText).Tables[0];
            List<StationAssignData> data = MappingTable(dt);
            return data;
        }

        public List<StationAssignData> MappingTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<StationAssignData> data = new List<StationAssignData>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(MappingRow(row));
            }
            return data;
        }

        StationAssignData MappingRow(DataRow row)
        {
            GLTService.Operation.BaseEntity.Entity entity = new BaseEntity.Entity(this.Operator);
            GLTService.Operation.BaseEntity.Route route = new BaseEntity.Route(this.Operator);

            StationAssignData data = new StationAssignData();
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