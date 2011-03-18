using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.DataGrid;
using System.Threading;
using GLTWarter.Tools;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GLTWarter.ExternalData
{
    enum RowModes { GroupHeader, Data, GroupTotal, GrandTotal };

    enum CellType { String, Date, Currency };

    class RowData
    {
        public RowModes Mode;
        public int Indent;
        public string GroupHeader;
        public CellData[] Cells;
    }

    class ColumnProcessing
    {
        public string Name;
        public bool isSelected; // If columnFilters were supplied and selected
        public bool isKey; // If this is the key column for merging

        public string Header;
        public DataTemplate Template;
        public DataTemplateSelector TemplateSelector;
        public bool IsAddress;
        public bool IsLinesAsColumns;

        public int ColumnWidth;
    }

    class CellData
    {
        public Object[,] datum;
        public CellType[,] format;
    }

    class VisualProcessingWorkItem : TemplateStringExtractorWorkItem
    {
        public int ExcelRow;
        public int Column;
        public CellData cellData;
    }

    class XceedFlattener : Xceed.Wpf.DataGrid.Export.ExporterBase
    {
        List<ColumnProcessing> columns = new List<ColumnProcessing>();
        public List<ColumnProcessing> Columns { get { return columns; } }

        List<RowData> rawGrids = new List<RowData>();
        public List<RowData> RawGrids { get { return rawGrids; } }

        int currentIndent = 0;
        int maxIndent = 0;
        string keycolumn = null;
        HashSet<string> columnsFilter = null;

        public int MaxIndent { get { return maxIndent; } }
        public int MaxRows { get { return jExcel; } }

        public TemplateStringExtractor Extractor { get { return extractor; } }

        DataGridControl grid;
        public XceedFlattener(DataGridControl control)
            : base(control)
        {
            this.UseFieldNamesInHeader = true;
            this.StatFunctionDepth = int.MaxValue;
            this.grid = control;
        }

        /// <summary>
        /// Exports only those selected columns, and ignore all the grouping
        /// </summary>
        /// <param name="control"></param>
        /// <param name="columnsFilter"></param>
        public XceedFlattener(DataGridControl control, string keyColumn, IEnumerable<string> columnsFilter)
            : this(control)
        {
            this.keycolumn = keyColumn;
            this.columnsFilter = new HashSet<string>(columnsFilter);
        }

        /// <summary>
        /// Calling Export() will only provides <see cref="RawGrids"/>, which has all data that requires no template, and 
        /// <see cref="Extractor"/> which have all template which needed further processing delayed.
        /// By calling this function, the program will block until all data postponed in the Extractor is processed and re-injected into the RawGrids.
        /// </summary>
        public void CompleteDataExtractor()
        {
            extractor.SignalAllWorkItemsQueued();

            Thread proc = new Thread(extractor.Run);
            proc.SetApartmentState(ApartmentState.STA);
            proc.Start();

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
                    wi.cellData.datum = new object[1, 1] { { wi.Result } };
                    wi.cellData.format = new CellType[1, 1] { { CellType.String } };
                }
            }
        }

        protected override void StartHeaderField(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, string headerValue)
        {
            Column gridColumn = grid.Columns[headerValue];

            ColumnProcessing column = new ColumnProcessing();
            column.Name = gridColumn.FieldName;
            if (columnsFilter == null || columnsFilter.Contains(gridColumn.FieldName))
                column.isSelected = true;
            if (gridColumn.FieldName == keycolumn)
                column.isKey = true;
            column.Header = gridColumn.Title == null ? string.Empty : gridColumn.Title.ToString();

            if (gridColumn.CellContentTemplate != null)
            {
                column.Template = gridColumn.CellContentTemplate;
            }
            else if (gridColumn.CellContentTemplateSelector != null &&
                    gridColumn.CellContentTemplateSelector.GetType().Name != "GenericContentTemplateSelector"
                    )
            {
                column.TemplateSelector = gridColumn.CellContentTemplateSelector;
            }
            column.Template = gridColumn.CellContentTemplate;
            column.IsAddress = Styles.ExportHint.GetIsAddress(gridColumn);
            column.IsLinesAsColumns = Styles.ExportHint.GetIsLinesAsColumns(gridColumn);


            column.ColumnWidth = 1;


            this.columns.Add(column);
        }
        int jExcel = 1;
        int jExcelStep = 1;
        RowData row;
        CellData[] rawRow;

        protected override void StartGroup(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, CollectionViewGroup group, string title)
        {
            if (columnsFilter == null)
            {
                row = new RowData();
                row.Mode = RowModes.GroupHeader;
                row.GroupHeader = grid.Columns[title].Title + ": " + (group.Name == null ? string.Empty : group.Name.ToString());
                row.Indent = currentIndent;
                rawGrids.Add(row);

                currentIndent++;
                maxIndent = Math.Max(maxIndent, currentIndent);
                jExcel++;
            }
        }

        protected override void EndGroup(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, CollectionViewGroup group, string title)
        {
            if (columnsFilter == null)
            {
                currentIndent--;
            }
        }

        protected override void StartDataItem(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, object dataItem)
        {
            jExcelStep = 1;
            rawRow = new CellData[this.columns.Count];
            row = new RowData();
            row.Mode = RowModes.Data;
            row.Indent = currentIndent;
            row.Cells = rawRow;
        }

        protected override void StartDataItemField(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, object fieldValue)
        {
            ColumnProcessing column = columns[exportContext.FieldIndex];
            if (column.isSelected || column.isKey)
            {
                CellData cell = ProcessDataField(column, exportContext, fieldValue);
                if (cell.datum != null && cell.datum.GetLength(1) > column.ColumnWidth)
                {
                    column.ColumnWidth = cell.datum.GetLength(1);
                }
            }
        }
        protected override void EndDataItem(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, object dataItem)
        {
            rawGrids.Add(row);
            jExcel += jExcelStep;
        }

        protected override void StartSummary(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, Xceed.Wpf.DataGrid.Stats.StatFunctionCollection statFunctions)
        {
            if (columnsFilter != null) return;
            jExcelStep = 1;
            rawRow = new CellData[this.columns.Count];
            row = new RowData();
            row.Mode = currentIndent == 0 ? RowModes.GrandTotal : RowModes.GroupTotal;
            row.Cells = rawRow;
            row.Indent = maxIndent;
        }
        protected override void StartSummaryField(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, Xceed.Wpf.DataGrid.Stats.StatFunction statFunction, object value)
        {
            if (columnsFilter != null) return;
            if (exportContext.FieldIndex >= 0 &&
                !statFunction.ResultPropertyName.StartsWith("Total"))
            {
                CellData cell = ProcessDataField(columns[exportContext.FieldIndex], exportContext, value);
                if (cell.datum != null && cell.datum.GetLength(1) > columns[exportContext.FieldIndex].ColumnWidth)
                {
                    columns[exportContext.FieldIndex].ColumnWidth = cell.datum.GetLength(1);
                }
            }
        }
        protected override void EndSummary(Xceed.Wpf.DataGrid.Export.ExportContext exportContext, Xceed.Wpf.DataGrid.Stats.StatFunctionCollection statFunctions)
        {
            if (columnsFilter != null) return;
            rawGrids.Add(row);
            jExcel += jExcelStep;
        }

        TemplateStringExtractor extractor = new TemplateStringExtractor();
        CellData ProcessDataField(ColumnProcessing column, Xceed.Wpf.DataGrid.Export.ExportContext exportContext, object value)
        {
            CellData cell = new CellData();
            int fieldIndex = exportContext.FieldIndex;
            rawRow[fieldIndex] = cell;

           
                ProcessDataField(cell, fieldIndex, value);
           
            return cell;
        }

        void ProcessDataField(CellData cell, int fieldIndex, object fieldValue)
        {
            if (fieldValue != null)
            {
                if (fieldValue is DateTime?)
                {
                    cell.datum = new object[1, 1] { { ((DateTime?)fieldValue).Value } };
                    cell.format = new CellType[1, 1] { { CellType.Date } };
                }
                else if (fieldValue is DateTime)
                {
                    cell.datum = new object[1, 1] { { ((DateTime)fieldValue) } };
                    cell.format = new CellType[1, 1] { { CellType.Date } };
                }
                else if (fieldValue is IEnumerable<string>)
                {
                    IEnumerable<string> values = (IEnumerable<string>)fieldValue;
                    if (this.columns[fieldIndex].IsLinesAsColumns)
                    {
                        int count = values.Count();
                        cell.datum = new object[1, count];
                        cell.format = new CellType[1, count];
                        int i = 0;
                        foreach (string s in values)
                        {
                            cell.datum[0, i] = s;
                            cell.format[0, i] = CellType.String;
                            i++;
                        }
                    }
                    else
                    {
                        cell.datum = new object[1, 1] { { string.Join(Environment.NewLine, values.ToArray()) } };
                        cell.format = new CellType[1, 1] { { CellType.String } };
                    }
                }
                else
                {
                    if (this.columns[fieldIndex].Template != null || this.columns[fieldIndex].TemplateSelector != null)
                    {
                        DataTemplate dt;
                        if (columns[fieldIndex].Template != null)
                        {
                            dt = columns[fieldIndex].Template;
                        }
                        else
                        {
                            dt = columns[fieldIndex].TemplateSelector.SelectTemplate(fieldValue, null);
                        }

                        if (dt != null)
                        {
                            HostVisual hostVisual = new HostVisual();
                            VisualProcessingWorkItem wi = new VisualProcessingWorkItem();
                            wi.Column = fieldIndex;
                            wi.ExcelRow = jExcel;
                            wi.Template = dt;
                            wi.Context = fieldValue;
                            wi.Host = hostVisual;
                            wi.cellData = cell;

                            extractor.QueueWorkItem(wi);
                        }
                    }
                    else if (fieldValue is int || fieldValue is Int64 || fieldValue is int? || fieldValue is Int64?)
                    {
                        cell.datum = new object[1, 1] { { fieldValue } };
                        cell.format = new CellType[1, 1] { { CellType.String } };
                    }
                    else
                    {
                        cell.datum = new object[1, 1] { { fieldValue.ToString() } };
                        cell.format = new CellType[1, 1] { { CellType.String } };
                    }
                }
            }
        }
    }
}
