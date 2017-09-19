using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Data.Odbc;

namespace eBookGenerator
{

    public class LocalStrings
    {
        private IList<LocalString> _localStrings;

        private const int COL_LANGUAGE_KEY = 0;
        private const int COL_LABEL_KEY = 1;
        private const int COL_LABEL_TEXT = 2;
        private const int COL_EXPORT = 3;

        public LocalStrings()
        {
            _localStrings = new List<LocalString>();

            string sql = "SELECT LanguageKey, LabelKey, LabelText, Export FROM Label ORDER BY LanguageKey, LabelKey";

            using (OdbcConnection conn = new OdbcConnection(AccessHelper.AccessConnectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                {

                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            LocalString localString = new LocalString();
                            localString.LanguageKey = AccessHelper.GetDBString(reader, COL_LANGUAGE_KEY);
                            localString.LabelKey = AccessHelper.GetDBString(reader, COL_LABEL_KEY);
                            localString.LabelText = AccessHelper.GetDBString(reader, COL_LABEL_TEXT);
                            localString.Export = AccessHelper.GetDBBool(reader, COL_EXPORT);
                            _localStrings.Add(localString);
                        }
                    }
                }
            }
        }

        public IList<LocalString> GetList()
        {
            return _localStrings;
        }

        public void WriteXMLFile(string fileName, Language language)
        {
            IList<LocalString> localStrings = _localStrings.Where(item => item.LanguageKey == language.LanguageKey && item.Export).ToList();

            XmlSerializer ser = new XmlSerializer(localStrings.GetType());

            using (TextWriter tw = new StreamWriter(string.Format("{0}_{1}.js", fileName, language.LanguageKey), true, Encoding.Unicode))
            {
                using (XmlTextWriter writer = new XmlTextWriter(tw))
                {
                    writer.Formatting = Formatting.None;
                    writer.QuoteChar = '\'';

                    tw.Write("var _localStringsXML=\"");
                    ser.Serialize(writer, localStrings);
                    tw.WriteLine("\";");
                }
            }
        }

        public void WriteXMLFile2_obsolete(string fileName, Language language)
        {
            IList<LocalString> localStrings = _localStrings.Where(item => item.LanguageKey == language.LanguageKey).ToList();

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(localStrings.GetType());
            TextWriter tw = new StreamWriter(string.Format("{0}_{1}.xml", fileName, language.LanguageKey));
            serializer.Serialize(tw, localStrings);
            tw.Close();
        }
    }
}