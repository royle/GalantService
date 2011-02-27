using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public class AppStatusCach : BaseData
    {
        public AppStatusCach() : base() { }

        public AppStatusCach(string Operation)
            : base(Operation)
        {
        }

        private Entity staff;

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        [DataMember]
        public Entity Staff
        {
            get { return staff; }
            set { staff = value; OnPropertyChanged("Staff"); }
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
        public List<Route> Routes
        {
            get { return routes; }
            set { routes = value; OnPropertyChanged("Routes"); }
        }
    }
}
