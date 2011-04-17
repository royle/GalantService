using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace GLTWarter.Controls
{
    public partial class DetailsContextMenu : ContextMenu
    {
        object markedObject;

        public DetailsContextMenu()
        {
            InitializeComponent();
        }

        Object placementTarget;
        protected override void OnOpened(RoutedEventArgs e)
        {
            base.OnOpened(e);
            bool keepOpen = false; // If an menu should be closed, or opened because there are valid object
            markedObject = null;

            placementTarget = null;
            if (this.PlacementTarget is Xceed.Wpf.DataGrid.DataGridControl)
            {
                Xceed.Wpf.DataGrid.DataGridControl grid = this.PlacementTarget as Xceed.Wpf.DataGrid.DataGridControl;
                placementTarget = this.PlacementTarget;
                if (grid.SelectedItem != null)
                {
                    markedObject = grid.SelectedItem;
                }
            }

            menuShipment.Visibility = Visibility.Collapsed;

            if (markedObject != null)
            {
                if (markedObject is Galant.DataEntity.Paper)
                {
                    
					menuShipment.Visibility = Visibility.Visible;
                   
                    keepOpen = true;
                }

                if (keepOpen && this.Placement != System.Windows.Controls.Primitives.PlacementMode.MousePoint)
                {
                    Visual q = null;
                    if (this.PlacementTarget is ItemsControl)
                    {
                        q = Utils.ContainerFromItemInList(((ItemsControl)this.PlacementTarget), markedObject) as Visual;
                    }
                    if (q != null)
                    {
                        MatrixTransform t = (MatrixTransform)this.PlacementTarget.TransformToVisual(q);
                        Rect y = VisualTreeHelper.GetDescendantBounds(q);
                        this.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
                        this.PlacementRectangle = new Rect(new Point(-t.Matrix.OffsetX + Math.Min(y.Size.Height, y.Size.Width) / 2, -t.Matrix.OffsetY + Math.Min(y.Size.Height, y.Size.Width) / 2), y.Size);
                    }
                }
            }

            this.Visibility = keepOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void menuPackage_Click(object sender, RoutedEventArgs e)
        {
            Page page = null;
            if (markedObject is Galant.DataEntity.EventLog)
            {
                //page = new Pages.Shipment.Entity.Details(((Data.EventLog)markedObject).Shipment);
            }
            else if (markedObject is Galant.DataEntity.Package)
            {
                //page = new Pages.Shipment.Entity.Details(((Data.Complaint)markedObject).Shipment);
            }
            else if (markedObject is Galant.DataEntity.Entity)
            {
                //page = new Pages.Shipment.Entity.Details(((Data.Bill)markedObject).Shipment);
            }
            else
            {
                page = new Pages.Order.PaperDetail((Galant.DataEntity.Paper)markedObject);
            }
            AppCurrent.Active.MainScreen.NavigateActive(page);
        }
    }
}
