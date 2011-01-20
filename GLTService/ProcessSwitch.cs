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

namespace GLTService
{
    public class ProcessSwitch
    {
        public static DetailBase ProcessRequest(DetailBase DetailObj, string OperationType)
        {
            MySqlconnector connector = new MySqlconnector();

            switch (OperationType)
            {
                case "Login":
                    UserPermission permission = new UserPermission();
                    if (permission.CanLogin(connector.conn, DetailObj.UserName, DetailObj.PassWord))
                    {
                        return permission.GetInitUserLogin(DetailObj);
                    }
                    else
                    {
                        DetailObj.ErrorCode = "1001";
                        return DetailObj;
                    }
                    break;
                default:
                    break;
            }
            DetailObj.ErrorCode = "9999";
            return DetailObj;
        }
    }
}
