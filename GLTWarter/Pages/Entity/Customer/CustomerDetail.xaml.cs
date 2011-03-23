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
    /// Interaction logic for CustomerDetail.xaml
    /// </summary>
    public partial class CustomerDetail : DetailsBase
    {
        public CustomerDetail(Galant.DataEntity.Entity data)
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
            if (this.textAlias.Visibility == System.Windows.Visibility.Visible && this.textAlias.IsEnabled)
                this.comboPayType.Focus();
            else
                this.textName.Focus();
        }

        protected void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.dataCurrent.Operation = "Save";
            base.buttonOk_Click(sender, e);
        }

        protected override bool OnSavedNewItem()
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgCustomerCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.dataCurrent = new Galant.DataEntity.Entity();
            this.dataCurrent.Operation = "Save";
            (this.dataCurrent as Galant.DataEntity.Entity).EntityType = Galant.DataEntity.EntityType.Client;
            (this.dataCurrent as Galant.DataEntity.Entity).AbleFlag = true;
            (this.dataCurrent as Galant.DataEntity.Entity).PayType = Galant.DataEntity.PayType.AtTime;
            Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
            this.DataContext = this.dataCurrent;
            this.dataBackup = (Galant.DataEntity.BaseData)data.Clone();
            return false;
        }

        protected override bool OnSavedEditedItem()
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgCustomerModified, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.dataCurrent.Operation = "Save";
            return true;
        }
    }
}
