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

namespace GLTWarter.Pages.StationAssign
{
    /// <summary>
    /// Interaction logic for StationAssign.xaml
    /// </summary>
    public partial class StationAssign : DetailsBase
    {
        public StationAssign(Galant.DataEntity.BaseData data)
            : base(data)
        {
            InitializeComponent();
            data.Operation = BaseOperatorName.StationRouteSearch;
        }

        bool somethingSelected = false;
        bool SomethingSelected
        {
            get { return somethingSelected; }
            set
            {
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
            SomethingSelected = (from s in ((Galant.DataEntity.StationAssign.Result)DataContext).ResultData where s.MarkMode == Galant.DataEntity.StationAssign.StationAssignData.MarkModes.Confirm select s).Any();

            if (e is KeyEventArgs) listResult.MoveHightlightToNext();
            e.Handled = true;
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            incomingData.Operation = BaseOperatorName.StationRouteSearch;
            ((Galant.DataEntity.StationAssign.Result)incomingData).Entities= AppCurrent.Active.AppCach.Staffs;
            base.OnNext(incomingData);
        }

        protected override void SetDoOkOperatorString()
        {
            Galant.DataEntity.Entity deliver = (Galant.DataEntity.Entity)this.cmbAssingDeliver.SelectionBoxItem;
            Galant.DataEntity.StationAssign.Result data = (Galant.DataEntity.StationAssign.Result)this.dataCurrent;
            foreach (Galant.DataEntity.StationAssign.StationAssignData assign in data.ResultData)
            {
                if (!assign.IsMarked)
                    continue;

                assign.NewPaperSubStatus = Galant.DataEntity.PaperSubState.InTransit;
                assign.Holder = deliver;
                assign.DeliverB = deliver;
                assign.DeliverBTime = DateTime.Now;
            }
            data.Operation = BaseOperatorName.SaveStationAssign;
        }
    }

    public class MarkModesTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(container))
            {
                if (item is Galant.DataEntity.StationAssign.StationAssignData.MarkModes)
                {
                    Galant.DataEntity.StationAssign.StationAssignData.MarkModes mode = (Galant.DataEntity.StationAssign.StationAssignData.MarkModes)item;
                    switch (mode)
                    {
                        case Galant.DataEntity.StationAssign.StationAssignData.MarkModes.Confirm:
                            return ((ContentControl)container).FindResource("MarkConfirmTemplate") as DataTemplate;
                        //case Galant.DataEntity.StationAssign.StationAssignData.MarkModes.Standby:
                        //    return ((ContentControl)container).FindResource("MarkStandbyTemplate") as DataTemplate;
                        default:
                            return ((ContentControl)container).FindResource("MarkNoneTemplate") as DataTemplate;
                    }
                }
            }
            return null;
        }
    }
}
