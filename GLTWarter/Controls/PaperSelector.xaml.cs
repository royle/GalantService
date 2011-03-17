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
    public delegate void PaperSelectorEnterEventHandler(object sender, PaperSelectorEnterEventArgs e);

    /// <summary>
    /// Interaction logic for WaitingIcon.xaml
    /// </summary>
    public partial class PaperSelector : UserControl
    {
        private const int MAX_HINTS_COUNT = 10;

        public PaperSelector()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PaperSelector_Loaded);
        }

        void PaperSelector_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(PaperSelector_Loaded);
            comboBarcode.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(comboBarcode_TextChanged), true);
            comboBarcode.AddHandler(TextBox.PreviewTextInputEvent, new TextCompositionEventHandler(comboBarcode_PreviewTextInput), true);
            comboBarcode.AddHandler(ComboBox.PreviewKeyDownEvent, new KeyEventHandler(comboBarcode_KeyDown), true);
            GeneratePackageList();
        }

        ComboBox comboBarcode;
        Button buttonEnter;

        public static readonly DependencyProperty PapersListProperty = 
            DependencyProperty.Register("PapersList", typeof(System.Collections.IList), typeof(PaperSelector), new PropertyMetadata(new PropertyChangedCallback(PapersListChanged)));
        public System.Collections.IList PapersList
        {
            get { return (System.Collections.IList)this.GetValue(PapersListProperty); }
            set { this.SetValue(PapersListProperty, value); }
        }
        public static void PapersListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PaperSelector)d).GeneratePackageList();
        }

        public static readonly DependencyProperty SelectedPaperProperty =
            DependencyProperty.Register("SelectedPaper", typeof(Galant.DataEntity.Paper), typeof(PaperSelector), new PropertyMetadata(new PropertyChangedCallback(SelectedPaperChanged)));
        public Galant.DataEntity.Paper SelectedPaper
        {
            get { return (Galant.DataEntity.Paper)this.GetValue(SelectedPaperProperty); }
            set
            {
                this.SetValue(SelectedPaperProperty, value);
                if (!string.IsNullOrEmpty(comboBarcode.Text) && value == null && this.SelectedPaper == null)
                    SelectedPaperChanged();
            }
        }

        void SelectedPaperChanged()
        {
            comboBarcode.SelectedItem = null;
            comboBarcode.Text = string.Empty;
            GeneratePackageList();
        }

        public static void SelectedPaperChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                ((PaperSelector)d).SelectedPaperChanged();
            }
        }

        public static readonly RoutedEvent EnterEvent = EventManager.RegisterRoutedEvent("Enter", RoutingStrategy.Bubble, typeof(PaperSelectorEnterEventHandler), typeof(PaperSelector));
        public event RoutedEventHandler Enter
        {
            add { AddHandler(EnterEvent, value); }
            remove { RemoveHandler(EnterEvent, value); }
        }

        PaperSelectorEnterEventArgs RaiseEnterEvent(string enteredText, bool isUnknownMode)
        {
            PaperSelectorEnterEventArgs arg = new PaperSelectorEnterEventArgs(EnterEvent, this)
            {
                EnteredText = enteredText,
                IsUnknownMode = isUnknownMode,
                SoundPlayed = false
            };
            RaiseEvent(arg);
            return arg;
        }

        /// <summary>
        /// The text should be displayed when no Papers were found in the stored list
        /// </summary>
        public static readonly DependencyProperty UnknownTextProperty =
            DependencyProperty.Register("UnknownText", typeof(string), typeof(PaperSelector), new PropertyMetadata(null));
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
                GeneratePackageList();
            }

            string hint = comboBarcode.Text.Trim();
            if (comboBarcode.SelectedItem as PaperSelectorItem != null) SelectedPaper = ((PaperSelectorItem)comboBarcode.SelectedItem).SelectedPaper;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<PaperSelectorItem>().Count() == 1
                && MatchPaper(hint, comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper) != null)
                SelectedPaper = comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper;
        }

        void GeneratePackageList()
        {
            if (comboBarcode == null) return;

            string hint = comboBarcode.Text;
            List<object> ret = new List<object>();
            if (PapersList != null)
            {
                if (!string.IsNullOrEmpty(hint) && hint.Trim().Length > 0)
                {
                    hint = hint.Trim();
                    foreach (Galant.DataEntity.Paper sm in PapersList)
                    {
                        PaperSelectorItem si = MatchPaper(hint, sm);
                        if (si != null) ret.Add(si);
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                else
                {
                    foreach (Galant.DataEntity.Paper sm in PapersList)
                    {

                        ret.Add(new PaperSelectorItem(true, sm.PaperId, sm));
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                if (!string.IsNullOrEmpty(this.UnknownText) && ret.Count == 0)
                {
                    if (string.IsNullOrEmpty(Data.Validators.IsValidBarcode(hint)))
                    {
                        ret.Add(new PaperSelectorUnknownItem(this.UnknownText, hint));
                    }
                }
                comboBarcode.ItemsSource = ret;
            }
        }

        private PaperSelectorItem MatchPaper(string hint, Galant.DataEntity.Paper sm)
        {
            if (sm == null || string.IsNullOrEmpty(hint)) return null;
            if (!string.IsNullOrEmpty(sm.PaperId) && sm.PaperId.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return new PaperSelectorItem(true, sm.PaperId, sm);
            }

            if (sm.OriginName!= null)
            {
                foreach (string orf in sm.OriginName)
                {
                    if (orf.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        return new PaperSelectorItem(false, orf, sm);
                    }
                }
            }
            return null;
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            string hint = comboBarcode.Text.Trim();
            bool isUnknownMode = false;
            bool isSelected = true;
            
            if (comboBarcode.SelectedItem as PaperSelectorItem != null) SelectedPaper = ((PaperSelectorItem)comboBarcode.SelectedItem).SelectedPaper;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<PaperSelectorItem>().Count() == 1
                && MatchPaper(hint, comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper) != null)
                SelectedPaper = comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper;

            if (SelectedPaper == null || MatchPaper(hint, SelectedPaper) == null)
            {
                GeneratePackageList();
            }

            if (comboBarcode.SelectedItem as PaperSelectorItem != null)
            {
                SelectedPaper = ((PaperSelectorItem)comboBarcode.SelectedItem).SelectedPaper;
            }
            else if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<PaperSelectorItem>().Count() == 1
                && MatchPaper(hint, comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper) != null)
            {
                SelectedPaper = comboBarcode.ItemsSource.OfType<PaperSelectorItem>().First().SelectedPaper;
            }
            else
            {
                isUnknownMode = comboBarcode.ItemsSource == null ? true : comboBarcode.ItemsSource.OfType<PaperSelectorUnknownItem>().Count() > 0;
                SelectedPaper = null;
                isSelected = false;
            }

            PaperSelectorEnterEventArgs arg = RaiseEnterEvent(hint, isUnknownMode);
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

    class PaperSelectorUnknownItem
    {
        public PaperSelectorUnknownItem(string textTemplate, string value)
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

    class PaperSelectorItem
    {
        public PaperSelectorItem(bool byid, string value, Galant.DataEntity.Paper Paper)
        {
            selectedById = byid;
            selectedValue = value;
            selectedPaper = Paper;
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

        Galant.DataEntity.Paper selectedPaper;
        public Galant.DataEntity.Paper SelectedPaper
        {
            get { return selectedPaper; }
        }

        public override string ToString()
        {
            return SelectedValue;
        }
    }

    public class PaperSelectorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(container))
            {
                PaperSelectorItem s = item as PaperSelectorItem;
                if (s != null)
                {
                    if (s.SelectedById)
                    {
                        return ((ContentPresenter)container).FindResource("ItemById") as DataTemplate;
                    }
                    else
                    {
                        return ((ContentPresenter)container).FindResource("ItemByOriginName") as DataTemplate;
                    }
                }
                return ((ContentPresenter)container).FindResource("ItemUnknown") as DataTemplate;
            }
            return null;
        }
    }

    public class PaperSelectorEnterEventArgs : RoutedEventArgs
    {
        public PaperSelectorEnterEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public string EnteredText { get; set; }
        public bool IsUnknownMode { get; set; }
        public bool SoundPlayed { get; set; }
    }
}
