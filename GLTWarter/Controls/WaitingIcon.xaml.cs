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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GLTWarter.Controls
{
    /// <summary>
    /// Interaction logic for WaitingIcon.xaml
    /// </summary>
    public partial class WaitingIcon : UserControl
    {
        public WaitingIcon()
        {
            this.Loaded += new RoutedEventHandler(WaitingIcon_Loaded);
            InitializeComponent();
        }

        private void WaitingIcon_Loaded(object sender, RoutedEventArgs e)
        {
            if (
                this.Content is string && string.IsNullOrEmpty((string)this.Content) ||
                this.Content == null)
            {
                this.Content = this.FindResource("LoadingString").ToString();
            }
        }
    }
}