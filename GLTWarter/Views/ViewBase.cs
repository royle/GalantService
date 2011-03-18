using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GLTWarter.Views
{
    /// <summary>
    /// State object that to be returned by ViewBase
    /// </summary>
    public abstract class ViewReturnStateBase { };
    public class ViewReturnCancel : ViewReturnStateBase { };
    
    /// <summary>
    /// State object that signal going back all the way to the welcome page, then another page
    /// </summary>
    public class ViewReturnGoToPage : ViewReturnCancel
    {
        public ViewBase Page { get; protected set; }
        public ViewReturnGoToPage(ViewBase page)
        {
            this.Page = page;
        }
    }

    public class ViewBase : PageFunction<ViewReturnStateBase>, INotifyPropertyChanged, IDataErrorInfo
    {
        static log4net.ILog Logger = log4net.LogManager.GetLogger("ViewServiceFault");

        #region Deprecated
        /// <summary>
        /// Satisfy deprecated attribute for XAML style
        /// </summary>
        public bool IsLatestVersion { get { return true; } }
        /// <summary>
        /// Satisfy deprecated attribute for XAML style
        /// </summary>
        public bool IsDirty { get { return true; } }
        /// <summary>
        /// Satisfy deprecated attribute for XAML style
        /// </summary>
        public bool IsNew { get { return true; } }
        #endregion

        #region Service States
        object processLock = new object();

        /// <summary>
        /// State container for async service call
        /// </summary>
        class ServiceCallState
        {
            public object State { get; set; }
            public bool LockUI{get;set;}
        }

        #endregion

        /// <summary>
        /// The base object that will be used for Refresh
        /// Refresh button will be only available if this is set, and IsRefreshable is set to true.
        /// </summary>
        // protected virtual DataTransferObjectBase BaseObject { set; get; }

        public ViewBase()
        {
            this.KeepAlive = true;
            this.DataContext = this;

            this.AddHandler(Page.GotFocusEvent, new RoutedEventHandler(Page_RememberFocus), true);
            this.AddHandler(Page.UnloadedEvent, new RoutedEventHandler(ViewBase_Unloaded), true);
            this.Loaded += new System.Windows.RoutedEventHandler(ViewBase_Loaded);
        }

        /*
        protected ViewBase(DataTransferObjectBase baseObject)
            : this()
        {
            this.BaseObject = baseObject;
        }
         */


        #region UI Behavior
        int lockUICount = 0;

        /// <summary>
        /// Put the view in the locked state
        /// The call is stackable - It can be called multiple times, and same number of call to <see cref="DecreaseLockUICount"/> is needed to release the UI
        /// </summary>
        protected void IncreaseLockUICount()
        {
            Interlocked.Increment(ref lockUICount);
            OnPropertyChangedInternal("IsUIReady");
        }

        /// <summary>
        /// Release the view from the locked state
        /// </summary>
        protected void DecreaseLockUICount()
        {
            int ret = Interlocked.Decrement(ref lockUICount) ;
            Debug.Assert(ret >= 0, "More DecreaseLockUICount has been called than IncreaseLockUICount");
            if (ret == 0)
            {
                Restorefocus();
            }                   
            OnPropertyChangedInternal("IsUIReady");
        }

        /// <summary>
        /// Return if UI is ready or not
        /// </summary>
        public bool IsUIReady { get { return lockUICount == 0; } }

        IInputElement lastFocusElement;

        CommandBinding cb_Forward;
        CommandBinding cb_Back;
        CommandBinding cb_Refresh;
        CommandBindingCollection FrameCommandBindings;

        void ViewBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameCommandBindings = Utils.FindVisualParent<Frame>(this).CommandBindings;

            cb_Refresh = new CommandBinding(NavigationCommands.Refresh);
            cb_Refresh.Executed += new ExecutedRoutedEventHandler(OnExecuteRefresh);
            cb_Refresh.CanExecute += new CanExecuteRoutedEventHandler(OnQueryRefresh);
            FrameCommandBindings.Insert(0, cb_Refresh);

            cb_Forward = new CommandBinding(NavigationCommands.BrowseForward);
            cb_Forward.CanExecute += new CanExecuteRoutedEventHandler(OnQueryForward);
            cb_Forward.PreviewExecuted += new ExecutedRoutedEventHandler(OnExecuteForward);
            FrameCommandBindings.Insert(0, cb_Forward);

            cb_Back = new CommandBinding(NavigationCommands.BrowseBack);
            cb_Back.CanExecute += new CanExecuteRoutedEventHandler(OnQueryBack);
            cb_Back.PreviewExecuted += new ExecutedRoutedEventHandler(OnExecuteBack);
            FrameCommandBindings.Insert(0, cb_Back);

            Restorefocus();
        }

        /// <summary>
        /// Represent if this view can be Refreshed by F5.
        /// Default is based on the presences of the base object. Implementation should override this if needed.
        /// </summary>
        public virtual bool IsRefreshable
        {
            // get { return BaseObject != null && IsUIReady; }
            get { return false; }
        }

        /// <summary>
        /// On F5 or Refresh command, this will be called.
        /// </summary>
        public virtual void OnRefresh()
        {
            // SendServiceCall(BaseObject, null);
        }

        /// <summary>
        /// If ButtonNext is not null, on browser forward, this button is clicked instead. 
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

        void ViewBase_Unloaded(object sender, RoutedEventArgs e)
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
            e.CanExecute = IsRefreshable;
            e.Handled = true;
        }

        void OnExecuteRefresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OnRefresh();
        }

        void OnQueryForward(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ButtonNext != null)
            {
                // Claim that we can handle this to override the basic functionality
                e.Handled = true;
                e.CanExecute = true;
            }
        }

        void OnExecuteForward(object sender, ExecutedRoutedEventArgs e)
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

        void OnQueryBack(object sender, CanExecuteRoutedEventArgs e)
        {
            if (BackAllowed == false)
            {
                // Claim that we can handle this to override the basic functionality
                e.Handled = true;
                e.CanExecute = true;
            }
        }

        void OnExecuteBack(object sender, ExecutedRoutedEventArgs e)
        {
            if (BackAllowed == false)
            {
                e.Handled = true;
                OnCancel();
            }
        }

        /// <summary>
        /// When user pressed the Back button (Alt+Left), this will be called.
        /// The default implementation is OnReturn(null)
        /// </summary>
        protected virtual void OnBack()
        {
            OnReturn(null);
        }


        /// <summary>
        /// Return (back) this page and signal the caller that this is cancelled.
        /// The caller should listen to the Return event, and compare if the returned value is ViewReturnCancel.
        /// </summary>
        protected void OnCancel()
        {
            OnReturn(new ViewReturnCancel());
        }

        /// <summary>
        /// Return the page with specific view return state
        /// </summary>
        protected void OnReturn(ViewReturnStateBase state)
        {
            base.OnReturn(new ReturnEventArgs<ViewReturnStateBase>(state));
        }

        /// <summary>
        /// A <see cref="ReturnEventHandler"/> that will call <see cref="OnCancel"/> in a cascade fashion.
        /// That is, if the page hooked to has returned a cancel event, a cancel event will be called.
        /// </summary>
        protected void ViewBase_CascadeCancel(object sender, ReturnEventArgs<ViewReturnStateBase> e)
        {
            if (e != null && e.Result is ViewReturnCancel)
            {
                OnReturn(e);
            }
        }

        void Page_RememberFocus(object sender, RoutedEventArgs e)
        {
            if (IsUIReady)
            {
                BackupFocus();
            }
        }

        /// <summary>
        /// Set the focus to some input element. It works even if UI is not ready.
        /// </summary>
        /// <param name="element">The input element that should have the focus</param>
        protected void SetFocus(IInputElement element)
        {
            lastFocusElement = element;
            element.Focus();
        }

        void Restorefocus()
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

        void BackupFocus()
        {
            // FIXME: it is broken now. (It works when it's breakpoint'ed though)
            FrameworkElement dpo = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) as FrameworkElement;
            if (dpo != null)
            {
                //Xceed.Wpf.DataGrid.DataGridControl dgc = Utils.FindVisualParent<Xceed.Wpf.DataGrid.DataGridControl>(dpo);
                //if (dgc != null)
                //{
                //    lastFocusElement = dgc;
                //    return;
                //}
            }
            lastFocusElement = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this));
        }

        /// <summary>
        /// This function will be called when loading finish. Intended for setting the focus to the first control.
        /// The first available focusable control will be focus. 
        /// Implementation could override the logic if necessary.
        /// </summary>
        protected virtual void FocusFirstControl()
        {
            IInputElement element = Utils.GetLeafFocusableChild(this);
            if (element != null)
                element.Focus();
        }

        protected void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // TODO: AppCurrent.Active.MainScreen.NavigateEntityDetails(((Hyperlink)sender).Tag);
        }        
        #endregion

