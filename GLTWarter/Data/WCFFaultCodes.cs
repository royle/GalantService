using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galant.DataEntity;

namespace GLTWarter.Data
{
    public static class WCFFaultCodes
    {
        public const int InsufficientPermission = 1005; // Client's current login has insufficient permission to perform the task
        public static string GetString(WCFFaultException ex)
        {
            string faultString = string.Empty;
            switch (ex.FaultCode)
            {
                case InsufficientPermission:
                    faultString = Resource.errorRpcApplicationError;
                    break;
            }
            return Resource.errorRpcServerError;
        }
    }
}
