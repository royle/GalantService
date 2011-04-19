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
    /// Interaction logic for PaperDetail.xaml
    /// </summary>
    public partial class PaperDetail : DetailsBase
    {
        public PaperDetail(Galant.DataEntity.Paper data):base(data)
        {
            InitializeComponent();
            this.DoRefresh();
        }

        private void btnModifyAmount_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnPostDataInitialization()
        {
            Galant.DataEntity.Paper data = dataCurrent as Galant.DataEntity.Paper;

            Galant.DataEntity.PaperOperation.PaperForcedReturnRequest requestFR = new Galant.DataEntity.PaperOperation.PaperForcedReturnRequest(data);
            opboxForcedReturn.DataContext = requestFR;

            Galant.DataEntity.PaperOperation.PaperRevertFinishingRequest requestRF = new Galant.DataEntity.PaperOperation.PaperRevertFinishingRequest(data);
            opboxRevertFinishing.DataContext = requestRF;
           
        }

        private void DetailsBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.DoRefresh();
        }

        private void LinkForcedReturn_Click(object sender, RoutedEventArgs e)
        {
            DetailsBase page = new OpForcedReturn(opboxForcedReturn.DataContext as Galant.DataEntity.PaperOperation.PaperForcedReturnRequest);
            page.Return += new ReturnEventHandler<Galant.DataEntity.BaseData>(pageOp_Return);
            this.NavigationService.Navigate(page);
        }

        private void LinkRevertFinishing_Click(object sender, RoutedEventArgs e)
        {
            DetailsBase page = new OpRevertFinishing(opboxRevertFinishing.DataContext as Galant.DataEntity.PaperOperation.PaperRevertFinishingRequest);
            page.Return += new ReturnEventHandler<Galant.DataEntity.BaseData>(pageOp_Return);
            this.NavigationService.Navigate(page);
        }

    }
}
