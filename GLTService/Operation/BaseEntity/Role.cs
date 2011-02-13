using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class Role:BaseOperator
    {
        public Role(DataOperator data)
            : base(data) { }

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
    }
}