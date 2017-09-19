using System.Text;
using System.Configuration;
using System.Data.Odbc;

namespace eBookGenerator
{

    public class AccessHelper
    {
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\VSTFS\ALS_eBookGenerator\Trunk\Database\Ebook.mdb;Uid=Admin;Pwd=;";
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=Database\Ebook.mdb;Uid=Admin;Pwd=;";
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=\\Infaio01\Temp\EPoillion\InstallTest\Database\Ebook.mdb;Uid=Admin;Pwd=;";
        //private String _accessConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\somepath\mydb.mdb;UserId=admin;Password=;";

        public static string AccessConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }

        public static string FixNull(object o)
        {
            if (o == null)
            {
                return string.Empty;
            }
            else
            {
                return o.ToString();
            }
        }

        public static string GetDBString(OdbcDataReader reader, int col)
        {
            if (reader.IsDBNull(col))
            {
                return string.Empty;
            }
            else
            {
                return reader.GetString(col);
            }
        }

        public static bool GetDBBool(OdbcDataReader reader, int col)
        {
            if (reader.IsDBNull(col))
            {
                return false;
            }
            else
            {
                return reader.GetBoolean(col);
            }
        }
    }
}