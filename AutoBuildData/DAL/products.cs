using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using Maticsoft.DBUtility;//Please add references
namespace Galant.DAL
{
	/// <summary>
	/// 数据访问类:products
	/// </summary>
	public class products
	{
		public products()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("Product_id", "products"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Product_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from products");
			strSql.Append(" where Product_id=@Product_id ");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_id", MySqlDbType.Int32)};
			parameters[0].Value = Product_id;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(Galant.Model.products model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into products(");
			strSql.Append("Product_Name,Alias,Amount,Type,Discretion,Need_back,Return_Name,Return_Value,Able_flag)");
			strSql.Append(" values (");
			strSql.Append("@Product_Name,@Alias,@Amount,@Type,@Discretion,@Need_back,@Return_Name,@Return_Value,@Able_flag)");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_Name", MySqlDbType.VarChar,20),
					new MySqlParameter("@Alias", MySqlDbType.VarChar,20),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Discretion", MySqlDbType.Text),
					new MySqlParameter("@Need_back", MySqlDbType.Int32,1),
					new MySqlParameter("@Return_Name", MySqlDbType.VarChar,20),
					new MySqlParameter("@Return_Value", MySqlDbType.Decimal,10),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Product_Name;
			parameters[1].Value = model.Alias;
			parameters[2].Value = model.Amount;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.Discretion;
			parameters[5].Value = model.Need_back;
			parameters[6].Value = model.Return_Name;
			parameters[7].Value = model.Return_Value;
			parameters[8].Value = model.Able_flag;

			DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Galant.Model.products model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update products set ");
			strSql.Append("Product_Name=@Product_Name,");
			strSql.Append("Alias=@Alias,");
			strSql.Append("Amount=@Amount,");
			strSql.Append("Type=@Type,");
			strSql.Append("Discretion=@Discretion,");
			strSql.Append("Need_back=@Need_back,");
			strSql.Append("Return_Name=@Return_Name,");
			strSql.Append("Return_Value=@Return_Value,");
			strSql.Append("Able_flag=@Able_flag");
			strSql.Append(" where Product_id=@Product_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_id", MySqlDbType.Int32,11),
					new MySqlParameter("@Product_Name", MySqlDbType.VarChar,20),
					new MySqlParameter("@Alias", MySqlDbType.VarChar,20),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@Type", MySqlDbType.Int32,11),
					new MySqlParameter("@Discretion", MySqlDbType.Text),
					new MySqlParameter("@Need_back", MySqlDbType.Int32,1),
					new MySqlParameter("@Return_Name", MySqlDbType.VarChar,20),
					new MySqlParameter("@Return_Value", MySqlDbType.Decimal,10),
					new MySqlParameter("@Able_flag", MySqlDbType.Int32,1)};
			parameters[0].Value = model.Product_id;
			parameters[1].Value = model.Product_Name;
			parameters[2].Value = model.Alias;
			parameters[3].Value = model.Amount;
			parameters[4].Value = model.Type;
			parameters[5].Value = model.Discretion;
			parameters[6].Value = model.Need_back;
			parameters[7].Value = model.Return_Name;
			parameters[8].Value = model.Return_Value;
			parameters[9].Value = model.Able_flag;

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
		public bool Delete(int Product_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from products ");
			strSql.Append(" where Product_id=@Product_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_id", MySqlDbType.Int32)
};
			parameters[0].Value = Product_id;

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
		public bool DeleteList(string Product_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from products ");
			strSql.Append(" where Product_id in ("+Product_idlist + ")  ");
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
		public Galant.Model.products GetModel(int Product_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Product_id,Product_Name,Alias,Amount,Type,Discretion,Need_back,Return_Name,Return_Value,Able_flag from products ");
			strSql.Append(" where Product_id=@Product_id");
			MySqlParameter[] parameters = {
					new MySqlParameter("@Product_id", MySqlDbType.Int32)
};
			parameters[0].Value = Product_id;

			Galant.Model.products model=new Galant.Model.products();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Product_id"].ToString()!="")
				{
					model.Product_id=int.Parse(ds.Tables[0].Rows[0]["Product_id"].ToString());
				}
				model.Product_Name=ds.Tables[0].Rows[0]["Product_Name"].ToString();
				model.Alias=ds.Tables[0].Rows[0]["Alias"].ToString();
				if(ds.Tables[0].Rows[0]["Amount"].ToString()!="")
				{
					model.Amount=decimal.Parse(ds.Tables[0].Rows[0]["Amount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Type"].ToString()!="")
				{
					model.Type=int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
				}
				model.Discretion=ds.Tables[0].Rows[0]["Discretion"].ToString();
				if(ds.Tables[0].Rows[0]["Need_back"].ToString()!="")
				{
					model.Need_back=int.Parse(ds.Tables[0].Rows[0]["Need_back"].ToString());
				}
				model.Return_Name=ds.Tables[0].Rows[0]["Return_Name"].ToString();
				if(ds.Tables[0].Rows[0]["Return_Value"].ToString()!="")
				{
					model.Return_Value=decimal.Parse(ds.Tables[0].Rows[0]["Return_Value"].ToString());
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
			strSql.Append("select Product_id,Product_Name,Alias,Amount,Type,Discretion,Need_back,Return_Name,Return_Value,Able_flag ");
			strSql.Append(" FROM products ");
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
			parameters[0].Value = "products";
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

