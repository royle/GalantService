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

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        [DataMember]
        public Entity StaffCurrent
        {
            get { return staffCurrent; }
            set { staffCurrent = value; OnPropertyChanged("StaffCurrent"); }
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
            }
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
    }
}
