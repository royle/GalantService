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
using Xceed.Wpf.DataGrid;
using System.Collections.ObjectModel;
using System.IO;
using GLTWarter.Styles;

namespace GLTWarter.ExternalData
{
    public partial class ExcelXceedMerge : Window
    {
        MergingContext context;
        DataGridControl target;
        public ExcelXceedMerge(DataGridControl target)
        {
            this.target = target;
            this.DataContext = context = new MergingContext(target);
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ExcelXceedMerge_Loaded);
        }

        void ExcelXceedMerge_Loaded(object sender, RoutedEventArgs e)
        {
            this.ButtonExcelBrowse.Focus();
        }

        private void ButtonExcelBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = Resource.msgFileDialogExcelFileFilter;
            dialog.ValidateNames = true;
            if (dialog.ShowDialog() == true)
            {
                context.DataExcel = dialog.FileName;
            }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (context.IsValid)
            {
                string mergeingKey = GetMergingKeyName(target);
                if (mergeingKey == null)
                    throw new ApplicationException("Merging key column is not found");

                DeploymentSettings.Default.MergingContextSelectedColumns = string.Join(",", context.SelectedColumns.ToArray());
                DeploymentSettings.Default.Save();

                AppCurrent.Active.MainScreen.ExportJob_ReportDoWork(new ExcelXceedMergeExporter(
                    target,
                    context.DataExcel,
                    mergeingKey,
                    context.SelectedColumns)
                    );
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GetMergingKeyName(DataGridControl target)
        {
            return (from c in target.Columns where ExportHint.GetMergingKey(c) != null select c.FieldName).FirstOrDefault();
        }
    }

    public class MergingContext : Galant.DataEntity.BaseData
    {
        List<MergingColumns> columns = new List<MergingColumns>();
        public MergingContext(DataGridControl target)
        {
            HashSet<string> selectedColumns = new HashSet<string>(DeploymentSettings.Default.MergingContextSelectedColumns.Split(',').ToArray());
            foreach (Column c in target.VisibleColumns)
            {
                if (!string.IsNullOrEmpty(c.Title as string))
                {
                    if (ExportHint.GetMergingKey(c) == null)
                        columns.Add(new MergingColumns()
                        {
                            Title = c.Title as string,
                            Name = c.FieldName,
                            IsSelected = selectedColumns.Contains(c.FieldName)
                        });
                }
            }
        }

        string dataExcel;
        public string DataExcel
        {
            get { return dataExcel; }
            set { dataExcel = value; OnPropertyChanged("DataExcel"); }
        }

        public ReadOnlyCollection<MergingColumns> Columns
        {
            get { return new ReadOnlyCollection<MergingColumns>(columns); }
            set { }
        }

        public IEnumerable<string> SelectedColumns
        {
            get { return (from c in Columns where c.IsSelected select c.Name); }
        }

        protected override string ValidateProperty(string columnName, Enum stage)
        {
            switch (columnName)
            {
                case "DataExcel":
                    if (string.IsNullOrEmpty(this.DataExcel) || !File.Exists(this.DataExcel)) return Resource.validationExportMergeDataExcelEmpty;
                    return string.Empty;
                case "Columns":
                    if (!Columns.Any(c => c.IsSelected)) return Resource.validationExportMergeColumnEmpty;
                    return string.Empty;
            }
            return null;
        }
    }

    public class MergingColumns
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}