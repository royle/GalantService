using System;
namespace Galant.Model
{
	/// <summary>
	/// roles:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class roles
	{
		public roles()
		{}
		#region Model
		private int _role_id;
		private int _entity_id;
		private int? _station_id;
		private int _role_type;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Role_ID
		{
			set{ _role_id=value;}
			get{return _role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int entity_id
		{
			set{ _entity_id=value;}
			get{return _entity_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Station_id
		{
			set{ _station_id=value;}
			get{return _station_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Role_Type
		{
			set{ _role_type=value;}
			get{return _role_type;}
		}
		#endregion Model

	}
}

