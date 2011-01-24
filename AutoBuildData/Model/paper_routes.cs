using System;
namespace Galant.Model
{
	/// <summary>
	/// paper_routes:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class paper_routes
	{
		public paper_routes()
		{}
		#region Model
		private int _step_id;
		private string _paper_id;
		private int _route_id;
		private int _is_routed;
		private int _able_flag;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Step_id
		{
			set{ _step_id=value;}
			get{return _step_id;}
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
		public int Route_id
		{
			set{ _route_id=value;}
			get{return _route_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Is_Routed
		{
			set{ _is_routed=value;}
			get{return _is_routed;}
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

