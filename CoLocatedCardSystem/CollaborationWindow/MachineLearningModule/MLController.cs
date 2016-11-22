using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.MachineLearningModule
{
    class MLController
    {
        CentralControllers controllers;
        string[][] defaultTopicList = new string[][] {
            new string[] { "one" , "get" , "stay" ,"day", "night","desk" , "go", "front","us","park" },
            new string[] { "place" , "bed" , "stay" , "clean" , "area" , "one" , "great" , "breakfast" , "like" , "nice" },
            new string[] { "breakfast" , "stay" , "free" , "clean" , "like" , "park" , "good" , "well" , "disney" , "day" },
            new string[] { "stay" ,"nice","clean" ,"area","night" ,"place","pool" ,"day" ,"one" ,"get"},
            new string[] { "look" ,"stay" ,"time" , "see", "like" , "around" , "bed" , "us", "clean", "one" }
        };


        Token[][] defaultTopicTokenList;

        internal Token[][] DefaultTopicTokenList
        {
            get
            {
                return defaultTopicTokenList;
            }

            set
            {
                defaultTopicTokenList = value;
            }
        }

        public CentralControllers Controllers
        {
            get
            {
                return controllers;
            }

            set
            {
                controllers = value;
            }
        }

        public MLController(CentralControllers ctrlers) {
            this.controllers = ctrlers;
        }
        public void Init() {
            defaultTopicTokenList = new Token[defaultTopicList.Length][];
            for (int i = 0; i < defaultTopicList.Length; i++)
            {
                defaultTopicTokenList[i] = new Token[defaultTopicList[i].Length];
                for (int j = 0; j < defaultTopicList[i].Length; j++)
                {
                    defaultTopicTokenList[i][j] = new Token();
                    defaultTopicTokenList[i][j].OriginalWord = defaultTopicList[i][j];
                    defaultTopicTokenList[i][j].Process();
                }
            }
        }

        internal Token[] GetTopicToken(Document[] documents) {
            Token[] result = null;
            String doctokens = "";
            foreach (Document doc in documents) {
                for (int index = 0; index < doc.ProcessedDocument.Length; index++) {
                    Token[] tokens = doc.ProcessedDocument[index].List;
                    String rate = doc.DocumentAttributes.Rating[index];
                    String id = doc.DocID;
                    String jpgs = doc.DocumentAttributes.Jpg[index];
                    foreach( Token token in tokens)
                    {
                        if( token.WordType != WordType.STOPWORD && token.WordType != WordType.PUNCTUATION)
                        {
                            //Debug.WriteLine(token.StemmedWord);
                            doctokens += token.StemmedWord + " ";
                        }
                    }
                    doctokens += "|||||";
                    Debug.WriteLine(doctokens);
                }
            }

            string arg = string.Format(@"E:\Courses\Information Visualization\Project\PythonScript\LDA.py"); // Path to the Python code
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"F:\Anaconda2\python.exe", arg );
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true; // Hide the command line window
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardError = false;
            Process processChild = Process.Start(p.StartInfo);

            Debug.WriteLine("");
            return defaultTopicTokenList[0];
        }
    }
}
