using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Odbc;
using System.Configuration;

namespace eBookGenerator
{
    class CDNames
    {
        private IList<CDName> _cdNames;

        private const int COL_LITCD = 0;
        private const int COL_OUTPUT_FOLDER = 1;
        private const int COL_IMAGE_NAME = 2;

        public CDNames()
        {
            _cdNames = new List<CDName>();

            string sql = "SELECT * FROM tblCDName ORDER BY LitCD";

            using (OdbcConnection conn = new OdbcConnection(AccessHelper.AccessConnectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                {

                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CDName cdName = new CDName();
                            cdName.LitCD = AccessHelper.GetDBString(reader, COL_LITCD);
                            cdName.OutputFolder = AccessHelper.GetDBString(reader, COL_OUTPUT_FOLDER);
                            cdName.ImageName = AccessHelper.GetDBString(reader, COL_IMAGE_NAME);
                            _cdNames.Add(cdName);
                        }
                    }
                }
            }

        }

        public IList<CDName> GetList()
        {
            return _cdNames;
        }

        public CDName GetByLitCD(string litCD)
        {
            return _cdNames.Where(item => item.LitCD == litCD).SingleOrDefault();
        }
    }
}
