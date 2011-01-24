using System;
namespace Galant.Model
{
	/// <summary>
	/// products:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class products
	{
		public products()
		{}
		#region Model
		private int _product_id;
		private string _product_name;
		private string _alias;
		private decimal _amount;
		private int _type;
		private string _discretion;
		private int _need_back;
		private string _return_name;
		private decimal? _return_value;
		private int? _able_flag;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Product_id
		{
			set{ _product_id=value;}
			get{return _product_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Product_Name
		{
			set{ _product_name=value;}
			get{return _product_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Alias
		{
			set{ _alias=value;}
			get{return _alias;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Discretion
		{
			set{ _discretion=value;}
			get{return _discretion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Need_back
		{
			set{ _need_back=value;}
			get{return _need_back;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Return_Name
		{
			set{ _return_name=value;}
			get{return _return_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Return_Value
		{
			set{ _return_value=value;}
			get{return _return_value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Able_flag
		{
			set{ _able_flag=value;}
			get{return _able_flag;}
		}
		#endregion Model

	}
}

