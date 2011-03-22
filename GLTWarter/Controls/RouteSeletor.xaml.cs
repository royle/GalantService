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
using System.Globalization;
using Odyssey.Controls;

namespace GLTWarter.Controls
{
    /// <summary>
    /// Interaction logic for RouteSeletor.xaml
    /// </summary>
    public partial class RouteSeletor : UserControl
    {
        public RouteSeletor()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(RouteSeletor_Loaded);
        }
        bool suppressAutoOpen = false;

        void RouteSeletor_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(RouteSeletor_Loaded);
            bar.SelectedBreadcrumbChanged += new RoutedEventHandler(bar_SelectedBreadcrumbChanged);
            box.AddHandler(ComboBox.PreviewKeyDownEvent, new KeyEventHandler(box_PreviewKeyDown), true);
            GeneratePackageList();
        }



        public static readonly DependencyProperty RootProperty =
            DependencyProperty.Register("Root", typeof(Galant.DataEntity.Entity), typeof(RouteSeletor), new PropertyMetadata(new PropertyChangedCallback(RootChanged)));
        public Galant.DataEntity.Entity Root
        {
            get { return (Galant.DataEntity.Entity)this.GetValue(RootProperty); }
            set { this.SetValue(RootProperty, value); }
        }
        public static void RootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RouteSeletor)d).bar.Root = e.NewValue;
            PopulateRoutes(((RouteSeletor)d).bar.RootItem);
        }

        public static readonly DependencyProperty SelectedRouteProperty =
            DependencyProperty.Register("SelectedRoute", typeof(Galant.DataEntity.Route), typeof(RouteSeletor),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RouteSeletor.SelectedRouteChanged)));
        public Galant.DataEntity.Route SelectedRoute
        {
            get { return (Galant.DataEntity.Route)this.GetValue(SelectedRouteProperty); }
            set { this.SetValue(SelectedRouteProperty, value); }
        }
        public static void SelectedRouteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BreadcrumbBar bar = ((RouteSeletor)d).bar;
            RouteSeletor rs = (RouteSeletor)d;
            if (bar.SelectedItem != e.NewValue)
            {
                rs.suppressAutoOpen = true;
                Galant.DataEntity.Route selected = e.NewValue as Galant.DataEntity.Route;
                if (bar.RootItem != null)
                {
                    bar.RootItem.Items.Clear();
                    IEnumerable<Galant.DataEntity.Route> routes = ComplexData.RouteCache.PathTo(bar.Root as Galant.DataEntity.Entity, selected);
                    // Try to build the whole path
                    if (!bar.BuildBreadcrumbsFromValues(routes.OfType<object>()))
                    {
                        // If failed, just add the root, and the selected result to the root directly
                        bar.RootItem.Items.Clear();
                        PopulateRoutes(bar.RootItem);
                        bar.RootItem.SelectedItem = e.NewValue;
                        if (bar.RootItem.SelectedItem == null && e.NewValue != null)
                        {
                            bar.RootItem.Items.Add(e.NewValue);
                        }
                    }
                }
                RestoreToBar(rs);
                rs.suppressAutoOpen = false;
            }
        }

        private static void RestoreToBar(RouteSeletor d)
        {
            d.box.LostFocus -= new RoutedEventHandler(d.box_LostFocus);
            d.box.Visibility = Visibility.Hidden;
            d.bar.Visibility = Visibility.Visible;
            d.bar.Focus();
        }

        private void bar_InputStateEntered(object sender, RoutedEventArgs e)
        {
            box.Text = SelectedRoute != null ? SelectedRoute.RountName : string.Empty;
            box.Visibility = Visibility.Visible;
            bar.Visibility = Visibility.Hidden;
            box.Focus();
            box.LostFocus += new RoutedEventHandler(box_LostFocus);
        }

        void box_LostFocus(object sender, RoutedEventArgs e)
        {
            RestoreToBar(this);
        }

        int openVersion = 0;
        private void bar_SelectedBreadcrumbChanged(object sender, RoutedEventArgs e)
        {
            if (!suppressAutoOpen && bar.SelectedBreadcrumb != null)
            {
                //BreadcrumbButton.OpenOverflowCommand.Execute(null, bar.SelectedBreadcrumb);
                BreadcrumbButton button = Utils.FindVisualChild<BreadcrumbButton>(bar.SelectedBreadcrumb);
                if (button != null)
                {
                    int thisOpenVersion = ++openVersion;
                    Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.ContextIdle,
                        (Action)delegate()
                        {
                            if (thisOpenVersion == openVersion)
                            {
                                openVersion++;
                                BreadcrumbButton.OpenOverflowCommand.Execute(null, button);
                            }
                        }
                    );
                }
            }
            this.SelectedRoute = bar.SelectedItem as Galant.DataEntity.Route;
        }

        private void bar_PopulateItems(object sender, Odyssey.Controls.BreadcrumbItemEventArgs e)
        {
            BreadcrumbItem item = e.Item;
            if (item.Items.Count == 0)
            {
                PopulateRoutes(item);
                e.Handled = true;
            }
        }

        private static void PopulateRoutes(BreadcrumbItem item)
        {
            if (item != null)
            {
                BreadcrumbBar bar = item.BreadcrumbBar;
                if (item.Data is Galant.DataEntity.Route)
                {
                    Galant.DataEntity.Route route = item.Data as Galant.DataEntity.Route;
                    if (route.ToEntity != null)
                    {
                        foreach (Galant.DataEntity.Route r in ComplexData.RouteCache.OneHopRoutesFrom(route.ToEntity).OrderBy(y => y.RountName))
                        {
                            // Last mile or Internal Transfer
                            if ((route.FromEntity == null || r.ToEntity == null || !r.ToEntity.EntityEquals(route.FromEntity))
                                && (r.ToEntity == null || r.ToEntity.EntityType == Galant.DataEntity.EntityType.Station)
                                )
                            {
                                item.Items.Add(r);
                            }
                        }
                    }
                }
                else if (item.Data is Galant.DataEntity.Entity)
                {
                    Galant.DataEntity.Entity entity = item.Data as Galant.DataEntity.Entity;
                    foreach (Galant.DataEntity.Route r in ComplexData.RouteCache.OneHopRoutesFrom(entity).OrderBy(y => y.RountName))
                    {
                        // Last mile or Internal Transfer
                        if (r.ToEntity == null || r.ToEntity.EntityType == Galant.DataEntity.EntityType.Station)
                        {
                            item.Items.Add(r);
                        }
                    }
                }
            }
        }

        private void bar_BreadcrumbItemDropDownOpened(object sender, BreadcrumbItemEventArgs e)
        {
            BreadcrumbItem item = e.Item;
            if (!(item.Data is BreadcrumbItem))
            {
                item.Items.Clear();
                PopulateRoutes(item);
            }
        }

        private void bar_BreadcrumbItemDropDownClosed(object sender, BreadcrumbItemEventArgs e)
        {

        }

        private void box_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            box.IsDropDownOpen = true;
            box.SelectedItem = null;
        }

        private void box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.Count == 1 && (e.Changes.First().AddedLength == 0 || e.Changes.First().AddedLength == 1))
            {
                GeneratePackageList();
            }
        }

        private void box_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                if (!ConfirmSelectedRoute())
                {
                    GeneratePackageList();
                    if (!ConfirmSelectedRoute())
                    {
                        SelectedRoute = null;
                    }
                }
                RestoreToBar(this);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                RestoreToBar(this);
                e.Handled = true;
            }
        }

        private void box_DropDownClosed(object sender, EventArgs e)
        {
            ConfirmSelectedRoute();
        }

        private bool ConfirmSelectedRoute()
        {
            try
            {
                suppressAutoOpen = true;
                string hint = box.Text.Trim();
                if (box.SelectedItem != null)
                {
                    SelectedRoute = ((Galant.DataEntity.Route)box.SelectedItem);
                    return true;
                }
                if (box.ItemsSource != null && ((IList<Galant.DataEntity.Route>)box.ItemsSource).Count == 1
                    && MatchRoute(hint, ((IList<Galant.DataEntity.Route>)box.ItemsSource)[0]) != null)
                {
                    SelectedRoute = ((IList<Galant.DataEntity.Route>)box.ItemsSource)[0];
                    return true;
                }
                if (box.ItemsSource == null)
                {
                    return false;
                }
                Galant.DataEntity.Route full_match = box.ItemsSource.OfType<Galant.DataEntity.Route>().Where(y => y.RountName == hint).FirstOrDefault();
                if (full_match != null)
                {
                    SelectedRoute = full_match;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                suppressAutoOpen = false;
            }
        }

        void GeneratePackageList()
        {
            string hint = box.Text;
            if (Root != null)
            {
                if (!string.IsNullOrEmpty(hint) && hint.Trim().Length > 0)
                {
                    List<Galant.DataEntity.Route> ret = new List<Galant.DataEntity.Route>();
                    hint = hint.Trim();
                    foreach (Galant.DataEntity.Route route in ComplexData.RouteCache.AllRoutesFrom(Root))
                    {
                        Galant.DataEntity.Route r = MatchRoute(hint, route);
                        if (r != null) ret.Add(r);
                    }
                    box.ItemsSource = ret;
                }
                else
                {
                    box.ItemsSource = ComplexData.RouteCache.AllRoutesFrom(Root);
                }
            }
        }

        private Galant.DataEntity.Route MatchRoute(string hint, Galant.DataEntity.Route route)
        {
            if (route == null || string.IsNullOrEmpty(hint)) return null;
            if (!string.IsNullOrEmpty(route.RountName) && route.RountName.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return route;
            } if (!string.IsNullOrEmpty(route.RountName) && route.RountName.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return route;
            }
            return null;
        }

        private void bar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                bar.SetInputState();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                SelectedRoute = null;
                e.Handled = true;
            }
        }

        private void UserControl_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus == this)
            {
                if (bar.IsVisible)
                {
                    bar.Focus();
                    e.Handled = true;
                }
                else if (box.IsVisible)
                {
                    box.Focus();
                    e.Handled = true;
                }
            }
        }
    }

    [ValueConversion(typeof(object), typeof(string))]
    class RouteSelectorItemTraceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as Galant.DataEntity.Route != null)
            {
                return Data.RouteNameConverter.Convert(value as Galant.DataEntity.Route);
            }
            else if (value as Galant.DataEntity.Entity != null)
            {
                return Data.EntityNameConverter.Convert(value as Galant.DataEntity.Entity);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
