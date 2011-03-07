using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GLTWarter.Pages.Product
{
    /// <summary>
    /// Interaction logic for ProductionSearch.xaml
    /// </summary>
    public partial class ProductionSearch : DetailsBase
    {
        public ProductionSearch(Galant.DataEntity.BaseData data):base(data)
        {
            if (data != null)
                data.Operation = "";
            InitializeComponent();
        }
    }
}
