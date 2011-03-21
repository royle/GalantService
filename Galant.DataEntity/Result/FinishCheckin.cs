using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity.Result
{
    public class FinishCheckin:BaseData
    {
        private List<Paper> checkinPapers = new List<Paper>();
        [DataMember]
        public List<Paper> CheckinPapers
        {
            get { return checkinPapers; }
            set
            {
                checkinPapers = value;
                OnPropertyChanged("CheckinPapers");
                OnPropertyChanged("FinishPapers");
                OnPropertyChanged("ReDeliveryPapers");
                OnPropertyChanged("FailPapers");
            }
        }

         private List<Paper> checkinCollections = new List<Paper>();
         [DataMember]
         public List<Paper> CheckinCollections
         {
             get { return checkinCollections; }
             set { checkinCollections = value; }
         }

        /// <summary>
        /// 成功列表
        /// </summary>
        [IgnoreDataMember]
        public List<Paper> FinishPapers
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.NextActionAssured).ToList(); }
            set { }
        }
        /// <summary>
        /// 需要二次派送的列表
        /// </summary>
        [IgnoreDataMember]
        public List<Paper> ReDeliveryPapers
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.CheckinException).ToList(); }
            set { }
        }

        /// <summary>
        /// 派送失败的列表
        /// </summary>
        [IgnoreDataMember]
        public List<Paper> FailPapers
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.DeliveryToReturn).ToList(); }
            set { }
        }
    }
}
