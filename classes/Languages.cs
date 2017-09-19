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

    public class Languages
    {
        private IList<Language> _languages;

        private const int COL_LANGUAGE_GROUP_KEY = 0;
        private const int COL_LANGUAGE_KEY = 1;
        private const int COL_LANGUAGE_TEXT = 2;

        public Languages()
        {
            LoadLanguages();
        }

        public Languages(ModelsToLits modelToLits)
        {
            LoadLanguages();
            _languages = GetAvailableLanguages(modelToLits);
        }

        private void LoadLanguages()
        {
            _languages = new List<Language>();

            string sql = "SELECT LanguageGroupKey, LanguageKey, LanguageText FROM Language ORDER BY SortOrder";

            using (OdbcConnection conn = new OdbcConnection(AccessHelper.AccessConnectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                {

                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Language language = new Language();
                            language.LanguageGroupKey = AccessHelper.GetDBString(reader, COL_LANGUAGE_GROUP_KEY);
                            language.LanguageKey = AccessHelper.GetDBString(reader, COL_LANGUAGE_KEY);
                            language.LanguageText = AccessHelper.GetDBString(reader, COL_LANGUAGE_TEXT);
                            _languages.Add(language);
                        }
                    }
                }
            }
        }

        public IList<Language> GetAvailableLanguages(ModelsToLits modelsToLits)
        {
            IList<String> validLanguages = modelsToLits.GetList().GroupBy(item => item.Lang).Select(item => item.Key).ToList();
            IList<Language> availableLanguages = _languages.GroupBy(item => item.LanguageKey).Where(item => validLanguages.Contains(item.Key)).Select(item => item.First()).ToList();

            //IList<Language> availableLanguages = new List<Language>();

            //foreach (Language language in _languages)
            //{
            //    if (availableLanguages.Any(item => item.LanguageKey == language.LanguageKey)) continue;
            //    if (modelsToLits.GetList().Any(item => item.Lang == language.LanguageKey))
            //    {
            //        availableLanguages.Add(language);
            //    }
            //}

            ////Language[] sortedLanguages = new Language[availableLanguages.Count];

            ////int colNum = 0;
            ////int rowNum = 0;
            ////int colCount = 4;

            ////foreach (Language language in availableLanguages)
            ////{
            ////    int insertIndex = (rowNum * colCount) + colNum;

            ////    if (insertIndex > availableLanguages.Count)
            ////    {
            ////        colNum++;
            ////        rowNum = 0;
            ////        insertIndex = (rowNum * colCount) + colNum;
            ////    }

            ////    sortedLanguages[insertIndex] = language;
            ////    rowNum++;
            ////}

            ////return sortedLanguages.ToList<Language>();

            return availableLanguages;
        }

        public IList<Language> GetList()
        {
            return _languages;
        }

        public void WriteXMLFile(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(_languages.GetType());

            using (TextWriter tw = new StreamWriter(fileName, true, Encoding.Unicode))
            {
                using (XmlTextWriter writer = new XmlTextWriter(tw))
                {
                    writer.Formatting = Formatting.None;
                    writer.QuoteChar = '\'';

                    tw.Write("var _languageXML=\"");
                    ser.Serialize(writer, _languages);
                    tw.WriteLine("\";");
                }
            }
        }

        public void WriteXMLFile2(string fileName)
        {
            using (TextWriter tw = new StreamWriter(fileName))
            {
                string xml = GetXMLString();

                tw.Write("var _languageXML=\"");
                tw.Write(xml.Replace("encoding='utf-16'", "encoding='utf-8'"));
                tw.WriteLine("\";");
                tw.Close();
            }
        }

        public string GetXMLString()
        {
            XmlSerializer ser = new XmlSerializer(_languages.GetType());
            StringBuilder sb = new StringBuilder();

            using (XmlTextWriter writer = new XmlTextWriter(new StringWriter(sb)))
            {
                writer.Formatting = Formatting.None;
                writer.QuoteChar = '\'';
                ser.Serialize(writer, _languages);
                return sb.ToString();
            }
        }
    }
}