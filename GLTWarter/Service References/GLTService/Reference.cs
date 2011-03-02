﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GLTWarter.GLTService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GLTService.IServiceAPI")]
    public interface IServiceAPI {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceAPI/GetData", ReplyAction="http://tempuri.org/IServiceAPI/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IServiceAPI/GetData", ReplyAction="http://tempuri.org/IServiceAPI/GetDataResponse")]
        System.IAsyncResult BeginGetData(int value, System.AsyncCallback callback, object asyncState);
        
        string EndGetData(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceAPI/DoRequest", ReplyAction="http://tempuri.org/IServiceAPI/DoRequestResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Package))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Product))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Paper))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Route))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.AppStatusCach))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Entity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.Role))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Galant.DataEntity.EventLog))]
        Galant.DataEntity.BaseData DoRequest(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IServiceAPI/DoRequest", ReplyAction="http://tempuri.org/IServiceAPI/DoRequestResponse")]
        System.IAsyncResult BeginDoRequest(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType, System.AsyncCallback callback, object asyncState);
        
        Galant.DataEntity.BaseData EndDoRequest(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceAPIChannel : GLTWarter.GLTService.IServiceAPI, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DoRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public DoRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Galant.DataEntity.BaseData Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Galant.DataEntity.BaseData)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceAPIClient : System.ServiceModel.ClientBase<GLTWarter.GLTService.IServiceAPI>, GLTWarter.GLTService.IServiceAPI {
        
        private BeginOperationDelegate onBeginGetDataDelegate;
        
        private EndOperationDelegate onEndGetDataDelegate;
        
        private System.Threading.SendOrPostCallback onGetDataCompletedDelegate;
        
        private BeginOperationDelegate onBeginDoRequestDelegate;
        
        private EndOperationDelegate onEndDoRequestDelegate;
        
        private System.Threading.SendOrPostCallback onDoRequestCompletedDelegate;
        
        public ServiceAPIClient() {
        }
        
        public ServiceAPIClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceAPIClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAPIClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceAPIClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<GetDataCompletedEventArgs> GetDataCompleted;
        
        public event System.EventHandler<DoRequestCompletedEventArgs> DoRequestCompleted;
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetData(int value, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetData(value, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string EndGetData(System.IAsyncResult result) {
            return base.Channel.EndGetData(result);
        }
        
        private System.IAsyncResult OnBeginGetData(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int value = ((int)(inValues[0]));
            return this.BeginGetData(value, callback, asyncState);
        }
        
        private object[] OnEndGetData(System.IAsyncResult result) {
            string retVal = this.EndGetData(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetDataCompleted(object state) {
            if ((this.GetDataCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetDataCompleted(this, new GetDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetDataAsync(int value) {
            this.GetDataAsync(value, null);
        }
        
        public void GetDataAsync(int value, object userState) {
            if ((this.onBeginGetDataDelegate == null)) {
                this.onBeginGetDataDelegate = new BeginOperationDelegate(this.OnBeginGetData);
            }
            if ((this.onEndGetDataDelegate == null)) {
                this.onEndGetDataDelegate = new EndOperationDelegate(this.OnEndGetData);
            }
            if ((this.onGetDataCompletedDelegate == null)) {
                this.onGetDataCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetDataCompleted);
            }
            base.InvokeAsync(this.onBeginGetDataDelegate, new object[] {
                        value}, this.onEndGetDataDelegate, this.onGetDataCompletedDelegate, userState);
        }
        
        public Galant.DataEntity.BaseData DoRequest(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType) {
            return base.Channel.DoRequest(composite, staff, OperationType);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginDoRequest(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDoRequest(composite, staff, OperationType, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Galant.DataEntity.BaseData EndDoRequest(System.IAsyncResult result) {
            return base.Channel.EndDoRequest(result);
        }
        
        private System.IAsyncResult OnBeginDoRequest(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Galant.DataEntity.BaseData composite = ((Galant.DataEntity.BaseData)(inValues[0]));
            Galant.DataEntity.Entity staff = ((Galant.DataEntity.Entity)(inValues[1]));
            string OperationType = ((string)(inValues[2]));
            return this.BeginDoRequest(composite, staff, OperationType, callback, asyncState);
        }
        
        private object[] OnEndDoRequest(System.IAsyncResult result) {
            Galant.DataEntity.BaseData retVal = this.EndDoRequest(result);
            return new object[] {
                    retVal};
        }
        
        private void OnDoRequestCompleted(object state) {
            if ((this.DoRequestCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DoRequestCompleted(this, new DoRequestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DoRequestAsync(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType) {
            this.DoRequestAsync(composite, staff, OperationType, null);
        }
        
        public void DoRequestAsync(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType, object userState) {
            if ((this.onBeginDoRequestDelegate == null)) {
                this.onBeginDoRequestDelegate = new BeginOperationDelegate(this.OnBeginDoRequest);
            }
            if ((this.onEndDoRequestDelegate == null)) {
                this.onEndDoRequestDelegate = new EndOperationDelegate(this.OnEndDoRequest);
            }
            if ((this.onDoRequestCompletedDelegate == null)) {
                this.onDoRequestCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDoRequestCompleted);
            }
            base.InvokeAsync(this.onBeginDoRequestDelegate, new object[] {
                        composite,
                        staff,
                        OperationType}, this.onEndDoRequestDelegate, this.onDoRequestCompletedDelegate, userState);
        }
    }
}
