using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GLTService.DBConnector;
using MySql.Data.MySqlClient;

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

        public List<Galant.DataEntity.Package> GetPackagesByPaper(DataOperator dataOper, string paperID)
        {
            string SqlSearch = @"select * from packages where paper_id = @paper_id";
            List<MySqlParameter> paras = new List<MySqlParameter>();
            paras.Add(new MySqlParameter("@paper_id", paperID));
            DataTable dt = SqlHelper.ExecuteDataset(dataOper.myConnection, CommandType.Text, SqlSearch, paras.ToArray()).Tables[0];

            List<Galant.DataEntity.Package> packages = new List<Galant.DataEntity.Package>();
            if (dt.Rows.Count <= 0)
                return packages;
            Galant.DataEntity.Production.Search pSearch = new Galant.DataEntity.Production.Search();
            Product product = new Product(dataOper);
            List<Galant.DataEntity.Product> producs = product.SearchProductes(dataOper, pSearch);
            foreach (DataRow dr in dt.Rows)
            {
                Galant.DataEntity.Package pg = MappingRow(dr) as Galant.DataEntity.Package;
                pg.Product = producs.Where(p => p.ProductId == pg.ProductId).FirstOrDefault();
                packages.Add(pg);
            }
            return packages;
        }
    }
}