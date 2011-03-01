using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GLTWarter.Styles
{
    internal class ItemsControlBehavior
    {
        static System.Windows.Style lastSourceStyle;
        static System.Windows.Style cachedNewStyle;

        public static readonly DependencyProperty AlternateItemContainerStyleProperty = DependencyProperty.RegisterAttached(
            "AlternateItemContainerStyle",
            typeof(System.Windows.Style),
            typeof(ItemsControlBehavior),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnAlternateItemContainerStyleChanged)));

        public static void SetAlternateItemContainerStyle(DependencyObject element, System.Windows.Style value)
        {
            element.SetValue(AlternateItemContainerStyleProperty, value);
        }

        public static System.Windows.Style GetAlternateItemContainerStyle(DependencyObject element)
        {
            return (System.Windows.Style)element.GetValue(AlternateItemContainerStyleProperty);
        }

        private static void OnAlternateItemContainerStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {            
            ItemsControl control = sender as ItemsControl;            
            
            if (e.NewValue != null && control != null)
            {                
                if (control.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                {
                    SetAlternateItemContainerStyle(control, (System.Windows.Style)e.NewValue);
                }
                else
                {
                    control.ItemContainerGenerator.StatusChanged += delegate
                    {                        
                        if (control.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                        {
                            control.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, 
                                (Action)delegate()
                                {
                                    SetAlternateItemContainerStyle(control, (System.Windows.Style)e.NewValue);
                                }
                            );
                        }
                    };
                }
            }
        }

        private static void SetAlternateItemContainerStyle(ItemsControl control, System.Windows.Style alternateStyle)
        {
            if (control.Items != null && control.Items.Count > 0)
            {
                GroupItem group = null;
                bool firstStyle = true;
                for (int i = 0, count = 0; i < control.Items.Count; i++, count++)
                {
                    FrameworkElement container = control.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                    if (container != null)
                    {
                        GroupItem thisgroup = Utils.FindVisualParent<GroupItem>(container);
                        if (thisgroup != group)
                        {
                            count = 0;
                            group = thisgroup;
                        }
                        if (count / 5 % 2 != 0)
                        {
                            if (lastSourceStyle != container.Style || firstStyle)
                            {
                                lastSourceStyle = container.Style;
                                cachedNewStyle = new System.Windows.Style(container.GetType(), container.Style);
                                foreach (SetterBase s in alternateStyle.Setters)
                                {
                                    cachedNewStyle.Setters.Add(s);
                                }
                                firstStyle = false;
                            }
                            container.Style = cachedNewStyle;
                        }
                    }
                }
            }
        }
    }
}
