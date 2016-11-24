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

        ConcurrentDictionary<string, Topic> list;
        //since the default topic idnex are numbers.
        Dictionary<int, string> defaultMatch = new Dictionary<int, string>();
        Token[][] defaultTopicTokenList;

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

        internal ConcurrentDictionary<string, Topic> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        public MLController(CentralControllers ctrlers)
        {
            this.controllers = ctrlers;
        }
        public void Init()
        {
            list = new ConcurrentDictionary<string, Topic>();
            for (int i = 0; i < defaultTopicList.Length; i++)
            {
                Topic tp = new Topic();
                tp.Id = Guid.NewGuid().ToString();//gen topic id
                defaultMatch.Add(i, tp.Id);
                for (int j = 0; j < defaultTopicList[i].Length; j++)
                {
                    Token tk = new Token();
                    tk.OriginalWord = defaultTopicList[i][j];
                    tk.Process();
                    tp.AddToken(tk);
                }
                list.TryAdd(tp.Id, tp);
            }
        }
        internal string GetDefaultTopicIDByIndex(int index)
        {
            return defaultMatch[index];
        }
        internal Topic GetTopicById(string topicID)
        {
            return list[topicID];
        }
        internal Token[] GetTopicToken(Document[] documents)
        {
            Token[] result = null;
            String doctokens = "";
            foreach (Document doc in documents)
            {
                for (int index = 0; index < doc.ProcessedDocument.Length; index++)
                {
                    Token[] tokens = doc.ProcessedDocument[index].List;
                    foreach (Token token in tokens)
                    {
                        if (token.WordType == WordType.REGULAR)
                        {
                            doctokens += token.StemmedWord + " ";
                        }
                    }
                    doctokens += "|||";
                    //Debug.WriteLine(doctokens);
                }
            }
            LDACommandLineOptions option = new LDACommandLineOptions();
            option.beta = 0.1;
            option.K = 5;
            option.niters = 10;
            option.savestep = 100;
            option.twords = 20;
            option.data = doctokens;
            option.est = true;
            option.modelName = "model-final";

            result = new Token[option.K * option.twords];

            Estimator estimator = new Estimator();
            estimator.init(option);
            List<Dictionary<string, double>> topicWordProbpairs = estimator.estimate();
            int i = 0;
            foreach (Dictionary<string, double> dic in topicWordProbpairs)
            {
                foreach (String key in dic.Keys)
                {
                    Token temp = new Token();
                    temp.OriginalWord = key;
                    temp.Process();
                    Debug.WriteLine(key);
                    result[i] = temp;
                    i++;
                }
            }
            return result;
        }

    }
}
