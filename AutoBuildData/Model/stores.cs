using System;
namespace Galant.Model
{
	/// <summary>
	/// stores:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class stores
	{
		public stores()
		{}
		#region Model
		private int _store_id;
		private int _store_log;
		private int _product_id;
		private decimal? _product_count;
		private int? _bound;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Store_id
		{
			set{ _store_id=value;}
			get{return _store_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Store_log
		{
			set{ _store_log=value;}
			get{return _store_log;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int product_id
		{
			set{ _product_id=value;}
			get{return _product_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? product_count
		{
			set{ _product_count=value;}
			get{return _product_count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Bound
		{
			set{ _bound=value;}
			get{return _bound;}
		}
		#endregion Model

	}
}

