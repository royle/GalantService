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

namespace GLTWarter.Pages.Product
{
    /// <summary>
    /// Interaction logic for ProductionManagement.xaml
    /// </summary>
    public partial class ProductionManagement : DetailsBase
    {
        public ProductionManagement(Galant.DataEntity.BaseData data):base(data)
        {
            InitializeComponent();
            data.Operation = BaseOperatorName.DataSave;
        }

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override bool OnSavedEditedItem()
        {
            return false;
        }

        protected override bool OnSavedNewItem()
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgProductCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
