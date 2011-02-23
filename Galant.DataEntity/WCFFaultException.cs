using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galant.DataEntity
{
    public class WCFFaultException : ApplicationException
    {
        public WCFFaultException(int TheCode, string TheString, string ErrorString)
            : base("Server returned a fault exception: [" + TheCode.ToString() +
                    "] " + TheString)
        {
            m_faultCode = TheCode;
            m_faultString = TheString;
            m_errorString = ErrorString;
        }

        int m_faultCode;
        string m_faultString;
        string m_errorString;

        public int FaultCode
        {
            get { return m_faultCode; }
        }

        public string FaultString
        {
            get { return m_faultString; }
        }

        public string ErrorString
        { get { return m_errorString; } }
    }
}
