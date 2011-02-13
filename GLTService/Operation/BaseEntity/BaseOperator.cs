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

        private BaseOperator()
        {
            DicDataMapping = new Dictionary<string, string>();
            MappingDataName();
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

        protected virtual void MappingDataName()
        { }

        protected virtual void SetTableName()
        { }

        public virtual string SqlAddNewSql
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
            List<MySqlParameter> param = new List<MySqlParameter>();
            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                object obj = info.GetValue(data, null);
                if (!obj.Equals(DBNull.Value))
                    param.Add(new MySqlParameter(MarkParameter(info.Name), obj));
                else if (obj.GetType() == typeof(List<>).MakeGenericType(typeof(BaseData)))
                    continue;
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
