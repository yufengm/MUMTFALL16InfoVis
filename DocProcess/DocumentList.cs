using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcess
{
    class DocumentList
    {
        Dictionary<string, ProcessedDocument> fileList = new Dictionary<string, ProcessedDocument>();
        Dictionary<string, string> entityList = new Dictionary<string, string>();
        internal void Load(String dir)
        {
            List<string[]> items = new List<string[]>();
            var lines = File.ReadLines(dir);
            int count = 0;
            foreach (string line in lines)
            {
                string[] fields = CSVParser.Parse(line);
                if (items.Count()>0&&fields[0].Equals(items[0][0]))
                {
                    items.Add(fields);
                }
                else
                {
                    if (items.Count() > 2&&items.Count()<=5)
                    {
                        ProcessedDocument doc = new ProcessedDocument(Guid.NewGuid().ToString());
                        doc.LoadDoc(items.ToArray());
                        fileList.Add(doc.DocID, doc);
                        Console.WriteLine(count++);
                    }
                    items.Clear();
                    items.Add(CSVParser.Parse(line));
                }
            }
        }
        internal ProcessedDocument[] GetList() {
            return fileList.Values.ToArray();
        }
    }
}
