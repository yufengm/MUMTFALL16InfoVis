﻿using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
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

        public MLController(CentralControllers ctrlers) {
            this.controllers = ctrlers;
        }
        public void Init()
        {
            list = new ConcurrentDictionary<string, Topic>();
            for (int i = 0; i < defaultTopicList.Length; i++)
            {
                Topic tp = new Topic();
                tp.Id = Guid.NewGuid().ToString();
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
        internal Token[] GetTopicToken(Document[] documents) {
            Token[] result = null;
            foreach (Document doc in documents) {
                for (int index = 0; index < doc.ProcessedDocument.Length; index++) {
                    Token[] tokens = doc.ProcessedDocument[index].List;
                    String rate = doc.DocumentAttributes.Rating[index];
                    String id = doc.DocumentAttributes.Id;
                    String jpgs = doc.DocumentAttributes.Jpg[index];
                }
            }
            return defaultTopicTokenList[0];
        }
    }
}
