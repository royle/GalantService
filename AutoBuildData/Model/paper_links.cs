using System;
namespace Galant.Model
{
	/// <summary>
	/// paper_links:ʵ����(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class paper_links
	{
		public paper_links()
		{}
		#region Model
		private int _link_id;
		private string _paper_id;
		private int? _parent_id;
		private int? _able_flag;
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
		public int? Parent_id
		{
			set{ _parent_id=value;}
			get{return _parent_id;}
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

