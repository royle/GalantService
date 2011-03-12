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
        public ProductionSearch(Galant.DataEntity.Production.Result data):base(data)
        {
            InitializeComponent();
        }

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            return new Galant.DataEntity.Production.Result();
        }

        protected override bool DataRefreshSuppressed
        {
            get
            {
                return true;
            }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            incomingData.Operation = BaseOperatorName.ProductSearch;
            this.DataContext = this.dataCurrent = incomingData;
        }

        void HandleItemActivate(object source, RoutedEventArgs e)
        {
            if (e is KeyEventArgs)
            {
                KeyEventArgs ke = (KeyEventArgs)e;
                if (!(ke.Key == Key.Enter && ke.KeyboardDevice.Modifiers == ModifierKeys.None))
                {
                    return;
                }
            }
            Galant.DataEntity.Product data = listResult.GetItemFromContainer((System.Windows.DependencyObject)source) as Galant.DataEntity.Product;
            if (data != null)
            {
                data.Operation = "Save";
            }
            this.NavigationService.Navigate(new ProductionManagement(data));
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            DetailsBase page = new ProductionManagement(new Galant.DataEntity.Product() { ProductType = Galant.DataEntity.ProductEnum.Autonomy});
            this.NavigationService.Navigate(page);
        }

    }
}
