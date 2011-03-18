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
using System.Text.RegularExpressions;
using System.Globalization;

namespace GLTWarter.ExternalData
{
    class ExcelXceedMergeWriter
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
        /// Draw horizontal hairline border in the grid. Must be called before <see cref="Process"/>
        /// </summary>
        public bool DrawHorizontalHairline { set; get; }

        public ExcelXceedMergeWriter(DataGridControl list, Worksheet worksheet, int rowOffset, int columnOffset, XceedFlattener exporter)
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
            Regex columnNameMatch = new Regex(@"\[([a-zA-Z\._-]+)\]");
            
            try
            {                
                list.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        exporter.Export(new System.IO.MemoryStream());
                    }
                );
                exporter.CompleteDataExtractor();
                RaiseProgress(20);
 
                // Read existing excel data
                string[,] content = ReadExcel();
                int max_y = content.GetLength(0);
                int max_x = content.GetLength(1);

                // Recognize previously saved columns
                //   null = original data
                //   not null = previously saved column
                List<string> columnMap = new List<string>();
                for (int i = 0; i < max_x; i++)
                {
                    Match m = columnNameMatch.Match(content[0, i]);
                    if (m.Success)
                    {
                        columnMap.Add(m.Groups[1].Value);
                    }
                    else
                    {
                        columnMap.Add(null);
                    }
                }
                RaiseProgress(30);

                // Match the row with Merging key
                int keyIndex = 0;
                for (int i = 0; i < exporter.Columns.Count; i++)
                {
                    if (exporter.Columns[i].isKey)
                    {
                        keyIndex = i;
                        break;
                    }
                }

                int?[] rowMatching = new int?[max_y - 1];
                int?[] sourceMatching = new int?[exporter.RawGrids.Count];
                for (int k = exporter.RawGrids.Count - 1; k >= 0; k--)
                {
                    CellData data = exporter.RawGrids[k].Cells[keyIndex];
                    HashSet<string> key = new HashSet<string>();
                    for (int m = 0; m < data.datum.GetLength(1); m++)
                    {
                        key.Add(data.datum[0, m] as string);
                    }

                    for (int j = 1; j < max_y; j++)
                    {
                        for (int i = 0; i < max_x; i++)
                        {
                            if (columnMap[i] != null)
                                continue;
                            if (key.Contains(content[j, i]))
                            {
                                if (sourceMatching[k].HasValue)
                                {
                                    if (sourceMatching[k].Value != -1)
                                    {
                                        // No match
                                        // The same source row matched multiple excel row
                                        rowMatching[sourceMatching[k].Value] = -1;
                                        sourceMatching[k] = -1;
                                        j = max_y;
                                        break;
                                    }
                                }
                                else
                                {

                                    // Matches?
                                    if (!rowMatching[j - 1].HasValue)
                                    {
                                        rowMatching[j - 1] = k;
                                        sourceMatching[k] = j - 1;
                                        break; // Match - starting look at the next row
                                    }
                                    else
                                    {
                                        // No match
                                        // More than 1 source row matched the excel row
                                        rowMatching[j - 1] = -1;
                                        j = max_y;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                RaiseProgress(50);

                int selectedColumnCount = (from c in exporter.Columns where c.isSelected select c).Count();
                int exportedColumnCount = 0;

                // Paste data into Excel, add column if needed
                for (int m = 0; m < exporter.Columns.Count; m++)
                {
                    ColumnProcessing column = exporter.Columns[m];
                    if (!column.isSelected)
                        continue;
                    object[,] cell_data_array = new object[max_y, column.ColumnWidth];
                    string[,] cell_format_array = new string[max_y, column.ColumnWidth];

                    // Get the excel column to be updated
                    int columnIndex = columnMap.IndexOf(column.Name);
                    if (columnIndex < 0)
                    {
                        // If it wasn't saved. Create a new column.
                        columnIndex = columnMap.Count;
                        columnMap.Add(column.Name);
                    }

                    // Consider when ColumnWidth > 1
                    // Extend columnMap to accomodate larger column
                    for (int i = columnMap.Count; i < columnIndex + column.ColumnWidth; i++)
                    {
                        columnMap.Add(null);
                    }
                    // Consider when ColumnWidth > 1
                    // Overwrite the header which will be occupied by the current column
                    for (int i = columnIndex + 1; i < columnIndex + column.ColumnWidth; i++)
                    {
                        columnMap[i] = null;
                    }

                    cell_data_array[0, 0] = string.Format(CultureInfo.InvariantCulture, "{0} [{1}]", column.Header, column.Name);
                    cell_format_array[0, 0] = "@";
                    for (int r = 1; r < max_y; r++)
                    {
                        int? row = rowMatching[r - 1];
                        if (row.HasValue && row.Value >= 0)
                        {
                            // If a row was matched - get its data
                            CellData cell = exporter.RawGrids[row.Value].Cells[m];
                            if (cell != null && cell.datum != null && cell.datum.GetLength(0) == 1)
                            {
                                // Single row, one or more columns are copied to the destination array
                                for (int l = cell.datum.GetLength(1) - 1; l >= 0; l--)
                                {
                                    cell_data_array[r, l] = cell.datum[0, l];
                                    switch (cell.format[0, l])
                                    {
                                        case CellType.Date:
                                            cell_format_array[r, l] = Resource.exportDateFormat;
                                            break;
                                        case CellType.Currency:
                                            cell_format_array[r, l] = ACCOUNT_NUMBER_FORMAT;
                                            break;
                                        default:
                                            cell_format_array[r, l] = "@";
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If a row wasn't matched - get the old data
                            for (int l = column.ColumnWidth - 1; l >= 0; l--)
                            {
                                cell_format_array[r, l] = null;
                                if (columnIndex + l < max_x)
                                    cell_data_array[r, l] = content[r, columnIndex + l];
                            }
                        }
                    }

                    Range range;
                    // Bulk Formating
                    for (int i = 0; i < column.ColumnWidth; i++)
                    {
                        Dictionary<string, int> formats = new Dictionary<string, int>();
                        for (int j = 1; j < max_y; j++)
                        {
                            if (cell_format_array[j, i] != null)
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
                            else
                            {
                                formats.Clear();
                                break;
                            }
                        }
                        string maxString = (from f in formats where f.Value == formats.Max(y => y.Value) select f.Key).Take(1).SingleOrDefault();

                        if (maxString != null)
                        {
                            range = (Range)ws.Cells[2, 1 + i + columnIndex];
                            range = range.get_Resize(max_y - 1, 1);
                            range.NumberFormat = maxString;
                        }

                        for (int j = 0; j < max_y; j++)
                        {
                            if (cell_format_array[j, i] != null && cell_format_array[j, i] != maxString)
                            {
                                range = (Range)ws.Cells[1 + j, 1 + i + columnIndex];
                                range.NumberFormat = cell_format_array[j, i];
                            }
                        }
                    }

                    // Assign value to Excel
                    range = (Range)ws.Cells[1, 1 + columnIndex];
                    range = range.get_Resize(max_y, column.ColumnWidth);
                    range.Value2 = cell_data_array;
                    ((Range)ws.Columns[1 + columnIndex]).EntireColumn.AutoFit();

                    exportedColumnCount++;
                    RaiseProgress((int)((double)(95 - 60) * exportedColumnCount /selectedColumnCount) + 60);
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
            }
        }

        public string[,] ReadExcel()
        {
            Range range = ((Range)worksheet.UsedRange);
            int max_x = range.Columns.Count;
            int max_y = range.Rows.Count;

            // All Excel Range is 1 based.
            object[,] data = (object[,])range.Value2;

            // Reduce Row
            for (int j = max_y; j > 0; j--)
            {
                bool clean = true;
                for (int i = max_x; i > 0; i--)
                {
                    object value = data[j, i];
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        clean = false;
                        break;
                    }
                }
                if (!clean)
                {
                    max_y = j;
                    break;
                }
            }
            // Reduce Column
            for (int i = max_x; i > 0; i--)
            {
                bool clean = true;
                for (int j = max_y; j > 0; j--)
                {
                    object value = data[j, i];
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        clean = false;
                        break;
                    }
                }
                if (!clean)
                {
                    max_x = i;
                    break;
                }
            }

            string[,] content = new string[max_y, max_x];
            for (int j = 0; j < max_y; j++)
            {
                for (int i = 0; i < max_x; i++)
                {
                    object value = data[j + 1, i + 1];
                    content[j, i] = value == null ? string.Empty : value.ToString();
                }
            }
            return content;
        }
    }
}
