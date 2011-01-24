using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// ���ݷ�����:paper_routes
	/// </summary>
	public class paper_routes
	{
		public paper_routes()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Step_id", "paper_routes"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int Step_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from paper_routes");
			strSql.Append(" where Step_id=@Step_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Step_id", MySqlDbType.Int32)};
			parameters[0].Value = Step_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(Galant.Model.paper_routes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into paper_routes(");
			strSql.Append("Paper_id,Route_id,Is_Routed,Able_flag)");
			strSql.Append(" values (");
			strSql.Append("@Paper_id,@Route_id,@Is_Routed,@Able_flag)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Route_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Is_Routed", MySqlDbType.Int32,1),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Paper_id;
			parameters[1].Value = model.Route_id;
			parameters[2].Value = model.Is_Routed;
			parameters[3].Value = model.Able_flag;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(Galant.Model.paper_routes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update paper_routes set ");
			strSql.Append("Paper_id=@Paper_id,");
			strSql.Append("Route_id=@Route_id,");
			strSql.Append("Is_Routed=@Is_Routed,");
			strSql.Append("Able_flag=@Able_flag");
			strSql.Append(" where Step_id=@Step_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Step_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Paper_id", MySqlDbType.VarChar,8),
					new MySqlParameter("@Route_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Is_Routed", MySqlDbType.Int32,1),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Step_id;
			parameters[1].Value = model.Paper_id;
			parameters[2].Value = model.Route_id;
			parameters[3].Value = model.Is_Routed;
			parameters[4].Value = model.Able_flag;

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
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int Step_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from paper_routes ");
			strSql.Append(" where Step_id=@Step_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Step_id", MySqlDbType.Int32)
};
			parameters[0].Value = Step_id;

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
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string Step_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from paper_routes ");
			strSql.Append(" where Step_id in ("+Step_idlist + ")  ");
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
		/// �õ�һ������ʵ��
		/// </summary>
		public Galant.Model.paper_routes GetModel(int Step_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Step_id,Paper_id,Route_id,Is_Routed,Able_flag from paper_routes ");
			strSql.Append(" where Step_id=@Step_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Step_id", MySqlDbType.Int32)
};
			parameters[0].Value = Step_id;

			Galant.Model.paper_routes model=new Galant.Model.paper_routes();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Step_id"].ToString()!="")
				{
					model.Step_id=int.Parse(ds.Tables[0].Rows[0]["Step_id"].ToString());
				}
				model.Paper_id=ds.Tables[0].Rows[0]["Paper_id"].ToString();
				if(ds.Tables[0].Rows[0]["Route_id"].ToString()!="")
				{
					model.Route_id=int.Parse(ds.Tables[0].Rows[0]["Route_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Is_Routed"].ToString()!="")
				{
					model.Is_Routed=int.Parse(ds.Tables[0].Rows[0]["Is_Routed"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Able_flag"].ToString()!="")
				{
					model.Able_flag=int.Parse(ds.Tables[0].Rows[0]["Able_flag"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Step_id,Paper_id,Route_id,Is_Routed,Able_flag ");
			strSql.Append(" FROM paper_routes ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// ��ҳ��ȡ�����б�
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
			parameters[0].Value = "paper_routes";
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

