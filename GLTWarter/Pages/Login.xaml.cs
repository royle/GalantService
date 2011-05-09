using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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

namespace GLTWarter.Pages
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Login : DetailsBase
    {

        public Login(Galant.DataEntity.BaseData data)
            : base(data)
        {
            InitializeComponent();
            data.Operation = "Login";
            this.Unloaded += new RoutedEventHandler(Login_Unloaded);
        }

        void Login_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void FocusFirstControl()
        {
            this.textAlias.Text = "admin";
            this.passwordPassword.Password = "admin!@#$";
            this.textAlias.Focus();

        }

        private void passwordPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordPopulation();
        }

        protected override void PreCommit()
        {
            if (dataCurrent != null)
            {
                Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
                if (data.IsValid)
                {
                    PasswordPopulation();
                }
            }
        }

        private void PasswordPopulation()
        {
            if (dataCurrent != null)
            {
                Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
                data.Password = passwordPassword.Password; 
            }
        }

        protected override bool DataRefreshSuppressed
        {
            get
            {
                    return true;
            }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            AppCurrent.Active.AppCach = incomingData as Galant.DataEntity.AppStatusCach;
            AppCurrent.Active.StaffCurrent = AppCurrent.Active.AppCach.StaffCurrent;
            if(AppCurrent.Active.AppCach.StaffCurrent.Roles!=null && AppCurrent.Active.AppCach.StaffCurrent.Roles.Count>0)
            {
                AppCurrent.Active.AppCach.StationCurrent = AppCurrent.Active.AppCach.StaffCurrent.Roles[0].Station;
            }
            OnVerdictReceived(incomingData);
        }

        protected void OnVerdictReceived(Galant.DataEntity.BaseData incomingData)
        {
            System.Windows.Navigation.ReturnEventHandler<Galant.DataEntity.BaseData> handler = VerdictReceived;
            if (handler != null)
            {
                handler(this, new System.Windows.Navigation.ReturnEventArgs<Galant.DataEntity.BaseData>(incomingData));
            }
        }

        private void buttonCancelPrivate_Click(object sender, RoutedEventArgs e)
        {
            OnVerdictReceived(null);
        }

        private void DetailsBase_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {

        }
        public event System.Windows.Navigation.ReturnEventHandler<Galant.DataEntity.BaseData> VerdictReceived;

    }
}
