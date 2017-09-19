using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace eBookGenerator
{
    class LitToCDs
    {
         private IList<LitToCD> _litToCDs;

        private const int COL_TYPEOFLIT = 0;
        private const int COL_NAMEOFCD = 1;
        
        public LitToCDs()
        {
            _litToCDs = new List<LitToCD>();



            string sql = "SELECT * FROM tblLitToCD";

            using (OdbcConnection conn = new OdbcConnection(AccessHelper.AccessConnectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                {

                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            LitToCD litToCD = new LitToCD();
                            litToCD.TypeOfLit = AccessHelper.GetDBString(reader, COL_TYPEOFLIT);
                            litToCD.NameOfCD = AccessHelper.GetDBString(reader, COL_NAMEOFCD);
                            _litToCDs.Add(litToCD);
                        }
                    }
                }
            }

        }

        public IList<LitToCD> GetList()
        {
            return _litToCDs;
        }
   }
}
