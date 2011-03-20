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
using System.IO;
using System.IO.Packaging;
using System.Collections.ObjectModel;

namespace GLTWarter.Pages
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : GLTWarter.Views.ViewBase
    {
        public Welcome()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Welcome_Loaded);
           
        }

        void Welcome_Loaded(object sender, RoutedEventArgs e)
        {
            FocusSelectedTabItem();
        }

        protected override void FocusFirstControl()
        {
            FocusSelectedTabItem();
        }

        void EffectiveRole_SelectedEntityChanged(object sender, EventArgs e)//将焦点指定在已显示的TabItem上
        {
            this.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.DataBind,
                (Action)delegate()
                {                    
                    FocusSelectedTabItem();
                }
            );
        }

        void FocusSelectedTabItem()
        { 
            //if (this.tabControlMain.SelectedItem == null)
            //{
            //    FocusFirstUsableTab();
            //}
            //else if (((TabItem)this.tabControlMain.SelectedItem).Visibility != Visibility.Visible)
            //{
            //    FocusFirstUsableTab();
            //}
        }

        void FocusFirstUsableTab()
        {
            //foreach (TabItem ti in this.tabControlMain.Items)
            //{
            //    if (ti.Visibility == Visibility.Visible)
            //    {
            //        if (ti.Focusable)
            //        {
            //            this.tabControlMain.SelectedItem = ti;
            //            ti.Focus();
            //            break;
            //        }
            //    }
            //}
        }

        private void textQuickShipmentQuery_KeyDown(object sender, KeyEventArgs e)
        {
            //KeyEventArgs ke = (KeyEventArgs)e;
            //if (!(ke.Key == Key.Enter && ke.KeyboardDevice.Modifiers == ModifierKeys.None))
            //{
            //    return;
            //}
            //e.Handled = true;

            //Shipment.Entity.SearchDataById data = new Shipment.Entity.SearchDataById();
            //data.ShipmentId = ((TextBox)sender).Text;
            //((TextBox)sender).Text = string.Empty;

            //this.NavigationService.Navigate(
            //    new Pages.Search(new Shipment.Entity.SearchById(data), new Shipment.Entity.Result(), new Shipment.Entity.ActionSingleGo())
            //);
        }

        public override bool IsRefreshable
        {
            get
            {
                return true;
            }
        }

        public override void OnRefresh()
        {
        }

        

       
       

        private void linkStationSimpleCheckout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("待开发");
        }

        private void linkWarterOrder_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = new Galant.DataEntity.Paper();
            paper.ContactB = new Galant.DataEntity.Entity();
            paper.Packages = new ObservableCollection<Galant.DataEntity.Package>();
            paper.Operation = BaseOperatorName.DataSave;
            this.NavigationService.Navigate(new GLTWarter.Pages.Order.CustomerServiceBookingOrder(paper));

        }

        private void linkDeliverOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManageStation_Click(object sender, RoutedEventArgs e)
        {
            //站点管理
            this.NavigationService.Navigate(new Entity.Station.StationManagement(null));

        }

        private void ManageUser_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Result.SearchEntityResult data=new Galant.DataEntity.Result.SearchEntityResult();
            data.SearchCondition.Type = Galant.DataEntity.EntityType.Staff;
            data.Operation = "SearchEntity";
            this.NavigationService.Navigate(new GLTWarter.Pages.Entity.SearchEntity(data));
        }

        private void ManageProduct_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Production.Result data = new Galant.DataEntity.Production.Result();
            data.SearchCondition.Type = Galant.DataEntity.ProductEnum.Autonomy;
            data.Operation = BaseOperatorName.ProductSearch;
            this.NavigationService.Navigate(new GLTWarter.Pages.Product.ProductionSearch(data));
        }

        private void WarterFinishing_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Result.FinishingListResult data = new Galant.DataEntity.Result.FinishingListResult();
            data.SearchCondition = new Galant.DataEntity.Search.FinishListCondition();
            data.SearchCondition.Station = AppCurrent.Active.AppCach.StationCurrent;
            data.Operation = "SearchFinishingList";
            this.NavigationService.Navigate(new GLTWarter.Pages.Finishing.FinishingList(data));
        }
    }
}