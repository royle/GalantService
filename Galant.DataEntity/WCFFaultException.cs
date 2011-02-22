using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    class WCFFaultException : ApplicationException
    {
        public WCFFaultException(int TheCode, string TheString)
            : base("Server returned a fault exception: [" + TheCode.ToString() +
                    "] " + TheString)
        {
            m_faultCode = TheCode;
            m_faultString = TheString;
        }

        int m_faultCode;
        string m_faultString;

        public int FaultCode
        {
            get { return m_faultCode; }
        }

        public string FaultString
        {
            get { return m_faultString; }
        }
    }
}
