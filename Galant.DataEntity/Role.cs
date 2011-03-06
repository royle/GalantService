using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum RoleType
    {
        /// <summary>
        /// 总经理
        /// </summary>
        [EnumMember]
        GeneralManager = 0x00,
        /// <summary>
        /// 营运经理
        /// </summary>
        [EnumMember]
        OperationManager = 0x01,
        /// <summary>
        /// 财务主管
        /// </summary>
        [EnumMember]
        FinanceManager = 0x02,
        /// <summary>
        /// 财务出纳
        /// </summary>
        [EnumMember]
        Finance = 0x03,
        /// <summary>
        /// 分拣主管
        /// </summary>
        [EnumMember]
        SortingManager = 0x04,
        /// <summary>
        /// 站长
        /// </summary>
        [EnumMember]
        StationManager = 0x05,
        /// <summary>
        /// 客服主管
        /// </summary>
        [EnumMember]
        CustomerSupportManager = 0x06,
        /// <summary>
        /// 客服
        /// </summary>
        [EnumMember]
        CustomerSupport = 0x07,
    }
    [DataContract]
    public class Role:BaseData
    {
        public Role(Entity entity) : base() 
        {
            this.StaffEntity = entity;
        }

        public Role()
            : base()
        { }

        private int? roleId;
        [DataMember]
        public int? RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private Entity StaffEntity;
        private int? entityId;
        [DataMember]
        public int? EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }

        private Entity station;
        [DataMember]
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
        [DataMember]
        public RoleType RoleType
        {
            get { return roleType; }
            set { roleType = value; }
        }
    }
}
