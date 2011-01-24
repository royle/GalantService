using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:papers
	/// </summary>
	public class papers
	{
		public papers()
		{}
		#region  Method

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string paper_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from papers");
			strSql.Append(" where paper_id=@paper_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@paper_id", MySqlDbType.VarChar)};
			parameters[0].Value = paper_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.papers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into papers(");
			strSql.Append("paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status)");
			strSql.Append(" values (");
			strSql.Append("@paper_id,@Status,@SubState,@Holder,@Bound,@Contact_a,@Contact_b,@Contact_c,@Deliver_a,@Deliver_b,@Deliver_c,@Deliver_a_time,@Deliver_b_time,@Deliver_c_time,@Start_time,@Finish_time,@Salary,@Comment,@Type,@Next_Route,@Mobile_Status)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Status", MySqlDbType.Int32,11),
					new MySqlParameter("@SubState", MySqlDbType.Int32,11),
					new MySqlParameter("@Holder", MySqlDbType.Int32,11),
					new MySqlParameter("@Bound", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_a", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_b", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_c", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_a", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_b", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_c", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_a_time", MySqlDbType.DateTime),
					new MySqlParameter("@Deliver_b_time", MySqlDbType.DateTime),
					new MySqlParameter("@Deliver_c_time", MySqlDbType.DateTime),
					new MySqlParameter("@Start_time", MySqlDbType.DateTime),
					new MySqlParameter("@Finish_time", MySqlDbType.DateTime),
					new MySqlParameter("@Salary", MySqlDbType.Decimal,10),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Next_Route", MySqlDbType.Int32,11),
					new MySqlParameter("@Mobile_Status", MySqlDbType.Int32,11)};
			parameters[0].Value = model.paper_id;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.SubState;
			parameters[3].Value = model.Holder;
			parameters[4].Value = model.Bound;
			parameters[5].Value = model.Contact_a;
			parameters[6].Value = model.Contact_b;
			parameters[7].Value = model.Contact_c;
			parameters[8].Value = model.Deliver_a;
			parameters[9].Value = model.Deliver_b;
			parameters[10].Value = model.Deliver_c;
			parameters[11].Value = model.Deliver_a_time;
			parameters[12].Value = model.Deliver_b_time;
			parameters[13].Value = model.Deliver_c_time;
			parameters[14].Value = model.Start_time;
			parameters[15].Value = model.Finish_time;
			parameters[16].Value = model.Salary;
			parameters[17].Value = model.Comment;
			parameters[18].Value = model.Type;
			parameters[19].Value = model.Next_Route;
			parameters[20].Value = model.Mobile_Status;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.papers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update papers set ");
			strSql.Append("Status=@Status,");
			strSql.Append("SubState=@SubState,");
			strSql.Append("Holder=@Holder,");
			strSql.Append("Bound=@Bound,");
			strSql.Append("Contact_a=@Contact_a,");
			strSql.Append("Contact_b=@Contact_b,");
			strSql.Append("Contact_c=@Contact_c,");
			strSql.Append("Deliver_a=@Deliver_a,");
			strSql.Append("Deliver_b=@Deliver_b,");
			strSql.Append("Deliver_c=@Deliver_c,");
			strSql.Append("Deliver_a_time=@Deliver_a_time,");
			strSql.Append("Deliver_b_time=@Deliver_b_time,");
			strSql.Append("Deliver_c_time=@Deliver_c_time,");
			strSql.Append("Start_time=@Start_time,");
			strSql.Append("Finish_time=@Finish_time,");
			strSql.Append("Salary=@Salary,");
			strSql.Append("Comment=@Comment,");
			strSql.Append("Type=@Type,");
			strSql.Append("Next_Route=@Next_Route,");
			strSql.Append("Mobile_Status=@Mobile_Status");
			strSql.Append(" where paper_id=@paper_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Status", MySqlDbType.Int32,11),
					new MySqlParameter("@SubState", MySqlDbType.Int32,11),
					new MySqlParameter("@Holder", MySqlDbType.Int32,11),
					new MySqlParameter("@Bound", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_a", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_b", MySqlDbType.Int32,11),
					new MySqlParameter("@Contact_c", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_a", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_b", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_c", MySqlDbType.Int32,11),
					new MySqlParameter("@Deliver_a_time", MySqlDbType.DateTime),
					new MySqlParameter("@Deliver_b_time", MySqlDbType.DateTime),
					new MySqlParameter("@Deliver_c_time", MySqlDbType.DateTime),
					new MySqlParameter("@Start_time", MySqlDbType.DateTime),
					new MySqlParameter("@Finish_time", MySqlDbType.DateTime),
					new MySqlParameter("@Salary", MySqlDbType.Decimal,10),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Next_Route", MySqlDbType.Int32,11),
					new MySqlParameter("@Mobile_Status", MySqlDbType.Int32,11)};
			parameters[0].Value = model.paper_id;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.SubState;
			parameters[3].Value = model.Holder;
			parameters[4].Value = model.Bound;
			parameters[5].Value = model.Contact_a;
			parameters[6].Value = model.Contact_b;
			parameters[7].Value = model.Contact_c;
			parameters[8].Value = model.Deliver_a;
			parameters[9].Value = model.Deliver_b;
			parameters[10].Value = model.Deliver_c;
			parameters[11].Value = model.Deliver_a_time;
			parameters[12].Value = model.Deliver_b_time;
			parameters[13].Value = model.Deliver_c_time;
			parameters[14].Value = model.Start_time;
			parameters[15].Value = model.Finish_time;
			parameters[16].Value = model.Salary;
			parameters[17].Value = model.Comment;
			parameters[18].Value = model.Type;
			parameters[19].Value = model.Next_Route;
			parameters[20].Value = model.Mobile_Status;

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
		public bool Delete(string paper_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from papers ");
			strSql.Append(" where paper_id=@paper_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@paper_id", MySqlDbType.VarChar)};
			parameters[0].Value = paper_id;

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
		public bool DeleteList(string paper_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from papers ");
			strSql.Append(" where paper_id in ("+paper_idlist + ")  ");
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
		public Galant.Model.papers GetModel(string paper_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status from papers ");
			strSql.Append(" where paper_id=@paper_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@paper_id", MySqlDbType.VarChar)};
			parameters[0].Value = paper_id;

			Galant.Model.papers model=new Galant.Model.papers();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				model.paper_id=ds.Tables[0].Rows[0]["paper_id"].ToString();
				if(ds.Tables[0].Rows[0]["Status"].ToString()!="")
				{
					model.Status=int.Parse(ds.Tables[0].Rows[0]["Status"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SubState"].ToString()!="")
				{
					model.SubState=int.Parse(ds.Tables[0].Rows[0]["SubState"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Holder"].ToString()!="")
				{
					model.Holder=int.Parse(ds.Tables[0].Rows[0]["Holder"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Bound"].ToString()!="")
				{
					model.Bound=int.Parse(ds.Tables[0].Rows[0]["Bound"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Contact_a"].ToString()!="")
				{
					model.Contact_a=int.Parse(ds.Tables[0].Rows[0]["Contact_a"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Contact_b"].ToString()!="")
				{
					model.Contact_b=int.Parse(ds.Tables[0].Rows[0]["Contact_b"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Contact_c"].ToString()!="")
				{
					model.Contact_c=int.Parse(ds.Tables[0].Rows[0]["Contact_c"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_a"].ToString()!="")
				{
					model.Deliver_a=int.Parse(ds.Tables[0].Rows[0]["Deliver_a"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_b"].ToString()!="")
				{
					model.Deliver_b=int.Parse(ds.Tables[0].Rows[0]["Deliver_b"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_c"].ToString()!="")
				{
					model.Deliver_c=int.Parse(ds.Tables[0].Rows[0]["Deliver_c"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_a_time"].ToString()!="")
				{
					model.Deliver_a_time=DateTime.Parse(ds.Tables[0].Rows[0]["Deliver_a_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_b_time"].ToString()!="")
				{
					model.Deliver_b_time=DateTime.Parse(ds.Tables[0].Rows[0]["Deliver_b_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deliver_c_time"].ToString()!="")
				{
					model.Deliver_c_time=DateTime.Parse(ds.Tables[0].Rows[0]["Deliver_c_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Start_time"].ToString()!="")
				{
					model.Start_time=DateTime.Parse(ds.Tables[0].Rows[0]["Start_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Finish_time"].ToString()!="")
				{
					model.Finish_time=DateTime.Parse(ds.Tables[0].Rows[0]["Finish_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Salary"].ToString()!="")
				{
					model.Salary=decimal.Parse(ds.Tables[0].Rows[0]["Salary"].ToString());
				}
				model.Comment=ds.Tables[0].Rows[0]["Comment"].ToString();
				if(ds.Tables[0].Rows[0]["Type"].ToString()!="")
				{
					model.Type=int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Next_Route"].ToString()!="")
				{
					model.Next_Route=int.Parse(ds.Tables[0].Rows[0]["Next_Route"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Mobile_Status"].ToString()!="")
				{
					model.Mobile_Status=int.Parse(ds.Tables[0].Rows[0]["Mobile_Status"].ToString());
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
			strSql.Append("select paper_id,Status,SubState,Holder,Bound,Contact_a,Contact_b,Contact_c,Deliver_a,Deliver_b,Deliver_c,Deliver_a_time,Deliver_b_time,Deliver_c_time,Start_time,Finish_time,Salary,Comment,Type,Next_Route,Mobile_Status ");
			strSql.Append(" FROM papers ");
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
			parameters[0].Value = "papers";
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