#if false
        #region Service Call
        /// <summary>
        /// Send the request DTO to the service call. The UI will be locked until the process complete. Using the same request type that "Refresh" use.
        /// </summary>
        /// <param name="request">The DTO to be sent</param>
        /// <param name="state">User defined state object</param>
        protected void SendServiceCall(DataTransferObjectBase request, object state)
        {
            IncreaseLockUICount();
            OnBeginServiceCall(request,
                new ServiceCallState()
                {
                    State = state
                }
            );
        }

        /// <summary>
        /// This should send a request, and use <see cref="ServiceCallback"/> as the AsyncCallback.
        /// The default implementation will send the request through RpcHelper.Current
        /// </summary>
        /// <param name="request">The DTO to be sent</param>
        /// <param name="state">Async object state</param>
        protected virtual void OnBeginServiceCall(DataTransferObjectBase request, object state)
        {
            RpcHelper.Current.BeginCallEx(request, ServiceCallback, state);
        }

        /// <summary>
        /// This should conclude the request by call EndCall.
        /// The default implementation will conclude the request through RpcHelper.Current
        /// </summary>
        /// <param name="asr">The IAsyncResult state from the callback</param>
        protected virtual DataTransferObjectBase OnEndServiceCall(IAsyncResult asr)
        {
            return RpcHelper.Current.EndCall(asr);
        }

        /// <summary>
        /// Implementation must inherit this to carry out operation when a DTO is received from EndCall.
        /// </summary>
        /// <param name="result">The DTO received</param>
        /// <param name="state">Async object state</param>
        protected virtual void OnServiceProcessCall(DataTransferObjectBase result, object state) { }

        /// <summary>
        /// This is the callback endpoint for Service Call.
        /// </summary>
        /// <param name="asr">The IAsyncResult state assigned</param>
        protected void ServiceCallback(GLTWarterRpcCallCompletedEventArgs e)
        {
            ServiceCallState state = e.UserState as ServiceCallState;

            if (e.Result != null)
            {
                OnServiceProcessCall(e.Result, state.State);
            }
            else
            {
                HandleServiceFault(e.Exception);
            }

            DecreaseLockUICount();
        }

        /// <summary>
        /// For view that expected view specified fault, it should override and handle it here.
        /// <see cref="InjectError"/> is useful to inject validation error.
        /// </summary>
        /// <param name="fault">The service fault returned</param>
        /// <returns>Return true if fault is handled. Otherwise it should return false and it will be logged and handled accordingly.</returns>
        virtual protected bool OnServiceProcessFault(FaultException<ServiceFault> fault)
        {
            return false;
        }

        protected void HandleServiceFault(CommunicationException ex)
        {
            if (ex is MessageSecurityException)
            {
                ex = new FaultException<ServiceFault>(new ServiceFault(), string.Empty, new FaultCode(ServiceFaultCode.Auth));
            }
            if (ex is FaultException<ServiceFault>)
            {
                bool isReportNeeded;
                FaultException<ServiceFault> fault = ex as FaultException<ServiceFault>;

                bool viewHandled = fault.Code != null && this.OnServiceProcessFault(fault);
                if (!viewHandled)
                {
                    Logger.Error(
                        string.Format(CultureInfo.InvariantCulture,
                        "{0}: {1}\nState:\n{3}\nStack:\n{2}",
                        fault.Code != null ? fault.Code.Name : string.Empty,
                        fault.Message,
                        fault.Detail.State ?? "<null>",
                        fault.Detail.StackTrace
                        ));

                    string message = FaultHelper.ConvertToMessage(fault, out isReportNeeded);

                    if (message != null)
                    {
                        InjectError(string.Empty, message);
                    }
                    else
                    {
                        InjectError(string.Empty, FaultResource.errorUnknownCode);
                    }
                }
            }
            else if (ex is ServerTooBusyException || ex is EndpointNotFoundException || ex is ServiceActivationException)
            {
                InjectError(string.Empty, FaultResource.errorCommunication);
            }
            else
            {
                InjectError(string.Empty, FaultResource.errorUnknown);
            }
        }
        #endregion
