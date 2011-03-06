using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.ObjectModel;
using Xceed.Wpf.DataGrid;
using Galant.DataEntity;

namespace GLTWarter.Styles
{
    partial class Style
    {
       
        private void DataGridGroup_Click(object sender, RoutedEventArgs e)
        {
            Xceed.Wpf.DataGrid.Group dc = ((FrameworkElement)sender).DataContext as Xceed.Wpf.DataGrid.Group;
            if (dc != null)
            {
                IEnumerable objs = dc.Items;
                if (objs != null)
                {
                    IEnumerable<Galant.DataEntity.BaseData> bes = objs.OfType<BaseData>();
                    bool IsAllMarked = bes.Where(b => b.IsMarked).Count() == bes.Count();

                    foreach (BaseData ent in objs.OfType<BaseData>())
                    {
                        ent.IsMarked = !IsAllMarked;
                    }
                }
            }

            //ResultPane rp = GLTWarter.Utils.FindVisualParent<ResultPane>((FrameworkElement)sender);
            //if (rp != null)
            //{
            //    rp.CalculateSelectedResults();
            //}
        }

        private void textQuickShipmentQuery_KeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (!(ke.Key == Key.Enter && ke.KeyboardDevice.Modifiers == ModifierKeys.None))
            {
                return;
            }
            e.Handled = true;

            string key = ((TextBox)sender).Text;
            ((TextBox)sender).Text = string.Empty;

            MainScreen.QuickSearch.Execute(key, (TextBox)sender);
        }

        void textQuickShipmentQuery_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = Utils.FindVisualChildren<TextBox>(((Button)sender).Parent).Single(c => c.Name == "textQuickShipmentQuery");
            MainScreen.QuickSearch.Execute(t.Text, t);
        }        
    }

    class AlternateRowIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value / 5 % 2 != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public static class RightClickSelects
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(RightClickSelects), new FrameworkPropertyMetadata(Changed));

        public static void SetEnabled(DependencyObject element, bool value)
        {
            element.SetValue(EnabledProperty, value);
        }

        public static bool GetEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(EnabledProperty);
        }

        public static void Changed(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataGridControl dg = obj as DataGridControl;
            if (null == dg) return;
            dg.PreviewMouseRightButtonDown += new MouseButtonEventHandler(dg_PreviewMouseRightButtonDown);
        }

        static void dg_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridControl dg = sender as DataGridControl;
            if (dg != null)
            {
                DependencyObject hit = dg.InputHitTest(e.GetPosition(dg)) as DependencyObject;
                if (hit != null)
                {
                    DependencyObject row = Utils.FindVisualParent<DataRow>(hit);
                    if (row != null)
                    {
                        object item = dg.GetItemFromContainer(row);
                        if (item != null)
                        {
                            dg.SelectedItem = item;
                        }
                    }
                }
            }
        }
    } 
}
