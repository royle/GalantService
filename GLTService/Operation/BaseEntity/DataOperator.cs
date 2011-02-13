﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace GLTService.Operation.BaseEntity
{
    public abstract class DataOperator
    {
        public DataOperator(DataOperator data)
        {
            this.Operator = data;
        }

        private DataOperator _operator;
        public DataOperator Operator
        {
            set { _operator = value; }
            get { return _operator; }
        }

        #region Base Operator For DB
        internal MySqlTransaction mytransaction;
        internal MySqlConnection myConnection;

        internal const string ConnectionString = @"";

        internal void CreateConnection()
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

        internal void CreateTransaction()
        {
            CreateConnection();
            if (CanDoTransaction)
                mytransaction = myConnection.BeginTransaction();
        }

        internal void Close()
        {
            if (CanDoTransaction)
                throw new Exception("Transaction Hasn't Commit or RollBack Plase Do it Frist");
            myConnection.Close();
        }

        internal void CommitAndClose()
        {
            if (CanDoTransaction)
                mytransaction.Commit();
            Close();
        }

        internal void RollBackAndClose()
        {
            if (CanDoTransaction)
                mytransaction.Rollback();
            Close();
        }

        internal void RollBackAndClear()
        {
            RollBackAndClose();
            Clear();
        }

        internal void Clear()
        {
            mytransaction = null;
            myConnection = null;
        }

        internal void CommitAndClear()
        {
            CommitAndClose();
        }

        public virtual void CreateConnectionAndTransaction()
        {
            CreateConnection();
            CreateTransaction();
        }

        internal bool CanDoTransaction
        {
            get { return IsNeedTransaction && (mytransaction == null); }
        }

        public virtual bool IsNeedTransaction
        {
            get { return true; }
        }

        #endregion //Base Operator For DB

    }
}
