using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum PaperStatus
    {
        Finish = 0, Adviced = 1, Transfer = 2, FinishProcessing = 9
    }
    [DataContract]
    public enum PaperSubState
    {
        FinishGood = 0x00,
        FinishWithLost = 0x01,
        FinishWithCancel = 0x02,
        FinishWithAbondon = 0x03,

        AdviceReceived = 0x10,
        AdviceInStation = 0x11,

        InStation = 0x20,
        InTransit = 0x21,
        Unloading = 0x22,
        Loading = 0x23,

        // These are only for state differentiation for PackageFinish. Will never appear in Database
        Implicit = 0x90,
        NextActionAssured = 0x91,
        CheckinException = 0x92,
        SemiDeliveryToReturn = 0x93,
        DeliveryToReturn = 0x94,
        SemiReturnToDelivery = 0x95,
        ReturnToDelivery = 0x96,
        NextFinish = 0x97,
        CheckinExceptionWithoutIn = 0x98
    }
    [DataContract]
    public enum PaperBound
    {
        Stay = 0x00,
        ToA = 0x01,
        ToB = 0x02,
        ToC = 0x03
    }
    [DataContract]
    public enum PaperType
    { 
        
    }

    [DataContract]
    public enum PaperMobileStatus
    {
        
    }

    [DataContract]
    public class Paper:BaseData
    {
        private String paperId;

        public String PaperId
        {
            get { return paperId; }
            set { paperId = value; OnPropertyChanged("PaperId"); }
        }
        private PaperStatus paperStatus;

        public PaperStatus PaperStatus
        {
            get { return paperStatus; }
            set { paperStatus = value; OnPropertyChanged("PaperStatus"); }
        }
        private PaperSubState paperSubStatus;

        public PaperSubState PaperSubStatus
        {
            get { return paperSubStatus; }
            set { paperSubStatus = value; OnPropertyChanged("PaperSubStatus"); }
        }
        private Entity holder;

        public Entity Holder
        {
            get { return holder; }
            set { holder = value; OnPropertyChanged("Holder"); }
        }
        private PaperBound bound;

        public PaperBound Bound
        {
            get { return bound; }
            set { bound = value; OnPropertyChanged("Bound"); }
        }
        private Entity contactA;

        public Entity ContactA
        {
            get { return contactA; }
            set { contactA = value; OnPropertyChanged("ContactA"); }
        }
        private Entity contactB;

        public Entity ContactB
        {
            get { return contactB; }
            set { contactB = value; OnPropertyChanged("ContactB"); }
        }
        private Entity contactC;

        public Entity ContactC
        {
            get { return contactC; }
            set { contactC = value; OnPropertyChanged("ContactC"); }
        }
        private Entity deliverA;

        public Entity DeliverA
        {
            get { return deliverA; }
            set { deliverA = value; OnPropertyChanged("DeliverA"); }
        }
        private Entity deliverB;

        public Entity DeliverB
        {
            get { return deliverB; }
            set { deliverB = value; OnPropertyChanged("DeliverB"); }
        }
        private Entity deliverC;

        public Entity DeliverC
        {
            get { return deliverC; }
            set { deliverC = value; OnPropertyChanged("DeliverC"); }
        }

        private DateTime deliverATime;

        public DateTime DeliverATime
        {
            get { return deliverATime; }
            set { deliverATime = value; OnPropertyChanged("DeliverATime"); }
        }
        private DateTime deliverBTime;

        public DateTime DeliverBTime
        {
            get { return deliverBTime; }
            set { deliverBTime = value; OnPropertyChanged("DeliverBTime"); }
        }
        private DateTime deliverCTime;

        public DateTime DeliverCTime
        {
            get { return deliverCTime; }
            set { deliverCTime = value; OnPropertyChanged("DeliverCTime"); }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged("StartTime"); }
        }
        private DateTime finishTime;

        public DateTime FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; OnPropertyChanged("FinishTime"); }
        }

        private decimal salary;

        public decimal Salary
        {
            get { return salary; }
            set { salary = value; OnPropertyChanged("Salary"); }
        }
        private string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged("Comment"); }
        }
        private PaperType paperType;

        public PaperType PaperType
        {
            get { return paperType; }
            set { paperType = value; OnPropertyChanged("PaperType"); }
        }
        private Route nextRoute;

        public Route NextRoute
        {
            get { return nextRoute; }
            set { nextRoute = value; OnPropertyChanged("NextRoute"); }
        }
        private PaperMobileStatus mobileStatus;

        public PaperMobileStatus MobileStatus
        {
            get { return mobileStatus; }
            set { mobileStatus = value; OnPropertyChanged("MobileStatus"); }
        }

        private List<String> originName;
        public List<String> OriginName
        {
            get { return originName; }
            set { originName = value; OnPropertyChanged("OriginName"); }
        }

        private List<Paper> childPapers;
        public List<Paper> ChildPapers
        {
            get { return childPapers; }
            set { childPapers = value; OnPropertyChanged("ChildPapers"); }
        }

        private List<Route> routes;
        public List<Route> Routes
        {
            get { return routes; }
            set { routes = value; OnPropertyChanged("Routes"); }
        }

        private List<Package> packages;

        public List<Package> Packages
        {
            get { return packages; }
            set { packages = value; OnPropertyChanged("Packages"); }
        }
    }
}
