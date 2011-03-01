using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.DBConnector;
using System.Data;

namespace GLTService.Operation.BaseEntity
{
    public class Role:BaseOperator
    {
        public Role(DataOperator data)
            : base(data) { }

        public Role():base()
        { }

        public override string SqlAddNewSql
        {
            get
            {
                return @"INSERT INTO roles(
entity_id,Station_id,Role_Type)
VALUES (
@entity_id,@Station_id,@Role_Type)";
            }
        }



        public override string KeyId
        {
            get
            {
                return "Role_ID";
            }
        }

        protected override void SetTableName()
        {
            TableName = "roles";
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("RoleId", "Role_ID");
            DicDataMapping.Add("EntityId", "entity_id");
            DicDataMapping.Add("StationId", "Station_id");
            DicDataMapping.Add("RoleType", "Role_Type");
        }

        public Galant.DataEntity.Role MappingRow(DataRow dr)
        {
            if (dr == null)
                return null;
            Galant.DataEntity.Role role = new Galant.DataEntity.Role();
            return role;
        }

        public List<Galant.DataEntity.Role> GetRolesByEntityID(DataOperator data, string EntityId)
        {
            string SqlSearch = this.BuildSearchSQL() + " WHERE entity_id = '" + EntityId + "'";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<Galant.DataEntity.Role> roles = new List<Galant.DataEntity.Role>();
                Entity entity=new Entity();
                foreach (DataRow dr in dt.Rows)
                {
                    Galant.DataEntity.Role role = new Galant.DataEntity.Role();
                    role.RoleId = Convert.ToInt32(dr["Role_ID"]);
                    role.RoleType = (Galant.DataEntity.RoleType)dr["Role_Type"];
                    role.Station = entity.GetEntityByID(data, dr["Station_id"].ToString(), false);
                    int entity_id = 0;
                    int.TryParse(EntityId, out entity_id);
                    role.EntityId = entity_id;                        
                    roles.Add(role);
                }
                return roles;
            }
            return null;
        }
    }
}