#endif

        #region INotifyPropertyChanged Members
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChangedInternal(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region Validations and IDataErrorInfo Members
        Dictionary<string, string[]> errorStrings = new Dictionary<string, string[]>();
        List<string> shouldValidateProperties;

        /// <summary>
        /// The Validator delegate for property validator. A validator must be sent for GetIsValid check.
        /// </summary>
        /// <param name="name">Property that to be checked. If this is empty, the whole UI should be validated as a whole.</param>
        /// <returns>Errors found during validation. Null means no error.</returns>
        public delegate string[] Validator(string name);

        /// <summary>
        /// The default Validator. Implementation could implement this if only one validator is needed, 
        /// and this will be used for <see cref="OnPropertyChanged"/> and <see cref="GetIsValid"/> with no validator supplied.
        /// </summary>
        /// <param name="name">Property that to be checked. If this is empty, the whole UI should be validated as a whole.</param>
        /// <returns>Errors found during validation. Null means no error.</returns>
        protected virtual string[] OnValidateProperty(string name)
        {
            return null;
        }

        protected void InjectError(string name, string message)
        {
            lock (errorStrings)
            {
                // If message is null, we just want the field to be marked. 
                // So create an empty array.
                string[] messages = 
                    message == null ?
                    new string[] { } :
                    new string[] { message };
                if (errorStrings.ContainsKey(name))
                {
                    errorStrings[name] = errorStrings[name].Concat(messages).ToArray();
                }
                else
                {
                    errorStrings[name] = messages;
                }
            }
            OnPropertyChangedInternal(name);
            OnPropertyChangedInternal("Errors");
        }

        bool ValidateProperty(Validator validator, string name)
        {
            string[] errors = validator(name);
            lock (errorStrings)
            {
                if (errors != null)
                {
                    errorStrings[name] = errors;
                }
                else
                {
                    errorStrings.Remove(name);
                }
            }
            return errors == null;
        }


        /// <summary>
        /// Revalidate all the properties (Only those Public Writable properties will be validated) using default validator
        /// </summary>
        /// <returns>True if everything is valid</returns>
        protected bool GetIsValid()
        {
            return GetIsValid(OnValidateProperty);
        }

        /// <summary>
        /// Revalidate all the properties (Only those explicitly marked properties will be validated)
        /// </summary>
        /// <param name="validator">The validator that will be used for validation.</param>
        /// <returns>True if everything is valid</returns>
        protected bool GetIsValid(Validator validator)
        {
            lock (errorStrings)
            {
                errorStrings.Clear();

                if (shouldValidateProperties == null)
                {
                    shouldValidateProperties =
                        (from m in this.GetType().GetProperties()
                         let p = m.GetCustomAttributes(typeof(ShouldValidateAttribute), true)
                         where p.Length == 1
                         orderby ((ShouldValidateAttribute)p[0]).Order
                         select m.Name).ToList();
                }
                foreach (string name in shouldValidateProperties)
                {
                    ValidateProperty(validator, name);
                    OnPropertyChangedInternal(name);
                }
                ValidateProperty(validator, string.Empty);
                OnPropertyChangedInternal("Errors");
                return errorStrings.Count == 0;
            }
        }

        public virtual string Error
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string this[string columnName]
        {
            get
            {
                lock (errorStrings)
                {
                    if (errorStrings.ContainsKey(columnName))
                    {
                        return string.Join(Environment.NewLine, errorStrings[columnName]);
                    }
                }
                return string.Empty;
            }
        }

        public virtual string[] Errors
        {
            get
            {
                return errorStrings.Values.SelectMany(values => values).ToArray();
            }
        }

        /// <summary>
        /// Calling this will remove all the validation errors
        /// </summary>
        protected void ClearInvalid()
        {
            List<string> keys = errorStrings.Keys.ToList();
            errorStrings.Clear();
            foreach (string name in keys)
            {
                OnPropertyChangedInternal(name);
            }
            OnPropertyChangedInternal("Errors");
        }

        static Regex regexSetterFrame = new Regex("^set_(.+)$");

        /// <summary>
        /// This should be called to propagate the property change of the current property to the UI, and do the validation using the default Validator.
        /// The property name will be determined by reflectoring. Only use this in the setter of the property.
        /// </summary>
        protected void OnPropertyChanged()
        {
            StackFrame frame = new StackTrace().GetFrame(1);
            Match m = regexSetterFrame.Match(frame.GetMethod().Name);
            if (!m.Success)
                throw new InvalidOperationException("Unable to extract the proprety setter and its name from the stack. Consider using OnPropertyChanged(name).");
            OnPropertyChanged(OnValidateProperty, m.Groups[1].Value);
        }


        /// <summary>
        /// This should be called to propagate the property change of the current property to the UI, and do the validation using the specified Validator.
        /// The property name will be determined by reflectoring. Only use this in the setter of the property.
        /// </summary>
        protected void OnPropertyChanged(Validator validator)
        {
            StackFrame frame = new StackTrace().GetFrame(1);
            Match m = regexSetterFrame.Match(frame.GetMethod().Name);
            if (!m.Success)
                throw new InvalidOperationException("Unable to extract the proprety setter and its name from the stack. Consider using OnPropertyChanged(name).");
            OnPropertyChanged(validator, m.Groups[1].Value);
        }

        /// <summary>
        /// This should be called to propagate the property change to the UI, and do the validation using the default Validator.
        /// </summary>
        /// <param name="name">The name of the property that got changed</param>
        protected void OnPropertyChanged(string name)
        {
            OnPropertyChanged(OnValidateProperty, name);
        }

        /// <summary>
        /// This should be called to propagate the property change to the UI, and do the validation using the specified Validator.
        /// </summary>
        /// <param name="name">The name of the property that got changed</param>
        protected void OnPropertyChanged(Validator validator, string name)
        {
            if (!ValidateProperty(validator, name))
            {
                OnPropertyChangedInternal("Errors");
            }
            OnPropertyChangedInternal(name);
        }
        #endregion
    }
}