using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Galant.DataEntity
{
    [DataContract]
    public enum PaperStatus
    {
        [EnumMember]
        Finish = 0,
        [EnumMember]
        Adviced = 1,
        [EnumMember]
        Transfer = 2,
        [EnumMember]
        FinishProcessing = 9
    }
    [DataContract]
    public enum PaperSubState
    {
        [EnumMember]
        FinishGood = 0x00,
        [EnumMember]
        FinishWithLost = 0x01,
        [EnumMember]
        FinishWithCancel = 0x02,
        [EnumMember]
        FinishWithAbondon = 0x03,

        [EnumMember]
        AdviceReceived = 0x10,
        [EnumMember]
        AdviceInStation = 0x11,

        [EnumMember]
        InStation = 0x20,
        [EnumMember]
        InTransit = 0x21,
        [EnumMember]
        Unloading = 0x22,
        [EnumMember]
        Loading = 0x23,
        [EnumMember]
        Routeing = 0x24,

        // These are only for state differentiation for PackageFinish. Will never appear in Database
        [EnumMember]
        Implicit = 0x90,
        [EnumMember]
        NextActionAssured = 0x91,
        [EnumMember]
        CheckinException = 0x92,
        [EnumMember]
        SemiDeliveryToReturn = 0x93,
        [EnumMember]
        DeliveryToReturn = 0x94,
        [EnumMember]
        SemiReturnToDelivery = 0x95,
        [EnumMember]
        ReturnToDelivery = 0x96,
        [EnumMember]
        NextFinish = 0x97,
        [EnumMember]
        CheckinExceptionWithoutIn = 0x98
    }
    [DataContract]
    public enum PaperBound
    {
        [EnumMember]
        Stay = 0x00,
        [EnumMember]
        ToA = 0x01,
        [EnumMember]
        ToB = 0x02,
        [EnumMember]
        ToC = 0x03
    }
    [DataContract]
    public enum PaperType
    {
        /// <summary>
        /// 快递
        /// </summary>
        [EnumMember]
        Deliver = 0,
        /// <summary>
        /// 送水
        /// </summary>
        [EnumMember]
        Product =1
    }

    [DataContract]
    public class Paper:BaseData
    {
        public Paper():base()
        { }
        private String paperId;
        [DataMember]
        public String PaperId
        {
            get { return paperId; }
            set { paperId = value; OnPropertyChanged("PaperId"); }
        }
        private PaperStatus? paperStatus;
        [DataMember]
        public PaperStatus? PaperStatus
        {
            get { return paperStatus; }
            set { paperStatus = value; OnPropertyChanged("PaperStatus"); }
        }

        private PaperSubState? paperSubStatus;
        [DataMember]
        public PaperSubState? PaperSubStatus
        {
            get { return paperSubStatus; }
            set { paperSubStatus = value; OnPropertyChanged("PaperSubStatus"); }
        }
        private Entity holder;
        [DataMember]
        public Entity Holder
        {
            get { return holder; }
            set { holder = value; OnPropertyChanged("Holder"); }
        }
        private PaperBound? bound;
        [DataMember]
        public PaperBound? Bound
        {
            get { return bound; }
            set { bound = value; OnPropertyChanged("Bound"); }
        }
        private Entity contactA;
        [DataMember]
        public Entity ContactA
        {
            get { return contactA; }
            set { contactA = value; OnPropertyChanged("ContactA"); }
        }
        private Entity contactB;
        [DataMember]
        public Entity ContactB
        {
            get { return contactB; }
            set { contactB = value; OnPropertyChanged("ContactB"); }
        }
        private Entity contactC;
        [DataMember]
        public Entity ContactC
        {
            get { return contactC; }
            set { contactC = value; OnPropertyChanged("ContactC"); }
        }
        private Entity deliverA;
        [DataMember]
        public Entity DeliverA
        {
            get { return deliverA; }
            set { deliverA = value; OnPropertyChanged("DeliverA"); }
        }
        private Entity deliverB;
        [DataMember]
        public Entity DeliverB
        {
            get { return deliverB; }
            set { deliverB = value; OnPropertyChanged("DeliverB"); }
        }
        private Entity deliverC;
        [DataMember]
        public Entity DeliverC
        {
            get { return deliverC; }
            set { deliverC = value; OnPropertyChanged("DeliverC"); }
        }

        private DateTime? deliverATime;
        [DataMember]
        public DateTime? DeliverATime
        {
            get { return deliverATime; }
            set { deliverATime = value; OnPropertyChanged("DeliverATime"); }
        }
        private DateTime? deliverBTime;
        [DataMember]
        public DateTime? DeliverBTime
        {
            get { return deliverBTime; }
            set { deliverBTime = value; OnPropertyChanged("DeliverBTime"); }
        }
        private DateTime? deliverCTime;
        [DataMember]
        public DateTime? DeliverCTime
        {
            get { return deliverCTime; }
            set { deliverCTime = value; OnPropertyChanged("DeliverCTime"); }
        }

        private DateTime? startTime;
        [DataMember]
        public DateTime? StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged("StartTime"); }
        }
        private DateTime? finishTime;
        [DataMember]
        public DateTime? FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; OnPropertyChanged("FinishTime"); }
        }

        private decimal? salary;
        [DataMember]
        public decimal? Salary
        {
            get { return salary; }
            set { salary = value; OnPropertyChanged("Salary"); }
        }
        private string comment;
        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged("Comment"); }
        }
        private PaperType? paperType;
        [DataMember]
        public PaperType? PaperType
        {
            get { return paperType; }
            set { paperType = value; OnPropertyChanged("PaperType"); }
        }
        private Route nextRoute;
        [DataMember]
        public Route NextRoute
        {
            get { return nextRoute; }
            set { nextRoute = value; OnPropertyChanged("NextRoute"); OnPropertyChanged("NextEntity"); }
        }


        [IgnoreDataMember]
        public Entity NextEntity
        {
            get { return (NextRoute == null && NextRoute.ToEntity == null) ? new Entity() : NextRoute.ToEntity; }
            set
            {
                if (NextRoute == null)
                {
                    NextRoute = new Route() { IsFinally = false };
                }
                NextRoute.ToEntity = value; OnPropertyChanged("NextEntity"); OnPropertyChanged("NextRoute");
            }
        }

        private PaperSubState? mobileStatus;
        [DataMember]
        public PaperSubState? MobileStatus
        {
            get { return mobileStatus; }
            set { mobileStatus = value; OnPropertyChanged("MobileStatus"); }
        }

        private List<String> originName;
        [DataMember]
        public List<String> OriginName
        {
            get { return originName; }
            set { originName = value; OnPropertyChanged("OriginName"); OnPropertyChanged("OriginReference"); }
        }

        [IgnoreDataMember]
        public String OriginReference
        {
            get { return string.Join(Environment.NewLine, originName); }
        }

        private List<Paper> childPapers;
        [DataMember]
        public List<Paper> ChildPapers
        {
            get { return childPapers; }
            set { childPapers = value; OnPropertyChanged("ChildPapers"); }
        }

        public int? ChildsCount
        {
            get { return ChildPapers == null ? 0 : ChildPapers.Count; }
        }

        private Route routes;
        [DataMember]
        public Route Routes
        {
            get { return routes; }
            set { routes = value; OnPropertyChanged("Routes"); }
        }

        private ObservableCollection<Package> packages;
        [DataMember]
        public ObservableCollection<Package> Packages
        {
            get { return packages; }
            set { packages = value; OnPropertyChanged("Packages"); }
        }

        private bool isCollection;
        [DataMember]
        public bool IsCollection
        {
            get { return isCollection; }
            set { isCollection = value; OnPropertyChanged("IsCollection"); }
        }
        [IgnoreDataMember]
        public string NewSubStatus
        {
            get { return this.PaperSubStatus == null ? string.Empty : this.PaperSubStatus.ToString(); }
            set { this.PaperSubStatus = string.IsNullOrEmpty(value) ? this.PaperSubStatus : (Galant.DataEntity.PaperSubState)Enum.Parse(typeof(Galant.DataEntity.PaperSubState), value); }
        }
    }
}
