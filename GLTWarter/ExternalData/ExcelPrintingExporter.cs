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
using System.Windows.Markup;
using System.IO;
using GLTWarter.Styles;

namespace GLTWarter.ExternalData
{
    /// <summary>
    /// Export an printing template to Excel
    /// </summary>
    class ExcelPrintingExporter : ExcelExporterBase
    {
        class PrintingWorkItem : TemplateStringExtractorWorkItem
        {
            public int Row;
            public int Column;
            public int RowSpan;
            public int ColumnSpan;
        }

        Object context;
        string uriTemplate;

        public ExcelPrintingExporter(string uriTemplate,Object context)
        {
            if (uriTemplate == null) throw new ArgumentNullException("uriTemplate");
            if (context == null) throw new ArgumentNullException("context");

            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.AddExtension = true;
            dialog.OverwritePrompt = true;
            dialog.Filter = Resource.msgFileDialogExcelFileFilter;
            dialog.ValidateNames = true;
            if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                int extLength = System.IO.Path.GetExtension(dialog.FileName).Length;
                Filename = dialog.FileName.Substring(0, dialog.FileName.Length - extLength);
                this.uriTemplate = uriTemplate;
                this.context = context;
            }
        }

        public bool IsReady
        {
            get { return this.uriTemplate != null; }
        }

