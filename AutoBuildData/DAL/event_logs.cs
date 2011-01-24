using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:event_logs
	/// </summary>
	public class event_logs
	{
		public event_logs()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Event_id", "event_logs"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Event_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from event_logs");
			strSql.Append(" where Event_id=@Event_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Event_id", MySqlDbType.Int32)};
			parameters[0].Value = Event_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.event_logs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into event_logs(");
			strSql.Append("Paper_id,Insert_Time,Relation_Entity,At_Station,Event_Type,Event_Data)");
			strSql.Append(" values (");
			strSql.Append("@Paper_id,@Insert_Time,@Relation_Entity,@At_Station,@Event_Type,@Event_Data)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Insert_Time", MySqlDbType.DateTime),
					new MySqlParameter("@Relation_Entity", MySqlDbType.Int32,11),
					new MySqlParameter("@At_Station", MySqlDbType.Int32,11),
					new MySqlParameter("@Event_Type", MySqlDbType.VarChar,10),
					new MySqlParameter("@Event_Data", MySqlDbType.Text)};
			parameters[0].Value = model.Paper_id;
			parameters[1].Value = model.Insert_Time;
			parameters[2].Value = model.Relation_Entity;
			parameters[3].Value = model.At_Station;
			parameters[4].Value = model.Event_Type;
			parameters[5].Value = model.Event_Data;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.event_logs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update event_logs set ");
			strSql.Append("Paper_id=@Paper_id,");
			strSql.Append("Insert_Time=@Insert_Time,");
			strSql.Append("Relation_Entity=@Relation_Entity,");
			strSql.Append("At_Station=@At_Station,");
			strSql.Append("Event_Type=@Event_Type,");
			strSql.Append("Event_Data=@Event_Data");
			strSql.Append(" where Event_id=@Event_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Event_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Insert_Time", MySqlDbType.DateTime),
					new MySqlParameter("@Relation_Entity", MySqlDbType.Int32,11),
					new MySqlParameter("@At_Station", MySqlDbType.Int32,11),
					new MySqlParameter("@Event_Type", MySqlDbType.VarChar,10),
					new MySqlParameter("@Event_Data", MySqlDbType.Text)};
			parameters[0].Value = model.Event_id;
			parameters[1].Value = model.Paper_id;
			parameters[2].Value = model.Insert_Time;
			parameters[3].Value = model.Relation_Entity;
			parameters[4].Value = model.At_Station;
			parameters[5].Value = model.Event_Type;
			parameters[6].Value = model.Event_Data;

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
		public bool Delete(int Event_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from event_logs ");
			strSql.Append(" where Event_id=@Event_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Event_id", MySqlDbType.Int32)
};
			parameters[0].Value = Event_id;

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
		public bool DeleteList(string Event_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from event_logs ");
			strSql.Append(" where Event_id in ("+Event_idlist + ")  ");
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
		public Galant.Model.event_logs GetModel(int Event_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Event_id,Paper_id,Insert_Time,Relation_Entity,At_Station,Event_Type,Event_Data from event_logs ");
			strSql.Append(" where Event_id=@Event_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Event_id", MySqlDbType.Int32)
};
			parameters[0].Value = Event_id;

			Galant.Model.event_logs model=new Galant.Model.event_logs();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Event_id"].ToString()!="")
				{
					model.Event_id=int.Parse(ds.Tables[0].Rows[0]["Event_id"].ToString());
				}
				model.Paper_id=ds.Tables[0].Rows[0]["Paper_id"].ToString();
				if(ds.Tables[0].Rows[0]["Insert_Time"].ToString()!="")
				{
					model.Insert_Time=DateTime.Parse(ds.Tables[0].Rows[0]["Insert_Time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Relation_Entity"].ToString()!="")
				{
					model.Relation_Entity=int.Parse(ds.Tables[0].Rows[0]["Relation_Entity"].ToString());
				}
				if(ds.Tables[0].Rows[0]["At_Station"].ToString()!="")
				{
					model.At_Station=int.Parse(ds.Tables[0].Rows[0]["At_Station"].ToString());
				}
				model.Event_Type=ds.Tables[0].Rows[0]["Event_Type"].ToString();
				model.Event_Data=ds.Tables[0].Rows[0]["Event_Data"].ToString();
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
			strSql.Append("select Event_id,Paper_id,Insert_Time,Relation_Entity,At_Station,Event_Type,Event_Data ");
			strSql.Append(" FROM event_logs ");
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
			parameters[0].Value = "event_logs";
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

