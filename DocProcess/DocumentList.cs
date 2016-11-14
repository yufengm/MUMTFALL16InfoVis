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

        internal Dictionary<string, ProcessedDocument> FileList
        {
            get
            {
                return fileList;
            }
        }

        internal void Load(String dir)
        {
            List<string[]> items = new List<string[]>();
            var lines = File.ReadLines(dir);
            int count = 0;
            foreach (string line in lines)
            {
                string[] fields = CSVParser.Parse(line);
                //Add the review about the same hotel to fields
                if (items.Count()>0&&fields[0].Equals(items[0][0]))
                {
                    items.Add(fields);
                }
                else//Process the document
                {
                    if (items.Count() > 2)
                    {
                        ProcessedDocument doc = new ProcessedDocument(Guid.NewGuid().ToString());
                        if (items.Count > 1) {//only use the first.
                            string[] first = items[0];
                            items.Clear();
                            items.Add(first);
                        }
                        doc.LoadDoc(items.ToArray());
                        fileList.Add(doc.DocID, doc);
                        Console.WriteLine(count++);
                    }
                    //Clear the item list and add the new one.
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
