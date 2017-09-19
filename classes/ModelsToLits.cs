using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Odbc;
using System.Configuration;

namespace eBookGenerator
{
    public class ModelsToLits
    {
        private IList<ModelsToLit> _modelsToLits;

        private const int COL_MODELNO = 0;
        private const int COL_CUSMOD = 1;
        private const int COL_PRODTYPE = 2;
        private const int COL_PARTNO = 3;
        private const int COL_TYPEOFLIT = 4;
        private const int COL_ECOMMENTS = 5;
        private const int COL_LITCD = 6;
        private const int COL_LANG = 7;

        public ModelsToLits(String cdName)
        {
            IList<Language> languages = new Languages().GetList();

            _modelsToLits = new List<ModelsToLit>();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ModelNo, CUSMOD, ProdType, PartNo, TypeOfLit, Ecomments, LitCD, Lang ");
            sb.Append("FROM tblEBookModelsToLit ");
            sb.Append("WHERE (TypeOfLit IN (SELECT TypeOfLit FROM tblLitToCD WHERE NameOfCD = '" + cdName + "')) AND LitCD = '" + cdName + "' ");
            sb.Append("ORDER BY Lang, ModelNo, TypeOfLit, PartNo");
            string sql = sb.ToString();

            using (OdbcConnection conn = new OdbcConnection(AccessHelper.AccessConnectionString))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(sql, conn))
                {

                    using (OdbcDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string lgk = AccessHelper.GetDBString(reader, COL_LANG);
                            if (string.IsNullOrEmpty(lgk)) lgk = "Blank";

                            foreach (Language language in languages.Where(item => item.LanguageGroupKey == lgk).ToList())
                            {
                                ModelsToLit mtl = new ModelsToLit();
                                mtl.ModelNo = AccessHelper.GetDBString(reader, COL_MODELNO);
                                mtl.ManufacturingNumber = AccessHelper.GetDBString(reader, COL_CUSMOD).Trim();
                                mtl.ProdType = AccessHelper.GetDBString(reader, COL_PRODTYPE);
                                mtl.PartNo = AccessHelper.GetDBString(reader, COL_PARTNO);
                                mtl.TypeOfLit = AccessHelper.GetDBString(reader, COL_TYPEOFLIT);
                                mtl.Ecomments = AccessHelper.GetDBString(reader, COL_ECOMMENTS);
                                mtl.LitCD = AccessHelper.GetDBString(reader, COL_LITCD);
                                mtl.Lang = language.LanguageKey;
                                _modelsToLits.Add(mtl);
                            }
                        }
                    }
                }
            }

        }

        public IList<ModelsToLit> GetList()
        {
            return _modelsToLits;
        }

        public IList<ModelsToLit> GetListByLanguage(Language language)
        {
            IList<ModelsToLit> listByLanguage = new List<ModelsToLit>();
            LocalStrings localStrings = new LocalStrings();


            string englishOnlyMessage = localStrings.GetList().Where(item => item.LanguageKey == language.LanguageKey && item.LabelKey == "english_msg").Single().LabelText;

            var groupList = (from rawData in _modelsToLits
                             group rawData by rawData.ModelNo into groupedData
                             select new
                             {
                                 Key = groupedData.Key,
                                 mtl = groupedData.ToList()
                             });

            foreach (var foo in groupList)
            {
                //bool hasLanguage = foo.mtl.Any(item => item.Lang == language.LanguageKey);
                //if (!hasLanguage) continue;

                var groupList2 = (from rawData2 in foo.mtl
                                 group rawData2 by rawData2.TypeOfLit into groupedData2
                                 select new
                                 {
                                     Key2 = groupedData2.Key,
                                     mtl2 = groupedData2.ToList()
                                 });

                foreach (var foo2 in groupList2)
                {
                    string targetLanguage = language.LanguageKey;
                    bool addEnglishOnlyMessage = false;

                    if (!foo2.mtl2.Any(item => item.Lang == language.LanguageKey))
                    {
                        targetLanguage = "English";
                        addEnglishOnlyMessage = true;
                    }

                    foreach (ModelsToLit modelsToLit in foo2.mtl2)
                    {
                        if (modelsToLit.Lang == targetLanguage)
                        {
                            ModelsToLit mtl = modelsToLit.Clone();
                            if (addEnglishOnlyMessage && !mtl.Ecomments.Contains(englishOnlyMessage))
                            {
                                mtl.Ecomments += string.Format("  ({0})", englishOnlyMessage);
                            }
                            listByLanguage.Add(mtl);
                        }
                    }
                }
            }

            return listByLanguage;
        }

        public IList<ModelsToLit> GetListByLanguageConverted(Language language)
        {
            IList<ModelsToLit> models = GetListByLanguage(language);
            IList<LocalString> localStrings = new LocalStrings().GetList();

            foreach (ModelsToLit mtl in models)
            {
                mtl.ProdType = GetStringByLanguage(mtl.ProdType, language.LanguageKey, localStrings);
                mtl.TypeOfLit = GetStringByLanguage(mtl.TypeOfLit, language.LanguageKey, localStrings);
            }
            return models;
        }

        public IList<ModelsToLit> GetModelList(Language language)
        {
            IList<ModelsToLit> models = GetListByLanguage(language).Distinct(new ModelEqualityComparer()).OrderBy(item => item.ModelNo).ToList();
            IList<LocalString> localStrings = new LocalStrings().GetList();

            foreach (ModelsToLit mtl in models)
            {
                mtl.ProdType = GetStringByLanguage(mtl.ProdType, language.LanguageKey, localStrings);
                mtl.TypeOfLit = GetStringByLanguage(mtl.TypeOfLit, language.LanguageKey, localStrings);
            }
            return models;
        }

        public IList<ModelsToLit> GetManufacturerNumberList(Language language)
        {
            IList<ModelsToLit> models = GetListByLanguage(language).Distinct(new ModelEqualityComparer2()).OrderBy(item => item.ManufacturingNumber).ToList();
            IList<LocalString> localStrings = new LocalStrings().GetList();

            foreach (ModelsToLit mtl in models)
            {
                mtl.ProdType = GetStringByLanguage(mtl.ProdType, language.LanguageKey, localStrings);
                mtl.TypeOfLit = GetStringByLanguage(mtl.TypeOfLit, language.LanguageKey, localStrings);
            }
            return models;
        }

        private String GetStringByLanguage(string labelKey, string languageKey, IList<LocalString> localStrings)
        {
            string val = labelKey;

            LocalString ls = localStrings.Where(item => item.LanguageKey == languageKey && item.LabelKey == labelKey).FirstOrDefault();
            if (ls != null)
            {
                val = ls.LabelText;
            }

            return val;
        }
    }
}
