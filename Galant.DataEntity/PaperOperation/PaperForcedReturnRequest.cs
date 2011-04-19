using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity.PaperOperation
{
    /// <summary>
    /// 取消配送
    /// </summary>
    [DataContract]
    public class PaperForcedReturnRequest : BaseData
    {
        public PaperForcedReturnRequest(Paper p)
        {
            this.Paper = p;
        }

        Paper paper;
        [IgnoreDataMember]
        public Paper Paper
        {
            get { return paper; }
            set
            {
                paper = value;
                paperId = paper.PaperId;
                OnPropertyChanged("Paper");
                OnPropertyChanged("IsVisible");
                OnPropertyChanged("IsEnabled");
                OnPropertyChanged("Note");
            }
        }
        string paperId;
        [DataMember]
        public string PaperId
        {
            get { return paperId; }
            set { paperId = value; }
        }

        string note;
        [DataMember]
        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged("Note"); }
        }

        public bool IsVisible
        {
            get
            {
                return !this.Paper.IsCollection;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return this.Paper.PaperSubStatus != Galant.DataEntity.PaperSubState.FinishGood;
            }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "Note":
                    if (string.IsNullOrEmpty(Note)) return "必须填写原因。";
                    return string.Empty;
                case "":
                    if (!IsEnabled && !IsVisible) return "数据未能通过验证";
                    return string.Empty;
            }
            return null;
        }
    }
}
