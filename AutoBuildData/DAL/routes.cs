using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:routes
	/// </summary>
	public class routes
	{
		public routes()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Route_ID", "routes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Route_ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from routes");
			strSql.Append(" where Route_ID=@Route_ID ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Route_ID", MySqlDbType.Int32)};
			parameters[0].Value = Route_ID;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.routes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into routes(");
			strSql.Append("Route_Name,from_entity,to_entity,Is_finally)");
			strSql.Append(" values (");
			strSql.Append("@Route_Name,@from_entity,@to_entity,@Is_finally)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Route_Name", MySqlDbType.VarChar,32),
					new MySqlParameter("@from_entity", MySqlDbType.Int32,11),
					new MySqlParameter("@to_entity", MySqlDbType.Int32,11),
					new MySqlParameter("@Is_finally", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Route_Name;
			parameters[1].Value = model.from_entity;
			parameters[2].Value = model.to_entity;
			parameters[3].Value = model.Is_finally;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.routes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update routes set ");
			strSql.Append("Route_Name=@Route_Name,");
			strSql.Append("from_entity=@from_entity,");
			strSql.Append("to_entity=@to_entity,");
			strSql.Append("Is_finally=@Is_finally");
			strSql.Append(" where Route_ID=@Route_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Route_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@Route_Name", MySqlDbType.VarChar,32),
					new MySqlParameter("@from_entity", MySqlDbType.Int32,11),
					new MySqlParameter("@to_entity", MySqlDbType.Int32,11),
					new MySqlParameter("@Is_finally", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Route_ID;
			parameters[1].Value = model.Route_Name;
			parameters[2].Value = model.from_entity;
			parameters[3].Value = model.to_entity;
			parameters[4].Value = model.Is_finally;

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
		public bool Delete(int Route_ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from routes ");
			strSql.Append(" where Route_ID=@Route_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Route_ID", MySqlDbType.Int32)
};
			parameters[0].Value = Route_ID;

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
		public bool DeleteList(string Route_IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from routes ");
			strSql.Append(" where Route_ID in ("+Route_IDlist + ")  ");
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
		public Galant.Model.routes GetModel(int Route_ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Route_ID,Route_Name,from_entity,to_entity,Is_finally from routes ");
			strSql.Append(" where Route_ID=@Route_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Route_ID", MySqlDbType.Int32)
};
			parameters[0].Value = Route_ID;

			Galant.Model.routes model=new Galant.Model.routes();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Route_ID"].ToString()!="")
				{
					model.Route_ID=int.Parse(ds.Tables[0].Rows[0]["Route_ID"].ToString());
				}
				model.Route_Name=ds.Tables[0].Rows[0]["Route_Name"].ToString();
				if(ds.Tables[0].Rows[0]["from_entity"].ToString()!="")
				{
					model.from_entity=int.Parse(ds.Tables[0].Rows[0]["from_entity"].ToString());
				}
				if(ds.Tables[0].Rows[0]["to_entity"].ToString()!="")
				{
					model.to_entity=int.Parse(ds.Tables[0].Rows[0]["to_entity"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Is_finally"].ToString()!="")
				{
					model.Is_finally=int.Parse(ds.Tables[0].Rows[0]["Is_finally"].ToString());
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
			strSql.Append("select Route_ID,Route_Name,from_entity,to_entity,Is_finally ");
			strSql.Append(" FROM routes ");
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
			parameters[0].Value = "routes";
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

