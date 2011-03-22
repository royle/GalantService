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
using System.Globalization;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using GLTWarter.Controls;
using System.Collections.Specialized;
using System.Threading;

namespace GLTWarter.Pages.Finishing
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class PackageFinish : DetailsBase
    {
        delegate bool BatchHandler();
        Queue<Galant.DataEntity.Paper> batchShipment = null;
        BatchHandler batchFunction = null;
        bool isUpdateCurrency = true;//是否批量修改状态时更新收款方式

        public PackageFinish(Galant.DataEntity.Result.FinishCheckin data)
            : base(data)
        {
            //Data.CheckinRequest c = (Data.CheckinRequest)data;
            //if (c.WorkingOn.Length > 0)
            //    c.FinishingHolder = c.WorkingOn[0].Holder;
            //c.MarkAllAsImplicit();
            //c.MarkAllShipmentsChildren();
            //c.ProcessMobileStatus();
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PackageFinish_Loaded);
        }

        public override void OnApplyTemplate()
        {//增加事件
            //(this.listSuccess.SelectedItems as INotifyCollectionChanged).CollectionChanged +=
            //    new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ResultXceedSelectionChanged);

            //(this.listReGLTWarter.SelectedItems as INotifyCollectionChanged).CollectionChanged +=
            //    new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ReGLTWarterGLTWarterXceedSelectionChanged);
            //(this.listRefuse.SelectedItems as INotifyCollectionChanged).CollectionChanged +=
            //    new System.Collections.Specialized.NotifyCollectionChangedEventHandler(GLTWarterToReturnXceedSelectionChanged);
        }

        void PackageFinish_Loaded(object sender, RoutedEventArgs e)
        {
            base.buttonNext_Click(sender, e);
            //if (dataBackup != null) dataBackup.Stage = Data.CheckinRequest.Stages.Finishing;
            //if (dataCurrent != null) dataCurrent.Stage = Data.CheckinRequest.Stages.Finishing;
        }

        protected override bool BackAllowed
        {
            get
            {
                return false;
            }
        }

        protected override Galant.DataEntity.BaseData CreateNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override void FocusFirstControl()
        {
            this.selectorShipment.Focus();
        }

        protected override void PreCommit()
        {
            //Data.CheckinRequest data = (Data.CheckinRequest)dataCurrent;
            //data.Checkins = data.ActualWorkingList;
        }

        protected override void OnNext(Galant.DataEntity.BaseData incomingData)
        {
            Galant.DataEntity.Result.FinishCheckin checkin = incomingData as Galant.DataEntity.Result.FinishCheckin;
            checkin.Operation = "CheckinFinish";
            this.DataContext = checkin;
            this.dataCurrent = checkin;
        }

        private void Shipment_selectorEnter(object sender, PaperSelectorEnterEventArgs e)
        {
            Galant.DataEntity.Paper sm = this.selectorShipment.SelectedPaper;
            //处理
            if (bool.Parse(this.rdoDetail.IsChecked.ToString()))
            {//详细
                GoToDetail(sm, e.IsUnknownMode, e.EnteredText);
            }
            else if ((bool)this.rdoSuccess.IsChecked)
            {//成功
                //if (this.comboScanningCurrency.SelectedItem != null && sm != null && !string.IsNullOrEmpty(this.comboScanningCurrency.SelectedItem.ToString()) && !sm.ContactA.SupportedCurrencies.Contains(this.comboScanningCurrency.SelectedItem.ToString())
                //    && sm.FinishingCanHasNewBill)
                //{
                //    e.SoundPlayed = true;
                //    Utils.PlaySound(Resource.soundDing);
                //    ((Data.CheckinRequest)dataCurrent).ErrorMessage = string.Format(Resource.verificationEntityNotSupportCurrent, sm.ContactA.FullName, this.comboScanningCurrency.SelectedItem.ToString());
                //    ((Data.CheckinRequest)dataCurrent).OnCheckValid(((Data.CheckinRequest)dataCurrent).Stage);
                //    ((Data.CheckinRequest)dataCurrent).ErrorMessage = null;
                //    return;
                //}
                FigureBatchSimple(sm, e.IsUnknownMode, e.EnteredText, (BatchHandler)BatchSuccessHandler);
            }
            else if ((bool)this.rdoReGLTWarter.IsChecked)
            {//二次配送
                FigureBatchSimple(sm, e.IsUnknownMode, e.EnteredText, (BatchHandler)BatchReGLTWarterHandler);
            }
            else if ((bool)this.rdoException.IsChecked)
            {//异常
                FigureBatchSimple(sm, e.IsUnknownMode, e.EnteredText, (BatchHandler)BatchExceptionWithoutInHandler);
            }
        }

        private void Finish_ButtonClick(object sender, RoutedEventArgs e)
        {//归班为成功
            isUpdateCurrency = false;
            BuildBatchHandle((BatchHandler)BatchSuccessHandler);
        }

        private void ReGLTWarter_ButtonClick(object sender, RoutedEventArgs e)
        {//归班为二次配送
            BuildBatchHandle((BatchHandler)BatchReGLTWarterHandler);
        }

        private void Exception_ButtonClick(object sender, RoutedEventArgs e)
        {//只录入异常
            BuildBatchHandle((BatchHandler)BatchExceptionWithoutInHandler);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="handler"></param>
        private void BuildBatchHandle(BatchHandler handler)
        {
            BuildBatchHandle(handler, listPending);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="listxceed"></param>
        private void BuildBatchHandle(BatchHandler handler, Xceed.Wpf.DataGrid.DataGridControl listxceed)
        {
            batchFunction = (BatchHandler)handler;
            batchShipment = new Queue<Galant.DataEntity.Paper>(listxceed.SelectedItems.OfType<Galant.DataEntity.Paper>());
            ProcessBatch();
        }

        private void Finish_Undo_ButtonClick(object sender, RoutedEventArgs e)
        {//完成 未处理
            //BuildBatchHandle((BatchHandler)BatchReDoHandler, listSuccess);
        }

        private void ReGLTWarter_Undo_ButtonClick(object sender, RoutedEventArgs e)
        {//二次配送 未处理 
           // BuildBatchHandle((BatchHandler)BatchReDoHandler, listReGLTWarter);
        }

        private void Exception_Undo_ButtonClick(object sender, RoutedEventArgs e)
        {//拒收 未处理 
            //BuildBatchHandle((BatchHandler)BatchReDoHandler, listRefuse);
        }

        /// <summary>
        /// 转到详细页
        /// </summary>
        private void GoToDetail(Galant.DataEntity.Paper sm, bool isUnknownMode, string enteredText)
        {
            //Galant.DataEntity.Paper parentShipment = GetTrackingParent((Data.CheckinRequest)dataCurrent, sm);
            //selectorShipment.SelectedShipment = null;
            //((Data.CheckinRequest)dataCurrent).ClearInvalid();

            //if (parentShipment != null)
            //{
            //    Data.CheckinRequest data = ((Data.CheckinRequest)dataCurrent);

            //    if (sm.IsSealed.Value)
            //    {
            //        sm = (from s in parentShipment.Children where s.EntityEquals(sm) select s).Single();
            //        sm.IsMarked = true;
            //    }
            //    data.Checkin = parentShipment;
            //    DetailsBase page = new PackageFinishException(data);
            //    page.Return += new ReturnEventHandler<Galant.DataEntity.BaseData>(page_Return);
            //    this.NavigationService.Navigate(page);
            //}
            //else if (sm != null)
            //{
            //    ShowPopulateError("Finish checkin must have a tracking shipment parent (2)");
            //}
            //else if (isUnknownMode)
            //{
            //    DetailsBase pageNext = PackageFinishUnknownShipmentQuery.NewQuery(false, enteredText, ((Data.CheckinRequest)dataCurrent).Station);
            //    pageNext.Return += new ReturnEventHandler<GLTWarter.Galant.DataEntity.BaseData>(page_Return);
            //    this.NavigationService.Navigate(pageNext);
            //}
        }

        private void FigureBatchSimple(Galant.DataEntity.Paper sm, bool isUnknownMode, string enteredText, BatchHandler handle)
        {
            //Galant.DataEntity.Paper parentShipment = GetTrackingParent((Data.CheckinRequest)dataCurrent, sm);
            //selectorShipment.SelectedShipment = null;
            //((Data.CheckinRequest)dataCurrent).ClearInvalid();

            //if (parentShipment != null)
            //{
            //    batchFunction = handle;
            //    batchShipment = new Queue<GLTWarter.Galant.DataEntity.Paper>(new Galant.DataEntity.Paper[] { parentShipment });

            //    ProcessBatch();
            //}
            //else if (sm != null)
            //{
            //    ShowPopulateError("Finish checkin must have a tracking shipment parent (2)");
            //}
            //else if (isUnknownMode)
            //{
            //    DetailsBase pageNext = PackageFinishUnknownShipmentQuery.NewQuery(true, enteredText, ((Data.CheckinRequest)dataCurrent).Station);
            //    pageNext.Return += new ReturnEventHandler<GLTWarter.Galant.DataEntity.BaseData>(page_Return);
            //    this.NavigationService.Navigate(pageNext);
            //}
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        bool BatchSuccessHandler()
        {            
            //Galant.DataEntity.Paper sm = batchShipment.Dequeue();

            //sm.CloneOldToNewBills();
            //if (sm.FinishingCanHasNewBill)
            //{
            //    if (isUpdateCurrency && this.comboScanningCurrency.SelectedItem != null && !string.IsNullOrEmpty(this.comboScanningCurrency.SelectedItem.ToString()) && sm.NewBills != null)
            //    {
            //        foreach (Data.Bill bill in sm.NewBills)
            //        {
            //            bill.Currency = this.comboScanningCurrency.SelectedItem.ToString();
            //        }
            //    }
            //}

            //if (sm.Nature.Value == GLTWarter.Galant.DataEntity.PaperNature.Return
            //    && sm.Bound.Value == GLTWarter.Galant.DataEntity.PaperBound.ToB)
            //{
            //    Data.CheckinRequest data = ((Data.CheckinRequest)dataCurrent);
            //    data.Checkin = sm;
            //    DetailsBase page = new PackageFinishException(data);
            //    page.Return += new ReturnEventHandler<GLTWarter.Galant.DataEntity.BaseData>(pageBatch_Return);
            //    this.NavigationService.Navigate(page);
            //    return false;
            //}
            //else
            //{
            //    sm.LastUIOperatingTime = DateTime.Now;
            //    sm.Stage = Galant.DataEntity.Paper.Stages.FinishException;
            //    sm.ResetChildrenImplicitAndMark();
            //    sm.SubStatus = GLTWarter.Galant.DataEntity.PaperSubStatus.NextActionAssured;
            //    sm.Normalize();
            //    return true;
            //}
            return true;
        }
        /// <summary>
        /// 二次入库
        /// </summary>
        /// <returns></returns>
        bool BatchReGLTWarterHandler()
        {
            Galant.DataEntity.Paper sm = batchShipment.Dequeue();
            //sm.Stage = Galant.DataEntity.Paper.Stages.FinishException;
            //sm.ResetChildrenImplicitAndMark();
            //sm.SubStatus = GLTWarter.Galant.DataEntity.PaperSubStatus.CheckinException;
            //sm.NewBills = null;
            //sm.Normalize();
            //sm.LastUIOperatingTime = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 二次不入库
        /// </summary>
        /// <returns></returns>
        bool BatchExceptionWithoutInHandler()
        {
            Galant.DataEntity.Paper sm = batchShipment.Dequeue();
            //sm.Stage = Galant.DataEntity.Paper.Stages.FinishException;
            //sm.ResetChildrenImplicitAndMark();
            //sm.SubStatus = GLTWarter.Galant.DataEntity.PaperSubStatus.CheckinExceptionWithoutIn;
            //sm.NewBills = null;
            //sm.Normalize();
            //sm.LastUIOperatingTime = DateTime.Now;
            return true;
        }
        /// <summary>
        /// 未操作
        /// </summary>
        /// <returns></returns>
        bool BatchReDoHandler()
        {
            Galant.DataEntity.Paper sm = batchShipment.Dequeue();
            sm.Stage = null;
            //sm.ResetChildrenImplicitAndMark();
            //sm.SubStatus = GLTWarter.Galant.DataEntity.PaperSubStatus.Implicit;
            //sm.NewBills = null;
            //sm.Normalize();
            //sm.FinishingException = null;
            //sm.LastUIOperatingTime = DateTime.Now;
            return true;
        }

        void pageBatch_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            if (Data.BackObject.IsReturnGenuine(e))
            {
                //if (e.Result as Data.CheckinRequest != null)
                //{
                //    Data.CheckinRequest c = (Data.CheckinRequest)e.Result;
                //    Galant.DataEntity.Paper shipment = c.Shipment;
                //    ((Data.CheckinRequest)dataCurrent).ReplaceWorkingList(shipment);
                //    this.ProcessBatch();
                //}
                //else
                //{
                //    batchShipment = null;
                //}
            }
            else
            {
                batchShipment = null;
            }
        }

        void page_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            //if (Data.BackObject.IsReturnGenuine(e))
            //{
            //    if (e.Result as Data.CheckinRequest != null)
            //    {
            //        Data.CheckinRequest c = (Data.CheckinRequest)e.Result;
            //        Data.CheckinRequest data = (Data.CheckinRequest)dataCurrent;

            //        if (string.IsNullOrEmpty(c.ErrorMessage))
            //        {
            //            bool directDetail = (bool)this.rdoDetail.IsChecked;
            //            Galant.DataEntity.Paper shipment = c.Shipment;
            //            if (!directDetail)
            //            {
            //                Galant.DataEntity.Paper parentShipment = GetTrackingParent((Data.CheckinRequest)dataCurrent, shipment);

            //                if (parentShipment != null)
            //                {
            //                    if (this.rdoSuccess.IsChecked.Value)
            //                    {
            //                        batchFunction = (BatchHandler)BatchSuccessHandler;
            //                    }
            //                    else if (this.rdoReGLTWarter.IsChecked.Value)
            //                    {
            //                        batchFunction = (BatchHandler)BatchReGLTWarterHandler;
            //                    }
            //                    else if (this.rdoException.IsChecked.Value)
            //                    {
            //                        batchFunction = (BatchHandler)BatchExceptionWithoutInHandler;
            //                    }
            //                    else
            //                    {
            //                        batchFunction = (BatchHandler)BatchReDoHandler;
            //                    }
            //                    batchShipment = new Queue<GLTWarter.Galant.DataEntity.Paper>(new Galant.DataEntity.Paper[] { parentShipment });

            //                    List<string> listOriginReference = new List<string>();
            //                    foreach (Galant.DataEntity.Paper sp in batchShipment)
            //                    {
            //                        if (sp.ContactA.IsFinishingSignInfo)
            //                        {
            //                            listOriginReference.Add(string.Join(null as string ?? Environment.NewLine, sp.OriginReference as string[]));
            //                        }
            //                    }

            //                    if (listOriginReference != null && listOriginReference.Count > 0)
            //                    {
            //                        string errorMessage = string.Format(CultureInfo.InvariantCulture, string.Join(Resource.validationImportPreAdviceSeparator, listOriginReference.ToArray()) + Resource.validationCheckinFinishSignInfoMissing);
            //                        MessageBox.Show(AppCurrent.Active.MainScreen, errorMessage, this.Title);
            //                        return;
            //                    }
            //                }
            //                else if (shipment != null)
            //                {
            //                    ShowPopulateError("Finish checkin must have a tracking shipment parent (2)");
            //                    return;
            //                }
            //            }

            //            List<Galant.DataEntity.Paper> list = new List<GLTWarter.Galant.DataEntity.Paper>();
            //            list = ((Data.CheckinRequest)dataCurrent).WorkingOn.ToList();

            //            if (c.WorkingOn != null)
            //            {
            //                foreach (Galant.DataEntity.Paper cs in c.WorkingOn)
            //                {
            //                    if (data.WorkingOn.All(s => s.ShipmentId != cs.ShipmentId))
            //                    {
            //                        List<Galant.DataEntity.Paper> listWorkingOn = data.WorkingOn.ToList();
            //                        listWorkingOn.Add(cs);
            //                        data.WorkingOn = listWorkingOn.ToArray();
            //                    }
            //                }
            //            }
            //            data.ReplaceWorkingList(shipment);
            //            shipment.LastUIOperatingTime = DateTime.Now;
            //            if (!directDetail)
            //            {
            //                ProcessBatch();
            //            }
            //            this.FocusFirstControl();
            //        }
            //        else
            //        {
            //            ((Data.CheckinRequest)dataCurrent).ErrorMessage = c.ErrorMessage;
            //            ((Data.CheckinRequest)dataCurrent).OnCheckValid(((Data.CheckinRequest)dataCurrent).Stage);
            //            ((Data.CheckinRequest)dataCurrent).ErrorMessage = null;
            //        }
            //    }
            //}
        }

        void ProcessBatch()
        {//执行Handler
            //this.Dispatcher.BeginInvoke(
            //    System.Windows.Threading.DispatcherPriority.Loaded,
            //    (System.Action)delegate()
            //    {
            //        while (batchShipment != null && batchShipment.Count > 0)
            //        {
            //            if (!batchFunction()) break;
            //        }
            //        this.isUpdateCurrency = true;
            //        ((Data.CheckinRequest)dataCurrent).WorkingListChanged();
            //    });
        }

        //internal static Galant.DataEntity.Paper GetTrackingParent(Data.CheckinRequest data, Galant.DataEntity.Paper scanned)
        //{
        //    if (scanned == null) return null;

        //    Galant.DataEntity.Paper[] parents = (from s in scanned.ParentShipments where !s.IsCollection.Value && !s.IsSealed.Value select s).ToArray();
        //    if (parents.Length > 1)
        //    {
        //        // TODO: Multiple tracking parents is not supported
        //        throw new InvalidOperationException("TODO: Multiple tracking parents is not supported");
        //    }

        //    if (parents.Length == 1)
        //    {
        //        // Normal Path: Package is scanned 
        //        return (from s in data.FlatternWorkingList where s.EntityEquals(parents[0]) select s).First();
        //    }
        //    else if (!scanned.IsSealed.Value)
        //    {
        //        if (!scanned.IsCollection.Value)
        //        {
        //            // Shipment Record is scanned
        //            return scanned;
        //        }
        //        else
        //        {
        //            // TODO: Big Seal Package is scanned
        //            throw new InvalidOperationException("Finish checkin must have a tracking shipment parent (1)");
        //        }
        //    }
        //    return null;
        //}

        private void SignSuccess_TextChanged(object sender, TextChangedEventArgs e)
        {//修改成功签收信息
            //if (this.textSuccessSignInfo.IsFocused)
            //{
            //    string signInfo = this.textSuccessSignInfo.Text.Trim();
            //    foreach (Galant.DataEntity.Paper sm in this.listSuccess.SelectedItems.OfType<Galant.DataEntity.Paper>())
            //    {
            //        //if (sm.FinishingCanHasSignInfo) { sm.SignInfo = signInfo; }
            //    }
            //}
        }

        private void reasonReGLTWarter_Selected(object sender, RoutedEventArgs e)
        {
            //foreach (Galant.DataEntity.Paper sm in this.listReGLTWarter.SelectedItems.OfType<Galant.DataEntity.Paper>())
            //{
            //    if (sm.FinishingCanHasException)
            //    {
            //        if (sm.FinishingException == null)
            //            sm.FinishingException = new Data.EventLog();
            //        sm.FinishingException.DigestedEventData["R"] = this.reasonReGLTWarter.ExceptionEvent.DigestedEventData["R"];
            //        sm.FinishingException.DigestedEventData["C"] = this.reasonReGLTWarter.ExceptionEvent.DigestedEventData["C"];
            //    }
            //}
            //((Data.CheckinRequest)this.dataCurrent).PropertyChanged_CheckinList();
        }

        private void reasonRefuse_Selected(object sender, RoutedEventArgs e)
        {            
            //foreach (Galant.DataEntity.Paper sm in this.listRefuse.SelectedItems.OfType<Galant.DataEntity.Paper>())
            //{
            //    if (sm.FinishingCanHasException)
            //    {
            //        if (sm.FinishingException == null)
            //            sm.FinishingException = new Data.EventLog();
            //        sm.FinishingException.DigestedEventData["R"] = this.reasonRefuse.ExceptionEvent.DigestedEventData["R"];
            //        sm.FinishingException.DigestedEventData["C"] = this.reasonRefuse.ExceptionEvent.DigestedEventData["C"];
            //    }
            //}
            //((Data.CheckinRequest)this.dataCurrent).PropertyChanged_CheckinList();
        }

        private void ResultXceedSelectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {//根据选择成功包裹，修改签收信息
            SelectedSuccessRowsSign();
            BindSelectedSuccessRowsCurrencyAmounts();
        }

        private void ReGLTWarterGLTWarterXceedSelectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {//根据选择二次包裹，修改异常信息
            //SelectedExceptionRowsReason(this.listReGLTWarter,this.reasonReGLTWarter);
        }
        private void GLTWarterToReturnXceedSelectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {//根据选择二次包裹，修改异常信息
            //SelectedExceptionRowsReason(this.listRefuse, this.reasonRefuse);
        }   

        /// <summary>
        /// 读取所有已选行，得到异常原因信息。
        /// </summary>
        /// <param name="p_DataGrid"></param>
        /// <param name="p_Selector"></param>
        private void SelectedExceptionRowsReason(Xceed.Wpf.DataGrid.DataGridControl p_DataGrid,GLTWarter.Controls.CheckinExceptionReasonSelector p_Selector)
        {             
            //Data.EventLog log = null;
            //bool isSetEventCode = true;
            //bool isSetComm = true;
            //foreach (Galant.DataEntity.Paper sm in new List<Galant.DataEntity.Paper>(p_DataGrid.SelectedItems.OfType<Galant.DataEntity.Paper>()))
            //{
            //    if (log == null)
            //    {
            //        log = new Data.EventLog();
            //        log.EventData = sm.FinishingException == null ? string.Empty : sm.FinishingException.EventData;
            //    }
             
            //    if (sm.FinishingException == null || (
            //        !sm.FinishingException.DigestedEventData.ContainsKey("R") ||
            //        !log.DigestedEventData.ContainsKey("R") ||
            //        sm.FinishingException.DigestedEventData["R"] != log.DigestedEventData["R"])) //异常原因不一样直接跳出
            //    {
            //        isSetEventCode = false;
            //        break;
            //    }
            //    if (isSetComm && (
            //        !sm.FinishingException.DigestedEventData.ContainsKey("C") ||
            //        !log.DigestedEventData.ContainsKey("C") ||
            //        sm.FinishingException.DigestedEventData["C"] != log.DigestedEventData["C"]))
            //    {
            //        isSetComm = false;
            //        log.DigestedEventData["C"] = string.Empty;
            //    }
            //}
            //if (p_DataGrid.SelectedItems.Count > 1)
            //        p_Selector.IsRiaseSelectedEvent = false;
            //if (isSetEventCode && log != null)
            //{
            //    p_Selector.DataContext = log;
            //}
            //else
            //{
            //    log = new Data.EventLog();
            //    log.DigestedEventData["C"] = string.Empty;
            //    log.DigestedEventData["R"] = string.Empty;
            //    p_Selector.DataContext = log;
            //}
        }

        /// <summary>
        /// 读取所有已选行，得到签收信息。
        /// </summary>
        /// <returns></returns>
        private void SelectedSuccessRowsSign()
        {
            string signInfo = String.Empty;
            bool isSet = false;
            //foreach (Galant.DataEntity.Paper sm in new List<Galant.DataEntity.Paper>(listSuccess.SelectedItems.OfType<Galant.DataEntity.Paper>()))
            //{
            //    if (!isSet)
            //    {
            //        signInfo = sm.SignInfo ?? string.Empty;
            //        isSet = true;
            //    }
            //    else if (signInfo != (sm.SignInfo ?? string.Empty))
            //    {
            //        signInfo = string.Empty;
            //        break;
            //    }
            //}
            //this.textSuccessSignInfo.Text = signInfo;
        }

        private void BindSelectedSuccessRowsCurrencyAmounts()
        {
            string CurrencyInfo = string.Empty;
            bool isSet = false;
            bool isSame = true;

            //List<string> selectedCurrency = (from s in new List<Galant.DataEntity.Paper>(listSuccess.SelectedItems.OfType<Galant.DataEntity.Paper>()) from c in s.ContactA.SupportedCurrencies select c).Distinct().OrderBy(c => c).ToList();
            //foreach (Galant.DataEntity.Paper sm in listSuccess.SelectedItems.OfType<Galant.DataEntity.Paper>())
            //{
                //if (sm.NewBills == null)
                //    continue;

                //if ((from n in sm.NewBills select n.Currency).Distinct().ToArray().Length > 1)
                //{//如果归班为多种收款方式不允许批量更改收款方式。
                //    selectedCurrency = new List<string>();
                //    break;
                //}

                //selectedCurrency = selectedCurrency.Intersect(sm.ContactA.SupportedCurrencies).ToList();

                ////检查选中行是否选择的一样
                //if (sm.NewBills != null && isSame)
                //{
                //    foreach (Data.Bill bill in sm.NewBills)
                //    {
                //        if (!isSet)
                //        {
                //            CurrencyInfo = bill.Currency;
                //            isSet = true;
                //        }
                //        else if (CurrencyInfo != bill.Currency)
                //        {
                //            isSame = false;
                //            CurrencyInfo = string.Empty;
                //        }
                //    }
                //}
            //}
            //this.comboSuccessCurrency.ItemsSource = selectedCurrency;
            //this.comboSuccessCurrency.SelectedValue = CurrencyInfo;
            //this.comboSuccessCurrency.IsEnabled = (selectedCurrency.Count > 0);
        }

        private void rdoSuccess_Checked(object sender, RoutedEventArgs e)
        {
            if (rowScanningCurrency != null && rdoSuccess != null)
                rowScanningCurrency.Visibility = rdoSuccess.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        private void comboSuccessCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//修改收款方式
            //if (this.comboSuccessCurrency.SelectedItem == null)
            //    return;

            //foreach (Galant.DataEntity.Paper sm in new List<Galant.DataEntity.Paper>(listSuccess.SelectedItems.OfType<Galant.DataEntity.Paper>()))
            //{
            //    if (sm.NewBills == null) continue;
            //    foreach (Data.Bill bill in sm.NewBills)
            //    {
            //        bill.Currency = this.comboSuccessCurrency.SelectedItem.ToString();
            //    }
            //}
            //((Data.CheckinRequest)dataCurrent).PropertyChanged_CheckinList();
        }
    }
}