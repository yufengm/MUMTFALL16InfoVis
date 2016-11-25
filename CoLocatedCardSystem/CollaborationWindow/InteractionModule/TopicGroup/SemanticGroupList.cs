using CoLocatedCardSystem.CollaborationWindow.DocumentModule;
using CoLocatedCardSystem.CollaborationWindow.MachineLearningModule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoLocatedCardSystem.CollaborationWindow.InteractionModule.SemanticGroup;
using static CoLocatedCardSystem.CollaborationWindow.InteractionModule.Topic;

namespace CoLocatedCardSystem.CollaborationWindow.InteractionModule
{
    class SemanticGroupList
    {
        ConcurrentDictionary<string, SemanticGroup> list = new ConcurrentDictionary<string, SemanticGroup>();// key is the topic id
        Dictionary<int, string> defaultMatch = new Dictionary<int, string>();//match the inital topic id with the topic id.

        internal void Init(Document[] docs, MLController mlController)
        {
            for (int i = 0; i < mlController.DefaultTopicList.Length; i++)
            {
                Topic tp = new Topic();
                tp.Id = Guid.NewGuid().ToString();//gen topic id
                defaultMatch.Add(i, tp.Id);
                for (int j = 0; j < mlController.DefaultTopicList[i].Length; j++)
                {
                    Token tk = new Token();
                    tk.OriginalWord = mlController.DefaultTopicList[i][j];
                    tk.Process();
                    SemanticAttribute sa = new SemanticAttribute();
                    sa.isHighlighted = false;
                    tp.AddToken(tk, sa);
                }
                SemanticGroup sg = new SemanticGroup();
                sg.SetTopic(tp);
                list.TryAdd(tp.Id, sg);
            }
            foreach (Document doc in docs)
            {
                string topicID =  defaultMatch[doc.GetDefaultTopicIndex()];
                list[topicID].AddDoc(doc.DocID);
            }
        }

        internal IEnumerable<SemanticGroup> GetSemanticGroup()
        {
            return list.Values;
        }

        internal void Deinit()
        {
            throw new NotImplementedException();
        }
    }
}
