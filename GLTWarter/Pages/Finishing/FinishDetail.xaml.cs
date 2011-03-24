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

namespace GLTWarter.Pages.Entity.Customer
{
    /// <summary>
    /// Interaction logic for FinishDetail.xaml
    /// </summary>
    public partial class FinishDetail : DetailsBase
    {
        public FinishDetail(Galant.DataEntity.Entity data)
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
            this.dataCurrent.Operation = "Save";
            base.buttonOk_Click(sender, e);
        }
        
    }
}
