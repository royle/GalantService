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
using GLTService.Operation.BaseEntity;

namespace GLTService
{
    public class ProcessSwitch
    {
        public static Galant.DataEntity.BaseData ProcessRequest(Galant.DataEntity.BaseData DetailObj, Galant.DataEntity.Entity staff, string OperationType)
        {
            Operation.BaseEntity.DataOperator dataOper = new Operation.BaseEntity.DataOperator();
            dataOper.CreateConnectionAndTransaction();
            Entity entity =new Entity();
            Galant.DataEntity.Entity AuthorizedStaff = entity.Authorize(dataOper, staff.Alias, staff.Password, false);
            switch (OperationType)
            {
                case "Login":
                    GLTService.Operation.Login log = new Operation.Login();
                    return log.InitAppCach(dataOper, AuthorizedStaff);
                default:
                    break;
            }
            //DetailObj.Error = "9999";
            return DetailObj;
        }
    }
}
