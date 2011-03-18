using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
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
using System.Windows.Threading;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using GLTWarter.Tools;
using GLTWarter.Util;

namespace GLTWarter.ExternalData
{
    class ExcelExporter : ExcelExporterBase
    {
        const string ACCOUNT_NUMBER_FORMAT = "_([${0}] * #,##0.00_);_([${0}] * (#,##0.00);_([${0}] * 0.00_);_(@_)";

        class BindingReader : FrameworkElement
        {
            public static readonly DependencyProperty ObjectProperty = DependencyProperty.Register(
                "Object", typeof(Object), typeof(BindingReader)
                );

            public object Object
            {
                get { return (object)this.GetValue(ObjectProperty); }
            }            
        }

        class ColumnProcessing
        {
            public string Header;
            public BindingBase Bindings;
            public DataTemplate Template;
            public DataTemplateSelector TemplateSelector;
            public bool IsAddress;
        }

        class CellData
        {
            public Object[] datum;
            public string[] format;
        }

        class VisualProcessingWorkItem : TemplateStringExtractorWorkItem
        {
            public int ExcelRow;
            public int Column;
        }
        
        ListView list;
        ColumnProcessing[] columns;
        ICollectionView view;

        public ExcelExporter(ListView list)
        {
            if (list == null) throw new ArgumentNullException("list");

            if (list.View as GridView != null)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                dialog.Filter = Resource.msgFileDialogExcelFileFilter;
                dialog.ValidateNames = true;
                if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
                {
                    int extLength = System.IO.Path.GetExtension(dialog.FileName).Length;
                    Filename = dialog.FileName.Substring(0, dialog.FileName.Length - extLength);

                    this.list = list;
                    view = CollectionViewSource.GetDefaultView(list.ItemsSource);

                    GridView gView = (GridView)list.View;
                    ScanColumn(gView);
                }
            }
        }
        
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = DeploymentSettings.Default.Locale;
            System.Threading.Thread.CurrentThread.CurrentCulture = DeploymentSettings.Default.Locale;

