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

namespace GLTWarter.Pages
{
    public class DetailsBase : PageFunction<Galant.DataEntity.BaseData>, INotifyPropertyChanged
    {        
        protected Galant.DataEntity.BaseData dataBackup;
        protected Galant.DataEntity.BaseData dataCurrent;

        protected string originalQueryId;

        protected bool isLastItemNew;

        IInputElement lastFocusElement;

        CommandBinding cb_Forward;
        CommandBinding cb_Back;
        CommandBinding cb_Refresh;
        CommandBindingCollection FrameCommandBindings;

        public DetailsBase() { /*throw new NotImplementedException("Do not use this. This is just to satisfy the designer.");*/ }
            
        public DetailsBase(Galant.DataEntity.BaseData data)
        {
            this.KeepAlive = true;
            this.AddHandler(Page.GotFocusEvent, new RoutedEventHandler(Page_RememberFocus), true);
            this.AddHandler(Page.UnloadedEvent, new RoutedEventHandler(DetailsBase_Unloaded), true);

            if (data == null)
            {
                data = this.CreateNewEntity();
            }
            this.DataContext = data;
            this.dataCurrent = data;
            this.dataBackup = null;
            originalQueryId = data.QueryId;

            dataCurrent.IsLoading = false;
            this.DataReadyRefresh();
            this.Loaded += new RoutedEventHandler(DetailsBase_LoadedOnce);
            this.Loaded += new RoutedEventHandler(DetailsBase_Loaded);
        }

        void DetailsBase_Loaded(object sender, RoutedEventArgs e)
        {
            FrameCommandBindings = Utils.FindVisualParent<Frame>(this).CommandBindings;

            cb_Refresh = new CommandBinding(NavigationCommands.Refresh);
            cb_Refresh.Executed += new ExecutedRoutedEventHandler(OnRefresh);
            cb_Refresh.CanExecute += new CanExecuteRoutedEventHandler(OnQueryRefresh);
            FrameCommandBindings.Insert(0, cb_Refresh);

            cb_Forward = new CommandBinding(NavigationCommands.BrowseForward);
            cb_Forward.CanExecute += new CanExecuteRoutedEventHandler(OnQueryForward);
            cb_Forward.PreviewExecuted += new ExecutedRoutedEventHandler(OnForward);
            FrameCommandBindings.Insert(0, cb_Forward); 

            cb_Back = new CommandBinding(NavigationCommands.BrowseBack);
            cb_Back.CanExecute += new CanExecuteRoutedEventHandler(OnQueryBack);
            cb_Back.PreviewExecuted += new ExecutedRoutedEventHandler(OnBack);
            FrameCommandBindings.Insert(0, cb_Back); 

            Restorefocus();
        }

        void DetailsBase_LoadedOnce(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(DetailsBase_LoadedOnce);

            if (dataCurrent != null) dataCurrent.ClearInvalid();

            OnDataInitialization(null, null);
        }

        protected virtual bool DataRefreshSuppressed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// On browser forward, next button is clicked instead. 
        /// </summary>
        protected virtual Button ButtonNext
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// On browser back, the page is actually cancelled.
        /// Page requires server operation to retreive data must override this.
        /// </summary>
        protected virtual bool BackAllowed
        {
            get
            {
                return true;
            }
        }

        void DetailsBase_Unloaded(object sender, RoutedEventArgs e)
        {
            FrameCommandBindings.Remove(cb_Forward);
            FrameCommandBindings.Remove(cb_Back);
            FrameCommandBindings.Remove(cb_Refresh);
            cb_Forward = null;
            cb_Back = null;
            cb_Refresh = null;
            FrameCommandBindings = null;
        }

        void OnQueryRefresh(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = dataCurrent != null && !dataCurrent.IsNew && !dataCurrent.IsLoading && !this.DataRefreshSuppressed;
            e.Handled = true;
        }

        void OnRefresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            DoRefresh();
        }

        protected void DoRefresh()
        {
            if (dataCurrent != null)
            {
                bool oldIsLoading = dataCurrent.IsLoading;
                if (!dataCurrent.IsNew && !this.DataRefreshSuppressed)
                {
                    if (IsUIReady)
                    {
                        BackupFocus();
                        dataCurrent.IsLoading = true;
                        InvokeEnsureLoaded(null, null);
                        this.DataReadyRefresh();
                    }
                    if (!oldIsLoading)
                    {   
                        GLTService.ServiceAPIClient client = new GLTService.ServiceAPIClient();
                        client.DoRequestCompleted += new EventHandler<GLTService.DoRequestCompletedEventArgs>(PopulateCallback);
                        client.DoRequestAsync(this.DataContext as Galant.DataEntity.BaseData, AppCurrent.Active.StaffCurrent, "Refresh");         
                    }
                }
            }
        }

