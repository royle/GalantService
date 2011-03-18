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
    class ExcelXceedMergeExporter : ExcelExporterBase
    {
        DataGridControl list;
        string keyColumn;
        IEnumerable<string> columns;

        public ExcelXceedMergeExporter(DataGridControl list, string filename, string keyColumn, IEnumerable<string> columns)
        {
            if (list == null) throw new ArgumentNullException("list");
            this.list = list;
            this.Filename = filename;
            this.keyColumn = keyColumn;
            this.columns = columns;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = DeploymentSettings.Default.Locale;
            System.Threading.Thread.CurrentThread.CurrentCulture = DeploymentSettings.Default.Locale;
            
            ApplicationClass app = null;
            Workbook wb = null;
            Worksheet ws = null;
            try
            {
                using (new ExcelCultureBlock())
                {
                    app = new ApplicationClass();
                    app.DisplayAlerts = false;
                    wb = app.Workbooks.Open(Filename, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    ws = (Worksheet)wb.ActiveSheet;

                    RaiseProgress(10);

                    if (ws == null)
                        throw new InvalidOperationException(Resource.excelExportMergeWorksheetMissing);
                    if (((Range)ws.UsedRange).Rows.Count < 2)
                        throw new InvalidOperationException(Resource.excelExportMergeWorksheetInvalidFormat);

                    ExcelXceedMergeWriter writer = new ExcelXceedMergeWriter(list, ws, 0, 0, new XceedFlattener(list, keyColumn, columns));
                    writer.WriterProgress += new WriterProgressHandler(writer_WriterProgress);
                    writer.Process();

                    RaiseProgress(95);
                    wb.Save();
                    Filename = wb.FullName;
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
