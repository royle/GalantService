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
using System.Collections.ObjectModel;

namespace GLTWarter.Pages.Order
{
    /// <summary>
    /// Interaction logic for CustomerServiceBookingOrder.xaml
    /// </summary>
    public partial class CustomerServiceBookingOrder : DetailsBase
    {
        public CustomerServiceBookingOrder(Galant.DataEntity.Paper paper)
            : base(paper)
        {
            InitializeComponent();
            packet.PaperId = paper.PaperId;
            packet.PackageType = Galant.DataEntity.PackageState.New;
            packet.Count = 1;
            this.packetageSelecter.DataContext = packet;
        }

        private Galant.DataEntity.Package packet = new Galant.DataEntity.Package(string.Empty);

        private void btnBookPaper_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper data = (this.DataContext as Galant.DataEntity.Paper);
            data.ContactA = new Galant.DataEntity.Entity();
            data.ContactA.EntityId = 0;
            data.ContactC = new Galant.DataEntity.Entity();
            data.ContactC.EntityId = 0;
            data.Holder = new Galant.DataEntity.Entity();
            data.Holder.EntityId = 0;
            data.PaperStatus = Galant.DataEntity.PaperStatus.Adviced;
            data.PaperSubStatus = Galant.DataEntity.PaperSubState.AdviceInStation;
            if (data.ContactB != null && (this.DataContext as Galant.DataEntity.Paper).ContactB.RountStation != null)
            {
                data.PaperSubStatus = Galant.DataEntity.PaperSubState.Routeing;
            }
            this.buttonNext_Click(sender, e);
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgOrderCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.Navigate(new Welcome());
        }

        private void EntitySelector_Enter(object sender, GLTWarter.Controls.EntitySelectorEnterEventArgs e)
        {
            if (this.entitySelector.SelectedEntity != null)
            {
                (this.DataContext as Galant.DataEntity.Paper).ContactB = this.entitySelector.SelectedEntity;
            }
        }

        private void ProductSelector_Enter(object sender, GLTWarter.Controls.ProductSelectorEnterEventArgs e)
        {
            try
            {
                this.packet.Product = productSelector.SelectedProduct;
                this.packet.Amount = this.packet.Product.Amount * this.packet.Count;
            }
            catch
            { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (this.productSelector.SelectedProduct == null || string.IsNullOrEmpty(this.txtCount.Text))
            {
                Utils.PlaySound(Resource.soundMismatch);
                return;
            }
            if (this.packet.Product != null)
            {
                this.packet.Amount = this.packet.Product.Amount * this.packet.Count;
                (this.DataContext as Galant.DataEntity.Paper).Packages.Add(this.packet.Clone() as Galant.DataEntity.Package);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Package data = listResult.GetItemFromContainer((System.Windows.DependencyObject)sender) as Galant.DataEntity.Package;
            (this.DataContext as Galant.DataEntity.Paper).Packages.Remove(data);
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as Galant.DataEntity.Paper).ContactB = new Galant.DataEntity.Entity();
            (this.DataContext as Galant.DataEntity.Paper).ContactB.EntityType = Galant.DataEntity.EntityType.Client;
        }
    }
}
