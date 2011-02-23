﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galant.DataEntity;

namespace GLTService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    
    public class Service1 : IServiceAPI
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public BaseData DoRequest(BaseData composite, Entity staff, string OperationType)
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
                return staff;
            }
        }

    }
}