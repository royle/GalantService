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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace GLTService.DBConnector
{
    public class MySqlconnector
    {
        private static string mySqlConnectString = "Server=127.0.0.1;Port=3306;Database=GLT;Uid=root;Pwd=;";

        private static string MySqlConnectString
        {
            get { return MySqlconnector.mySqlConnectString; }
        }

        public MySql.Data.MySqlClient.MySqlConnection conn = new MySqlConnection(MySqlConnectString);

        private MySqlTransaction trans;

        public MySqlconnector()
        {
            conn.Open();
            trans = conn.BeginTransaction();
        }

        public MySqlConnection GetNewConnection()
        {
            conn = new MySqlConnection();
            conn.Open();
            trans = conn.BeginTransaction();
            return conn;
        }

        public void Commit()
        {
            trans.Commit();
        }

        public void RollBack()
        {
            trans.Rollback();
        }
    }
}
