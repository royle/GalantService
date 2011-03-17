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
    [ContentProperty("Presentation")]
    [DefaultProperty("Presentation")]
    public class FooterRow : System.Windows.Controls.Control
    {
        private const string ElementContent = "TMPL_CONTENT";
        ContentControl _contentContent;

        public FooterRow()
        {
        }

        public static readonly DependencyProperty PresentationProperty =
            DependencyProperty.Register("Presentation", typeof(object), typeof(FooterRow), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        

        public object Presentation
        {
            get { return (object)this.GetValue(PresentationProperty); }
            set { this.SetValue(PresentationProperty, value); }
        }

        public static void OurPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FooterRow)d).OnPropertyChanged(e.Property.Name);
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
