using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Data;

namespace GLTWarter.Styles
{
    public class BindableExtender
    {
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj,
            string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text",
                typeof(string),
                typeof(BindableExtender),
                new UIPropertyMetadata(null,
                    TextProperty_PropertyChanged));

        private static void TextProperty_PropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Run)
            {
                ((Run)dependencyObject).Text = (string)e.NewValue;
            }
        }
    }
}
