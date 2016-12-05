using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoLocatedCardSystem.CollaborationWindow.Tool;

namespace CoLocatedCardSystem.CollaborationWindow.MachineLearningModule
{
    class MLController
    {
        CentralControllers controllers;

        internal CentralControllers Controllers
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

        internal MLController(CentralControllers ctrlers)
        {
            this.controllers = ctrlers;
        }
        internal void Init()
        {
        }
        internal async Task<Dictionary<Topic, List<string>>> GetTopicToken(string[] docID, int topicNum)
        {
            Document[] docs = controllers.DocumentController.GetDocument(docID);
            return await GetTopicToken(docs, topicNum);
        }
        internal async Task<Dictionary<Topic, List<string>>> GetTopicToken(Document[] documents, int topicNum)
        {
            return await Task.Run(() =>
             {
                 string doctokens = "";
                 foreach (Document doc in documents)
                 {
                     List<ImageVector> vectors = doc.GetImageVector(controllers.DocumentController);
                     vectors.Select(a => a.List.Select(b => b.Key));
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
                 option.K = topicNum;
                 option.niters = 3;
                 option.savestep = 100;
                 option.twords = 10;
                 option.data = doctokens;
                 option.est = true;
                 option.modelName = "model-final";

                 Estimator estimator = new Estimator();
                 estimator.init(option);
                 List<Dictionary<string, double>> topicWordProbpairs = estimator.estimate();
                 Dictionary<Topic, List<string>> result = new Dictionary<Topic, List<string>>();
                 int[] docIndex = estimator.GetTopicIndex();
                 int topicIndex = 0;
                 foreach (Dictionary<string, double> dic in topicWordProbpairs)
                 {
                     Topic topic = new Topic();
                     topic.Id = Guid.NewGuid().ToString();
                     foreach (KeyValuePair<string, double> pair in dic)
                     {
                         Token tk = controllers.DocumentController.FindToken(pair.Key, documents);
                         if (tk != null)
                         {
                             topic.AddToken(tk);
                         }
                         else
                         {
                             tk = new Token();
                             tk.OriginalWord = pair.Key;
                             tk.Process();
                             topic.AddToken(tk);
                         }
                         topic.SetTopicWeight(User.NONE, tk, pair.Value);
                     }
                     result.Add(topic, new List<string>());
                     for (int i = 0; i < documents.Length; i++)
                     {
                         if (docIndex[i] == topicIndex)
                         {
                             result[topic].Add(documents[i].DocID);
                         }
                     }
                     topicIndex++;
                 }
                 return result;
             });
        }
    }
}
