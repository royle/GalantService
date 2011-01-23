using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GLTService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    
    public class Service1 : IServiceAPI
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
       
        public DetailBase DoRequest(DetailBase composite,string OperationType)
        {
            return ProcessSwitch.ProcessRequest(composite,OperationType);
        }

    }
}