﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace WinFormsClient.localhost {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="DBWebServiceSoap", Namespace="localhost")]
    public partial class DBWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetEmployeeListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetMessageListOperationCompleted;
        
        private System.Threading.SendOrPostCallback InsertMessageOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public DBWebService() {
            this.Url = global::WinFormsClient.Properties.Settings.Default.WinFormsClient_localhost_DBWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetEmployeeListCompletedEventHandler GetEmployeeListCompleted;
        
        /// <remarks/>
        public event GetMessageListCompletedEventHandler GetMessageListCompleted;
        
        /// <remarks/>
        public event InsertMessageCompletedEventHandler InsertMessageCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("localhost/GetEmployeeList", RequestNamespace="localhost", ResponseNamespace="localhost", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Employee[] GetEmployeeList() {
            object[] results = this.Invoke("GetEmployeeList", new object[0]);
            return ((Employee[])(results[0]));
        }
        
        /// <remarks/>
        public void GetEmployeeListAsync() {
            this.GetEmployeeListAsync(null);
        }
        
        /// <remarks/>
        public void GetEmployeeListAsync(object userState) {
            if ((this.GetEmployeeListOperationCompleted == null)) {
                this.GetEmployeeListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEmployeeListOperationCompleted);
            }
            this.InvokeAsync("GetEmployeeList", new object[0], this.GetEmployeeListOperationCompleted, userState);
        }
        
        private void OnGetEmployeeListOperationCompleted(object arg) {
            if ((this.GetEmployeeListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetEmployeeListCompleted(this, new GetEmployeeListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("localhost/GetMessageList", RequestNamespace="localhost", ResponseNamespace="localhost", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Message[] GetMessageList() {
            object[] results = this.Invoke("GetMessageList", new object[0]);
            return ((Message[])(results[0]));
        }
        
        /// <remarks/>
        public void GetMessageListAsync() {
            this.GetMessageListAsync(null);
        }
        
        /// <remarks/>
        public void GetMessageListAsync(object userState) {
            if ((this.GetMessageListOperationCompleted == null)) {
                this.GetMessageListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMessageListOperationCompleted);
            }
            this.InvokeAsync("GetMessageList", new object[0], this.GetMessageListOperationCompleted, userState);
        }
        
        private void OnGetMessageListOperationCompleted(object arg) {
            if ((this.GetMessageListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMessageListCompleted(this, new GetMessageListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("localhost/InsertMessage", RequestNamespace="localhost", ResponseNamespace="localhost", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void InsertMessage(Message message) {
            this.Invoke("InsertMessage", new object[] {
                        message});
        }
        
        /// <remarks/>
        public void InsertMessageAsync(Message message) {
            this.InsertMessageAsync(message, null);
        }
        
        /// <remarks/>
        public void InsertMessageAsync(Message message, object userState) {
            if ((this.InsertMessageOperationCompleted == null)) {
                this.InsertMessageOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsertMessageOperationCompleted);
            }
            this.InvokeAsync("InsertMessage", new object[] {
                        message}, this.InsertMessageOperationCompleted, userState);
        }
        
        private void OnInsertMessageOperationCompleted(object arg) {
            if ((this.InsertMessageCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InsertMessageCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="localhost")]
    public partial class Employee {
        
        private int employeeIdField;
        
        private string nameField;
        
        /// <remarks/>
        public int EmployeeId {
            get {
                return this.employeeIdField;
            }
            set {
                this.employeeIdField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="localhost")]
    public partial class Message {
        
        private int messageIdField;
        
        private string titleField;
        
        private System.DateTime dateField;
        
        private string contentField;
        
        private int recipientIdField;
        
        private int senderIdField;
        
        private string recipientField;
        
        private string senderField;
        
        /// <remarks/>
        public int MessageId {
            get {
                return this.messageIdField;
            }
            set {
                this.messageIdField = value;
            }
        }
        
        /// <remarks/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime Date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        public string Content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        public int RecipientId {
            get {
                return this.recipientIdField;
            }
            set {
                this.recipientIdField = value;
            }
        }
        
        /// <remarks/>
        public int SenderId {
            get {
                return this.senderIdField;
            }
            set {
                this.senderIdField = value;
            }
        }
        
        /// <remarks/>
        public string Recipient {
            get {
                return this.recipientField;
            }
            set {
                this.recipientField = value;
            }
        }
        
        /// <remarks/>
        public string Sender {
            get {
                return this.senderField;
            }
            set {
                this.senderField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetEmployeeListCompletedEventHandler(object sender, GetEmployeeListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetEmployeeListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetEmployeeListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Employee[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Employee[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetMessageListCompletedEventHandler(object sender, GetMessageListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMessageListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetMessageListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Message[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Message[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void InsertMessageCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591