using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace GLTService.Operation.BaseEntity
{
    public class Paper:BaseOperator
    {
        public Paper(DataOperator data)
            : base(data) { }

        public override string SqlAddNewSql
        {
            get
            {
                return base.SqlInsertDataSql;
//                return @"insert into papers(
//paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status)
//values (
//@paper_id,@Status,@SubState,@Holder,@Bound,@Contact_a,@Contact_b,@Contact_c,@Deliver_a,@Deliver_b,@Deliver_c,@Deliver_a_time,@Deliver_b_time,@Deliver_c_time,@Start_time,@Finish_time,@Salary,@Comment,@Type,@Next_Route,@Mobile_Status)";
            }
        }

        public override string KeyId
        {
            get
            {
                return "paper_id";
            }
        }

        protected override void MappingDataName()
        {
            DicDataMapping.Add("PaperId", "paper_id");
            DicDataMapping.Add("PaperStatus", "Status");
            DicDataMapping.Add("PaperSubStatus", "SubState");
            DicDataMapping.Add("Holder", "Holder");
            DicDataMapping.Add("Bound", "Bound");
            DicDataMapping.Add("ContactA", "Contact_a");
            DicDataMapping.Add("ContactB", "Contact_b");
            DicDataMapping.Add("ContactC", "Contact_c");
            DicDataMapping.Add("DeliverA", "Deliver_a");
            DicDataMapping.Add("DeliverB", "Deliver_b");
            DicDataMapping.Add("DeliverC", "Deliver_c");
            DicDataMapping.Add("DeliverATime", "Deliver_a_time");
            DicDataMapping.Add("DeliverBTime", "Deliver_b_time");
            DicDataMapping.Add("DeliverCTime", "Deliver_c_time");
            DicDataMapping.Add("StartTime", "Start_time");
            DicDataMapping.Add("FinishTime", "Finish_time");
            DicDataMapping.Add("Salary", "Salary");
            DicDataMapping.Add("Comment", "Comment");
            DicDataMapping.Add("PaperType", "Type");
            DicDataMapping.Add("NextRoute", "Next_Route");
            DicDataMapping.Add("MobileStatus", "Mobile_Status");
        }

        protected override void SetTableName()
        {
            TableName = "papers";
        }

        public override bool AddNewData(Galant.DataEntity.BaseData data)
        {
            (data as Galant.DataEntity.Paper).PaperId = this.GenerateId();
            if ((data as Galant.DataEntity.Paper).ContactB.IsNew)
            {
                Entity entity = new Entity(this.Operator);
                entity.SaveEntity(this.Operator,(data as Galant.DataEntity.Paper).ContactB);
            }
            base.AddNewData(data);

            Package opPackage = new Package(this.Operator);
            foreach (Galant.DataEntity.Package pg in (data as Galant.DataEntity.Paper).Packages)
            {
                pg.PackageType = Galant.DataEntity.PackageState.New;
                pg.PaperId = (data as Galant.DataEntity.Paper).PaperId;
                opPackage.AddNewData(pg);
            }

            return true;
        }

        private string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }

        public override MySqlParameter[] BuildParameteres(Galant.DataEntity.BaseData data, Dictionary<string, string> DicData, bool isInsert, bool isUpdate)
        {
            List<String> conditions = new List<string>();
            List<String> parameters = new List<string>();
            List<String> updateParamters = new List<string>();
            List<MySqlParameter> param = new List<MySqlParameter>();
            String keyParam = String.Empty;

            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                string key = string.Empty;
                string propertyName = info.Name;

                if (DicData.TryGetValue(propertyName, out key))
                {
                    object obj = info.GetValue(data, null);
                    if (obj == null)
                        continue;

                    string parameterName = "@" + key;

                    if (obj.GetType().BaseType == typeof(Enum))
                    {
                        param.Add(new MySqlParameter(parameterName, (int)obj));
                    }
                    else if(obj.GetType().BaseType==typeof(Galant.DataEntity.BaseData))
                    {
                        param.Add(new MySqlParameter(parameterName, (obj as Galant.DataEntity.BaseData).QueryId));
                    }
                    else
                    {
                        param.Add(new MySqlParameter(parameterName, obj));
                    }
                    conditions.Add(key);
                    parameters.Add(parameterName);
                    if (key == this.KeyId)
                    {
                        keyParam = key + "=" + parameterName;
                    }
                    else
                    {
                        updateParamters.Add(key + "=" + parameterName);
                       
                    }
                }
            }
            if (isUpdate)
            {
                SqlUpdateDataSql = string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, string.Join(" ,", updateParamters), keyParam);
            }
            if (isInsert)
            {
                SqlInsertDataSql = string.Format("INSERT INTO {0} ({1}) VALUES({2})", TableName, String.Join(" ,", conditions), String.Join(" ,", parameters));
            }
            return param.ToArray();
        }
    }
}