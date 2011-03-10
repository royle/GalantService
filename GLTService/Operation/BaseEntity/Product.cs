using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public class Product:BaseOperator
    {
        public Product() : base() { }
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

        internal List<Galant.DataEntity.Product> SearchProductes(DataOperator data, Galant.DataEntity.Production.Search search) 
        {
            String sqlText = BuildSearchSQL();
            List<String> conditions = new List<string>();
            if (search.Type.HasValue)
                conditions.Add("type = " + (int)search.Type.Value);
            if (!String.IsNullOrEmpty(search.ProductName))
                conditions.Add("product_name = '" + search.ProductName + "'");
            if (!String.IsNullOrEmpty(search.Alias))
                conditions.Add("Alias = '" + search.Alias + "'");
            if (conditions.Count > 0)
                sqlText += "WHERE" + string.Join(" AND ", conditions);
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, sqlText).Tables[0];

            List<Galant.DataEntity.Product> Tproductes = new List<Galant.DataEntity.Product>();
            foreach (DataRow row in dt.Rows)
            {
                Tproductes.Add(MappingRow(row) as Galant.DataEntity.Product);
            }
            return Tproductes;
        }
    }
}