        Exception workException = null;
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(Filename))
            {
                e.Cancel = true;
                return;
            }

            Thread thread = new Thread(DoRealWork);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (workException != null)
            {
                throw workException;
            }
        }

        void DoRealWork()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = DeploymentSettings.Default.Locale;
                System.Threading.Thread.CurrentThread.CurrentCulture = DeploymentSettings.Default.Locale;

                FlowDocument doc = null;
                try
                {
                    string templateAbsolutePath = Utils.GetExternalFile(uriTemplate);
                    if (!String.IsNullOrEmpty(templateAbsolutePath) && File.Exists(templateAbsolutePath))
                    {
                        using (FileStream fs = File.OpenRead(templateAbsolutePath))
                        {
                            doc = (FlowDocument)XamlReader.Load(fs);
                        }
                    }
                }
                catch (IOException ex)
                {
                    throw;
                }
                catch (UnauthorizedAccessException ex)
                {
                    throw;
                }
                catch (XamlParseException ex)
                {
                    throw;
                }
                if (doc == null)
                {
                    try
                    {
                        doc = (FlowDocument)System.Windows.Application.LoadComponent(new Uri(uriTemplate, UriKind.Relative));
                    }
                    catch (XamlParseException ex)
                    {
                        throw;
                    }
                }

                if (doc == null)
                    return;

                try
                {
                    doc.DataContext = context;
                }
                catch (InvalidOperationException) { }

                // This is necessary to pump the message pipe such that binding are rendered in the Xceed control.
                doc.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.DataBind,
                    (Action)delegate() { });

                ApplicationClass app = null;
                Workbook wb = null;
                Worksheet ws = null;
                Thread proc = null;
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

                        int lastRow = 0;

                        proc = new Thread(extractor.Run);
                        proc.SetApartmentState(ApartmentState.STA);
                        proc.Start();

                        Block blockPageHeader = null;
                        Block block = doc.Blocks.FirstBlock;
                        while (block != null)
                        {
                            if (block is BlockUIContainer && block.Name == "ReportPageHeader")
                            {
                                blockPageHeader = block;
                            } else if (block is Section)
                            {
                                Section sect = block as Section;
                                if (!string.IsNullOrEmpty(sect.Tag as string ?? sect.Name) &&
                                    doc.FindResource(sect.Tag as string ?? sect.Name) as DataTemplate != null)
                                {
                                    DataTemplate t = doc.FindResource(sect.Tag ?? sect.Name) as DataTemplate;
                                    if (sect.DataContext is DataGridControl)
                                    {
                                        ExcelXceedWriter writer = new ExcelXceedWriter(sect.DataContext as DataGridControl, ws, lastRow, 0, new XceedFlattener(sect.DataContext as DataGridControl));
                                        writer.DrawHorizontalHairline = true;
                                        writer.Process();
                                        lastRow += writer.RowsWritten;
                                    }
                                }                                
                            } else if (block is BlockUIContainer && ReportHint.GetForExport(block))
                            {
                                DependencyObject root = (block as BlockUIContainer).Child;
                                int minColumn = int.MaxValue;
                                int maxColumn = 0;
                                int newLastRow = 0;
                                var textblocks = Utils.FindVisualChildren<TextBlock>(root);
                                var contents = Utils.FindVisualChildren<ContentControl>(root);
                                foreach (TextBlock tb in textblocks)
                                {
                                    if (tb.Visibility != Visibility.Visible)
                                        continue;
                                    string value = TemplateStringExtractor.ExtractStringFromTextBlock(tb);
                                    Range r = ((Range)ws.Cells[ReportHint.GetRow(tb) + lastRow + 1, ReportHint.GetColumn(tb) + 1])
                                        .get_Resize(ReportHint.GetRowSpan(tb), ReportHint.GetColumnSpan(tb));
                                    r.Merge(false);
                                    r.Value2 = value;
                                    r.NumberFormat = "@";
                                    AssignExcelCellStyle(r, ReportHint.GetFontSize(tb), tb);
                                    newLastRow = Math.Max(newLastRow, ReportHint.GetRow(tb) + ReportHint.GetRowSpan(tb));
                                    minColumn = Math.Min(minColumn, ReportHint.GetColumn(tb));
                                    maxColumn = Math.Max(maxColumn, ReportHint.GetColumn(tb) + ReportHint.GetColumnSpan(tb));
                                }
                                foreach (ContentControl cc in contents)
                                {
                                    if (cc.Visibility != Visibility.Visible)
                                        continue;
                                    if (cc.ContentTemplate != null || cc.ContentTemplateSelector != null)
                                    {
                                        DataTemplate dt;
                                        if (cc.Template != null)
                                        {
                                            dt = cc.ContentTemplate;
                                        }
                                        else
                                        {
                                            dt = cc.ContentTemplateSelector.SelectTemplate(cc.Content, null);
                                        }

                                        if (dt != null)
                                        {
                                            HostVisual hostVisual = new HostVisual();
                                            PrintingWorkItem wi = new PrintingWorkItem();
                                            wi.Row = ReportHint.GetRow(cc) + lastRow;
                                            wi.Column = ReportHint.GetColumn(cc);
                                            wi.RowSpan = ReportHint.GetRowSpan(cc);
                                            wi.ColumnSpan = ReportHint.GetColumnSpan(cc);
                                            wi.Template = dt;
                                            wi.Context = cc.Content;
                                            wi.Host = hostVisual;

                                            extractor.QueueWorkItem(wi);
                                            Range r = ((Range)ws.Cells[ReportHint.GetRow(cc) + lastRow + 1, ReportHint.GetColumn(cc) + 1])
                                                .get_Resize(ReportHint.GetRowSpan(cc), ReportHint.GetColumnSpan(cc));
                                            r.Merge(false);
                                            AssignExcelCellStyle(r, ReportHint.GetFontSize(cc), cc);
                                        }
                                    }
                                    newLastRow = Math.Max(newLastRow, ReportHint.GetRow(cc) + ReportHint.GetRowSpan(cc));
                                    minColumn = Math.Min(minColumn, ReportHint.GetColumn(cc));
                                    maxColumn = Math.Max(maxColumn, ReportHint.GetColumn(cc) + ReportHint.GetColumnSpan(cc));
                                }

                                Thickness borderThickness = ReportHint.GetBorderThickness(block);
                                AssignExcelBorder(
                                    ((Range)ws.Cells[lastRow + 1, minColumn + 1]).get_Resize(1, maxColumn - minColumn).Borders[XlBordersIndex.xlEdgeTop],
                                    borderThickness.Top);
                                AssignExcelBorder(
                                    ((Range)ws.Cells[lastRow + newLastRow, minColumn + 1]).get_Resize(1, maxColumn - minColumn).Borders[XlBordersIndex.xlEdgeBottom],
                                    borderThickness.Bottom);
                                lastRow += newLastRow;
                            }
                            block = block.NextBlock;
                        }
                        extractor.SignalAllWorkItemsQueued();

                        lock (extractor.ResultQueue)
                        {
                            while (!extractor.IsDone || extractor.ResultQueue.Count > 0)
                            {
                                if (extractor.ResultQueue.Count == 0)
                                {
                                    doc.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.ContextIdle,
                                        (Action)delegate(){ });
                                    Monitor.Wait(extractor.ResultQueue);
                                    if (extractor.ResultQueue.Count == 0)
                                        break;
                                }
                                PrintingWorkItem wi = extractor.ResultQueue.Dequeue() as PrintingWorkItem;
                                Range r = ((Range)ws.Cells[wi.Row + 1, wi.Column + 1]);
                                r.Value2 = wi.Result;
                                r.NumberFormat = "@";
                            }
                        }

                        ExcelPageSetup(ws);
                        if (blockPageHeader != null)
                            AssignExcelPageHeader(ws, (blockPageHeader as BlockUIContainer).Child);

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
                    if (proc != null)
                    {
                        extractor.SignalAllWorkItemsQueued();
                        proc.Join(TimeSpan.FromSeconds(5));
                    }
                }
            }
            catch (Exception ex)
            {
                workException = ex;
            }
        }

        void AssignExcelCellStyle(Microsoft.Office.Interop.Excel.Range range, double fontSize, FrameworkElement element)
        {
            if (fontSize >= 1)
            {
                range.Font.Size = fontSize;
            }
            switch (element.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    range.HorizontalAlignment = XlHAlign.xlHAlignLeft; break;
                case HorizontalAlignment.Right:
                    range.HorizontalAlignment = XlHAlign.xlHAlignRight; break;
                case HorizontalAlignment.Center:
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter; break;
                case HorizontalAlignment.Stretch:
                    range.HorizontalAlignment = XlHAlign.xlHAlignJustify; break;
            }
            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    range.VerticalAlignment = XlVAlign.xlVAlignTop; break;
                case VerticalAlignment.Bottom:
                    range.VerticalAlignment = XlVAlign.xlVAlignBottom; break;
                case VerticalAlignment.Center:
                    range.VerticalAlignment = XlVAlign.xlVAlignCenter; break;
                case VerticalAlignment.Stretch:
                    range.VerticalAlignment = XlVAlign.xlVAlignJustify; break;
            }
        }

        void AssignExcelBorder(Microsoft.Office.Interop.Excel.Border border, double thickness)
        {
            if (thickness > 0)
            {
                border.LineStyle = XlLineStyle.xlContinuous;
                if (thickness == 1)
                    border.Weight = XlBorderWeight.xlHairline;
                if (thickness == 2)
                    border.Weight = XlBorderWeight.xlThin;
                if (thickness == 3)
                    border.Weight = XlBorderWeight.xlMedium;
                if (thickness == 4)
                    border.Weight = XlBorderWeight.xlThick;
            }
        }

        void ExcelPageSetup(Worksheet worksheet)
        {
            worksheet.PageSetup.Zoom = false;
            worksheet.PageSetup.FitToPagesWide = 1;
            worksheet.PageSetup.FitToPagesTall = false;
            worksheet.PageSetup.CenterHorizontally = true;
        }

        void AssignExcelPageHeader(Worksheet worksheet, DependencyObject root)
        {
            var textblocks = Utils.FindVisualChildren<TextBlock>(root);
            foreach (TextBlock tb in textblocks)
            {
                string value = TemplateStringExtractor.ExtractStringFromTextBlock(tb);
                switch (tb.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        switch (tb.TextAlignment)
                        {
                            case TextAlignment.Left:
                                worksheet.PageSetup.LeftHeader = value; break;
                            case TextAlignment.Right:
                                worksheet.PageSetup.RightHeader = value; break;
                            case TextAlignment.Center:
                                worksheet.PageSetup.CenterHeader = value; break;
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        switch (tb.TextAlignment)
                        {
                            case TextAlignment.Left:
                                worksheet.PageSetup.LeftFooter = value; break;
                            case TextAlignment.Right:
                                worksheet.PageSetup.RightFooter = value; break;
                            case TextAlignment.Center:
                                worksheet.PageSetup.CenterFooter = value; break;
                        }
                        break;
                }
            }
        }
    }
}
