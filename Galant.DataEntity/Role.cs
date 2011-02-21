using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum RoleType
    { }
    [DataContract]
    public class Role:BaseData
    {
        public Role(Entity entity) : base() 
        {
            this.StaffEntity = entity;
        }

        private int roleId;
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private Entity StaffEntity;
        public int? EntityId
        {
            get { return StaffEntity.EntityId; }
        }

        private Entity station;
        public Entity Station
        {
            get { return station; }
            set { station = value; }
        }

        public int? StationId
        {
            get { return Station.EntityId; }
        }

        private RoleType roleType;
        public RoleType RoleType
        {
            get { return roleType; }
            set { roleType = value; }
        }
    }
}
