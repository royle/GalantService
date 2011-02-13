using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galant.DataEntity;
using System.Reflection;

namespace GLTService
{
    // NOTE: If you change the interface name "IServiceAPI" here, you must also update the reference to "IService1" in Web.config.
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypesProvider))] 
    public interface IServiceAPI
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        BaseData DoRequest(BaseData composite, Entity staff, string OperationType);

        // TODO: Add your service operations here
    }

    static class KnownTypesProvider
    {
        static Type[] GetKnownTypes(ICustomAttributeProvider knownTypeAttributeTarget)
        {




            Type contractType = (Type)knownTypeAttributeTarget;


            return contractType.GetGenericArguments();


        }

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
        List<BaseData> data;

        [DataMember]
        public List<BaseData> Data
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
