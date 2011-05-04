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

namespace GLTWarter.Pages.Entity.Station
{
    /// <summary>
    /// Interaction logic for StationManagement.xaml
    /// </summary>
    public partial class StationManagement : DetailsBase
    {
        public StationManagement(Galant.DataEntity.Entity data)
            : base(data)
        {
            InitializeComponent();
        }

        protected override Button ButtonNext
        {
            get
            {
                return this.btnNext;
            }
        }

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override bool ExceptionCallback(Exception ex, string message)
        {
            return base.ExceptionCallback(ex, message);
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            MessageBox.Show(App.Current.MainWindow, Resource.msgStationCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.Navigate(new Welcome());
            base.OnNext(incomingData);
        }

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            Galant.DataEntity.BaseData data = new Galant.DataEntity.Entity() { EntityType = Galant.DataEntity.EntityType.Station , Password=BaseOperatorName.NullPassword};
            data.Operation = BaseOperatorName.DataSave;
            return data;
        }
    }
}
