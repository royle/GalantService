using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLTService.Operation.BaseEntity
{
    public class Product:BaseOperator
    {
        public Product(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return @"INSERT INTO products(
Product_Name,Alias,Amount,Type,Discretion,Need_back,Return_Name,Return_Value,Able_flag)
VALUES (
@Product_Name,@Alias,@Amount,@Type,@Discretion,@Need_back,@Return_Name,@Return_Value,@Able_flag)";
            }
        }

        protected override void SetTableName()
        {
            TableName = "products";
        }

        public override string KeyId
        {
            get
            {
                return "Product_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("ProductId", "Product_id");
            DicDataMapping.Add("ProductName", "Product_Name");
            DicDataMapping.Add("Alias", "Alias");
            DicDataMapping.Add("Amount", "Amount");
            DicDataMapping.Add("ProductType", "Type");
            DicDataMapping.Add("Discretion", "Discretion");
            DicDataMapping.Add("NeedBack", "Need_back");
            DicDataMapping.Add("ReturnName", "Return_Name");
            DicDataMapping.Add("ReturnValue", "Return_Value");
            DicDataMapping.Add("AbleFlag", "Able_flag");
        }
    }
}