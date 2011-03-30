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
            data.Operation = BaseOperatorName.StationRouteSearch;
            InitializeComponent();
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
            SomethingSelected = (from s in ((Galant.DataEntity.Assign.Result)DataContext).ResultData where s.MarkMode == Galant.DataEntity.Assign.CenterAssignData.MarkModes.Standby select s).Any();

            if (e is KeyEventArgs) listResult.MoveHightlightToNext();
            e.Handled = true;
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            incomingData.Operation = BaseOperatorName.StationRouteSearch;
            base.OnNext(incomingData);
        }
    }
}
