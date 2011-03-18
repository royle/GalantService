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
using System.IO;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Xceed.Wpf.DataGrid;
using GLTWarter.ExternalData;
using System.Globalization;
using System.Threading;

namespace GLTWarter
{
    internal class ReportGenerator
    {
        internal static void PopulateTemplate(FlowDocument doc, Object bind)
        {
            try
            {
                doc.DataContext = bind;
            }
            catch (InvalidOperationException) { }

            // This is necessary to pump the message pipe such that binding are rendered in the Xceed control.
            doc.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.DataBind,
                (Action)delegate() { });

            Block block = doc.Blocks.FirstBlock;
            while (block != null)
            {
                if (block is Section)
                {
                    Section sect = block as Section;
                    if (!string.IsNullOrEmpty(sect.Tag as string ?? sect.Name) &&
                        doc.FindResource(sect.Tag as string ?? sect.Name) as DataTemplate != null)
                    {
                        DataTemplate t = doc.FindResource(sect.Tag ?? sect.Name) as DataTemplate;
                        if (sect.DataContext is DataGridControl)
                        {
                            PopulateXceedSection(doc, sect, t);
                        }
                        else if (sect.DataContext as System.Collections.IEnumerable != null)
                        {
                            PopulateTemplateSection(doc, sect, t);
                        }
                    }
                }
                block = block.NextBlock;
            }
        }

        class ReportCellData
        {
            string[] datum;
            public string[] Datum
            {
                get { return datum; }
                set { datum = value; }
            }
        };

