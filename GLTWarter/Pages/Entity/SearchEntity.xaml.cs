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

namespace GLTWarter.Pages.Entity
{
    /// <summary>
    /// Interaction logic for SearchEntity.xaml
    /// </summary>
    public partial class SearchEntity : DetailsBase
    {
        public SearchEntity(Galant.DataEntity.Result.SearchEntityResult data):base(data)
        {
            InitializeComponent();
        }

        void HandleItemActivate(object source, RoutedEventArgs e)
        {
            if (e is KeyEventArgs)
            {
                KeyEventArgs ke = (KeyEventArgs)e;
                if (!(ke.Key == Key.Enter && ke.KeyboardDevice.Modifiers == ModifierKeys.None))
                {
                    return;
                }
            }
            Galant.DataEntity.Entity data = listResult.GetItemFromContainer((System.Windows.DependencyObject)source) as Galant.DataEntity.Entity;
            if (data != null)
            {
                switch (data.EntityType)
                {
                    case Galant.DataEntity.EntityType.Headquarter:
                        
                        break;
                    case Galant.DataEntity.EntityType.Station:
                        break;
                    case Galant.DataEntity.EntityType.Staff:
                        data.Operation = "Save";
                        this.NavigationService.Navigate(new GLTWarter.Pages.Entity.Users.UserDetail(data));
                        break;
                    case Galant.DataEntity.EntityType.Client:
                        data.Operation = "Save";
                        this.NavigationService.Navigate(new GLTWarter.Pages.Entity.Customer.CustomerDetail(data));
                        break;
                    case Galant.DataEntity.EntityType.Individual:
                        break;
                    default:
                        break;
                }
            }
        }

        protected override bool DataRefreshSuppressed
        {
            get { return true; }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            this.DataContext = incomingData;
            this.dataCurrent = incomingData;
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.EntityType? type = (this.DataContext as Galant.DataEntity.Result.SearchEntityResult).SearchCondition.Type;
            switch (type)
            {
                case Galant.DataEntity.EntityType.Headquarter:

                    break;
                case Galant.DataEntity.EntityType.Station:
                    break;
                case Galant.DataEntity.EntityType.Staff:
                    Galant.DataEntity.Entity data = new Galant.DataEntity.Entity();
                    data.EntityType = Galant.DataEntity.EntityType.Staff;
                    data.Operation = "Save";
                    data.AbleFlag = true;
                    this.NavigationService.Navigate(new GLTWarter.Pages.Entity.Users.UserDetail(data));
                    break;
                case Galant.DataEntity.EntityType.Client:
                    Galant.DataEntity.Entity client = new Galant.DataEntity.Entity();
                    client.EntityType = Galant.DataEntity.EntityType.Client;
                    client.Operation = "Save";
                    client.AbleFlag = true;
                    this.NavigationService.Navigate(new GLTWarter.Pages.Entity.Customer.CustomerDetail(client));
                    break;
                case Galant.DataEntity.EntityType.Individual:
                    break;
                default:
                    break;
            }
        }
       
    }

  
}
