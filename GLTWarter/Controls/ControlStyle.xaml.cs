using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows;
using GLTWarter.Data;

namespace GLTWarter.Controls
{
    partial class ControlStyle
    {
        private void ButtonRemoveCondition_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is DependencyObject)
            {
                ContentPresenter conditionControl = Utils.FindVisualParent<ContentPresenter>(sender as DependencyObject);
                ItemsControl itemsControl = Utils.FindVisualParent<ItemsControl>(sender as DependencyObject);
                if (conditionControl != null && itemsControl != null)
                {
                    SearchCondition condition = itemsControl.ItemContainerGenerator.ItemFromContainer(conditionControl) as SearchCondition;
                    ISearchDataWithConditions context = itemsControl.DataContext as ISearchDataWithConditions;
                    if (context != null && condition != null)
                    {
                        context.RemoveCondition(condition);
                    }
                }
            }
        }
    }
}
