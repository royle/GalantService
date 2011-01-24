using System;
namespace Galant.Model
{
	/// <summary>
	/// entities:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class entities
	{
		public entities()
		{}
		#region Model
		private int _entity_id;
		private string _alias;
		private string _password;
		private string _full_name;
		private string _home_phone;
		private string _cell_phone1;
		private string _cell_phone2;
		private int? _type;
		private string _address_family;
		private string _address_child;
		private string _comment;
		private int _store_log;
		private decimal _deposit;
		private int _pay_type;
		private int? _route_station;
		private int _able_flag=1;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Entity_id
		{
			set{ _entity_id=value;}
			get{return _entity_id;}
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
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Full_Name
		{
			set{ _full_name=value;}
			get{return _full_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Home_phone
		{
			set{ _home_phone=value;}
			get{return _home_phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Cell_phone1
		{
			set{ _cell_phone1=value;}
			get{return _cell_phone1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Cell_phone2
		{
			set{ _cell_phone2=value;}
			get{return _cell_phone2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address_Family
		{
			set{ _address_family=value;}
			get{return _address_family;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address_Child
		{
			set{ _address_child=value;}
			get{return _address_child;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Comment
		{
			set{ _comment=value;}
			get{return _comment;}
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
		public decimal Deposit
		{
			set{ _deposit=value;}
			get{return _deposit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Pay_type
		{
			set{ _pay_type=value;}
			get{return _pay_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Route_Station
		{
			set{ _route_station=value;}
			get{return _route_station;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Able_flag
		{
			set{ _able_flag=value;}
			get{return _able_flag;}
		}
		#endregion Model

	}
}

