using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace eBookGenerator
{
    public class Config
    {
        public string ImageName { get; set; }
        public string LinkText { get; set; }

        public void WriteXMLFile(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(this.GetType());

            using (TextWriter tw = new StreamWriter(fileName, true, Encoding.Unicode))
            {
                using (XmlTextWriter writer = new XmlTextWriter(tw))
                {
                    writer.Formatting = Formatting.None;
                    writer.QuoteChar = '\'';

                    tw.Write("var _configXML=\"");
                    ser.Serialize(writer, this);
                    tw.WriteLine("\";");
                }
            }
        }

        public void WriteXMLFile2(string fileName)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.GetType());
            TextWriter tw = new StreamWriter(fileName);
            serializer.Serialize(tw, this);
            tw.Close();
        }
    }
}
