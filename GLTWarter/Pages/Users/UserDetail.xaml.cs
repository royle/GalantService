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

namespace GLTWarter.Pages.Users
{
    /// <summary>
    /// Interaction logic for UserDetail.xaml
    /// </summary>
    public partial class UserDetail : DetailsBase
    {
        public UserDetail()
        {
            InitializeComponent();
        }


        private void ButtonAddRole_Click(object sender, RoutedEventArgs e)
        {
            //Data.Role r = new Data.Role();
            //r.IsEditable = true;

            //Data.Entity data = (Data.Entity)dataCurrent;
            //data.Roles.Add(r);
        }

        private void ButtonRemoveRole_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement s = (FrameworkElement)sender;
            //Data.Role r = (Data.Role)s.DataContext;

            //Data.Entity data = (Data.Entity)dataCurrent;
            //data.Roles.Remove(r);
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
            //if (((ContentControl)source).Content is Data.EventLog)
            //{
            //    objectCursor = (ContentControl)source;
            //    Pages.Shipment.Events.Details page = new Pages.Shipment.Events.Details((Data.BusinessEntity)(objectCursor.Content));
            //    page.Return += PageFunction_Returned;
            //    this.NavigationService.Navigate(page);
            //}
        }
    }
}
