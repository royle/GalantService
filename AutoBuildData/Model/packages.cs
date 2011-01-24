using System;
namespace Galant.Model
{
	/// <summary>
	/// packages:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class packages
	{
		public packages()
		{}
		#region Model
		private int _package_id;
		private int _product_id;
		private decimal _count;
		private decimal _amount;
		private decimal _origin_amount;
		private string _paper_id;
		private int _state;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Package_id
		{
			set{ _package_id=value;}
			get{return _package_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Product_id
		{
			set{ _product_id=value;}
			get{return _product_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Count
		{
			set{ _count=value;}
			get{return _count;}
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
		public decimal Origin_Amount
		{
			set{ _origin_amount=value;}
			get{return _origin_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Paper_id
		{
			set{ _paper_id=value;}
			get{return _paper_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

