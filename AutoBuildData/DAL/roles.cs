using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:roles
	/// </summary>
	public class roles
	{
		public roles()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Role_ID", "roles"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Role_ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from roles");
			strSql.Append(" where Role_ID=@Role_ID ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Role_ID", MySqlDbType.Int32)};
			parameters[0].Value = Role_ID;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into roles(");
			strSql.Append("entity_id,Station_id,Role_Type)");
			strSql.Append(" values (");
			strSql.Append("@entity_id,@Station_id,@Role_Type)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@entity_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Station_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Role_Type", MySqlDbType.Int32,11)};
			parameters[0].Value = model.entity_id;
			parameters[1].Value = model.Station_id;
			parameters[2].Value = model.Role_Type;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update roles set ");
			strSql.Append("entity_id=@entity_id,");
			strSql.Append("Station_id=@Station_id,");
			strSql.Append("Role_Type=@Role_Type");
			strSql.Append(" where Role_ID=@Role_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Role_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@entity_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Station_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Role_Type", MySqlDbType.Int32,11)};
			parameters[0].Value = model.Role_ID;
			parameters[1].Value = model.entity_id;
			parameters[2].Value = model.Station_id;
			parameters[3].Value = model.Role_Type;

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
		public bool Delete(int Role_ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from roles ");
			strSql.Append(" where Role_ID=@Role_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Role_ID", MySqlDbType.Int32)
};
			parameters[0].Value = Role_ID;

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
		public bool DeleteList(string Role_IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from roles ");
			strSql.Append(" where Role_ID in ("+Role_IDlist + ")  ");
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
		public Galant.Model.roles GetModel(int Role_ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Role_ID,entity_id,Station_id,Role_Type from roles ");
			strSql.Append(" where Role_ID=@Role_ID");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Role_ID", MySqlDbType.Int32)
};
			parameters[0].Value = Role_ID;

			Galant.Model.roles model=new Galant.Model.roles();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Role_ID"].ToString()!="")
				{
					model.Role_ID=int.Parse(ds.Tables[0].Rows[0]["Role_ID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["entity_id"].ToString()!="")
				{
					model.entity_id=int.Parse(ds.Tables[0].Rows[0]["entity_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Station_id"].ToString()!="")
				{
					model.Station_id=int.Parse(ds.Tables[0].Rows[0]["Station_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Role_Type"].ToString()!="")
				{
					model.Role_Type=int.Parse(ds.Tables[0].Rows[0]["Role_Type"].ToString());
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
			strSql.Append("select Role_ID,entity_id,Station_id,Role_Type ");
			strSql.Append(" FROM roles ");
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
			parameters[0].Value = "roles";
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

