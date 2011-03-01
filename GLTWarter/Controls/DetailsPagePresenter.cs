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
    public partial class DetailsPagePresenter : System.Windows.Controls.Control
    {
        public DetailsPagePresenter()
        {
            LatestVersionButtonsPanel = new List<UIElement>();
            OldVersionButtonsPanel = new List<UIElement>();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        public static readonly DependencyProperty TitleForNewProperty =
            DependencyProperty.Register("TitleForNew", typeof(string), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty TitleForEditProperty =
            DependencyProperty.Register("TitleForEdit", typeof(string), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty TitleBrushProperty =
            DependencyProperty.Register("TitleBrush", typeof(Brush), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty ShowTopLoadingBarProperty =
            DependencyProperty.Register("ShowTopLoadingBar", typeof(bool), typeof(DetailsPagePresenter), new PropertyMetadata(true, new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty LatestVersionButtonsPanelProperty =
            DependencyProperty.Register("LatestVersionButtonsPanel", typeof(List<UIElement>), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty OldVersionButtonsPanelProperty =
            DependencyProperty.Register("OldVersionButtonsPanel", typeof(List<UIElement>), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));
        public static readonly DependencyProperty PresentationProperty =
            DependencyProperty.Register("Presentation", typeof(object), typeof(DetailsPagePresenter), new PropertyMetadata(new PropertyChangedCallback(OurPropertyChanged)));

        public string TitleForNew
        {
            get { return (string)this.GetValue(TitleForNewProperty); }
            set { this.SetValue(TitleForNewProperty, value); }
        }
        public string TitleForEdit
        {
            get { return (string)this.GetValue(TitleForEditProperty); }
            set { this.SetValue(TitleForEditProperty, value); }
        }
        public Brush TitleBrush
        {
            get { return (Brush)this.GetValue(TitleBrushProperty); }
            set { this.SetValue(TitleBrushProperty, value); }
        }
        public bool ShowTopLoadingBar
        {
            get { return (bool)this.GetValue(ShowTopLoadingBarProperty); }
            set { this.SetValue(ShowTopLoadingBarProperty, value); }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<UIElement> LatestVersionButtonsPanel
        {
            get { return (List<UIElement>)this.GetValue(LatestVersionButtonsPanelProperty); }
            set { this.SetValue(LatestVersionButtonsPanelProperty, value); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<UIElement> OldVersionButtonsPanel
        {
            get { return (List<UIElement>)this.GetValue(OldVersionButtonsPanelProperty); }
            set { this.SetValue(OldVersionButtonsPanelProperty, value); }
        }

        public object Presentation
        {
            get { return (object)this.GetValue(PresentationProperty); }
            set { this.SetValue(PresentationProperty, value); }
        }

        public static void OurPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DetailsPagePresenter)d).OnPropertyChanged(e.Property.Name);
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
