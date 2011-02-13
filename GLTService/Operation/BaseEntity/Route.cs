using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class Route:BaseOperator
    {
        public Route(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return @"INSERT INTO routes(
Route_Name,from_entity,to_entity,Is_finally)
VALUES (
@Route_Name,@from_entity,@to_entity,@Is_finally)";
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
    }
}