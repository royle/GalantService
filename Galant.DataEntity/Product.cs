using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum ProductEnum
    { }
    [DataContract]
    public class Product:BaseData
    {
        private int productId;

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        private String productName;

        public String ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        private String alias;

        public String Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        private decimal amount;

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private ProductEnum productType;

        public ProductEnum ProductType
        {
            get { return productType; }
            set { productType = value; }
        }
        private String discretion;

        public String Discretion
        {
            get { return discretion; }
            set { discretion = value; }
        }

        private bool needBack;

        public bool NeedBack
        {
            get { return needBack; }
            set { needBack = value; }
        }
        private String returnName;

        public String ReturnName
        {
            get { return returnName; }
            set { returnName = value; }
        }
        private decimal returnValue;

        public decimal ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }
        private bool ableFlag;

        public bool AbleFlag
        {
            get { return ableFlag; }
            set { ableFlag = value; }
        }

    }
}
