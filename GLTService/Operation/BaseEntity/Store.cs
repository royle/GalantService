using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class Store : BaseOperator
    {
        public Store(DataOperator data)
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
            TableName = "stores";
        }

        public override string KeyId
        {
            get
            {
                return "store_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("StoreId", "store_id");
            DicDataMapping.Add("EntityID", "entity_id");
            DicDataMapping.Add("ProductID", "product_id");
            DicDataMapping.Add("ProductCount", "product_count");
            DicDataMapping.Add("Bound", "bound");
        }
    }
}