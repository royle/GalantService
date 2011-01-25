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

namespace WCFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs ev)
        {
            ServiceAPI.ServiceAPIClient client = new ServiceAPI.ServiceAPIClient();
            ServiceAPI.DetailBase de = new ServiceAPI.DetailBase();
            de.UserName = "";
            de.PassWord = "";
            Galant.DataEntity.Entity e = new Galant.DataEntity.Entity();
            Galant.DataEntity.Role r = new Galant.DataEntity.Role();
            r.RoleId = 2;
            e.Roles = new List<Galant.DataEntity.Role>();
            e.Roles.Add(r);
            de.Data = new Galant.DataEntity.BaseData[1];
            de.Data[0] = e;

           ServiceAPI.DetailBase d1= client.DoRequest(de, "Login");
           string ss = "";
        }
    }
}
