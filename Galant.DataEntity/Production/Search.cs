using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity.Production
{
    public class Search:BaseData
    {
        public Search() : base("SearchProduction") { }

        private ProductEnum? type;
        public ProductEnum? Type
        {
            get { return this.type; }
            set { type = value; OnPropertyChanged("Type"); }
        }

        private String alias;
        public String Alias
        {
            get { return alias; }
            set { alias = value; OnPropertyChanged("Alias"); }
        }

        private String productName;
        public String ProductName
        {
            get { return productName; }
            set { productName = value; OnPropertyChanged("ProductName"); }
        }
    }
}
