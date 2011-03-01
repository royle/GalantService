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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using GLTWarter.Data;

namespace GLTWarter
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
        private static extern int DrawMenuBar(IntPtr hwnd);

        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint SC_CLOSE = 0xF060;
       
        Pages.Login pageLogin;
        public LoginScreen()
        {
            InitializeComponent();
            pageLogin = new Pages.Login(AppCurrent.Active.StaffCurrent);
            this.Loaded += new RoutedEventHandler(LoginScreen_Loaded);
        }

        void LoginScreen_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    frameContent.Navigate(pageLogin);
                    pageLogin.VerdictReceived += new System.Windows.Navigation.ReturnEventHandler<Galant.DataEntity.BaseData>(pageLogin_Return);
                }
            );

            this.Closing += new CancelEventHandler(Login_Closing);
        }

        void pageLogin_Return(object sender, System.Windows.Navigation.ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            if (e.Result!=null)
            {
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
        }

        void Login_Closing(object sender, CancelEventArgs e)
        {
           
        }

        bool IsWaiting
        {
            set
            {
                /*
                WindowInteropHelper helper = new WindowInteropHelper(this);
                IntPtr windowHandle = helper.Handle;
                IntPtr hmenu = GetSystemMenu(windowHandle, 0);
                if (value)
                {
                    EnableMenuItem(hmenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                }
                else
                {
                    EnableMenuItem(hmenu, SC_CLOSE, MF_BYCOMMAND );
                }
                 */
            }
        }
    }
}
