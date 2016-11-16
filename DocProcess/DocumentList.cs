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
                ProcessedDocument doc = new ProcessedDocument(Guid.NewGuid().ToString());
                doc.LoadDoc(new string[][] { fields });
                fileList.Add(doc.DocID, doc);
                Console.WriteLine(count++);
            }
        }
        internal ProcessedDocument[] GetList()
        {
            return fileList.Values.ToArray();
        }
    }
}
