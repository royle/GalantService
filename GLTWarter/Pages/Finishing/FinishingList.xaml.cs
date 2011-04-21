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
using System.Collections.Specialized;

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
            incomingData.Operation = "SearchFinishingList";
            this.DataContext = incomingData;
            this.dataCurrent = incomingData;
        }

        public override void OnApplyTemplate()
        {
            (listCollection.SelectedItems as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(List_CollectionChanged);
            (listDeliveryMan.SelectedItems as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(List_CollectionChanged);
        }

        void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateSelectedResults();
        }

        TabItem SelectedTab = null;

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//Tab切换
            if (e.AddedItems.Count == 0)
                return;

            SelectedTab = (TabItem)e.AddedItems[0];
            this.CalculateSelectedResults();
        }

        protected override void OnDataInitialization(object sender, RoutedEventArgs e)
        {
            base.OnDataInitialization(sender, e);
            this.dataCurrent.Operation = "SearchFinishingList";
            base.buttonNext_Click(sender, e);
        }

        private void CalculateSelectedResults()
        {
            if (SelectedTab == tabDeliveryMan)
            {
                if (this.listDeliveryMan.SelectedItems != null)
                {
                    List<Galant.DataEntity.Paper> list = new List<Galant.DataEntity.Paper>();
                    foreach (Galant.DataEntity.Result.HolderPapers holder in this.listDeliveryMan.SelectedItems)
                    {
                        list.AddRange(holder.Papers);
                    }
                    SelectedResults = list;
                }
            }
            else
            {
                if (this.listCollection.SelectedItems != null)
                {
                    SelectedResults = this.listCollection.SelectedItems;
                }
            }
        }

        public System.Collections.IList SelectedResults
        {
            get;
            set;
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedResults == null || this.SelectedResults.Count <= 0)
            {
                MessageBox.Show(AppCurrent.Active.MainWindow, "请选择要归班的清单", this.Title);
                return;
            }
            Galant.DataEntity.Result.FinishCheckin checkin = new Galant.DataEntity.Result.FinishCheckin();
            checkin.CheckinCollections = this.SelectedResults.OfType<Galant.DataEntity.Paper>().ToList();
            checkin.Operation = "CheckinFinish";
            DetailsBase pageNext = new GLTWarter.Pages.Finishing.PackageFinish(checkin);
            pageNext.Return+=new ReturnEventHandler<Galant.DataEntity.BaseData>(pageNext_Return);
            this.NavigationService.Navigate(pageNext);

        }

        protected override void pageNext_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            this.dataCurrent.Operation = "SearchFinishingList";
            base.buttonNext_Click(sender, null);
        }

        private void HandleItemActivate(object source, RoutedEventArgs e)
        {
            if (e is KeyEventArgs)
            {
                KeyEventArgs ke = (KeyEventArgs)e;
                if (!(ke.Key == Key.Enter && ke.KeyboardDevice.Modifiers == ModifierKeys.None))
                {
                    return;
                }
            }
            this.buttonNew_Click(source, null);
        }
       
    }

  
}
