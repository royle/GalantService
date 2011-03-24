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
        /// <summary>
        /// 归班订单列表
        /// </summary>
        [DataMember]
        public List<Paper> CheckinPapers
        {
            get { return checkinPapers; }
            set
            {
                checkinPapers = value;
                OnPropertyChanged("CheckinPapers");
                OnPropertyChanged("FinishDoneList");
                OnPropertyChanged("CheckinExceptionList");
                OnPropertyChanged("SemiDeliverToReturnList");
                OnPropertyChanged("WorkPendingList");
            }
        }

         private List<Paper> checkinCollections = new List<Paper>();
        /// <summary>
        /// 准备归班的清单
        /// </summary>
         [DataMember]
         public List<Paper> CheckinCollections
         {
             get { return checkinCollections; }
             set { checkinCollections = value; }
         }

         /// <summary>
         /// 未处理
         /// </summary>
         [IgnoreDataMember]
         public List<Paper> WorkPendingList
         {
             get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.InTransit).ToList(); }
             set { }
         }

         /// <summary>
         /// 已处理
         /// </summary>
         [IgnoreDataMember]
         public List<Paper> WorkDoneList
         {
             get { return CheckinPapers.Where(p => p.PaperSubStatus != Galant.DataEntity.PaperSubState.InTransit).ToList(); }
             set { }
         }


        /// <summary>
        /// 成功列表
        /// </summary>
        [IgnoreDataMember]
         public List<Paper> FinishDoneList
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.NextActionAssured).ToList(); }
            set { }
        }
        /// <summary>
        /// 需要二次派送的列表
        /// </summary>
        [IgnoreDataMember]
        public List<Paper> CheckinExceptionList
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.CheckinException).ToList(); }
            set { }
        }

        /// <summary>
        /// 派送失败的列表
        /// </summary>
        [IgnoreDataMember]
        public List<Paper> SemiDeliverToReturnList
        {
            get { return CheckinPapers.Where(p => p.PaperSubStatus == Galant.DataEntity.PaperSubState.DeliveryToReturn).ToList(); }
            set { }
        }
    }
}
