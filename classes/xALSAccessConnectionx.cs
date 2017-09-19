using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data.Odbc;

namespace eBookGenerator
{
    public class xALSAccessConnectionx
    {
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\VSTFS\ALS_eBookGenerator\Trunk\Database\Ebook.mdb;Uid=Admin;Pwd=;";
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=Database\Ebook.mdb;Uid=Admin;Pwd=;";
        private String _accessConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        //private String _accessConnectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=\\Infaio01\Temp\EPoillion\InstallTest\Database\Ebook.mdb;Uid=Admin;Pwd=;";
        //private String _accessConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\somepath\mydb.mdb;UserId=admin;Password=;";
        protected OdbcConnection _odbcConnection = null;
        protected OdbcCommand _odbcCommand = null;
        protected OdbcDataReader _odbcDataReader = null;
    
        public xALSAccessConnectionx()
        {
            _odbcConnection = new OdbcConnection(_accessConnectionString);
            _odbcConnection.Open();
        }

        protected void GetReader(String command)
        {
            _odbcCommand = new OdbcCommand(command, _odbcConnection);
            _odbcDataReader = _odbcCommand.ExecuteReader();
        }

        protected String FixNull(object o)
        {
            if (o == null)
            {
                return String.Empty;
            }
            else
            {
                return o.ToString();
            }
        }

        protected String GetDBString(int col)
        {
            if (_odbcDataReader.IsDBNull(col))
            {
                return String.Empty;
            }
            else
            {
                return _odbcDataReader.GetString(col);
            }
        }

        protected bool GetDBBool(int col)
        {
            if (_odbcDataReader.IsDBNull(col))
            {
                return false;
            }
            else
            {
                return _odbcDataReader.GetBoolean(col);
            }
        }
    }

}
