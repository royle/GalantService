using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class Package:BaseOperator
    {
        public Package(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return base.SqlInsertDataSql;
//                return @"INSERT INTO packages(
//Product_id,Count,Amount,Origin_Amount,Paper_id,State)
//VALUES (
//@Product_id,@Count,@Amount,@Origin_Amount,@Paper_id,@State)";
            }
        }

        protected override void SetTableName()
        {
            TableName = "packages";
        }

        public override string KeyId
        {
            get
            {
                return "Package_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("PackageId", "Package_id");
            DicDataMapping.Add("ProductId", "Product_id");
            DicDataMapping.Add("Count", "Count");
            DicDataMapping.Add("Amount", "Amount");
            DicDataMapping.Add("OriginAmount", "Origin_Amount");
            DicDataMapping.Add("PaperId", "Paper_id");
            DicDataMapping.Add("PackageType", "State");
        }
    }
}