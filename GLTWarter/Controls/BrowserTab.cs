using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;

namespace GLTWarter.Controls
{
    public class BrowserTabControl : TabControl
    {
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            Reindex();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            Reindex();
        }

        void Reindex()
        {
            if (this.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    TabItem item = this.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;
                    if (item != null)
                    {
                        Panel.SetZIndex(item, item.IsSelected ? 1 : -i);
                    }
                }
                Panel panel = this.Template.FindName("HeaderPanel", this) as Panel;
                if (panel != null)
                {
                    try
                    {
                        panel.Dispatcher.Invoke(DispatcherPriority.Input, (Action)Dummy);
                    }
                    catch (InvalidOperationException) { }
                }
            }
        }

        static void Dummy() { }

        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new BrowserTabItem();
        }
    }

    public class BrowserTabItem : TabItem
    {
        static BrowserTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrowserTabItem),
                new FrameworkPropertyMetadata(typeof(BrowserTabItem)));
        }

        public static readonly RoutedEvent CloseTabEvent =
            EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(BrowserTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button closeButton = base.GetTemplateChild("PART_Close") as Button;
            if (closeButton != null)
                closeButton.Click += new System.Windows.RoutedEventHandler(closeButton_Click);
            Grid grid = base.GetTemplateChild("PART_Grid") as Grid;
            if (grid != null)
            {
                grid.MouseDown += new System.Windows.Input.MouseButtonEventHandler(content_MouseDown);
            }
        }

        void content_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
                this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }

        void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
