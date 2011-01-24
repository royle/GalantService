using System;
namespace Galant.Model
{
	/// <summary>
	/// papers:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class papers
	{
		public papers()
		{}
		#region Model
		private string _paper_id;
		private int _status;
		private int _substate;
		private int _holder;
		private int _bound;
		private int _contact_a;
		private int _contact_b;
		private int _contact_c;
		private int? _deliver_a;
		private int? _deliver_b;
		private int? _deliver_c;
		private DateTime? _deliver_a_time;
		private DateTime? _deliver_b_time;
		private DateTime? _deliver_c_time;
		private DateTime? _start_time;
		private DateTime? _finish_time;
		private decimal? _salary;
		private string _comment;
		private int? _type;
		private int? _next_route;
		private int? _mobile_status;
		/// <summary>
		/// 
		/// </summary>
		public string paper_id
		{
			set{ _paper_id=value;}
			get{return _paper_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SubState
		{
			set{ _substate=value;}
			get{return _substate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Holder
		{
			set{ _holder=value;}
			get{return _holder;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Bound
		{
			set{ _bound=value;}
			get{return _bound;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Contact_a
		{
			set{ _contact_a=value;}
			get{return _contact_a;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Contact_b
		{
			set{ _contact_b=value;}
			get{return _contact_b;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Contact_c
		{
			set{ _contact_c=value;}
			get{return _contact_c;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Deliver_a
		{
			set{ _deliver_a=value;}
			get{return _deliver_a;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Deliver_b
		{
			set{ _deliver_b=value;}
			get{return _deliver_b;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Deliver_c
		{
			set{ _deliver_c=value;}
			get{return _deliver_c;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Deliver_a_time
		{
			set{ _deliver_a_time=value;}
			get{return _deliver_a_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Deliver_b_time
		{
			set{ _deliver_b_time=value;}
			get{return _deliver_b_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Deliver_c_time
		{
			set{ _deliver_c_time=value;}
			get{return _deliver_c_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Start_time
		{
			set{ _start_time=value;}
			get{return _start_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Finish_time
		{
			set{ _finish_time=value;}
			get{return _finish_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Salary
		{
			set{ _salary=value;}
			get{return _salary;}
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
		public int? Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Next_Route
		{
			set{ _next_route=value;}
			get{return _next_route;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Mobile_Status
		{
			set{ _mobile_status=value;}
			get{return _mobile_status;}
		}
		#endregion Model

	}
}

