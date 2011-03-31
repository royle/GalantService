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
using System.Globalization;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using GLTWarter.Controls;
using System.Collections.Specialized;
using System.Threading;

namespace GLTWarter.Pages.Finishing
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class PackageFinish : DetailsBase
    {
        delegate bool BatchHandler();
        Queue<Galant.DataEntity.Paper> batchShipment = null;
        BatchHandler batchFunction = null;
        bool isUpdateCurrency = true;//是否批量修改状态时更新收款方式

        public PackageFinish(Galant.DataEntity.Result.FinishCheckin data)
            : base(data)
        {
            
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PackageFinish_Loaded);
            this.Loaded += new RoutedEventHandler(PackageFinish_LoadedOnce);
        }

        public override void OnApplyTemplate()
        {
        }

        void PackageFinish_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                Galant.DataEntity.Result.FinishCheckin checkin = this.DataContext as Galant.DataEntity.Result.FinishCheckin;
                checkin.NotifyPapersChanged();
            }
        }

        

        void PackageFinish_LoadedOnce(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(PackageFinish_LoadedOnce);
            base.buttonNext_Click(sender, e);
        }

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override void FocusFirstControl()
        {
            this.selectorShipment.Focus();
        }

        protected override void PreCommit()
        {
           
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            Galant.DataEntity.Result.FinishCheckin checkin = incomingData as Galant.DataEntity.Result.FinishCheckin;
            checkin.Operation = "CheckinFinish";
            this.DataContext = checkin;
            this.dataCurrent = checkin;
        }

        private void Shipment_selectorEnter(object sender, PaperSelectorEnterEventArgs e)
        {
            Galant.DataEntity.Paper sm = this.selectorShipment.SelectedPaper;
            //处理
            if (sm == null)
                return;
            GoToDetail(sm, e.IsUnknownMode, e.EnteredText);
        }

        private void Finish_ButtonClick(object sender, RoutedEventArgs e)
        {//归班为成功
            if (this.listPending.SelectedItem == null)
                return;
            Galant.DataEntity.Paper paper = this.listPending.SelectedItem as Galant.DataEntity.Paper;
            if(paper !=null)
                this.NavigationService.Navigate(new GLTWarter.Pages.Finishing.FinishDetail(paper));
        }

       

        /// <summary>
        /// 转到详细页
        /// </summary>
        private void GoToDetail(Galant.DataEntity.Paper sm, bool isUnknownMode, string enteredText)
        {
            if (sm != null)
                this.NavigationService.Navigate(new GLTWarter.Pages.Finishing.FinishDetail(sm));
        }

        private void FigureBatchSimple(Galant.DataEntity.Paper sm, bool isUnknownMode, string enteredText, BatchHandler handle)
        {
        }

       

        void pageBatch_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            
        }

        void page_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
           
        }

        private void buttonNextDry_Click(object sender, RoutedEventArgs e)
        {
            Galant.DataEntity.Result.FinishCheckin data = this.DataContext as Galant.DataEntity.Result.FinishCheckin;
            if (data != null)
            {
                DetailsBase pageNext = new GLTWarter.Pages.Finishing.FinishConsign(data);
                pageNext.Return += new ReturnEventHandler<Galant.DataEntity.BaseData>(pageNext_Return);

                this.NavigationService.Navigate(pageNext);
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
            Xceed.Wpf.DataGrid.DataRow row =source as Xceed.Wpf.DataGrid.DataRow;
            Galant.DataEntity.Paper data = row.DataContext as Galant.DataEntity.Paper;
            if (data != null)
            {
                this.NavigationService.Navigate(new GLTWarter.Pages.Finishing.FinishDetail(data));
            }
        }

        private void buttonUndo_Click(object sender, RoutedEventArgs e)
        {
            if (this.listDown.SelectedItems == null || this.listDown.SelectedItems.Count == 0)
                return;
            foreach (object o in this.listDown.SelectedItems)
            {
                Galant.DataEntity.Paper p = o as Galant.DataEntity.Paper;
                p.PaperSubStatus = Galant.DataEntity.PaperSubState.InTransit;
            }

            if (this.DataContext != null)
            {
                Galant.DataEntity.Result.FinishCheckin checkin = this.DataContext as Galant.DataEntity.Result.FinishCheckin;
                checkin.NotifyPapersChanged();
            }
        }

      
    
    }
}