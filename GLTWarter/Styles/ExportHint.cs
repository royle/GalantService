using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;

namespace GLTWarter.Styles
{
    internal class ExportHint
    {
        /// <summary>
        /// Mark the List View as exportable
        /// </summary>
        public static readonly DependencyProperty IsExportableProperty = DependencyProperty.RegisterAttached(
            "IsExportable",
            typeof(bool),
            typeof(ExportHint),
            new FrameworkPropertyMetadata(false));

        public static void SetIsExportable(DependencyObject element, bool value)
        {
            if (element != null) { element.SetValue(IsExportableProperty, value); }
        }

        public static bool GetIsExportable(DependencyObject element)
        {
            if (element != null) { return (bool)element.GetValue(IsExportableProperty); }
            return false;
        }

        /// <summary>
        /// Save lines as multiple columns in the exported Excel
        /// </summary>
        public static readonly DependencyProperty IsLinesAsColumnsProperty = DependencyProperty.RegisterAttached(
            "IsLinesAsColumns", typeof(bool), typeof(ExportHint), new FrameworkPropertyMetadata(false));

        public static void SetIsLinesAsColumns(DependencyObject element, bool value)
        {
            if (element != null) { element.SetValue(IsLinesAsColumnsProperty, value); }
        }

        public static bool GetIsLinesAsColumns(DependencyObject element)
        {
            if (element != null) { return (bool)element.GetValue(IsLinesAsColumnsProperty); }
            return false;
        }

        /// <summary>
        /// Mark the List View as exportable
        /// </summary>
        public static readonly DependencyProperty IsExportMergableProperty = DependencyProperty.RegisterAttached(
            "IsExportMergable", typeof(bool), typeof(ExportHint), new FrameworkPropertyMetadata(false));

        public static void SetIsExportMergable(DependencyObject element, bool value)
        {
            if (element != null) { element.SetValue(IsExportMergableProperty, value); }
        }

        public static bool GetIsExportMergable(DependencyObject element)
        {
            if (element != null) { return (bool)element.GetValue(IsExportMergableProperty); }
            return false;
        }

        /// <summary>
        /// Mark the column would be used as searching key in a merging operation
        /// </summary>
        public static readonly DependencyProperty MergingKeyProperty = DependencyProperty.RegisterAttached(
            "MergingKey", typeof(string), typeof(ExportHint), new FrameworkPropertyMetadata(null));

        public static void SetMergingKey(DependencyObject element, string value)
        {
            if (element != null) { element.SetValue(MergingKeyProperty, value); }
        }

        public static string GetMergingKey(DependencyObject element)
        {
            if (element != null) { return (string)element.GetValue(MergingKeyProperty); }
            return null;
        }

        /// <summary>
        /// Specify an binding for actual source data. Only used in Old style list view.
        /// </summary>
        public static readonly DependencyProperty BindingProperty = DependencyProperty.RegisterAttached(
            "Binding",
            typeof(BindingExpression),
            typeof(ExportHint),
            new FrameworkPropertyMetadata(null));

        public static void SetBinding(DependencyObject element, BindingExpression value)
        {
            if (element != null)
            {
                element.SetValue(BindingProperty, value);
            }
        }

        public static BindingExpression GetBinding(DependencyObject element)
        {
            if (element != null)
            {
                return (BindingExpression)element.GetValue(BindingProperty);
            }
            return null;
        }

        /// <summary>
        /// Mark a particular as "Address" content, so apporiate export restriction is applied.
        /// </summary>
        public static readonly DependencyProperty IsAddressProperty = DependencyProperty.RegisterAttached(
            "IsAddress",
            typeof(bool),
            typeof(ExportHint),
            new FrameworkPropertyMetadata(false));

        public static void SetIsAddress(DependencyObject element, bool value)
        {
            if (element != null)
            {
                element.SetValue(IsAddressProperty, value);
            }
        }

        public static bool GetIsAddress(DependencyObject element)
        {
            if (element != null)
            {
                return (bool)element.GetValue(IsAddressProperty);
            }
            return false;
        }

        /// <summary>
        /// Minimum indentation in exported header row.
        /// Used in exporting print template to excel, so that multiple tables in the same excel 
        ///   could have the same indentation even if the data is empty.
        /// </summary>
        public static readonly DependencyProperty MinGroupLevelProperty = DependencyProperty.RegisterAttached(
            "MinGroupLevel",
            typeof(int),
            typeof(ExportHint),
            new FrameworkPropertyMetadata(0));

        public static void SetMinGroupLevel(DependencyObject element, int value)
        {
            if (element != null)
                element.SetValue(MinGroupLevelProperty, value);
        }

        public static int GetMinGroupLevel(DependencyObject element)
        {
            if (element != null)
                return (int)element.GetValue(MinGroupLevelProperty);
            return 0;
        }
    }
}