        static void PopulateXceedSection(FlowDocument doc, Section sect, DataTemplate tmpl)
        {
            DataGridControl list = sect.DataContext as DataGridControl;
            XceedFlattener flattener = new XceedFlattener(sect.DataContext as DataGridControl);
            flattener.Export(new MemoryStream());
            flattener.CompleteDataExtractor();

            IList<ColumnProcessing> columns = flattener.Columns;
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

            foreach (RowData data in flattener.RawGrids)
            {                
                if (data.Mode == RowModes.GroupHeader)
                {
                    Grid row = new Grid();
                    row.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(data.Indent * 12, GridUnitType.Pixel)
                    });

                    row.ColumnDefinitions.Add(new ColumnDefinition() { Width=new GridLength(1 , GridUnitType.Star)});
                    TextBlock block = new TextBlock() { Text = data.GroupHeader };
                    row.Children.Add(block);
                    Grid.SetColumn(block, 1);

                    BlockUIContainer container = new BlockUIContainer(row);
                    sect.Blocks.Add(container);
                }
                else
                {
                    ReportCellData reportCell = new ReportCellData();
                    reportCell.Datum = new string[maxColumnWidth];

                    List<string> reportCellData = new List<string>();

                    for (int c = 0; c < data.Cells.Length; c++)
                    {
                        CellData cell = data.Cells[c];
                        if (cell == null)
                            continue;
                        for (int i = 0; i < cell.datum.GetLength(1); i++)
                        {
                            string s = string.Empty;
                            for (int j = 0; j < cell.datum.GetLength(0); j++)
                            {
                                if (j > 0)
                                    s += Environment.NewLine;
                                switch (cell.format[j, i])
                                {
                                    case CellType.Currency:
                                        s += ((decimal)cell.datum[j, i]).ToString("F", CultureInfo.InvariantCulture);
                                        break;
                                    case CellType.Date:
                                        s += ((DateTime)cell.datum[j, i]).ToString(Resource.exportDateFormat, CultureInfo.InvariantCulture);
                                        break;
                                    default:
                                        s += cell.datum[j, i].ToString();
                                        break;
                                }
                            }
                            reportCell.Datum[columnStartIndex[c] + i] = s;
                        }
                    }

                    ContentControl cc = new ContentControl();
                    cc.ContentTemplate = tmpl;
                    cc.Content = reportCell;

                    BlockUIContainer container = new BlockUIContainer(cc);
                    if (data.Mode == RowModes.GroupTotal)
                    {
                        container.BorderBrush = new SolidColorBrush(Colors.Black);
                        container.BorderThickness = new Thickness(0, 0, 0, 1);
                        container.FontWeight = FontWeights.Bold;
                    }
                    else if (data.Mode == RowModes.GrandTotal)
                    {
                        container.BorderBrush = new SolidColorBrush(Colors.Black);
                        container.BorderThickness = new Thickness(0, 1, 0, 0);
                        container.FontWeight = FontWeights.Bold;
                    }
                    sect.Blocks.Add(container);
                }
            }
        }

        static void PopulateTemplateSection(FlowDocument doc, Section sect, DataTemplate tmpl)
        {
            System.Collections.IEnumerable coll = sect.DataContext as System.Collections.IEnumerable;
            foreach (Object obj in coll)
            {
                ContentControl cc = new ContentControl();
                cc.ContentTemplate = tmpl;
                cc.Content = obj;

                BlockUIContainer bc = new BlockUIContainer(cc);
                sect.Blocks.Add(bc);
            }
        }
    }

    internal class PageDescriptor
    {
        public int CurrentPage { get; set;}
        public int PageCount { get; set;}
        public DocumentPaginator Paginator { get; set; }
        public Object Context { get; set;}
    }

    internal class ReportDocumentPaginator : DocumentPaginator
    {
        ResourceDictionary m_Template;
        List<PageDescriptor> descriptors;

        public ReportDocumentPaginator(IEnumerable<DocumentPaginator> paginators, ResourceDictionary template, IEnumerable<Object> context)
        {
            descriptors = new List<PageDescriptor>();
            m_Template = template;
                        
            var contextEnumerator = context.GetEnumerator();
            foreach (DocumentPaginator dp in paginators)
            {
                dp.ComputePageCount();
                contextEnumerator.MoveNext();
                Object c = contextEnumerator.Current;
                for (int i = 1; i <= dp.PageCount; i++)
                {
                    descriptors.Add(new PageDescriptor() { CurrentPage = i, PageCount = dp.PageCount, Context = c, Paginator = dp });
                }
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            PageDescriptor pd = descriptors[pageNumber];
            Size pageSize = pd.Paginator.PageSize;

            DocumentPage origPage = pd.Paginator.GetPage(pd.CurrentPage - 1);
            
            ContainerVisual cont = new ContainerVisual();
            cont.Children.Add(origPage.Visual);

            DataTemplate t;
            t = m_Template["PageHeader"] as DataTemplate;
            if (t != null)
            {
                ContentControl cc = new ContentControl();
                cc.Content = pd;
                cc.ContentTemplate = t;

                FlowDocument fd = new FlowDocument();

                fd.PageWidth = pageSize.Width;
                fd.PageHeight = pageSize.Height;
                fd.ColumnWidth = pageSize.Width;
                if (m_Template["PageHeaderStyle"] as Style != null)
                {
                    fd.Style = m_Template["PageHeaderStyle"] as System.Windows.Style;
                }
                Figure f = new Figure(new BlockUIContainer(cc));
                f.Width = new FigureLength(1, FigureUnitType.Page);
                f.Height = new FigureLength(1, FigureUnitType.Page);
                f.Margin = new Thickness(0);
                f.Padding = new Thickness(0);
                Paragraph p = new Paragraph(f);
                p.Margin = new Thickness(0);
                p.Padding = new Thickness(0);
                f.BorderThickness = new Thickness(2);

                fd.Blocks.Add(p);

                DocumentPaginator ipg = ((IDocumentPaginatorSource)fd).DocumentPaginator;
                ipg.PageSize = pageSize;
                DocumentPage dp = ipg.GetPage(0);
                cont.Children.Add(dp.Visual);
            }

            return new DocumentPage(cont, pageSize, origPage.BleedBox, origPage.ContentBox);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return descriptors.Count;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }

        public override Size PageSize
        {
            get { return Size.Empty; }
            set { }
        }
    }
}