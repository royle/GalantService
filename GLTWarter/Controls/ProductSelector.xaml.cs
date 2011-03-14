using System;
using System.Threading;
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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Globalization;

namespace GLTWarter.Controls
{
    public delegate void ProductSelectorEnterEventHandler(object sender, ProductSelectorEnterEventArgs e);

    /// <summary>
    /// Interaction logic for WaitingIcon.xaml
    /// </summary>
    public partial class ProductSelector : UserControl
    {
        private const int MAX_HINTS_COUNT = 10;

        public ProductSelector()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ProductSelector_Loaded);
        }

        void ProductSelector_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ProductSelector_Loaded);
            comboBarcode.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(comboBarcode_TextChanged), true);
            comboBarcode.AddHandler(TextBox.PreviewTextInputEvent, new TextCompositionEventHandler(comboBarcode_PreviewTextInput), true);
            comboBarcode.AddHandler(ComboBox.PreviewKeyDownEvent, new KeyEventHandler(comboBarcode_KeyDown), true);
            GenerateProductList();
        }

        ComboBox comboBarcode;
        Button buttonEnter;

        public static readonly DependencyProperty ProductsListProperty = 
            DependencyProperty.Register("ProductList", typeof(System.Collections.IList), typeof(ProductSelector), new PropertyMetadata(new PropertyChangedCallback(ProductsListChanged)));
        public System.Collections.IList ProductList
        {
            get { return (System.Collections.IList)this.GetValue(ProductsListProperty); }
            set { this.SetValue(ProductsListProperty, value); }
        }
        public static void ProductsListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProductSelector)d).GenerateProductList();
        }

        public static readonly DependencyProperty SelectedProductProperty =
            DependencyProperty.Register("SelectedProduct", typeof(Galant.DataEntity.Product), typeof(ProductSelector), new PropertyMetadata(new PropertyChangedCallback(SelectedProductChanged)));
        public Galant.DataEntity.Product SelectedProduct
        {
            get { return (Galant.DataEntity.Product)this.GetValue(SelectedProductProperty); }
            set
            {
                this.SetValue(SelectedProductProperty, value);
                if (!string.IsNullOrEmpty(comboBarcode.Text) && value == null && this.SelectedProduct == null)
                    SelectedProductChanged();
            }
        }

        void SelectedProductChanged()
        {
            comboBarcode.SelectedItem = null;
            comboBarcode.Text = string.Empty;
            GenerateProductList();
        }

        public static void SelectedProductChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                ((ProductSelector)d).SelectedProductChanged();
            }
        }

        public static readonly RoutedEvent EnterEvent = EventManager.RegisterRoutedEvent("Enter", RoutingStrategy.Bubble, typeof(ProductSelectorEnterEventHandler), typeof(ProductSelector));
        public event RoutedEventHandler Enter
        {
            add { AddHandler(EnterEvent, value); }
            remove { RemoveHandler(EnterEvent, value); }
        }

        ProductSelectorEnterEventArgs RaiseEnterEvent(string enteredText, bool isUnknownMode)
        {
            ProductSelectorEnterEventArgs arg = new ProductSelectorEnterEventArgs(EnterEvent, this)
            {
                EnteredText = enteredText,
                IsUnknownMode = isUnknownMode,
                SoundPlayed = false
            };
            RaiseEvent(arg);
            return arg;
        }

        /// <summary>
        /// The text should be displayed when no Products were found in the stored list
        /// </summary>
        public static readonly DependencyProperty UnknownTextProperty =
            DependencyProperty.Register("UnknownText", typeof(string), typeof(ProductSelector), new PropertyMetadata(null));
        public string UnknownText
        {
            get { return (string)this.GetValue(UnknownTextProperty); }
            set { this.SetValue(UnknownTextProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            ContentPresenter cp = Utils.FindVisualChild<ContentPresenter>(this);
            if (comboBarcode == null) comboBarcode = (ComboBox)this.ContentTemplate.FindName("comboBarcode", cp);
            if (buttonEnter == null) buttonEnter = (Button)this.ContentTemplate.FindName("buttonEnter", cp);
        }

        private void comboBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.None && !String.IsNullOrEmpty(comboBarcode.Text))
            {
                e.Handled = true;
                try
                {
                    ((IInvokeProvider)UIElementAutomationPeer.CreatePeerForElement(buttonEnter).GetPattern(PatternInterface.Invoke)).Invoke();
                }
                catch (ElementNotEnabledException)
                {
                }
            }
        }        

        private void comboBarcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ((ComboBox)sender).IsDropDownOpen = true;
            comboBarcode.SelectedItem = null;
        }

        private void comboBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.Count == 1 && (e.Changes.First().AddedLength == 0 || e.Changes.First().AddedLength == 1))
            {
                GenerateProductList();
            }

            string hint = comboBarcode.Text.Trim();
            if (comboBarcode.SelectedItem as ProductSelectorItem != null) SelectedProduct = ((ProductSelectorItem)comboBarcode.SelectedItem).SelectedProduct;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<ProductSelectorItem>().Count() == 1
                && MatchProduct(hint, comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct) != null)
                SelectedProduct = comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct;
            if (SelectedProduct != null)
                this.buttonEnter_Click(sender, e);
        }

        void GenerateProductList()
        {
            if (comboBarcode == null) return;

            string hint = comboBarcode.Text;
            List<object> ret = new List<object>();
            if (ProductList != null)
            {
                if (!string.IsNullOrEmpty(hint) && hint.Trim().Length > 0)
                {
                    hint = hint.Trim();
                    foreach (Galant.DataEntity.Product en in ProductList)
                    {
                        ProductSelectorItem si = MatchProduct(hint, en);
                        if (si != null) ret.Add(si);
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                else
                {
                    foreach (Galant.DataEntity.Product en in ProductList)
                    {

                        ret.Add(new ProductSelectorItem(true, en.Alias, en));
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                if (!string.IsNullOrEmpty(this.UnknownText) && ret.Count == 0)
                {
                    if (string.IsNullOrEmpty(Data.Validators.IsValidBarcode(hint)))
                    {
                        ret.Add(new ProductSelectorUnknownItem(this.UnknownText, hint));
                    }
                }
                comboBarcode.ItemsSource = ret;
            }
        }

        private ProductSelectorItem MatchProduct(string hint, Galant.DataEntity.Product en)
        {
            if (en == null || string.IsNullOrEmpty(hint)) return null;
            if (en.AbleFlag && !string.IsNullOrEmpty(en.Alias) && en.Alias.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return new ProductSelectorItem(true, en.Alias, en);
            }

            if (en.AbleFlag && !string.IsNullOrEmpty(en.ProductName) && en.ProductName.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return new ProductSelectorItem(false, en.ProductName, en);
            }
            return null;
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            string hint = comboBarcode.Text.Trim();
            bool isUnknownMode = false;
            bool isSelected = true;
            
            if (comboBarcode.SelectedItem as ProductSelectorItem != null) SelectedProduct = ((ProductSelectorItem)comboBarcode.SelectedItem).SelectedProduct;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<ProductSelectorItem>().Count() == 1
                && MatchProduct(hint, comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct) != null)
                SelectedProduct = comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct;

            if (SelectedProduct == null || MatchProduct(hint, SelectedProduct) == null)
            {
                GenerateProductList();
            }

            if (comboBarcode.SelectedItem as ProductSelectorItem != null)
            {
                SelectedProduct = ((ProductSelectorItem)comboBarcode.SelectedItem).SelectedProduct;
            }
            else if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<ProductSelectorItem>().Count() == 1
                && MatchProduct(hint, comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct) != null)
            {
                SelectedProduct = comboBarcode.ItemsSource.OfType<ProductSelectorItem>().First().SelectedProduct;
            }
            else
            {
                isUnknownMode = comboBarcode.ItemsSource == null ? true : comboBarcode.ItemsSource.OfType<ProductSelectorUnknownItem>().Count() > 0;
                SelectedProduct = null;
                isSelected = false;
            }

            ProductSelectorEnterEventArgs arg = RaiseEnterEvent(hint, isUnknownMode);
            if (!arg.SoundPlayed)
            {
                if (isSelected)
                {
                    Utils.PlaySound(Resource.soundSelected);
                }
                else
                {
                    Utils.PlaySound(Resource.soundMismatch);
                }
            }
        }

        private void UserControl_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource == sender)
            {
                if (e.NewFocus == this)
                {
                    if (e.OldFocus != comboBarcode)
                    {
                        if (comboBarcode != null) comboBarcode.Focus();
                    }
                    else
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    }
                    e.Handled = true;
                }
            }
        }
    }

    class ProductSelectorUnknownItem
    {
        public ProductSelectorUnknownItem(string textTemplate, string value)
        {
            selectedValue = value;
            this.textTemplate = textTemplate;
        }

        string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
        }

        string textTemplate;
        public string DisplayText
        {
            get { return string.Format(CultureInfo.InvariantCulture, textTemplate, selectedValue); }
        }

        public override string ToString()
        {
            return SelectedValue;
        }
    }

    class ProductSelectorItem
    {
        public ProductSelectorItem(bool byid, string value, Galant.DataEntity.Product Product)
        {
            selectedById = byid;
            selectedValue = value;
            selectedProduct = Product;
        }

        bool selectedById;
        public bool SelectedById
        {
            get { return selectedById; }
        }

        string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
        }

        Galant.DataEntity.Product selectedProduct;
        public Galant.DataEntity.Product SelectedProduct
        {
            get { return selectedProduct; }
        }

        public override string ToString()
        {
            return SelectedValue;
        }
    }

    public class ProductSelectorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(container))
            {
                ProductSelectorItem s = item as ProductSelectorItem;
                if (s != null)
                {
                    if (s.SelectedById)
                    {
                        return ((ContentPresenter)container).FindResource("ProductById") as DataTemplate;
                    }
                    else
                    {
                        return ((ContentPresenter)container).FindResource("ProductByName") as DataTemplate;
                    }
                }
                return ((ContentPresenter)container).FindResource("ItemUnknown") as DataTemplate;
            }
            return null;
        }
    }

    public class ProductSelectorEnterEventArgs : RoutedEventArgs
    {
        public ProductSelectorEnterEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public string EnteredText { get; set; }
        public bool IsUnknownMode { get; set; }
        public bool SoundPlayed { get; set; }
    }
}
