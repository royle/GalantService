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
        }

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

            Galant.DataEntity.Package pac = new Galant.DataEntity.Package(paper.PaperId);
            Galant.DataEntity.Product pr = new Galant.DataEntity.Product() { ProductType= Galant.DataEntity.ProductEnum.Autonomy, NeedBack=true,ReturnName=""};
            pac.Product = pr;
            paper.ReturnPackages.Add(pac);

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

            Galant.DataEntity.Package pac = new Galant.DataEntity.Package(paper.PaperId);
            Galant.DataEntity.Product pr = new Galant.DataEntity.Product() { ProductType = Galant.DataEntity.ProductEnum.Ticket, NeedBack = false, ReturnName = "" };
            pac.Product = pr;
            paper.ReturnPackages.Add(pac);
            paper.NotifyReturnListChanget();
        }


        
    }
}