            if (!string.IsNullOrEmpty(Filename))
            {
                int pendingResult = 0;
                ApplicationClass app = null;
                Thread proc = null;
                Workbook wb = null;
                Worksheet ws = null;

                TemplateStringExtractor extractor = new TemplateStringExtractor();

                try
                {
                    using (new ExcelCultureBlock())
                    {
                        app = new ApplicationClass();
                        app.DisplayAlerts = false;
                        wb = app.Workbooks.Add(Missing.Value);
                        ws = (Worksheet)wb.Worksheets[1];
                        for (int i = 1; i <= wb.Worksheets.Count; i++)
                        {
                            if (wb.Worksheets[i] != ws)
                            {
                                ((Worksheet)wb.Worksheets[i]).Delete();
                                i--;
                            }
                        }
                        RaiseProgress(5);
                        wb.SaveAs(Filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        Filename = wb.FullName;
                        RaiseProgress(10);

                        bool[] isColumnsFormatted = new bool[columns.Length];
                        List<CellData[]> cell_datas = new List<CellData[]>();
                        Range range;

                        int jExcel = 1;
                        int jExcelStep;

                        Thread staWorker = new Thread(delegate(object content)
                        {
                            BindingReader reader = new BindingReader();
                            for (view.MoveCurrentToFirst(); view.CurrentItem != null; jExcel += jExcelStep, view.MoveCurrentToNext())
                            {
                                CellData[] row_data = new CellData[columns.Length];
                                cell_datas.Add(row_data);

                                reader.DataContext = view.CurrentItem;
                                jExcelStep = 1;
                                for (int i = 0; i < columns.Length; i++)
                                {
                                    CellData cell = row_data[i] = new CellData();
                                    if (columns[i].Bindings != null)
                                    {
                                        reader.SetBinding(BindingReader.ObjectProperty, columns[i].Bindings);
                                        if (reader.Object != null)
                                        {
                                            if (reader.Object is DateTime?)
                                            {
                                                cell.datum = new object[1] { ((DateTime?)reader.Object).Value };
                                                cell.format = new string[1] { Resource.exportDateFormat };
                                            }
                                            else if (reader.Object is DateTime)
                                            {
                                                cell.datum = new object[1] { ((DateTime)reader.Object) };
                                                cell.format = new string[1] { Resource.exportDateFormat };
                                            }
                                            else
                                            {
                                                cell.datum = new object[1] { reader.Object.ToString() };
                                                cell.format = new string[1] { "@" };
                                            }
                                        }
                                    }
                                    else if (columns[i].Template != null || columns[i].TemplateSelector != null)
                                    {
                                        DataTemplate dt;
                                        if (columns[i].Template != null)
                                        {
                                            dt = columns[i].Template;
                                        }
                                        else
                                        {
                                            dt = columns[i].TemplateSelector.SelectTemplate(view.CurrentItem, null);
                                        }

                                        HostVisual hostVisual = new HostVisual();
                                        VisualProcessingWorkItem wi = new VisualProcessingWorkItem();
                                        wi.Column = i;
                                        wi.ExcelRow = jExcel;
                                        wi.Template = dt;
                                        wi.Context = view.CurrentItem;
                                        wi.Host = hostVisual;

                                        pendingResult++;
                                        extractor.QueueWorkItem(wi);
                                    }
                                }
                            }
                        });

                        staWorker.SetApartmentState(ApartmentState.STA);
                        staWorker.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                        staWorker.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                        staWorker.Start();
                        staWorker.Join();

                        extractor.SignalAllWorkItemsQueued();
                        RaiseProgress(30);

                        proc = new Thread(extractor.Run);
                        proc.SetApartmentState(ApartmentState.STA);
                        proc.Start();

                        object[,] cell_data_array = new object[jExcel, columns.Length];
                        string[,] cell_format_array = new string[jExcel, columns.Length];
                        int r = 1;
                        foreach (CellData[] row_data in cell_datas)
                        {
                            jExcelStep = 1;
                            for (int i = columns.Length - 1; i >= 0; i--)
                            {
                                if (row_data[i] != null && row_data[i].datum != null)
                                {
                                    for (int k = row_data[i].datum.Length - 1; k >= 0; k--)
                                    {
                                        cell_data_array[r + k, i] = row_data[i].datum[k];
                                        cell_format_array[r + k, i] = row_data[i].format[k];
                                    }
                                }
                            }
                            r += jExcelStep;
                        }

                        lock (extractor.ResultQueue)
                        {
                            while (pendingResult > 0)
                            {
                                if (extractor.ResultQueue.Count == 0)
                                {
                                    Monitor.Wait(extractor.ResultQueue);
                                }
                                VisualProcessingWorkItem wi = extractor.ResultQueue.Dequeue() as VisualProcessingWorkItem;
                                cell_data_array[wi.ExcelRow, wi.Column] = wi.Result;
                                cell_format_array[wi.ExcelRow, wi.Column] = "@";

                                pendingResult--;
                            }
                        }
                        RaiseProgress(50);

                        range = (Range)ws.get_Range("1:1", Missing.Value);
                        range.NumberFormat = "@";
                        for (int i = 0; i < columns.Length; i++)
                        {
                            cell_data_array[0, i] = columns[i].Header;

                        }

                        if (jExcel > 1)
                        {
                            for (int i = 0; i < columns.Length; i++)
                            {

                                Dictionary<string, int> formats = new Dictionary<string, int>();
                                for (int j = 1; j < jExcel; j++)
                                {
                                    if (!string.IsNullOrEmpty(cell_format_array[j, i]))
                                    {
                                        if (formats.ContainsKey(cell_format_array[j, i]))
                                        {
                                            formats[cell_format_array[j, i]]++;
                                        }
                                        else
                                        {
                                            formats[cell_format_array[j, i]] = 1;
                                        }
                                    }
                                }
                                string maxString = (from f in formats where f.Value == formats.Max(y => y.Value) select f.Key).FirstOrDefault();

                                range = (Range)ws.get_Range("A1", Missing.Value);
                                range = range.get_Offset(1, i).get_Resize(jExcel - 1, 1);
                                range.NumberFormat = maxString;

                                for (int j = 1; j < jExcel; j++)
                                {
                                    if (cell_format_array[j, i] != maxString)
                                    {
                                        range = (Range)ws.get_Range("A1", Missing.Value);
                                        range = range.get_Offset(j, i);
                                        range.NumberFormat = cell_format_array[j, i];
                                    }
                                }
                            }

                            range = (Range)ws.get_Range("A1", Missing.Value);
                            range = range.get_Resize(jExcel, columns.Length);
                            range.Value2 = cell_data_array;
                        }
                        RaiseProgress(80);
                        ws.Columns.AutoFit();
                        wb.Save();
                    }
                }
                catch (System.Runtime.InteropServices.COMException ex)
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }
                finally
                {
                    if (proc != null)
                    {
                        extractor.SignalAllWorkItemsQueued();
                        proc.Join();
                    }
                    wb = null; ws = null;
                    if (app != null)
                    {
                        app.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ScanColumn(GridView gView)
        {
            columns = new ColumnProcessing[gView.Columns.Count];
            for (int i = 0; i < gView.Columns.Count; i++)
            {
                columns[i] = new ColumnProcessing();
                columns[i].Header = gView.Columns[i].Header as String;
                columns[i].IsAddress = Styles.ExportHint.GetIsAddress(gView.Columns[i]);

                BindingExpression exp = Styles.ExportHint.GetBinding(gView.Columns[i]);
                if (exp != null)
                {
                    columns[i].Bindings = exp.ParentBindingBase;
                }
                else if (gView.Columns[i].DisplayMemberBinding != null)
                {
                    columns[i].Bindings = gView.Columns[i].DisplayMemberBinding;
                }
                else if (gView.Columns[i].CellTemplate != null)
                {
                    columns[i].Template = gView.Columns[i].CellTemplate;
                }
                else if (gView.Columns[i].CellTemplateSelector != null)
                {
                    columns[i].TemplateSelector = gView.Columns[i].CellTemplateSelector;
                }
                else
                {
                    throw new InvalidOperationException("The supplied GridView has columns with no valid data source");
                }
            }
        }
    }
}
