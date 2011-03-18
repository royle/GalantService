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
using GLTWarter.Styles;

namespace GLTWarter.ExternalData
{
    class ExcelXceedWriter
    {
        public event WriterProgressHandler WriterProgress;
        void RaiseProgress(int progress)
        {
            if (WriterProgress != null)
                WriterProgress(progress);
        }

        const string ACCOUNT_NUMBER_FORMAT = "_( * #,##0.00_);_( * (#,##0.00);_( * 0.00_);_(@_)";

        DataGridControl list;
        Worksheet worksheet;
        int rowOffset;
        int columnOffset;
        XceedFlattener exporter;

        /// <summary>
        /// Number of rows written after <see cref="Process"/>
        /// </summary>
        public int RowsWritten { protected set; get; }

        /// <summary>
        /// Number of columns written after <see cref="Process"/>
        /// </summary>
        public int ColumnsWritten { protected set; get; }

        /// <summary>
        /// Draw horizontal hairline border in the grid. Must be called before <see cref="Process"/>
        /// </summary>
        public bool DrawHorizontalHairline { set; get; }

        public ExcelXceedWriter(DataGridControl list, Worksheet worksheet, int rowOffset, int columnOffset, XceedFlattener exporter)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (worksheet == null)
                throw new ArgumentNullException("worksheet");

            this.list = list;
            this.worksheet = worksheet;
            this.rowOffset = rowOffset;
            this.columnOffset = columnOffset;
            this.exporter = exporter;
        }

        /// <summary>
        /// Flattern and write the DataGridControl content into the worksheet.
        /// <see cref="RowsWritten"/> and <see cref="ColumnsWritten"/> will be filled after this.
        /// </summary>
        /// <exception cref="InvalidOperationException">If any exception was thrown during Excel operation, it will be wrapped and rethrown is InvalidOperationException.</exception>
        public void Process()
        {
            Thread proc = null;
            TemplateStringExtractor extractor = exporter.Extractor;
            Worksheet ws = worksheet;

            try
            {
                proc = new Thread(extractor.Run);
                proc.SetApartmentState(ApartmentState.STA);
                proc.Start();

                list.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        exporter.Export(new System.IO.MemoryStream());
                    }
                );
                extractor.SignalAllWorkItemsQueued();
                RaiseProgress(20);

