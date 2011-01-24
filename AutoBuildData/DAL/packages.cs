using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:packages
	/// </summary>
	public class packages
	{
		public packages()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Package_id", "packages"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Package_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from packages");
			strSql.Append(" where Package_id=@Package_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Package_id", MySqlDbType.Int32)};
			parameters[0].Value = Package_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.packages model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into packages(");
			strSql.Append("Product_id,Count,Amount,Origin_Amount,Paper_id,State)");
			strSql.Append(" values (");
			strSql.Append("@Product_id,@Count,@Amount,@Origin_Amount,@Paper_id,@State)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Count", MySqlDbType.Decimal,8),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Origin_Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,32),
					new MySqlParameter("@State", MySqlDbType.Int32,11)};
			parameters[0].Value = model.Product_id;
			parameters[1].Value = model.Count;
			parameters[2].Value = model.Amount;
			parameters[3].Value = model.Origin_Amount;
			parameters[4].Value = model.Paper_id;
			parameters[5].Value = model.State;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.packages model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update packages set ");
			strSql.Append("Product_id=@Product_id,");
			strSql.Append("Count=@Count,");
			strSql.Append("Amount=@Amount,");
			strSql.Append("Origin_Amount=@Origin_Amount,");
			strSql.Append("Paper_id=@Paper_id,");
			strSql.Append("State=@State");
			strSql.Append(" where Package_id=@Package_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Package_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Product_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Count", MySqlDbType.Decimal,8),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Origin_Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,32),
					new MySqlParameter("@State", MySqlDbType.Int32,11)};
			parameters[0].Value = model.Package_id;
			parameters[1].Value = model.Product_id;
			parameters[2].Value = model.Count;
			parameters[3].Value = model.Amount;
			parameters[4].Value = model.Origin_Amount;
			parameters[5].Value = model.Paper_id;
			parameters[6].Value = model.State;

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
		public bool Delete(int Package_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from packages ");
			strSql.Append(" where Package_id=@Package_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Package_id", MySqlDbType.Int32)
};
			parameters[0].Value = Package_id;

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
		public bool DeleteList(string Package_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from packages ");
			strSql.Append(" where Package_id in ("+Package_idlist + ")  ");
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
		public Galant.Model.packages GetModel(int Package_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Package_id,Product_id,Count,Amount,Origin_Amount,Paper_id,State from packages ");
			strSql.Append(" where Package_id=@Package_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Package_id", MySqlDbType.Int32)
};
			parameters[0].Value = Package_id;

			Galant.Model.packages model=new Galant.Model.packages();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Package_id"].ToString()!="")
				{
					model.Package_id=int.Parse(ds.Tables[0].Rows[0]["Package_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Product_id"].ToString()!="")
				{
					model.Product_id=int.Parse(ds.Tables[0].Rows[0]["Product_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Count"].ToString()!="")
				{
					model.Count=decimal.Parse(ds.Tables[0].Rows[0]["Count"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Amount"].ToString()!="")
				{
					model.Amount=decimal.Parse(ds.Tables[0].Rows[0]["Amount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Origin_Amount"].ToString()!="")
				{
					model.Origin_Amount=decimal.Parse(ds.Tables[0].Rows[0]["Origin_Amount"].ToString());
				}
				model.Paper_id=ds.Tables[0].Rows[0]["Paper_id"].ToString();
				if(ds.Tables[0].Rows[0]["State"].ToString()!="")
				{
					model.State=int.Parse(ds.Tables[0].Rows[0]["State"].ToString());
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
			strSql.Append("select Package_id,Product_id,Count,Amount,Origin_Amount,Paper_id,State ");
			strSql.Append(" FROM packages ");
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
			parameters[0].Value = "packages";
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

