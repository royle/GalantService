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

namespace GLTWarter.Pages.Assign
{
    /// <summary>
    /// Interaction logic for AssignCenter.xaml
    /// </summary>
    public partial class AssignCenter : DetailsBase
    {
        public AssignCenter(Galant.DataEntity.BaseData data):base(data)
        {
            InitializeComponent();
        }
    }
}
