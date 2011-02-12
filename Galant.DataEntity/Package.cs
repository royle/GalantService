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
        }

        private int packageId;

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }
        private Product product;

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

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private decimal originAmount;

        public decimal OriginAmount
        {
            get { return originAmount; }
            set { originAmount = value; }
        }
        private PackageState packageType;

        public PackageState PackageType
        {
            get { return packageType; }
            set { packageType = value; }
        }
    }
}
