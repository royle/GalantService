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
using Xceed.Wpf.DataGrid;
using GLTWarter.Util;

namespace GLTWarter.ExternalData
{
    class ExcelXceedExporter : ExcelExporterBase
    {
        DataGridControl list;

        public ExcelXceedExporter(DataGridControl list)
        {
            if (list == null) throw new ArgumentNullException("list");

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
            }
        }

        public bool IsReady
        {
            get { return this.list != null; }
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = DeploymentSettings.Default.Locale;
            System.Threading.Thread.CurrentThread.CurrentCulture = DeploymentSettings.Default.Locale;

            if (string.IsNullOrEmpty(Filename) || !(this.list.ItemsSource is DataGridCollectionViewBase))
            {
                e.Cancel = true;
                return;
            }

            ApplicationClass app = null;
            Workbook wb = null;
            Worksheet ws = null;
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

                    ExcelXceedWriter writer = new ExcelXceedWriter(list, ws, 0, 0, new XceedFlattener(list));
                    writer.WriterProgress += new WriterProgressHandler(writer_WriterProgress);
                    writer.Process();

                    RaiseProgress(95);
                    ws.Columns.AutoFit();
                    wb.Save();
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            finally
            {
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

        void writer_WriterProgress(int progress)
        {
            RaiseProgress(progress);
        }
    }
}
