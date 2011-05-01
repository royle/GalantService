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

namespace GLTWarter.Pages.Finishing
{
    /// <summary>
    /// Interaction logic for CheckinBottle.xaml
    /// </summary>
    public partial class CheckinBottle : DetailsBase
    {
        public CheckinBottle(Galant.DataEntity.CheckinBottle data)
            : base(data)
        {
            InitializeComponent();
            if (data.ReturnPackages == null || data.ReturnPackages.Count <= 0)
            {
                data.ReturnPackages = new List<Galant.DataEntity.Package>();
                this.AddNewPackage(Galant.DataEntity.ProductEnum.Autonomy);
            }
        }

        public Galant.DataEntity.PaperSubState? originStatus;

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override void FocusFirstControl()
        {
            
        }

        protected void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.CheckinBottle paper = this.DataContext as Galant.DataEntity.CheckinBottle;
            if (this.CheckFinishValue(paper))
                OnReturn(null);
            else
                base.buttonNext_Click(sender, e);
        }

		private void EntitySelector_Enter(object sender, GLTWarter.Controls.EntitySelectorEnterEventArgs e)
        {
            if (this.entitySelector.SelectedEntity != null)
            {
                (this.DataContext as Galant.DataEntity.CheckinBottle).Holder = this.entitySelector.SelectedEntity;
            }
        }

        private void btnAddBulk_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.CheckinBottle paper = this.DataContext as Galant.DataEntity.CheckinBottle;
            if (paper.ReturnBulk !=null && paper.ReturnBulk.Where(p => p.Count == 0).Count() >= 1)
            {
                Utils.PlaySound(Resource.soundMismatch);
                return;
            }

            this.AddNewPackage(Galant.DataEntity.ProductEnum.Autonomy);
        }

        private void AddNewPackage(Galant.DataEntity.ProductEnum productType)
        {
            Galant.DataEntity.CheckinBottle paper = this.DataContext as Galant.DataEntity.CheckinBottle;
            switch (productType)
            {
                case Galant.DataEntity.ProductEnum.Autonomy:

                    Galant.DataEntity.Package pac = new Galant.DataEntity.Package("checkinBottle");
                    Galant.DataEntity.Product pr = AppCurrent.Active.AppCach.ProductsNeedReturn == null ?
                                                    new Galant.DataEntity.Product() { ProductType = Galant.DataEntity.ProductEnum.Autonomy, NeedBack = true, ReturnName = "" } :
                                                    AppCurrent.Active.AppCach.ProductsNeedReturn.FirstOrDefault();
                    pac.Product = pr;
                    paper.ReturnPackages.Add(pac);
                    break;
                case Galant.DataEntity.ProductEnum.Ticket:
                    Galant.DataEntity.Package pacTicket = new Galant.DataEntity.Package("checkinBottle");
                    Galant.DataEntity.Product prTicket = AppCurrent.Active.AppCach.ProductsTickets == null ?
                                                    new Galant.DataEntity.Product() { ProductType = Galant.DataEntity.ProductEnum.Ticket, NeedBack = false, ReturnName = "" } :
                                                    AppCurrent.Active.AppCach.ProductsTickets.FirstOrDefault();
                    pacTicket.Product = prTicket;
                    paper.ReturnPackages.Add(pacTicket);
                    break;
                default:
                    break;
            }
            paper.NotifyReturnListChanget();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            paper.PaperSubStatus = this.originStatus;
            paper.NotifyReturnListChanget();
            base.buttonCancel_Click(sender, e);
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgFinishCheckinComplete, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.Navigate(new Welcome());
        }

        private bool CheckFinishValue(Galant.DataEntity.CheckinBottle paper)
        {
            return false;
 
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                (sender as TextBox).SelectAll();
        }
    }
}
