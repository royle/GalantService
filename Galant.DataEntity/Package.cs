using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum PackageState
    {
        /// <summary>
        /// 完成
        /// </summary>
        [EnumMember]
        Finish=0,
        /// <summary>
        /// 新建
        /// </summary>
        [EnumMember]
        New =1,        
        /// <summary>
        /// 取消
        /// </summary>
        [EnumMember]
        Cancle = 9,        
    }

    [DataContract]
    public class Package:BaseData
    {
        public Package(string paperid):base()
        {
            this.paperId = paperid;
        }

        private string paperId;

        public string PaperId
        {
            get { return paperId; }
            set { paperId = value; }
        }

        private int packageId;
        [DataMember]  
        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }
        private Product product;
        [DataMember]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        public int ProductId
        {
            get { return Product.ProductId; }
        }

        private int count;
        [DataMember]
        public int Count
        {
            get { return count; }
            set { count = value; this.Amount = this.Product == null ? 0 : this.Product.Amount * value; }
        }
        private decimal amount;
        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private decimal originAmount;
        [DataMember]
        public decimal OriginAmount
        {
            get { return originAmount; }
            set { originAmount = value; }
        }
        private PackageState packageType;
        [DataMember]
        public PackageState PackageType
        {
            get { return packageType; }
            set { packageType = value; }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "Count":
                    if (Count==null || Count <= 0) return "数量不能小于0！";
                    return string.Empty;              
            }
            return null;
        }
    }
}
