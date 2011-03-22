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

namespace GLTWarter.Pages.Assign
{
    /// <summary>
    /// Interaction logic for AssignCenter.xaml
    /// </summary>
    public partial class AssignCenter : DetailsBase
    {
        public AssignCenter(Galant.DataEntity.BaseData data):base(data)
        {
            data.Operation = BaseOperatorName.CenterRouteSearch;
            InitializeComponent();
        }

        protected override bool BackAllowed
        {
            get
            {
                return true;
            }
        }

        protected override bool DataRefreshSuppressed
        {
            get
            {
                return true;
            }
        }

        protected override void SetDoOkOperatorString()
        {
            dataCurrent.Operation = BaseOperatorName.SaveCenterRouteSearch;
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            incomingData.Operation = BaseOperatorName.CenterRouteSearch;
            base.OnNext(incomingData);
        }

        bool somethingSelected = false;
        bool SomethingSelected
        {
            get { return somethingSelected; }
            set
            {
                gridManualRouting.IsEnabled = value;
                if (value && !somethingSelected)
                {
                    routeManualRouting.SelectedItem = null;
                }
                somethingSelected = value;
            }
        }

        private void HandleItemActivate(object source, RoutedEventArgs e)
        {
            bool isAllMarked = true;
            foreach (Galant.DataEntity.Paper be in listResult.SelectedItems)
            {
                if (!be.IsMarked) { isAllMarked = false; break; }
            }
            foreach (Galant.DataEntity.Paper be in listResult.SelectedItems)
            {
                be.IsMarked = !isAllMarked;
            }
            SomethingSelected = (from s in ((Galant.DataEntity.Assign.Result)DataContext).ResultData where s.MarkMode == Galant.DataEntity.Assign.CenterAssignData.MarkModes.Standby select s).Any();

            if (e is KeyEventArgs) listResult.MoveHightlightToNext();
            e.Handled = true;
        }

        private void buttonManualRouting_Click(object sender, RoutedEventArgs e)
        {
            UpdateRoute((Galant.DataEntity.Entity)routeManualRouting.SelectionBoxItem);
            listResult.Focus();
        }

        void UpdateRoute(Galant.DataEntity.Entity route)
        {
            foreach (Galant.DataEntity.Assign.CenterAssignData rc in (from s in ((Galant.DataEntity.Assign.Result)DataContext).ResultData where s.MarkMode == Galant.DataEntity.Assign.CenterAssignData.MarkModes.Standby select s))
            {
                rc.HasNewRoute = true;
                rc.NextEntity = route;
                rc.Holder = route;
                rc.NewSubStatus = Galant.DataEntity.PaperSubState.InStation;
                rc.IsMarked = false;
            }
            SomethingSelected = false;
        }
    }

    public class MarkModesTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(container))
            {
                if (item is Galant.DataEntity.Assign.CenterAssignData.MarkModes)
                {
                    Galant.DataEntity.Assign.CenterAssignData.MarkModes mode = (Galant.DataEntity.Assign.CenterAssignData.MarkModes)item;
                    switch (mode)
                    {
                        case Galant.DataEntity.Assign.CenterAssignData.MarkModes.Confirm:
                            return ((ContentControl)container).FindResource("MarkConfirmTemplate") as DataTemplate;
                        case Galant.DataEntity.Assign.CenterAssignData.MarkModes.Standby:
                            return ((ContentControl)container).FindResource("MarkStandbyTemplate") as DataTemplate;
                        default:
                            return ((ContentControl)container).FindResource("MarkNoneTemplate") as DataTemplate;
                    }
                }
            }
            return null;
        }
    }
}
