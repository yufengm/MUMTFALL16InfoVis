using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            String dir = @"Orlando_200_topics.csv";
            DocumentList mdir = new DocumentList();
            mdir.Load(dir);
            string fileName = "review.preload";

            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.Write("");
            }
            // Open the file to read from.
            using (StreamWriter sw = File.AppendText(fileName))
            {
                foreach (ProcessedDocument pd in mdir.GetList())
                {
                    sw.WriteLine(pd.ToJson());
                }
            }

            updatePics(mdir);
            Console.ReadLine();
        }

        private static void updatePics(DocumentList list)
        {
            foreach (ProcessedDocument pd in list.FileList.Values) {
                foreach (string s in pd.Jpg)
                {
                    string[] files = s.Split(',');
                    foreach (string name in files)
                    {
                        string sourceFile = @"reviewpics\" + name;
                        string destinationFile = @"selected\" + name;
                        try
                        {
                            // To move a file or folder to a new location:
                            System.IO.File.Copy(sourceFile, destinationFile);

                        } catch (Exception ex) {
                            Console.WriteLine(sourceFile);
                        }
                    }
                }
            }
        }
    }
}
