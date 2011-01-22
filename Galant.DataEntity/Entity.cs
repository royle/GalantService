using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    public enum EntityType
    {
        /// <summary>
        /// 总部
        /// </summary>
        Headquarter = 0x00,
        /// <summary>
        /// 站点
        /// </summary>
        Station = 0x01,
        /// <summary>
        /// 员工
        /// </summary>
        Staff = 0x02,
        /// <summary>
        /// 收件人
        /// </summary>
        Client = 0x03,
        /// <summary>
        /// 供应商
        /// </summary>
        Individual = 0x04,
    }

    public enum PayType
    {
        AtTime = 0, Before = 1,  After = 2
    }

    public class Entity:BaseData
    {
        private int entityId;

        public int EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }
        private String alias;

        public String Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        private String password;

        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        private String fullName;

        public String FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        private String homePhone;

        public String HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }
        private String cellPhoneOne;

        public String CellPhoneOne
        {
            get { return cellPhoneOne; }
            set { cellPhoneOne = value; }
        }
        private String cellPhoneTwo;

        public String CellPhoneTwo
        {
            get { return cellPhoneTwo; }
            set { cellPhoneTwo = value; }
        }
        private EntityType entityType;

        public EntityType EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }
        private String addressFamily;

        public String AddressFamily
        {
            get { return addressFamily; }
            set { addressFamily = value; }
        }
        private String addressChild;

        public String AddressChild
        {
            get { return addressChild; }
            set { addressChild = value; }
        }
        private String comment;

        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        private int storeLog;

        public int StoreLog
        {
            get { return storeLog; }
            set { storeLog = value; }
        }
        private decimal deposit;

        public decimal Deposit
        {
            get { return deposit; }
            set { deposit = value; }
        }
        private PayType payType;
        public PayType PayType
        {
            get { return payType; }
            set { payType = value; }
        }
        private int rountStation;
        public int RountStation
        {
            get { return rountStation; }
            set { rountStation = value; }
        }
        private bool ableFlag;
        public bool AbleFlag
        {
            get { return ableFlag; }
            set { ableFlag = value; }
        }

        private List<Role> roles;
        public List<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }
    }


}
