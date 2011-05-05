using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GLTService.DBConnector;
using System.Data;
using System.Collections.ObjectModel;

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

        public override string SqlUpdateSql
        {
            get
            {
                return @"UPDATE roles SET 
                entity_id = @entity_id,Station_id = @Station_id,Role_Type = @Role_Type 
                WHERE Role_ID = @Role_ID 
                ";

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

        protected override void MappingInsertName()
        {

            DicInsertMapping.Add("EntityId", "entity_id");
            DicInsertMapping.Add("StationId", "Station_id");
            DicInsertMapping.Add("RoleType", "Role_Type");
        }

        protected override void MappingUpdateName()
        {
            DicUpdateMapping.Add("RoleId", "Role_ID");
            DicUpdateMapping.Add("EntityId", "entity_id");
            DicUpdateMapping.Add("StationId", "Station_id");
            DicUpdateMapping.Add("RoleType", "Role_Type");
        }

        public Galant.DataEntity.Role MappingRow(DataOperator data ,DataRow dr)
        {
            if (dr == null)
                return null;
            Entity entity=new Entity(data);
            Galant.DataEntity.Role role = new Galant.DataEntity.Role();
            role.RoleId = Convert.ToInt32(dr["Role_ID"]);
            role.RoleType = (Galant.DataEntity.RoleType)dr["Role_Type"];
            role.Station = entity.GetEntityByID(data, dr["Station_id"].ToString(), false);
            role.EntityId = Convert.ToInt32(dr["entity_id"]);
            return role;
        }

        public ObservableCollection<Galant.DataEntity.Role> GetRolesByEntityID(DataOperator data, string EntityId)
        {
            string SqlSearch = this.BuildSearchSQL() + " WHERE entity_id = '" + EntityId + "'";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ObservableCollection<Galant.DataEntity.Role> roles = new ObservableCollection<Galant.DataEntity.Role>();
                Entity entity=new Entity(data);
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

        public void AddRolesForEntity(DataOperator data,Galant.DataEntity.Entity entity)
        {
            string SqlDelete = "delete from roles where entity_id = " + entity.EntityId.ToString();
            SqlHelper.ExecuteNonQuery(data.mytransaction, CommandType.Text, SqlDelete);
            if (entity.Roles != null)
            {
                foreach (Galant.DataEntity.Role r in entity.Roles)
                {
                    r.EntityId = entity.EntityId;
                    SqlHelper.ExecuteNonQuery(data.mytransaction, CommandType.Text, this.SqlAddNewSql, this.BuildInsertParameteres(r));
                }
                Galant.DataEntity.EventLog e = new Galant.DataEntity.EventLog()
                {
                    AtStation = this.Operator.EntityOperator.CurerentStationID,
                    EventType = "E-roles",
                    InsertTime = System.DateTime.Now,
                    RelationEntity = this.Operator.EntityOperator.CurerentStationID,
                    EntityID = this.Operator.EntityOperator.EntityId,
                    EventData = "修改权限"
                };
                this.AddEvent(e);
            }
        }

        
    }
}