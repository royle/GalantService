using System;
namespace Galant.Model
{
	/// <summary>
	/// event_logs:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class event_logs
	{
		public event_logs()
		{}
		#region Model
		private int _event_id;
		private string _paper_id;
		private DateTime _insert_time;
		private int _relation_entity;
		private int? _at_station;
		private string _event_type;
		private string _event_data;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Event_id
		{
			set{ _event_id=value;}
			get{return _event_id;}
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
		public DateTime Insert_Time
		{
			set{ _insert_time=value;}
			get{return _insert_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Relation_Entity
		{
			set{ _relation_entity=value;}
			get{return _relation_entity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? At_Station
		{
			set{ _at_station=value;}
			get{return _at_station;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Event_Type
		{
			set{ _event_type=value;}
			get{return _event_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Event_Data
		{
			set{ _event_data=value;}
			get{return _event_data;}
		}
		#endregion Model

	}
}

