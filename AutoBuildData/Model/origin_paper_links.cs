using System;
namespace Galant.Model
{
	/// <summary>
	/// origin_paper_links:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class origin_paper_links
	{
		public origin_paper_links()
		{}
		#region Model
		private int _link_id;
		private string _paper_id;
		private string _origin_name;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Link_id
		{
			set{ _link_id=value;}
			get{return _link_id;}
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
		public string Origin_Name
		{
			set{ _origin_name=value;}
			get{return _origin_name;}
		}
		#endregion Model

	}
}

