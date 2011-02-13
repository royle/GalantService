using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GLTService.Operation.BaseEntity
{
    public class Entity:BaseOperator
    {
        public Entity(DataOperator data)
            : base(data)
        { }

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


    }
}
