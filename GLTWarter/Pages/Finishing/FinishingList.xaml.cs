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

namespace GLTWarter.Pages.Finishing
{
    /// <summary>
    /// Interaction logic for FinishingList.xaml
    /// </summary>
    public partial class FinishingList : DetailsBase
    {
        public FinishingList(Galant.DataEntity.Result.FinishingListResult data):base(data)
        {
            InitializeComponent();
        }
       
        protected override bool DataRefreshSuppressed
        {
            get { return true; }
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            incomingData.Operation = "FinishingList";
            this.DataContext = incomingData;
            this.dataCurrent = incomingData;
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
           
        }
        TabItem SelectedTab = null;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Tab切换
            if (e.AddedItems.Count == 0)
                return;

            SelectedTab = (TabItem)e.AddedItems[0];
        }
       
    }

  
}
