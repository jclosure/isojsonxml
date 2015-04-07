using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ServiceStack.Text.Jsv;


    public static class Extensions
    {
        public static string Dump<T>(this T instance)
        {
            return instance.Dump();
        }


        public static string XmlToJson(this string xml)
        { 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }

        public static string JsonToXml(this string jsonText)
        {
            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonText);
            return doc.OuterXml;
        }

        public static List<T> ToList<T>(this string jsonText) where T : class
        {
            var list = new List<T>();



            return list;
        }


        public static string CsvToJson(this string value)
        {
            // Get lines.
            if (value == null) return null;
            string[] lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) throw new InvalidDataException("Must have header line.");

            // Get headers.
            string[] headers = lines.First().Split(',');

            // Build JSON array.
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields.Length != headers.Length) throw new InvalidDataException("Field count must match header count.");
                var jsonElements = headers.Zip(fields, (header, field) => string.Format("{0}: {1}", header, field)).ToArray();
                string jsonObject = "{" + string.Format("{0}", string.Join(",", jsonElements)) + "}";
                if (i < lines.Length - 1)
                    jsonObject += ",";
                sb.AppendLine(jsonObject);
            }
            sb.AppendLine("]");
            return sb.ToString();
        }


        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        public static string ToCSV(this DataTable table, string delimator)
        {
            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
            }
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\n" : delimator);
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
            //return result.ToString();
        }
    }

