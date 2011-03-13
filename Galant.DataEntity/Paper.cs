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
            set { paperId = value; }
        }
        private PaperStatus paperStatus;

        public PaperStatus PaperStatus
        {
            get { return paperStatus; }
            set { paperStatus = value; }
        }
        private PaperSubState paperSubStatus;

        public PaperSubState PaperSubStatus
        {
            get { return paperSubStatus; }
            set { paperSubStatus = value; }
        }
        private Entity holder;

        public Entity Holder
        {
            get { return holder; }
            set { holder = value; }
        }
        private PaperBound bound;

        public PaperBound Bound
        {
            get { return bound; }
            set { bound = value; }
        }
        private Entity contactA;

        public Entity ContactA
        {
            get { return contactA; }
            set { contactA = value; }
        }
        private Entity contactB;

        public Entity ContactB
        {
            get { return contactB; }
            set { contactB = value; }
        }
        private Entity contactC;

        public Entity ContactC
        {
            get { return contactC; }
            set { contactC = value; }
        }
        private Entity deliverA;

        public Entity DeliverA
        {
            get { return deliverA; }
            set { deliverA = value; }
        }
        private Entity deliverB;

        public Entity DeliverB
        {
            get { return deliverB; }
            set { deliverB = value; }
        }
        private Entity deliverC;

        public Entity DeliverC
        {
            get { return deliverC; }
            set { deliverC = value; }
        }

        private DateTime deliverATime;

        public DateTime DeliverATime
        {
            get { return deliverATime; }
            set { deliverATime = value; }
        }
        private DateTime deliverBTime;

        public DateTime DeliverBTime
        {
            get { return deliverBTime; }
            set { deliverBTime = value; }
        }
        private DateTime deliverCTime;

        public DateTime DeliverCTime
        {
            get { return deliverCTime; }
            set { deliverCTime = value; }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime finishTime;

        public DateTime FinishTime
        {
            get { return finishTime; }
            set { finishTime = value; }
        }

        private decimal salary;

        public decimal Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        private string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        private PaperType paperType;

        public PaperType PaperType
        {
            get { return paperType; }
            set { paperType = value; }
        }
        private Route nextRoute;

        public Route NextRoute
        {
            get { return nextRoute; }
            set { nextRoute = value; }
        }
        private PaperMobileStatus mobileStatus;

        public PaperMobileStatus MobileStatus
        {
            get { return mobileStatus; }
            set { mobileStatus = value; }
        }

        private List<String> originName;
        public List<String> OriginName
        {
            get { return originName; }
            set { originName = value; }
        }

        private List<Paper> childPapers;
        public List<Paper> ChildPapers
        {
            get { return childPapers; }
            set { childPapers = value; }
        }

        private List<Route> routes;
        public List<Route> Routes
        {
            get { return routes; }
            set { routes = value; }
        }

        private List<Package> packages;

        public List<Package> Packages
        {
            get { return packages; }
            set { packages = value; }
        }
    }
}
