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
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace GLTWarter.Pages.Order
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class OpForcedReturn : DetailsBase
    {
        public OpForcedReturn(Galant.DataEntity.Paper data)
            : base(data)
        {
            InitializeComponent();
        }

        protected override bool DataRefreshSuppressed
        {
            get { return true; }
        }

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override void FocusFirstControl()
        {
            textNote.Focus();
        }

    }
}
