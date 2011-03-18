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
using System.Windows.Shapes;
using GLTWarter.Data;
using System.Globalization;

namespace GLTWarter.Printings
{
    public partial class LabelPaddingScreen : Window
    {
        LabelPaddingSettings settings = new LabelPaddingSettings();
        public LabelPaddingScreen()
        {
            settings.LoadFromSettings();
            this.DataContext = settings;

            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LabelPaddingScreen_Loaded);
        }

        void LabelPaddingScreen_Loaded(object sender, RoutedEventArgs e)
        {
            this.textLeftPadding.Focus();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (settings.IsValid)
            {
                settings.SaveToSettings();
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class LabelPaddingSettings : Galant.DataEntity.BaseData
    {
        public void LoadFromSettings()
        {
            this.LeftPadding = AppCurrent.Active.Printing.LabelLeftPadding.ToString("F2", CultureInfo.CurrentCulture);
            this.RightPadding = AppCurrent.Active.Printing.LabelRightPadding.ToString("F2", CultureInfo.CurrentCulture);
        }

        public void SaveToSettings()
        {
            AppCurrent.Active.Printing.LabelLeftPadding = double.Parse(LeftPadding, NumberStyles.Any, CultureInfo.CurrentCulture);
            AppCurrent.Active.Printing.LabelRightPadding = double.Parse(RightPadding, NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        string leftPadding;
        public string LeftPadding
        {
            get { return leftPadding; }
            set { leftPadding = value; OnPropertyChanged("LeftPadding"); }
        }

        string rightPadding;
        public string RightPadding
        {
            get { return rightPadding; }
            set { rightPadding = value; OnPropertyChanged("RightPadding"); }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            double test;
            switch (columnName)
            {
                case "LeftPadding":
                    if (!double.TryParse(LeftPadding, NumberStyles.Any, CultureInfo.CurrentCulture, out test) || test < 0)
                        return Resource.validationInvalidLabelPadding;
                    return string.Empty;
                case "RightPadding":
                    if (!double.TryParse(RightPadding, NumberStyles.Any, CultureInfo.CurrentCulture, out test) || test < 0)
                        return Resource.validationInvalidLabelPadding;
                    return string.Empty;
        }
            return null;
        }
    }
}
