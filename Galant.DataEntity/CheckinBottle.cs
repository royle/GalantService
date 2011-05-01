using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galant.DataEntity
{
    public class CheckinBottle : BaseData
    {

        private Entity holder;
        [DataMember]
        public Entity Holder
        {
            get { return holder; }
            set { holder = value; OnPropertyChanged("Holder"); }
        }


        private List<Package> returnPackages = new List<Package>();
        /// <summary>
        /// 归班返回的物品
        /// </summary>
        [DataMember]
        public List<Package> ReturnPackages
        {
            get { return returnPackages; }
            set { returnPackages = value; OnPropertyChanged("ReturnPackages"); OnPropertyChanged("ReturnBulk"); }
        }

        public void NotifyReturnListChanget()
        {
            OnPropertyChanged("ReturnPackages"); OnPropertyChanged("ReturnBulk");;
        }


        /// <summary>
        /// 归班返回的空桶
        /// </summary>
        [IgnoreDataMember]
        public List<Package> ReturnBulk
        {
            get { return ReturnPackages.Where(p => p.Product != null && p.Product.ProductType == ProductEnum.Autonomy && p.Product.NeedBack).ToList(); }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "Note":
                    if (Holder==null || string.IsNullOrEmpty(Holder.Alias)) return "请选择配送员。";
                    return string.Empty;
                case "ReturnBulk":
                    if (ReturnBulk.FirstOrDefault()==null) return "未添加任何空桶";
                    return string.Empty;
            }
            return null;
        }
    }
}
