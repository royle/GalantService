using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using GLTService.DBConnector;
using MySql.Data.MySqlClient;
using GLTService.Operation;
using Galant.DataEntity;

namespace GLTService
{
    public class ProcessSwitch
    {
        public static BaseData ProcessRequest(BaseData DetailObj, Entity staff, string OperationType)
        {
            MySqlconnector connector = new MySqlconnector();
            GLTService.Operation.Login log = new Operation.Login();
            log.Operator.CreateConnectionAndTransaction();
            switch (OperationType)
            {
                case "Login":
                  
                    if (log.LoginTest(staff.Alias)!=null)
                    {
                        return DetailObj;
                    }
                    else
                    {
                        DetailObj.Error = "1001";
                        return DetailObj;
                    }
                default:
                    break;
            }
            DetailObj.Error = "9999";
            return DetailObj;
        }
    }
}
