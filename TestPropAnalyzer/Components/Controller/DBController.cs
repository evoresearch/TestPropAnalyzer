using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPropAnalyzer.Components.Controller
{
   public class DBController
    {
        private SqlConnection sqlConnection;
        public DBController()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["SiteSqlServer"]);
        }

        public bool AddUUTPairMatch;
    }
}