        protected virtual void OnQueryForward(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ButtonNext != null)
            {
                // Claim that we can handle this to override the basic functionality
                e.Handled = true;
                e.CanExecute = true;
            }
        }

        protected virtual void OnForward(object sender, ExecutedRoutedEventArgs e)
        {
            if (ButtonNext != null)
            {
                e.Handled = true;
                try
                {
                    ((IInvokeProvider)UIElementAutomationPeer.CreatePeerForElement(ButtonNext).GetPattern(PatternInterface.Invoke)).Invoke();
                }
                catch (ElementNotEnabledException)
                {
                }
            }
        }

        protected virtual void OnQueryBack(object sender, CanExecuteRoutedEventArgs e)
        {
            if (BackAllowed == false)
            {
                // Claim that we can handle this to override the basic functionality
                e.Handled = true;
                e.CanExecute = true;
            }
        }

        protected virtual void OnBack(object sender, ExecutedRoutedEventArgs e)
        {
            if (BackAllowed == false)
            {
                e.Handled = true;
                OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(new Data.BackObject()));
            }
        }

        private void Restorefocus()
        {
            if (lastFocusElement != null)
            {
                this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Loaded,
                    (Action)delegate()
                    {
                        lastFocusElement.Focus();
                    }
                );
            }
            else
            {
                this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Loaded,
                    (Action)delegate()
                    {
                        FocusFirstControl();
                    });
            }
        }

        private void Page_RememberFocus(object sender, RoutedEventArgs e)
        {
            if (dataCurrent != null && !dataCurrent.IsLoading)
            {
                BackupFocus();
            }
        }

        /// <summary>
        /// 当出现 用户输入错误时 将焦点返回 最后一次操作的 控件上
        /// </summary>
        private void BackupFocus()
        {
            // FIXME: it is broken now. (It works when it's breakpoint'ed though)
            lastFocusElement = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this));
        }

        protected virtual Galant.DataEntity.BaseData CreateNewEntity() { return null; }

        protected virtual void FocusFirstControl() { }

        protected virtual void OnPostDataInitialization() { }

        protected virtual void OnDataInitialization(object sender, RoutedEventArgs e)
        {
            if (dataCurrent != null)
            {
                if (!dataCurrent.IsNew && !this.DataRefreshSuppressed)
                {
                    this.DoRefresh();
                }
                else
                {
                    dataCurrent.IsLoading = false;
                    OnPostDataInitialization();
                    FocusFirstControl();
                }
                this.DataReadyRefresh();
            }
        }

        protected virtual void PopulateDataMigrate(Galant.DataEntity.BaseData newData, Galant.DataEntity.BaseData oldData)
        {            
        }

        void PopulateCallback(object sender, GLTService.DoRequestCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    try
                    {
                        Galant.DataEntity.BaseData newData = e.Result;//dataCurrent.EndPopulate(asr) as Galant.DataEntity.BaseData;
                        if (newData != null)
                        {
                            if (dataCurrent != null)
                            {
                                newData.IsLoading = true;
                                newData.MigrateFrom(dataCurrent);
                                PopulateDataMigrate(newData, dataCurrent);
                            }
                            dataCurrent = newData;
                            dataBackup = dataCurrent.Clone() as Galant.DataEntity.BaseData;
                            dataBackup.IsLoading = false;
                            this.DataContext = dataCurrent;
                            dataCurrent.IsLoading = false;                            
                            OnPostDataInitialization();
                            this.DataReadyRefresh();
                            this.Restorefocus();
                            return;
                        }
                    }
                   
                    catch (System.Net.WebException ex)
                    {
                       
                        ShowPopulateError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                       
                        ShowPopulateError(ex.ToString());
                    }
                    this.Restorefocus();
                    OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(dataBackup));
                }
            );
        }

        protected void ShowPopulateError(string message)
        {
            MessageBox.Show(AppCurrent.Active.MainWindow, message, this.Title);
        }

        protected virtual void PreCommit()
        {
        }

        protected virtual bool IsDataValid()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                bool ret = false;
                this.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)delegate()
                    {
                        PreCommit();
                        if (dataCurrent != null)
                        {
                            if (dataCurrent.IsValid)
                            {
                                dataCurrent.Normalize();
                                if (dataCurrent.IsValid)
                                {
                                    ret = true;
                                }
                            }
                        }
                    }
                );
                return ret;
            }
            finally
            {
                this.Cursor = null;
            }
        }

        protected void DoOk()
        {
            if (IsDataValid())
            {
                isLastItemNew = dataCurrent.IsNew;
                BackupFocus();
                dataCurrent.IsLoading = true;
                this.DataReadyRefresh();
                //dataCurrent.BeginSave(SaveCallback, null);
            }
        }

        protected void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            InvokeEnsureLoaded((Action)DoOk, null);
        }

        void SaveCallback(IAsyncResult asr)
        {
            this.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    try
                    {
                        Galant.DataEntity.BaseData data = null;//dataCurrent.EndPopulate(asr) as Galant.DataEntity.BaseData;
                        if (data != null)
                        {
                            dataCurrent = data;
                            dataBackup = dataCurrent.Clone() as Galant.DataEntity.BaseData;
                            dataBackup.IsLoading = false;

                            if (this.DataContext as Galant.DataEntity.BaseData != null) (this.DataContext as Galant.DataEntity.BaseData).IsLoading = false;
                            bool isFinishReturn =
                                isLastItemNew ? OnSavedNewItem() : OnSavedEditedItem();
                            this.DataContext = dataCurrent;
                            dataCurrent.IsLoading = false;
                            dataCurrent.ClearInvalid();
                            if (isFinishReturn) OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(data.IsNew ? null : data));
                            dataCurrent.IsLoading = false;
                            this.DataReadyRefresh();
                            this.Restorefocus();
                            return;
                        }
                    }
                    
                    catch (Exception ex)
                    {
                        
                        ShowPopulateError(ex.ToString());
                    }
                    dataCurrent.IsLoading = false;
                    this.DataReadyRefresh();
                    this.Restorefocus();
                }
            );
        }

        protected void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataCurrent != null)
            {
                BackupFocus();
                dataCurrent.IsLoading = true;
                this.DataReadyRefresh();
                //dataCurrent.BeginDelete(DeleteCallback, null);
            }
        }

        void DeleteCallback(IAsyncResult asr)
        {
            this.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (Action)delegate()
                {
                    try
                    {
                        //dataCurrent.EndDelete(asr);
                        dataBackup = dataCurrent.Clone() as Galant.DataEntity.BaseData;
                        dataCurrent.IsLoading = true;
                        bool isFinishReturn = OnDeletedItem();
                        this.DataContext = dataCurrent;
                        dataCurrent.IsLoading = false;
                        dataCurrent.CheckValid();
                        if (isFinishReturn) OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(null));
                        dataCurrent.IsLoading = false;
                        this.DataReadyRefresh();
                        this.Restorefocus();
                        return;
                    }
                   
                    catch (Exception ex)
                    {
                        //App.Logger.Error("Exception occurs while Deleting", ex);
                        ShowPopulateError(ex.ToString());
                    }
                    dataCurrent.IsLoading = false;
                    this.DataReadyRefresh();
                    this.Restorefocus();
                }
            );
        }

        protected void DoNextDry()
        {
            if (IsDataValid())
            {
                dataCurrent.ClearDirty();
                OnNext(dataCurrent);
            }
        }
        protected void buttonNextDry_Click(object sender, RoutedEventArgs e)
        {
            InvokeEnsureLoaded((Action)DoNextDry, null);
        }

        protected void DoNext()
        {
            if (IsDataValid())
            {
                BackupFocus();
                dataCurrent.IsLoading = true;
                this.DataReadyRefresh();
                GLTService.ServiceAPIClient client = new GLTService.ServiceAPIClient();
                client.DoRequestCompleted += new EventHandler<GLTService.DoRequestCompletedEventArgs>(NextCallback);
                client.DoRequestAsync(this.DataContext as Galant.DataEntity.BaseData, AppCurrent.Active.StaffCurrent, (this.DataContext as Galant.DataEntity.BaseData).Operation);                
            }
        }

       
        protected void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            InvokeEnsureLoaded((Action)DoNext, null);
        }

        void NextCallback(object sender, GLTService.DoRequestCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                (System.Action)delegate()
                {
                    try
                    {
                        Galant.DataEntity.BaseData incomingData = e.Result;
                        Galant.DataEntity.BaseData mydata = (Galant.DataEntity.BaseData)dataCurrent;
                        string errorString = this.GetErrorString(incomingData);
                        if (string.IsNullOrEmpty(errorString))
                        {
                            OnNext(incomingData);
                            dataCurrent.IsLoading = false;
                            this.DataReadyRefresh();
                            this.Restorefocus();
                        }
                        else
                        {
                            AppCurrent.Logger.Info(errorString);
                            ShowPopulateError(errorString);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppCurrent.Logger.Error("Exception occurs while Nexting", ex);
                        ShowPopulateError(ex.ToString());
                    }
                    dataCurrent.IsLoading = false;
                    this.DataReadyRefresh();
                    this.Restorefocus();
                }
            );
        }

        string GetErrorString(Galant.DataEntity.BaseData incomingData)
        {
            if (!string.IsNullOrEmpty(incomingData.WCFErrorString))
            {
                return incomingData.WCFErrorString;
            }
            else if (incomingData.WCFFaultCode != null)
            {
                //添加错误代码转换为提示信息方法
            }
            return string.Empty;
        }

        /// <summary>
        /// Exception from RPC callback would be routed to here. Returning true will prevent the default handler from processing.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="message">The message converted from resources</param>
        /// <returns>True if handled</returns>
        protected virtual bool ExceptionCallback(Exception ex, string message)
        {
            return false;
        }

        protected virtual bool OnSavedNewItem() { return true; }
        protected virtual bool OnSavedEditedItem() { return true; }
        protected virtual bool OnDeletedItem() { return true; }
        protected virtual void OnNext(Galant.DataEntity.BaseData incomingData) { }

        protected void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Prompt About to lose change!!
            if (dataBackup != null && originalQueryId != dataBackup.QueryId)
            {
                OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(new Data.BackObject()));
            }
            else
            {
                OnReturn(new ReturnEventArgs<Galant.DataEntity.BaseData>(
                    dataBackup ?? 
                    (dataCurrent != null ? 
                        (dataCurrent.IsNew ? new Data.BackObject() : null) 
                        : null)
                    )
                );
            }
        }

        protected void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
           // App.Active.MainScreen.NavigateEntityDetails(((Hyperlink)sender).Tag);
        }

        protected virtual void pageNext_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            OnReturn(e);
        }

        protected void pageOp_Return(object sender, ReturnEventArgs<Galant.DataEntity.BaseData> e)
        {
            if (Data.BackObject.IsReturnGenuine(e)) DoRefresh();
        }
        
        /// <summary>
        /// Override this to true if UI is to be immediately available to user
        /// </summary>
        protected virtual bool IsLoadingAsync
        {
            get
            {
                return false;
            }
        }

        private void DataReadyRefresh()
        {
            lock (AfterLoadInvokeList)
            {
                if (ReadyCheck())
                {
                    foreach (KeyValuePair<Delegate, Object[]> invokePair in AfterLoadInvokeList)
                    {
                        if (invokePair.Key != null)
                        {
                            this.Dispatcher.Invoke(
                                System.Windows.Threading.DispatcherPriority.Input,
                                (Action)delegate()
                                {
                                    invokePair.Key.DynamicInvoke(invokePair.Value);
                                });
                        }
                    }
                    AfterLoadInvokeList.Clear();
                }
                OnPropertyChangedInternal("IsUIReady");
            }
        }

        private bool ReadyCheck()
        {
            return !(dataCurrent == null || dataCurrent.IsLoading);
        }

        public bool IsUIReady
        {
            get
            {
                lock (AfterLoadInvokeList)
                {
                    return ReadyCheck() || IsLoadingAsync && AfterLoadInvokeList.Count == 0;
                }
            }
        }

        List<KeyValuePair<Delegate, Object[]>> AfterLoadInvokeList = new List<KeyValuePair<Delegate,object[]>>();
        protected void InvokeEnsureLoaded(Delegate method, object[] arguements)
        {
            lock (AfterLoadInvokeList)
            {
                if (ReadyCheck())
                {
                    method.DynamicInvoke(arguements);
                }
                else
                {
                    AfterLoadInvokeList.Add(new KeyValuePair<Delegate, object[]>(method, arguements));
                    OnPropertyChangedInternal("IsUIReady");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChangedInternal(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
