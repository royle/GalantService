using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
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

namespace GLTWarter.Controls
{
    public class PropertyRow : ContentControl
    {
        public PropertyRow()
        {
            this.Loaded += new RoutedEventHandler(PropertyRow_Loaded);
            this.LayoutUpdated += new EventHandler(PropertyRow_LayoutUpdated);
        }

        void PropertyRow_LayoutUpdated(object sender, EventArgs e)
        {
            ResetTarget();
            if (Target != null)
                this.LayoutUpdated -= new EventHandler(PropertyRow_LayoutUpdated);
        }

        void PropertyRow_Loaded(object sender, RoutedEventArgs e)
        {
            ResetTarget();
            if (Target != null)
                this.Loaded -= new RoutedEventHandler(PropertyRow_Loaded);
        }

        public static readonly DependencyProperty HeaderColumnWidthProperty =
            DependencyProperty.Register("HeaderColumnWidth", typeof(GridLength), typeof(PropertyRow), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(PropertyRow), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(Object), typeof(PropertyRow), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
       
        /// <summary>
        /// Find a focusable child and set the target (Target for AccessKey) accordingly
        /// </summary>
        void ResetTarget()
        {
            if (HasContent && Content is IInputElement)
            {
                if (Target == null)
                {
                    IInputElement element = Content as IInputElement;
                    if (element.Focusable)
                    {
                        Target = element;
                    }
                    else
                    {
                        Target = Utils.GetLeafFocusableChild(Content as IInputElement);
                    }
                }
            }
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ResetTarget();
        }
        
        public GridLength HeaderColumnWidth
        {
            get { return (GridLength)this.GetValue(HeaderColumnWidthProperty); }
            set { this.SetValue(HeaderColumnWidthProperty, value); }
        }

        public string HeaderText
        {
            get { return (string)this.GetValue(HeaderTextProperty); }
            set { this.SetValue(HeaderTextProperty, value); }
        }

        public Object Target
        {
            get { return (Object)this.GetValue(TargetProperty); }
            set { this.SetValue(TargetProperty, value); }
        }

        public static void OurPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyRow)d).OnPropertyChanged(e.Property.Name);
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
