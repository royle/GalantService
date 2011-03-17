using System;
using System.ComponentModel;
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
using Microsoft.Windows.Controls;

namespace GLTWarter.Controls
{
    [TemplatePart(Name = DateTimeBox.ElementDatePicker, Type = typeof(Microsoft.Windows.Controls.DatePicker))]
    [TemplatePart(Name = DateTimeBox.ElementHourTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = DateTimeBox.ElementMinTextBox, Type = typeof(TextBox))]
    public partial class DateTimeBox : UserControl
    {
        private const string ElementDatePicker = "PART_DatePicker";
        private const string ElementHourTextBox = "PART_HourTextBox";
        private const string ElementMinTextBox = "PART_MinTextBox";

        Microsoft.Windows.Controls.DatePicker _datePicker;
        TextBox _minTextBox;
        TextBox _hourTextBox;

        private IDictionary<DependencyProperty, bool> _isHandlerSuspended;

        public DateTimeBox()
        {
        }

        public override void OnApplyTemplate()
        {
            this.Focusable = true;
            this.PreviewGotKeyboardFocus += new KeyboardFocusChangedEventHandler(DateTimeBox_PreviewGotKeyboardFocus);

            _datePicker = GetTemplateChild(ElementDatePicker) as Microsoft.Windows.Controls.DatePicker;
            if (_datePicker != null)
            {
                Binding binding = new Binding("DateComponent");
                binding.Source = this;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                _datePicker.SetBinding(Microsoft.Windows.Controls.DatePicker.SelectedDateProperty, binding);
            }
            _hourTextBox = GetTemplateChild(ElementHourTextBox) as TextBox;
            if (_hourTextBox != null)
            {
                _hourTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(TextBox_GotKeyboardFocus);
                _hourTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(TextBox_LostKeyboardFocus);
                Binding binding = new Binding("HourComponent");
                binding.Source = this;
                binding.Converter = new Data.DoubleDigitsIntInputConverter();
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                _hourTextBox.SetBinding(TextBox.TextProperty, binding);
            }
            _minTextBox = GetTemplateChild(ElementMinTextBox) as TextBox;
            if (_minTextBox != null)
            {
                _minTextBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(TextBox_GotKeyboardFocus);
                _minTextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(TextBox_LostKeyboardFocus);

                Binding binding = new Binding("MinComponent");
                binding.Source = this;
                binding.Converter = new Data.DoubleDigitsIntInputConverter();
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                _minTextBox.SetBinding(TextBox.TextProperty, binding);
            }
        }

        void DateTimeBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource == sender)
            {
                if (e.NewFocus == this)
                {
                    if (e.OldFocus != _datePicker)
                    {
                        if (_datePicker != null) _datePicker.Focus();
                    }
                    else
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    }
                    e.Handled = true;
                }
            }
        }

        void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;
            int dummy;
            if (source.Text.Length == 1 && int.TryParse(source.Text, out dummy))
            {
                source.Text = source.Text.PadLeft(2, '0');
            }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime?), typeof(DateTimeBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TimeChanged), null));

        protected static readonly DependencyProperty DateComponentProperty =
            DependencyProperty.Register("DateComponent", typeof(DateTime?), typeof(DateTimeBox), new PropertyMetadata(null, new PropertyChangedCallback(OnComponentChanged)));
        protected static readonly DependencyProperty HourComponentProperty =
            DependencyProperty.Register("HourComponent", typeof(int?), typeof(DateTimeBox), new PropertyMetadata(null, new PropertyChangedCallback(OnComponentChanged), new CoerceValueCallback(OnCoerceHourComponent)));
        protected static readonly DependencyProperty MinComponentProperty =
            DependencyProperty.Register("MinComponent", typeof(int?), typeof(DateTimeBox), new PropertyMetadata(null, new PropertyChangedCallback(OnComponentChanged), new CoerceValueCallback(OnCoerceMinComponent)));
        
        private static void OnComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimeBox dp = d as DateTimeBox;
            if (!dp.IsHandlerSuspended(e.Property))
            {
                dp.UpdateTime();
            }
        }

        private static object OnCoerceHourComponent(DependencyObject d, object baseValue)
        {
            int? value = (int?)baseValue;
            if (value.HasValue && !(value.Value >= 0 && value.Value <= 23))
            {
                return null;
            }
            return baseValue;
        }

        private static object OnCoerceMinComponent(DependencyObject d, object baseValue)
        {
            int? value = (int?)baseValue;
            if (value.HasValue && !(value.Value >= 0 && value.Value <= 59))
            {
                return null;
            }
            return baseValue;
        }

        public DateTime? Time
        {
            get { return (DateTime?)this.GetValue(TimeProperty); }
            set { this.SetValue(TimeProperty, value); }
        }

        public static void TimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimeBox dp = d as DateTimeBox;
            if (!dp.IsHandlerSuspended(TimeProperty))
            {
                try
                {
                    dp.SetIsHandlerSuspended(DateComponentProperty, true);
                    dp.SetIsHandlerSuspended(HourComponentProperty, true);
                    dp.SetIsHandlerSuspended(MinComponentProperty, true);
                    if (e.NewValue == null)
                    {
                        dp.DateComponent = null;
                        dp.HourComponent = null;
                        dp.MinComponent = null;
                    }
                    else
                    {
                        DateTime combined = (e.NewValue as DateTime?).Value;
                        dp.DateComponent = combined.Date;
                        dp.HourComponent = combined.Hour;
                        dp.MinComponent = combined.Minute;
                    }
                }
                finally
                {
                    dp.SetIsHandlerSuspended(DateComponentProperty, false);
                    dp.SetIsHandlerSuspended(HourComponentProperty, false);
                    dp.SetIsHandlerSuspended(MinComponentProperty, false);
                }
            }
        }

        void UpdateTime()
        {
            SetIsHandlerSuspended(TimeProperty, true);
            try
            {
                if (!(DateComponent.HasValue && HourComponent.HasValue && MinComponent.HasValue))
                {
                    Time = null;
                }
                else
                {
                    DateTime combined = DateComponent.Value.Date;
                    combined = combined.Add(new TimeSpan(HourComponent.Value, MinComponent.Value, 0));
                    Time = combined;
                }
            }
            finally
            {
                SetIsHandlerSuspended(TimeProperty, false);
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.KeyboardDevice != null)
            {
                ((TextBox)e.Source).SelectAll();
            }
        }

        public DateTime? DateComponent
        {
            get { return (DateTime?)this.GetValue(DateComponentProperty); }
            set { this.SetValue(DateComponentProperty, value); }
        }

        public int? HourComponent
        {
            get { return (int?)this.GetValue(HourComponentProperty); }
            set { this.SetValue(HourComponentProperty, value); }
        }

        public int? MinComponent
        {
            get { return (int?)this.GetValue(MinComponentProperty); }
            set { this.SetValue(MinComponentProperty, value); }
        }

        private bool IsHandlerSuspended(DependencyProperty property)
        {
            return _isHandlerSuspended != null && _isHandlerSuspended.ContainsKey(property);
        }

        private void SetIsHandlerSuspended(DependencyProperty property, bool value)
        {
            if (value)
            {
                if (_isHandlerSuspended == null)
                {
                    _isHandlerSuspended = new Dictionary<DependencyProperty, bool>(2);
                }

                _isHandlerSuspended[property] = true;
            }
            else
            {
                if (_isHandlerSuspended != null)
                {
                    _isHandlerSuspended.Remove(property);
                }
            }
        }
    }
}
