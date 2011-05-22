using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    /// <summary>
    /// 应用程序对象  缓存
    /// </summary>
    [DataContract]
    public class AppStatusCach : BaseData
    {
        public AppStatusCach() : base() { }

        public AppStatusCach(string Operation)
            : base(Operation)
        {
        }

        private Entity staffCurrent;

        private Entity stationCurrent;
        public Entity StationCurrent
        {
            get { return stationCurrent; }
            set { 
                stationCurrent = value;
                if (staffCurrent != null)
                    StaffCurrent.CurerentStationID = value == null ? null : value.EntityId;
                OnPropertyChanged("StationCurrent"); }
        }

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        [DataMember]
        public Entity StaffCurrent
        {
            get { return staffCurrent; }
            set
            {
                staffCurrent = value;
                this.Powers = this.Getpowers(value);
                OnPropertyChanged("StaffCurrent");
                OnPropertyChanged("Powers");
            }
        }

        /// <summary>
        /// 是否有公司经理权限
        /// </summary>
        public bool IsShowHq
        {
            get
            {
                return this.IsManager || (from p in this.AllowStations
                        where p.EntityType== EntityType.Headquarter select p).Count() > 0;
            }
        }

        /// <summary>
        /// 是否有公司经理权限
        /// </summary>
        public bool IsManager
        {
            get 
            {
                return (from p in Powers where p == RoleType.CustomerSupportManager || 
                            p== RoleType.FinanceManager ||
                            p== RoleType.GeneralManager ||
                            p== RoleType.OperationManager select p).Count()>0;
            }
        }
        /// <summary>
        /// 是否显示客服功能
        /// </summary>
        public bool IsShowCustomerService
        {
            get
            {
                return (from p in Powers
                        where p == RoleType.CustomerSupportManager ||
                        p == RoleType.CustomerSupport ||
                            p == RoleType.GeneralManager ||
                            p == RoleType.OperationManager ||
                            p == RoleType.StationManager
                        select p).Count() > 0;
            }
        }

        /// <summary>
        /// 是否显示调度
        /// </summary>
        public bool IsShowRoute
        {
            get
            {
                return (from p in Powers
                        where p == RoleType.CustomerSupportManager ||
                         p == RoleType.CustomerSupport ||
                            p == RoleType.GeneralManager ||
                            p == RoleType.OperationManager ||
                            p == RoleType.StationManager
                        select p).Count() > 0;
            }
        }

        /// <summary>
        /// 是否显示水站
        /// </summary>
        public bool IsShowStation
        {
            get
            {
                return (from p in Powers
                        where p == RoleType.CustomerSupportManager ||
                         p == RoleType.CustomerSupport ||
                            p == RoleType.GeneralManager ||
                            p == RoleType.OperationManager ||
                            p == RoleType.StationManager
                        select p).Count() > 0;
            }
        }

        /// <summary>
        /// 是否显示系统管理
        /// </summary>
        public bool IsShowSystemAdmin
        {
            get
            {
                return (from p in Powers
                        where p == RoleType.GeneralManager ||
                            p == RoleType.OperationManager 
                        select p).Count() > 0;
            }
        }

        private List<RoleType> Getpowers(Entity user)
        {
            return user.Roles==null? new List<RoleType>() : (from r in user.Roles select r.RoleType).ToList();
        }

        public List<Entity> allowStations;

        /// <summary>
        /// 有权限的站点集合
        /// </summary>
        public List<Entity> AllowStations
        {
            get {
                if (allowStations == null)
                    allowStations = (this.StaffCurrent.Roles == null ? new List<Entity>() : (from r in StaffCurrent.Roles select r.Station).ToList());
                return IsManager ? this.Stations : allowStations; }
            set { allowStations = value; }
        }

        List<RoleType> powers;
        /// <summary>
        /// 权限
        /// </summary>
       [IgnoreDataMember]
        public List<RoleType> Powers
        {
            get
            {
                if (powers == null)
                    this.Getpowers(this.StaffCurrent);
                return powers;
            }
            set
            {
                this.powers = value;
                OnPropertyChanged("Powers");
            }
        }

        private List<Entity> entities;
        /// <summary>
        /// 所有实体集和
        /// </summary>
        [DataMember]
        public List<Entity> Entities
        {
            get { return entities; }
            set
            {
                entities = value;
                OnPropertyChanged("Entities");
                OnPropertyChanged("Stations");
                OnPropertyChanged("Customers");
                OnPropertyChanged("Staffs");
                OnPropertyChanged("Headquarters");
            }
        }

        /// <summary>
        /// 总部
        /// </summary>
        public List<Entity> Headquarters
        {
            get { return (this.IsManager || this.IsShowHq) ? (Entities == null ? null : Entities.Where(e => e.EntityType == EntityType.Headquarter).ToList()) : null; }
        }


        /// <summary>
        /// 站点集合
        /// </summary>
        public List<Entity> Stations
        {
            get { return Entities == null ? null : Entities.Where(e => e.EntityType == EntityType.Station).ToList(); }
        }
        /// <summary>
        /// 客户集合
        /// </summary>
        public List<Entity> Customers
        {
            get { return Entities == null ? null : Entities.Where(e => e.EntityType == EntityType.Client).ToList(); }
        }

        /// <summary>
        /// 员工集合
        /// </summary>
        public List<Entity> Staffs
        {
            get { return Entities == null ? null : Entities.Where(e => e.EntityType == EntityType.Staff).ToList(); }
        }

        private List<Route> routes;
        /// <summary>
        /// 路线集合
        /// </summary>
        [DataMember]
        public List<Route> Routes
        {
            get { return routes; }
            set { routes = value; OnPropertyChanged("Routes"); }
        }

        private List<Product> products;
        /// <summary>
        /// 系统定义商品
        /// </summary>
        [DataMember]
        public List<Product> Products
        {
            get { return products; }
            set { products = value; OnPropertyChanged("Products"); }
        }

        /// <summary>
        /// 需要返回的商品
        /// </summary>
        [IgnoreDataMember]
        public List<Product> ProductsNeedReturn
        {
            get 
            {
                if (Products == null)
                    Products = new List<Product>();
                return Products.Where(p => p.NeedBack && p.ProductType == Galant.DataEntity.ProductEnum.Autonomy).ToList();
            }
        }

        /// <summary>
        /// 水票
        /// </summary>
        [IgnoreDataMember]
        public List<Product> ProductsTickets
        {
            get
            {
                if (Products == null)
                    Products = new List<Product>();
                return Products.Where(p => p.ProductType==ProductEnum.Ticket).ToList();
            }
        }


        /// <summary>
        /// 快递产品
        /// </summary>
        [IgnoreDataMember]
        public List<Product> ProductsDelivery
        {
            get
            {
                if (Products == null)
                    Products = new List<Product>();
                return Products.Where(p=>p.ProductType == Galant.DataEntity.ProductEnum.Delivery).ToList();
            }
        }

        /// <summary>
        /// 送水类产品
        /// </summary>
        [IgnoreDataMember]
        public List<Product> ProductsWarter
        {
            get
            {
                if (Products == null)
                    Products = new List<Product>();
                return Products.Where(p => p.ProductType == Galant.DataEntity.ProductEnum.Autonomy || p.ProductType== ProductEnum.Ticket).ToList();
            }
        }
    }
}
