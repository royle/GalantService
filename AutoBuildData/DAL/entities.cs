using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:entities
	/// </summary>
	public class entities
	{
		public entities()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Entity_id", "entities"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Entity_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from entities");
			strSql.Append(" where Entity_id=@Entity_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Entity_id", MySqlDbType.Int32)};
			parameters[0].Value = Entity_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.entities model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into entities(");
			strSql.Append("Alias,Password,Full_Name,Home_phone,Cell_phone1,Cell_phone2,Type,Address_Family,Address_Child,Comment,Store_log,Deposit,Pay_type,Route_Station,Able_flag)");
			strSql.Append(" values (");
			strSql.Append("@Alias,@Password,@Full_Name,@Home_phone,@Cell_phone1,@Cell_phone2,@Type,@Address_Family,@Address_Child,@Comment,@Store_log,@Deposit,@Pay_type,@Route_Station,@Able_flag)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Alias", MySqlDbType.VarChar,50),
					new MySqlParameter("@Password", MySqlDbType.VarChar,50),
					new MySqlParameter("@Full_Name", MySqlDbType.VarChar,50),
					new MySqlParameter("@Home_phone", MySqlDbType.VarChar,15),
					new MySqlParameter("@Cell_phone1", MySqlDbType.VarChar,15),
					new MySqlParameter("@Cell_phone2", MySqlDbType.VarChar,15),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Address_Family", MySqlDbType.Text),
					new MySqlParameter("@Address_Child", MySqlDbType.Text),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Store_log", MySqlDbType.Int32,11),
					new MySqlParameter("@Deposit", MySqlDbType.Decimal,10),
					new MySqlParameter("@Pay_type", MySqlDbType.Int32,11),
					new MySqlParameter("@Route_Station", MySqlDbType.Int32,11),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Alias;
			parameters[1].Value = model.Password;
			parameters[2].Value = model.Full_Name;
			parameters[3].Value = model.Home_phone;
			parameters[4].Value = model.Cell_phone1;
			parameters[5].Value = model.Cell_phone2;
			parameters[6].Value = model.Type;
			parameters[7].Value = model.Address_Family;
			parameters[8].Value = model.Address_Child;
			parameters[9].Value = model.Comment;
			parameters[10].Value = model.Store_log;
			parameters[11].Value = model.Deposit;
			parameters[12].Value = model.Pay_type;
			parameters[13].Value = model.Route_Station;
			parameters[14].Value = model.Able_flag;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.entities model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update entities set ");
			strSql.Append("Alias=@Alias,");
			strSql.Append("Password=@Password,");
			strSql.Append("Full_Name=@Full_Name,");
			strSql.Append("Home_phone=@Home_phone,");
			strSql.Append("Cell_phone1=@Cell_phone1,");
			strSql.Append("Cell_phone2=@Cell_phone2,");
			strSql.Append("Type=@Type,");
			strSql.Append("Address_Family=@Address_Family,");
			strSql.Append("Address_Child=@Address_Child,");
			strSql.Append("Comment=@Comment,");
			strSql.Append("Store_log=@Store_log,");
			strSql.Append("Deposit=@Deposit,");
			strSql.Append("Pay_type=@Pay_type,");
			strSql.Append("Route_Station=@Route_Station,");
			strSql.Append("Able_flag=@Able_flag");
			strSql.Append(" where Entity_id=@Entity_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Entity_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Alias", MySqlDbType.VarChar,50),
					new MySqlParameter("@Password", MySqlDbType.VarChar,50),
					new MySqlParameter("@Full_Name", MySqlDbType.VarChar,50),
					new MySqlParameter("@Home_phone", MySqlDbType.VarChar,15),
					new MySqlParameter("@Cell_phone1", MySqlDbType.VarChar,15),
					new MySqlParameter("@Cell_phone2", MySqlDbType.VarChar,15),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Address_Family", MySqlDbType.Text),
					new MySqlParameter("@Address_Child", MySqlDbType.Text),
					new MySqlParameter("@Comment", MySqlDbType.Text),
					new MySqlParameter("@Store_log", MySqlDbType.Int32,11),
					new MySqlParameter("@Deposit", MySqlDbType.Decimal,10),
					new MySqlParameter("@Pay_type", MySqlDbType.Int32,11),
					new MySqlParameter("@Route_Station", MySqlDbType.Int32,11),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Entity_id;
			parameters[1].Value = model.Alias;
			parameters[2].Value = model.Password;
			parameters[3].Value = model.Full_Name;
			parameters[4].Value = model.Home_phone;
			parameters[5].Value = model.Cell_phone1;
			parameters[6].Value = model.Cell_phone2;
			parameters[7].Value = model.Type;
			parameters[8].Value = model.Address_Family;
			parameters[9].Value = model.Address_Child;
			parameters[10].Value = model.Comment;
			parameters[11].Value = model.Store_log;
			parameters[12].Value = model.Deposit;
			parameters[13].Value = model.Pay_type;
			parameters[14].Value = model.Route_Station;
			parameters[15].Value = model.Able_flag;

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
		public bool Delete(int Entity_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from entities ");
			strSql.Append(" where Entity_id=@Entity_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Entity_id", MySqlDbType.Int32)
};
			parameters[0].Value = Entity_id;

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
		public bool DeleteList(string Entity_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from entities ");
			strSql.Append(" where Entity_id in ("+Entity_idlist + ")  ");
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
		public Galant.Model.entities GetModel(int Entity_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Entity_id,Alias,Password,Full_Name,Home_phone,Cell_phone1,Cell_phone2,Type,Address_Family,Address_Child,Comment,Store_log,Deposit,Pay_type,Route_Station,Able_flag from entities ");
			strSql.Append(" where Entity_id=@Entity_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Entity_id", MySqlDbType.Int32)
};
			parameters[0].Value = Entity_id;

			Galant.Model.entities model=new Galant.Model.entities();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Entity_id"].ToString()!="")
				{
					model.Entity_id=int.Parse(ds.Tables[0].Rows[0]["Entity_id"].ToString());
				}
				model.Alias=ds.Tables[0].Rows[0]["Alias"].ToString();
				model.Password=ds.Tables[0].Rows[0]["Password"].ToString();
				model.Full_Name=ds.Tables[0].Rows[0]["Full_Name"].ToString();
				model.Home_phone=ds.Tables[0].Rows[0]["Home_phone"].ToString();
				model.Cell_phone1=ds.Tables[0].Rows[0]["Cell_phone1"].ToString();
				model.Cell_phone2=ds.Tables[0].Rows[0]["Cell_phone2"].ToString();
				if(ds.Tables[0].Rows[0]["Type"].ToString()!="")
				{
					model.Type=int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
				}
				model.Address_Family=ds.Tables[0].Rows[0]["Address_Family"].ToString();
				model.Address_Child=ds.Tables[0].Rows[0]["Address_Child"].ToString();
				model.Comment=ds.Tables[0].Rows[0]["Comment"].ToString();
				if(ds.Tables[0].Rows[0]["Store_log"].ToString()!="")
				{
					model.Store_log=int.Parse(ds.Tables[0].Rows[0]["Store_log"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Deposit"].ToString()!="")
				{
					model.Deposit=decimal.Parse(ds.Tables[0].Rows[0]["Deposit"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Pay_type"].ToString()!="")
				{
					model.Pay_type=int.Parse(ds.Tables[0].Rows[0]["Pay_type"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Route_Station"].ToString()!="")
				{
					model.Route_Station=int.Parse(ds.Tables[0].Rows[0]["Route_Station"].ToString());
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Entity_id,Alias,Password,Full_Name,Home_phone,Cell_phone1,Cell_phone2,Type,Address_Family,Address_Child,Comment,Store_log,Deposit,Pay_type,Route_Station,Able_flag ");
			strSql.Append(" FROM entities ");
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
			parameters[0].Value = "entities";
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

