using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Galant.DataEntity;
using MySql.Data.MySqlClient;
using GLTService.DBConnector;

namespace GLTService.Operation.BaseEntity
{
    public abstract class BaseOperator : IDisposable
    {
        public BaseOperator()
        {
            DicDataMapping = new Dictionary<string, string>();
            MappingDataName();
            SetTableName();
        }
        #region Base Operator For DB
        private MySqlTransaction mytransaction;
        private MySqlConnection myConnection;

        protected const string ConnectionString = @"";

        private void CreateConnection()
        {
            if (myConnection == null)
            {
                myConnection = new MySqlConnection(ConnectionString);
            }
            else if (myConnection.State == System.Data.ConnectionState.Closed || myConnection.State == System.Data.ConnectionState.Broken)
            {
                myConnection.Close();
            }
        }

        private void CreateTransaction()
        {
            CreateConnection();
            if (CanDoTransaction)
                mytransaction = myConnection.BeginTransaction();
        }

        private void Close()
        {
            if (CanDoTransaction)
                throw new Exception("Transaction Hasn't Commit or RollBack Plase Do it Frist");
            myConnection.Close();
        }

        private void CommitAndClose()
        {
            if (CanDoTransaction)
                mytransaction.Commit();
            Close();
        }

        private void RollBackAndClose()
        {
            if (CanDoTransaction)
                mytransaction.Rollback();
            Close();
        }

        private void RollBackAndClear()
        {
            RollBackAndClose();
            Clear();
        }

        private void Clear()
        {
            mytransaction = null;
            myConnection = null;
        }

        private void CommitAndClear()
        {
            CommitAndClose();
        }

        public virtual void CreateConnectionAndTransaction()
        {
            CreateConnection();
            CreateTransaction();
        }

        private bool CanDoTransaction
        {
            get { return IsNeedTransaction && (mytransaction == null); }
        }

        public virtual bool IsNeedTransaction
        {
            get { return true; }
        }

        #endregion //Base Operator For DB

        #region IDisposable Members

        public void Dispose()
        {
            Clear();
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
            CreateConnectionAndTransaction();

            if (CanDoTransaction)
                SqlHelper.ExecuteNonQuery(mytransaction, System.Data.CommandType.Text, SqlAddNewSql, BuildParameteres(data));
            else
                MySqlHelper.ExecuteNonQuery(myConnection, SqlAddNewSql, BuildParameteres(data));

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
    }
}
