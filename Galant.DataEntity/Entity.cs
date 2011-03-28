using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

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
    public class Entity : BaseData
    {
        public Entity() : base() {
            this.Deposit = decimal.Zero;
            this.PayType = Galant.DataEntity.PayType.AtTime;
            this.AbleFlag = true;
        }

        public Entity(string Operation)
            : base(Operation)        {        }
        private int? entityId;
        [DataMember]
        public int? EntityId
        {
            get { return entityId; }
            set { entityId = value; OnPropertyChanged("IsPasswordAllowed"); }
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
            set { homePhone = value; OnPropertyChanged("HomePhone"); OnPropertyChanged("Phones"); }
        }
        private String cellPhoneOne;
        [DataMember]
        public String CellPhoneOne
        {
            get { return cellPhoneOne; }
            set { cellPhoneOne = value; OnPropertyChanged("CellPhoneOne"); OnPropertyChanged("Phones"); }
        }
        private String cellPhoneTwo;
        [DataMember]
        public String CellPhoneTwo
        {
            get { return cellPhoneTwo; }
            set { cellPhoneTwo = value; OnPropertyChanged("CellPhoneTwo"); OnPropertyChanged("Phones"); }
        }
        private EntityType entityType;
        [DataMember]
        public EntityType EntityType
        {
            get { return entityType; }
            set { entityType = value; OnPropertyChanged("EntityType"); OnPropertyChanged("IsPasswordAllowed"); }
        }

        private List<string> phones;
        public List<string> Phones
        {
            get
            {
                phones = new List<string>();
                if (!string.IsNullOrEmpty(this.HomePhone))
                    phones.Add(this.HomePhone);
                if (!string.IsNullOrEmpty(this.CellPhoneOne))
                    phones.Add(this.CellPhoneOne);
                if (!string.IsNullOrEmpty(this.CellPhoneTwo))
                    phones.Add(this.CellPhoneTwo);
                return phones;
            }
            set { phones = value; OnPropertyChanged("Phones"); }
        }

        private String addressFamily;
        [DataMember]
        public String AddressFamily
        {
            get { return addressFamily; }
            set { addressFamily = value; OnPropertyChanged("AddressFamily"); }
        }
        private String addressChild;
        [DataMember]
        public String AddressChild
        {
            get { return addressChild; }
            set { addressChild = value; OnPropertyChanged("AddressChild"); }
        }
        private String comment;
        [DataMember]
        public String Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged("Comment"); }
        }
        private int? storeLog;
        [DataMember]
        public int? StoreLog
        {
            get { return storeLog; }
            set { storeLog = value; OnPropertyChanged("StoreLog"); }
        }
        private decimal? deposit;
        [DataMember]
        public decimal? Deposit
        {
            get { return deposit; }
            set { deposit = value; OnPropertyChanged("Deposit"); }
        }
        private PayType payType;
        [DataMember]
        public PayType PayType
        {
            get { return payType; }
            set { payType = value; OnPropertyChanged("PayType"); }
        }
        private int? rountStation;
        [DataMember]
        public int? RountStation
        {
            get { return rountStation; }
            set { rountStation = value; OnPropertyChanged("RountStation"); }
        }
        private bool ableFlag;
        [DataMember]
        public bool AbleFlag
        {
            get { return ableFlag; }
            set { ableFlag = value; OnPropertyChanged("AbleFlag"); }
        }
        private int? curerentStationID;
        [DataMember]
        public int? CurerentStationID
        {
            get { return curerentStationID; }
            set { curerentStationID = value; OnPropertyChanged("CurerentStationID"); }
        }

        private ObservableCollection<Role> roles = new ObservableCollection<Role>();
        [DataMember]
        public ObservableCollection<Role> Roles
        {
            get {
                if (roles == null)
                    roles = new ObservableCollection<Role>();
                return roles;
                }
            set { roles = value; OnPropertyChanged("Roles"); }
        }

        public bool IsHavePhoneNo
        { get { return !(string.IsNullOrEmpty(this.HomePhone) && string.IsNullOrEmpty(this.CellPhoneOne) && string.IsNullOrEmpty(this.CellPhoneTwo)); } }

        public bool IsPasswordAllowed
        {
            get { return !this.EntityId.HasValue && this.EntityType == DataEntity.EntityType.Staff; }
        }

        public bool IsAliasEnable
        { get { return this.IsAliasAllowed && !this.EntityId.HasValue; } }

        public bool IsAliasAllowed
        {
            get
            {
                return (this.EntityType != DataEntity.EntityType.Client);
            }
        }


        
        public override string QueryId
        {
            get
            {
                return this.EntityId.HasValue ? this.EntityId.ToString() : null;
            }
            set
            {
                this.EntityId = value == null ? (int?)null : (int?)Int32.Parse(value);
            }
        }

        public  string IsValidPassword(string password, string passwordConfirm)
        {
            if (string.IsNullOrEmpty(password)) return "必须输入密码";
            if (password.Length < 4) return "密码最短为4个字符";
            if (password != passwordConfirm && passwordConfirm != null) return "两次输入的密码不同。";
            return string.Empty;
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "Alias":
                    if (String.IsNullOrEmpty(Alias) && this.IsNew && this.EntityType != DataEntity.EntityType.Client) return "用户名不能为空！";
                    return string.Empty;
                case "PasswordConfirm":
                    if (String.IsNullOrEmpty(Password) && (this.Operation == "Login" || (this.EntityType == DataEntity.EntityType.Staff && this.IsNew))) return "密码不能为空！";
                    return string.Empty;
                case "Password":
                    if (String.IsNullOrEmpty(Password) && (this.Operation == "Login" || this.EntityType == DataEntity.EntityType.Staff)) return "密码不能为空！";
                    return string.Empty;
                case "PayType":
                    if (this.IsNew && this.EntityType == DataEntity.EntityType.Client && this.PayType==null)
                        return "客户付款类型不能为空！";
                    return string.Empty;
            }
            return null;
        }
    }


}
