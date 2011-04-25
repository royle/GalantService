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
using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Printing;
using System.ComponentModel;
using GLTWarter.Controls;
using GLTWarter.ExternalData;
using System.Threading;
using GLTWarter.Printings;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace GLTWarter
{
    public partial class MainScreen : Window
    {
        
        const string WelcomePageUri = "pack://application:,,,/GLTWarter;component/Pages/Welcome.xaml";

        private TabPages tabPages = new TabPages();

        public static RoutedUICommand BrowseHomepage = new RoutedUICommand();
        public static RoutedUICommand NewTab = new RoutedUICommand();
        public static RoutedUICommand CloseTab = new RoutedUICommand();
        public static RoutedUICommand FocusQuickSearchBox = new RoutedUICommand();
        public static RoutedUICommand QuickSearch = new RoutedUICommand();
        public static RoutedUICommand Help = new RoutedUICommand(); 

        public static RoutedUICommand Tab1 = new RoutedUICommand("Tab 1", "1", typeof(MainScreen));
        public static RoutedUICommand Tab2 = new RoutedUICommand("Tab 2", "2", typeof(MainScreen));
        public static RoutedUICommand Tab3 = new RoutedUICommand("Tab 3", "3", typeof(MainScreen));
        public static RoutedUICommand Tab4 = new RoutedUICommand("Tab 4", "4", typeof(MainScreen));
        public static RoutedUICommand Tab5 = new RoutedUICommand("Tab 5", "5", typeof(MainScreen));
        public static RoutedUICommand Tab6 = new RoutedUICommand("Tab 6", "6", typeof(MainScreen));
        public static RoutedUICommand Tab7 = new RoutedUICommand("Tab 7", "7", typeof(MainScreen));
        public static RoutedUICommand Tab8 = new RoutedUICommand("Tab 8", "8", typeof(MainScreen));
        public static RoutedUICommand LastTab = new RoutedUICommand("Last Tab", "Last", typeof(MainScreen));
        public static RoutedUICommand PreviousTab = new RoutedUICommand("Previous Tab", "Previous", typeof(MainScreen));
        public static RoutedUICommand NextTab = new RoutedUICommand("Next Tab", "Next", typeof(MainScreen));

       
        public MainScreen()
        {
            InitializeComponent();
            this.DataContext = this;
            tabPages.Add(new TabPage(WelcomePageUri));
            tabPageControl.ItemsSource = tabPages;

            this.AddHandler(BrowserTabItem.CloseTabEvent, new RoutedEventHandler(this.CloseTab_Event));
        }

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam);

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case BriSDKLib.BRI_EVENT_MESSAGE:
                    {
                        BriSDKLib.TBriEvent_Data EventData = (BriSDKLib.TBriEvent_Data)Marshal.PtrToStructure(lParam, typeof(BriSDKLib.TBriEvent_Data));
                        string strValue = "";
                        switch (EventData.lEventType)
                        {
                            case BriSDKLib.BriEvent_PhoneHook:
                                {
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：电话机摘机";
                                } break;
                            case BriSDKLib.BriEvent_PhoneHang:
                                {
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：电话机挂机";
                                } break;
                            case BriSDKLib.BriEvent_CallIn:
                                {////两声响铃结束后开始呼叫转移到CC
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：来电响铃";
                                } break;
                            case BriSDKLib.BriEvent_GetCallID:
                                {
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：接收到来电号码 " + Utils.FromASCIIByteArray(EventData.szData);

                                } break;
                            case BriSDKLib.BriEvent_StopCallIn:
                                {
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：停止呼入，产生一个未接电话 ";
                                } break;
                            case BriSDKLib.BriEvent_GetDTMFChar: strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：接收到按键 " + Utils.FromASCIIByteArray(EventData.szData); break;
                            case BriSDKLib.BriEvent_RemoteHang:
                                {
                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：远程挂机 ";
                                } break;
                            case BriSDKLib.BriEvent_Busy:
                                {

                                    strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：接收到忙音,线路已经断开 ";
                                } break;
                            case BriSDKLib.BriEvent_DialTone: strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：检测到拨号音 "; break;
                            case BriSDKLib.BriEvent_PhoneDial: strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：电话机拨号 " + Utils.FromASCIIByteArray(EventData.szData); break;
                            case BriSDKLib.BriEvent_RingBack: strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：拨号后接收到回铃音 "; break;
                            case BriSDKLib.BriEvent_DevErr:
                                {
                                    if (EventData.lResult == 3)
                                    {
                                        strValue = "通道" + (EventData.uChannelID + 1).ToString() + "：设备可能被移除 ";
                                    }
                                } break;
                            default: break;
                        }
                        if (strValue.Length > 0)
                        {
                            MessageBox.Show(strValue);
                        }
                    } break;
                default:

                    break;

            }
            return DefWindowProc(hwnd, msg, wParam, lParam);

        }



        #region Excel
        public static readonly DependencyProperty ExportJobsCountProperty = DependencyProperty.Register(
            "ExportJobsCount", typeof(int), typeof(MainScreen), new FrameworkPropertyMetadata(0));

        Dictionary<object, double> excelExportJobs = new Dictionary<object, double>();

        public int ExportJobsCount
        {
            get { return (int)this.GetValue(ExportJobsCountProperty); }
            set { this.SetValue(ExportJobsCountProperty, value); }
        }

        public void ExportJob_ReportDoWork(ExcelExporterBase exporter)
        {
            excelExportJobs.Add(exporter, 0);
            exporter.Context = SynchronizationContext.Current;

            UpdateExcelProgress();

            ExportJobsCount++;
            this.Cursor = System.Windows.Input.Cursors.AppStarting;

            exporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.ExportJob_RunWorkerCompleted);
            exporter.ProgressChanged += new ProgressChangedEventHandler(exporter_ProgressChanged);
            exporter.RunWorkerAsync();
        }

        private void MenuItem_ReportPrinterSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppCurrent.Active.Printing.Report.Setup();
            }
            catch (System.Printing.PrintSystemException ex)
            {
                MessageBox.Show(AppCurrent.Active.MainWindow, ex.ToString(), this.Title);
            }
        }

        private void MenuItem_ReportPrinterTest_Click(object sender, RoutedEventArgs e)
        {
            AppCurrent.Active.Printing.Report.Print(PrintTemplates.ReportDemo, null);
        }

        void exporter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            excelExportJobs[sender] = e.ProgressPercentage / 100.0;
            UpdateExcelProgress();
        }

        void UpdateExcelProgress()
        {
            double progress = (from x in excelExportJobs select x.Value).Sum() / excelExportJobs.Count;
            progressExcelJobs.Value = excelExportJobs.Count == 0 ? 1 : progress;
            progressExcelJobs.IsIndeterminate = (progress < 0.01);
        }

        void ExportJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            excelExportJobs.Remove(sender);
            UpdateExcelProgress();

            if (--ExportJobsCount == 0) this.Cursor = null;
            ((BackgroundWorker)sender).RunWorkerCompleted -= new RunWorkerCompletedEventHandler(ExportJob_RunWorkerCompleted);
            if (e.Error != null)
            {
                MessageBox.Show(AppCurrent.Active.MainScreen, string.Format(System.Globalization.CultureInfo.InvariantCulture, Resource.exportError, e.Error.Message), this.Title);
            }
            else
            {
                this.OpenFile(((IExcelExporter)sender).Filename);
            }

        }

        private void OpenFile(string Filename)
        {
            try
            {
                if (string.IsNullOrEmpty(Filename))
                    return;
                System.Diagnostics.Process.Start(Filename);
            }
            catch (FileNotFoundException)
            {
                // TODO: Logging
            }
            catch (Win32Exception)
            {
                // TODO: Logging
            }
        }
        #endregion

        #region Tab
        private void CloseTab_Event(object source, RoutedEventArgs args)
        {
            if (tabPages.Count > 1)
                tabPages.Remove(this.tabPageControl.ItemContainerGenerator.ItemFromContainer(this.tabPageControl.ContainerFromElement((DependencyObject)args.OriginalSource)) as TabPage);
        }

        private void NewTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tabPages.Add(new TabPage(WelcomePageUri));
            tabPageControl.SelectedIndex = tabPages.Count - 1;
        }

        private void SwitchTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string name = ((RoutedUICommand)e.Command).Name;
            if (name == "Last")
            {
                this.tabPageControl.SelectedIndex = this.tabPageControl.Items.Count - 1;
            }
            else if (name == "Next")
            {
                if (this.tabPageControl.SelectedIndex + 1 < this.tabPageControl.Items.Count)
                    this.tabPageControl.SelectedIndex = this.tabPageControl.SelectedIndex + 1;
            }
            else if (name == "Previous")
            {
                if (this.tabPageControl.SelectedIndex > 0)
                    this.tabPageControl.SelectedIndex = this.tabPageControl.SelectedIndex - 1;
            }
            else
            {
                int index = int.Parse(name);
                if (this.tabPageControl.Items.Count >= index)
                {
                    this.tabPageControl.SelectedIndex = index - 1;
                }
            }
        }

        private void CloseTab_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tabPages.Remove(tabPageControl.SelectedItem as TabPage);
        }

        private void CloseTab_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tabPages.Count > 1;
        }
        #endregion

        #region Menu


        private void MenuItem_EffectiveRole_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = ((MenuItem)e.Source);
            AppCurrent.Active.AppCach.StationCurrent = m.Tag as Galant.DataEntity.Entity;
        }

        /// <summary>
        /// Whenever you press F12 , you will be navigated to the Welcome.xaml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_HomePage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigateActive(new Pages.Welcome());
        }

        /// <summary>
        /// Navigate to the Homepage. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseHomepage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (tabPageControl.SelectedContent as TabPage != null)
            {
                if ((tabPageControl.SelectedContent as TabPage).Page as Frame != null)
                {
                    Frame tab = ((tabPageControl.SelectedContent as TabPage).Page as Frame);
                    while (tab.CanGoBack) { tab.GoBack(); }
                }
            }
        }
        #endregion

        #region Navigation
        //internal Pages.DetailsBase NavigateEntityDetails(object obj)
        //{
        //    Pages.DetailsBase page = null;
        //    if (obj is Data.Entity)
        //    {
        //        page = new Pages.Entity.Details((Data.Entity)obj);
        //    }
        //    else if (obj is Data.Shipment)
        //    {
        //        page = new Pages.Shipment.Entity.Details((Data.Shipment)obj);
        //    }
        //    else if (obj is Data.Trx)
        //    {
        //        page = new Pages.Bill.TrxDetails((Data.Trx)obj);
        //    }
        //    else if (obj is Data.Bill)
        //    {
        //        page = new Pages.Bill.Details((Data.Bill)obj);
        //    }
        //    else if (obj is Data.EventLog)
        //    {
        //        page = new Pages.Shipment.Events.Details((Data.EventLog)obj);
        //    }
        //    else if (obj is Data.Route)
        //    {
        //        page = new Pages.Route.Details((Data.Route)obj);
        //    }
        //    else if (obj is Data.Complaint)
        //    {
        //        page = new Pages.Complaint.Details((Data.Complaint)obj);
        //    }
        //    if (page != null)
        //    {
        //        this.NavigateActive(page);
        //    }
        //    return page;
        //}

        internal void NavigateActive(Page page)
        {
            if (tabPageControl.SelectedContent as TabPage != null)
            {
                if ((tabPageControl.SelectedContent as TabPage).Page as Frame != null)
                {
                    ((tabPageControl.SelectedContent as TabPage).Page as Frame).Navigate(page);
                }
            }
        }


        private void FocusQuickSearchBox_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Frame frame = ((TabPage)tabPageControl.SelectedValue).Page;
            TextBox box = frame.Template.FindName("textQuickShipmentQuery", frame) as TextBox;
            box.Focus();
            box.SelectAll();
        }

        private void QuickSearch_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Galant.DataEntity.Result.ResultPapersByID data = new Galant.DataEntity.Result.ResultPapersByID();
            data.PaperId = e.Parameter as string;
            data.Operation = "SearchPapersByID";
            this.NavigateActive(new Pages.Order.PaperListByID(data));
            //data.ShipmentId = e.Parameter as string;

            //this.NavigateActive(
            //    new Pages.Search(new Pages.Shipment.Entity.SearchById(data), new Pages.Shipment.Entity.ResultById(), new Pages.Shipment.Entity.ActionSingleGo())
            //);
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string Url = System.IO.Path.GetDirectoryName(asm.Location) + @"\help.chm";
            if (File.Exists(Url))
            {
                System.Windows.Forms.Help.ShowHelp(null, Url);
            }
        }
        #endregion

        HwndSource hwndSource;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = (HwndSource)HwndSource.FromVisual(this);
            hwndSource.AddHook(new HwndSourceHook(WndProc)); 

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            hwndSource.RemoveHook(new HwndSourceHook(WndProc));
        }
    }

    public class MainWindowTabDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return Application.Current.MainWindow.FindResource("NormalTabTemplate") as DataTemplate;
        }
    }

    public class MainWindowEffectiveRoleCheckConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (AppCurrent.Active.AppCach.StationCurrent!= null)
            {
                return AppCurrent.Active.AppCach.StationCurrent == values[0] as Galant.DataEntity.Entity;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MainWindowWeighScaleSelectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (AppCurrent.Active == null)
                return DependencyProperty.UnsetValue;

            bool selected=false;
         
            if ((bool)parameter)
            {
                return !selected ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return selected ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MainWindowWeighScaleCheckConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool selected = false;
            return selected;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MainWindowEntityRolesMapHeaderConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Galant.DataEntity.Entity rm = values as Galant.DataEntity.Entity;
            return string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                Resource.converterEntityRolesMap,
                Data.EntityNameConverter.Convert(rm));
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
