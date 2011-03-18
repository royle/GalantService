using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;

namespace GLTWarter.Styles
{
    internal class ReportHint
    {
        /// <summary>
        /// When applies to BlockUIContainer, all the TextBlock inside should be used for Report Exporting purpose.
        /// </summary>
        public static readonly DependencyProperty ForExportProperty =
            DependencyProperty.RegisterAttached("ForExport", typeof(bool), typeof(ReportHint), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached("Row", typeof(int), typeof(ReportHint), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached("Column", typeof(int), typeof(ReportHint), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached("RowSpan", typeof(int), typeof(ReportHint), new FrameworkPropertyMetadata(1));
        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached("ColumnSpan", typeof(int), typeof(ReportHint), new FrameworkPropertyMetadata(1));
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(ReportHint), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.RegisterAttached("BorderThickness", typeof(Thickness), typeof(ReportHint), new FrameworkPropertyMetadata(new Thickness(0)));


        public static void SetForExport(DependencyObject element, bool value)
        {
            if (element != null)
                element.SetValue(ForExportProperty, value);
        }
        public static bool GetForExport(DependencyObject element)
        {
            if (element != null)
                return (bool)element.GetValue(ForExportProperty);
            return false;
        }

        public static void SetRow(DependencyObject element, int value)
        {
            if (element != null)
                element.SetValue(RowProperty, value);
        }
        public static int GetRow(DependencyObject element)
        {
            if (element != null)
                return (int)element.GetValue(RowProperty);
            return 0;
        }

        public static void SetColumn(DependencyObject element, int value)
        {
            if (element != null)
                element.SetValue(ColumnProperty, value);
        }
        public static int GetColumn(DependencyObject element)
        {
            if (element != null)
                return (int)element.GetValue(ColumnProperty);
            return 0;
        }

        public static void SetRowSpan(DependencyObject element, int value)
        {
            if (element != null)
                element.SetValue(RowSpanProperty, value);
        }
        public static int GetRowSpan(DependencyObject element)
        {
            if (element != null)
                return (int)element.GetValue(RowSpanProperty);
            return 1;
        }

        public static void SetColumnSpan(DependencyObject element, int value)
        {
            if (element != null)
                element.SetValue(ColumnSpanProperty, value);
        }
        public static int GetColumnSpan(DependencyObject element)
        {
            if (element != null)
                return (int)element.GetValue(ColumnSpanProperty);
            return 1;
        }        

        public static void SetFontSize(DependencyObject element, double value)
        {
            if (element != null)
                element.SetValue(FontSizeProperty, value);
        }
        public static double GetFontSize(DependencyObject element)
        {
            if (element != null)
                return (double)element.GetValue(FontSizeProperty);
            return 0;
        }

        public static void SetBorderThickness(DependencyObject element, Thickness value)
        {
            if (element != null)
                element.SetValue(BorderThicknessProperty, value);
        }
        public static Thickness GetBorderThickness(DependencyObject element)
        {
            if (element != null)
                return (Thickness)element.GetValue(BorderThicknessProperty);
            return new Thickness(0);
        }
    }
}
