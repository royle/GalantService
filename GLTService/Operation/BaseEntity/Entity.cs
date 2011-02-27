using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public class Entity:BaseOperator
    {
        public Entity(DataOperator data)
            : base(data)
        { }

        public Entity()
        {}

        public override string SqlAddNewSql
        {
            get
            {
                return @"insert into entities(
                Alias,Password,Full_Name,Home_phone,Cell_phone1,Cell_phone2,Type,
                Address_Family,Address_Child,Comment,Store_log,Deposit,Pay_type,Route_Station,Able_flag)
                values (
                @Alias,@Password,@Full_Name,@Home_phone,@Cell_phone1,@Cell_phone2,@Type,
                @Address_Family,@Address_Child,@Comment,@Store_log,@Deposit,@Pay_type,@Route_Station,@Able_flag)";
            }
        }

        public override string KeyId
        {
            get
            {
                return "Entity_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("EntityId", "Entity_id");
            DicDataMapping.Add("Alias", "Alias");
            DicDataMapping.Add("Password", "Password");
            DicDataMapping.Add("FullName", "Full_Name");
            DicDataMapping.Add("HomePhone", "Home_phone");
            DicDataMapping.Add("CellPhoneOne", "Cell_phone1");
            DicDataMapping.Add("CellPhoneTwo", "Cell_phone2");
            DicDataMapping.Add("EntityType", "Type");
            DicDataMapping.Add("AddressFamily", "Address_Family");
            DicDataMapping.Add("AddressChild", "Address_Child");
            DicDataMapping.Add("Comment", "Comment");
            DicDataMapping.Add("StoreLog", "Store_log");
            DicDataMapping.Add("Deposit", "Deposit");
            DicDataMapping.Add("PayType", "Pay_type");
            DicDataMapping.Add("RountStation", "Route_Station");
            DicDataMapping.Add("AbleFlag", "Able_flag");
        }

        protected override void SetTableName()
        {
            TableName = "entities";
        }

        public Galant.DataEntity.Entity Authorize(DataOperator data, string Alias,string Password, bool IsDetail)
        {
            string SqlSearch = this.BuildSearchSQL() + "WHERE Alias = '" + Alias + "' AND Password = '" + Password + "'";
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Galant.DataEntity.Entity entity = this.MappingRow(dt.Rows[0]) as Galant.DataEntity.Entity;
                return this.GetEntityByID(data, entity.EntityId.ToString(), IsDetail);
            }
            throw new Galant.DataEntity.WCFFaultException(999, "Authorize Fail", "用户名或密码错误");
        }

        public Galant.DataEntity.Entity GetEntityByID(DataOperator data, string EntityId, bool IsDetail)
        {
            string SqlSearch = this.BuildSearchSQLByKey(EntityId);
            DataTable dt = SqlHelper.ExecuteDataset(data.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Galant.DataEntity.Entity entity = this.MappingRow(dt.Rows[0]) as Galant.DataEntity.Entity;
                if (!IsDetail)
                    return entity;
                entity.Roles = null;

            }
            return null;
        }
    }
}
