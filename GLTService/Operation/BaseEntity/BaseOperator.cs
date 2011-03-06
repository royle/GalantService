using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Galant.DataEntity;
using MySql.Data.MySqlClient;
using GLTService.DBConnector;
using System.Text;
using System.Data;

namespace GLTService.Operation.BaseEntity
{
    public abstract class BaseOperator : IDisposable
    {
        public BaseOperator(DataOperator data):this()
        {
            Operator = data;
        }

        public BaseOperator()
        {
            DicDataMapping = new Dictionary<string, string>();
            DicInsertMapping = new Dictionary<string, string>();
            DicUpdateMapping = new Dictionary<string, string>();
            MappingDataName();
            MappingInsertName();
            MappingUpdateName();
            SetTableName();
        }

        #region IDisposable Members

        private DataOperator Operator
        { get; set; }


        public void Dispose()
        {
            Operator.Clear();
        }

        #endregion

        public string TableName;
        public Dictionary<string, string> DicDataMapping;
        public Dictionary<string, string> DicInsertMapping;
        public Dictionary<string, string> DicUpdateMapping;

        protected virtual void MappingDataName()
        { }

        protected virtual void MappingInsertName()
        { }

        protected virtual void MappingUpdateName()
        { }

        protected virtual void SetTableName()
        { }

        public virtual string SqlAddNewSql
        {
            get { return string.Empty; }
        }

        public virtual string SqlUpdateSql
        {
            get { return string.Empty; }
        }

        public virtual bool AddNewData(BaseData data)
        {
            Operator.CreateConnectionAndTransaction();

            if (Operator.CanDoTransaction)
                SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, SqlAddNewSql, BuildParameteres(data));
            else
                MySqlHelper.ExecuteNonQuery(Operator.myConnection, SqlAddNewSql, BuildParameteres(data));

            return true;
        }

        private const string ParameteraStartWith = @"@";
        private const string SqlInsertStringFormat = @"INSERT INTO {0} ({1}) VALUES({2})";

        public virtual string KeyId
        {
            get { return string.Empty; }
        }

        public virtual MySqlParameter[] BuildParameteres(BaseData data)
        {
            return BuildParameteres(data, DicDataMapping);
        }

        public virtual MySqlParameter[] BuildInsertParameteres(BaseData data)
        {
            return BuildParameteres(data, this.DicInsertMapping);
        }

        public virtual MySqlParameter[] BuildUpdateParameteres(BaseData data)
        {
            return BuildParameteres(data, this.DicUpdateMapping);
        }

        public virtual MySqlParameter[] BuildParameteres(BaseData data,Dictionary<string,string> DicData)
        {
            List<MySqlParameter> param = new List<MySqlParameter>();
            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                string key = string.Empty;
                if (DicData.TryGetValue(info.Name, out key))
                {
                    object obj = info.GetValue(data, null);
                    param.Add(new MySqlParameter(ParameteraStartWith + key, obj));
                }
            }
            return param.ToArray();
        }

        private static string MarkParameter(string propertyName)
        {
            return string.Format("{0}{1}", ParameteraStartWith, propertyName);
        }

        public virtual bool UpdateData(string dataId)
        {
            return true;
        }

        public virtual List<BaseData> SearchByKeyId(string id)
        {
            String SqlSearch = BuildSearchSQLByKey(id);
            DataTable dt = SqlHelper.ExecuteDataset(Operator.myConnection, CommandType.Text, SqlSearch).Tables[0];
            return MappingTable(dt);
        }

        public List<BaseData> MappingTable(DataTable table)
        {
            if (table == null)
                return null;
            List<BaseData> datas = new List<BaseData>();
            foreach (DataRow row in table.Rows)
            {
                datas.Add(MappingRow(row));
            }
            return datas;
        }

        public BaseData MappingRow(DataRow row)
        {
            BaseData data = BuildNewBaseData(this.GetType().Name);
            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                string key = string.Empty;
                if (DicDataMapping.TryGetValue(info.Name, out key))
                {
                    info.SetValue(info, row[key], null);
                }
            }
            return data;
        }

        private BaseData BuildNewBaseData(string propertyName)
        {
            switch (propertyName)
            { 
                case "Entity":
                    return new Galant.DataEntity.Entity();
                default:
                    throw new NotImplementedException("Add PropertyName Before This Exception.");
            }
        }


        public virtual String BuildSearchSQLByKey(string idValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(string.Join(", ", DicDataMapping.Values.ToArray()));
            sql.AppendLine(" FROM ");
            sql.Append(TableName);
            sql.AppendLine(" WHERE ");
            sql.Append(KeyId);
            sql.Append(" = '");
            sql.Append(idValue);
            sql.Append("'");
            return sql.ToString();
        }

        public String BuildSearchSQL()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(string.Join(", ", DicDataMapping.Values.ToArray()));
            sql.AppendLine(" FROM ");
            sql.Append(TableName);
            return sql.ToString();
        }
    }
}
