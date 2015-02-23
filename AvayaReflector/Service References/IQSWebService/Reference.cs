﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvayaReflector.IQSWebService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="KeyValue", Namespace="http://iq-services.com/")]
    [System.SerializableAttribute()]
    public partial class KeyValue : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="WsResult", Namespace="http://iq-services.com/")]
    [System.SerializableAttribute()]
    public partial class WsResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private bool SuccessField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ResultField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://iq-services.com/", ConfigurationName="IQSWebService.VcReflectorWsSoap")]
    public interface VcReflectorWsSoap {
        
        // CODEGEN: Generating message contract since element name myList from namespace http://iq-services.com/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://iq-services.com/AcceptKeyValueList", ReplyAction="*")]
        AvayaReflector.IQSWebService.AcceptKeyValueListResponse AcceptKeyValueList(AvayaReflector.IQSWebService.AcceptKeyValueListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iq-services.com/AcceptKeyValueList", ReplyAction="*")]
        System.Threading.Tasks.Task<AvayaReflector.IQSWebService.AcceptKeyValueListResponse> AcceptKeyValueListAsync(AvayaReflector.IQSWebService.AcceptKeyValueListRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AcceptKeyValueListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AcceptKeyValueList", Namespace="http://iq-services.com/", Order=0)]
        public AvayaReflector.IQSWebService.AcceptKeyValueListRequestBody Body;
        
        public AcceptKeyValueListRequest() {
        }
        
        public AcceptKeyValueListRequest(AvayaReflector.IQSWebService.AcceptKeyValueListRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://iq-services.com/")]
    public partial class AcceptKeyValueListRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public AvayaReflector.IQSWebService.KeyValue[] myList;
        
        public AcceptKeyValueListRequestBody() {
        }
        
        public AcceptKeyValueListRequestBody(AvayaReflector.IQSWebService.KeyValue[] myList) {
            this.myList = myList;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AcceptKeyValueListResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AcceptKeyValueListResponse", Namespace="http://iq-services.com/", Order=0)]
        public AvayaReflector.IQSWebService.AcceptKeyValueListResponseBody Body;
        
        public AcceptKeyValueListResponse() {
        }
        
        public AcceptKeyValueListResponse(AvayaReflector.IQSWebService.AcceptKeyValueListResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://iq-services.com/")]
    public partial class AcceptKeyValueListResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public AvayaReflector.IQSWebService.WsResult AcceptKeyValueListResult;
        
        public AcceptKeyValueListResponseBody() {
        }
        
        public AcceptKeyValueListResponseBody(AvayaReflector.IQSWebService.WsResult AcceptKeyValueListResult) {
            this.AcceptKeyValueListResult = AcceptKeyValueListResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface VcReflectorWsSoapChannel : AvayaReflector.IQSWebService.VcReflectorWsSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VcReflectorWsSoapClient : System.ServiceModel.ClientBase<AvayaReflector.IQSWebService.VcReflectorWsSoap>, AvayaReflector.IQSWebService.VcReflectorWsSoap {
        
        public VcReflectorWsSoapClient() {
        }
        
        public VcReflectorWsSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VcReflectorWsSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VcReflectorWsSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VcReflectorWsSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        AvayaReflector.IQSWebService.AcceptKeyValueListResponse AvayaReflector.IQSWebService.VcReflectorWsSoap.AcceptKeyValueList(AvayaReflector.IQSWebService.AcceptKeyValueListRequest request) {
            return base.Channel.AcceptKeyValueList(request);
        }
        
        public AvayaReflector.IQSWebService.WsResult AcceptKeyValueList(AvayaReflector.IQSWebService.KeyValue[] myList) {
            AvayaReflector.IQSWebService.AcceptKeyValueListRequest inValue = new AvayaReflector.IQSWebService.AcceptKeyValueListRequest();
            inValue.Body = new AvayaReflector.IQSWebService.AcceptKeyValueListRequestBody();
            inValue.Body.myList = myList;
            AvayaReflector.IQSWebService.AcceptKeyValueListResponse retVal = ((AvayaReflector.IQSWebService.VcReflectorWsSoap)(this)).AcceptKeyValueList(inValue);
            return retVal.Body.AcceptKeyValueListResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<AvayaReflector.IQSWebService.AcceptKeyValueListResponse> AvayaReflector.IQSWebService.VcReflectorWsSoap.AcceptKeyValueListAsync(AvayaReflector.IQSWebService.AcceptKeyValueListRequest request) {
            return base.Channel.AcceptKeyValueListAsync(request);
        }
        
        public System.Threading.Tasks.Task<AvayaReflector.IQSWebService.AcceptKeyValueListResponse> AcceptKeyValueListAsync(AvayaReflector.IQSWebService.KeyValue[] myList) {
            AvayaReflector.IQSWebService.AcceptKeyValueListRequest inValue = new AvayaReflector.IQSWebService.AcceptKeyValueListRequest();
            inValue.Body = new AvayaReflector.IQSWebService.AcceptKeyValueListRequestBody();
            inValue.Body.myList = myList;
            return ((AvayaReflector.IQSWebService.VcReflectorWsSoap)(this)).AcceptKeyValueListAsync(inValue);
        }
    }
}
