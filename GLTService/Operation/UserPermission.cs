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

namespace GLTService.Operation
{
    public class UserPermission
    {
        public bool CanLogin(MySqlConnection conn, string userName, string passWord)
        {
            MySqlCommand comm = new MySqlCommand("SELECT COUNT(1) FROM entities WHERE alias = ?name AND password = ?pwd", conn);
            MySqlParameter parUser = new MySqlParameter("?name", userName);
            MySqlParameter parPwd = new MySqlParameter("?pwd", passWord);
            comm.Parameters.Add(parUser);
            comm.Parameters.Add(parPwd);
            MySqlDataAdapter da = new MySqlDataAdapter(comm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
            {
                return true;
            }
            return false;
        }

       
    }
}
