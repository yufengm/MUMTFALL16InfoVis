using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public string[][] DefaultTopicList
        {
            get
            {
                return defaultTopicList;
            }

            set
            {
                defaultTopicList = value;
            }
        }

        public MLController(CentralControllers ctrlers)
        {
            this.controllers = ctrlers;
        }
        public void Init()
        {
        }
        internal async Task<Topic> GetTopicToken(string[] docID)
        {
            Document[] docs = controllers.DocumentController.GetDocument(docID);
            return await GetTopicToken(docs);
        }
        internal async Task<Topic> GetTopicToken(Document[] documents)
        {
            return await Task.Run(() =>
             {
                 Topic topic = new Topic();
                 string doctokens = "";
                 foreach (Document doc in documents)
                 {
                     for (int index = 0; index < doc.ProcessedDocument.Length; index++)
                     {
                         Token[] tokens = doc.ProcessedDocument[index].List;
                         foreach (Token token in tokens)
                         {
                             if (token.WordType == WordType.REGULAR && token.StemmedWord.Length > 1)
                             {
                                 doctokens += token.StemmedWord + " ";
                             }
                         }
                         doctokens += "|||";
                     }
                 }
                 LDACommandLineOptions option = new LDACommandLineOptions();
                 option.beta = 0.1;
                 option.K = 1;
                 option.niters = 10;
                 option.savestep = 100;
                 option.twords = 5;
                 option.data = doctokens;
                 option.est = true;
                 option.modelName = "model-final";


                 string[] resultStr = new string[option.K * option.twords];

                 Estimator estimator = new Estimator();
                 estimator.init(option);
                 List<Dictionary<string, double>> topicWordProbpairs = estimator.estimate();
                 int i = 0;
                 foreach (Dictionary<string, double> dic in topicWordProbpairs)
                 {
                     foreach (String key in dic.Keys)
                     {
                         resultStr[i] = key;
                         i++;
                     }
                 }
                 foreach (string r in resultStr)
                 {
                     Token tk = controllers.DocumentController.FindToken(r, documents);
                     if (tk != null)
                     {
                         topic.AddToken(tk);
                     }
                     else
                     {
                         tk = new Token();
                         tk.OriginalWord = r;
                         tk.Process();
                         topic.AddToken(tk);
                     }
                 }
                 return topic;
             });
        }
    }
}
