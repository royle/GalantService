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

namespace GLTService.Operation.Assign
{
    public class CenterAssign:BaseOperator
    {
        public CenterAssign(DataOperator data) : base(data) { }

        public readonly string sqlSelect = @"SELECT PAPER_ID,SUBSTATE,HOLDER,BOUND,CONTACT_A,CONTACT_B,`TYPE`,NEXT_ROUTE FROM PAPERS
WHERE `TYPE`= 1 AND HOLDER = 0 AND SUBSTATE <= 36 ";


        public List<CenterAssignData> ReadCenterAssgin()
        {
            DataTable dt = MySqlHelper.ExecuteDataset(Operator.myConnection, sqlSelect).Tables[0];
            List<CenterAssignData> data = MappingTable(dt);
            return data;
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