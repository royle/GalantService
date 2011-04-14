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
        public PaperDetail()
        {
            InitializeComponent();
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
    }
}
