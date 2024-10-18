using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegerReportServices
{
    public class ConnectionDA
    {
        public static string connectionString;
        EventLog ev = new EventLog();
        public ConnectionDA()
        {
            ev.Source = "Application";
            ev.WriteEntry("Into Conn String");
            connectionString = ConfigurationManager.ConnectionStrings["AtishayRecharge"].ConnectionString;


        }
    }
}
