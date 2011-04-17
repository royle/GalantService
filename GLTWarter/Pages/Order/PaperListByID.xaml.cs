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

namespace GLTWarter.Pages.Order
{
    /// <summary>
    /// Interaction logic for PaperListByID.xaml
    /// </summary>
    public partial class PaperListByID : DetailsBase
    {
        public PaperListByID(Galant.DataEntity.Result.ResultPapersByID data):base(data)
        {
            InitializeComponent();
            this.DoNext();
        }

        private void btnModifyAmount_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            base.OnNext(incomingData);
            Galant.DataEntity.Result.ResultPapersByID result = incomingData as Galant.DataEntity.Result.ResultPapersByID;
            if (result != null && result.Papers != null && result.Papers.Count == 1)
            {
                this.NavigationService.Navigate(new Pages.Order.PaperDetail(result.Papers[0]));
            }
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
            Galant.DataEntity.Paper data = listResult.GetItemFromContainer((System.Windows.DependencyObject)source) as Galant.DataEntity.Paper;
            this.NavigationService.Navigate(new Pages.Order.PaperDetail(data));
        }
    }
}
