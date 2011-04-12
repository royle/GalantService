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
    /// Interaction logic for FinishDetail.xaml
    /// </summary>
    public partial class FinishDetail : DetailsBase
    {
        public FinishDetail(Galant.DataEntity.Paper data)
            : base(data)
        {
            InitializeComponent();
            originStatus = data.PaperSubStatus;
            if (data.ReturnPackages == null || data.ReturnPackages.Count <= 0)
            {
                data.ReturnPackages = new List<Galant.DataEntity.Package>();
                this.AddNewPackage(Galant.DataEntity.ProductEnum.Autonomy);
                this.AddNewPackage(Galant.DataEntity.ProductEnum.Ticket);
            }
            if (data.PaperSubStatus == Galant.DataEntity.PaperSubState.InTransit)
                data.PaperSubStatus = Galant.DataEntity.PaperSubState.NextActionAssured;
        }

        public Galant.DataEntity.PaperSubState? originStatus;

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            this.DataContext = incomingData;
            this.dataCurrent = incomingData;
        }

        protected override void FocusFirstControl()
        {
            
        }

        protected void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            if (this.CheckFinishValue(paper))
                OnReturn(null);
        }

        private void FinishResult_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddBulk_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            if (paper.ReturnBulk !=null && paper.ReturnBulk.Where(p => p.Count == 0).Count() >= 1)
            {
                Utils.PlaySound(Resource.soundMismatch);
                return;
            }

            this.AddNewPackage(Galant.DataEntity.ProductEnum.Autonomy);
        }

        private void AddNewPackage(Galant.DataEntity.ProductEnum productType)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            switch (productType)
            {
                case Galant.DataEntity.ProductEnum.Autonomy:

                    Galant.DataEntity.Package pac = new Galant.DataEntity.Package(paper.PaperId);
                    Galant.DataEntity.Product pr = AppCurrent.Active.AppCach.ProductsNeedReturn == null ?
                                                    new Galant.DataEntity.Product() { ProductType = Galant.DataEntity.ProductEnum.Autonomy, NeedBack = true, ReturnName = "" } :
                                                    AppCurrent.Active.AppCach.ProductsNeedReturn.FirstOrDefault();
                    pac.Product = pr;
                    paper.ReturnPackages.Add(pac);
                    break;
                case Galant.DataEntity.ProductEnum.Ticket:
                    Galant.DataEntity.Package pacTicket = new Galant.DataEntity.Package(paper.PaperId);
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

        private void btnAddTicket_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            if (paper.ReturnTicket.Where(p => p.Count == 0).Count() >= 1)
            {
                Utils.PlaySound(Resource.soundMismatch);
                return;
            }

            this.AddNewPackage(Galant.DataEntity.ProductEnum.Ticket);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Paper paper = this.DataContext as Galant.DataEntity.Paper;
            paper.PaperSubStatus = this.originStatus;
            paper.NotifyReturnListChanget();
            base.buttonCancel_Click(sender, e);
        }

        private bool CheckFinishValue(Galant.DataEntity.Paper paper)
        {
            if (paper.ContactB != null && paper.ContactB.PayType == Galant.DataEntity.PayType.AtTime)
            {
                if (paper.PaperAmount > paper.ReturnedAmount)
                {
                    if (MessageBox.Show(AppCurrent.Active.MainWindow,
                    string.Format(Resource.validationPaperReturnValueError, paper.PaperAmount.ToString(), paper.ReturnedAmount.ToString()),
                    this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
                        return false;
                }
            }
            return true;
 
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                (sender as TextBox).SelectAll();
        }
    }
}
