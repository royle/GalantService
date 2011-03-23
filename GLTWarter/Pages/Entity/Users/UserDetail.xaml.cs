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

namespace GLTWarter.Pages.Entity.Users
{
    /// <summary>
    /// Interaction logic for UserDetail.xaml
    /// </summary>
    public partial class UserDetail : DetailsBase
    {
        public UserDetail(Galant.DataEntity.Entity data):base(data)
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

        protected override void FocusFirstControl()
        {
            if (this.textAlias.Visibility == System.Windows.Visibility.Visible && this.textAlias.IsEnabled)
                this.textAlias.Focus();
            else
                this.textName.Focus();
        }

        protected  void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            this.dataCurrent.Operation = "Save";
            base.buttonOk_Click(sender, e);
        }

        private void ButtonAddRole_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Role r = new Galant.DataEntity.Role();

            Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
            data.Roles.Add(r);
        }

        private void ButtonRemoveRole_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement s = (FrameworkElement)sender;
            Galant.DataEntity.Role r = (Galant.DataEntity.Role)s.DataContext;

            Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
            data.Roles.Remove(r);
        }

        protected override bool OnSavedNewItem()
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgUserCreated, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);            
            this.dataCurrent = new Galant.DataEntity.Entity();
            this.dataCurrent.Operation = "Save";
            (this.dataCurrent as Galant.DataEntity.Entity).EntityType = Galant.DataEntity.EntityType.Staff;
            (this.dataCurrent as Galant.DataEntity.Entity).AbleFlag = true;
            Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
            this.DataContext = this.dataCurrent;
            this.dataBackup = (Galant.DataEntity.BaseData)data.Clone();
            passwordPassword.Clear();
            passwordConfirm.Clear();
            return false;
        }

        protected override bool OnSavedEditedItem()
        {
            MessageBox.Show(AppCurrent.Active.MainScreen, Resource.msgUserModified, this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.dataCurrent.Operation = "Save";
            return true;
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
        }
        private void passwordPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordPopulation();
        }

        protected override void PreCommit()
        {
            PasswordPopulation();
        }

        private void PasswordPopulation()
        {
            if (dataCurrent != null && (dataCurrent as Galant.DataEntity.Entity).IsPasswordAllowed)
            {
                Galant.DataEntity.Entity data = (Galant.DataEntity.Entity)dataCurrent;
                data.Password = passwordPassword.Password;
                data.PasswordConfirm = passwordConfirm.Password;
            }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            this.DataContext = incomingData;
            this.dataCurrent = incomingData;
        }

        private void DetailsBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dataCurrent.QueryId))
            {
                this.DoRefresh();
            }
        }
    }
}
