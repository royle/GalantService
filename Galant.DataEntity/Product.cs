﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    [DataContract]
    public enum ProductEnum:int
    { 
        /// <summary>
        /// 水票
        /// </summary>
        [EnumMember]
        Ticket = 0x01,
        /// <summary>
        /// 水产品
        /// </summary>
        [EnumMember]
        Autonomy = 0x02,
        /// <summary>
        /// 快递
        /// </summary>
        [EnumMember]
        Delivery = 0x03
    }

    [DataContract]
    public class Product:BaseData
    {
        public Product()
        {
            AbleFlag = true;
        }

        private int productId;
        [DataMember]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        private String productName;
        [DataMember]
        public String ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        private String alias;
        [DataMember]
        public String Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        private decimal amount;
        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        private ProductEnum productType;
        [DataMember]
        public ProductEnum ProductType
        {
            get { return productType; }
            set { productType = value; }
        }
        private String discretion;
        [DataMember]
        public String Discretion
        {
            get { return discretion; }
            set { discretion = value; }
        }

        private bool needBack;
        [DataMember]
        public bool NeedBack
        {
            get { return needBack; }
            set { needBack = value; }
        }
        private String returnName;
        [DataMember]
        public String ReturnName
        {
            get { return returnName; }
            set { returnName = value; }
        }
        private decimal returnValue;
        [DataMember]
        public decimal ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }
        private bool ableFlag;
        [DataMember]
        public bool AbleFlag
        {
            get { return ableFlag; }
            set { ableFlag = value; }
        }

    }
}
