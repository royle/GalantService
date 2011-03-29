using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class PaperCheckin : BaseOperator
    {
        public PaperCheckin(DataOperator data)
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
            TableName = "paper_checkins";
        }

        public override string KeyId
        {
            get
            {
                return "checkin_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("CheckinId", "checkin_id");
            DicDataMapping.Add("PaperId", "paper_id");
            DicDataMapping.Add("CheckinAmount", "checkin_amount");
            DicDataMapping.Add("ProductId", "product_id");
            DicDataMapping.Add("ProductCount", "product_count");
            DicDataMapping.Add("CheckinType", "checkin_type");
        }
    }
}