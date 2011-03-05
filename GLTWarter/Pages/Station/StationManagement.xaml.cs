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

namespace GLTWarter.Pages.Station
{
    /// <summary>
    /// Interaction logic for StationManagement.xaml
    /// </summary>
    public partial class StationManagement : DetailsBase
    {
        public StationManagement(Galant.DataEntity.BaseData data)
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

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            Galant.DataEntity.BaseData data = new Galant.DataEntity.Entity() { EntityType = Galant.DataEntity.EntityType.Station };
            return data;
        }

        protected override bool OnSavedNewItem()
        {
            MessageBox.Show(App.Current.MainWindow, Resource.msgStationCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);

            this.dataCurrent = new Galant.DataEntity.Entity();
            this.DataContext = this.dataCurrent;
            Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)this.dataCurrent;

            data.EntityType = ((Galant.DataEntity.Entity)this.dataBackup).EntityType;
            this.dataBackup = (Galant.DataEntity.BaseData)data.Clone();

            return false;
        }
    }
}
