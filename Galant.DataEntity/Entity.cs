using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum EntityType
    {
        /// <summary>
        /// 总部
        /// </summary>
        [EnumMember]
        Headquarter = 0x00,
        /// <summary>
        /// 站点
        /// </summary>
        [EnumMember]
        Station = 0x01,
        /// <summary>
        /// 员工
        /// </summary>
        [EnumMember]
        Staff = 0x02,
        /// <summary>
        /// 收件人
        /// </summary>
        [EnumMember]
        Client = 0x03,
        /// <summary>
        /// 供应商
        /// </summary>
        [EnumMember]
        Individual = 0x04,
    }

    [DataContract]
    public enum PayType
    {
        [EnumMember]
        AtTime = 0,
        [EnumMember]
        Before = 1,
        [EnumMember]
        After = 2
    }

    [DataContract]
    public class Entity:BaseData
    {
        public Entity() : base() { }

        public Entity(string Operation)
            : base(Operation)
        {
        }
        private int? entityId;
        [DataMember]
        public int? EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }
        private String alias;

        [DataMember]
        public String Alias
        {
            get { return alias; }
            set { alias = value; OnPropertyChanged("Alias"); }
        }
        private String password;

        [DataMember]
        public String Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); OnPropertyChanged("PasswordConfirm"); }
        }

        private string passwordConfirm;
        [DataMember]
        public string PasswordConfirm
        {
            get { return passwordConfirm; }
            set { passwordConfirm = value; OnPropertyChanged("Password"); OnPropertyChanged("PasswordConfirm"); }
        }

        private String fullName;
        [DataMember]
        public String FullName
        {
            get { return fullName; }
            set { fullName = value; OnPropertyChanged("FullName"); }
        }
        private String homePhone;
        [DataMember]
        public String HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; OnPropertyChanged("HomePhone"); }
        }
        private String cellPhoneOne;
        [DataMember]
        public String CellPhoneOne
        {
            get { return cellPhoneOne; }
            set { cellPhoneOne = value; }
        }
        private String cellPhoneTwo;
        [DataMember]
        public String CellPhoneTwo
        {
            get { return cellPhoneTwo; }
            set { cellPhoneTwo = value; }
        }
        private EntityType entityType;
        [DataMember]
        public EntityType EntityType
        {
            get { return entityType; }
            set { entityType = value; }
        }
        private String addressFamily;
        [DataMember]
        public String AddressFamily
        {
            get { return addressFamily; }
            set { addressFamily = value; }
        }
        private String addressChild;
        [DataMember]
        public String AddressChild
        {
            get { return addressChild; }
            set { addressChild = value; }
        }
        private String comment;
        [DataMember]
        public String Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged("Comment"); }
        }
        private int storeLog;
        [DataMember]
        public int StoreLog
        {
            get { return storeLog; }
            set { storeLog = value; }
        }
        private decimal deposit;
        [DataMember]
        public decimal Deposit
        {
            get { return deposit; }
            set { deposit = value; }
        }
        private PayType payType;
        [DataMember]
        public PayType PayType
        {
            get { return payType; }
            set { payType = value; }
        }
        private int rountStation;
        [DataMember]
        public int RountStation
        {
            get { return rountStation; }
            set { rountStation = value; }
        }
        private bool ableFlag;
        [DataMember]
        public bool AbleFlag
        {
            get { return ableFlag; }
            set { ableFlag = value; }
        }
        
        private List<Role> roles;
        [DataMember]
        public List<Role> Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "Alias":
                    if (String.IsNullOrEmpty(Alias)) return "用户名不能为空！";
                    return string.Empty;
                case "Password":
                    if (String.IsNullOrEmpty(Password)) return "密码不能为空！";
                    return string.Empty;
            }
            return null;
        }
    }


}
