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
    public delegate void EntitySelectorEnterEventHandler(object sender, EntitySelectorEnterEventArgs e);

    /// <summary>
    /// Interaction logic for WaitingIcon.xaml
    /// </summary>
    public partial class EntitySelector : UserControl
    {
        private const int MAX_HINTS_COUNT = 10;

        public EntitySelector()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(EntitySelector_Loaded);
        }

        void EntitySelector_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(EntitySelector_Loaded);
            comboBarcode.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(comboBarcode_TextChanged), true);
            comboBarcode.AddHandler(TextBox.PreviewTextInputEvent, new TextCompositionEventHandler(comboBarcode_PreviewTextInput), true);
            comboBarcode.AddHandler(ComboBox.PreviewKeyDownEvent, new KeyEventHandler(comboBarcode_KeyDown), true);
            comboBarcode.AddHandler(ComboBox.LostFocusEvent, new RoutedEventHandler(comboBarcode_LostFocus), true);
            GenerateEntityList();
        }

        void comboBarcode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (comboBarcode.SelectedItem != null && (comboBarcode.SelectedItem as EntitySelectorItem) != null)
            {
                if (this.SelectedEntity == null || !this.SelectedEntity.EntityEquals((comboBarcode.SelectedItem as EntitySelectorItem).SelectedEntity))
                    this.buttonEnter_Click(sender, e);
            }
        }

        

        ComboBox comboBarcode;
        Button buttonEnter;

        public static readonly DependencyProperty EntitysListProperty = 
            DependencyProperty.Register("EntitysList", typeof(System.Collections.IList), typeof(EntitySelector), new PropertyMetadata(new PropertyChangedCallback(EntitysListChanged)));
        public System.Collections.IList EntitysList
        {
            get { return (System.Collections.IList)this.GetValue(EntitysListProperty); }
            set { this.SetValue(EntitysListProperty, value); }
        }
        public static void EntitysListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EntitySelector)d).GenerateEntityList();
        }

        public static readonly DependencyProperty SelectedEntityProperty =
            DependencyProperty.Register("SelectedEntity", typeof(Galant.DataEntity.Entity), typeof(EntitySelector), new PropertyMetadata(new PropertyChangedCallback(SelectedEntityChanged)));
        public Galant.DataEntity.Entity SelectedEntity
        {
            get { return (Galant.DataEntity.Entity)this.GetValue(SelectedEntityProperty); }
            set
            {
                this.SetValue(SelectedEntityProperty, value);
                if (!string.IsNullOrEmpty(comboBarcode.Text) && value == null && this.SelectedEntity == null)
                    SelectedEntityChanged();
            }
        }

        void SelectedEntityChanged()
        {
            comboBarcode.SelectedItem = null;
            comboBarcode.Text = string.Empty;
            GenerateEntityList();
        }

        public static void SelectedEntityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                ((EntitySelector)d).SelectedEntityChanged();
            }
        }

        public static readonly RoutedEvent EnterEvent = EventManager.RegisterRoutedEvent("Enter", RoutingStrategy.Bubble, typeof(EntitySelectorEnterEventHandler), typeof(EntitySelector));
        public event RoutedEventHandler Enter
        {
            add { AddHandler(EnterEvent, value); }
            remove { RemoveHandler(EnterEvent, value); }
        }

        EntitySelectorEnterEventArgs RaiseEnterEvent(string enteredText, bool isUnknownMode)
        {
            EntitySelectorEnterEventArgs arg = new EntitySelectorEnterEventArgs(EnterEvent, this)
            {
                EnteredText = enteredText,
                IsUnknownMode = isUnknownMode,
                SoundPlayed = false
            };
            RaiseEvent(arg);
            return arg;
        }

        /// <summary>
        /// The text should be displayed when no entitys were found in the stored list
        /// </summary>
        public static readonly DependencyProperty UnknownTextProperty =
            DependencyProperty.Register("UnknownText", typeof(string), typeof(EntitySelector), new PropertyMetadata(null));
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
                GenerateEntityList();
            }

            string hint = comboBarcode.Text.Trim();
            if (comboBarcode.SelectedItem as EntitySelectorItem != null) SelectedEntity = ((EntitySelectorItem)comboBarcode.SelectedItem).SelectedEntity;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<EntitySelectorItem>().Count() == 1
                && MatchEntity(hint, comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity) != null)
                SelectedEntity = comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity;
        }

        void GenerateEntityList()
        {
            if (comboBarcode == null) return;

            string hint = comboBarcode.Text;
            List<object> ret = new List<object>();
            if (EntitysList != null)
            {
                if (!string.IsNullOrEmpty(hint) && hint.Trim().Length > 0)
                {
                    hint = hint.Trim();
                    foreach (Galant.DataEntity.Entity en in EntitysList)
                    {
                        EntitySelectorItem si = MatchEntity(hint, en);
                        if (si != null) ret.Add(si);
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                else
                {
                    foreach (Galant.DataEntity.Entity en in EntitysList)
                    {

                        ret.Add(new EntitySelectorItem(false,true, en.Alias, en));
                        if (ret.Count >= MAX_HINTS_COUNT) break;
                    }
                }
                if (!string.IsNullOrEmpty(this.UnknownText) && ret.Count == 0)
                {
                    if (string.IsNullOrEmpty(Data.Validators.IsValidBarcode(hint)))
                    {
                        ret.Add(new EntitySelectorUnknownItem(this.UnknownText, hint));
                    }
                }
                comboBarcode.ItemsSource = ret;
            }
        }

        private EntitySelectorItem MatchEntity(string hint, Galant.DataEntity.Entity en)
        {
            if (en == null || string.IsNullOrEmpty(hint)) return null;
            if (en.AbleFlag && !string.IsNullOrEmpty(en.Alias) && en.Alias.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return new EntitySelectorItem(true,false, en.Alias, en);
            }
            if (en.AbleFlag && !string.IsNullOrEmpty(en.FullName) && en.FullName.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                return new EntitySelectorItem(false,true, en.FullName, en);
            }
            if (en.Phones!= null && en.Phones.Count>0)
            {
                foreach (string orf in en.Phones)
                {
                    if (orf.IndexOf(hint, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        return new EntitySelectorItem(false, false, orf, en);
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
            
            if (comboBarcode.SelectedItem as EntitySelectorItem != null) SelectedEntity = ((EntitySelectorItem)comboBarcode.SelectedItem).SelectedEntity;
            if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<EntitySelectorItem>().Count() == 1
                && MatchEntity(hint, comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity) != null)
                SelectedEntity = comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity;

            if (SelectedEntity == null || MatchEntity(hint, SelectedEntity) == null)
            {
                GenerateEntityList();
            }

            if (comboBarcode.SelectedItem as EntitySelectorItem != null)
            {
                SelectedEntity = ((EntitySelectorItem)comboBarcode.SelectedItem).SelectedEntity;
            }
            else if (comboBarcode.ItemsSource != null && comboBarcode.ItemsSource.OfType<EntitySelectorItem>().Count() == 1
                && MatchEntity(hint, comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity) != null)
            {
                SelectedEntity = comboBarcode.ItemsSource.OfType<EntitySelectorItem>().First().SelectedEntity;
            }
            else
            {
                isUnknownMode = comboBarcode.ItemsSource == null ? true : comboBarcode.ItemsSource.OfType<EntitySelectorUnknownItem>().Count() > 0;
                SelectedEntity = null;
                isSelected = false;
            }

            EntitySelectorEnterEventArgs arg = RaiseEnterEvent(hint, isUnknownMode);
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

    class EntitySelectorUnknownItem
    {
        public EntitySelectorUnknownItem(string textTemplate, string value)
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

    class EntitySelectorItem
    {
        public EntitySelectorItem(bool byid,bool byName, string value, Galant.DataEntity.Entity entity)
        {
            selectedById = byid;
            selectedValue = value;
            selectedEntity = entity;
            selectedByName = byName;
        }

        bool selectedById;
        public bool SelectedById
        {
            get { return selectedById; }
        }

        bool selectedByName;
        public bool SelectedByName
        {
            get { return selectedById; }
        }

        string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
        }

        Galant.DataEntity.Entity selectedEntity;
        public Galant.DataEntity.Entity SelectedEntity
        {
            get { return selectedEntity; }
        }

        public override string ToString()
        {
            return SelectedValue;
        }
    }

    public class EntitySelectorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(container))
            {
                EntitySelectorItem s = item as EntitySelectorItem;
                if (s != null)
                {
                    if (s.SelectedById)
                    {
                        return ((ContentPresenter)container).FindResource("ItemById") as DataTemplate;
                    }
                    else if (s.SelectedByName)
                    {
                        return ((ContentPresenter)container).FindResource("ItemByName") as DataTemplate;
                    }
                    else
                    {
                        return ((ContentPresenter)container).FindResource("ItemByPhoneNo") as DataTemplate;
                    }
                }
                return ((ContentPresenter)container).FindResource("ItemUnknown") as DataTemplate;
            }
            return null;
        }
    }

    public class EntitySelectorEnterEventArgs : RoutedEventArgs
    {
        public EntitySelectorEnterEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        public string EnteredText { get; set; }
        public bool IsUnknownMode { get; set; }
        public bool SoundPlayed { get; set; }
    }
}
