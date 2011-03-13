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

namespace GLTWarter.Pages.Order
{
    /// <summary>
    /// Interaction logic for CustomerServiceBookingOrder.xaml
    /// </summary>
    public partial class CustomerServiceBookingOrder : DetailsBase
    {
        public CustomerServiceBookingOrder(Galant.DataEntity.Paper paper):base(paper)
        {
            InitializeComponent();
        }

        private void btnBookPaper_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EntitySelector_Enter(object sender, GLTWarter.Controls.EntitySelectorEnterEventArgs e)
        {
            if (this.entitySelector.SelectedEntity != null)
            {
                (this.DataContext as Galant.DataEntity.Paper).ContactA = this.entitySelector.SelectedEntity;
            }
        }

        private void ProductSelector_Enter(object sender, GLTWarter.Controls.ProductSelectorEnterEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
