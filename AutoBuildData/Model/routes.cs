using System;
namespace Galant.Model
{
	/// <summary>
	/// routes:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class routes
	{
		public routes()
		{}
		#region Model
		private int _route_id;
		private string _route_name;
		private int _from_entity;
		private int? _to_entity;
		private int _is_finally;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Route_ID
		{
			set{ _route_id=value;}
			get{return _route_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Route_Name
		{
			set{ _route_name=value;}
			get{return _route_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int from_entity
		{
			set{ _from_entity=value;}
			get{return _from_entity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? to_entity
		{
			set{ _to_entity=value;}
			get{return _to_entity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Is_finally
		{
			set{ _is_finally=value;}
			get{return _is_finally;}
		}
		#endregion Model

	}
}

