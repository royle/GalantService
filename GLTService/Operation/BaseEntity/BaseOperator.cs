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

        protected DataOperator Operator
        { get; set; }


        public void Dispose()
        {
            Operator.Clear();
        }

        #endregion

        public string TableName;
        String sqlLastInsertId = @"SELECT LAST_INSERT_ID()";
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

        internal string SqlInsertDataSql;
        public virtual string SqlAddNewSql
        {
            get { return string.Empty; }
        }
        internal string SqlUpdateDataSql;
        public virtual string SqlUpdateSql
        {
            get { return string.Empty; }
        }

        public virtual bool AddNewData(BaseData data)
        {
            Operator.CreateConnectionAndTransaction();
            MySqlParameter[] paramArray = BuildParameteres(data, DicDataMapping, true, false);
            if (Operator.CanDoTransaction)
                SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, SqlAddNewSql, paramArray);
            else
                MySqlHelper.ExecuteNonQuery(Operator.myConnection, SqlAddNewSql, paramArray);

            return true;
        }

        public virtual bool UpdateData(BaseData data)
        {
            Operator.CreateConnectionAndTransaction();
            MySqlParameter[] paramArray = BuildParameteres(data, DicDataMapping, false, true);
            if (Operator.CanDoTransaction)
                SqlHelper.ExecuteNonQuery(Operator.mytransaction, System.Data.CommandType.Text, SqlUpdateSql , paramArray);
            else
                MySqlHelper.ExecuteNonQuery(Operator.myConnection, SqlUpdateSql, paramArray);

            return true;
        }

        private const string ParameteraStartWith = @"@";
        private const string SqlInsertStringFormat = @"INSERT INTO {0} ({1}) VALUES({2})";
        private const string SqlUpdateStringFormat = @"UPDATE {0} SET {1} WHERE {2}";

        public virtual string KeyId
        {
            get { return string.Empty; }
        }

        public virtual MySqlParameter[] BuildParameteres(BaseData data)
        {
            return BuildParameteres(data, DicDataMapping, false, false);
        }

        public virtual MySqlParameter[] BuildInsertParameteres(BaseData data)
        {
            return BuildParameteres(data, this.DicInsertMapping,true,false);
        }

        public virtual MySqlParameter[] BuildUpdateParameteres(BaseData data)
        {
            return BuildParameteres(data, this.DicUpdateMapping, false, true);
        }

        public virtual MySqlParameter[] BuildParameteres(BaseData data, Dictionary<string, string> DicData, bool isInsert, bool isUpdate)
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

                    string parameterName = ParameteraStartWith + key;

                    if (obj.GetType().BaseType == typeof(Enum))
                    {
                        param.Add(new MySqlParameter(parameterName, (int)obj));
                    }
                    else if (obj.GetType().BaseType == typeof(Galant.DataEntity.BaseData))
                    {
                        param.Add(new MySqlParameter(parameterName, (obj as Galant.DataEntity.BaseData).QueryId));
                    }
                    else
                    {
                        param.Add(new MySqlParameter(parameterName, obj));
                    }
                    if (key == this.KeyId)
                    {
                        keyParam = key + "=" + parameterName;
                    }
                    else
                    {
                        updateParamters.Add(key + "=" + parameterName);
                        conditions.Add(key);
                        parameters.Add(parameterName);
                    }
                }
            }
            if (isUpdate)
            {
                SqlUpdateDataSql = string.Format(SqlUpdateStringFormat, TableName, string.Join(" ,", updateParamters), keyParam);
            }
            if (isInsert)
            {
                SqlInsertDataSql = string.Format(SqlInsertStringFormat, TableName, String.Join(" ,", conditions), String.Join(" ,", parameters));
            }
            return param.ToArray();
        }

        private static string MarkParameter(string propertyName)
        {
            return string.Format("{0}{1}", ParameteraStartWith, propertyName);
        }

        public virtual List<BaseData> SearchByKeyId(string id)
        {
            String SqlSearch = BuildSearchSQLByKey(id);
            DataTable dt = SqlHelper.ExecuteDataset(Operator.myConnection, CommandType.Text, SqlSearch).Tables[0];
            return MappingTable(dt);
        }

        public virtual BaseData SearchById(string id)
        {
            String SqlSearch = BuildSearchSQLByKey(id);
            DataTable dt = SqlHelper.ExecuteDataset(Operator.myConnection, CommandType.Text, SqlSearch).Tables[0];
            if (dt.Rows.Count == 0)
                return null;//应该报错
            return MappingRow(dt.Rows[0]);
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

        public virtual BaseData MappingRow(DataRow row)
        {
            BaseData data = BuildNewBaseData(this.GetType().Name);
            foreach (System.Reflection.PropertyInfo info in data.GetType().GetProperties())
            {
                string key = string.Empty;
                if (DicDataMapping.TryGetValue(info.Name, out key))
                {
                    if (!String.IsNullOrEmpty(row[key].ToString()))
                    {
                        info.SetValue(data, row[key], null);
                    }
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
                case "Product":
                    return new Galant.DataEntity.Product();
                case "Package":
                    return new Galant.DataEntity.Package();
                case "Store":
                    return new Galant.DataEntity.Store();
                case "Paper":
                    return new Galant.DataEntity.Paper();
                case "PaperCheckin":
                    return new Galant.DataEntity.PaperCheckin();
                case "EventLog":
                    return new Galant.DataEntity.EventLog();
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

        public String ReadLastInsertId()
        {
            object obj= MySqlHelper.ExecuteScalar(this.Operator.myConnection, sqlLastInsertId);
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 增加事件
        /// </summary>
        protected void AddEvent(Galant.DataEntity.EventLog data)
        {
            EventLog eventlog = new EventLog(this.Operator);
            eventlog.AddNewData(data);
        }
    }
}
