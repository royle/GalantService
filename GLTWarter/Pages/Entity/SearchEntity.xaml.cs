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
        public SearchEntity():base(new Galant.DataEntity.Result.SearchEntityResult())
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
                //App.Active.MainScreen.NavigateEntityDetails(data);
            }
        }
    }

  
}
