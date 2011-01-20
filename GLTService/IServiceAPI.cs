using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GLTService
{
    // NOTE: If you change the interface name "IServiceAPI" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    public interface IServiceAPI
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        DetailBase DoRequest(DetailBase composite, string OperationType);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class DetailBase
    {
        bool boolValue = true;
        string stringValue = "Hello ";
        string errorCode = "";

        [DataMember]
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        string userName="";
        string passWord = "";
        List<Object> data;

        [DataMember]
        public List<Object> Data
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        
    }
}
