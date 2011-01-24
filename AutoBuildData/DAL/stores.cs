using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:stores
	/// </summary>
	public class stores
	{
		public stores()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Store_id", "stores"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Store_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from stores");
			strSql.Append(" where Store_id=@Store_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Store_id", MySqlDbType.Int32)};
			parameters[0].Value = Store_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.stores model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into stores(");
			strSql.Append("Store_log,product_id,product_count,Bound)");
			strSql.Append(" values (");
			strSql.Append("@Store_log,@product_id,@product_count,@Bound)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Store_log", MySqlDbType.Int32,11),
					new MySqlParameter("@product_id", MySqlDbType.Int32,11),
					new MySqlParameter("@product_count", MySqlDbType.Decimal,8),
					new MySqlParameter("@Bound", MySqlDbType.Int32,11)};
			parameters[0].Value = model.Store_log;
			parameters[1].Value = model.product_id;
			parameters[2].Value = model.product_count;
			parameters[3].Value = model.Bound;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.stores model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update stores set ");
			strSql.Append("Store_log=@Store_log,");
			strSql.Append("product_id=@product_id,");
			strSql.Append("product_count=@product_count,");
			strSql.Append("Bound=@Bound");
			strSql.Append(" where Store_id=@Store_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Store_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Store_log", MySqlDbType.Int32,11),
					new MySqlParameter("@product_id", MySqlDbType.Int32,11),
					new MySqlParameter("@product_count", MySqlDbType.Decimal,8),
					new MySqlParameter("@Bound", MySqlDbType.Int32,11)};
			parameters[0].Value = model.Store_id;
			parameters[1].Value = model.Store_log;
			parameters[2].Value = model.product_id;
			parameters[3].Value = model.product_count;
			parameters[4].Value = model.Bound;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Store_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from stores ");
			strSql.Append(" where Store_id=@Store_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Store_id", MySqlDbType.Int32)
};
			parameters[0].Value = Store_id;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Store_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from stores ");
			strSql.Append(" where Store_id in ("+Store_idlist + ")  ");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Galant.Model.stores GetModel(int Store_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Store_id,Store_log,product_id,product_count,Bound from stores ");
			strSql.Append(" where Store_id=@Store_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Store_id", MySqlDbType.Int32)
};
			parameters[0].Value = Store_id;

			Galant.Model.stores model=new Galant.Model.stores();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Store_id"].ToString()!="")
				{
					model.Store_id=int.Parse(ds.Tables[0].Rows[0]["Store_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Store_log"].ToString()!="")
				{
					model.Store_log=int.Parse(ds.Tables[0].Rows[0]["Store_log"].ToString());
				}
				if(ds.Tables[0].Rows[0]["product_id"].ToString()!="")
				{
					model.product_id=int.Parse(ds.Tables[0].Rows[0]["product_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["product_count"].ToString()!="")
				{
					model.product_count=decimal.Parse(ds.Tables[0].Rows[0]["product_count"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Bound"].ToString()!="")
				{
					model.Bound=int.Parse(ds.Tables[0].Rows[0]["Bound"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Store_id,Store_log,product_id,product_count,Bound ");
			strSql.Append(" FROM stores ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			MySqlParameter[] parameters = {
					new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@PageSize", MySqlDbType.Int32),
					new MySqlParameter("@PageIndex", MySqlDbType.Int32),
					new MySqlParameter("@IsReCount", MySqlDbType.Bit),
					new MySqlParameter("@OrderType", MySqlDbType.Bit),
					new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
					};
			parameters[0].Value = "stores";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

