using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Printing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using GLTWarter.ExternalData;

namespace GLTWarter.Printings
{

    /// <summary>
    /// 打印类型
    /// </summary>
    public enum PrintModes
    {
        Summary, Full
    };

    public static class PrintTemplates
    {
        public const string LabelDemo = "PrintTemplates/LabelDemo.xaml";
        public const string BigLabelDemo = "PrintTemplates/BigLabelDemo.xaml";
        public const string ReportDemo = "PrintTemplates/ReportDemo.xaml";
        public const string Transfer = "PrintTemplates/Transfer.xaml";
        public const string ShipmentLabel = "PrintTemplates/ShipmentLabel.xaml";
        public const string ShipmentBigLabel = "PrintTemplates/ShipmentBigLabel.xaml";
        public const string Trx = "PrintTemplates/Trx.xaml";
        public const string Trx2 = "PrintTemplates/Trx2.xaml";
        public const string TransferEdge = "PrintTemplates/TransferEdge.xaml";
        public const string EntityLabel = "PrintTemplates/EntityLabel.xaml";
        public const string TransferCheckoutAssigned = "PrintTemplates/TransferCheckoutAssigned.xaml";
    }

    public class Printer : INotifyPropertyChanged
    {
        string serverName;
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; OnPropertyChangedInternal("ServerName"); }
        }

        private string queueName;
        public string QueueName
        {
            get { return queueName; }
            set { queueName = value; OnPropertyChangedInternal("QueueName"); }
        }

        private Exception printError;
        public Exception PrintError
        {
            get { return printError; }
            set
            {
                printError = value;
                OnPropertyChangedInternal("PrintError");
            }
        }

        PrintTicket printTicket;
        public PrintTicket PrintTicket
        {
            get { return printTicket; }
            set { printTicket = value; OnPropertyChangedInternal("PrintTicket"); }
        }

        public bool IsValid
        {
            get
            {
                return GeneratePrintDialog() != null;
            }
        }

        bool isEnable;
        public bool IsEnable
        {
            get
            {
                return isEnable && isVisible;
            }
            set
            {
                isEnable = value;
                OnPropertyChangedInternal("IsEnable");
            }
        }

        bool isVisible;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChangedInternal("IsVisible");
            }
        }



        PrintModes printMode;
        public PrintModes PrintMode
        {
            get
            {
                return printMode;
            }
            set
            {
                printMode = value;
                OnPropertyChangedInternal("PrintMode");
                OnPropertyChangedInternal("IsPrintReportSummary");
                OnPropertyChangedInternal("IsPrintReportFull");
            }
        }

        /// <summary>
        /// 打印汇总
        /// </summary>
        public bool IsPrintReportSummary
        {
            get
            {
                return printMode == PrintModes.Summary;
            }
            set
            {
                if (value == true)
                {
                    PrintMode = PrintModes.Summary;
                }
            }
        }

        /// <summary>
        /// 打印明细
        /// </summary>
        public bool IsPrintReportFull
        {
            get
            {
                return printMode == PrintModes.Full;
            }
            set
            {
                if (value == true)
                {
                    PrintMode = PrintModes.Full;
                }
            }
        }


        public Printer(string serverName, string queueName, string printTicketString, bool isEnable, PrintModes printMode)
        {
            this.ServerName = serverName;
            this.QueueName = queueName;
            if (!string.IsNullOrEmpty(printTicketString))
            {
                try
                {
                    PrintTicket pt;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (StreamWriter sw = new StreamWriter(ms))
                        {
                            sw.Write(printTicketString);
                            sw.Flush();
                            ms.Position = 0;
                            pt = new PrintTicket(ms);
                        }
                    }
                    this.PrintTicket = pt;
                }
                catch (FormatException)
                {
                    // TODO: Log
                }
            }
            this.IsEnable = isEnable;
            this.printMode = printMode;
            this.IsVisible = true;
        }

        private PrintDialog GeneratePrintDialog()
        {
            if (String.IsNullOrEmpty(ServerName) ||
                String.IsNullOrEmpty(QueueName) ||
                PrintTicket == null)
            {
                return null;
            }
            PrintDialog pd = null;
            try
            {
                PrintQueue pq = new PrintQueue(new PrintServer(ServerName), QueueName);
                pd = new PrintDialog();
                pd.PrintQueue = pq;
                pd.PrintTicket = PrintTicket;
            }
            catch (PrintSystemException)
            {
                pd = null;
            }
            catch (UnauthorizedAccessException)
            {
                pd = null;
            }
            return pd;
        }

        public void Setup()
        {
            PrintDialog pd = GeneratePrintDialog() ?? new PrintDialog();
            bool? result = pd.ShowDialog();
            if (result == true)
            {
                ServerName = pd.PrintQueue.HostingPrintServer.Name;
                QueueName = pd.PrintQueue.Name;
                PrintTicket = pd.PrintTicket;
                OnPropertyChangedInternal("IsValid");
            }
        }


        public void Print(string uriTemplate, Object context)
        {
            this.PrintMultiple(uriTemplate, new object[] { context });
        }

        public void PrintMultiple(string uriTemplate, IEnumerable<object> context)
        {
            if (IsEnable)
            {
                if (QueueName.Equals("Microsoft XPS Document Writer", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (Action)delegate() { PrintWorker(uriTemplate, context); });
                }
                else
                {
                    Thread t = new Thread(new ThreadStart((Action)delegate() { PrintWorker(uriTemplate, context); }));
                    t.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                    t.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                    t.TrySetApartmentState(ApartmentState.STA);
                    t.Start();
                }
            }
        }

        public void ExportToExcel(string uriTemplate, Object context)
        {
            //ExcelPrintingExporter exporter = new ExcelPrintingExporter(uriTemplate, context);
            //AppCurrent.Active.MainScreen.ExportJob_ReportDoWork(exporter);
        }

        delegate FlowDocument TemplaterLoader();

        //delegate PrintTemplate
        private void PrintWorker(String templatePath, IEnumerable<Object> context)
        {
            try
            {
                TemplaterLoader xamlLoader = null;

                PrintDialog pd = GeneratePrintDialog();
                if (pd != null)
                {
                    // External
                    try
                    {
                        string templateAbsolutePath = Utils.GetExternalFile(templatePath);
                        if (!String.IsNullOrEmpty(templateAbsolutePath) && File.Exists(templateAbsolutePath))
                        {
                            string xaml = File.ReadAllText(templateAbsolutePath);
                            xamlLoader = new TemplaterLoader((TemplaterLoader)delegate()
                            {
                                return CreateFlowDocument(xaml);
                            });
                        }
                    }
                    catch (IOException ex)
                    {
                        PrintError = ex;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        PrintError = ex;
                    }
                    catch (XamlParseException ex)
                    {
                        PrintError = ex;
                    }

                    // Customization
                    try
                    {
                        //if (xamlLoader == null)
                        //{
                        //    if (templatePath == PrintTemplates.ShipmentBigLabel && AppCurrent.Active.Customization.PrintingShipmentBigLabel != null)
                        //    {
                        //        string xaml = AppCurrent.Active.Customization.PrintingShipmentBigLabel;
                        //        xamlLoader = new TemplaterLoader((TemplaterLoader)delegate()
                        //        {
                        //            return CreateFlowDocument(xaml);
                        //        });
                        //    }
                        //    else if (templatePath == PrintTemplates.ShipmentLabel && AppCurrent.Active.Customization.PrintingShipmentLabel != null)
                        //    {
                        //        string xaml = AppCurrent.Active.Customization.PrintingShipmentLabel;
                        //        xamlLoader = new TemplaterLoader((TemplaterLoader)delegate()
                        //        {
                        //            return CreateFlowDocument(xaml);
                        //        });
                        //    }
                        //}
                    }
                    catch (XamlParseException ex)
                    {
                        PrintError = ex;
                    }

                    // Internal Templates
                    if (xamlLoader == null)
                    {
                        try
                        {
                            xamlLoader = new TemplaterLoader((TemplaterLoader)delegate()
                            {
                                return (FlowDocument)Application.LoadComponent(new Uri(templatePath, UriKind.Relative));;
                            });
                        }
                        catch (XamlParseException ex)
                        {
                            PrintError = ex;
                        }
                    }
                    if (xamlLoader != null && context.Count() > 0)
                    {
                        FlowDocument currentDoc = null;
                        List<DocumentPaginator> paginators = new List<DocumentPaginator>();

                        foreach (Object c in context)
                        {
                            currentDoc = xamlLoader();
                            ReportGenerator.PopulateTemplate(currentDoc, c);
                            DocumentPaginator op = ((IDocumentPaginatorSource)currentDoc).DocumentPaginator;
                            op.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
                            paginators.Add(op);
                        }

                        DocumentPaginator p = new ReportDocumentPaginator(paginators, currentDoc.Resources, context);
                        pd.PrintDocument(p, AppCurrent.ProgramTitle);
                    }
                }
            }
            catch (PrintSystemException ex)
            {
                PrintError = ex;
            }
            catch (IOException ex)
            {
                PrintError = ex;
            }
            catch (InvalidCastException ex)
            {
                PrintError = ex;
            }
            catch (Exception ex) { }
        }

        private FlowDocument CreateFlowDocument(string xaml)
        {
            using (StringReader srStringReader = new StringReader(xaml))
            {
                XmlReader xrDoc = XmlReader.Create(srStringReader);
                return (FlowDocument)XamlReader.Load(xrDoc);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChangedInternal(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
