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
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private decimal Amount;

        public decimal Amount1
        {
            get { return Amount; }
            set { Amount = value; }
        }
        private decimal OriginAmount;

        public decimal OriginAmount1
        {
            get { return OriginAmount; }
            set { OriginAmount = value; }
        }
        private PackageState packageType;

        public PackageState PackageType
        {
            get { return packageType; }
            set { packageType = value; }
        }
    }
}
