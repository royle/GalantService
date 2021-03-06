using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:origin_paper_links
	/// </summary>
	public class origin_paper_links
	{
		public origin_paper_links()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Link_id", "origin_paper_links"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Link_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from origin_paper_links");
			strSql.Append(" where Link_id=@Link_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Link_id", MySqlDbType.Int32)};
			parameters[0].Value = Link_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.origin_paper_links model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into origin_paper_links(");
			strSql.Append("Paper_id,Origin_Name)");
			strSql.Append(" values (");
			strSql.Append("@Paper_id,@Origin_Name)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Origin_Name", MySqlDbType.VarChar,50)};
			parameters[0].Value = model.Paper_id;
			parameters[1].Value = model.Origin_Name;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.origin_paper_links model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update origin_paper_links set ");
			strSql.Append("Paper_id=@Paper_id,");
			strSql.Append("Origin_Name=@Origin_Name");
			strSql.Append(" where Link_id=@Link_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Link_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Origin_Name", MySqlDbType.VarChar,50)};
			parameters[0].Value = model.Link_id;
			parameters[1].Value = model.Paper_id;
			parameters[2].Value = model.Origin_Name;

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
		public bool Delete(int Link_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from origin_paper_links ");
			strSql.Append(" where Link_id=@Link_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Link_id", MySqlDbType.Int32)
};
			parameters[0].Value = Link_id;

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
		public bool DeleteList(string Link_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from origin_paper_links ");
			strSql.Append(" where Link_id in ("+Link_idlist + ")  ");
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
		public Galant.Model.origin_paper_links GetModel(int Link_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Link_id,Paper_id,Origin_Name from origin_paper_links ");
			strSql.Append(" where Link_id=@Link_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Link_id", MySqlDbType.Int32)
};
			parameters[0].Value = Link_id;

			Galant.Model.origin_paper_links model=new Galant.Model.origin_paper_links();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Link_id"].ToString()!="")
				{
					model.Link_id=int.Parse(ds.Tables[0].Rows[0]["Link_id"].ToString());
				}
				model.Paper_id=ds.Tables[0].Rows[0]["Paper_id"].ToString();
				model.Origin_Name=ds.Tables[0].Rows[0]["Origin_Name"].ToString();
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
			strSql.Append("select Link_id,Paper_id,Origin_Name ");
			strSql.Append(" FROM origin_paper_links ");
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
			parameters[0].Value = "origin_paper_links";
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