                int indent = 0;
                list.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        indent = ExportHint.GetMinGroupLevel(list);
                    }
                );
                indent = Math.Max(indent, exporter.MaxIndent);

                List<ColumnProcessing> columns = exporter.Columns;
                int jExcel = exporter.MaxRows;
                List<RowData> rawGrids = exporter.RawGrids;

                Range range;

                int[] columnStartIndex = new int[columns.Count];
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i == 0)
                    {
                        columnStartIndex[i] = 0;
                    }
                    else
                    {
                        columnStartIndex[i] = columnStartIndex[i - 1] + columns[i - 1].ColumnWidth;
                    }
                }
                int maxColumnWidth = columns.Count == 0 ? 0 : columnStartIndex[columns.Count - 1] + columns[columns.Count - 1].ColumnWidth;

                // Content
                List<KeyValuePair<int, RowData>> mergeRows = new List<KeyValuePair<int, RowData>>();
                object[,] cell_data_array = new object[jExcel, maxColumnWidth + indent];
                string[,] cell_format_array = new string[jExcel, maxColumnWidth + indent];
                int r = 1;
                foreach (RowData row in rawGrids)
                {
                    int jExcelStep = 1;
                    switch (row.Mode)
                    {
                        case RowModes.GroupHeader:
                            cell_data_array[r, row.Indent] = row.GroupHeader;
                            cell_format_array[r, row.Indent] = "@";
                            mergeRows.Add(new KeyValuePair<int, RowData>(r, row));
                            break;
                        default:
                            for (int i = columns.Count - 1; i >= 0; i--)
                            {
                                if (row.Cells[i] != null && row.Cells[i].datum != null)
                                {
                                    for (int k = row.Cells[i].datum.GetLength(0) - 1; k >= 0; k--)
                                    {
                                        for (int l = row.Cells[i].datum.GetLength(1) - 1; l >= 0; l--)
                                        {
                                            cell_data_array[r + k, columnStartIndex[i] + row.Indent + l] = row.Cells[i].datum[k, l];
                                            switch (row.Cells[i].format[k, l])
                                            {
                                                case CellType.Date:
                                                    cell_format_array[r + k, columnStartIndex[i] + row.Indent + l] = Resource.exportDateFormat;
                                                    break;
                                                case CellType.Currency:
                                                    cell_format_array[r + k, columnStartIndex[i] + row.Indent + l] = ACCOUNT_NUMBER_FORMAT;
                                                    break;
                                                default:
                                                    cell_format_array[r + k, columnStartIndex[i] + row.Indent + l] = "@";
                                                    break;
                                            }
                                        }
                                    }
                                    jExcelStep = Math.Max(jExcelStep, row.Cells[i].datum.GetLength(0));
                                }
                            }
                            if (row.Mode == RowModes.GroupTotal)
                            {
                                range = (Range)ws.Cells[rowOffset + 1 + r, columnOffset + 1];
                                range = range.get_Resize(jExcelStep, maxColumnWidth + indent);
                                range.Font.Bold = true;

                                Microsoft.Office.Interop.Excel.Border topBorder = range.Borders.get_Item(XlBordersIndex.xlEdgeTop);
                                topBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                                topBorder.LineStyle = XlLineStyle.xlContinuous;
                                topBorder.Weight = XlBorderWeight.xlThin;
                                
                                Microsoft.Office.Interop.Excel.Border bottomBorder = range.Borders.get_Item(XlBordersIndex.xlEdgeBottom);
                                bottomBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                                bottomBorder.LineStyle = XlLineStyle.xlContinuous;
                                bottomBorder.Weight = XlBorderWeight.xlThin;
                            } else if (row.Mode == RowModes.GrandTotal)
                            {
                                range = (Range)ws.Cells[rowOffset + 1 + r, columnOffset + 1];
                                range = range.get_Resize(jExcelStep, maxColumnWidth + indent);
                                range.Font.Bold = true;

                                Microsoft.Office.Interop.Excel.Border topBorder = range.Borders.get_Item(XlBordersIndex.xlEdgeTop);
                                topBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                                topBorder.LineStyle = XlLineStyle.xlContinuous;
                                topBorder.Weight = XlBorderWeight.xlThin;
                                Microsoft.Office.Interop.Excel.Border bottomBorder = range.Borders.get_Item(XlBordersIndex.xlEdgeBottom);
                                bottomBorder.ColorIndex = XlColorIndex.xlColorIndexAutomatic;
                                bottomBorder.LineStyle = XlLineStyle.xlContinuous;
                                bottomBorder.Weight = XlBorderWeight.xlThick;
                            }
                            break;
                    }
                    r += jExcelStep;
                }
                RaiseProgress(40);

                lock (extractor.ResultQueue)
                {
                    while (!extractor.IsDone || extractor.ResultQueue.Count > 0)
                    {
                        if (extractor.ResultQueue.Count == 0)
                        {
                            Monitor.Wait(extractor.ResultQueue);
                            if (extractor.ResultQueue.Count == 0)
                                break;
                        }
                        VisualProcessingWorkItem wi = extractor.ResultQueue.Dequeue() as VisualProcessingWorkItem;
                        cell_data_array[wi.ExcelRow, columnStartIndex[wi.Column]] = wi.Result;
                        cell_format_array[wi.ExcelRow, columnStartIndex[wi.Column]] = "@";
                    }
                }
                RaiseProgress(50);

                // Header                
                range = (Range)ws.Cells[rowOffset + 1, columnOffset + 1];
                range = range.get_Resize(1, maxColumnWidth + indent);
                range.NumberFormat = "@";
                range.RowHeight = (double)range.RowHeight * 2;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                Microsoft.Office.Interop.Excel.Border headerBorder = range.Borders[XlBordersIndex.xlEdgeBottom];
                headerBorder.Weight = XlBorderWeight.xlMedium;
                for (int i = 0; i < columns.Count; i++)
                {
                    cell_data_array[0, columnStartIndex[i] + indent] = columns[i].Header;
                }
                RaiseProgress(60);

                // Final Saving
                if (jExcel > 1)
                {
                    for (int i = 0; i < maxColumnWidth + indent; i++)
                    {
                        if (jExcel > 1)
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
                            string maxString = (from f in formats where f.Value == formats.Max(y => y.Value) select f.Key).Take(1).SingleOrDefault();

                            range = (Range)ws.Cells[rowOffset + 1 + 1, columnOffset + 1 + i];
                            range = range.get_Resize(jExcel - 1, 1);
                            range.NumberFormat = maxString;

                            for (int j = 1; j < jExcel; j++)
                            {
                                if (cell_format_array[j, i] != maxString)
                                {
                                    range = (Range)ws.Cells[rowOffset + 1 + j, columnOffset + 1 + i];
                                    range.NumberFormat = cell_format_array[j, i];
                                }
                            }
                        }
                    }

                    range = (Range)ws.Cells[rowOffset + 1, columnOffset + 1];
                    range = range.get_Resize(jExcel, maxColumnWidth + indent);
                    range.Value2 = cell_data_array;
                    /* This doesn't work
                    if (DrawHorizontalHairline)
                    {
                        Microsoft.Office.Interop.Excel.Border rangeBorder = range.Borders[XlBordersIndex.xlInsideHorizontal];
                        rangeBorder.Color = XlColorIndex.xlColorIndexAutomatic;
                        rangeBorder.LineStyle = XlLineStyle.xlContinuous;
                        rangeBorder.Weight = XlBorderWeight.xlHairline;
                    }
                     */
                }
                RaiseProgress(90);

                foreach (KeyValuePair<int, RowData> merge in mergeRows)
                {
                    range = (Range)ws.Cells[rowOffset + 1 + merge.Key, columnOffset + 1 + merge.Value.Indent];
                    range = range.get_Resize(1, maxColumnWidth + indent - merge.Value.Indent);
                    range.Merge(false);
                }

                RowsWritten = jExcel;
                ColumnsWritten = maxColumnWidth + indent;
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
            }
        }
    }
}
