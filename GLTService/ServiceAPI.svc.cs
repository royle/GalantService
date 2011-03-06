using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galant.DataEntity;

namespace GLTService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service1 : IServiceAPI
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public BaseData DoRequest(Galant.DataEntity.BaseData composite, Galant.DataEntity.Entity staff, string OperationType)
        {
            try
            {
                return ProcessSwitch.ProcessRequest(composite, staff, OperationType);
            }
            catch (Galant.DataEntity.WCFFaultException exWCF)
            {
                staff.WCFFaultCode = exWCF.FaultCode;
                staff.WCFFaultString = exWCF.FaultString;
                staff.WCFErrorString = exWCF.ErrorString;
                return staff;
            }
            catch (Exception ex)
            {
                composite.WCFErrorString = ex.Message;
                composite.WCFFaultString = "未处理异常";
                return composite;
            }
        }

    }
}